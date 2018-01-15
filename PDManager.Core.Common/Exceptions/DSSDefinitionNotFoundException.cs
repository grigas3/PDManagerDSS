using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Exceptions
{
    /// <summary>
    /// Aggregation Definition not found exception
    /// </summary>
    public class DSSDefinitionNotFoundException:Exception
    {

        /// <summary>
        /// Definition File not found
        /// </summary>
        public string DefFile { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defFile">Definition file</param>
        public DSSDefinitionNotFoundException(string defFile)
        {

            this.DefFile = defFile;
        }

    }
}
