using System.Collections.Generic;

namespace PDManager.Core.DSS
{
    /// <summary>
    /// Numeric Bin
    /// </summary>
    public class DSSNumericBin
    {
        /// <summary>
        /// Minimum numeric Value
        /// </summary>
        public double MinValue { get; set; }

        /// <summary>
        /// Maximum numeric value
        /// </summary>
        public double MaxValue { get; set; }

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
    /// Numeric Bin Collection
    /// </summary>
    public class DSSNumericBinCollection:List<DSSNumericBin>
    {

    }
}