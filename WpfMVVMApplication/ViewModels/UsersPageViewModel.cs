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
using WpfMVVMApplication.Services;
using WpfTestApplication.Models;

namespace WpfMVVMApplication.ViewModels
{
    public class UsersPageViewModel : BindableBase
    {
        private EmployeeRepository employeeRepository;
        private IEventAggregator _ed;
        private IUserWindowService _userWindowService;
        public ICommand RefreshListCommand { get; set; }

        public ICommand SelectionChanged { get; set; }
        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                SetProperty(ref _employees, value);

            }
        }
        private EmployeeRepository employeeRep { get; set; }

        
        public UsersPageViewModel(IUserWindowService userWindowService, IEventAggregator ea)
        {
            this._ed = ea;
            this._userWindowService = userWindowService;
            _ed.GetEvent<MessagEmpsSentEvent>().Subscribe(SetEmpsData);
            employeeRepository = EmployeeRepository.GetEmployeeInstance();
            RefreshListCommand = new DelegateCommand(Refresh);
            SelectionChanged = new DelegateCommand<Employee>(this.SelectedObjChanged);
            Employees = employeeRepository.GetAll();
        }

        private void Refresh()
        {
            Employees.Clear();
            Employees.AddRange(employeeRepository.GetAll());

        }

        private void SetEmpsData(Employee obj)
        {
            var indexItem = Employees.ToList().FindIndex(x => x.Id == obj.Id);
            if (indexItem != -1)
            {

                Employees.RemoveAt(indexItem);
                Employees.Insert(indexItem, obj);


            }
        }

        void SelectedObjChanged(Employee employee)
        {
            if (employee == null)
            {
                return;
            }
            _userWindowService.Create();
            _ed.GetEvent<MessageSentEvent>().Publish(employee.Id);
            _userWindowService.Show();

        }
    }
}
