using PDManager.Core.Common.Exceptions;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDManager.Core.Common.Extensions;

namespace PDManager.Core.Aggregators
{

    /// <summary>
    /// UPDRS Score Aggregator
    /// </summary>
    public class GenericAggregator : IAggregator
    {



        #region Private Const declarations
        private const int timeInterval = 30 * 60 * 1000;
        private const int dayInterval = 60 * 60 * 24 * 1000;
        /// <summary>
        /// Time aggregation type value
        /// </summary>
        private const string TimeAggregationType = "time";
        /// <summary>
        /// Day aggregation type value
        /// </summary>
        private const string DayAggregationType = "day";
        /// <summary>
        /// Total aggregation type value
        /// </summary>
        private const string TotalAggregationType = "total";

        private readonly IDataProxy _proxy;
        private readonly IGenericLogger _logger;
        private readonly IAggrDefinitionProvider _aggrDefinitionDictionary;
        private const int MAXTAKE = 1000;

        private AggrConfig _definition;

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
        public GenericAggregator(IDataProxy proxy,IGenericLogger logger, IAggrDefinitionProvider aggrDefinitionDictionary)
        {
            this._proxy = proxy;
            this._aggrDefinitionDictionary = aggrDefinitionDictionary;
            this._logger = logger;

        }




