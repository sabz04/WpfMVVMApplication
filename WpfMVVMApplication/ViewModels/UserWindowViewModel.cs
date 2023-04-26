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
    public class UserWindowViewModel : BindableBase
    {
        private Employee _Emp;
        private ObservableCollection<Role> _RoleNames;
        private IEventAggregator _eventAg;
        private Role _Role;
        private IMessageBoxService _messageBoxService;
        private EmployeeRepository _employeeRep;
        public ICommand SelectionChanged { get; set; }
        public DelegateCommand SaveUserChanges { get; }
        public ObservableCollection<Role> RoleNames
        {
            get { return _RoleNames; }
            set { SetProperty(ref _RoleNames, value); }
        }
        public Role Role
        {
            get { return _Role; }
            set { SetProperty(ref _Role, value); }
        }
        public Employee Emp
        {
            get { return _Emp; }
            set { SetProperty(ref _Emp, value); } 
        }
        
        public UserWindowViewModel(IEventAggregator eventAggregator, IMessageBoxService messageBoxService)
        {
            this._eventAg = eventAggregator;
            this._messageBoxService = messageBoxService;
            this.SaveUserChanges = new DelegateCommand(SaveChanges);
            this.SelectionChanged = new DelegateCommand<Role>(UserRoleSelectionChanged); 
            this._employeeRep = EmployeeRepository.GetEmployeeInstance();
            this.RoleNames = RoleRepository.GetRoleInstance().GetAll();
            _eventAg.GetEvent<MessageSentEvent>().Subscribe(GetEmployeeGuid);
        }

        private void UserRoleSelectionChanged(Role role)
        {
            if (role == null)
                return;
            Role = role;
        }

        private void SaveChanges()
        {
            var isUpdated = _employeeRep.Update(new Employee
            {
                Id = Emp.Id,
                FirstName = this.Emp.FirstName,
                LastName = this.Emp.LastName,
                Salary = this.Emp.Salary,
                Position =  Role
            });
            if(isUpdated != null)
            {
                _messageBoxService.Show("Обновление успешно!");
                _eventAg.GetEvent<MessagEmpsSentEvent>().Publish(isUpdated);
                Emp = isUpdated;
            }
            else
            {
                _messageBoxService.Show("Неизвестная ошибка!");
            }
        }

        private void GetEmployeeGuid(Guid obj)
        {
            
            Emp = _employeeRep.GetById(obj);
            
            
        }
    }
}
