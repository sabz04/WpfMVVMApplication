using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMVVMApplication.Views;

namespace WpfMVVMApplication.Services
{
    public interface IMainWindowService
    {
        void Show();
        void Close();
    }
    internal class MainWindowService : IMainWindowService
    {
        MainWindow main;
        public void Close()
        {
            if(main != null) { main.Close(); }
        }

        public void Show()
        {
            main= new MainWindow();
            main.ShowDialog();
        }
    }
}
