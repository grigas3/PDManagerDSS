using Newtonsoft.Json;
using PDManager.Core.Aggregators;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Common.Models;
using PDManager.Core.DSS;
using PDManager.Core.Web.Entities;

namespace PDManager.Core.Web.Extensions
{

    /// <summary>
    /// DSS Model Extension Helpers
    /// </summary>
    public static class ModelExtensions
    {

        /// <summary>
        /// Get Config from deserializing model config in Json format
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DSSConfig GetConfig(this DSSModel model)
        {

             var dssConfig= JsonConvert.DeserializeObject<DSSConfig>(model.Config);
             return dssConfig;
            

            

        }

        /// <summary>
        /// Get Config from file
        /// </summary>
        /// <param name="dssConfigFile"></param>
        /// <returns></returns>
        public static DSSConfig GetDSSConfig(string dssConfigFile)
        {
            var config = DSSConfig.LoadFromFile(dssConfigFile);
            return config;


        }




        /// <summary>
        /// Get Config from deserializing model config in Json format
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static AggrConfigDefinition GetConfig(this AggrModel model)
        {

            var dssConfig = JsonConvert.DeserializeObject<AggrConfigDefinition>(model.Config);
            return dssConfig;




        }

        /// <summary>
        /// Get Config from file
        /// </summary>
        /// <param name="aggrConfigFile"></param>
        /// <returns></returns>
        public static AggrConfigDefinition GetAggrConfig(string aggrConfigFile)
        {
            var config = AggrConfigDefinition.LoadFromFile(aggrConfigFile);
            return config;


        }


    }
}
