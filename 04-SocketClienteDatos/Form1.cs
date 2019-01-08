using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;


/*
 *DATOS SOCKET - CLIENTE
 */

namespace _04_SocketClienteDatos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Socket misocketCliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miServidor = new IPEndPoint(IPAddress.Parse(this.textBox1.Text), 2000);
            try
            {
                misocketCliente.Connect(miServidor);
                Debug.WriteLine("Conectado con éxito.");

                // Envío de datos:
                byte[] textoEnviar;
                textoEnviar = Encoding.Default.GetBytes(this.textBox2.Text);
                misocketCliente.Send(textoEnviar, 0, textoEnviar.Length, SocketFlags.None);
                Debug.WriteLine("Texto Enviado Exitosamente");

                // Cerrar:
                misocketCliente.Close();
            }
            catch(Exception error)
            {
                Debug.WriteLine("Error: " + error.ToString());
            }
        }
    }
}
