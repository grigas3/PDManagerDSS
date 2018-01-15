using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Interfaces
{
    /// <summary>
    /// Job Factory
    /// </summary>
    public interface IJobFactory
    {
        IEnumerable<IRecurringJob> GetJobs();
    }
}
