using PDManager.Core.Common.Interfaces;
using System.Collections.Generic;

namespace PDManager.Core.Aggregators
{
    /// <summary>
    /// Fast Data Aggregator
    /// </summary>
    public interface IFastDataAggregator
    {
        /// <summary>
        /// Check if it accepts data
        /// </summary>
        /// <param name="dataCode">Type of data</param>
        /// <returns>True/False</returns>
        bool AcceptData(string dataCode);

        /// <summary>
        /// Aggregate observations (of the same type, accepted by AcceptData)
        /// </summary>
        /// <param name="observations"></param>
        void Aggregate(IEnumerable<IObservation> observations);
    }
}