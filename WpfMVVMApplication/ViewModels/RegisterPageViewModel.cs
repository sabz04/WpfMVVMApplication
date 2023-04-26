using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using WpfMVVMApplication.DataContext;
using WpfMVVMApplication.Services;
using WpfMVVMApplication.Views;
using WpfTestApplication.Models;

namespace WpfMVVMApplication.ViewModels
{
    internal class RegisterPageViewModel : BindableBase
    {
        private String _FirstName;
        private String _LastName;
        private IUserWindowService _userWindowService;
        private IEventAggregator _eventAgr;
        private IMessageBoxService _messageBoxService;
        private IMainWindowService _mainWindowService;
         
        public DelegateCommand RegisterButtonClicked { get; }
        private EmployeeRepository employeeRepository;
        public String FirstName
        {
            get
            {

                return _FirstName;
            }
            set
            {
                SetProperty(ref _FirstName, value);
            }
        }

        public String LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                SetProperty(ref _LastName, value);
            }
        }

       

        public RegisterPageViewModel(IMessageBoxService messageBoxService, IMainWindowService mainWindowService, IEventAggregator eventAggregator)
        {
            
            this._eventAgr = eventAggregator;
            _messageBoxService = messageBoxService;
            _mainWindowService = mainWindowService;

            RegisterButtonClicked = new DelegateCommand(this.ButtonClicked);
            
        }
        bool InitDatabase()
        {
            try
            {
                JsonDataContext.GetInstance().InitJsonDatabase();
                return true;
            }
            catch (Exception ex)
            {
                _messageBoxService.Show("Неверная конфигурация. Смените конфигурационные настройки");
                return false;
            }
        }
        void ButtonClicked()
        {
            if(!InitDatabase())
            {
                return;
            }
            //JsonDataContext.GetInstance().InitJsonDatabase();
            if (LastName != null || FirstName != null)
            {
                var emp = EmployeeRepository.GetEmployeeInstance().Create(new Employee
                {
                    FirstName = FirstName,
                    LastName = LastName
                });
                if (emp != null)
                {
                    
                    _messageBoxService.Show("Регистрация успешна!");
                    _eventAgr.GetEvent<MessageSendStringEvent>().Publish("Success");
                    _mainWindowService.Show();
                    
                }
                else
                {
                    
                    _messageBoxService.Show("Ошибка регистрации!");
                }
            }
        }
    }
    interface ICLoseView
    {
        Action Close { get; set; }
    }
    }
