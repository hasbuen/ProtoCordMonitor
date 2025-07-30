namespace ProtoCord_Monitor
{
    partial class Configuracao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuracao));
            this.cbDesativarAberturaAutomaticaDeLink = new System.Windows.Forms.CheckBox();
            this.cbSolicitarConfirmacaoAntesDeAbrirLink = new System.Windows.Forms.CheckBox();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbDesativarAberturaAutomaticaDeLink
            // 
            this.cbDesativarAberturaAutomaticaDeLink.AutoSize = true;
            this.cbDesativarAberturaAutomaticaDeLink.Location = new System.Drawing.Point(12, 12);
            this.cbDesativarAberturaAutomaticaDeLink.Name = "cbDesativarAberturaAutomaticaDeLink";
            this.cbDesativarAberturaAutomaticaDeLink.Size = new System.Drawing.Size(379, 17);
            this.cbDesativarAberturaAutomaticaDeLink.TabIndex = 0;
            this.cbDesativarAberturaAutomaticaDeLink.Text = "Desativar abertura automática de link em nova aba ao receber notificação.";
            this.cbDesativarAberturaAutomaticaDeLink.UseVisualStyleBackColor = true;
            // 
            // cbSolicitarConfirmacaoAntesDeAbrirLink
            // 
            this.cbSolicitarConfirmacaoAntesDeAbrirLink.AutoSize = true;
            this.cbSolicitarConfirmacaoAntesDeAbrirLink.Location = new System.Drawing.Point(12, 35);
            this.cbSolicitarConfirmacaoAntesDeAbrirLink.Name = "cbSolicitarConfirmacaoAntesDeAbrirLink";
            this.cbSolicitarConfirmacaoAntesDeAbrirLink.Size = new System.Drawing.Size(356, 17);
            this.cbSolicitarConfirmacaoAntesDeAbrirLink.TabIndex = 1;
            this.cbSolicitarConfirmacaoAntesDeAbrirLink.Text = "Ativar pergunta de confirmação antes de abrir o link automaticamente.";
            this.cbSolicitarConfirmacaoAntesDeAbrirLink.UseVisualStyleBackColor = true;
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Location = new System.Drawing.Point(320, 68);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(75, 23);
            this.btnConfirmar.TabIndex = 2;
            this.btnConfirmar.Text = "Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // Configuracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 103);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.cbSolicitarConfirmacaoAntesDeAbrirLink);
            this.Controls.Add(this.cbDesativarAberturaAutomaticaDeLink);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Configuracao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuracões";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbDesativarAberturaAutomaticaDeLink;
        private System.Windows.Forms.CheckBox cbSolicitarConfirmacaoAntesDeAbrirLink;
        private System.Windows.Forms.Button btnConfirmar;
    }
}