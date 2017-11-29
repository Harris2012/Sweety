using Microsoft.Win32;
using Sweety.Entity;
using Sweety.Model;
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
            List<ProductEntity> productEntityList = GetProductEntityList();

            List<Business> businessList = GetBusinessList(productEntityList);

            List<OriginEntity> originEntityList = ToOriginEntityList(businessList);

            //var xx = ExcelHelper.ReadFromExcel<OriginEntity>(@"F:\sample.xlsx");

            ExcelHelper.WriteToExcel(@"F:\sample2.xlsx", productEntityList);
            ExcelHelper.WriteToExcel(@"F:\sample2.xlsx", originEntityList);
        }

        private static List<Business> GetBusinessList(List<ProductEntity> productEntityList)
        {
            List<Business> businessList = new List<Business>();

            Random random = new Random();

            //一共100个交易号
            int businessCount = 100;

            for (int businessIndex = 0; businessIndex < businessCount; businessIndex++)
            {
                Business business = new Business();
                business.BusinessNo = string.Format("BN{0:D6}", businessIndex + 1);

                //每个交易号随机拆成1到3个单据号
                int paperCount = random.Next(1, 4);

                business.PaperList = new List<Paper>();
                for (int paperIndex = 0; paperIndex < paperCount; paperIndex++)
                {
                    Paper paper = new Paper();
                    paper.PaperNo = string.Format("PN{0:D6}", businessIndex * 98 + paperIndex);
                    business.PaperList.Add(paper);

                    //每张单据里面放一个产品
                    paper.ProductNo = productEntityList[random.Next(productEntityList.Count)].ProductNo;

                    if (paperCount == 1)
                    {
                        paper.BuyCount = random.Next(200, 300);
                        paper.SellCount = paper.BuyCount;
                    }
                    else if (paperIndex == paperCount - 1)
                    {
                        int totalBuyCount = business.PaperList.Sum(v => v.BuyCount);
                        int totalSellCount = business.PaperList.Sum(v => v.SellCount);
                        if (totalBuyCount == totalSellCount)
                        {
                            paper.BuyCount = random.Next(200, 300);
                            paper.SellCount = paper.BuyCount;
                        }
                        else if (totalBuyCount < totalSellCount)
                        {
                            paper.BuyCount = totalSellCount - totalBuyCount;
                        }
                        else
                        {
                            paper.SellCount = totalBuyCount - totalSellCount;
                        }
                    }
                    else
                    {
                        //随机填充采购数和销售数
                        switch (random.Next() % 3)
                        {
                            case 0:
                                {
                                    paper.BuyCount = random.Next(200, 300);
                                    paper.SellCount = paper.BuyCount;
                                }
                                break;
                            case 1:
                                {
                                    paper.BuyCount = random.Next(150, 250);
                                    paper.SellCount = random.Next(250, 350);
                                }
                                break;
                            case 2:
                                {
                                    paper.SellCount = random.Next(150, 250);
                                    paper.BuyCount = random.Next(250, 350);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                businessList.Add(business);
            }

            return businessList;
        }

        private static List<OriginEntity> ToOriginEntityList(List<Business> businessList)
        {
            List<OriginEntity> returnValue = new List<OriginEntity>();

            foreach (var business in businessList)
            {
                foreach (var paper in business.PaperList)
                {
                    OriginEntity originEntity = new OriginEntity();

                    originEntity.BusinessNo = business.BusinessNo;
                    originEntity.PaperNo = paper.PaperNo;
                    originEntity.ProductNo = paper.ProductNo;
                    originEntity.BuyCount = paper.BuyCount;
                    originEntity.SellCount = paper.SellCount;

                    returnValue.Add(originEntity);
                }
            }

            return returnValue;
        }

        private static List<ProductEntity> GetProductEntityList()
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

            return productEntityList;
        }

    }
}