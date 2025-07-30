using Newtonsoft.Json;
using Supabase.Postgrest;
using Supabase;
using Supabase.Realtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using config;
using ProtoCord_Monitor;


namespace ProtoCordMonitor
{
    public partial class Form : System.Windows.Forms.Form
    {
        private readonly ContextMenuStrip menuTray;
        private readonly ToolStripMenuItem desligarItem;

        private readonly string url = Config.GetUrl();
        private readonly string ws = Config.GetWs();
        private readonly string senha = Config.GetSenha();
        private readonly string apiKey = Config.GetApiKey();
        public Form()
        {
            InitializeComponent();

            // Ícone da bandeja
            string caminhoIcone = Path.Combine(Application.StartupPath, "Assets", "protocord.ico");
            notifyIcon1.Icon = new Icon(caminhoIcone);
            notifyIcon1.Text = "ProtoCord Monitor";
            notifyIcon1.Visible = true;

            // Menu de contexto
            menuTray = new ContextMenuStrip();

            var configuracaoItem = new ToolStripMenuItem("Configurações");
            configuracaoItem.Click += (sender, e) =>
            {
                using (var formConfig = new Configuracao())
                {
                    formConfig.ShowDialog();
                }
            };
            menuTray.Items.Add(configuracaoItem);


            desligarItem = new ToolStripMenuItem("Desligar");
            desligarItem.Click += DesligarItem_Click;
            menuTray.Items.Add(desligarItem);

            notifyIcon1.ContextMenuStrip = menuTray;
            notifyIcon1.ShowBalloonTip(3000, "ProtoCord", "Monitor iniciado!", ToolTipIcon.Info);

            // Janela oculta
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            IniciarEscutaRealtime();
        }

        private void DesligarItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit(); // Encerra o programa
        }
        private async void IniciarEscutaRealtime()
        {
            var realtimeUrl = ws + apiKey;


            var client = new Supabase.Client(
                "https://zygbtkcmdnkyqldezknz.supabase.co",
                apiKey
            );

            await client.InitializeAsync();

            // 2. Faz login
            var session = await client.Auth.SignIn("juliocesar.ovidiobueno@outlook.com", senha);
            var jwt = session.AccessToken;

            // 3. Usa o JWT na Realtime.Client
            var options = new Supabase.Realtime.ClientOptions
            {
                Parameters = new Supabase.Realtime.Socket.SocketOptionsParameters
                {
                    ApiKey = apiKey
                }
            };

            var realtimeClient = new Supabase.Realtime.Client(
                ws, options
            );

            realtimeClient.SetAuth(jwt);
            await realtimeClient.ConnectAsync();


            var canal = realtimeClient.Channel("realtime", "public", "notificacao");

            canal.AddPostgresChangeHandler(Supabase.Realtime.PostgresChanges.PostgresChangesOptions.ListenType.Inserts, async (_, __) =>
            {
                await VerificarAPI();
            });

            await canal.Subscribe();
        }

        public async Task VerificarAPI()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                    client.DefaultRequestHeaders.Add("apikey", apiKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string endpoint = "/rest/v1/notificacao?select=*";
                    var response = await client.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var registros = JsonConvert.DeserializeObject<List<Registro>>(json);

                    if (registros.Count == 0)
                        return;

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

                        string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "params.ini");
                        IniFile ini = new IniFile(iniPath);

                        // Carrega os valores
                        bool desativarLinkAuto = bool.TryParse(ini.Read("Notificacoes", "DesativarAberturaAutomaticaDeLink"), out bool result1) ? result1 : false;
                        bool solicitarConfirmacao = bool.TryParse(ini.Read("Notificacoes", "SolicitarConfirmacaoAntesDeAbrirLink"), out bool result2) ? result2 : false;
                        Console.WriteLine("Valor do parametro desativarLinkAuto:   " + desativarLinkAuto);
                        Console.WriteLine("Valor do parametro solicitarConfirmacao:   " + solicitarConfirmacao);
                        if (desativarLinkAuto)
                        {
                            if (registro.operacao == 0 && !string.IsNullOrEmpty(registro.link))
                            {
                                bool podeAbrir = true;

                                if (solicitarConfirmacao)
                                {
                                    var resultado = MessageBox.Show(
                                        $"Deseja abrir o link do ticket que gerou o protocolo {registro.prt}?",
                                        "Confirmar abertura de link",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question
                                    );

                                    podeAbrir = (resultado == DialogResult.Yes);
                                }

                                if (podeAbrir)
                                {
                                    Process.Start(registro.link);
                                }
                            }
                        }
                    }

                    // Aguarda 5 segundos antes de limpar
                    await Task.Delay(5000);

                    // Chamada única à rota de limpeza
                    using (HttpClient limparClient = new HttpClient())
                    {
                        var limparResponse = await limparClient.GetAsync("https://modelo-discord-server.vercel.app/api/limpar_notificacao");
                        if (limparResponse.IsSuccessStatusCode)
                            Console.WriteLine("Notificações foram limpas com sucesso!");
                        else
                            Console.WriteLine("Erro ao limpar notificações: " + limparResponse.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao acessar Supabase: " + ex.Message);
            }
        }
    }
}