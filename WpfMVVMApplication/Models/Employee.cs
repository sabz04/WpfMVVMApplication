using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApplication.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Role Position { get; set; }
        public double Salary { get; set; }
        public Department Department { get; set; }

        [JsonProperty("RoleId")]
        public Guid RoleId { get; set; }
        
        public Employee() {
            Id = Guid.NewGuid();
            
        }
    }
}
