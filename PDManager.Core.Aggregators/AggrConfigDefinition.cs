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
    public class AggrConfigDefinition
    {

     

        /// <summary>
        /// Aggregation Type. Possible values
        /// time: Aggregate observations per time of day
        /// day: Aggregate observation per day
        /// total: Aggregation of all values
        /// </summary>
        public string AggregationType { get; set; }


        /// <summary>
        /// Meta Aggregation Type.
        /// Meta Aggregation occurs after aggregation on raw observations and filtering
        /// Possible values
        /// sum: Sum of observations
        /// average: Average of observations        
        /// std: Std of observations
        /// max: Std of observations
        /// min: Std of observations
        /// mfi: Mean Fluctuation Index
        /// cv:  Coefficient of variation
        /// count: Std of observations
        /// none: All observations
        /// </summary>
        public string MetaAggregationType { get; set; }


        /// <summary>
        /// Scale meta aggregated value
        /// Default 1.0
        /// </summary>

        [DefaultValue(1)]
        public double MetaScale { get; set; }


        /// <summary>
        /// Variables
        /// </summary>
        public List<AggrConfigVarDefinition> Variables { get; set; }
        /// <summary>
        /// Beta
        /// </summary>
        public double Beta { get;  set; }

        /// <summary>
        /// Threshold value
        /// Threshold is required in Off Time or Dyskinesia Time estimation
        /// </summary>
        public double ThresholdValue { get; set; }


        /// <summary>
        /// Theshold Type
        /// Thresholds get be 
        /// "fixed" where a static threshold is used
        /// "std" the Threshold is ThresholdValue x STD
        /// 
        /// </summary>
        public string ThresholdType { get; set; }

        /// <summary>
        /// Threshold
        /// </summary>
        public bool Threshold { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }

        #region Helpers
        /// <summary>
        /// Save Aggregation Definition to file
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="file"></param>
        public static void SaveToFile(AggrConfigDefinition definition,string file)
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
        public static AggrConfigDefinition LoadFromFile(string file)
        {
            AggrConfigDefinition ret = null;
            StreamReader fstr = null;
            JsonTextReader reader = null;
            try
            {
                fstr = new StreamReader(file);
                reader = new JsonTextReader(fstr);
                JsonSerializer serializer = new JsonSerializer();
                ret = serializer.Deserialize<AggrConfigDefinition>(reader);
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
        public static AggrConfigDefinition FromString(string configJson)
        {
            AggrConfigDefinition ret = null;            
             return   ret = JsonConvert.DeserializeObject<AggrConfigDefinition>(configJson);
            
        }
        #endregion



    }

    /// <summary>
    /// Aggregation Variable Definition
    /// </summary>
    public class AggrConfigVarDefinition
    {

        //TODO: Probably Remove Source
        /// <summary>
        /// Source of Variable
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Code 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Weight
        /// </summary>
        public double Weight { get; set; }

    }
}
