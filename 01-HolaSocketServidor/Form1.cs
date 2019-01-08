using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 *HOLA SOCKET - SERVIDOR
 */

namespace _01_HolaSocketServidor
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

                // Linea bloqueante (paraliza el programa):
                Socket cliente = miSocketServidor.Accept();

                Debug.WriteLine("Conexion exitosa del cliente: " + cliente.RemoteEndPoint.ToString());

                miSocketServidor.Close();
            }
            catch (Exception error)
            {
                Debug.WriteLine("Error : {0}", error.ToString());
            }
        }
    }
}
