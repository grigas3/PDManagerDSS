using Newtonsoft.Json;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Common.Models;
using PDManager.Core.DSS.Dexi;
using PDManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PDManager.Core.DSS
{
    /// <summary>
    /// DSS Runner
    /// </summary>
    public class DSSRunner : IDSSRunner
    {
        private readonly IDataProxy _dataProxy;
        private readonly IAggregator _aggregator;


        private const string ObservationType = "observation";
        private const string MetaObservationType = "metaobservation";
        private const string ClinicalInfoType = "clinical";
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="dataProxy"></param>
        public DSSRunner(IAggregator aggregator,IDataProxy dataProxy)
        {
            this._dataProxy = dataProxy;
            this._aggregator = aggregator;
        }

        #region Private Declaration

        //private Dictionary<string, int> _valueMapping = new Dictionary<string, int>();
        //private Model _dssModel;
        //private DSSConfig _config;

        #endregion Private Declaration

        /// <summary>
        /// C# DateTime to Java
        /// </summary>
        /// <param name="dateTime">C# DateTime</param>
        /// <returns></returns>
        protected static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (long)(TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }

        /// <summary>
        /// Load Model from a file
        /// TODO: We could cache models
        /// </summary>
        /// <param name="modelFileName"></param>
        /// <returns></returns>
        private Model LoadModel(string modelFileName)
        {
            return new Model(modelFileName);
        }

        /// <summary>
        /// Get Clinical Information List
        /// The basic info are the Code and the Value
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClinicalInfo> GetClinicalInfoList(string clinicalInfo)
        {
            if (string.IsNullOrEmpty(clinicalInfo))
            {
                //If CLinical info string is null or emtpy return empty lsit

                return new List<ClinicalInfo>();
            }
            else
            {
                try
                {
                    return JsonConvert.DeserializeObject<IEnumerable<ClinicalInfo>>(clinicalInfo);
                }
                catch
                {
                    return new List<ClinicalInfo>();
                }
            }
        }


        /// <summary>
        /// Run using specific values
        /// </summary>        
        /// <param name="configJson">Dss config in json format</param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IEnumerable<DSSValue> Run(string configJson, Dictionary<string,string> values)       
        { 
            Dictionary<string, int> valueMapping = new Dictionary<string, int>();
            var config = JsonConvert.DeserializeObject<DSSConfig>(configJson);

            //TODO: Handle Exceptions
            var model = LoadModel(config.DexiFile);
            foreach (var parameterInfo in config.Input)
            {
                var key = parameterInfo.Name;

                if (values.ContainsKey(key) && !string.IsNullOrEmpty(values[key]))
                {             
                  
                        if (parameterInfo.Numeric)
                        {

                            double v = double.Parse(values[key]);
                            model.SetInputValue(parameterInfo.Name, parameterInfo.GetNumericMapping(v));
                        }
                        else
                        {
                            int? v = parameterInfo.GetCategoryMapping(values[key]);
                            if (v.HasValue)
                                model.SetInputValue(parameterInfo.Name, v.Value);
                            else
                                model.SetInputValue(parameterInfo.Name, parameterInfo.DefaultValue);

                        }

                    

                }
                else
                {

                    model.SetInputValue(parameterInfo.Name, parameterInfo.DefaultValue);
                }

            }

            model.Evaluate(Model.Evaluation.SET, true);
            var ret = model.Aggregate.Select(e => new DSSValue() { Name = e.Name, Value = e.ValuesString });
            return ret;

        }
        /// <summary>
        /// Get Input Values for DSS model of specific patient
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="configJson">DSS Mapping file</param>
        /// <returns></returns>
        public async Task<IEnumerable<DSSValue>> GetInputValues(string patientId, string configJson)
        {

            List<DSSValue> values = new List<DSSValue>();
            Dictionary<string, int> valueMapping = new Dictionary<string, int>();
            var config = JsonConvert.DeserializeObject<DSSConfig>(configJson);



            //  var accessToken = proxy.GetAccessToken("admin","newpass");//ConfigurationManager.AppSettings["B3NetProxyUserName"], ConfigurationManager.AppSettings["B3NetProxyPassword"]);

            //Codes from observations
            List<string> observationCodes = new List<string>();

            //Codes from Meta o
            List<string> metaObservationCodes = new List<string>();

            //Codes from Patient Clinical Info
            List<string> clinicalInfoCodes = new List<string>();

            foreach (var c in config.Input)
            {
              

                if(string.IsNullOrEmpty(c.Source))
                {
                    //TODO: Add some error to response
                    continue;
                }


                if (c.Source.ToLower() == ObservationType)
                {
                    observationCodes.Add(c.Code);
                }
                else if (c.Source.ToLower() == MetaObservationType)
                {
                    metaObservationCodes.Add(c.Code);

                }
                else if (c.Source.ToLower() == ClinicalInfoType)
                {
                    clinicalInfoCodes.Add(c.Code);
                }
                else
                {

                    throw new NotSupportedException($"Source type {c.Source} not supported from PDManager DSS");

                }
            }

            #region Observation Codes

        
            //TODO: Join observations codes in a single request in case 

            foreach (var code in observationCodes)
            {
                try
                {
                    var observations = await _dataProxy.Get<PDObservation>(10, 0, String.Format("{{patientid:\"{0}\",deviceid:\"\",datefrom:{2},dateto:{3},codeid:\"{1}\",aggr:\"total\"}}", patientId, code, DateTimeToUnixTimestamp(DateTime.Now.AddDays(-config.AggregationPeriodDays)), DateTimeToUnixTimestamp(DateTime.Now)), null);

                    foreach (var c in observations)
                    {
                        
                        foreach(var observation in config.Input.Where(e => e.Code == c.CodeId))
                        {
                            values.Add(new DSSValue() { Name = observation.Name, Code = observation.Code, Value = c.Value.ToString() });
                            
                        }
                     
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            #endregion Meta Observation Codes

            #region Meta Observation Codes


            foreach (var code in metaObservationCodes)
            {
                try
                {
                    //Get Aggregated observation
                    var aggrObservation = await _aggregator.Run(patientId, code, DateTime.Now.AddDays(-config.AggregationPeriodDays));

                    //Find Corresponding observation in config
                    

                    foreach(var metaObservation in config.Input.Where(e => e.Code == code))
                    {
                        //Meta Observations are only numeric
                        values.Add(new DSSValue() { Name = metaObservation.Name, Code = metaObservation.Code,Value = aggrObservation.Select(e => e.Value).DefaultIfEmpty(0).Average().ToString() });
                        
                        
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            #endregion Observation Codes


            #region Clinical Info

            try
            {
                var patient = await _dataProxy.Get<PDPatient>(patientId);

                var clinicalInfoList = GetClinicalInfoList(patient.ClinicalInfo);
                foreach (var c in clinicalInfoList)
                {
                    var clinicalInfo = config.Input.FirstOrDefault(e => e.Code.ToLower() == c.Code.ToLower());

                    if (clinicalInfo != null)
                    {
                       
                            values.Add(new DSSValue() { Name = clinicalInfo.Name,Code = clinicalInfo.Code, Value = c.Value });
                     
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion Clinical Info

            return values;
        }


        /// <summary>
        /// Run DSS
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="dssMappingFile">DSS Mapping file</param>
        /// <returns></returns>
        public async Task<IEnumerable<DSSValue>> Run(string patientId, string dssMappingFile)
        {
            Dictionary<string, int> valueMapping = new Dictionary<string, int>();
            var config = DSSConfig.LoadFromFile(dssMappingFile);

            //TODO: Handle Exceptions
            var model = LoadModel(config.DexiFile);

            

            //  var accessToken = proxy.GetAccessToken("admin","newpass");//ConfigurationManager.AppSettings["B3NetProxyUserName"], ConfigurationManager.AppSettings["B3NetProxyPassword"]);

            //Codes from observations
            List<string> observationCodes = new List<string>();

            //Codes from Meta o
            List<string> metaObservationCodes = new List<string>();

            //Codes from Patient Clinical Info
            List<string> clinicalInfoCodes = new List<string>();

            foreach (var c in config.Input)
            {
                model.SetInputValue(c.Name, c.DefaultValue);

                if (c.Source == ObservationType)
                {
                    observationCodes.Add(c.Code);
                }
                else if (c.Source ==MetaObservationType)
                {
                    metaObservationCodes.Add(c.Code);

                }
                else if (c.Source == ClinicalInfoType)
                {
                    clinicalInfoCodes.Add(c.Code);
                }
                else
                {

                    throw new NotSupportedException($"Source type {c.Source} not supported from PDManager DSS");

                }
            }

            #region Observation Codes

            var codes = string.Join(",", observationCodes);
            try
            {
                var observations = await _dataProxy.Get<PDObservation>( 10, 0, String.Format("{{patientid:\"{0}\",deviceid:\"\",datefrom:{2},dateto:{3},codeid:\"{1}\",aggr:\"total\"}}", patientId, codes, DateTimeToUnixTimestamp(DateTime.Now.AddDays(-config.AggregationPeriodDays)), DateTimeToUnixTimestamp(DateTime.Now)), null);

                foreach (var c in observations)
                {
                    var observation = config.Input.FirstOrDefault(e => e.Code.ToLower() == c.CodeId.ToLower());

                    if (observation != null)
                    {
                        //Observations are only numeric

                        model.SetInputValue(observation.Name, observation.GetNumericMapping(c.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            #endregion Meta Observation Codes

            #region Meta Observation Codes


            foreach (var code in metaObservationCodes)
            {
                try
                {
                    //Get Aggregated observation
                    var aggrObservation=await _aggregator.Run(patientId, code, DateTime.Now.AddDays(-config.AggregationPeriodDays));                   

                    //Find Corresponding observation in config
                    var metaObservation = config.Input.FirstOrDefault(e => e.Code.ToLower() == code.ToLower());

                        if (metaObservation != null)
                        {
                            //Meta Observations are only numeric
                            model.SetInputValue(metaObservation.Name, metaObservation.GetNumericMapping(aggrObservation.Select(e=>e.Value).DefaultIfEmpty().Average()));
                        }
                   
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }
            }

            #endregion Observation Codes


            #region Clinical Info

            try
            {
                var patient = await _dataProxy.Get<PDPatient>(patientId);

                var clinicalInfoList = GetClinicalInfoList(patient.ClinicalInfo);
                foreach (var c in clinicalInfoList)
                {
                    var clinicalInfo = config.Input.FirstOrDefault(e => e.Code.ToLower() == c.Code.ToLower());

                    if (clinicalInfo != null)
                    {
                        if (clinicalInfo.NumericMapping != null)
                        {
                            double v = double.Parse(c.Value);

                            model.SetInputValue(clinicalInfo.Name, clinicalInfo.GetNumericMapping(v));
                        }
                        else
                        {
                            var v = clinicalInfo.GetCategoryMapping(c.Value);
                            if (v.HasValue)
                            {
                                model.SetInputValue(clinicalInfo.Name,v.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion Clinical Info

            model.Evaluate(Model.Evaluation.SET, true);
            var ret = model.Aggregate.Select(e=>new DSSValue() { Name=e.Name,Value=e.ValuesString});
            return ret;
        }

      
    }
}