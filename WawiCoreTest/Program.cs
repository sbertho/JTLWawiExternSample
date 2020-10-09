using System;
using WawiCoreTest.Core;
using WawiCoreTest.JTL;

namespace WawiCoreTest
{
    /// <summary>
    ///     Wir nutzen die JTLWawiExtern.dll aus der JTL-Wawi Installation. Damit die Assemblys der JTL-Wawi nachgeladen
    ///     werden können, müssen wir dem AssemblyResolver sagen, dass er auch im Verzeichnis der JTL-Wawi suchen muss.
    ///     Das übernimmt der WawiConnectorService. Allerdings darf dann in allen Klassen, die *vor* dem Aufruf des
    ///     WawiConnectorService *initialisiert* werden (also insbesondere statische Klassen und die Hauptklasse) keine
    ///     Referenz auf die JTLWawiExtern.dll enthalten sein.
    ///     Deshalb gehen wir hier den Weg über eine zweite Klasse, in der die eigentliche Programmlogik liegt.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            WawiConnectorService.Connect(new Version(1, 5, 10, 0));

            var program = new MyProgram();
            program.Run(args);
        }
    }
}