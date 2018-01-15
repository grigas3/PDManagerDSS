using Newtonsoft.Json;
using PDManager.Core.Aggregators;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Common.Models;
using PDManager.Core.DSS;
using PDManager.Core.Web.Entities;
using System;
using System.IO;

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
            var config = LoadFromFile(dssConfigFile);
            return config;


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

        /// <summary>
        /// Get Config from deserializing model config in Json format
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static AggrConfig GetConfig(this AggrModel model)
        {

            var dssConfig = JsonConvert.DeserializeObject<AggrConfig>(model.Config);
            return dssConfig;




        }

        /// <summary>
        /// Get Config from file
        /// </summary>
        /// <param name="aggrConfigFile"></param>
        /// <returns></returns>
        public static AggrConfig GetAggrConfig(string aggrConfigFile)
        {
            var config = AggrConfig.LoadFromFile(aggrConfigFile);
            return config;


        }


    }
}
