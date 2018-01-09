using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Interfaces
{

    /// <summary>
    /// A service exposing a dictionary to get the correct aggregation definition file from a 
    /// </summary>
    public interface IAggrDefinitionProvider
    {

        /// <summary>
        /// Get Aggregation Confing in Json format for specific observation code
        /// </summary>
        /// <param name="code">Meta observation code</param>
        /// <returns>Path of definiton file</returns>
        string GetJsonConfigFromCode(string code);
    
    }
}
