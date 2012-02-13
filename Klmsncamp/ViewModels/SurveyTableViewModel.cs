using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klmsncamp.ViewModels
{
    public class SurveyTableViewModel
    {
        public int SurveyTableID { get; set; }

        public int RequestIssueID { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class SurveyTableDetailViewModel
    {
        public string SurveyNodeDescription { get; set; }

        public string SurveyRecordTypeDescription { get; set; }

        public int? Score { get; set; }

        public bool ApprovalStatus { get; set; }

        public string ApprovalStatusStr
        {
            get
            {
                if (this.ApprovalStatus)
                {
                    return "Evet";
                }
                else
                {
                    return "Hayır";
                }
            }
        }

        public string Note { get; set; }
    }
}