using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos;
using Newtonsoft.Json;

namespace CAPAVISTA
{
    public partial class Form1 : Form
    {
        public string variable=null;
        public int selectedId = 0;

        public Form1()
        {
            InitializeComponent();
            CustomizeDataGridView();
            CustomizeButtons();
            SetFormFont();
        }
        private void CustomizeDataGridView()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = Color.FromArgb(24, 34, 56);
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(224, 224, 224);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 58, 147);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
        }

        private void CustomizeButtons()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button button)
                {
                    button.BackColor = Color.FromArgb(31, 58, 147);
                    button.ForeColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.Font = new Font("Segoe UI", 12, FontStyle.Regular);
                    button.Padding = new Padding(6);
                    button.Size = new Size(120, 40);
                    button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(52, 73, 94);
                    button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(31, 58, 147);
                }
            }
        }

        private void SetFormFont()
        {
            this.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            variable=null;
            string respuesta = await GettHttp();
            List<Empleados> lista = JsonConvert.DeserializeObject<List<Empleados>>(respuesta);
            dataGridView1.DataSource = lista;
            variable = "Empleados";

           
        }
        private async Task<string> GettHttp()
        {
            WebRequest request = WebRequest.Create("https://localhost:7020/api/Empleados/Listar");
            WebResponse response = await request.GetResponseAsync();
            StreamReader leer = new StreamReader(response.GetResponseStream());
            return await leer.ReadToEndAsync();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            variable = null;
            string respuesta = await GettHttp1();
            List<Permisos> lista = JsonConvert.DeserializeObject<List<Permisos>>(respuesta);
            dataGridView1.DataSource = lista;
            variable = "Permisos";
        }
        private async Task<string> GettHttp1()
        {
            
            WebRequest request = WebRequest.Create("https://localhost:7020/api/Permisos/Listar");
            WebResponse response = await request.GetResponseAsync();
            StreamReader leer = new StreamReader(response.GetResponseStream());
            return await leer.ReadToEndAsync();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            variable = null;
            string respuesta = await GettHttp2();
            List<Nominas> lista = JsonConvert.DeserializeObject<List<Nominas>>(respuesta);
            dataGridView1.DataSource = lista;
            variable = "Nominas";
        }
        private async Task<string> GettHttp2()
        {

            WebRequest request = WebRequest.Create("https://localhost:7020/api/Nomina/Listar");
            WebResponse response = await request.GetResponseAsync();
            StreamReader leer = new StreamReader(response.GetResponseStream());
            return await leer.ReadToEndAsync();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            variable = null;
            string respuesta = await GettHttp3();
            List<Capacitaciones> lista = JsonConvert.DeserializeObject<List<Capacitaciones>>(respuesta);
            dataGridView1.DataSource = lista;
            variable = "Capacitaciones";
        }
        private async Task<string> GettHttp3()
        {
            WebRequest request = WebRequest.Create("https://localhost:7020/api/Capacitaciones/Listar");
            WebResponse response = await request.GetResponseAsync();
            StreamReader leer = new StreamReader(response.GetResponseStream());
            return await leer.ReadToEndAsync();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(24, 34, 56);
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            variable = null;
            string respuesta = await GettHttp4();
            List<Vacaciones> lista = JsonConvert.DeserializeObject<List<Vacaciones>>(respuesta);
            dataGridView1.DataSource = lista;
            variable = "Vacaciones";
        }
        private async Task<string> GettHttp4()
        {
            WebRequest request = WebRequest.Create("https://localhost:7020/api/Vacaciones/Listar");
            WebResponse response = await request.GetResponseAsync();
            StreamReader leer = new StreamReader(response.GetResponseStream());
            return await leer.ReadToEndAsync();
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            variable = null;
            string respuesta = await GettHttp5();
            List<Informes> lista = JsonConvert.DeserializeObject<List<Informes>>(respuesta);
            dataGridView1.DataSource = lista;
            variable = "Informes";
        }
        private async Task<string> GettHttp5()
        {
            WebRequest request = WebRequest.Create("https://localhost:7020/api/Informes/Listar");
            WebResponse response = await request.GetResponseAsync();
            StreamReader leer = new StreamReader(response.GetResponseStream());
            return await leer.ReadToEndAsync();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (variable == "Empleados")
            {
                CrearEmpleados crearEmpleados = new CrearEmpleados();
                crearEmpleados.ShowDialog();
            }
            else if (variable == "Capacitaciones")
            {
                CrearCapacitaciones agregarCapacitaciones = new CrearCapacitaciones();
                agregarCapacitaciones.ShowDialog();
            }
            else if (variable == "Vacaciones")
            {
                CrearVacaciones crearVacaciones = new CrearVacaciones();
                crearVacaciones.ShowDialog();
            }
            else if (variable == "Permisos") 
            {
                CrearPermisos crearPermisos = new CrearPermisos(); 
            }
            else if (variable == "Nominas") 
            {
                CrearNominas crearNominas = new CrearNominas(); 
                crearNominas.ShowDialog();
            }
            else if (variable == "Informes")
            {
                CrearInformes crearInformes = new CrearInformes();
                crearInformes.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una opción válida.");
            }
        }


        private async void button9_Click(object sender, EventArgs e)
        {
            if (selectedId > 0) 
            {
                string itemType = variable == "Empleados" ? "empleado" :
                                  variable == "Capacitaciones" ? "capacitación" :
                                  variable == "Vacaciones" ? "vacación" :
                                  variable == "Permisos" ? "permiso" :
                                  variable == "Nominas" ? "nómina" :
                                  variable == "Informes" ? "informe" : "registro";

                var result = MessageBox.Show($"¿Estás seguro de que deseas eliminar este {itemType}?", "Confirmar eliminación", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    if (variable == "Empleados")
                    {
                        await EliminarEmpleado(selectedId);
                        MessageBox.Show("Empleado eliminado exitosamente.");
                        button1_Click(sender, e);
                    }
                    else if (variable == "Capacitaciones")
                    {
                        await EliminarCapacitacion(selectedId);
                        MessageBox.Show("Capacitación eliminada exitosamente.");
                        button2_Click(sender, e); 
                    }
                    else if (variable == "Vacaciones")
                    {
                        await EliminarVacaciones(selectedId);
                        MessageBox.Show("Vacación eliminada exitosamente.");
                        button3_Click(sender, e); 
                    }
                    else if (variable == "Permisos")
                    {
                        await EliminarPermiso(selectedId);
                        MessageBox.Show("Permiso eliminado exitosamente.");
                        button4_Click(sender, e); 
                    }
                    else if (variable == "Nominas")
                    {
                        await EliminarNomina(selectedId);
                        MessageBox.Show("Nómina eliminada exitosamente.");
                        button3_Click(sender, e);
                    }
                    else if (variable == "Informes")
                    {
                        await EliminarInforme(selectedId);
                        MessageBox.Show("Informe eliminado exitosamente.");
                        button6_Click(sender, e); 
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un elemento para eliminar.");
            }
        }

        private async Task EliminarPermiso(int id)
        {
            WebRequest request = WebRequest.Create($"https://localhost:7020/api/Permisos/Eliminar/{id}");
            request.Method = "DELETE";

            using (WebResponse response = await request.GetResponseAsync())
            {
            }
        }

        private async Task EliminarNomina(int id)
        {
            WebRequest request = WebRequest.Create($"https://localhost:7020/api/Nomina/Eliminar/{id}");
            request.Method = "DELETE";

            using (WebResponse response = await request.GetResponseAsync())
            {
            }
        }

        private async Task EliminarInforme(int id)
        {
            WebRequest request = WebRequest.Create($"https://localhost:7020/api/Informes/Eliminar/{id}");
            request.Method = "DELETE";

            using (WebResponse response = await request.GetResponseAsync())
            {
            }
        }

        private async Task EliminarEmpleado(int id)
        {
            WebRequest request = WebRequest.Create($"https://localhost:7020/api/Empleados/Eliminar/{id}");
            request.Method = "DELETE";

            using (WebResponse response = await request.GetResponseAsync())
            {
            }
        }
        private async Task EliminarCapacitacion(int id)
        {
            WebRequest request = WebRequest.Create($"https://localhost:7020/api/Capacitaciones/Eliminar/{id}");
            request.Method = "DELETE";

            using (WebResponse response = await request.GetResponseAsync())
            {
            }
        }
        private async Task EliminarVacaciones(int id)
        {
            WebRequest request = WebRequest.Create($"https://localhost:7020/api/Vacaciones/Eliminar/{id}");
            request.Method = "DELETE";

            using (WebResponse response = await request.GetResponseAsync())
            {
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                selectedId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["IdEmpleado"].Value);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (selectedId > 0)
            {
                if (variable == "Empleados")
                {
                    EditarEmpleados editarEmpleados = new EditarEmpleados(selectedId);
                    editarEmpleados.ShowDialog();
                }
                else if (variable == "Capacitaciones")
                {
                    EditarCapacitaciones editarCapacitaciones = new EditarCapacitaciones(selectedId);
                    editarCapacitaciones.ShowDialog();
                }
                else if (variable == "Vacaciones")
                {
                    EditarVacaciones editarVacaciones = new EditarVacaciones(selectedId);
                    editarVacaciones.ShowDialog();
                }
                else if (variable == "Permisos")
                {
                    EditarPermisos editarPermisos = new EditarPermisos(selectedId); 
                    editarPermisos.ShowDialog();
                }
                else if (variable == "Nominas") 
                {
                    EditarNominas editarNominas = new EditarNominas(selectedId);
                    editarNominas.ShowDialog();
                }
                else if (variable == "Informes")
                {
                    EditarInformes editarInformes = new EditarInformes(selectedId); 
                    editarInformes.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona una opción válida.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un elemento para editar.");
            }
        }

    }
}
