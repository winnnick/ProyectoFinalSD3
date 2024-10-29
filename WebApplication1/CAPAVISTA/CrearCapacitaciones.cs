using CapaDatos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAPAVISTA
{
    public partial class CrearCapacitaciones : Form
    {
        public CrearCapacitaciones()
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
                    comboBox1.DisplayMember = "nombre";
                    comboBox1.ValueMember = "idEmpleado";
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

            Capacitaciones nuevaCapacitacion = new Capacitaciones
            {
                IdEmpleado = (int)comboBox1.SelectedValue,
                FechaInicio = dateTimePicker1.Value,
                FechaFin = dateTimePicker2.Value,
                TipoCapacitacion = textBox1.Text
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7020/api/Capacitaciones/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                string jsonCapacitacion = JsonConvert.SerializeObject(nuevaCapacitacion);
                StringContent content = new StringContent(jsonCapacitacion, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Insertar", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Capacitación añadida correctamente.");
                    this.DialogResult = DialogResult.OK; 
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Error al añadir capacitación: {response.StatusCode}");
                }
            }
        }
    }
}
