using PDManager.Core.Common.Interfaces;
using System.Collections.Generic;

namespace PDManager.Core.Aggregators
{
    /// <summary>
    /// Batch Data Aggregator
    /// </summary>
    public interface IBatchDataAggregator
    {
        /// <summary>
        /// Aggregator Code
        /// </summary>
        /// <returns>The unique code of the aggregator</returns>
        string GetCode();

        /// <summary>
        /// Aggregate a list of observations
        /// </summary>
        /// <param name="observations">List of observations</param>
        void Aggregate(IEnumerable<IObservation> observations);
    }
}