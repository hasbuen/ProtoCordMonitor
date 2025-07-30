using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProtoCordMonitor
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {

            {
                // 👉 Redireciona busca de DLLs para a subpasta "libs"
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    string dllName = new AssemblyName(args.Name).Name + ".dll";
                    string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libs", dllName);
                    return File.Exists(dllPath) ? Assembly.LoadFrom(dllPath) : null;
                };

                bool instanciaUnica;
                using (Mutex mutex = new Mutex(true, "ProtoCordMonitorAppMutex", out instanciaUnica))
                {
                    if (instanciaUnica)
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form());
                    }
                    else
                    {
                        MessageBox.Show("O ProtoCord Monitor já está em execução.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
