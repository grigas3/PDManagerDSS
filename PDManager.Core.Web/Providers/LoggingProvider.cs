using Microsoft.Extensions.Logging;
using PDManager.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDManager.Core.Web.Providers
{

    /// <summary>
    /// Implentation of IGenericLogger based on ILogger of Microsoft.Extensions.Logging
    /// </summary>
    public class LoggingProvider:IGenericLogger
    {

        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Microsoft.Extensions.Logging Logger</param>
        public LoggingProvider(ILogger<LoggingProvider> logger)
        {

            this._logger = logger;

        }

        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public void LogError(Exception ex, string message)
        {
            _logger?.LogError(ex, message);
        }
    }
}
