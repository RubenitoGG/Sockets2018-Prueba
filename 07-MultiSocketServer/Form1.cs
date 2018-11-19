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
using System.Threading;

namespace _07_MultiSocketServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ManejarCliente(TcpClient cliente)
        {
            // Creamos variables para comunicarnos con el cliente:
            string data;
            NetworkStream netStream = cliente.GetStream();
            StreamReader reader = new StreamReader(netStream);
            StreamWriter writer = new StreamWriter(netStream);

            // Enviamos mensaje al cliente:
            writer.WriteLine("Bienvenido, intenta adivinar mi número");
            writer.Flush();

            // Escucha infinita de peticiones enviadas desde un cliente:
            while (true)
            {
                try
                {
                    // Linea bloqueante:
                    data = reader.ReadLine();
                    Debug.WriteLine(data);
                    
                    // Si acierta el número, pone adivinado y salimos del bucle:
                    if(data == "55")
                    {
                        writer.WriteLine("Adivinado");
                        writer.Flush();
                        break;
                    }
                    // Si pone 'exit' salimos del bucle:
                    else if(data == "exit")
                        break;
                    // Si falla el numero, envia un mensaje y se sigue en el bucle:
                    else
                    {
                        writer.WriteLine("Sigue intentándolo");
                        writer.Flush();
                    }
                }
                catch (Exception error)
                {
                    Debug.WriteLine("Error: " + error.ToString());
                }
            }

            netStream.Close();
            cliente.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TcpListener newSocket = new TcpListener(IPAddress.Any, 2000);
            newSocket.Start();

            while (true)
            {
                // Linea bloqueante:
                TcpClient cliente = newSocket.AcceptTcpClient();

                // Instanciamos el hilo y lo ejecutamos:
                Thread t = new Thread(() => this.ManejarCliente(cliente));
                t.Start();
            }
        }
    }
}
