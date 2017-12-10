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

            Process(inputGroup);

            List<OutputBuyEntity> outputBuyEntityList = new List<OutputBuyEntity>();
            List<OutputSellEntity> outputSellEntityList = new List<OutputSellEntity>();

            ToEntity(inputGroup, outputBuyEntityList, outputSellEntityList);

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
                if (string.IsNullOrEmpty(buyEntity.Id))
                {
                    continue;
                }

                BuyModel buyModel = new BuyModel();

                buyModel.Id = buyEntity.Id;
                buyModel.ProductNo = buyEntity.ProductNo;
                buyModel.BuyContractNo = buyEntity.BuyContractNo;
                buyModel.BianHao = buyEntity.BianHao;
                buyModel.XiaoFangMinCheng = buyEntity.XiaoFangMinCheng;
                buyModel.ProductName = buyEntity.ProductName;
                buyModel.ZuBie = buyEntity.ZuBie;
                buyModel.ShouPiaoHaoMa = buyEntity.BillNo;
                buyModel.FaPiaoJinE = buyEntity.BillMoney;
                buyModel.BuHanShuiJinE = buyEntity.WithoutTaxMoney;
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
                if (string.IsNullOrEmpty(sellEntity.SellContractNo))
                {
                    continue;
                }

                SellModel sellModel = new SellModel();

                sellModel.XiaoShouContractNo = sellEntity.SellContractNo;
                sellModel.GouHuoDanWei = sellEntity.GouHuoDanWei;
                sellModel.ProductName = sellEntity.ProductName;
                sellModel.ProductCount = sellEntity.ProductCount;
                sellModel.ShangPinHanSuiDanJia = sellEntity.ShangPinHanShuiDanJia;
                sellModel.ShangPinHanShuiJinE = sellEntity.ShangPinHanShuiJinE;
                sellModel.FaPiaoHao = sellEntity.FaPiaoHao;

                sellModelList.Add(sellModel);
            }

            return sellModelList;
        }

        private static List<MappingModel> ToModel(List<MappingEntity> mappingEntityList)
        {
            List<MappingModel> mappingModelList = new List<MappingModel>();

            foreach (var mappingEntity in mappingEntityList)
            {
                if (string.IsNullOrEmpty(mappingEntity.Id))
                {
                    continue;
                }

                MappingModel mappingModel = new MappingModel();

                mappingModel.Id = mappingEntity.Id;
                mappingModel.Applicant = mappingEntity.Applicant;
                mappingModel.ContractNo = mappingEntity.ContractNo;
                mappingModel.ProductNo = mappingEntity.ProductNo;
                mappingModel.GroupCategory = mappingEntity.GroupCategory;

                //合同状态
                switch (mappingEntity.ContractStatus)
                {
                    case "正常":
                        {
                            mappingModel.ContractStatus = 1;
                        }
                        break;
                    case "撤单":
                        {
                            mappingModel.ContractStatus = 2;
                        }
                        break;
                    case "撤单-还利差":
                        {
                            mappingModel.ContractStatus = 3;
                        }
                        break;
                    default:
                        break;
                }

                mappingModelList.Add(mappingModel);
            }

            return mappingModelList;
        }

        private static void Process(InputGroup inputGroup)
        {
            if (inputGroup.BuyModelList == null || inputGroup.BuyModelList.Count == 0)
            {
                return;
            }

            if (inputGroup.SellModelList == null || inputGroup.SellModelList.Count == 0)
            {
                return;
            }

            if (inputGroup.MappingModelList == null || inputGroup.MappingModelList.Count == 0)
            {
                return;
            }

            var sellModelList = inputGroup.SellModelList;
            var buyModelList = inputGroup.BuyModelList;
            var mappingModelList = inputGroup.MappingModelList;

            // Step 1. 从商务报表中找到“本期销项明细”表中销售合同号对应的货号，填充到本期销项明细表中
            foreach (var sellModel in sellModelList)
            {
                FindMapping(sellModel, mappingModelList);
            }

            //有货号的本期销项明细表
            var sellModelWithProductNoList = sellModelList.Where(v => !string.IsNullOrEmpty(v.ProductNo)).ToList();
            // Step 2. 查找直运
            foreach (var sellModel in sellModelWithProductNoList)
            {
                FindDirectBusiness(sellModel, buyModelList);
            }
        }

        /// <summary>
        /// 从商务报表里面找到比配的记录
        /// </summary>
        /// <param name="sellModel"></param>
        /// <param name="mappingModelList"></param>
        private static void FindMapping(SellModel sellModel, List<MappingModel> mappingModelList)
        {
            var mappings = mappingModelList.Where(v => v.ContractNo == sellModel.XiaoShouContractNo && v.ContractStatus == 1).ToList();

            if (mappings.Count == 1)
            {
                sellModel.ProductNo = mappings[0].ProductNo;
                return;
            }

            if (mappings.Count > 1)
            {
                sellModel.MappingIds = mappings.Select(v => v.Id).ToList();
                sellModel.Remarks.Add(Remark.FindMultiContractNoInMapping);
                return;
            }

            if (mappings.Count == 0)
            {
                sellModel.Remarks.Add(Remark.FindZeroContractNoInMapping);

                if (sellModel.XiaoShouContractNo.Contains("-"))
                {
                    var secondContractNo = sellModel.XiaoShouContractNo.Substring(0, sellModel.XiaoShouContractNo.LastIndexOf("-"));
                    var secondMappings = mappingModelList.Where(v => v.ContractNo == secondContractNo && v.ContractStatus == 1).ToList();

                    if (secondMappings.Count == 1)
                    {
                        sellModel.ProductNo = secondMappings[0].ProductNo;
                        sellModel.MappingIds = secondMappings.Select(v => v.Id).ToList();
                        sellModel.Remarks.Add(Remark.FindOneContractNoInMappingUsingSecondContractId);
                        return;
                    }

                    if (secondMappings.Count > 1)
                    {
                        sellModel.MappingIds = secondMappings.Select(v => v.Id).ToList();
                        sellModel.Remarks.Add(Remark.FindMultiContractNoInMappingUsingSecondContractId);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 在本期销项明细表和本期进项明细表中找到直运的记录
        /// </summary>
        /// <param name="sellModel"></param>
        /// <param name="buyModelList"></param>
        private static void FindDirectBusiness(SellModel sellModel, List<BuyModel> buyModelList)
        {
            var buyModelList_ProductNoMatched = buyModelList.Where(v => v.ProductNo.Equals(sellModel.ProductNo)).ToList();

            if (buyModelList_ProductNoMatched.Count == 1)
            {
                if (buyModelList_ProductNoMatched[0].ReceiveTicketCount == sellModel.ProductCount)
                {
                    sellModel.SellMode = 1;
                    buyModelList_ProductNoMatched[0].SellMode = 1;
                }
            }
        }

        private static void ToEntity(InputGroup inputGroup, List<OutputBuyEntity> outputBuyEntityList, List<OutputSellEntity> outputSellEntityList)
        {
            //本期销项明细
            if (inputGroup.SellModelList != null && inputGroup.SellModelList.Count > 0)
            {
                foreach (var sellModel in inputGroup.SellModelList)
                {
                    OutputSellEntity sellEntity = new OutputSellEntity();

                    sellEntity.ProductNo = sellModel.ProductNo;
                    sellEntity.XiaoShouContractNo = sellModel.XiaoShouContractNo;
                    sellEntity.GouHuoDanWei = sellModel.GouHuoDanWei;
                    sellEntity.ProductName = sellModel.ProductName;
                    sellEntity.ProductCount = sellModel.ProductCount;
                    sellEntity.ShangPinHanShuiDanJia = sellModel.ShangPinHanSuiDanJia;
                    sellEntity.ShangPinHanShuiJinE = sellModel.ShangPinHanShuiJinE;
                    sellEntity.FaPiaoHao = sellModel.FaPiaoHao;
                    sellEntity.JinXiangShouPiaoDunShu = sellModel.JinXiangShouPiaoDunShu;
                    sellEntity.KuCun = sellModel.KuCun;
                    sellEntity.SaleMode = ToSellMode(sellModel.SellMode);
                    sellEntity.CaiGouDanWei = sellModel.CaiGouDanWei;
                    sellEntity.CaiGouDanJia = sellModel.CaiGouDanJia;
                    sellEntity.ZanGuCaiGouZongJia = sellModel.ZanGuCaiGouZongJia;
                    sellEntity.CaiGouContractNo = sellModel.CaiGouContractNo;
                    sellEntity.ZanGuCaiGouBuHanShuiJinE = sellModel.ZanGuCaiGouBuHanShuiJinE;

                    outputSellEntityList.Add(sellEntity);
                }
            }

            //本期进项明细
            if (inputGroup.BuyModelList != null && inputGroup.BuyModelList.Count > 0)
            {
                foreach (var buyModel in inputGroup.BuyModelList)
                {
                    OutputBuyEntity buyEntity = new OutputBuyEntity();

                    buyEntity.Id = buyModel.Id;
                    buyEntity.GoodsNo = buyModel.ProductNo;
                    buyEntity.CaiGouContractNo = buyModel.BuyContractNo;
                    buyEntity.BianHao = buyModel.BianHao;
                    buyEntity.XiaoFangMinCheng = buyModel.XiaoFangMinCheng;
                    buyEntity.ProductName = buyModel.ProductName;
                    buyEntity.ZuBie = buyModel.ZuBie;
                    buyEntity.ShouPiaoHaoMa = buyModel.ShouPiaoHaoMa;
                    buyEntity.FaPiaoJinE = buyModel.FaPiaoJinE;
                    buyEntity.BuHanShuiJinE = buyModel.BuHanShuiJinE;
                    buyEntity.TaxMoney = buyModel.TaxMoney;
                    buyEntity.ReceiveTicketCount = buyModel.ReceiveTicketCount;
                    buyEntity.ShangPinHanShuiDanJia = buyModel.ShangPinHanShuiDanJia;
                    buyEntity.SellMode = ToSellMode(buyModel.SellMode);


                    outputBuyEntityList.Add(buyEntity);
                }
            }
        }

        private static string ToSellMode(int sellStatus)
        {
            switch (sellStatus)
            {
                case 1:
                    return "直运";
                default:
                    return null;
            }
        }
    }
}
