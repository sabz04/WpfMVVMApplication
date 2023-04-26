using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfMVVMApplication.ViewModels;

namespace WpfMVVMApplication.Views
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            Loaded += StartWindow_Loaded;
        }

        private void StartWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICLoseView vm)
            {
                vm.Close += () =>
                {
                    this.Hide();
                };
            }
        }
    }
}
