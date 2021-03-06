﻿using Sweety.InputEntity;
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

            ToOutputEntity(inputGroup, outputBuyEntityList, outputSellEntityList);

            ExcelHelper.WriteToExcel(outputFilePath, getRemarkList());
            ExcelHelper.WriteToExcel(outputFilePath, outputBuyEntityList);
            ExcelHelper.WriteToExcel(outputFilePath, outputSellEntityList);
        }

        private static List<RemarkEntity> getRemarkList()
        {
            List<RemarkEntity> returnValue = new List<RemarkEntity>();

            returnValue.Add(ToRemarkEntity(Remark._2001_FindZeroContractNoInMapping));
            returnValue.Add(ToRemarkEntity(Remark._2002_FindMultiContractNoInMapping));
            returnValue.Add(ToRemarkEntity(Remark._2003_FindOneContractNoInMappingUsingSecondContractId));
            returnValue.Add(ToRemarkEntity(Remark._2004_FindMultiContractNoInMappingUsingSecondContractId));
            returnValue.Add(ToRemarkEntity(Remark._2005_UsingPossibleProductNo));
            returnValue.Add(ToRemarkEntity(Remark._2006_FindMultiProductNoInSell));

            return returnValue;
        }

        private static RemarkEntity ToRemarkEntity(Remark remark)
        {
            return new RemarkEntity { Id = string.Format("R{0}", remark.RemarkNo), Message = remark.Message };
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

                sellModel.GouHuoDanWei = sellEntity.GouHuoDanWei;
                sellModel.ProductName = sellEntity.ProductName;
                sellModel.ProductCount = sellEntity.ProductCount;
                sellModel.ShangPinHanSuiDanJia = sellEntity.ShangPinHanShuiDanJia;
                sellModel.ShangPinHanShuiJinE = sellEntity.ShangPinHanShuiJinE;
                sellModel.FaPiaoHao = sellEntity.FaPiaoHao;

                sellModel.XiaoShouContractNo = sellEntity.SellContractNo;
                if (sellModel.XiaoShouContractNo.Contains("-"))
                {
                    var secondContractNo = sellModel.XiaoShouContractNo.Substring(0, sellModel.XiaoShouContractNo.LastIndexOf("-"));
                    sellModel.SetXiaoShouContractNoWithoutLine(secondContractNo);
                }
                else
                {
                    sellModel.SetXiaoShouContractNoWithoutLine(sellModel.XiaoShouContractNo);
                }

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

                //合同状态
                if (!"正常".Equals(mappingEntity.ContractStatus))
                {
                    continue;
                }

                MappingModel mappingModel = new MappingModel();

                mappingModel.Id = mappingEntity.Id;
                mappingModel.Applicant = mappingEntity.Applicant;
                mappingModel.ContractNo = mappingEntity.ContractNo;
                mappingModel.ProductNo = mappingEntity.ProductNo;
                mappingModel.GroupCategory = mappingEntity.GroupCategory;
                //mappingModel.ChengJiaoDunShu = mappingEntity.ChengJiaoDunShu;
                mappingModel.GouXiaoHeTongTaiTou = mappingEntity.GouXiaoHeTongTaiTou;
                mappingModel.FaPiaoShiJian = mappingEntity.FaPiaoShiJian;
                mappingModel.KaiChuShouDaoFaPiaoDunShu = mappingEntity.KaiChuShouDaoFaPiaoDunShu;
                mappingModel.FaPiaoHaoMa = mappingEntity.FaPiaoHaoMa;

                switch (mappingEntity.MaiMai)
                {
                    case "买":
                        mappingModel.MaiMai = 1;
                        break;
                    case "卖":
                        mappingModel.MaiMai = 2;
                        break;
                    default:
                        break;
                }

                //合同状态
                //switch (mappingEntity.ContractStatus)
                //{
                //    case "正常":
                //        {
                //            mappingModel.ContractStatus = 1;
                //        }
                //        break;
                //    case "撤单":
                //        {
                //            mappingModel.ContractStatus = 2;
                //        }
                //        break;
                //    case "撤单-还利差":
                //        {
                //            mappingModel.ContractStatus = 3;
                //        }
                //        break;
                //    default:
                //        break;
                //}

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

            // 【匹配货号】Step 1. 从商务报表中找到“本期销项明细”表中销售合同号对应的货号，填充到本期销项明细表中
            foreach (var sellModel in sellModelList)
            {
                FindMapping(sellModel, mappingModelList);
            }

            //标出销项明细表中同货号的记录
            {
                var items = sellModelList.Where(v => !string.IsNullOrEmpty(v.ProductNo)).GroupBy(v => v.ProductNo).Where(v => v.Count() > 1).ToList();
                foreach (var item in items)
                {
                    foreach (var sellModel in item)
                    {
                        sellModel.Remarks.Add(Remark._2006_FindMultiProductNoInSell);
                    }
                }
            }

            //【匹配货号】对于找到多个货号的销售合同号，尝试进行匹配
            {
                var sellModelList_WithoutProductNo = sellModelList.Where(v => string.IsNullOrEmpty(v.ProductNo)
                    && (v.Remarks.Contains(Remark._2002_FindMultiContractNoInMapping) || v.Remarks.Contains(Remark._2004_FindMultiContractNoInMappingUsingSecondContractId))).ToList();

                var sellModelGroups = sellModelList_WithoutProductNo.GroupBy(v => v.XiaoShouContractNoWithoutLine).ToList();
                foreach (var sellModelGroup in sellModelGroups)
                {
                    var mappingModelList_ContractNoMatched = mappingModelList.Where(v => v.ContractNo.Equals(sellModelGroup.Key)).ToList();
                    if (sellModelGroup.Count() == mappingModelList_ContractNoMatched.Count)
                    {
                        var sellModelList_ContractNoMatched = sellModelGroup.ToList();
                        for (int i = 0; i < sellModelList_ContractNoMatched.Count; i++)
                        {
                            sellModelList_ContractNoMatched[i].SetProductNo(mappingModelList_ContractNoMatched[i].ProductNo, mappingModelList_ContractNoMatched[i].Id);
                            sellModelList_ContractNoMatched[i].Remarks.Add(Remark._2005_UsingPossibleProductNo);
                        }
                    }
                }
            }

            //处理【本期销项明细】【直运】
            {
                // 单项直运
                {
                    var sellModelList_WithProductNo = sellModelList.Where(v => !string.IsNullOrEmpty(v.ProductNo)).ToList();
                    foreach (var sellModel in sellModelList_WithProductNo)
                    {
                        Step1_FindDirectBusiness(sellModel, buyModelList);
                    }
                }

                //多项累加直运
                {
                    var sellModelList_GroupByProductNo = sellModelList.Where(v => !string.IsNullOrEmpty(v.ProductNo) && v.SellMode == SellModelSaleMode.None).GroupBy(v => v.ProductNo).ToList();
                    foreach (var productNo in sellModelList_GroupByProductNo)
                    {
                        Step1_FindDirectBusiness(productNo.Key, productNo.ToList(), buyModelList);
                    }
                }
            }

            // 处理【本期销项明细】【结转前期库存】(在商务报表查找符合条件的)
            {
                var sellModelList2 = sellModelList.Where(v => !string.IsNullOrEmpty(v.ProductNo) && v.SellMode == SellModelSaleMode.None).ToList();
                foreach (var sellModel in sellModelList2)
                {
                    Step2_ProcessJieSuanQianQiKuCun(sellModel, mappingModelList);
                }
            }

            //处理【本期进项明细】【抵消前期暂估】(在商务报表中查找符合条件的)
            {
                var buyModelList2 = buyModelList.Where(v => v.SellMode == BuyModelSaleMode.None).ToList();
                foreach (var buyModel in buyModelList2)
                {
                    Step2_PromotionDiXiaoQianQiZanGu(buyModel, mappingModelList);
                }
            }
        }

        /// <summary>
        /// 从商务报表里面找到比配的记录
        /// </summary>
        /// <param name="sellModel"></param>
        /// <param name="mappingModelList"></param>
        private static void FindMapping(SellModel sellModel, List<MappingModel> mappingModelList)
        {
            var mappings = mappingModelList.Where(v => v.ContractNo == sellModel.XiaoShouContractNo).ToList();

            if (mappings.Count == 1)
            {
                sellModel.SetProductNo(mappings[0].ProductNo, mappings[0].Id);
                return;
            }

            if (mappings.Count > 1)
            {
                sellModel.SetMappingIds(mappings.Select(v => v.Id).ToList());
                sellModel.Remarks.Add(Remark._2002_FindMultiContractNoInMapping);
                return;
            }

            if (mappings.Count == 0)
            {
                sellModel.Remarks.Add(Remark._2001_FindZeroContractNoInMapping);

                if (!sellModel.XiaoShouContractNo.Equals(sellModel.XiaoShouContractNoWithoutLine))
                {
                    var secondMappings = mappingModelList.Where(v => v.ContractNo == sellModel.XiaoShouContractNoWithoutLine).ToList();

                    if (secondMappings.Count == 1)
                    {
                        sellModel.SetProductNo(secondMappings[0].ProductNo, secondMappings[0].Id);
                        sellModel.SetMappingIds(secondMappings.Select(v => v.Id).ToList());
                        sellModel.Remarks.Add(Remark._2003_FindOneContractNoInMappingUsingSecondContractId);
                        return;
                    }

                    if (secondMappings.Count > 1)
                    {
                        sellModel.SetMappingIds(secondMappings.Select(v => v.Id).ToList());
                        sellModel.Remarks.Add(Remark._2004_FindMultiContractNoInMappingUsingSecondContractId);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Step 1. 在本期销项明细表和本期进项明细表中找到直运的记录
        /// </summary>
        /// <param name="sellModel"></param>
        /// <param name="buyModelList"></param>
        private static void Step1_FindDirectBusiness(SellModel sellModel, List<BuyModel> buyModelList)
        {
            //在本期进项明细表中查找货号
            var buyModelList_ProductNoMatched = buyModelList.Where(v => v.ProductNo.Equals(sellModel.ProductNo)).ToList();

            //正好有一个货号匹配的并且产品数相同，则视为“直运”
            if (buyModelList_ProductNoMatched.Count == 1)
            {
                if (buyModelList_ProductNoMatched[0].ReceiveTicketCount == sellModel.ProductCount)
                {
                    var buyModel = buyModelList_ProductNoMatched[0];

                    // 设置销项
                    sellModel.SetSellMode(SellModelSaleMode.DirectBusiness);
                    sellModel.SetCaiGouContractNo(buyModel.BuyContractNo);
                    sellModel.SetCaiGouDanWei(buyModel.XiaoFangMinCheng);
                    sellModel.SetJinXiangShouPiaoDunShu(buyModel.ReceiveTicketCount);
                    sellModel.RelativeBusinessIds.Add(buyModel.Id);

                    //设置进项
                    buyModel.SetSellMode(BuyModelSaleMode.DirectBusiness);
                    buyModel.RelativeBusinessIds.Add(sellModel.BusinessNo);
                }
            }
        }

        private static void Step1_FindDirectBusiness(string productNo, List<SellModel> sellModelList, List<BuyModel> buyModelList)
        {
            //在本期销项明细中查找货号
            var buyModelList_ProductNoMatched = buyModelList.Where(v => v.ProductNo.Equals(productNo) && v.SellMode == BuyModelSaleMode.None).ToList();

            //货物的数目之和相等
            double sellTotalCount = sellModelList.Sum(v => v.ProductCount);
            double buyTotalCount = buyModelList_ProductNoMatched.Sum(v => v.ReceiveTicketCount);
            if (sellTotalCount == buyTotalCount)
            {
                //Set 销项
                List<string> caiGouKuCunList = buyModelList_ProductNoMatched.Select(v => v.BuyContractNo).Distinct().ToList();
                List<string> caiGouDanWeiList = buyModelList_ProductNoMatched.Select(v => v.XiaoFangMinCheng).Distinct().ToList();
                foreach (var sellModel in sellModelList)
                {
                    sellModel.SetSellMode(SellModelSaleMode.DirectBusiness);
                    sellModel.SetKuCun(sellTotalCount - sellModel.ProductCount);
                    sellModel.SetCaiGouContractNo(string.Join(";", caiGouKuCunList));
                    sellModel.SetCaiGouDanWei(string.Join(";", caiGouDanWeiList));
                    sellModel.SetJinXiangShouPiaoDunShu(buyTotalCount);
                    sellModel.RelativeBusinessIds.AddRange(buyModelList_ProductNoMatched.Select(v => v.Id));
                }

                //Set 进项
                foreach (var buyModel in buyModelList_ProductNoMatched)
                {
                    buyModel.SetSellMode(BuyModelSaleMode.DirectBusiness);
                    buyModel.RelativeBusinessIds.AddRange(sellModelList.Select(v => v.BusinessNo));
                }
            }
        }

        /// <summary>
        /// Step 2. 在商务报表查找符合条件的"结转前期库存"
        /// </summary>
        /// <param name="sellModel"></param>
        /// <param name="mappingEntityList"></param>
        private static void Step2_ProcessJieSuanQianQiKuCun(SellModel sellModel, List<MappingModel> mappingModelList)
        {
            //在商务报表中查找货号
            var mappingModelList_Buy = mappingModelList.Where(v => v.ProductNo.Equals(sellModel.ProductNo) && v.MaiMai == 2).ToList();
            var mappingModelList_Sell = mappingModelList.Where(v => v.ProductNo.Equals(sellModel.ProductNo) && v.MaiMai == 1).ToList();

            var buyTotalCount = mappingModelList_Buy.Sum(v => v.KaiChuShouDaoFaPiaoDunShu);
            var sellTotalCount = mappingModelList_Sell.Sum(v => v.KaiChuShouDaoFaPiaoDunShu);

            if (buyTotalCount == sellTotalCount)
            {
                List<string> butContractNoList = mappingModelList_Buy.Select(v => v.ContractNo).Distinct().ToList();
                List<string> gouXiaoHeTongTaiTou = mappingModelList_Buy.Select(v => v.GouXiaoHeTongTaiTou).Distinct().ToList();

                //设置销项
                sellModel.SetSellMode(SellModelSaleMode.JieSuanQianQiKuCun);
                sellModel.SetCaiGouContractNo(string.Join(";", butContractNoList));
                sellModel.SetCaiGouDanWei(string.Join(";", gouXiaoHeTongTaiTou));
                sellModel.SetJinXiangShouPiaoDunShu(buyTotalCount);
                sellModel.SetKuCun(buyTotalCount - sellModel.ProductCount);
                sellModel.RelativeBusinessIds.AddRange(mappingModelList_Buy.Select(v => v.Id));
                sellModel.RelativeBusinessIds.AddRange(mappingModelList_Sell.Select(v => v.Id));
                sellModel.RelativeBusinessIds.RemoveAll(v => sellModel.BusinessNo == v);
            }
        }

        /// <summary>
        /// Step 2. 在商务报表中查找符合条件的"抵消前期暂估"
        /// </summary>
        /// <param name="buyModel"></param>
        /// <param name="mappingModelList"></param>
        private static void Step2_PromotionDiXiaoQianQiZanGu(BuyModel buyModel, List<MappingModel> mappingModelList)
        {
            //在商务报表中查找货号
            var mappingModelList_Buy = mappingModelList.Where(v => v.ProductNo.Equals(buyModel.ProductNo) && v.MaiMai == 2).ToList();
            var mappingModelList_Sell = mappingModelList.Where(v => v.ProductNo.Equals(buyModel.ProductNo) && v.MaiMai == 1 && v.FaPiaoShiJian != DateTime.MinValue && !string.IsNullOrEmpty(v.FaPiaoHaoMa)).ToList();

            var buyTotalCount = mappingModelList_Buy.Sum(v => v.KaiChuShouDaoFaPiaoDunShu);
            var sellTotalCount = mappingModelList_Sell.Sum(v => v.KaiChuShouDaoFaPiaoDunShu);

            if (buyTotalCount == sellTotalCount)
            {
                buyModel.SetSellMode(BuyModelSaleMode.DiXiaoQianQiZanGu);
                buyModel.RelativeBusinessIds.AddRange(mappingModelList_Buy.Select(v => v.Id));
                buyModel.RelativeBusinessIds.AddRange(mappingModelList_Sell.Select(v => v.Id));
                buyModel.RelativeBusinessIds.RemoveAll(v => v == buyModel.Id);
            }
        }

        private static void ToOutputEntity(InputGroup inputGroup, List<OutputBuyEntity> outputBuyEntityList, List<OutputSellEntity> outputSellEntityList)
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

                    sellEntity.BusinessNo = sellModel.BusinessNo;
                    if (sellModel.Remarks.Contains(Remark._2001_FindZeroContractNoInMapping))
                    {
                        sellEntity.Remark2001 = "YES";
                    }
                    if (sellModel.Remarks.Contains(Remark._2002_FindMultiContractNoInMapping))
                    {
                        sellEntity.Remark2002 = "YES";
                    }
                    if (sellModel.Remarks.Contains(Remark._2003_FindOneContractNoInMappingUsingSecondContractId))
                    {
                        sellEntity.Remark2003 = "YES";
                    }
                    if (sellModel.Remarks.Contains(Remark._2004_FindMultiContractNoInMappingUsingSecondContractId))
                    {
                        sellEntity.Remark2004 = "YES";
                    }
                    if (sellModel.Remarks.Contains(Remark._2005_UsingPossibleProductNo))
                    {
                        sellEntity.Remark2005 = "YES";
                    }
                    if (sellModel.Remarks.Contains(Remark._2006_FindMultiProductNoInSell))
                    {
                        sellEntity.Remark2006 = "YES";
                    }
                    if (sellModel.RelativeBusinessIds.Count > 0)
                    {
                        sellEntity.RelativeBusinessIds = string.Join(";", sellModel.RelativeBusinessIds);
                    }

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

                    if (buyModel.RelativeBusinessIds.Count > 0)
                    {
                        buyEntity.RelativeBusinessIds = string.Join(";", buyModel.RelativeBusinessIds);
                    }

                    outputBuyEntityList.Add(buyEntity);
                }
            }
        }

        private static string ToSellMode(BuyModelSaleMode sellMode)
        {
            switch (sellMode)
            {
                case BuyModelSaleMode.DirectBusiness:
                    return "直运";
                case BuyModelSaleMode.DiXiaoQianQiZanGu:
                    return "抵消前期暂估";
                default:
                    return null;
            }
        }

        private static string ToSellMode(SellModelSaleMode sellMode)
        {
            switch (sellMode)
            {
                case SellModelSaleMode.DirectBusiness:
                    return "直运";
                case SellModelSaleMode.JieSuanQianQiKuCun:
                    return "结转前期库存";
                default:
                    return null;
            }
        }
    }
}
