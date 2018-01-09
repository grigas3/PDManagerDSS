using System;

namespace PDManager.Core.Common.Results
{
    /// <summary>
    /// Login Result
    /// </summary>
    public class LoginResult
    {

        //TODO: Use C# proper naming and Json attributes for the json fields

            /// <summary>
            /// Access token
            /// </summary>
        public String access_token { get; set; }
        /// <summary>
        /// Token Type
        /// </summary>
        public String token_type { get; set; }

        /// <summary>
        /// Expires in
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// Succes 
        /// </summary>
        public bool success { get; set; }
    }
}