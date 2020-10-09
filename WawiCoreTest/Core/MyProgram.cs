using System;
using JTLwawiExtern;

namespace WawiCoreTest.Core
{
    internal class MyProgram
    {
        public void Run(string[] strings)
        {
            var core = new WawiCore();
            try
            {
                // Initialisierung vom JTL-Wawi-Core. WIr können hier das Handle eines "Hauptfensters" übergeben. Die
                // modalen Dialoge, die wir dann über die JTLWawiExtern.dll öffnen sollten, nutzen dieses Handle dann
                // als Vater-Fenster. 
                // Hier kann grundsätzlich eine 0 übergeben werden, dann stimmt halt im schlimmsten Fall die modale Anordnung
                // nicht, und Fenster erscheinen im Hintergrund. 0 ist auch der Wert für nicht-UI-Programme.
                core.Initialize(0); // Initialisierung vom JTL-Wawi-Core

                // Es gibt verschiedene Login-Methoden im Core. Wichtig ist nur, dass ein Login durchgeführt wird.

                core.Login(@"Standard", "eB-Standard", new WawiCredentials
                                                       {
                                                           Username = "admin",
                                                           Password = "pass"
                                                       }); // Login ohne UI-Interaktion

                var stockItem = new StockItem
                                {
                                    BatchNumber = null,
                                    BestBeforeDate = null,
                                    BinLocationInternalId = 1,
                                    DeliveryDate = DateTimeOffset.Now,
                                    fEKNetto = 12.00m,
                                    ProductInternalId = 2,
                                    Quantity = 5.00m,
                                    Remark = "Korrekturbuchung vom 12. Januar 2020"
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
            public string SerialNumber { get; set; }
            public int ProductInternalId { get; set; }
            public decimal Quantity { get; set; }
            public string BatchNumber { get; set; }
            public DateTimeOffset? BestBeforeDate { get; set; }
            public int BinLocationInternalId { get; set; }
            public DateTimeOffset? DeliveryDate { get; set; }
            public decimal fEKNetto { get; set; }
            public string Remark { get; set; }
        }
    }
}