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
    /// Interaction logic for Changes.xaml
    /// </summary>
    public partial class Changes : Window
    {
        public Changes()
        {
            InitializeComponent();
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
                mainWindow.Closed += (s, e) => Close();
        }

        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var col = e.Column as DataGridTextColumn;
            var style = new Style(typeof(TextBlock));
            switch (e.Column.Header.ToString())
            {
                case "Id":
                    col.Width = 30;
                    break;
                case "Name":
                    style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
                    col.MinWidth = 70;
                    col.MaxWidth = 120;
                    col.ElementStyle = style;
                    break;
                case "Description":
                    style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
                    col.MinWidth = 200;
                    col.ElementStyle = style;
                    break;
                case "Source":
                    style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
                    col.MinWidth = 70;
                    col.MaxWidth = 120;
                    col.ElementStyle = style;
                    break;
                case "Obj":
                    style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
                    col.MinWidth = 70;
                    col.MaxWidth = 120;
                    col.Header = "Object";
                    col.ElementStyle = style;
                    break;
                case "Conf":
                    style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                    col.ElementStyle = style;
                    col.Width = 45;
                    break;
                case "Integr":
                    style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                    col.ElementStyle = style;
                    col.Width = 45;
                    break;
                case "Avail":
                    style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                    col.ElementStyle = style;
                    col.Width = 45;
                    break;
                default:
                    break;
            }
        }
    }
}
