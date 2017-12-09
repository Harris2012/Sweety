using Sweety.InputEntity;
using Sweety.Model;
using Sweety.OutputEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety
{
    static class Processor
    {
        public static void Run(string inputFilePath, string outputFilePath)
        {
            List<BuyEntity> buyEntityList = ExcelReader.ReadEntityList<BuyEntity>(inputFilePath, "本期进项明细");
            List<SellEntity> sellEntityList = ExcelReader.ReadEntityList<SellEntity>(inputFilePath, "本期销项明细");
            List<BusinessEntity> mappingEntityList = ExcelReader.ReadEntityList<BusinessEntity>(inputFilePath, "商务报表");

            InputGroup inputGroup = ToInputGroup(buyEntityList, sellEntityList, mappingEntityList);

            OutputGroup outputGroup = Process(inputGroup);

            List<OutputBuyEntity> outputBuyEntityList = new List<OutputBuyEntity>();
            List<OutputSellEntity> outputSellEntityList = new List<OutputSellEntity>();

            ToEntity(outputGroup, outputBuyEntityList, outputSellEntityList);

            ExcelHelper.WriteToExcel(outputFilePath, outputBuyEntityList);
            ExcelHelper.WriteToExcel(outputFilePath, outputSellEntityList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyEntityList">本期进项明细</param>
        /// <param name="sellEntityList">本期销项明细</param>
        /// <param name="mappingEntityList">商务报表</param>
        /// <returns></returns>
        private static InputGroup ToInputGroup(List<BuyEntity> buyEntityList, List<SellEntity> sellEntityList, List<BusinessEntity> mappingEntityList)
        {
            InputGroup returnValue = new InputGroup();

            if (buyEntityList != null && buyEntityList.Count > 0)
            {
                returnValue.BuyModelList = ToModel(buyEntityList);
            }

            if (sellEntityList != null && sellEntityList.Count > 0)
            {

            }

            return returnValue;
        }

        private static List<BuyModel> ToModel(List<BuyEntity> buyEntityList)
        {
            List<BuyModel> buyModelList = new List<BuyModel>();

            foreach (var buyEntity in buyEntityList)
            {
                BuyModel buyModel = new BuyModel();

                buyModel.Id = buyEntity.Id;
                buyModel.ProductNo = buyEntity.ProductNo;
                buyModel.BuyContractNo = buyEntity.BuyContractNo;
                buyModel.Number = buyEntity.Number;
                buyModel.销方名称 = buyEntity.销方名称;
                buyModel.ProductName = buyEntity.ProductName;
                buyModel.组别 = buyEntity.组别;
                buyModel.BillNo = buyEntity.BillNo;
                buyModel.BillMoney = buyEntity.BillMoney;
                buyModel.WithoutTaxMoney = buyEntity.WithoutTaxMoney;
                buyModel.TaxMoney = buyEntity.TaxMoney;
                buyModel.ReceiveTicketCount = buyEntity.ReceiveTicketCount;

                buyModelList.Add(buyModel);
            }

            return buyModelList;
        }

        private static List<SellModel> ToModel(List<SellEntity> sellEntityList)
        {
            List<SellModel> sellModelList = new List<SellModel>();

            foreach (var sellEntity in sellEntityList)
            {
                SellModel sellModel = new SellModel();

                sellModel.SellContractNo = sellEntity.SellContractNo;
                sellModel.BuProductOrganization = sellEntity.BuProductOrganization;
                sellModel.ProductName = sellEntity.ProductName;
                sellModel.ProductCount = sellEntity.ProductCount;
                sellModel.ProductUnitPriceWithTax = sellEntity.ProductUnitPriceWithTax;
                sellModel.ProductMoneyWithTax = sellEntity.ProductMoneyWithTax;
                sellModel.BillNo = sellEntity.BillNo;

                sellModelList.Add(sellModel);
            }

            return sellModelList;
        }

        private static List<MappingModel> ToModel()
        {

        }

        private static OutputGroup Process(InputGroup inputGroup)
        {
            OutputGroup returnValue = new OutputGroup();


            return returnValue;
        }

        private static void ToEntity(OutputGroup outputGroup, List<OutputBuyEntity> outputBuyEntityList, List<OutputSellEntity> outputSellEntityList)
        {

        }
    }
}
