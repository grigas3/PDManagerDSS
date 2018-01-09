using System.Collections.Generic;

namespace PDManager.Core.Common.Models
{
    /// <summary>
    /// DSS Value
    /// </summary>
    public class DSSValue
    {

        /// <summary>
        /// DSS Name for variable
        /// </summary>
        public string Name { get; set; }
    
        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Numeric or Categorical variable
        /// </summary>
        public bool Numeric { get; set; }
    }

    /// <summary>
    /// DSS Value Collection
    /// </summary>
    public class DSSValueCollection:List<DSSValue>
    {

    }
}