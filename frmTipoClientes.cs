using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetConectado
{
    public partial class frmTipoClientes : Form
    {
        string cadenaConexion = @"Server= localhost\MSSQLSERVERDEVEL; DataBase=BancoBD; user=sa; password=1234";
        public frmTipoClientes()
        {
            InitializeComponent();
        }
        private void cargarFormulario(object sender, EventArgs e)
        {
            cargarDatos();
        }
        private void cargarDatos()
        {
            //UDING DELIMITA EL CICLO DE VIDA DE UNA VARIABLE
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("SELECT * FROM TipoCliente", conexion))
                {
                    using (var lector = comando.ExecuteReader())
                    {
                        if(lector != null && lector.HasRows)
                        {
                            while (lector.Read())
                            {
                                dgvDatos.Rows.Add(lector[0], lector[1], lector[2], lector[3]);
                            }
                        }

                    }
                }

            }
        }

        private void nuevoRegistro(object sender, EventArgs e)
        {
            frmTipoClienteEdit frm= new frmTipoClienteEdit();
            
            if (frm.ShowDialog()==DialogResult.OK)
            {
                string nombre = frm.Controls["txtNombre"].Text;
                string descripcion = frm.Controls["txtDescripcion"].Text;
                //OperadorTernario
                int estado = ((CheckBox)frm.Controls["chkEstado"]).Checked== true ? 1: 0;

                using (var conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (var comando = new SqlCommand("INSERT INTO TipoClientes (Nombre, Descripcion,Estado)" +
                        "VALUES (@nombre,@descripcion,@estado)", conexion))
                    {
                        comando.Parameters.AddWithValue("@nombre", nombre);
                        comando.Parameters.AddWithValue("@descripcion", descripcion);
                        comando.Parameters.AddWithValue("@estado", estado);
                        int resultado = comando.ExecuteNonQuery();
                        if (resultado > 0) 
                        {
                            MessageBox.Show("Datos registrados", "Sistemas",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }else
                        {
                            MessageBox.Show("No se ha podido registrar los datps", "Sistemas",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            

        }
    }
}
