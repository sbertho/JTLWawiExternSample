using System;
using JTLwawiExtern;

namespace WawiCoreTest
{
    internal class MyProgram
    {
        public void Run()
        {
            var core = new WawiCore();
            try
            {
                core.Initialize(0); // Initialisierung vom JTL-Wawi-Core
                core.Login(@"Standard", "eB-Standard", "admin", "pass", false); // Login vom Wawi-Nutzer

                var stockItem = new StockItem
                                {
                                    BatchNumber = null,
                                    BestBeforeDate = null,
                                    BinLocationId = 12,
                                    DeliveryDate = DateTimeOffset.Now,
                                    fEKNetto = 12.00m,
                                    ProductInternalId = 12,
                                    Quantity = 5.00m,
                                    Remark = "Korrekturbuchung vom 12. Januar 2020",
                                    SerialNumber = null
                                };

                core.Stock.Import(new IStockItem[] {stockItem});
            }
            finally
            {
                Console.WriteLine("Shutdown vom JTL-Wawi-Core...");
                while (!core.CanShutDown())
                {
                    core.TryShutDown(TimeSpan.FromMinutes(5));
                }
            }
        }


        private class StockItem : IStockItem
        {
            public int ProductInternalId { get; set; }
            public decimal Quantity { get; set; }
            public string BatchNumber { get; set; }
            public DateTimeOffset? BestBeforeDate { get; set; }
            public string SerialNumber { get; set; }
            public int WarehouseInternalId { get; set; }
            public int BinLocationId { get; set; }
            public DateTimeOffset? DeliveryDate { get; set; }
            public decimal fEKNetto { get; set; }
            public string Remark { get; set; }
        }
    }
}