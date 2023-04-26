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
    public class DepartmentsPageViewModel : BindableBase
    {
        private Department _SelectedDepartment;
        public ICommand SelectionChanged { get; set; }
        public ICommand OpenCreateWindow { get; set; }
        public ICommand CheckChanged { get; set; }
        private ObservableCollection<Department> _Departments;
        public Department SelectedDepartment
        {
            get { return _SelectedDepartment; } 
            set { SetProperty(ref _SelectedDepartment, value); }
        }

        private ICreateDepartmentService _depServ;
        private IEventAggregator _eventAg;
        private IDepartmentWindowService _depWindow;

        public ObservableCollection<Department> Departments
        {
            get { return _Departments; }
            set { SetProperty(ref _Departments, value); }
        }
        public DepartmentsPageViewModel(IEventAggregator eventAggregator, IDepartmentWindowService departmentWindow, ICreateDepartmentService createDepartmentService)
        {
            this._depServ = createDepartmentService;
            this._eventAg = eventAggregator;
            this._depWindow  = departmentWindow;
            _eventAg.GetEvent<MessageDepsSentEvent>().Subscribe(ItemChanged);
            _eventAg.GetEvent<MessageDepCreatedSent>().Subscribe(ItemAdded);
            Departments = DepartmentRepository.GetDepartmentInstance().GetAll();
            SelectionChanged = new DelegateCommand(Selected);
            OpenCreateWindow = new DelegateCommand(OpenWindowCreate);
            
        }

       

        private void ItemAdded(Department obj)
        {
            if (obj == null)
            {
                return;
            }
            Departments.Add(obj);    
        }

        private void OpenWindowCreate()
        {
            this._depServ.Show();
        }

        private void ItemChanged(Department obj)
        {
            if(obj == null)
            {
                return;
            }
            var indexItem = Departments.ToList().FindIndex(x => x.Id == obj.Id);
            if (indexItem != -1)
            {
                Departments.RemoveAt(indexItem);
                Departments.Insert(indexItem, obj);
            }
        }

        private void Selected()
        {
            if(SelectedDepartment != null)
            {
                _depWindow.Show();
                this._eventAg.GetEvent<MessageDepsSentEvent>().Publish(SelectedDepartment);
            }
            
        }
    }
}
