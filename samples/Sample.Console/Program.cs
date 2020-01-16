using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sample.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var data = ExcelHelper.ExcelToDataTable(@"C:\Users\75795\Desktop\test.xlsx", "Sheet1", true);

            #region 分析是否为散落sku

            //var result = data.Clone();
            //var list = ExcelHelper.ToDataList<Model>(data);
            //var sgus = list.Select(x => x.Sgu).Distinct().ToList();
            //foreach (var sgu in sgus)
            //{
            //    var items = list.Where(x => x.Sgu == sgu).ToList();
            //    var itemIds = items.Where(x => x.ItemId != "#N/A").Select(x => x.ItemId).Distinct().ToList();
            //    var valid = itemIds.Count() > 1 ? "FALSE" : "TRUE";
            //    items.ForEach(x =>
            //    {
            //        x.Valid = valid;
            //    });
            //}
            //result = ExcelHelper.ToDataTableTow(list);
            //ExcelHelper.GridToExcelByNPOI(result, @"C:\Users\75795\Desktop\test1.xlsx");

            #endregion 分析是否为散落sku

            #region 散落统计

            //var list = ExcelHelper.ToDataList<Model>(data);
            //var types = list.Select(x => x.Type).Distinct().ToList();
            //var resultData = new List<StatisticsModel>();
            //foreach (var type in types)
            //{
            //    var items = list.Where(x => x.Type == type).ToList();
            //    var falseCount = items.Count(x => x.Valid == "FALSE");
            //    var model = new StatisticsModel
            //    {
            //        Type = type,
            //        Scattered = ((decimal)falseCount / items.Count() * 100).ToString("0.00") + "%"
            //    };
            //    resultData.Add(model);
            //}
            //var result = ExcelHelper.ToDataTableTow(resultData);
            //ExcelHelper.GridToExcelByNPOI(result, @"C:\Users\75795\Desktop\test1.xlsx");

            #endregion 散落统计

            //记录开始时间

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            int num = 100000000;
            for (int i = 0; i < num; i++)
            {
                BubbleSort(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            }

            stopwatch.Stop();
            Console.WriteLine((stopwatch.ElapsedMilliseconds / 1000m).ToString("0.000") + "s");
            Console.ReadKey();
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public static void BubbleSort(int[] arr)
        {
            var len = arr.Length;
            for (int i = 0; i < len - 1; i++)
            {
                for (int j = 0; j < len - 1 - i; j++)
                {
                    if (arr[j] < arr[j + 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
        }
    }

    public class StatisticsModel
    {
        public string Type { get; set; }

        public string Scattered { get; set; }
    }

    public class Model
    {
        public string Sgu { get; set; }

        public string Sku { get; set; }

        public string ItemId { get; set; }

        public string Valid { get; set; }

        public string Type { get; set; }
    }
}