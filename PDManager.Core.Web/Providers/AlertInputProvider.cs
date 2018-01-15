using PDManager.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDManager.Core.Web.Providers
{
    public class AlertInputProvider:IAlertInputProvider
    {
        private readonly Context.DSSContext _context;
        public AlertInputProvider(Context.DSSContext context)
        {
            this._context = context;
        }

        public IEnumerable<IAlertInput> GetAlertInputs()
        {
            return _context.AlertModels.ToList();
        }
    }
}
