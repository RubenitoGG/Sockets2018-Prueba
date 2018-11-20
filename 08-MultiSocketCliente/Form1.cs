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

namespace _08_MultiSocketCliente
{
    public partial class Form1 : Form
    {
        // Variables para conectar/comunicarse con el servidor:
        TcpClient cliente;
        NetworkStream netStream;
        StreamReader reader;
        StreamWriter writer;
        string datoVuelta;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Inicializamos variables y nos conectamos:
                cliente = new TcpClient(this.textBox1.Text, 2000);
                netStream = cliente.GetStream();
                reader = new StreamReader(netStream);
                writer = new StreamWriter(netStream);

                // Mensaje de bienvenida con delegado:
                datoVuelta = reader.ReadLine();
                Debug.WriteLine(datoVuelta);
                DelegadoRespuesta delegado = new DelegadoRespuesta(EscribirEnLabel);
                this.Invoke(delegado);
            }
            catch (Exception error)
            {
                Debug.WriteLine("Error: " + error.ToString());
            }
        }

        // Delegado:
        delegate void DelegadoRespuesta();

        private void EscribirEnLabel()
        {
            this.label1.Text = this.datoVuelta;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Enviamos mensaje al servidor:
            writer.WriteLine(this.textBox2.Text);
            writer.Flush();
            // Recibimos la respuesta con delegado:
            datoVuelta = reader.ReadLine();
            DelegadoRespuesta delegado = new DelegadoRespuesta(EscribirEnLabel);
            this.Invoke(delegado);
        }
    }
}
