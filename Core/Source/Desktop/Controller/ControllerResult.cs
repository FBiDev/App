using System.Collections.Generic;
using System.Linq;

namespace App.Core.Desktop
{
    public class ControllerResult
    {
        private string _successMessage;

        public ControllerResult()
        {
            SuccessMessage = ErrorMessage = string.Empty;
        }

        public bool Success { get; set; }

        public string SuccessTitle { get; set; }

        public string SuccessMessage
        {
            get
            {
                return _successMessage;
            }

            set
            {
                _successMessage = value;
                Success = string.IsNullOrWhiteSpace(value) == false;
            }
        }

        public string ErrorMessage { get; set; }

        public dynamic Object { get; set; }

        public List<dynamic> List { get; set; }

        public List<T> GetList<T>()
        {
            return List.Cast<T>().ToList();
        }

        public ListBind<T> GetBindList<T>()
        {
            return new ListBind<T>(List.Cast<T>().ToList());
        }

        public void SetList<T>(List<T> list)
        {
            List = list.Cast<dynamic>().ToList();
        }
    }
}