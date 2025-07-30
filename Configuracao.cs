using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProtoCord_Monitor
{
    public partial class Configuracao : Form
    {
        public Configuracao()
        {
            InitializeComponent();
            this.Load += Configuracao_Load;
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "params.ini");

            IniFile iniInicial = new IniFile(iniPath);

            iniInicial.Write("Notificacoes", "DesativarAberturaAutomaticaDeLink", cbDesativarAberturaAutomaticaDeLink.Checked.ToString());
            iniInicial.Write("Notificacoes", "SolicitarConfirmacaoAntesDeAbrirLink", cbSolicitarConfirmacaoAntesDeAbrirLink.Checked.ToString());

            MessageBox.Show("Configurações salvas com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (!File.Exists(iniPath))
            {
                File.WriteAllText(iniPath, "[Notificacoes]\nDesativarAberturaAutomaticaDeLink=false\nSolicitarConfirmacaoAntesDeAbrirLink=false");
            }

            this.Close();
        }

        private void Configuracao_Load(object sender, EventArgs e)
        {
            string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "params.ini");

            if (File.Exists(iniPath))
            {
                IniFile iniCarregado = new IniFile(iniPath);

                string valorAuto = iniCarregado.Read("Notificacoes", "DesativarAberturaAutomaticaDeLink");
                string valorConfirmacao = iniCarregado.Read("Notificacoes", "SolicitarConfirmacaoAntesDeAbrirLink");

                cbDesativarAberturaAutomaticaDeLink.Checked = valorAuto.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
                cbSolicitarConfirmacaoAntesDeAbrirLink.Checked = valorConfirmacao.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            }
        }


    }
}
