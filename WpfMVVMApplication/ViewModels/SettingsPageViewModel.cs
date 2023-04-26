using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Input;
using WpfMVVMApplication.DataContext;
using WpfMVVMApplication.Services;

namespace WpfMVVMApplication.ViewModels
{
    public class SettingsPageViewModel : BindableBase
    {
        private String _rolesDest;
        private String _depsDest;
        private String _empsDest;
        private JsonDataContext jsonDataContext;
        private IMessageBoxService _msgBoxService;
        private List<String> _configList;
        public ICommand Accept_Clicked { get; set; }
        public String RolesDestination
        {
            get { return _rolesDest; }
            set { SetProperty(ref _rolesDest, value); }
        }
        public String DepDest
        {
            get { return _depsDest; }
            set { SetProperty(ref _depsDest, value); }
        }
        public String EmployeesDestination
        {
            get { return _empsDest; }
            set { SetProperty(ref _empsDest, value); }
        }
        public SettingsPageViewModel(IMessageBoxService messageBoxService)
        {
            
            _configList = JsonDataContext.GetInstance().DeserializeConfig();

            if(_configList != null)
            {
                RolesDestination = _configList[1];
                DepDest = _configList[2];
                EmployeesDestination = _configList[0];
            }
            else
            {
                RolesDestination = "D:\\roles.json";
                DepDest = "D:\\departments.json";
                EmployeesDestination = "D:\\employees.json";
            }    
            
            
            
            this._msgBoxService = messageBoxService;
            
            
            Accept_Clicked = new DelegateCommand(AcceptChanges);
        }

        private void AcceptChanges()
        {
            JsonDataContext.GetInstance().EMPLOYEES_LINK = EmployeesDestination;
            JsonDataContext.GetInstance().DEPARTMENTS_LINK = DepDest;
            JsonDataContext.GetInstance().ROLES_LINK= RolesDestination;
            JsonDataContext.GetInstance().SerializeConfig();
            JsonDataContext.GetInstance().InitJsonDatabase();
            _msgBoxService.Show("Конфигурационные данные сохранены");
        }
    }
}
