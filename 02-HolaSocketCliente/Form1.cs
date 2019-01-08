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
 *HOLA SOCKET - CLIENTE
 */

namespace _02_HolaSocketCliente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ConectarServido_Click(object sender, EventArgs e)
        {
            Socket miSocketCliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint servidor = new IPEndPoint(IPAddress.Parse(this.textBox1.Text), 2000);

            try
            {
                miSocketCliente.Connect(servidor);
                Debug.WriteLine("Conectado con éxito.");

                miSocketCliente.Close(); // No sería necesario ya que cerramos el servidor.
            }
            catch (Exception error)
            {
                Debug.WriteLine("Error: {0}", error.ToString());
            }
        }
    }
}
