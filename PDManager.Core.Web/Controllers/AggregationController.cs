using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PDManager.Core.Aggregators.Testing;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Web.Entities;
using PDManager.Core.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDManager.Core.Web.Controllers
{ 
    
    
    /// <summary>
  /// DSS Controller
  /// </summary>
    [Route("api/v1/[controller]")]
    public class AggregationController : Controller
    {
        #region Private Declarations     
        private readonly IAggregator _aggregator;
        private readonly Context.DSSContext _context;
        private readonly ILogger _logger;
        #endregion

        #region Controllers
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Data Context</param>
        /// <param name="aggregator"></param>        
        /// <param name="logger">Logger</param>
        public AggregationController(Context.DSSContext context,IAggregator aggregator, ILogger<AggregationController> logger)
        {
            _context = context;
            _logger = logger;
            _aggregator = aggregator;
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
            var list = _context.Set<AggrModel>().ToList();
            return Ok(list);

        }

        /// <summary>
        /// Get dss model
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of aggregation models</returns>

        public IActionResult Get(int id)
        {
            var item = _context.Find<AggrModel>(id);

            if (item == null)
                return NotFound("DSS Model not found");
            //Return item
            return Ok(item);

        }



        /// <summary>
        /// Get Aggregation Config
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("config/{id}")]
        public IActionResult Config(string id)
        {

            //If Id is empty then we load the default model
            if (string.IsNullOrEmpty(id))
            {

                //FOR Test
                var ret = ModelExtensions.GetAggrConfig("onoff.json");
                return Ok(ret);
            }

            else
            {

                var model = _context.Find<AggrModel>(int.Parse(id));

                if (model == null)
                    return NotFound("Aggregation Model not found");

                var ret = model.GetConfig();
                return Ok(ret);

            }

        }

        /// <summary>
        /// Evaluate
        /// </summary>
        /// <param name="code"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("evaluate")]
        public async Task<IActionResult> Evaluate(string code, string patientId)
        {

            var res = await _aggregator.Run(patientId, code, null);            
            return Ok(res);
        }
        /// <summary>
        /// Post Aggr Model
        /// Call: POST api/aggregation/5
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(AggrModel model)
        {

            try
            {
                var newModel = await _context.AddAsync(model);
                var ret = await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Post DSS Model failed");
                return BadRequest("Post DSS Model failed");
            }


            return Ok(model);

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
            AddDummyAggr(_context);
       
            return Ok();
        }



        private void AddDummyAggr(Context.DSSContext context)
        {
            string config = string.Empty;
           
            try
            {

                List<AggrModel> models = new List<AggrModel>()
                {
                new AggrModel()
                {

                    Name = "OFFTIME",
                    Description = "Dummy on off estimation",
                    Code = "STOFFDUR",
                    CreatedBy = "admin",
                    ModifiedBy = "admin",
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Version = "1.0",
                    Id = 1


                },
                   new AggrModel()
                {

                    Name = "UPDRS",
                    Description = "Day (per 30 minute) UPDRS estimation",
                    Code = "UPDRS",
                    CreatedBy = "admin",
                    ModifiedBy = "admin",
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Version = "1.0",
                    Id = 2


                },
                     new AggrModel()
                {

                    Name = "STFLUCT",
                    Description = "UPDRS IV Patient Fluctuation Score based on the MFI score",
                    Code = "STFLUCT",
                    CreatedBy = "admin",
                    ModifiedBy = "admin",
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Version = "1.0",
                    Id = 3


                }
                };


                DummyAggrDefinitionProvider provider = new DummyAggrDefinitionProvider();

                foreach(var model in models)
                {

                    model.Config = provider.GetJsonConfigFromCode(model.Code);
                    context.Add(model);

                }

                context.SaveChanges();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Test Aggregation Data");

            }


        }

        #endregion


    }
}
