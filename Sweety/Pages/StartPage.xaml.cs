using Microsoft.Win32;
using Sweety.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Sweety.Pages
{
    /// <summary>
    /// Interaction logic for ReadPage.xaml
    /// </summary>
    public partial class StartPage : UserControl
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void StartCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(PlainFilePathTextBox.Text) && !string.IsNullOrEmpty(CipherFilePathTextBox.Text);
        }

        private void StartCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                AppendMessage("=================");

                string plainFilePath = PlainFilePathTextBox.Text;
                string cipherFilePath = CipherFilePathTextBox.Text;
                ValueType valueType = ValueType.RSA;
                if (AESRadioButton.IsChecked.HasValue && AESRadioButton.IsChecked.Value)
                {
                    valueType = ValueType.AES;
                }

                SetStatus(Status.Processing, string.Empty);

                ThreadPool.QueueUserWorkItem(new WaitCallback(item =>
                {
                    var returnValue = Processor.Start();

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        SetStatus(Status.Ready, string.Empty);

                        AppendMessage(returnValue);
                    }));

                }), 21);
            }
            catch (Exception ex)
            {
                AppendMessage(ex.ToString());

            }
        }

        private void AppendMessage(string message)
        {
            OutputTextBox.AppendText(DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + ": ");
            OutputTextBox.AppendText(message);
            OutputTextBox.AppendText(Environment.NewLine);
        }

        private void SetStatus(Status status, string message)
        {
            StatusBar.StatusItem = new StatusItem { Status = status, Message = message };

            switch (status)
            {
                case Status.Ready:
                    this.IsEnabled = true;
                    break;
                case Status.Processing:
                    this.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 打开明文文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlainFilePathTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            bool? result = dialog.ShowDialog();
            if (result != null && result.Value)
            {
                PlainFilePathTextBox.Text = dialog.FileName;
            }
        }

        private void CipherFilePathTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "KS文件|*.ks";

            bool? result = dialog.ShowDialog();
            if (result != null && result.Value)
            {
                CipherFilePathTextBox.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// 生成测试数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<ProductEntity> productEntityList = new List<ProductEntity>();

            var names = new List<string> { "苹果", "香蕉", "橘子", "柿子", "桃", "荔枝", "龙眼", "柑桔", "李子", "樱桃", "葡萄", "菠萝", "青梅", "椰子", "番石榴", "草莓" };
            for (int i = 0; i < names.Count; i++)
            {
                var productEntity = new ProductEntity();

                productEntity.ProductNo = string.Format("P{0:D5}", i + 1);
                productEntity.Name = names[i];

                productEntityList.Add(productEntity);
            }

            //var xx = ExcelHelper.ReadFromExcel<OriginEntity>(@"F:\sample.xlsx");

            ExcelHelper.WriteToExcel(@"F:\sample2.xlsx", productEntityList);
        }
    }
}