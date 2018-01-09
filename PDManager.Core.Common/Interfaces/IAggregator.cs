﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PDManager.Core.Common.Interfaces
{

    /// <summary>
    /// Aggregator interface
    /// </summary>
    public interface IAggregator
    {
        /// <summary>
        /// Run Aggregation for specific patient and code
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="code">Meta Observation Code</param>
        /// <param name="lastExecutionTime">Last Execution time</param>
        /// <returns></returns>
        Task<IObservation> Run(string patientId, string code, DateTime? lastExecutionTime);

    
    }
}
