﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.InputEntity
{
    [ExcelTable("商务报表")]
    class MappingEntity
    {
        /// <summary>
        /// 数据库编号
        /// </summary>
        [ExcelColumn("数据库编号", 1)]
        public string Id { get; set; }

        /// <summary>
        /// 合同状态
        /// </summary>
        [ExcelColumn("合同状态", 5)]
        public string ContractStatus { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        [ExcelColumn("组别", 9)]
        public string GroupCategory { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        [ExcelColumn("申请人", 10)]
        public string Applicant { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        [ExcelColumn("货号", 12)]
        public string ProductNo { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [ExcelColumn("合同编号", 13)]
        public string ContractNo { get; set; }

        /// <summary>
        /// 购销合同抬头
        /// </summary>
        [ExcelColumn("购销合同抬头", 14)]
        public string GouXiaoHeTongTaiTou { get; set; }

        /// <summary>
        /// 买/卖
        /// </summary>
        [ExcelColumn("卖/买", 15)]
        public string MaiMai { get; set; }

        /// <summary>
        /// 成交吨数
        /// </summary>
        //[ExcelColumn("成交吨数", 16)]
        //public double ChengJiaoDunShu { get; set; }

        [ExcelColumn("开出/收到发票时间", 42)]
        public DateTime FaPiaoShiJian { get; set; }

        /// <summary>
        /// 开出/收到发票吨数
        /// </summary>
        [ExcelColumn("开出/收到发票吨数", 44)]
        public double KaiChuShouDaoFaPiaoDunShu { get; set; }

        [ExcelColumn("发票号码", 46)]
        public string FaPiaoHaoMa { get; set; }

    }
}
