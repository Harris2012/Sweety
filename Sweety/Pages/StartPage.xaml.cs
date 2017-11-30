using Microsoft.Win32;
using Sweety.Entity;
using Sweety.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
                        AppendMessage("开始读取excel文件.");
                        Stopwatch watch = Stopwatch.StartNew();
                        List<SourceEntity> sourceEntityList = ExcelHelper.ReadFromExcel<SourceEntity>(inputFilePath);
                        watch.Stop();
                        AppendMessage("读取Excel文件完成，耗时" + watch.ElapsedMilliseconds + "毫秒");

                        AppendMessage("开始处理数据");
                        watch.Restart();
                        Dictionary<int, List<TargetEntity>> targetDictionary = Process(sourceEntityList);
                        AppendMessage("处理数据完成，耗时" + watch.ElapsedMilliseconds + "毫秒");

                        AppendMessage("开始写入结果文件");
                        foreach (var target in targetDictionary)
                        {
                            AppendMessage("正在写入" + target.Key + "的数据");
                            watch.Restart();
                            ExcelHelper.WriteToExcel(outputFilePath, target.Key.ToString(), target.Value);
                            watch.Stop();
                            AppendMessage("写入" + target.Key + "的数据完成，耗时" + watch.ElapsedMilliseconds + "毫秒");
                        }
                        AppendMessage("写入结果文件完成");

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

        private Dictionary<int, List<TargetEntity>> Process(List<SourceEntity> sourceEntityList)
        {
            Dictionary<int, List<TargetEntity>> returnValue = new Dictionary<int, List<TargetEntity>>();

            List<SourceModel> sourceModelList = ToModelList(sourceEntityList);

            List<SourceModel> history = new List<SourceModel>();

            var months = sourceModelList.GroupBy(v => v.Month).ToDictionary(v => v.Key, v => v.ToList());
            foreach (var month in months)
            {
                var items = month.Value;

                var processResult = ProcessMonth(month.Value, history);

                returnValue.Add(month.Key, processResult);
            }

            return returnValue;
        }

        /// <summary>
        /// 处理一个月之类的数据
        /// </summary>
        /// <param name="sourceModelList"></param>
        private static List<TargetEntity> ProcessMonth(List<SourceModel> sourceModelList, List<SourceModel> history)
        {
            List<TargetEntity> returnValue = new List<TargetEntity>();

            var groups = sourceModelList.GroupBy(v => v.BusinessNo).ToList();

            foreach (var group in groups)
            {
                List<TargetEntity> groupReturnValue = new List<TargetEntity>();

                var buyList = group.Where(v => v.Mode == 1).ToList();
                var sellList = group.Where(v => v.Mode == 2).ToList();

                int totalBuyTime = buyList.Count;
                int totalSellTime = sellList.Count;
                int totalBuyCount = buyList.Sum(v => v.Count);
                int totalSellCount = sellList.Sum(v => v.Count);

                foreach (var sourceModel in group)
                {
                    TargetEntity targetEntity = ToTargetEntity(sourceModel);
                    targetEntity.TotalBuyCount = totalBuyCount;
                    targetEntity.TotalBuyTime = totalBuyTime;
                    targetEntity.TotalSellCount = totalSellCount;
                    targetEntity.TotalSellTime = totalSellTime;
                    groupReturnValue.Add(targetEntity);
                }

                if (totalBuyCount == totalSellCount) //总进货数等于总销货数，判为直运
                {
                    foreach (var targetEntity in groupReturnValue)
                    {
                        targetEntity.SaleMode = "直运";
                        targetEntity.TotalRemain = 0;
                        targetEntity.TotalRequire = 0;
                    }
                }
                else if (totalBuyCount > totalSellCount) //进货数大于销货数
                {
                    int value = totalBuyCount - totalSellCount;
                    foreach (var targetEntity in groupReturnValue)
                    {
                        targetEntity.SaleMode = "没卖完";
                        targetEntity.TotalRemain = value;
                        targetEntity.TotalRequire = 0;
                    }
                }
                else //进货数大于销货数
                {
                    int value = totalSellCount - totalBuyCount;
                    foreach (var targetEntity in groupReturnValue)
                    {
                        targetEntity.SaleMode = "不够卖";
                        targetEntity.TotalRemain = 0;
                        targetEntity.TotalRequire = value;
                    }
                }

                returnValue.AddRange(groupReturnValue);
            }

            return returnValue;
        }

        private static List<SourceModel> ToModelList(List<SourceEntity> entityList)
        {
            List<SourceModel> returnValue = new List<SourceModel>();

            foreach (var entity in entityList)
            {
                var model = new SourceModel();

                model.BusinessNo = entity.BusinessNo;
                model.PaperNo = entity.PaperNo;
                model.ProductNo = entity.ProductNo;
                model.Count = entity.Count;
                model.CreateTime = entity.CreateTime;
                model.Mode = "采购".Equals(entity.Mode) ? 1 : 2;
                model.Month = entity.CreateTime.Year * 100 + entity.CreateTime.Month;

                returnValue.Add(model);
            }

            returnValue = returnValue.OrderBy(v => v.Month).ToList();

            return returnValue;
        }

        private static TargetEntity ToTargetEntity(SourceModel sourceModel)
        {
            TargetEntity targetEntity = new TargetEntity();

            targetEntity.BusinessNo = sourceModel.BusinessNo;
            targetEntity.PaperNo = sourceModel.PaperNo;
            targetEntity.ProductNo = sourceModel.ProductNo;
            if (sourceModel.Mode == 1)
            {
                targetEntity.BuyCount = sourceModel.Count;
            }
            else
            {
                targetEntity.SellCount = sourceModel.Count;
            }

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
            List<SourceEntity> sourceEntityList = GenerateSourceEntityList(businessEntityList, productEntityList);

            //数据源表
            ExcelHelper.WriteToExcel(fileName, businessEntityList);
            ExcelHelper.WriteToExcel(fileName, productEntityList);
            ExcelHelper.WriteToExcel(fileName, sourceEntityList);

        }

        // 生产基础数据表
        // 2017年，每天5-10单
        private static List<SourceEntity> GenerateSourceEntityList(List<BusinessEntity> businessNoList, List<ProductEntity> productEntityList)
        {
            List<SourceEntity> returnValue = new List<SourceEntity>();

            Random random = new Random();
            DateTime today = DateTime.Now.Date;

            int businessNo = 1;

            for (int i = -300; i < 0; i++)
            {
                //指定日期
                var day = today.AddDays(i);

                //每天30-50单
                var sourceCount = random.Next(30, 50);
                for (int sourceIndex = 0; sourceIndex < sourceCount; sourceIndex++)
                {
                    SourceEntity sourceEntity = new SourceEntity();

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

        private static List<ProductEntity> GenerateProductEntityList()
        {
            List<ProductEntity> productEntityList = new List<ProductEntity>();

            var names = new List<string> {
                "苹果", "香蕉", "橘子", "柿子", "桃", "荔枝"
                //, "龙眼", "柑桔", "李子", "樱桃", "葡萄", "菠萝", "青梅", "椰子", "番石榴", "草莓"
            };
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