using System.Collections.Generic;
using System.Linq;

namespace PDManager.Core.DSS
{
    /// <summary>
    /// Categorical Value Mapping
    /// This is used to map input categories to DSS specific values
    /// </summary>
    public class DSSCategoricalValueMapping
    {
        /// <summary>
        /// Name value stored in repository
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value in DSS
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Value Meaning in DSS
        /// </summary>
        public string ValueMeaning { get; set; }
    }

    /// <summary>
    /// Helper Class that is simple a list of DSSCategoricalValueMapping
    /// </summary>
    public class DSSCategoricalValueMappingList : List<DSSCategoricalValueMapping>
    {
      
    }
}