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
    public partial class EditarVacaciones : Form
    {
        public int selectedId = 0;
        public EditarVacaciones(int selectedId)
        {
            InitializeComponent();
            this.selectedId = selectedId;
            LoadVacacionesData();
        }
        private async void LoadVacacionesData()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7020/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage responseEmpleados = await client.GetAsync("Empleados/Listar");
                if (responseEmpleados.IsSuccessStatusCode)
                {
                    string jsonEmpleados = await responseEmpleados.Content.ReadAsStringAsync();
                    var empleados = JsonConvert.DeserializeObject<List<Empleados>>(jsonEmpleados);

                    comboBox1.DataSource = empleados;
                    comboBox1.DisplayMember = "Nombre";  
                    comboBox1.ValueMember = "IdEmpleado";
                }
                else
                {
                    MessageBox.Show("Error al cargar empleados.");
                    return;
                }

                HttpResponseMessage responseVacaciones = await client.GetAsync("Vacaciones/Listar");
                if (responseVacaciones.IsSuccessStatusCode)
                {
                    string jsonResponse = await responseVacaciones.Content.ReadAsStringAsync();
                    var vacaciones = JsonConvert.DeserializeObject<List<Vacaciones>>(jsonResponse);

                    var idsVacaciones = vacaciones.Select(v => v.IdVacaciones).ToList();
                    if (!idsVacaciones.Contains(selectedId))
                    {
                        MessageBox.Show($"Vacación con ID {selectedId} no encontrada. IDs disponibles: {string.Join(", ", idsVacaciones)}");
                        return;
                    }

                    Vacaciones vacacion = vacaciones.Find(v => v.IdVacaciones == selectedId);
                    if (vacacion != null)
                    {
                        comboBox1.SelectedValue = vacacion.IdEmpleado; 
                        dateTimePicker1.Value = vacacion.FechaInicio;
                        dateTimePicker2.Value = vacacion.FechaFin;
                        textBox1.Text = vacacion.TipoVacaciones; 
                    }
                    else
                    {
                        MessageBox.Show("Vacación no encontrada.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error al cargar datos de la vacación: {responseVacaciones.StatusCode}");
                }
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (selectedId > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7020/api/Vacaciones/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    Vacaciones vacacionEditada = new Vacaciones
                    {
                        IdVacaciones = selectedId,
                        IdEmpleado = (int)comboBox1.SelectedValue, 
                        FechaInicio = dateTimePicker1.Value,
                        FechaFin = dateTimePicker2.Value,
                        TipoVacaciones = textBox1.Text 
                    };

                    string jsonVacacion = JsonConvert.SerializeObject(vacacionEditada);
                    StringContent content = new StringContent(jsonVacacion, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"Actualizar/{selectedId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Vacación editada correctamente.");
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show($"Error al editar vacación: {response.StatusCode}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una vacación para editar.");
            }
        }
    }
}
