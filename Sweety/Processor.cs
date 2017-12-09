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
            List<MappingEntity> mappingEntityList = ExcelReader.ReadEntityList<MappingEntity>(inputFilePath, "商务报表");

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
        private static InputGroup ToInputGroup(List<BuyEntity> buyEntityList, List<SellEntity> sellEntityList, List<MappingEntity> mappingEntityList)
        {
            InputGroup returnValue = new InputGroup();

            if (buyEntityList != null && buyEntityList.Count > 0)
            {
                returnValue.BuyModelList = ToModel(buyEntityList);
            }

            if (sellEntityList != null && sellEntityList.Count > 0)
            {
                returnValue.SellModelList = ToModel(sellEntityList);
            }

            if (mappingEntityList != null && mappingEntityList.Count > 0)
            {
                returnValue.MappingModelList = ToModel(mappingEntityList);
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

        private static List<MappingModel> ToModel(List<MappingEntity> mappingEntityList)
        {
            List<MappingModel> mappingModelList = new List<MappingModel>();

            foreach (var mappingEntity in mappingEntityList)
            {
                MappingModel mappingModel = new MappingModel();

                mappingModel.Applicant = mappingEntity.Applicant;
                mappingModel.ContractNo = mappingEntity.ContractNo;
                mappingModel.ProductNo = mappingEntity.ProductNo;
                mappingModel.GroupCategory = mappingEntity.GroupCategory;

                mappingModelList.Add(mappingModel);
            }

            return mappingModelList;
        }

        private static OutputGroup Process(InputGroup inputGroup)
        {
            OutputGroup returnValue = new OutputGroup();

            if (inputGroup.BuyModelList == null || inputGroup.BuyModelList.Count == 0)
            {
                return returnValue;
            }

            if (inputGroup.SellModelList == null || inputGroup.SellModelList.Count == 0)
            {
                return returnValue;
            }

            if (inputGroup.MappingModelList == null || inputGroup.MappingModelList.Count == 0)
            {
                return returnValue;
            }

            var sellModelList = inputGroup.MappingModelList;
            var buyModelList = inputGroup.BuyModelList;
            var mappingModelList = inputGroup.MappingModelList;

            //从商务报表中找到“本期销项明细”表中销售合同号对应的货号
            foreach (var sellModel in inputGroup.SellModelList)
            {
                var mappings = mappingModelList.Where(v => v.ContractNo == sellModel.SellContractNo).ToList();
                if (mappings.Count == 0)
                {
                    sellModel.IsDone = true;
                    sellModel.Remarks.Add(Remark.FindZeroContractNoInMapping);
                }

                if (mappings.Count > 1)
                {
                    sellModel.IsDone = true;
                    sellModel.Remarks.Add(Remark.FindMultiContractNoInMapping);
                }

                var mapping = mappings[0];

                sellModel.ProductNo = mapping.ProductNo;
            }


            return returnValue;
        }

        private static void ToEntity(OutputGroup outputGroup, List<OutputBuyEntity> outputBuyEntityList, List<OutputSellEntity> outputSellEntityList)
        {

        }
    }
}
