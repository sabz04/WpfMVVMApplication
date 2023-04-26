using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using WpfMVVMApplication.Services;
using System.Windows.Controls;
using WpfMVVMApplication.Views;
using WpfTestApplication.Models;

namespace WpfMVVMApplication.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ICommand UsersButtonClick { get; set; }
        public ICommand DepsButtonClick { get; set; }

        private IEventAggregator _eventAg;
        private System.Windows.Controls.Page UsersPage;
        private System.Windows.Controls.Page DepsPage;

       

        private System.Windows.Controls.Page _CurrentPage;
        public System.Windows.Controls.Page CurrentPage
        {
            get { return _CurrentPage; }
            set
            {
                SetProperty(ref _CurrentPage, value);
            }
        }
        public MainWindowViewModel(IEventAggregator eventAggregator) {
            this._eventAg = eventAggregator;
            UsersPage = new UsersPage();
            DepsPage = new DepartmentsPage();
            

            CurrentPage = UsersPage;

            UsersButtonClick = new DelegateCommand(NavigateUsers);
            DepsButtonClick = new DelegateCommand(NavigateDeps);
        }

        private void NavigateDeps()
        {
            CurrentPage = DepsPage;
        }

        private void NavigateUsers()
        {
            CurrentPage = UsersPage;
        }
    }
}
