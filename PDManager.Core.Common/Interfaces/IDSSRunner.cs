using PDManager.Core.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDManager.Core.Common.Interfaces
{
    /// <summary>
    /// DSS Runner
    /// </summary>
    public interface IDSSRunner
    {

        /// <summary>
        /// Run (used for testing) with a dictionary of values
        /// </summary>
        /// <param name="configJson">DSS Config in Json format</param>
        /// <param name="values"></param>
        /// <returns>List of DSS Values</returns>
        IEnumerable<DSSValue> Run(string configJson, Dictionary<string, string> values);
        /// <summary>
        /// Run DSS
        /// </summary>
        /// <param name="patientId"> patient Id</param>
        /// <param name="dssConfigFile">DSS Config file (Json)</param>
        /// <returns>List of DSS Values</returns>
        Task<IEnumerable<DSSValue>> Run(string patientId, string dssConfigFile);

    }
}