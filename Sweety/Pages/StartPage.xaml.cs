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
                        List<OriginEntity> originEntityList = ExcelHelper.ReadFromExcel<OriginEntity>(inputFilePath);

                        List<TargetEntity> targetEntityList = Process(originEntityList);

                        ExcelHelper.WriteToExcel(outputFilePath, targetEntityList);

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

        private List<TargetEntity> Process(List<OriginEntity> originEntityList)
        {
            List<TargetEntity> returnValue = new List<TargetEntity>();

            var groups = originEntityList.GroupBy(v => v.BusinessNo).ToList();

            foreach (var group in groups)
            {
                StringBuilder remarkBuilder = new StringBuilder();
                if (group.Count() == 1)
                {
                    OriginEntity originEntity = group.First();
                    remarkBuilder.Append("仅此一单。");

                    TargetEntity targetEntity = ToTargetEntity(originEntity);

                    if (originEntity.SellCount == originEntity.BuyCount)
                    {
                        targetEntity.SaleMode = "直运";
                        remarkBuilder.Append("进货数等于销货数。");
                    }
                    else if (originEntity.SellCount < originEntity.BuyCount)
                    {
                        targetEntity.SaleMode = "滞销";
                        remarkBuilder.Append("进货数大于销货数。");
                    }
                    else
                    {
                        targetEntity.SaleMode = "超卖";
                        remarkBuilder.Append("进货数小于销货数。");
                    }

                    targetEntity.Remark = remarkBuilder.ToString();

                    returnValue.Add(targetEntity);
                }
                else
                {
                    remarkBuilder.Append("交易已被拆单。");

                    int totalBuyCount = group.Sum(v => v.BuyCount);
                    int totalSellCount = group.Sum(v => v.SellCount);

                    string saleMode = string.Empty;

                    if (totalSellCount == totalBuyCount)
                    {
                        saleMode = "整体直运";
                        remarkBuilder.Append("整体进货数等于销货数。");
                    }
                    else if (totalSellCount < totalBuyCount)
                    {
                        saleMode = "整体滞销";
                        remarkBuilder.Append("整体进货数大于销货数。");
                    }
                    else
                    {
                        saleMode = "整体超卖";
                        remarkBuilder.Append("整体进货数小于销货数。");
                    }

                    foreach (var originEntity in group)
                    {
                        StringBuilder singleRemarkBuilder = new StringBuilder();

                        TargetEntity targetEntity = ToTargetEntity(originEntity);

                        if (originEntity.SellCount == originEntity.BuyCount)
                        {
                            targetEntity.SaleMode = saleMode;
                            singleRemarkBuilder.Append("本单进货数等于销货数。");
                        }
                        else if (originEntity.SellCount < originEntity.BuyCount)
                        {
                            targetEntity.SaleMode = saleMode;
                            singleRemarkBuilder.Append("进货数大于销货数。");
                        }
                        else
                        {
                            targetEntity.SaleMode = saleMode;
                            singleRemarkBuilder.Append("进货数小于销货数。");
                        }

                        targetEntity.Remark = remarkBuilder.ToString() + singleRemarkBuilder.ToString();

                        returnValue.Add(targetEntity);
                    }
                }
            }

            return returnValue;
        }

        private static TargetEntity ToTargetEntity(OriginEntity originEntity)
        {
            TargetEntity targetEntity = new TargetEntity();

            targetEntity.BusinessNo = originEntity.BusinessNo;
            targetEntity.PaperNo = originEntity.PaperNo;
            targetEntity.ProductNo = originEntity.ProductNo;
            targetEntity.BuyCount = originEntity.BuyCount;
            targetEntity.SellCount = originEntity.SellCount;

            return targetEntity;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string fileName = @"F:\Input2017113001.xlsx";

            //商品表
            List<ProductEntity> productEntityList = GenerateProductEntityList();

            //交易
            List<BusinessEntity> businessEntityList = GenerateBusinessEntityList();

            //原始数据表
            List<SourceEnity> sourceEntityList = GenerateSourceEntityList(businessEntityList, productEntityList);

            //数据源表
            ExcelHelper.WriteToExcel(fileName, sourceEntityList);
            ExcelHelper.WriteToExcel(fileName, productEntityList);
            ExcelHelper.WriteToExcel(fileName, businessEntityList);

        }

        // 生产基础数据表
        // 2017年，每天5-10单
        private static List<SourceEnity> GenerateSourceEntityList(List<BusinessEntity> businessNoList, List<ProductEntity> productEntityList)
        {
            List<SourceEnity> returnValue = new List<SourceEnity>();

            Random random = new Random();
            DateTime today = DateTime.Now.Date;

            int businessNo = 1;

            for (int i = -300; i < 0; i++)
            {
                //指定日期
                var day = today.AddDays(i);

                //每天5-10单
                var sourceCount = random.Next(5, 10);
                for (int sourceIndex = 0; sourceIndex < sourceCount; sourceIndex++)
                {
                    SourceEnity sourceEntity = new SourceEnity();

                    //随机取交易号
                    sourceEntity.BusinessNo = businessNoList[random.Next(businessNoList.Count)].BusinessNo;

                    //生成单据号
                    sourceEntity.PaperNo = string.Format("PN{0:D6}", businessNo++);

                    //随机取产品号
                    sourceEntity.ProductNo = productEntityList[random.Next(productEntityList.Count)].ProductNo;

                    //模式
                    sourceEntity.Mode = random.Next() % 2 == 0 ? "销售" : "采购";

                    //数量
                    sourceEntity.Count = random.Next(100, 300);

                    //日期
                    sourceEntity.CreateTime = day;

                    returnValue.Add(sourceEntity);
                }
            }

            return returnValue;
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
                    originEntity.ProductNo = business.ProductNo;
                    originEntity.BuyCount = paper.BuyCount;
                    originEntity.SellCount = paper.SellCount;

                    returnValue.Add(originEntity);
                }
            }

            return returnValue;
        }

        private static List<ProductEntity> GenerateProductEntityList()
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

        private static List<BusinessEntity> GenerateBusinessEntityList()
        {
            List<BusinessEntity> returnValue = new List<BusinessEntity>();

            for (int i = 1; i < 100; i++)
            {
                BusinessEntity entity = new BusinessEntity();

                entity.BusinessNo = string.Format("BN{0:D5}", i + 1);
                entity.BusinessName = string.Format("交易名称{0:D5}", i + 1);

                returnValue.Add(entity);
            }

            return returnValue;
        }
    }
}