        private AggrConfig LoadAggrDefinition(string name)
        {
            return AggrConfig.LoadFromFile(name);

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
        /// <param name="aggregationType">Overrides the default aggregation type</param>
        /// <param name="filterType">Overrides the default filter type</param>
        /// <returns></returns>

        public async Task<IEnumerable<IObservation>> Run(string patientId, string code, DateTime? lastExecutionTime,string aggregationType=null,string filterType=null)
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

            if (_definition == null)
                _definition = AggrConfig.FromString(_aggrDefinitionDictionary.GetJsonConfigFromCode(code));

            if (_definition == null)
            {
                throw new AggrDefinitionNotFoundException(nameof(_definition));
            }


            if(!string.IsNullOrEmpty(aggregationType))
            {
                _definition.AggregationType = aggregationType;
            }


            if (!string.IsNullOrEmpty(filterType))
            {
                _definition.MetaAggregationType = filterType;
            }

            var lastExecutionTimeStamp = lastExecutionTime.HasValue ? lastExecutionTime.Value.ToUnixTimestamp() * 1000 : 0;

            //Get Data
            List<PDObservation> observations = new List<PDObservation>();
            foreach (var c in _definition.Variables)
            {              
                    try
                    {
                        //Get Observations For patient
                        var ret = await _proxy.Get<PDObservation>( MAXTAKE, 0, GetFilter(patientId, c.Code, lastExecutionTimeStamp, _definition.AggregationType), null);
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
        private IEnumerable<IObservation> PerformAggregation(AggrConfig definition, string patientId, long timestamp, IEnumerable<PDObservation> observations)
        {

            //-----------------
            // 1st Level Aggregation
            //------------
            IEnumerable<IObservation> metaObservations = null;
            //return PerformTotalAggregation(definition, patientId, timestamp, observations);

            if (definition.AggregationType == TimeAggregationType)
            {

                metaObservations= PerformTimeAggregation(definition, patientId, observations);


            }
            else if (definition.AggregationType == DayAggregationType)
            {

                metaObservations= PerformDayAggregation(definition, patientId, observations);

            }
            else
            {
                metaObservations= PerformTotalAggregation(definition, patientId, timestamp, observations);

            }


            if(metaObservations==null)
            {
                //Something wrong happened!!
                throw new Exception();

            }

            //---------------------
            //Threshold meta-observations
            //---------------------            
            Thresholding(definition, metaObservations);


            //---------------------
            //2nd Level aggregation
            //---------------------      
            return MetaAggregation(definition, patientId, metaObservations);



        }



        /// <summary>
        /// Total Aggregation
        /// </summary>
        /// <param name="definition">Aggregation definition</param>
        /// <param name="patientId">Patient Id</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="observations">Observations</param>
        /// <returns></returns>
        private IEnumerable<IObservation> PerformTotalAggregation(AggrConfig definition,string patientId,long timestamp,IEnumerable<PDObservation> observations)
        {
                   
            double v = definition.Beta;
            foreach (var c in definition.Variables)
            {

                var v1 = c.Weight * observations.Where(e=>e.CodeId==c.Code).Select(e => e.Value).DefaultIfEmpty().Average();
                v += v1;

            }
            return new List<IObservation>() { new PDObservation() { Value = v, PatientId = patientId, CodeId = definition.Code, Timestamp = timestamp } };

            

        }


        /// <summary>
        /// Day Aggregation
        /// </summary>
        /// <param name="definition">Aggregation Definition</param>
        /// <param name="patientId">Patient Id</param>
        /// <param name="observations">Observations</param>
        /// <returns></returns>
        private IEnumerable<IObservation> PerformDayAggregation(AggrConfig definition, string patientId, IEnumerable<PDObservation> observations)
        {

            // Get Min and Max Date
            var minT = observations.Select(e => e.Timestamp).DefaultIfEmpty().Min();
            var maxT = observations.Select(e => e.Timestamp).DefaultIfEmpty().Max();

            List<IObservation> metaObservations = new List<IObservation>();
            var startDate = DateTimeExtensions.FromUnixTimestampMilli(minT);
            var endDate = DateTimeExtensions.FromUnixTimestampMilli(maxT);

            var startT = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, DateTimeKind.Utc).ToUnixTimestampMilli();
            var endT = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59, DateTimeKind.Utc).ToUnixTimestampMilli();

            //Estimate mean and descriptive statistics
            for (long i = startT; i < endT; i += dayInterval)
            {
                double v = definition.Beta;
                foreach (var c in definition.Variables)
                {

                    var v1 = c.Weight * observations.Where(e => e.CodeId == c.Code&& e.Timestamp >= i && e.Timestamp < i + dayInterval).Select(e => e.Value).DefaultIfEmpty().Average();
                    v += v1;
                  
                }
                metaObservations.Add(new PDObservation() { CodeId = definition.Code, PatientId = patientId, Timestamp = i, Value = v });
                
            }
            return metaObservations;
          
        }
        private void Thresholding(AggrConfig definition, IEnumerable<IObservation> metaObservations)
        {
            var mean = metaObservations.Select(e => e.Value).DefaultIfEmpty(0).Average();
            var q2 = metaObservations.Select(e => e.Value * e.Value).DefaultIfEmpty(0).Average();
            int n = metaObservations.Count();
            var std = q2 / n - mean * mean;

           
            //-----------------
            //THRESHOLDING
            //-----------------
            //If not thresholding required return mean
            if (definition.Threshold)
            {

                //Set threshold based on threshold type and value
                var threshold = definition.ThresholdValue;

                if (definition.ThresholdType == "std")
                {
                    threshold = mean + definition.ThresholdValue * std;
                }

                //New observation timestamp
                foreach (var obs in metaObservations)
                {


                    obs.Value = (obs.Value > threshold) ? 1.0 : 0.0;
                }
            }
        }


        /// <summary>
        /// 2nd Level Aggregation
        /// </summary>
        /// <param name="definition">Aggregation Definition</param>
        /// <param name="patientId">Patient Id</param>
        /// <param name="metaObservations">Observations</param>
        /// <returns></returns>
            private IEnumerable<IObservation> MetaAggregation(AggrConfig definition, string patientId, IEnumerable<IObservation> metaObservations)
        {
           

            var endT = metaObservations.Select(e => e.Timestamp).DefaultIfEmpty().Max();
            
            if (definition.MetaAggregationType == "sum")
            {

                return new List<IObservation>() { new PDObservation() { CodeId = definition.Code, PatientId = patientId, Timestamp = endT, Value = definition.MetaScale * metaObservations.Select(e => e.Value).DefaultIfEmpty(0).Sum() } };
            }
            else if (definition.MetaAggregationType == "average")
            {
                return new List<IObservation>() { new PDObservation() { CodeId = definition.Code, PatientId = patientId, Timestamp = endT, Value = definition.MetaScale * metaObservations.Select(e => e.Value).DefaultIfEmpty(0).Average() } };

            }
       
            else if (definition.MetaAggregationType == "mfi")
            {
                return new List<IObservation>() { new PDObservation() { CodeId = definition.Code, PatientId = patientId, Timestamp = endT, Value = definition.MetaScale * (metaObservations.Select(e => e.Value).DefaultIfEmpty(0).Max()- metaObservations.Select(e => e.Value).DefaultIfEmpty(0).Average()) } };

            }
            else if (definition.MetaAggregationType == "count")
            {
                return new List<IObservation>() { new PDObservation() { CodeId = definition.Code, PatientId = patientId, Timestamp = endT, Value = definition.MetaScale * metaObservations.Count() } };

            }
            else if (definition.MetaAggregationType == "max")
            {
                return new List<IObservation>() { new PDObservation() { CodeId = definition.Code, PatientId = patientId, Timestamp = endT, Value = definition.MetaScale * metaObservations.Select(e => e.Value).DefaultIfEmpty(0).Max() } };

            }
            else if (definition.MetaAggregationType == "min")
            {
                return new List<IObservation>() { new PDObservation() { CodeId = definition.Code, PatientId = patientId, Timestamp = endT, Value = definition.MetaScale * metaObservations.Select(e => e.Value).DefaultIfEmpty(0).Min() } };

            }
            else if (definition.MetaAggregationType == "std")
            {

                var fmean = metaObservations.Select(e => e.Value).DefaultIfEmpty(0).Average();
                var fq2 = metaObservations.Select(e => e.Value * e.Value).DefaultIfEmpty(0).Average();
                int fn = metaObservations.Count();
                var fstd = fq2 / fn - fmean * fmean;

                return new List<IObservation>() { new PDObservation() { CodeId = definition.Code, PatientId = patientId, Timestamp = endT, Value = definition.MetaScale * fstd } };

            }
            else if (definition.MetaAggregationType == "cv")
            {

                var fmean = metaObservations.Select(e => e.Value).DefaultIfEmpty(0).Average();
                var fq2 = metaObservations.Select(e => e.Value * e.Value).DefaultIfEmpty(0).Average();
                int fn = metaObservations.Count();
                var fstd = fq2 / fn - fmean * fmean;

                return new List<IObservation>() { new PDObservation() { CodeId = definition.Code, PatientId = patientId, Timestamp = endT, Value =definition.MetaScale*100*fstd/fmean } };

            }
            else
            {

                return metaObservations;

            }


        }


        /// <summary>
        /// Time Aggregation
        /// </summary>
        /// <param name="definition">Aggregation Definition</param>
        /// <param name="patientId">Patient Id</param>
        /// <param name="observations">Observations</param>
        /// <returns></returns>
        private IEnumerable<IObservation> PerformTimeAggregation(AggrConfig definition,  string patientId, IEnumerable<PDObservation> observations)
        {

            List<IObservation> metaObservations = new List<IObservation>();        
            var minT = observations.Select(e => e.Timestamp).DefaultIfEmpty().Min();
            var maxT = observations.Select(e => e.Timestamp).DefaultIfEmpty().Max();

         
            var startDate = DateTimeExtensions.FromUnixTimestampMilli(minT);
            var endDate = DateTimeExtensions.FromUnixTimestampMilli(maxT);

            var t0= new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, DateTimeKind.Utc).ToUnixTimestampMilli();
            var tmax = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59, DateTimeKind.Utc).ToUnixTimestampMilli();

            for (long j = 0; j <= dayInterval; j += timeInterval)
            {
                               
                double m = 0;              
                int n0 = 0;
                double v = definition.Beta;
                foreach (var c in definition.Variables)
                {
                    n0 = 0;
                    m = 0;
                    for (long i = t0; i <= tmax; i += dayInterval)
                    {
                        long te = i + j + timeInterval;
                        long ts = i + j;
                        m += observations.Where(e => e.Timestamp >= ts && e.Timestamp < te&&e.CodeId==c.Code).Select(e => e.Value).DefaultIfEmpty(0).Sum();
                        n0 += observations.Where(e => e.Timestamp >= ts && e.Timestamp < te && e.CodeId == c.Code).Count();
                    }
                    if (n0 > 0)
                    {
                        
                        v +=( m/n0) * c.Weight;
                    }
                  
                }

                if (n0 > 0)
                {                 
                
                    metaObservations.Add(new PDObservation() { CodeId = definition.Code, PatientId = patientId, Timestamp = j, Value = v, Weight = n0 });
                }

            }

            return metaObservations;
          
        }


    }
}
