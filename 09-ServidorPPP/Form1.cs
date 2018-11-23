using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _09_ServidorPPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string nombreJ1 = ""; // Nick.
        string idJ1 = ""; // Referencia IP:port.
        string jugadaJ1 = ""; // Piedra, papel o tijera.
        int puntosJ1 = 0;

        string nombreJ2 = ""; // Nick.
        string idJ2 = ""; // Referencia IP:port.
        string jugadaJ2 = ""; // Piedra, papel o tijera.
        int puntosJ2 = 0;

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

        private void ManejarCliente(TcpClient cliente)
        {
            // Creamos variables para comunicarnos con el cliente:
            string data;
            NetworkStream netStream = cliente.GetStream();
            StreamReader reader = new StreamReader(netStream);
            StreamWriter writer = new StreamWriter(netStream);

            // Informe de protocolo:
            writer.WriteLine("#INSCRIBIR#nombre#");
            writer.WriteLine("#JUGADA#{piedra|papel|tijera}#");
            writer.WriteLine("#PUNTUACION#");
            writer.Flush();

            // Escucha infinita de peticiones enviadas desde un cliente:
            while (true)
            {
                try
                {
                    // Linea bloqueante:
                    data = reader.ReadLine();
                    Debug.WriteLine(data);

                    string[] subdatos = data.Split('#');
                    #region comINSCRIBIR
                    if (subdatos[1] == "INSCRIBIR")
                    {
                        if (nombreJ1 == "")
                        {
                            // Guardamos el jugador 1:
                            nombreJ1 = subdatos[2];
                            idJ1 = cliente.Client.RemoteEndPoint.ToString();
                            writer.WriteLine("#OK#");
                            writer.Flush();
                        }
                        else if (nombreJ2 == "")
                        {
                            // Si ya hay jugador uno guardamos el jugador 2:
                            nombreJ2 = subdatos[2];
                            idJ2 = cliente.Client.RemoteEndPoint.ToString();
                            writer.WriteLine("#OK#");
                            writer.Flush();
                        }
                        else
                        {
                            // Si ya hay dos jugadores enviamos un 'NOK';
                            writer.WriteLine("#NOK#ya hay dos jugadores");
                            writer.Flush();
                        }
                    }
                    #endregion comINSCRIBIR


                }
                catch (Exception error)
                {
                    Debug.WriteLine("Error: " + error.ToString());
                }
            }

            netStream.Close();
            cliente.Close();
        }
    }
}
