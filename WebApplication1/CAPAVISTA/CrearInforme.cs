using CapaDatos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CAPAVISTA
{
    public partial class CrearInformes : Form
    {
        public CrearInformes()
        {
            InitializeComponent();
            CargarEmpleadosAsync();
        }

        private async Task CargarEmpleadosAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7020/api/Empleados/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("Listar"); 

                if (response.IsSuccessStatusCode)
                {
                    string jsonEmpleados = await response.Content.ReadAsStringAsync();
                    List<Empleados> empleados = JsonConvert.DeserializeObject<List<Empleados>>(jsonEmpleados);

                    comboBox1.DataSource = empleados;
                    comboBox1.DisplayMember = "Nombre";
                    comboBox1.ValueMember = "IdEmpleado";
                }
                else
                {
                    MessageBox.Show("Error al cargar empleados.");
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Por favor, selecciona un empleado.");
                return;
            }

            Informes nuevoInforme = new Informes
            {
                IdEmpleado = (int)comboBox1.SelectedValue,
                FechaGeneracion = DateTime.Now,
                ContenidoInforme = textBox1.Text
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7020/api/Informes/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                string jsonInforme = JsonConvert.SerializeObject(nuevoInforme);
                StringContent content = new StringContent(jsonInforme, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Insertar", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Informe generado correctamente.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Error al generar informe: {response.StatusCode}");
                }
            }
        }
    }
}
