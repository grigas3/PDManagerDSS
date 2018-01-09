using System;
using System.ComponentModel.DataAnnotations;

namespace PDManager.Core.Web.Entities
{
    /// <summary>
    /// DSS model    
    /// </summary>
    public class DSSModel
    {
        /// <summary>
        /// DSS ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// DSS Title
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// DSS Description
        /// </summary>
        [StringLength(5000)]
        public string Description { get; set; }

        /// <summary>
        /// DSS Version
        /// </summary>
        [StringLength(10)]
        public string Version { get; set; }

        /// <summary>
        /// DSS Config in Json Format
        /// </summary>
        public string Config { get; set; }


        /// <summary>
        /// Created By
        /// </summary>
        [StringLength(100)]
        public string CreatedBy { get; set; }



        /// <summary>
        /// Modified By
        /// </summary>
        [StringLength(100)]
        public string ModifiedBy { get; set; }


        /// <summary>
        /// Created Date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Modified Date
        /// </summary>
        public DateTime ModifiedDate { get; set; }
        

      
    }
}