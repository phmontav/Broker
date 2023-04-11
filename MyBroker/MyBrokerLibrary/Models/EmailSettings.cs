using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBrokerLibrary.Models
{
    public class EmailSettings
    {
        public Smtp smtp { get; set; }
        public List<string> toEmail { get; set; }
    }
    public class Smtp
    {
        public string host { get; set; }
        public string port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string ssl { get; set; }
    }
}

