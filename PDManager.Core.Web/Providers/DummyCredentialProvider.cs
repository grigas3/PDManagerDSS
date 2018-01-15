using Microsoft.Extensions.Configuration;
using PDManager.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Web
{
    /// <summary>
    /// Dymmy Credential Provider
    /// In Case Data Proxy is used where data are collected from a web api with authentication
    /// then credentials are provided by IProxyCredientialsProvider
    /// </summary>
    public class DummyCredentialProvider : IProxyCredientialsProvider
    {

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public DummyCredentialProvider(IConfiguration configuration)
        {
            _configuration = configuration;


        }
        /// <summary>
        /// password
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return _configuration["PDUserName"];
        }

        /// <summary>
        /// User name
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return _configuration["PDPassword"];
        }
    }
    
}
