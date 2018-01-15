using PDManager.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PDManager.Core.Common.Interfaces
{


    /// <summary>
    /// Alert Evaluator Interface
    /// </summary>
    public interface IAlertEvaluator
    {

        /// <summary>
        /// Get Alert Level
        /// </summary>
        /// <param name="alert">Alert Input</param>
        /// <param name="patientId">Patient Id</param>
        /// <returns>0: No Alert, 1: Low Priority Alert, 2: Medium Priority Alert, 3: High Priority Alert</returns>
        Task<AlertLevel> GetAlertLevel(IAlertInput alert, string patientId);
      

    }
}
