using Hangfire;
using Microsoft.AspNetCore.Mvc;
using PDManager.Core.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDManager.Web.Controllers
{

    /// <summary>
    /// Job Controller
    /// </summary>
    [Route("api/v1/[controller]")]
    public class JobController:Controller
    {

        private readonly IJobFactory _jobFactory;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jobFactory"></param>
        public JobController(IJobFactory jobFactory)
        {

            _jobFactory = jobFactory;

        }
        /// <summary>
        /// start Job Scheduling
        /// </summary>
        /// <returns></returns>
        [HttpGet("start")]
        public IActionResult Start()
        {

            var jobs = _jobFactory.GetJobs();
            foreach (var job in jobs)
            {
                RecurringJob.AddOrUpdate(job.GetId(),
   () =>job.Run(),
  Cron.Daily);

            }

            return Ok($"{jobs.Count()} Jobs registered");

        }
    }
}
