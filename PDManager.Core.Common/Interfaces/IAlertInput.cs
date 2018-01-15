using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Interfaces
{
    /// <summary>
    /// Alert Evaluation required input
    /// </summary>
    public interface IAlertInput
    {
        /// <summary>
        /// Alert Name
        /// </summary>
         string Name { get; }
        /// <summary>
        /// Alert Message
        /// </summary>
         string Message { get; }
        
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
      
         string HighPriorityValue { get;  }

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
      
        string MediumPriorityValue { get; }


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
   
        string LowPriorityValue { get; }

        /// <summary>
        /// Target value code
        /// </summary>
      
        string TargetValueCode { get; }

        /// <summary>
        /// Source of Target Value Possible values:
        /// 1) observation
        /// 2) clinical
        /// 3) metaobservation
        /// 4) dss
        /// </summary>
      
         string TargetValueSource { get;  }

        /// <summary>
        /// True if target value is numeric otherwise is categorical
        /// </summary>
         bool TargetValueNumeric { get;}
        /// <summary>
        /// 
        /// </summary>
        int AggregationPeriodDays { get; set; }
    }
}
