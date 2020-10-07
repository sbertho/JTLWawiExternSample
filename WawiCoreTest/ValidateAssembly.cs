using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;

namespace WawiCoreTest
{
    public class ValidateAssembly
    {
        private string _wawiPath;

        public ValidateAssembly()
        {
            IsValid = false;
            ValidateAndLoadAssemblys();
        }

        public bool IsValid { get; private set; }

        private void ValidateAndLoadAssemblys()
        {
            this._wawiPath = FindInstallLocation();
            if (!string.IsNullOrWhiteSpace(this._wawiPath) && ValidExternDllVersion())
            {
                IsValid = true;
                AppDomain.CurrentDomain.AssemblyResolve += LoadAssemblys;
            }
            else
            {
                IsValid = false;
            }
        }

        /// <summary>
        ///     Sucht in der Registry nach einem Uninstall-Subkey für die Wawi
        /// </summary>
        /// <param name="baseKey">Basis-Registry-Zweig (für 32bit oder 64bit-Systeme)</param>
        /// <returns>Der Pfad zur Installation der Wawi</returns>
        private string FindUninstallSubkey(string baseKey)
        {
            var oKey = Registry.LocalMachine.OpenSubKey(baseKey);
            if (oKey == null)
            {
                return null;
            }

            return
                oKey.GetSubKeyNames()
                    .Select(oKey.OpenSubKey)
                    .Where(oSubKey => Equals("JTL-Wawi", oSubKey.GetValue("DisplayName")))
                    .Select(oSubKey => Convert.ToString(oSubKey.GetValue("InstallLocation")))
                    .FirstOrDefault();
        }

        /// <summary>
        ///     Sucht der Registry (sowohl 32bit als auch 64bit) nach dem Installationsort für die Wawi
        /// </summary>
        /// <returns>Wawi-Installationsort</returns>
        private string FindInstallLocation()
        {
            var cLocation =
                FindUninstallSubkey(@"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"); //32bit
            if (!string.IsNullOrEmpty(cLocation))
            {
                return cLocation;
            }

            return FindUninstallSubkey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall"); //64bit
        }

        private bool ValidExternDllVersion()
        {
            var externDllPath = Path.Combine(this._wawiPath, "JTLwawiExtern.dll");
            var externDllVersion = new Version(FileVersionInfo.GetVersionInfo(externDllPath).FileVersion);
            var eingebundeneExternDllVersion = new Version(1, 1, 0, 11); //Eingebette Version manuell pflegen
            if (externDllVersion < eingebundeneExternDllVersion)
            {
                return false;
            }

            return true;
        }

        private Assembly LoadAssemblys(object sender, ResolveEventArgs args)
        {
            var folderPath = Path.GetDirectoryName(this._wawiPath);
            var assemblyPath = Path.Combine(folderPath, string.Format("{0}.dll", new AssemblyName(args.Name).Name));
            if (!File.Exists(assemblyPath))
            {
                return null;
            }

            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
    }
}