using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Interfaces
{
    /// <summary>
    /// Interface for DSS Value Mapping from PDManager Observation to Dexi
    /// </summary>
    public interface IDSSValueMapping
    {
        /// <summary>
        /// Name in Dexi Model
        /// </summary>
         string Name { get; set; }

        /// <summary>
        /// Default Value
        /// </summary>
         int DefaultValue { get; set; }

        /// <summary>
        /// Numerical/Categorical
        /// </summary>
        string ValueType { get; set; }


        /// <summary>
        /// If Variable is numeric
        /// </summary>
        bool Numeric { get; }
    }
}
