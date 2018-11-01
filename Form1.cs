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
using ChatBot_IDE.Logica;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ChatBot_IDE
{
    public partial class Form1 : Form
    {
        List<string> impresiones;
        List<ErrorC> errores;

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
            TabPage pagina = new TabPage();
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
            impresiones = new List<string>();
            errores = new List<ErrorC>();
            consolaSalidas.Text = "";

            int actual = this.archivos.SelectedIndex;
            String texto = ((RichTextBox)((this.archivos.Controls[actual]).Controls[0])).Text;

            string json = texto;
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("https://localhost:44375/api/ComunicacionIDE", byteContent);

            var responseString = await response.Content.ReadAsStringAsync();

            string jsonR = Convert.ToString(responseString);

            JObject obj = JObject.Parse(jsonR);
            JToken uno = obj["Impresiones"];
            JToken dos = obj["Errores"];
            string one = uno.ToString();
            string two = dos.ToString();
            JArray cadenas = JArray.Parse(one);
            JArray errs = JArray.Parse(two);
            extraerImpresiones(cadenas);
            extraerErrores(errs);
        }

        private void verErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int actual = this.archivos.SelectedIndex;
            String texto = ((RichTextBox)((this.archivos.Controls[actual]).Controls[0])).Text;
            SaveFileDialog guardar = new SaveFileDialog();
            if (guardar.ShowDialog() == DialogResult.OK)
            {
                String file = guardar.FileName;
                File.WriteAllText(file, texto);
            }
        }

        private void extraerImpresiones(JArray impres) 
        {
            foreach (JObject impresion in impres)
            {
                string actual = Convert.ToString(impresion.GetValue("valor"));
                consolaSalidas.Text += actual + "\n";
                impresiones.Add(actual);
            }
        }

        private void extraerErrores(JArray errs)
        {
            ErrorC auxiliar;
            foreach (JObject error in errs)
            {
                auxiliar = new ErrorC();
                if (Convert.ToString(error.GetValue("tipo")).Equals("Semantico"))
                {
                    auxiliar.tipo = "Semantico";
                    auxiliar.descripcion = Convert.ToString(error.GetValue("descripcion"));
                    errores.Add(auxiliar);
                }
                else
                {
                    auxiliar.tipo = Convert.ToString(error.GetValue("tipo"));
                    auxiliar.descripcion = Convert.ToString(error.GetValue("descripcion"));
                    auxiliar.lexema = Convert.ToString(error.GetValue("lexema"));
                    auxiliar.fila = Convert.ToInt32(error.GetValue("fila"));
                    auxiliar.columna = Convert.ToInt32(error.GetValue("columna"));
                    errores.Add(auxiliar);
                }
            }
        }
    }
}
