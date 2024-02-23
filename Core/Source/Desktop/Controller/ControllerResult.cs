using System.Collections.Generic;
using System.Linq;

namespace App.Core.Desktop
{
    public class ControllerResult
    {
        public bool Sucess { get; set; }
        public string SucessTitle { get; set; }

        string sucMessage;
        public string SucessMessage
        {
            get
            {
                return sucMessage;
            }
            set
            {
                sucMessage = value;
                Sucess = string.IsNullOrWhiteSpace(value) == false;
            }
        }

        public string ErrorMessage { get; set; }
        public dynamic Object { get; set; }
        public List<dynamic> List { get; set; }

        public ControllerResult()
        {
            SucessMessage = ErrorMessage = string.Empty;
        }

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