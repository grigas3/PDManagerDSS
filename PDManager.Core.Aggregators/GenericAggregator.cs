using PDManager.Core.Common.Exceptions;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDManager.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace PDManager.Core.Aggregators
{

    /// <summary>
    /// UPDRS Score Aggregator
    /// </summary>
    public class GenericAggregator : IAggregator
    {

        #region Private Const declarations
        private const int timeInterval = 30*60*1000;
        private const int dayInterval = 60 * 60 * 24 * 1000;
        private readonly IDataProxy _proxy;
        private readonly ILogger _logger;
        private readonly IAggrDefinitionProvider _aggrDefinitionDictionary;
        private const int MAXTAKE = 100;

        private AggrConfigDefinition _definition;

        #endregion

        /// <summary>
        /// UPDRS Score Code
        /// </summary>
        public const string UPDRSCODE = "UPDRSAGGR";


        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="proxy">Data proxy</param>
        /// <param name="logger"></param>
        /// <param name="aggrDefinitionDictionary"></param>
        public GenericAggregator(IDataProxy proxy,ILogger<GenericAggregator> logger,IAggrDefinitionProvider aggrDefinitionDictionary)
        {
            this._proxy = proxy;
            this._aggrDefinitionDictionary = aggrDefinitionDictionary;
            this._logger = logger;

        }


       

        private AggrConfigDefinition LoadAggrDefinition(string name)
        {
            return AggrConfigDefinition.LoadFromFile(name);

        }

        /// <summary>
        /// Get Filter
        /// TODO: Get all codes together
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="code"></param>
        /// <param name="prevJobExecutionTimestamp"></param>
        /// <param name="aggrType"></param>
        /// <returns></returns>
        private string GetFilter(string patientId, string code, long prevJobExecutionTimestamp, string aggrType)
        {

            return $"{{patientid:\"{patientId}\",deviceid:\"\",codeid:\"{code}\",datefrom:{prevJobExecutionTimestamp},dateto:0,aggr:\"{aggrType}\"}}";
        }
        /// <summary>
        /// Run Aggregation
        /// This method 
        /// 1) loads the aggregation definition
        /// 2) Fetch all required observations using the DataProxy
        /// 3) Calls the PerformAggregation method to perform the aggregation and returns a new observation
        /// 
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="code">Meta observation Code</param>
        /// <param name="lastExecutionTime"></param>
        /// <returns></returns>

        public async Task<IObservation> Run(string patientId, string code, DateTime? lastExecutionTime)
        {
            if (patientId == null)
            {
                throw new ArgumentNullException(nameof(patientId));
            }

            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            // Get Definition File Name from AggrDefinitionDictionary

            if(_definition==null)
            _definition = AggrConfigDefinition.FromString(_aggrDefinitionDictionary.GetJsonConfigFromCode(code));

            if (_definition == null)
            {
                throw new AggrDefinitionNotFoundException(nameof(_definition));
            }

      

            var lastExecutionTimeStamp = lastExecutionTime.HasValue ? lastExecutionTime.Value.ToUnixTimestamp() * 1000 : 0;


            //Get Data
            List<PDObservation> observations = new List<PDObservation>();
            foreach (var c in _definition.Variables)
            {
                try
                {
                    //Get Observations For patient
                    var ret = await _proxy.Get<PDObservation>(c.Uri, MAXTAKE, 0, GetFilter(patientId, c.Code, lastExecutionTimeStamp, _definition.AggregationType), null);
                    observations.AddRange(ret);

                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error in aggregation");
                }

            }


            var metaObservation = PerformAggregation(_definition, patientId, lastExecutionTimeStamp, observations);
            return metaObservation;



        }



      


        /// <summary>
        /// PerformAggregation Aggregation
        /// </summary>
        /// <param name="definition">Aggregation Definition</param>
        /// <param name="patientId">Patient Id</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="observations">Observations</param>
        /// <returns></returns>
        private PDObservation PerformAggregation(AggrConfigDefinition definition, string patientId, long timestamp, IEnumerable<PDObservation> observations)
        {        

            if(definition.AggregationType== AggrConfigDefinition.TimeAggregationType)
            {

                var value = PerformTimeAggregation(definition, observations);
                return new PDObservation() { Value = value, PatientId = patientId, CodeId = definition.Code, Timestamp = timestamp };

            }
            else if(definition.AggregationType == AggrConfigDefinition.DayAggregationType)
            {

                var value = PerformDayAggregation(definition, observations);
                return new PDObservation() { Value = value, PatientId = patientId, CodeId = definition.Code, Timestamp = timestamp };

            }
            else
            {

                var value = PerformTotalAggregation(definition, observations);
                return new PDObservation() { Value = value, PatientId = patientId, CodeId = definition.Code, Timestamp = timestamp };


            }
        }



        /// <summary>
        /// Total Aggregation
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="observations"></param>
        /// <returns></returns>
        private double PerformTotalAggregation(AggrConfigDefinition definition, IEnumerable<PDObservation> observations)
        {

            double mean = 0;
            double q2 = 0;
            int n = 0;
            double std = 0;
          
                double v = definition.Beta;
                foreach (var c in definition.Variables)
                {

                    var v1 = c.Weight * observations.Select(e => e.Value).DefaultIfEmpty().Average();
                    v += v1;
                    mean += v1;
                    q2 += v1 * v1;
                    n++;
                }

           
            mean /= n;
            std = q2 / n - mean * mean;


            //If not thresholding required return mean
            if (!definition.Threshold)
            {
                return mean;
            }

            //Set threshold based on threshold type and value
            var threshold = definition.ThresholdValue;            
            if (definition.ThresholdType == "std")
            {
                throw new AggrThresholdTypeNotSupported();
            }
          

            return (mean > threshold) ? 1 : 0;



        }


        /// <summary>
        /// Day Aggregation
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="observations"></param>
        /// <returns></returns>
        private double PerformDayAggregation(AggrConfigDefinition definition, IEnumerable<PDObservation> observations)
        {

            double mean = 0;
            double q2 = 0;
            int n = 0;
            double std = 0;

            // Get Min and Max Date
            var minT = observations.Select(e => e.Timestamp).DefaultIfEmpty().Min();
            var maxT = observations.Select(e => e.Timestamp).DefaultIfEmpty().Max();


            var startDate=DateTimeExtensions.FromUnixTimestampMilli(minT);
            var endDate = DateTimeExtensions.FromUnixTimestampMilli(maxT);

            var startT = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, DateTimeKind.Utc).ToUnixTimestampMilli();
            var endT = new DateTime(endDate.Year, endDate.Month, endDate.Day, 24, 59, 59, DateTimeKind.Utc).ToUnixTimestampMilli();

            //Estimate mean and descriptive statistics
            for (long i = startT; i < endT; i += dayInterval)
            {
                double v = definition.Beta;
                foreach (var c in definition.Variables)
                {

                    var v1 = c.Weight * observations.Where(e => e.Timestamp >= i && e.Timestamp < i + dayInterval).Select(e => e.Value).DefaultIfEmpty().Average();
                    v += v1;
                    mean += v1;
                    q2 += v1 * v1;
                    n++;
                }

            }
            mean /= n;
            std = q2 / n - mean * mean;


            //If not thresholding required return mean
            if (!definition.Threshold)
            {
                return mean;
            }

            //Set threshold based on threshold type and value
            var threshold = definition.ThresholdValue;
            int n1 = 0;
            if (definition.ThresholdType == "std")
            {
                threshold = mean + definition.ThresholdValue * std;
            }
            for (long i = startT; i < endT; i += dayInterval)
            {
                double v = definition.Beta;
                foreach (var c in definition.Variables)
                {
                    var v1 = c.Weight * observations.Where(e => e.Timestamp >= i && e.Timestamp < i + dayInterval).Select(e => e.Value).DefaultIfEmpty().Average();
                    n1 += (v1 > threshold) ? 1 : 0;
                }
            }

            return ((double)n1) / n;



        }
              


        /// <summary>
        /// Time Aggregation
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="observations"></param>
        /// <returns></returns>
        private double PerformTimeAggregation(AggrConfigDefinition definition, IEnumerable<PDObservation> observations)
        {

            double mean = 0;
            double q2 = 0;
            int n = 0;
            double std = 0;
            var startT = observations.Select(e => e.Timestamp).DefaultIfEmpty().Min();
            var endT = observations.Select(e => e.Timestamp).DefaultIfEmpty().Max();

            for (long i = startT; i < endT; i += timeInterval)
            {
                double v = definition.Beta;
                foreach (var c in definition.Variables)
                {

                    var v1 = c.Weight * observations.Where(e => e.Timestamp >= i && e.Timestamp < i + timeInterval).Select(e => e.Value).DefaultIfEmpty().Average();
                    v += v1;
                    mean += v1;
                    q2 += v1 * v1;
                    n++;
                }

            }
            mean /= n;
            std = q2 / n - mean * mean;


            //If not thresholding required return mean
            if (!definition.Threshold)
            {
                return mean;
            }

            //Set threshold based on threshold type and value
            var threshold = definition.ThresholdValue;
            int n1 = 0;
            if (definition.ThresholdType == "std")
            {
                threshold = mean + definition.ThresholdValue * std;
            }
            for (long i = startT; i < endT; i += timeInterval)
            {
                double v = definition.Beta;
                foreach (var c in definition.Variables)
                {
                    var v1 = c.Weight * observations.Where(e => e.Timestamp >= i && e.Timestamp < i + timeInterval).Select(e => e.Value).DefaultIfEmpty().Average();
                    n1 += (v1 > threshold) ? 1 : 0;
                }
            }

            return ((double)n1) / n;



        }


    }
}
