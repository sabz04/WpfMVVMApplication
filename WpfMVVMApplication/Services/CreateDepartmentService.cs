using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMVVMApplication.Views;

namespace WpfMVVMApplication.Services
{
    public interface ICreateDepartmentService {
        void Show();
        
    }
    public class CreateDepartmentService : ICreateDepartmentService
    {
        public void Show()
        {
            DepartmentCreateWindow departmentCreateWindow= new DepartmentCreateWindow();
            departmentCreateWindow.Show();
        }
    }
}
