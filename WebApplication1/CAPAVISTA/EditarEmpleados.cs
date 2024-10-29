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
    public partial class EditarEmpleados : Form
    {
        public int selectedId = 0;
        public EditarEmpleados(int selectedId)
        {
            InitializeComponent();
            this.selectedId = selectedId;
            LoadEmpleadoData();
        }
        private async void LoadEmpleadoData()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7020/api/Empleados/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("Listar");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var empleados = JsonConvert.DeserializeObject<List<Empleados>>(jsonResponse);

                    Empleados empleado = empleados.Find(e => e.IdEmpleado == selectedId);
                    if (empleado != null)
                    {
                        textBox1.Text = empleado.Nombre;
                        textBox2.Text = empleado.Apellido;
                        dateTimePicker1.Value = empleado.FechaIngreso;
                        textBox3.Text = empleado.Cargo;
                    }
                    else
                    {
                        MessageBox.Show("Empleado no encontrado.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error al cargar datos del empleado: {response.StatusCode}");
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (selectedId > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7020/api/Empleados/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    Empleados empleadoEditado = new Empleados
                    {
                        IdEmpleado = selectedId, 
                        Nombre = textBox1.Text,
                        Apellido = textBox2.Text,
                        FechaIngreso = dateTimePicker1.Value, 
                        Cargo = textBox3.Text
                    };

                    string jsonEmpleado = JsonConvert.SerializeObject(empleadoEditado);
                    StringContent content = new StringContent(jsonEmpleado, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"Actualizar/{selectedId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Empleado editado correctamente.");
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show($"Error al editar empleado: {response.StatusCode}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un empleado para editar.");
            }
        }
    }
}
