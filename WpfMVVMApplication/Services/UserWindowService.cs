using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMVVMApplication.Views;

namespace WpfMVVMApplication.Services
{
    public interface IUserWindowService
    {
        void Create();
        void Show();
    }
    public class UserWindowService : IUserWindowService
    {
        UserWindow userV;
        public void Create()
        {
            userV = new UserWindow();
        }

        public void Show()
        {
            userV.Show();
        }
    }
}
