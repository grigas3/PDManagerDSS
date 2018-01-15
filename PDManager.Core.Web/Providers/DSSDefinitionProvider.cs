using PDManager.Core.Common.Exceptions;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Web.Entities;
using System;
using System.Linq;

namespace PDManager.Core.Web.Context
{

    /// <summary>
    /// Aggregation Definition Provider
    /// </summary>
    public class DSSDefinitionProvider: IDSSDefinitionProvider
    {

        #region Private Declarations
        private readonly Context.DSSContext _context;
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Context</param>
        public DSSDefinitionProvider(Context.DSSContext context)
        {
            _context = context;
            
        }

        /// <summary>
        /// Get Config in JSON format from meta-observation code
        /// </summary>
        /// <param name="code">Meta-observation code</param>
        /// <returns></returns>
        public string GetJsonConfigFromCode(string code)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }


            var model=_context.Set<DSSModel>().FirstOrDefault(e => e.Code == code);

            if (model == null)
                throw new DSSDefinitionNotFoundException(code);


            return model.Config;

        }
    }
}
