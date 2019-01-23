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
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace _12_ClientePPTSondeo
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
                       reader.ReadLine() + System.Environment.NewLine +
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
            writer.WriteLine("#INSCRIBIR#" + this.textBox2.Text + "#");
            writer.Flush();

            dato = reader.ReadLine(); // Bloquea el cliente hasta que recibe la respuesta del servidor;
            this.label1.Text = dato;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            writer.WriteLine("#PUNTUACION#");
            writer.Flush();

            dato = reader.ReadLine(); // Bloquea el cliente hasta que recibe la respuesta del servidor;
            this.label1.Text = dato;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            writer.WriteLine("#JUGADA#" + this.comboBox1.Text + "#");
            writer.Flush();

            dato = reader.ReadLine(); // Bloquea el cliente hasta que recibe la respuesta del servidor;
            this.label1.Text = dato;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            writer.WriteLine("#RESULTADOJUGADA#" + this.textBox3.Text + "#");
            writer.Flush();

            dato = reader.ReadLine(); // Bloquea el cliente hasta que recibe la respuesta del servidor;
            this.label1.Text = dato;
        }
    }
}
