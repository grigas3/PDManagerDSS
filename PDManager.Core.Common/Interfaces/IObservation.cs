using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Interfaces
{
    /// <summary>
    /// Basic interface with the minimum number of properties an Observation should have
    /// </summary>
    public interface IObservation
    {
        /// <summary>
        /// Value
        /// </summary>
        double Value { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        string CodeId { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        long Timestamp { get; set; }
    }
}
