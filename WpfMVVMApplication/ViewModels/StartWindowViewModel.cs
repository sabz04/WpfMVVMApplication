using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfMVVMApplication.Services;
using WpfMVVMApplication.Views;

namespace WpfMVVMApplication.ViewModels
{
    public class StartWindowViewModel : BindableBase, ICLoseView
    {
        private Page _CurrentPage;
        public ICommand Register_Click { get; set; }
        public ICommand Settings_Click { get; set; }

        private Page RegisterPage, SettingPage;
        public Page CurrentPage
        {
            get { return _CurrentPage; }
            set { SetProperty(ref _CurrentPage, value); }
        }

        private IEventAggregator _eventAgr;

        public Action Close { get ;set; }

        public StartWindowViewModel(IEventAggregator eventAggregator) {

            RegisterPage = new RegisterPage();
            SettingPage = new SettingsPage();

            CurrentPage= RegisterPage;
            this._eventAgr = eventAggregator;
            Register_Click = new DelegateCommand(Register);
            Settings_Click = new DelegateCommand(Settings);
            _eventAgr.GetEvent<MessageSendStringEvent>().Subscribe(CloseWindow);
        }

        private void CloseWindow(string obj)
        {
            Close?.Invoke();
        }

        private void Settings()
        {
            CurrentPage = SettingPage;
        }

        private void Register()
        {
            CurrentPage = RegisterPage;
        }
    }
}
