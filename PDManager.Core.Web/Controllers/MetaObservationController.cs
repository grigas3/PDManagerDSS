using Microsoft.AspNetCore.Mvc;
using PDManager.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDManager.Core.Web.Controllers
{
    /// <summary>
    /// Meta observation constroller
    /// </summary>
    [Route("api/v1/[controller]")]
    public class MetaObservationController:Controller
    {

        #region Private fields
        private readonly IAggregator _aggregator;      

        #endregion
        /// <summary>
        /// Meta-observation controller
        /// </summary>
        /// <param name="aggregator">Aggregator</param>
        public MetaObservationController(IAggregator aggregator)
        {
            this._aggregator = aggregator;

        }


        /// <summary>
        /// Get Observation
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="code">Meta-observation code</param>
        /// <returns>An observation</returns>
        [HttpGet]
       public IActionResult Get(string patientId,string code)
       {
            var observation=_aggregator.Run(patientId, code,null);
            //TODO: this may be refactored to the same response of observation controller
            return Ok(observation);

       }

    }
}
