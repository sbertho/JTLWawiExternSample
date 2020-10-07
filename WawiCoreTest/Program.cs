using System;

namespace WawiCoreTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var validateAssembly = new ValidateAssembly();
            if (!validateAssembly.IsValid)
            {
                Console.WriteLine("Problem: Keine installierte JTL-Wawi Version gefunden.");
                return;
            }

            // Da wir die Assembly-Auflösung umbiegen, darf die Startassembly keinen Verweis auf die
            // JTLWawiExtern.dll haben. Deshalb lagern wir den Kram in eine eigene Klasse aus.
            var program = new MyProgram();
            program.Run();
        }
    }
}