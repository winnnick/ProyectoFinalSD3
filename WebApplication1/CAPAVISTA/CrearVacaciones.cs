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
    public partial class CrearVacaciones : Form
    {
        public CrearVacaciones()
        {
            InitializeComponent();
            CargarEmpleadosAsync();
        }

        private async Task CargarEmpleadosAsync()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar empleados: {ex.Message}");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
 
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Por favor, selecciona un empleado.");
                return;
            }

            Vacaciones nuevaVacacion = new Vacaciones
            {
                IdEmpleado = (int)comboBox1.SelectedValue,
                FechaInicio = dateTimePicker1.Value,
                FechaFin = dateTimePicker2.Value,
                TipoVacaciones = textBox1.Text
            };

            if (nuevaVacacion.FechaFin < nuevaVacacion.FechaInicio)
            {
                MessageBox.Show("La fecha de fin debe ser mayor o igual a la fecha de inicio.");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7020/api/Vacaciones/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonVacacion = JsonConvert.SerializeObject(nuevaVacacion);
                    StringContent content = new StringContent(jsonVacacion, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("Insertar", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Vacaciones registradas correctamente.");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Error al registrar vacaciones: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
