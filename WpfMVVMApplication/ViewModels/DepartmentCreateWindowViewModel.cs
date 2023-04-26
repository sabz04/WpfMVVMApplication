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
    public class DepartmentCreateWindowViewModel : BindableBase
    {
        private String _DepName; 
        private String _Offices;

        private IEventAggregator _eventAg;
        public ICommand Save { get; set; }
        public String Offices
        {
            get { return _Offices; }
            set { SetProperty(ref _Offices, value); }
        }
        public String DepName
        {
            get { return _DepName; }
            set { SetProperty(ref _DepName, value); }
        }
        public DepartmentCreateWindowViewModel(IEventAggregator eventAggregator)
        {
            this._eventAg = eventAggregator;
            Save = new DelegateCommand(SaveDep);
            
        }
        
        private void SaveDep()
        {
            Department dep = new Department()
            {
                Name = DepName,
                Offices = DepartmentRepository.ConvertToObservableCollection(this.Offices)
            };
            var created = DepartmentRepository.GetDepartmentInstance().Create(dep);
            _eventAg.GetEvent<MessageDepCreatedSent>().Publish(created);
        }
    }
}
