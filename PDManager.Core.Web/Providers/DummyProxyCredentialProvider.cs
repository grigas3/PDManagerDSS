using PDManager.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDManager.Core.Web
{

    /// <summary>
    /// Dummy Proxy Credential Provider
    /// </summary>
    public class DummyProxyCredentialProvider : IProxyCredientialsProvider
    {

        /// <summary>
        /// password
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return "newpass";
        }

        /// <summary>
        /// User name
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return "admin";
        }
    }
}
