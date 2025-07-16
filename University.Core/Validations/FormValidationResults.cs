using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Core.Validations
{
    public class FormValidationResults
    {
       
        public bool isValid { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
        
        public FormValidationResults(bool isValid, List<ValidationResult> results)
        {
            this.isValid = isValid;
            if(isValid & results != null)
            {
                Errors = new Dictionary<string, List<string>>();
            }
            foreach(var item in results)
            {
                var messages = item.ErrorMessage;
                foreach (var memberName in item.MemberNames)
                {
                    if (!Errors.ContainsKey(memberName))
                    {
                        Errors[memberName] = new List<string>();
                    }
                    Errors[memberName].Add(messages ?? string.Empty);
                }
            }
        }
    }
}
