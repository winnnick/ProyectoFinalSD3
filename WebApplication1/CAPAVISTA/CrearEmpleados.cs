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
    public partial class CrearEmpleados : Form
    {
        public CrearEmpleados()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7020/api/Empleados/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                Empleados nuevoEmpleado = new Empleados();
                nuevoEmpleado.Nombre = textBox1.Text;
                nuevoEmpleado.Apellido = textBox2.Text;
                nuevoEmpleado.FechaIngreso = Convert.ToDateTime(dateTimePicker1.Text);
                nuevoEmpleado.Cargo = textBox3.Text;
                
                string jsonEmpleado = JsonConvert.SerializeObject(nuevoEmpleado);
                StringContent content = new StringContent(jsonEmpleado, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Insertar", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show( "Empleado registrado correctamente.");
                }
                else
                {
                    MessageBox.Show($"Error al registrar empleado: {response.StatusCode}") ;
                }
            }
        }
    }
}
