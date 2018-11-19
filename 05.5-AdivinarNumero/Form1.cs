using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _05._5_AdivinarNumero
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
            NetworkStream netStream = cliente.GetStream();
            StreamReader reader = new StreamReader(netStream);
            StreamWriter writer = new StreamWriter(netStream);

            // Enviamos mensaje:
            writer.WriteLine("Adivina un número:");
            writer.Flush();

            // Restauramos las etiquetas:
            this.label2.Text = "";
            this.label3.Text = "";
            this.label4.Text = "?";

            // Calculamos el número aleatorio:
            Random r = new Random();
            int numero = r.Next(0, 10);

            while (true)
            {
                try
                {
                    while(true)
                    {
                        // Linea bloqueante:
                        data = reader.ReadLine();

                        // Comprobamos el número:
                        if (Convert.ToInt32(data) == numero)
                        {
                            label4.Text = numero.ToString();
                            label2.Text = "ACIERTO";
                            break;
                        }
                        else
                        {
                            label3.Text += ", " + numero;
                        }
                    }
                }
                catch (Exception error)
                {
                    Debug.Write("Error: " + error.ToString());
                }
            }

            // Cerramos todo:
            netStream.Close();
            cliente.Close();
            newSocket.Stop();
        }
    }
}
