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
    public partial class CrearPermisos : Form
    {
        public CrearPermisos()
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
            Permisos nuevoPermiso = new Permisos
            {
                IdEmpleado = (int)comboBox1.SelectedValue,
                FechaInicio = dateTimePicker1.Value,
                FechaFin = dateTimePicker2.Value,
                TipoPermiso = textBox1.Text
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7020/api/Permisos/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPermiso = JsonConvert.SerializeObject(nuevoPermiso);
                StringContent content = new StringContent(jsonPermiso, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Insertar", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Permiso registrado correctamente.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Error al registrar permiso: {response.StatusCode}");
                }
            }
        }
    }
}
