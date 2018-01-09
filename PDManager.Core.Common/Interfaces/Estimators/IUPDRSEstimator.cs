using PDManager.Core.Common.Interfaces;
using PDManager.Core.Models;
using System.Collections.Generic;

namespace PDManager.Core.Estimators
{
    public interface IUPDRSEstimator
    {
        /// <summary>
        /// Get UPDRS overall score based on Observations
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="observations"></param>
        /// <returns></returns>
        UPDRSScoreResult GetScore(string pid, IEnumerable<IObservation> observations);
    }
}