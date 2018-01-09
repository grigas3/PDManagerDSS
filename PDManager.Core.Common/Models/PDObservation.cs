using PDManager.Core.Common.Interfaces;
using System.Collections.Generic;

namespace PDManager.Core.Models
{
    /// <summary>
    /// PD_Manager Observation
    /// This observation is used internaly in PD_Manager for fast and low footprint  data exchange.
    /// This observation has to to with symptom evaluations performed in mobile phone or other IoT devices
    /// The code is unique and the Code has all the necessairy information to convert this information
    /// to a full observation according to FHIR specificiation.
    /// </summary>
   
    public class PDObservation : BasePDObservation, IObservation
    {
        //For new Observations the id should be newid
        private string _id = "newid";
        /// <summary>
        /// Id
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// Value
        /// </summary>
        public double Value { get; set; }


        /// <summary>
        /// Observation Category
        /// </summary>
        public string Category { get; set; }
    }

    /// <summary>
    /// Base PD Observation
    /// </summary>
    public class BasePDObservation
    {

        /// <summary>
        /// Patient Id
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string CodeId { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        public long Timestamp { get; set; }
    }


    /// <summary>
    /// Base class for Fast Aggregations allowing incremental update
    /// </summary>
    public class BaseAggPDObservation : BasePDObservation
    {
        // [JsonIgnore]
        /// <summary>
        /// E[XX]
        /// </summary>
        public double Q2 { get; set; }

        // [JsonIgnore]
        /// <summary>
        /// E[XX]
        /// </summary>
        public double Q1 { get; set; }

        // [JsonIgnore]
        /// <summary>
        /// Number of samples
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// Timestamp of first observation
        /// </summary>
        public long sTimestamp { get; set; }
    }


    /// <summary>
    /// Collection of PD Observations
    /// </summary>
    public class PDObservationCollection:List<PDObservation>
        {

        }
}