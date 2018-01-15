﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PDManager.Core.Web.Entities
{
    /// <summary>
    /// Aggregation Model for Creating Meta-observations
    /// </summary>
    public  class AggrModel
    {
        /// <summary>
        /// DSS ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// DSS Name
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// DSS Description
        /// </summary>
        [StringLength(5000)]
        public string Description { get; set; }


        /// <summary>
        /// Meta-Observation Code
        /// </summary>
        [StringLength(10)]
        public string Code { get; set; }

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
