using DrumTrainer.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrumTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IView
    {
        public MainWindow()
        {
            this.DataContext = new MainViewModel(this);
            InitializeComponent();
        }


        public async Task ShowChildWindowAsync(ChildWindow childWindow)
        {
            await ChildWindowManager.ShowChildWindowAsync(this, childWindow).ConfigureAwait(true);
        }

        public async Task<T> ShowChildWindowAsync<T>(ChildWindow childWindow)
        {
            return await ChildWindowManager.ShowChildWindowAsync<T>(this, childWindow).ConfigureAwait(true);
        }
    }
}
