using System;

namespace PDManager.Core.Models
{
    /// <summary>
    /// PD Patient
    /// </summary>
    public class PDPatient
    {
        /// <summary>
        /// Patient Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Clinical Info
        /// </summary>
        public string ClinicalInfo { get; set; }

        /// <summary>
        /// Medical History
        /// </summary>
        public string MedicalHistory { get; set; }

        /// <summary>
        /// Birth Date
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// PD Appearance
        /// </summary>
        public DateTime? PDAppearance { get; set; }
    }
}