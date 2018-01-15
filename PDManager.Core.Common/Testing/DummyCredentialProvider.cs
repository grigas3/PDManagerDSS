using PDManager.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Testing
{
    /// <summary>
    /// Dymmy Credential Provider
    /// In Case Data Proxy is used where data are collected from a web api with authentication
    /// then credentials are provided by IProxyCredientialsProvider
    /// </summary>
    public class DummyCredentialProvider : IProxyCredientialsProvider
    {

        
        /// <summary>
        /// password Removed values for security
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return "XXXXX";
        }

        /// <summary>
        /// User name (Removed values for security)
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return "XXXX";
        }
    }
    
}
