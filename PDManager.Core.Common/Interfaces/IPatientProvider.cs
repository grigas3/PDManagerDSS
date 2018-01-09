using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Interfaces
{

    /// <summary>
    /// Patient Provider
    /// </summary>
    public interface IPatientProvider
    {

        /// <summary>
        /// Get Patient Ids
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        IEnumerable<string> GetPatientIds(int take = 0, int skip = 0);

    }
}
