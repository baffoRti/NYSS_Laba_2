using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
using ClosedXML.Excel;

namespace Laba_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<DataUnit> dataList = new List<DataUnit>();
        public static Paging bigGridPages;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDataList();
            bigGridPages = new Paging(dataList);
            currentPageText.Text = $"Current page: {1}";
            bigGrid.ItemsSource = bigGridPages.pages[0];
            
        }

        public static void InitializeDataList()
        {
            if (!File.Exists(@"..\..\thrlist.xlsx"))
            {
                MessageBoxResult res = MessageBox.Show("Локальной базы данных не существует.\nХотите скачать?", "Attention!!!!!!!!!!!!", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (res)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            DownloadFile(@"..\..\thrlist.xlsx");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Произошла ошибка. Попробуйте проверить доступ к интернету и все такое. Возможно, серв упал. Короче, попробуйте позже.\nСообщение ошибки: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            Environment.Exit(0);
                        }
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Ну и ладно(\nНу и пожалуйста((", "(((", MessageBoxButton.OK);
                        Environment.Exit(0);
                        break;
                }
            }
            using (XLWorkbook book = new XLWorkbook(@"..\..\thrlist.xlsx"))
            {
                var ws = book.Worksheet(1);
                int i = 3;
                while (ws.Cell($"A{i}").GetValue<string>() != "")
                {
                    dataList.Add(new DataUnit
                    (
                        ws.Cell($"A{i}").GetValue<int>(),
                        ws.Cell($"B{i}").GetValue<string>(),
                        ws.Cell($"C{i}").GetValue<string>(),
                        ws.Cell($"D{i}").GetValue<string>(),
                        ws.Cell($"E{i}").GetValue<string>(),
                        ws.Cell($"F{i}").GetValue<string>(),
                        ws.Cell($"G{i}").GetValue<string>(),
                        ws.Cell($"H{i}").GetValue<string>(),
                        ws.Cell($"J{i}").GetValue<DateTime>()
                    ));
                    i++;
                }
            }
        }

        public static void DownloadFile(string path) 
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", path);
            }
        }

        private void bigGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var col = e.Column as DataGridTextColumn;
            var style = new Style(typeof(TextBlock));
            switch (e.Column.Header.ToString())
            {
                case "FormatId":
                    col.Header = "Id";
                    col.Width = 70;
                    break;
                case "Name":
                    style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
                    col.ElementStyle = style;
                    break;
                default:
                    col.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            bigGrid.ItemsSource = bigGridPages.ToPreviousPage();
            currentPageText.Text = $"Current page: {bigGridPages.CurrentPage + 1}";
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            bigGrid.ItemsSource = bigGridPages.ToNextPage();
            currentPageText.Text = $"Current page: {bigGridPages.CurrentPage + 1}";
        }

        private void ButtonToFirst_Click(object sender, RoutedEventArgs e)
        {
            bigGrid.ItemsSource = bigGridPages.ToFirstPage();
            currentPageText.Text = $"Current page: {bigGridPages.CurrentPage + 1}";
        }

        private void ButtonToLast_Click(object sender, RoutedEventArgs e)
        {
            bigGrid.ItemsSource = bigGridPages.ToLastPage();
            currentPageText.Text = $"Current page: {bigGridPages.CurrentPage + 1}";
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DownloadFile(@"..\..\tmp.xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка. Попробуйте проверить доступ к интернету и все такое. Возможно, серв упал. Короче, попробуйте позже.\nСообщение ошибки: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (HashCompare(@"..\..\thrlist.xlsx", @"..\..\tmp.xlsx"))
            {
                MessageBox.Show("Изменений обнаружено не было.", "Refresh");
                File.Delete(@"..\..\tmp.xlsx");
                return;
            }
            else
            {
                List<DataUnit> changesBefore = new List<DataUnit>();
                List<DataUnit> changesAfter = new List<DataUnit>();
                using (XLWorkbook book = new XLWorkbook(@"..\..\tmp.xlsx"))
                {
                    var ws = book.Worksheet(1);
                    int i = 3;
                    while (ws.Cell($"A{i}").GetValue<string>() != "")
                    {
                        if (ws.Cell($"A{i}").GetValue<int>() > dataList.Last().Id)
                        {
                            DataUnit tmp = new DataUnit
                            (
                                ws.Cell($"A{i}").GetValue<int>(),
                                ws.Cell($"B{i}").GetValue<string>(),
                                ws.Cell($"C{i}").GetValue<string>(),
                                ws.Cell($"D{i}").GetValue<string>(),
                                ws.Cell($"E{i}").GetValue<string>(),
                                ws.Cell($"F{i}").GetValue<string>(),
                                ws.Cell($"G{i}").GetValue<string>(),
                                ws.Cell($"H{i}").GetValue<string>(),
                                ws.Cell($"J{i}").GetValue<DateTime>()
                            );
                            dataList.Add(tmp);
                            changesAfter.Add(tmp);
                        }
                        else if (ws.Cell($"J{i}").GetValue<DateTime>() != dataList[i-3].dateOfChange)
                        {
                            DataUnit tmp = new DataUnit
                            (
                                ws.Cell($"A{i}").GetValue<int>(),
                                ws.Cell($"B{i}").GetValue<string>(),
                                ws.Cell($"C{i}").GetValue<string>(),
                                ws.Cell($"D{i}").GetValue<string>(),
                                ws.Cell($"E{i}").GetValue<string>(),
                                ws.Cell($"F{i}").GetValue<string>(),
                                ws.Cell($"G{i}").GetValue<string>(),
                                ws.Cell($"H{i}").GetValue<string>(),
                                ws.Cell($"J{i}").GetValue<DateTime>()
                            );
                            changesBefore.Add(dataList[i-3]);
                            changesAfter.Add(tmp);
                            dataList[i - 3] = tmp;
                        }
                        i++;
                    }
                }
                MessageBox.Show($"Обновление прошло успешно!\nОбновлено записей: {changesBefore.Count}\nДобавлено записей: {changesAfter.Count - changesBefore.Count}", "Successful");
                Changes win = new Changes();
                win.beforeGrid.ItemsSource = changesBefore;
                win.afterGrid.ItemsSource = changesAfter;
                win.Show();
                bigGridPages = new Paging(dataList);
                bigGrid.ItemsSource = bigGridPages.pages[0];
                currentPageText.Text = $"Current page: {bigGridPages.CurrentPage + 1}";
                File.Delete(@"..\..\thrlist.xlsx");
                File.Copy(@"..\..\tmp.xlsx", @"..\..\thrlist.xlsx");
                File.Delete(@"..\..\tmp.xlsx");
            }
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (bigGrid.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать угрозу из списка!", "Show");
                return;
            }
            Info inf = new Info(bigGrid.SelectedItem.ToString());
            inf.Show();
        }

        private bool HashCompare(string path1, string path2)
        {
            byte[] oldFile;
            byte[] newFile;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path1))
                {
                    oldFile = md5.ComputeHash(stream);
                }
            }
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path2))
                {
                    newFile = md5.ComputeHash(stream);
                }
            }
            for (int i = 0; i < oldFile.Length; i++)
            {
                if (oldFile[i] != newFile[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
