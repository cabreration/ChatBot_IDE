using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace ChatBot_IDE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static HttpClient client = new HttpClient();

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog browser = new OpenFileDialog();
            if (browser.ShowDialog() == DialogResult.OK)
            {
                Stream contenido = browser.OpenFile();
                if (contenido != null)
                {
                    StreamReader reader = new StreamReader(contenido);
                    String salida = reader.ReadToEnd();
                    RichTextBox arch = new RichTextBox();
                    arch.Size = new Size(this.archivos.Width - 10, this.archivos.Height - 10);
                    arch.Text = salida;
                    TabPage pagina = new TabPage();
                    pagina.Controls.Add(arch);
                    this.archivos.Controls.Add(pagina);
                }
            }
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox arch = new RichTextBox();
            arch.Size = new Size(this.archivos.Width - 10, this.archivos.Height - 10);
            TabPage pagina = new TabPage("Nuevo.cbc");
            pagina.Controls.Add(arch);
            this.archivos.Controls.Add(pagina);
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int actual = this.archivos.SelectedIndex;
            this.archivos.Controls.RemoveAt(actual);
        }

        private async void ejecutarToolStripMenuItem_ClickAsync(object sender, EventArgs e)
        {
            int actual = this.archivos.SelectedIndex;
            String texto = ((RichTextBox)((this.archivos.Controls[actual]).Controls[0])).Text;

            string json = "\"" + texto + "\"";
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);

            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("https://localhost:44375/api/ComunicacionIDE", byteContent);

            var responseString = await response.Content.ReadAsStringAsync();
        }

        private void verErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
