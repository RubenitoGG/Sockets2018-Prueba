using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace _05_ServidorTCPStream
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data;
            TcpListener newSocket = new TcpListener(IPAddress.Any, 2000);
            newSocket.Start();

            // Linea bloqueante:
            Debug.WriteLine("Esperando cliente");
            TcpClient cliente = newSocket.AcceptTcpClient();
            
            // Creamos comunicación:
            NetworkStream netStream = cliente.GetStream();      // Crea canal de comunicaciones.
            StreamReader reader = new StreamReader(netStream);  // Canal en una dirección.
            StreamWriter writer = new StreamWriter(netStream);  // Canal en otra dirección.

            // Enviamos mensaje:
            writer.WriteLine("Bienvenido");
            writer.Flush(); // Garantizamos que lo envía, si no, lo envía cuando pueda. Paraliza el programa hasta que se envía.

            this.label1.Text = "";

            while (true)
            {
                try
                {
                    // Linea bloqueante:
                    data = reader.ReadLine();

                    //Escribimos en la etiqueta:
                    this.label1.Text += data + System.Environment.NewLine;
                    this.label1.Refresh();

                    // Efecto eco (mandar al cliente lo que nos mandó):
                    writer.WriteLine("#" + data + "#");
                    writer.Flush();

                    //Salimos del bucle:
                    if (data == "exit")
                        break;
                }
                catch (Exception error)
                {
                    Debug.WriteLine("Error: " + error.ToString());
                }
            }

            // Cerramos todo:
            netStream.Close();
            cliente.Close();
            newSocket.Stop();
        }
    }
}
