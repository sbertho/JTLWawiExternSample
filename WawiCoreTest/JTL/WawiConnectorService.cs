using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;

namespace WawiCoreTest.JTL
{
    /// <summary>
    ///     .NET sucht Assemblys nur im jeweils aktuellen Verzeichnis und im GAC. Wir laden einen mehr oder weniger
    ///     vollständigen
    ///     Wawi-Prozess in unseren Prozess, d.h. die JTLWawiExtern.dll lädt weitere Assemblys nach, die aber im
    ///     JTL-Wawi-Programmverzeichnis
    ///     liegen. Deshalb biegen wir hier das AssemblyResolve-Event um, um auch diese Assemblys fehlerfrei laden zu können.
    ///     Wenn die Anwendung im selben Verzeichnis wie die JTL-Wawi liegt, ist das Vorgehen nicht notwendig.
    /// </summary>
    internal static class WawiConnectorService
    {
        public static string WawiInstallLocation { get; private set; }

        public static void Connect(Version minimalExternDllVersion)
        {
            if (!string.IsNullOrEmpty(WawiInstallLocation))
            {
                return;
            }

            WawiInstallLocation = FindInstallLocation();

            if (string.IsNullOrEmpty(WawiInstallLocation))
            {
                throw new WawiNotFoundException();
            }

            var externDllPath = Path.Combine(WawiInstallLocation, @"JTLwawiExtern.dll");
            var externDllVersion = new Version(FileVersionInfo.GetVersionInfo(externDllPath).FileVersion);
            if (externDllVersion < minimalExternDllVersion)
            {
                throw new InvalidWawiVersionException(minimalExternDllVersion, externDllVersion);
            }

            AppDomain.CurrentDomain.AssemblyResolve -= LoadAssemblys;
            AppDomain.CurrentDomain.AssemblyResolve += LoadAssemblys;
        }

        /// <summary>
        ///     Sucht in der Registry nach einem Uninstall-Subkey für die Wawi
        /// </summary>
        /// <param name="baseKey">Basis-Registry-Zweig (für 32bit oder 64bit-Systeme)</param>
        /// <returns>Der Pfad zur Installation der Wawi</returns>
        private static string FindUninstallSubkey(string baseKey)
        {
            var registryKey = Registry.LocalMachine.OpenSubKey(baseKey);

            return
                registryKey?.GetSubKeyNames()
                            .Select(registryKey.OpenSubKey)
                            .Where(subkey => Equals(@"JTL-Wawi", subkey.GetValue(@"DisplayName")))
                            .Select(subkey => Convert.ToString(subkey.GetValue(@"InstallLocation")))
                            .FirstOrDefault();
        }

        /// <summary>
        ///     Sucht der Registry (sowohl 32bit als auch 64bit) nach dem Installationsort für die Wawi
        /// </summary>
        /// <returns>Wawi-Installationsort</returns>
        private static string FindInstallLocation()
        {
            var cLocation =
                FindUninstallSubkey(@"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"); //32bit
            return !string.IsNullOrEmpty(cLocation)
                ? cLocation
                : FindUninstallSubkey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
        }


        private static Assembly LoadAssemblys(object sender, ResolveEventArgs args)
        {
            var folderPath = Path.GetDirectoryName(WawiInstallLocation);
            if (string.IsNullOrEmpty(folderPath))
            {
                throw new InvalidOperationException("Wawi installation path is gone.");
            }

            var assemblyPath = Path.Combine(folderPath, $@"{new AssemblyName(args.Name).Name}.dll");
            if (!File.Exists(assemblyPath))
            {
                return null;
            }

            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
    }
}