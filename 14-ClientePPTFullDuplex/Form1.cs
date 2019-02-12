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
using System.Diagnostics;
using System.IO;
using System.Threading;


namespace _14_ClientePPTFullDuplex
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream netStream;
        StreamReader reader;
        StreamWriter writer;

        string dato;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient(this.textBox1.Text, 2000);
                netStream = client.GetStream();
                reader = new StreamReader(netStream);
                writer = new StreamWriter(netStream);
                dato = reader.ReadLine() + System.Environment.NewLine +
                       reader.ReadLine() + System.Environment.NewLine +
                       //reader.ReadLine() + System.Environment.NewLine +
                       reader.ReadLine();
                // No es necesario usar delegado porque tenemos un solo hilo.
                this.label1.Text = dato;
            }
            catch (Exception error)
            {
                Debug.WriteLine("Error: " + error.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            writer.WriteLine("#INSCRIBIR#" + this.textBox2.Text + "#" + this.textBox3.Text + "#");
            writer.Flush();

            dato = reader.ReadLine(); // Bloquea el cliente hasta que recibe la respuesta del servidor;
            this.label1.Text = dato;

            // Abrir thread escucha:
            Thread t = new Thread(this.EscucharResultados);
            t.IsBackground = true;
            t.Start();
        }

        private void EscucharResultados()
        {
            TcpListener newSock = new TcpListener(IPAddress.Any, System.Convert.ToInt32(this.textBox3.Text));
            newSock.Start();
            Debug.WriteLine("Esperando cliente");

            while (true)
            {
                TcpClient client = newSock.AcceptTcpClient();
                NetworkStream netStream = client.GetStream();
                StreamReader reader = new StreamReader(netStream);
                StreamWriter writer = new StreamWriter(netStream);

                this.dato = reader.ReadLine();
                writer.WriteLine("#OK#");
                writer.Flush();

                DelegadoRespuesta dr = new DelegadoRespuesta(EscribirFormulario);
                this.Invoke(dr);
            }
        }

        delegate void DelegadoRespuesta();
        private void EscribirFormulario()
        {
            this.label1.Text = dato;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            writer.WriteLine("#JUGADA#" + this.comboBox1.Text + "#");
            writer.Flush();

            dato = reader.ReadLine(); // Bloquea el cliente hasta que recibe la respuesta del servidor;
            DelegadoRespuesta dr = new DelegadoRespuesta(EscribirFormulario);
            this.Invoke(dr);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            writer.WriteLine("#PUNTUACION#");
            writer.Flush();

            dato = reader.ReadLine(); // Bloquea el cliente hasta que recibe la respuesta del servidor;
            DelegadoRespuesta dr = new DelegadoRespuesta(EscribirFormulario);
            this.Invoke(dr);
        }
    }
}
