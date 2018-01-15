using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Interfaces
{
    /// <summary>
    /// Alert Input Provider
    /// </summary>
    public interface IAlertInputProvider
    {
        /// <summary>
        /// Get Alert Inputs
        /// </summary>
        /// <returns></returns>
        IEnumerable<IAlertInput> GetAlertInputs();
    }
}
