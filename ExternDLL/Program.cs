using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ExternDLL
{
    [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
             * - Wawi Installationspfad finden -> Registry 
             * - JTLwawiExtern finden und Version prüfen
             * - ALLE JTL Assemblys aus dem Installationspfad laden
             */
            ValidateAssembly validateAssembly = new ValidateAssembly();
            if (!validateAssembly.IsValid)
                return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
