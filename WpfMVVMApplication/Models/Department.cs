using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApplication.Models
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<string> Offices { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        public Department()
        {
            Id = Guid.NewGuid();
            Offices = new ObservableCollection<string>();
            Employees = new ObservableCollection<Employee>();   
        }
    }
}
