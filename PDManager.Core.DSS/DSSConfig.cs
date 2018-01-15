using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Description("DSS name")]
        [JsonRequired]
        public string Name { get; set; }
        /// <summary>
        /// DSS Version
        /// </summary>
        [Description("Version of the DSS Model")]
        [JsonRequired]
        public string Version { get; set; }

        /// <summary>
        /// Dexi File
        /// </summary>
        [Description("Dexi File Reference")]
        [JsonRequired]
        public string DexiFile { get; set; }

        /// <summary>
        /// Value Mappings
        /// </summary>
        [Description("Input")]
        public List<DSSValueMapping> Input { get; set; }

        /// <summary>
        /// Aggregation Period Days (Default 30)
        /// </summary>
        [Description("Aggregation Period Days(Default 30)")]
        [JsonRequired]
        public int AggregationPeriodDays { get; set; }


        #region Helpers

        /// <summary>
        /// Load From File
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static DSSConfig FromString(string config)
        {
            DSSConfig ret = null;
            try
            {

                ret = JsonConvert.DeserializeObject<DSSConfig>(config);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }
        #endregion




    }
}