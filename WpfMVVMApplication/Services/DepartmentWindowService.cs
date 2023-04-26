using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMVVMApplication.Views;

namespace WpfMVVMApplication.Services
{
    public interface IDepartmentWindowService
    {
        void Create();
        void Show();
    }
    public class DepartmentWindowService : IDepartmentWindowService
    {
        DepartmentWindow departmentWindow;
        public void Create()
        {
            departmentWindow= new DepartmentWindow();
        }

        public void Show()
        {
            Create();
            departmentWindow.Show();
        }
    }
}
