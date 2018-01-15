using PDManager.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDManager.Core.Web.Entities
{
    /// <summary>
    /// Alert Model
    /// </summary>
    public class AlertModel:IAlertInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Alert Name
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Message of alert 
        /// </summary>
        [StringLength(100)]
        public string Message { get; set; }


        /// <summary>
        /// UserId
        /// Physician Id who (if) created the alert
        /// </summary>
        [StringLength(100)]
        public string UserId { get; set; }


        /// <summary>
        /// Alerts may be either system generated or physician generated
        /// System alerts SHOULD be only changed/deleted by PDManager administrator
        /// </summary>

        public bool IsSystem { get; set; }

        /// <summary>
        /// High Priority value
        /// High Priority value corresponds to high priority alerts
        /// If this rule is fired then a notification should be given to the corresponding physician
        /// If the TargetValue is numeric then this value should be a double number with 0.XX format
        /// If this value is string then it can be comma (;) separated to allow multiple values
        /// <example>
        /// moderate;severe
        /// </example>
        /// If this value is null then this is ignored
        /// 
        /// </summary>
        [StringLength(100)]
        public string HighPriorityValue { get; set; }

        /// <summary>
        /// Medium Priority value
        /// Medium Priority value corresponds to medium priority alerts
        /// If this rule is fired then a notification should be given to the corresponding physician WHEN he enters the mobile app
        /// If the TargetValue is numeric then this value should be a double number with 0.XX format
        /// If this value is null then this is ignored      
        /// If this value is string then it can be comma (;) separated to allow multiple values
        /// <example>
        /// moderate;severe
        /// </example>
        /// </summary>
        [StringLength(100)]
        public string MediumPriorityValue { get; set; }


        /// <summary>
        /// Low Priority value
        /// Low Priority value corresponds to low priority alerts
        /// If this rule is fired then this alert is visible only from the alerts tile of PDManger mobile app
        /// If the TargetValue is numeric then this value should be a double number with 0.XX format
        /// If this value is null then this is ignored
        /// Medium and High Priority are also visible in the patient's alert tile.
        /// If this value is string then it can be comma (;) separated to allow multiple values
        /// <example>
        /// moderate;severe
        /// </example>
        /// </summary>
        [StringLength(100)]
        public string LowPriorityValue { get; set; }

        /// <summary>
        /// Target value code
        /// </summary>
        [StringLength(10)]
        public string TargetValueCode { get; set; }

        /// <summary>
        /// Source of Target Value Possible values:
        /// 1) observation
        /// 2) clinical
        /// 3) metaobservation
        /// 4) dss
        /// </summary>
        [StringLength(50)]
        public string TargetValueSource { get; set; }

        /// <summary>
        /// True if target value is numeric otherwise is categorical
        /// </summary>
        public bool TargetValueNumeric { get; set; }

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
        /// <summary>
        /// Aggregation Period
        /// </summary>
        public int AggregationPeriodDays { get; set; }
    }
}
