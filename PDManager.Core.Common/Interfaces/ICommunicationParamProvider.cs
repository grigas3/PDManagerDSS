using PDManager.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Interfaces
{
    /// <summary>
    /// Communication Param Provider
    /// </summary>
    public interface  ICommunicationParamProvider
    {
        /// <summary>
        /// Get Parameters
        /// </summary>
        /// <returns></returns>
        CommunicationParameters GetParameters();
    }
}
