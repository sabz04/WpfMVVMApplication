using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Prism.DryIoc;
using Prism;
using Prism.Ioc;
using System.Threading.Tasks;
using System.Windows;
using WpfMVVMApplication.Views;
using WpfMVVMApplication.Services;
using Prism.Navigation;
using Prism.Modularity;
using WpfMVVMApplication.Module;

namespace WpfMVVMApplication
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var createShell = Container.Resolve<StartWindow>();
            return createShell;
        }
        
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            
            
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            containerRegistry.RegisterSingleton<IMessageBoxService, MessageBoxService>();
            containerRegistry.RegisterSingleton<IMainWindowService, MainWindowService>();
            containerRegistry.RegisterSingleton<IUserWindowService, UserWindowService>();
            containerRegistry.RegisterSingleton<IDepartmentWindowService, DepartmentWindowService>();
            containerRegistry.RegisterSingleton<ICreateDepartmentService, CreateDepartmentService>();
        }
    }
}
