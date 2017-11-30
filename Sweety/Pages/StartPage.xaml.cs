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
                        AppendTheMessage("开始读取excel文件.");
                        Stopwatch watch = Stopwatch.StartNew();
                        List<SourceEntity> sourceEntityList = ExcelHelper.ReadFromExcel<SourceEntity>(inputFilePath);
                        watch.Stop();
                        AppendTheMessage("读取Excel文件完成，耗时" + watch.ElapsedMilliseconds + "毫秒");

                        AppendTheMessage("开始处理数据");
                        watch.Restart();
                        Dictionary<int, List<TargetEntity>> targetDictionary = Process(sourceEntityList);
                        AppendTheMessage("处理数据完成，耗时" + watch.ElapsedMilliseconds + "毫秒");

                        AppendTheMessage("开始写入结果文件");
                        foreach (var target in targetDictionary)
                        {
                            AppendTheMessage("正在写入" + target.Key + "的数据");
                            watch.Restart();
                            ExcelHelper.WriteToExcel(outputFilePath, target.Key.ToString(), target.Value);
                            watch.Stop();
                            AppendTheMessage("写入" + target.Key + "的数据完成，耗时" + watch.ElapsedMilliseconds + "毫秒");
                        }
                        AppendTheMessage("写入结果文件完成");

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

            List<TargetEntity> theRemainHistory = new List<TargetEntity>();
            List<TargetEntity> theRequireHistory = new List<TargetEntity>();

            var months = sourceModelList.GroupBy(v => v.Month).ToDictionary(v => v.Key, v => v.ToList());
            foreach (var month in months)
            {
                var items = month.Value;

                //处理本月数据
                var processResult = ProcessMonth(month.Value);

                //本月有结余的产品
                var remainEntitys = processResult.Where(v => v.Remain > 0).ToList();
                if (remainEntitys.Count > 0)
                {
                    var remainGroups = ToGroup(remainEntitys);
                    foreach (var remainGroup in remainGroups)
                    {
                        ProcessRemain(remainGroup, theRequireHistory);
                    }
                }

                //本月不足的产品
                var requireEntitys = processResult.Where(v => v.Require > 0).ToList();
                if (requireEntitys.Count > 0)
                {
                    var requireGroups = ToGroup(requireEntitys);
                    foreach (var requireGroup in requireGroups)
                    {
                        ProcessRequire(requireGroup, theRemainHistory);
                    }
                }

                theRemainHistory.RemoveAll(v => v.Remain == 0);
                theRequireHistory.RemoveAll(v => v.Require == 0);

                remainEntitys = remainEntitys.Where(v => v.Remain > 0).ToList();
                theRemainHistory.AddRange(remainEntitys);

                requireEntitys = requireEntitys.Where(v => v.Require > 0).ToList();
                theRequireHistory.AddRange(requireEntitys);

                returnValue.Add(month.Key, processResult);
            }

            return returnValue;
        }

        private static void ProcessRemain(TargetEntityGroup remainGroup, List<TargetEntity> requireHistory)
        {
            var historyEntitys = requireHistory.Where(v => v.BusinessNo == remainGroup.BusinessNo && v.ProductNo == remainGroup.ProductNo).ToList();
            if (historyEntitys.Count == 0)
            {
                return;
            }

            List<string> remainPaperNos = remainGroup.EntityList.Select(v => "[" + v.PaperNo + "]").ToList();

            var requireGroups = ToGroup(historyEntitys);
            foreach (var requireGroup in requireGroups)
            {
                List<string> requirePaperNos = requireGroup.EntityList.Select(v => "(" + v.PaperNo + ")").ToList();

                int count = Math.Min(remainGroup.Remain, requireGroup.Require);
                remainGroup.Remain -= count;
                requireGroup.Require -= count;

                //处理子单
                foreach (var remainEntity in remainGroup.EntityList)
                {
                    remainEntity.Remain -= count;
                    remainEntity.RelatedPapers.AddRange(requirePaperNos);
                }

                foreach (var requireEntity in requireGroup.EntityList)
                {
                    requireEntity.RelatedPapers.AddRange(remainPaperNos);
                }

                if (remainGroup.Remain <= 0)
                {
                    break;
                }
            }
        }

        private static void ProcessRequire(TargetEntityGroup requireGroup, List<TargetEntity> remainHistory)
        {
            var historyEntitys = remainHistory.Where(v => v.BusinessNo == requireGroup.BusinessNo && v.ProductNo == requireGroup.ProductNo).ToList();
            if (historyEntitys.Count == 0)
            {
                return;
            }

            List<string> requirePaperNos = requireGroup.EntityList.Select(v => "(" + v.PaperNo + ")").ToList();

            var requireGroups = ToGroup(historyEntitys);
            foreach (var remainGroup in requireGroups)
            {
                List<string> remainPaperNos = remainGroup.EntityList.Select(v => "[" + v.PaperNo + "]").ToList();

                int count = Math.Min(requireGroup.Require, remainGroup.Remain);
                requireGroup.Require -= count;
                remainGroup.Remain -= count;

                //处理子单
                foreach (var requireEntity in requireGroup.EntityList)
                {
                    requireEntity.Require -= count;
                    requireEntity.RelatedPapers.AddRange(remainPaperNos);
                }

                foreach (var remainEntity in remainGroup.EntityList)
                {
                    remainEntity.RelatedPapers.AddRange(requirePaperNos);
                }

                if (requireGroup.Require <= 0)
                {
                    break;
                }
            }
        }

        private static List<TargetEntityGroup> ToGroup(List<TargetEntity> entityList)
        {
            List<TargetEntityGroup> returnValue = new List<TargetEntityGroup>();

            foreach (var entity in entityList)
            {
                var group = returnValue.FirstOrDefault(v => v.BusinessNo == entity.BusinessNo && v.ProductNo == entity.ProductNo && v.Month == entity.Month);
                if (group == null)
                {
                    group = new TargetEntityGroup();

                    group.BusinessNo = entity.BusinessNo;
                    group.ProductNo = entity.ProductNo;
                    group.Month = entity.Month;
                    group.Remain = entity.Remain;
                    group.Require = entity.Require;
                    group.EntityList = new List<TargetEntity>();

                    returnValue.Add(group);
                }

                group.EntityList.Add(entity);

            }

            return returnValue;
        }

        class TargetEntityGroup
        {
            public string BusinessNo { get; set; }

            public string ProductNo { get; set; }

            public int Month { get; set; }

            public int Remain { get; set; }

            public int Require { get; set; }

            public List<TargetEntity> EntityList { get; set; }
        }

        /// <summary>
        /// 处理一个月之类的数据
        /// </summary>
        /// <param name="monthSourceModelList"></param>
        private static List<TargetEntity> ProcessMonth(List<SourceModel> monthSourceModelList)
        {
            List<TargetEntity> returnValue = new List<TargetEntity>();

            var businesses = monthSourceModelList.GroupBy(v => v.BusinessNo).ToList();

            foreach (var business in businesses)
            {
                List<TargetEntity> groupReturnValue = new List<TargetEntity>();

                var productGroups = business.GroupBy(v => v.ProductNo).ToList();
                foreach (var productGroup in productGroups)
                {
                    var productBuyList = productGroup.Where(v => v.Mode == 1).ToList();
                    var productSellList = productGroup.Where(v => v.Mode == 2).ToList();

                    int productTotalBuyTime = productBuyList.Count;
                    int productTotalBuyCount = productBuyList.Sum(v => v.Count);
                    int productTotalSellTime = productSellList.Count;
                    int productTotalSellCount = productSellList.Sum(v => v.Count);

                    foreach (var sourceModel in productGroup)
                    {
                        TargetEntity targetEntity = ToTargetEntity(sourceModel);

                        if (targetEntity.BuyCount > 0)
                        {
                            targetEntity.ProductBuyTime = productTotalBuyTime;
                            targetEntity.ProductBuyCount = productTotalBuyCount;
                        }
                        if (targetEntity.SellCount > 0)
                        {
                            targetEntity.ProductSellTime = productTotalSellTime;
                            targetEntity.ProductSellCount = productTotalSellCount;
                        }

                        groupReturnValue.Add(targetEntity);
                    }

                    if (productTotalBuyCount == productTotalSellCount)
                    {
                        //总进货数等于总销货数，判为直运

                        foreach (var targetEntity in groupReturnValue)
                        {
                            targetEntity.SaleMode = "直运";
                        }
                    }
                    else if (productTotalBuyCount > productTotalSellCount)
                    {
                        //本月结余的量
                        int remain = productTotalBuyCount - productTotalSellCount;
                        foreach (var targetEntity in groupReturnValue)
                        {
                            targetEntity.SaleMode = "本月没卖完";
                            targetEntity.ProductTotalRemain = remain;
                            targetEntity.Remain = remain;
                        }
                    }
                    else
                    {
                        //进货数大于销货数
                        //本次结算，不足的部分，记录到历史账单中

                        int require = productTotalSellCount - productTotalBuyCount;
                        foreach (var targetEntity in groupReturnValue)
                        {
                            targetEntity.SaleMode = "本月不够卖";
                            targetEntity.ProductTotalRequire = require;
                            targetEntity.Require = require;
                        }
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
                model.Month = entity.CreateTime / 100;

                returnValue.Add(model);
            }

            returnValue = returnValue.OrderBy(v => v.Month).ToList();

            return returnValue;
        }

        private static DateTime ToTime(int value)
        {
            return new DateTime(value / 10000, value % 10000 / 100, value % 100);
        }
        private static int ToInt(DateTime value)
        {
            return value.Year * 10000 + value.Month * 100 + value.Day;
        }

        private static TargetEntity ToTargetEntity(SourceModel sourceModel)
        {
            TargetEntity targetEntity = new TargetEntity();

            targetEntity.BusinessNo = sourceModel.BusinessNo;
            targetEntity.PaperNo = sourceModel.PaperNo;
            targetEntity.ProductNo = sourceModel.ProductNo;
            targetEntity.Month = sourceModel.Month;
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
        private void AppendTheMessage(string message)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                AppendMessage(message);
            }));
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

            for (int i = -80; i < 0; i++)
            //for (int i = -300; i < 0; i++)
            {
                //指定日期
                var day = today.AddDays(i);

                //每天30-50单
                var sourceCount = random.Next(3, 5);
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
                    sourceEntity.CreateTime = ToInt(day);

                    returnValue.Add(sourceEntity);
                }
            }

            return returnValue;
        }

        private static List<ProductEntity> GenerateProductEntityList()
        {
            List<ProductEntity> productEntityList = new List<ProductEntity>();

            var names = new List<string> {
                "苹果", "香蕉"
                //, "橘子", "柿子", "桃", "荔枝"
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