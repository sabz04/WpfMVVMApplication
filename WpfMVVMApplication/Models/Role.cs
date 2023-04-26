using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApplication.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ObservableCollection<Employee> Employees { get; set; }
        public Role()
        {
            Id = Guid.NewGuid();
            Employees = new ObservableCollection<Employee>();
        }
    }
}
