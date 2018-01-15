using Newtonsoft.Json;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.DSS.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PDManager.Core.DSS
{
    /// <summary>
    /// DSS Value Mapping
    /// </summary>
    public class DSSValueMapping: IDSSValueMapping
    {
        /// <summary>
        /// Name in Dexi Model
        /// </summary>
        [Description("name")]
        public string Name { get; set; }

        /// <summary>
        /// Observation or Patient History Code
        /// </summary>
        [Description("Code")]
        [JsonRequired]
        public string Code { get; set; }

        /// <summary>
        /// Default Value.
        /// This value is used if the attribute is not available in the repository
        /// </summary>
        [Description("Default Value. This value is used if the attribute is not available in the repository")]
        [JsonRequired]
        public int DefaultValue { get; set; }


        /// <summary>
        /// Source
        /// Observations or Patient History (or other?)
        /// Possible values 
        /// 1) observation
        /// 2) clinical 
        /// </summary>
        [Description("Source of attribute. The possible values are 1) observation and 2) clinical ")]
        [JsonRequired]
        public string Source { get; set; }

        /// <summary>
        /// Numerical/Categorical.
        /// Value Type. Possible values are 'numeric' for numeric attributes 'categorical' for categorical attributes
        /// </summary>

        [Description("Value Type. Possible values are 'numeric' for numeric attributes 'categorical' for categorical attributes")]
        [JsonRequired]
        public string ValueType { get; set; }

        /// <summary>
        /// Name Mapping.
        /// Category mapping for categorical values. It maps the original value to a DEXI model one
        /// </summary>
        [Description("Category mapping for categorical values. It maps the original value to a DEXI model one")]
        public DSSCategoricalValueMappingList CategoryMapping { get; set; }

        /// <summary>
        /// Numeric Bins Mapping.
        /// Numeric bins are use to map continuous values to specific DEXI discrete input values
        /// </summary>
        //[JsonIgnore]

        [Description("Numeric bins are use to map continuous values to specific DEXI discrete input values")]
        public DSSNumericBinCollection NumericBins { get; set; }

        /// <summary>
        /// Numeric Mapping
        /// </summary>
        //[JsonIgnore]
        [Description("NumericMapping is used to scale and translate if required the original numeric value BEFORE  are use to map continuous values to specific DEXI discrete input values")]
        public DSSNumericMapping NumericMapping { get; set; }

        /// <summary>
        /// Numeric type
        /// </summary>
        public bool Numeric{ get { return !string.IsNullOrEmpty(ValueType)&&ValueType.ToLower() == "numeric"; } }

        /// <summary>
        /// Cet DEXI Category
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public int? GetCategoryMapping(string cat)
        {
            return CategoryMapping.FirstOrDefault(e => e.Name == cat)?.Value;// g[value];
        }

        /// <summary>
        /// Get Numeric Mapping
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetNumericMapping(double value)
        {
            double cvalue = value;
            if (this.NumericMapping != null)
                cvalue = (value * this.NumericMapping.Scale + this.NumericMapping.Bias);

            if (NumericBins != null)
            {
                foreach (var bin in NumericBins)
                {
                    if (cvalue >= bin.MinValue && cvalue <= bin.MaxValue)
                        return bin.Value;
                }
            }

            return (int)cvalue;
        }
    }



  
}