using Microsoft.Win32;
using Sweety.InputEntity;
using Sweety.Model;
using Sweety.OutputEntity;
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
            e.CanExecute = !string.IsNullOrEmpty(InputFilePathTextBox.Text) && !string.IsNullOrEmpty(OutputFilePathTextBox.Text);
        }

        private void StartCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                AppendMessage("=================");

                string inputFilePath = InputFilePathTextBox.Text;
                string outputFilePath = OutputFilePathTextBox.Text;
                ValueType valueType = ValueType.RSA;
                if (AESRadioButton.IsChecked.HasValue && AESRadioButton.IsChecked.Value)
                {
                    valueType = ValueType.AES;
                }

                SetStatus(Status.Processing, string.Empty);

                ThreadPool.QueueUserWorkItem(new WaitCallback(item =>
                {
                    string message = string.Empty;

                    try
                    {
                        //var business = ExcelReader.ReadEntityList<BusinessEntity>(inputFilePath, "商务报表");

                        var buy = ExcelReader.ReadEntityList<BuyEntity>(inputFilePath, "本期进项明细");

                        var sell = ExcelReader.ReadEntityList<SellEntity>(inputFilePath, "本期销项明细");
                        //Processor.Run(inputFilePath, outputFilePath);

                        var xx = 0;

                        message = "Success";
                    }
                    catch (Exception ex)
                    {
                        message = "程序出错了" + Environment.NewLine + ex.ToString();
                    }

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        SetStatus(Status.Ready, string.Empty);

                        AppendMessage(message);
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

        private void InputFilePathTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Excel文件|*.xlsx";

            bool? result = dialog.ShowDialog();
            if (result != null && result.Value)
            {
                InputFilePathTextBox.Text = dialog.FileName;
            }
        }

        private void OutputFilePathTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "Excel文件|*.xlsx";

            bool? result = dialog.ShowDialog();
            if (result != null && result.Value)
            {
                OutputFilePathTextBox.Text = dialog.FileName;
            }
        }
    }
}