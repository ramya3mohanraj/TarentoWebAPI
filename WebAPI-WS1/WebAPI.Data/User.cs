using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Data
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
