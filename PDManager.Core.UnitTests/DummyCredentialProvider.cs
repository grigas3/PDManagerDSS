using PDManager.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.UnitTests
{
    class DummyCredentialProvider : IProxyCredientialsProvider
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
