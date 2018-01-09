using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Interfaces
{

    /// <summary>
    /// Get Credentials for DataProxy
    /// This is used in order to get access token for B3D Web API
    /// Important! DataProxy should call directly MongoDb in integrated version
    /// and this interface may not be required    
    /// </summary>
    public interface IProxyCredientialsProvider
    {

        /// <summary>
        /// Get Test UserName
        /// </summary>
        /// <returns></returns>
        string GetUserName();

        /// <summary>
        /// Get Test Password
        /// </summary>
        /// <returns></returns>
        string GetPassword();
    }
}
