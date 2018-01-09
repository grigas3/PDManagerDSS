using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PDManager.Core.DSS
{
    /// <summary>
    /// DSS Mapping Class
    /// </summary>
    public class DSSConfig
    {

        /// <summary>
        /// DSS Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// DSS Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Dexi File
        /// </summary>
        public string DexiFile { get; set; }

        /// <summary>
        /// Value Mappings
        /// </summary>
        public List<DSSValueMapping> Input { get; set; }

        /// <summary>
        /// Aggregation Period Days (Default 30)
        /// </summary>
        public int AggregationPeriodDays { get; set; }

        /// <summary>
        /// Save as Json to a file
        /// </summary>
        /// <param name="config">DSS Config</param>
        /// <param name="file">Output File</param>
        public static void SaveToFile(DSSConfig config, string file)
        {
            StreamWriter fstr = null;
            JsonTextWriter writer = null;
            try
            {
                fstr = new StreamWriter(file);
                writer = new JsonTextWriter(fstr);
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, config);
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
        /// Load From File
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DSSConfig LoadFromFile(string file)
        {
            DSSConfig ret = null;
            StreamReader fstr = null;
            JsonTextReader reader = null;
            try
            {
                fstr = new StreamReader(file);
                reader = new JsonTextReader(fstr);
                JsonSerializer serializer = new JsonSerializer();
                ret = serializer.Deserialize<DSSConfig>(reader);
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
    }
}