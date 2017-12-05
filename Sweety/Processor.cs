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
            List<BuyEntity> buyEntityList = ExcelHelper.ReadFromExcel<BuyEntity>(inputFilePath);
            List<SellEntity> sellEntityList = ExcelHelper.ReadFromExcel<SellEntity>(inputFilePath);
            List<BusinessEntity> mappingEntityList = ExcelHelper.ReadFromExcel<BusinessEntity>(inputFilePath);

            InputGroup inputGroup = ToInputGroup(buyEntityList, sellEntityList, mappingEntityList);

            OutputGroup outputGroup = Process(inputGroup);

            List<OutputBuyEntity> outputBuyEntityList = new List<OutputBuyEntity>();
            List<OutputSellEntity> outputSellEntityList = new List<OutputSellEntity>();

            ToEntity(outputGroup, outputBuyEntityList, outputSellEntityList);

            ExcelHelper.WriteToExcel(outputFilePath, outputBuyEntityList);
            ExcelHelper.WriteToExcel(outputFilePath, outputSellEntityList);
        }


        private static InputGroup ToInputGroup(List<BuyEntity> buyEntityList, List<SellEntity> sellEntityList, List<BusinessEntity> mappingEntityList)
        {
            InputGroup returnValue = new InputGroup();


            return returnValue;
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
