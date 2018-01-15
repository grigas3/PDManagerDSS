using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PDManager.Core.Common.Enums;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Web.Entities;
using PDManager.Core.Web.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDManager.Core.Web.Controllers
{
    /// <summary>
    /// DSS Controller
    /// </summary>
    [Route("api/v1/[controller]")]
    public class AlertController : Controller
    {
        #region Private Declarations        
        private readonly Context.DSSContext _context;
        private readonly ILogger _logger;
        private readonly IAlertEvaluator _alertEvaluator;
        #endregion

        #region Controllers
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Data Context</param>
        /// <param name="alertEvaluator"></param>        
        /// <param name="logger">Logger</param>
        public AlertController(Context.DSSContext context,IAlertEvaluator alertEvaluator,ILogger<DSSController> logger)
        {          
            _context = context;
            _logger = logger;
            _alertEvaluator = alertEvaluator;
            
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get List of dss models
        /// </summary>
        /// <returns>List of dss models</returns>
        
        [HttpGet]
        public IActionResult Get()
        {
            var list = _context.Set<AlertModel>().ToList();
            return Ok(list);

        }
        
        /// <summary>
        /// Get dss model
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of dss models</returns>
        
        public IActionResult Get(int id)
        {
            var item = _context.Find<AlertModel>(id);

            if (item == null)
                return NotFound("DSS Model not found");
            //Return item
            return Ok(item);

        }

        /// <summary>
        /// Get DSS Config
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>

        [HttpGet("evaluate/{id}")]
        public async Task<IActionResult> Evaluate(string id,string patientId)
        {

          

                var model = _context.Find<AlertModel>(int.Parse(id));

                if (model == null)
                    return NotFound("DSS Model not found");

                var ret = await this._alertEvaluator.GetAlertLevel(model,patientId);

           

                return Ok(new {

                    Message=model.Message,
                    Level=ret,
                    Color=GetColor(ret)

                });

            

        }

        private string GetColor(AlertLevel ret)
        {

            switch(ret)
            {

                case AlertLevel.None:return "success";
                case AlertLevel.Low: return "info";
                    case AlertLevel.Medium: return "warning";
                case AlertLevel.High: return "danger";
                default:return "default";

            }

        }

      
        #endregion


        #region Testing Methods

        /// <summary>
        /// Create Dummy Data
        /// </summary>
        /// <returns></returns>
        // GET api/dss/dummy
        [HttpGet("dummy")]
        public IActionResult CreateDummy()
        {
           
            AddDummyAlerts(_context);

            return Ok();
        }
             

        private void AddDummyAlerts(Context.DSSContext context)
        {
            string config = string.Empty;
          
            try
            {

                context.Set<AlertModel>().Add(new AlertModel()
                {

                    Name = "Fluctuations",
                    Description = "Fluctuations",
                    TargetValueCode="STFLUCT",
                    IsSystem=true,
                    TargetValueNumeric=true,
                    Message="Patient has severe fluctuations",
                    TargetValueSource="metaobservation",                    
                    HighPriorityValue="8",
                    CreatedBy = "admin",
                    ModifiedBy = "admin",
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Id = 1


                });
                context.Set<AlertModel>().Add(new AlertModel()
                {

                    Name = "Medication Change",
                    Description = "Medication Change",
                    TargetValueCode = "MedicationChange",
                    IsSystem = true,
                    TargetValueNumeric = false,
                    Message = "Patient should change medication",
                    TargetValueSource = "dss",
                    LowPriorityValue = "maybe",
                    HighPriorityValue = "change",
                    CreatedBy = "admin",
                    ModifiedBy = "admin",
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Id = 2


                });

                context.SaveChanges();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Test Data");

            }


        }
        #endregion


    }
}