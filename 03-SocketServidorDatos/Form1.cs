using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 *DATOS SOCKET - SERVIDOR
 */

namespace _03_SocketServidorDatos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Socket miSocketServidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccionEscucha = new IPEndPoint(IPAddress.Any, 2000);
            try
            {
                miSocketServidor.Bind(miDireccionEscucha);
                miSocketServidor.Listen(1);

                // Linea bloqueante:
                Socket cliente = miSocketServidor.Accept();
                Debug.WriteLine("Conexion exitosa con del cliente: " + cliente.RemoteEndPoint.ToString());

                // Recepción de datos:
                byte[] bytesRecibidos = new byte[1000];
                int datos = cliente.Receive(bytesRecibidos, 0, bytesRecibidos.Length, SocketFlags.None);
                this.label1.Text = Encoding.Default.GetString(bytesRecibidos, 0, datos);

                // Cerramos cliente:
                cliente.Close();
                // Cerramos servidor:
                miSocketServidor.Close();
            }
            catch (Exception error)
            {
                Debug.WriteLine("Error: " + error.ToString());
            }
        }
    }
}
