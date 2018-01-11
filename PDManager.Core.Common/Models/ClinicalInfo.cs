using System.Collections.Generic;

namespace PDManager.Core.Models
{
    /// <summary>
    /// Clinical Information stored in the Patient ClinicalInfo Entry
    /// </summary>
    public class ClinicalInfo
    {
        /// <summary>
        /// Clinical Information Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Clinical Information Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Clinical Information
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Priority
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// Created By
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        public long Timestamp { get; set; }
    }


    /// <summary>
    /// Clinical Info Collection
    /// </summary>
    public class ClinicalInfoCollection:List<ClinicalInfo>
    {



    }
}