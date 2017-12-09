using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    /// <summary>
    /// 销项明细
    /// </summary>
    class SellModel
    {
        public string SellContractNo { get; set; }
        public string BuProductOrganization { get; set; }
        public string ProductName { get; set; }
        public float ProductCount { get; set; }
        public float ProductUnitPriceWithTax { get; set; }
        public float ProductMoneyWithTax { get; set; }
        public string BillNo { get; set; }

    }
}
