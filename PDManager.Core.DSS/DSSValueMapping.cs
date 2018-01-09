using Newtonsoft.Json;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.DSS.Helpers;
using System.Collections.Generic;
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
        public string Name { get; set; }

        /// <summary>
        /// Observation or Patient History Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Default Value
        /// </summary>
        public int DefaultValue { get; set; }

        /// <summary>
        /// For a second version, one could provide a generic URI that could be used to fetch data
        /// The URI must be interpolated with DSS properties (patientid, datetime, code etc)
        /// line angular interpolation
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Source
        /// Observations or Patient History (or other?)
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Numerical/Categorical
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// Name Mapping
        /// </summary>
      
        public DSSCategoricalValueMappingList CategoryMapping { get; set; }

        /// <summary>
        /// Numeric Bins Mapping
        /// </summary>
        //[JsonIgnore]
        public DSSNumericBinCollection NumericBins { get; set; }

        /// <summary>
        /// Numeric Mapping
        /// </summary>
        //[JsonIgnore]
        public DSSNumericMapping NumericMapping { get; set; }

        /// <summary>
        /// Numeric type
        /// </summary>
        public bool Numeric{ get { return ValueType == "Numeric"; } }

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