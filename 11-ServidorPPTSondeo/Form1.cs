﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

namespace _11_ServidorPPTSondeo
{
    public partial class Form1 : Form
    {
        string nombreJ1 = ""; // Nick.
        string idJ1 = ""; // Referencia IP:port.
        string jugadaJ1 = ""; // Piedra, papel o tijera.
        int puntosJ1 = 0;

        string nombreJ2 = ""; // Nick.
        string idJ2 = ""; // Referencia IP:port.
        string jugadaJ2 = ""; // Piedra, papel o tijera.
        int puntosJ2 = 0;

        int numJugada = 1; // Indice de jugada en curso.
        string[] textoVueltaJugada = new string[100]; // Guardar resultado jugadas (historico).

        Object o = new object(); // Para lock.

        public Form1()
        {
            InitializeComponent();
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
            writer.WriteLine("#RESULTADOJUGADA#numeroJugada#");
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
                    if (subdatos[1] == "JUGADA")
                    {
                        if ((subdatos[2] != "piedra") && (subdatos[2] != "papel") && (subdatos[2] != "tijera"))
                        {
                            writer.WriteLine("#NOK#valores validos: piedra/papel/tijera");
                            writer.Flush();
                        }
                        // Comprobar quien hace la jugada 'y la guardamos':
                        else if (idJ1 == cliente.Client.RemoteEndPoint.ToString() ||
                               idJ2 == cliente.Client.RemoteEndPoint.ToString())
                        {
                            if (idJ1 == cliente.Client.RemoteEndPoint.ToString()) // Estamos en el jugador 1.
                            {
                                jugadaJ1 = subdatos[2];
                            }
                            else // Estamos en el jugador 2.
                            {
                                jugadaJ2 = subdatos[2];
                            }
                            writer.WriteLine("#OK#" + numJugada + "#");
                            writer.Flush();

                            lock (o)// Solo uno puede entrar en ComprobarGanador:
                            {
                                // Comprobar si tengo emitidas el par de jugadas:
                                if (jugadaJ1 != "" && jugadaJ2 != "")
                                {
                                    ComprobarGanador();
                                }
                            }
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
                    #region comRESULTADOJUGADA
                    if (subdatos[1] == "RESULTADOJUGADA")
                    {
                        int numJugadaCliente = System.Convert.ToInt32(subdatos[2]); // No contempladas validaciones, numeros erróneos.
                        if ((numJugadaCliente > 0) && (numJugadaCliente < numJugada))
                        {
                            writer.WriteLine(textoVueltaJugada[numJugadaCliente - 1]);
                            writer.Flush();
                        }
                        else
                        {
                            writer.WriteLine("#NOK#numero de jugada no válido#");
                            writer.Flush();
                        }
                    }
                    #endregion
                }
                catch (Exception error)
                {
                    Debug.WriteLine("Error: " + error.ToString());
                }
            }
        }

        private void ComprobarGanador()
        {
            // Resolver la jugada:
            // Gana el 1:
            if ((jugadaJ1 == "piedra" && jugadaJ2 == "tijera") ||
                (jugadaJ1 == "papel" && jugadaJ2 == "piedra") ||
                (jugadaJ1 == "tijera" && jugadaJ2 == "papel"))
            {
                textoVueltaJugada[numJugada - 1] = "#OK#GANADOR:" + nombreJ1 + "#";
                puntosJ1++;
            }
            // Gana el 2:
            else if ((jugadaJ2 == "piedra" && jugadaJ1 == "tijera") ||
                (jugadaJ2 == "papel" && jugadaJ1 == "piedra") ||
                (jugadaJ2 == "tijera" && jugadaJ1 == "papel"))
            {
                textoVueltaJugada[numJugada - 1] = "#OK#GANADOR:" + nombreJ2 + "#";
                puntosJ2++;
            }
            // Empate:
            else
            {
                textoVueltaJugada[numJugada - 1] = "#OK#EMPATE#";
            }

            // Pasamos a la siguiente jugada:
            numJugada++;
            jugadaJ1 = "";
            jugadaJ2 = "";

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("hola");
        }
    }
}
