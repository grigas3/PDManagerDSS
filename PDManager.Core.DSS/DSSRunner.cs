using Newtonsoft.Json;
using PDManager.Core.Common.Extensions;
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
            //Dictionary<string, int> valueMapping = new Dictionary<string, int>();
            var config = JsonConvert.DeserializeObject<DSSConfig>(configJson);

            //TODO: Handle Exceptions
            var model = LoadModel(config.DexiFile);
            return Evaluate(model, config, values);

        }


        /// <summary>
        /// Evaluate
        /// </summary>
        /// <param name="model"></param>
        /// <param name="config"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private IEnumerable<DSSValue> Evaluate(Model model,DSSConfig config, Dictionary<string, string> values)
        {

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
            var ret = model.Aggregate.Select(e => new DSSValue() { Name = e.Name, Code=e.Name,Value = e.ValuesString });
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

           
            Dictionary<string, int> valueMapping = new Dictionary<string, int>();
            var config = JsonConvert.DeserializeObject<DSSConfig>(configJson);
            var ret = await GetInputValues(patientId, config);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private async Task<IEnumerable<DSSValue>> GetInputValues(string patientId, DSSConfig config)
        {
            List<DSSValue> values = new List<DSSValue>();
            //Codes from observations
            List<string> observationCodes = new List<string>();

            //Codes from Meta o
            List<string> metaObservationCodes = new List<string>();

            //Codes from Patient Clinical Info
            List<string> clinicalInfoCodes = new List<string>();

            foreach (var c in config.Input)
            {


                if (string.IsNullOrEmpty(c.Source))
                {
                    //TODO: Add some error to response
                    continue;
                }


                if (c.Source.ToLower() == ObservationType)
                {
                    if(!observationCodes.Contains(c.Code))
                    observationCodes.Add(c.Code);
                }
                else if (c.Source.ToLower() == MetaObservationType)
                {
                    if (!metaObservationCodes.Contains(c.Code))
                        metaObservationCodes.Add(c.Code);

                }
                else if (c.Source.ToLower() == ClinicalInfoType)
                {
                    if (!clinicalInfoCodes.Contains(c.Code))
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
                    var observations = await _dataProxy.Get<PDObservation>(10, 0, String.Format("{{patientid:\"{0}\",deviceid:\"\",datefrom:{2},dateto:{3},codeid:\"{1}\",aggr:\"total\"}}", patientId, code, (DateTime.Now.AddDays(-config.AggregationPeriodDays).ToUnixTimestampMilli()), (DateTime.Now.ToUnixTimestampMilli())), null);

                  
                        foreach (var observation in config.Input.Where(e => e.Code == code))
                        {
                            values.Add(new DSSValue() { Name = observation.Name, Code = observation.Code, Value = observations.Select(e => e.Value).DefaultIfEmpty(0).Average().ToString() });

                        }

                   // }
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
                        values.Add(new DSSValue() { Name = metaObservation.Name, Code = metaObservation.Code, Value = aggrObservation.Select(e => e.Value).DefaultIfEmpty(0).Average().ToString() });


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

                        values.Add(new DSSValue() { Name = clinicalInfo.Name, Code = clinicalInfo.Code, Value = c.Value });

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
        /// <param name="configJson">DSS Config as Json String file</param>
        /// <returns></returns>
        public async Task<IEnumerable<DSSValue>> Run(string patientId, string configJson)
        {
            Dictionary<string, int> valueMapping = new Dictionary<string, int>();
            var config =DSSConfig.FromString(configJson);

            //TODO: Handle Exceptions
            var model = LoadModel(config.DexiFile);

            // Set initial values
            foreach (var c in config.Input)
            {
                model.SetInputValue(c.Name, c.DefaultValue);
            }

            //Get DSS input values
            var values =await GetInputValues(patientId, configJson);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach(var c in values)
            {
                if(!dict.ContainsKey(c.Name))
                dict.Add(c.Name, c.Value);
                
            }
            return Evaluate(model, config, dict);
        }

      
    }
}