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

namespace Laba_2
{
    /// <summary>
    /// Interaction logic for Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        public Info(string data)
        {
            InitializeComponent();
            dataText.Text = data;
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
                mainWindow.Closed += (s, e) => Close();
        }
    }
}
