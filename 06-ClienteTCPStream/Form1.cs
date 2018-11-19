using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace _06_ClienteTCPStream
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Variables para interactuar con el servidor:
        TcpClient cliente;
        NetworkStream netStream;
        StreamReader reader;
        StreamWriter writer;

        private void button1_Click(object sender, EventArgs e)
        {
            // Conectamos con el servidor:
            cliente = new TcpClient(this.textBox1.Text, 2000);
            netStream = cliente.GetStream();
            reader = new StreamReader(netStream);
            writer = new StreamWriter(netStream);

            // Recibimos el mensaje que envia el servidor:
            string mensaje_servidor = reader.ReadLine();
            Debug.WriteLine(mensaje_servidor);
            this.label1.Text = mensaje_servidor;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Enviamos mensaje al servidor:
                writer.WriteLine(this.textBox2.Text);
                writer.Flush();

                // Leemos la respuesta:
                this.label1.Text = reader.ReadLine();
            }
            catch (Exception error)
            {

                this.label1.Text = error.ToString();
            }

        }
    }
}
