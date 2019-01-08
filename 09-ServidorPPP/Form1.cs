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
            StreamWriter writer = new StreamWriter(netStream, Encoding.UTF8);

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
                    #region comJUGADA
                    if (subdatos[1] == "jugada")
                    {
                        if ((subdatos[2] != "piedra") && (subdatos[2] != "papel") && (subdatos[2] != "tijera"))
                        {
                            writer.WriteLine("#NOK#valores validos: piedra/papel/tijera");
                            writer.Flush();
                        }
                        // Comprobar quien hace la jugada:
                        else if (idJ1 == cliente.Client.RemoteEndPoint.ToString() ||
                                idJ2 == cliente.Client.RemoteEndPoint.ToString())
                        {
                            if (idJ1 == cliente.Client.RemoteEndPoint.ToString()) // Estamos en el jugador 1.
                            {
                                jugadaJ1 = subdatos[2];
                                // Pausar hasta que jugador2 no haga su jugada:
                                while (jugadaJ2 == "")
                                {
                                    Thread.Sleep(100);
                                }
                            }
                            else // Estamos en el jugador 2.
                            {
                                jugadaJ2 = subdatos[2];
                                while (jugadaJ2 == "") Thread.Sleep(100);
                            }

                            // Resolver la jugada:
                            // Gana el 1:
                            if ((jugadaJ1 == "piedra" && jugadaJ2 == "tijera") ||
                                (jugadaJ1 == "papel" && jugadaJ2 == "piedra") ||
                                (jugadaJ1 == "tijera" && jugadaJ2 == "papel"))
                            {
                                writer.WriteLine("#OK#GANADOR:" + nombreJ1 + "#");
                                writer.Flush();
                                puntosJ1++;
                            }
                            // Gana el 2:
                            else if ((jugadaJ2 == "piedra" && jugadaJ1 == "tijera") ||
                                (jugadaJ2 == "papel" && jugadaJ1 == "piedra") ||
                                (jugadaJ2 == "tijera" && jugadaJ1 == "papel"))
                            {
                                writer.WriteLine("#OK#GANADOR:" + nombreJ2 + "#");
                                writer.Flush();
                                puntosJ2++;
                            }
                            // Empate:
                            else
                            {
                                writer.WriteLine("#OK#EMPATE#");
                                writer.Flush();
                            }

                            // 
                            Thread.Sleep(1000);
                            jugadaJ1 = "";
                            jugadaJ2 = "";
                        }
                        else
                        {
                            writer.WriteLine("#NOK#el jugador no esta en la partida#");
                            writer.Flush();
                        }
                    }
                    #endregion
                    #region comPUNTUACION
                    if (subdatos[1] == "PUNTUACION")
                    {
                        writer.WriteLine("#OK#" + nombreJ1 + ":" + puntosJ1.ToString() + "#"
                                                + nombreJ2 + ":" + puntosJ2.ToString() + "#");
                        writer.Flush();
                    }
                    #endregion

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
