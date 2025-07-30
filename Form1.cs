using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProtoCordMonitor
{
    public partial class Form1 : Form
    {
        List<string> idsAnteriores = new List<string>();
        List<Registro> registrosAnteriores = new List<Registro>();

        System.Timers.Timer timer;

        public Form1()
        {
            InitializeComponent();

            notifyIcon1.Icon = SystemIcons.Information;
            notifyIcon1.Text = "Monitor ProtoCord";
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(3000, "ProtoCord", "Monitor iniciado!", ToolTipIcon.Info);

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            IniciarMonitoramento();
        }

        private void IniciarMonitoramento()
        {
            timer = new System.Timers.Timer(600000); // 10 min
            timer.Elapsed += async (s, e) => await VerificarAPI();
            timer.Start();
        }

        private async Task VerificarAPI()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = await client.GetStringAsync("https://modelo-discord-server.vercel.app/api/notificacao");
                    var registros = JsonConvert.DeserializeObject<List<Registro>>(json);

                    foreach (var registro in registros)
                    {
                        string tipoTexto = registro.tipo == "0" ? "erro!" :
                                           registro.tipo == "1" ? "sugestão!" : "INDEFINIDO";

                        string titulo = registro.operacao == 0
                            ? $"Novo Protocolo de {tipoTexto}"
                            : "Protocolo Removido";

                        string mensagem = registro.operacao == 0
                            ? registro.prt
                            : $"Protocolo {registro.prt} foi excluído.";

                        ToolTipIcon icone = registro.operacao == 0
                            ? ToolTipIcon.Info
                            : ToolTipIcon.Warning;

                        notifyIcon1.ShowBalloonTip(6000, titulo, mensagem, icone);

                        if (registro.operacao == 0)
                        {
                            Process.Start(registro.link);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao consultar API: " + ex.Message);
            }
        }


    }
}