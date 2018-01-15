using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDManager.Core.Aggregators
{

    /// <summary>
    /// Aggregation Config Definition
    /// Meta Observations come from complex aggregation of basic observations
    /// An example is the OFF time which comes from the total UPDRS score
    /// <example>{"AggregationType":"time","Variables":[{"Uri":"api/observations","Code":"STBRADS30","Weight":1.0},{"Uri":"api/observations","Code":"STDYS30","Weight":-1.0}],"Beta":0.0}</example>
    /// </summary>
    public class AggrConfig
    {



        /// <summary>
        /// Aggregation Type. Possible values
        /// time: Aggregate observations per time of day
        /// day: Aggregate observation per day
        /// total: Aggregation of all values
        /// </summary>
        [JsonRequired]
        [Description("Aggregation Type. Possible values 1) time: Aggregate observations per time of day, 2) day: Aggregate observation per day, 3) total: Aggregation of all values")]
        public string AggregationType { get; set; }


        /// <summary>
        /// Meta Aggregation Type.
        /// Meta Aggregation occurs after aggregation on raw observations and filtering
        /// Possible values
        /// sum: Sum of observations
        /// average: Average of observations        
        /// std: Std of observations
        /// max: max of observations
        /// min: min of observations
        /// mfi: Mean Fluctuation Index
        /// cv:  Coefficient of variation
        /// count: count number of observations
        /// none: Get all metaobservations
        /// </summary>
        [JsonRequired]
        [Description("Meta Aggregation Type.  Meta Aggregation occurs after aggregation on raw observations and filtering. Possible values 1) sum: Sum of observations 2) average: Average of observations, 3) std: Std of observations, 4) max: max of observations 5) min: min of observations, 6) mfi: Mean Fluctuation Index, 7) cv:  Coefficient of variation, 8) count: count number of observations, 9)  none: Get all metaobservations         ")]
        public string MetaAggregationType { get; set; }


        /// <summary>
        /// Scale meta aggregated value
        /// Default 1.0
        /// </summary>

        [DefaultValue(1)]
        [JsonRequired]
        [Description("Scale meta aggregated value")]
        public double MetaScale { get; set; }


        /// <summary>
        /// Variables
        /// </summary>
        [JsonRequired]
        public List<AggrVariable> Variables { get; set; }
        /// <summary>
        /// Beta
        /// </summary>

        [Description("Linear Aggregation is a linear regression of the form A*X+B where X the input variables. The Beta property corresponse to the B of the linear regression function")]

        public double Beta { get;  set; }

        /// <summary>
        /// Threshold value
        /// Threshold is required in Off Time or Dyskinesia Time estimation
        /// </summary>
        [Description("Threshold Value applied in the filter step of the aggregation")]

        public double ThresholdValue { get; set; }


        /// <summary>
        /// Theshold Type
        /// Thresholds get be 
        /// "fixed" where a static threshold is used
        /// "std" the Threshold is ThresholdValue x STD
        /// 
        /// </summary>
        [Description("Threshold type applied on filter step of aggregation")]

        public string ThresholdType { get; set; }

        /// <summary>
        /// Threshold
        /// </summary>
        [Description("Use a thresholding in filter step.")]
        public bool Threshold { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        [Description("Aggregated observation code")]
        [JsonRequired]
        public string Code { get; set; }


        /// <summary>
        /// Name
        /// </summary>
        [Description("Name of the aggregation variable")]
        [JsonRequired]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Version
        /// </summary>
        [Description("Version of the aggregation definition")]
        [JsonRequired]
        public string Version { get; set; }

        #region Helpers
        /// <summary>
        /// Save Aggregation Definition to file
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="file"></param>
        public static void SaveToFile(AggrConfig definition,string file)
        {

            StreamWriter fstr = null;
            JsonTextWriter writer = null;
            try
            {
                fstr = new StreamWriter(file);
                writer = new JsonTextWriter(fstr);
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, definition);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
                if (fstr != null)
                    fstr.Dispose();
            }



        }

        /// <summary>
        /// Load Aggregation Definition from file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static AggrConfig LoadFromFile(string file)
        {
            AggrConfig ret = null;
            StreamReader fstr = null;
            JsonTextReader reader = null;
            try
            {
                fstr = new StreamReader(file);
                reader = new JsonTextReader(fstr);
                JsonSerializer serializer = new JsonSerializer();
                ret = serializer.Deserialize<AggrConfig>(reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (fstr != null)
                    fstr.Dispose();
            }

            return ret;
        }


        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {            
            return JsonConvert.SerializeObject(this);
            
        }

        /// <summary>
        /// Create Aggregation Definition from string
        /// </summary>
        /// <param name="configJson">Config in json</param>
        /// <returns></returns>
        public static AggrConfig FromString(string configJson)
        {
            AggrConfig ret = null;            
             return   ret = JsonConvert.DeserializeObject<AggrConfig>(configJson);
            
        }
        #endregion



    }

    /// <summary>
    /// Aggregation Variable Definition
    /// </summary>
    public class AggrVariable
    {

        //TODO: Probably Remove Source
        /// <summary>
        /// Source of Variable. The source can be 1) observation and 2) clinical
        /// </summary>
        [Description("Source of the variable the source can be 1) observation and 2) clinical")]
        [JsonRequired]
        public string Source { get; set; }
        /// <summary>
        /// Code 
        /// </summary>
        [Description("Variable code")]
        [JsonRequired]
        public string Code { get; set; }
        /// <summary>
        /// Weight
        /// </summary>
        [Description("Variable weight. This is the A in the Ax+B regression function.")]
        [JsonRequired]
        public double Weight { get; set; }

    }
}
