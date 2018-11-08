namespace _01_HolaSocketServidor
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConectarServidor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConectarServidor
            // 
            this.ConectarServidor.Location = new System.Drawing.Point(12, 12);
            this.ConectarServidor.Name = "ConectarServidor";
            this.ConectarServidor.Size = new System.Drawing.Size(199, 98);
            this.ConectarServidor.TabIndex = 0;
            this.ConectarServidor.Text = "Concetar Servidor";
            this.ConectarServidor.UseVisualStyleBackColor = true;
            this.ConectarServidor.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ConectarServidor);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ConectarServidor;
    }
}

