using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfMVVMApplication.DataContext;
using WpfMVVMApplication.Services;
using WpfTestApplication.Models;

namespace WpfMVVMApplication.ViewModels
{
    public class DepartmentWindowViewModel : BindableBase
    {
        
        private Department _Department;
        private String _Offices;
        private bool _IsSelected;

        public ICommand SelectionChangedToRemove { get; set;  }

        private ObservableCollection<Employee> _choosedEmloyees;
        private ObservableCollection<Employee> _employees;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        public ObservableCollection<Employee> ChoosedEmployees
        {
            get { return _choosedEmloyees;}

            set
            {
                SetProperty(ref _choosedEmloyees, value);
            }
        }
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                SetProperty(ref _employees, value);

            }
        }

        private IMessageBoxService _msgBox;
        private IEventAggregator _eventAg;
        public ICommand SelectionChanged { get; set; }
        public ICommand CheckChanged { get; set; }
        public ICommand Save { get; set; }
        public String Offices
        {
            get { return _Offices; }
            set { SetProperty(ref _Offices, value); } 
        }
        public Department Department { get { return _Department; }
            set { SetProperty(ref _Department, value); }
        }
        public DepartmentWindowViewModel(IEventAggregator eventAggregator, IMessageBoxService messageBoxService)
        {
            this._msgBox = messageBoxService;
            this._eventAg = eventAggregator;
            ChoosedEmployees = new ObservableCollection<Employee>();
            SelectionChangedToRemove = new DelegateCommand<Employee>(Remove);
            
            Employees = EmployeeRepository.GetEmployeeInstance().GetAll();
            SelectionChanged = new DelegateCommand<Employee>(SelectedIndexChanged);
            Save = new DelegateCommand(SaveDep);
            _eventAg.GetEvent<MessageDepsSentEvent>().Subscribe(GetDepartment);
        }

        private void Remove(Employee obj)
        {
            if (ChoosedEmployees.Contains(obj))
            {
                obj.Department = null;
                ChoosedEmployees.Remove(obj);
                
            }
            
        }

        private void SelectedIndexChanged(Employee employee)
        {
            if (employee != null && !ChoosedEmployees.Contains(employee))
            {
                
                employee.Department = Department;
                ChoosedEmployees.Add(employee);
            }
            else
            {
                _msgBox.Show("Ошибка добавления: объект уже добавлен");
            }    
        }

        

        private void SaveDep()
        {
            if(Department== null) { return; }
            var departMent = DepartmentRepository.GetDepartmentInstance().GetById(Department.Id);
            if (departMent != null)
            {
                Department.Offices = DepartmentRepository.ConvertToObservableCollection(this.Offices);
                Department.Employees = ChoosedEmployees;
                foreach(var employee in ChoosedEmployees) {
                    
                    EmployeeRepository.GetEmployeeInstance().Update(employee);
                }
                var res = DepartmentRepository.GetDepartmentInstance().Update(Department);
                _eventAg.GetEvent<MessageDepsSentEvent>().Publish(res);
                Department = res;
            }
        }

        private void GetDepartment(Department obj)
        {
            if(obj == null) return;
            ChoosedEmployees.Clear();
            Offices = "";
            var dep = DepartmentRepository.GetDepartmentInstance().GetById(obj.Id);
            if (dep != null) {
                Department = dep;
                Department.Offices.ToList().ForEach(Office => { Offices += Office+","; });
                foreach (var employee in Employees)
                {
                    if(employee.Department!= null && employee.Department.Id == obj.Id)
                    {
                        ChoosedEmployees.Add(employee);
                    }
                   
                }
            }

        }
    }
}
