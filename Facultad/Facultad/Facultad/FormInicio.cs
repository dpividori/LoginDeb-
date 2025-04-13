using Facultad.Persistencia;
using Negocio;
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

namespace Facultad
{
    public partial class FormInicio : Form
    {
        // partial class es una clase que se define en más de un archivo.
        // en este caso FormInicio está heredando de la clase Form.
        // PersistenciaUtils es una clase que maneja la persistencia de datos.
        //persistenciaUtils  es un objeto de la clase PersistenciaUtils .
        PersistenciaUtils persistenciaUtils = new PersistenciaUtils();
        List<String> listado = new List<String>();
        


        public FormInicio()
        {
            InitializeComponent();
            listado = persistenciaUtils.LeerRegistro("credenciales.csv");

            // InitializeComponent es un método que inicializa los componentes de la clase FormInicio.
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // 1) Validaciones

            // 1.1) Validaciones de integridad de datos
            // que usuario y password no estén vacíos

            string Msg = "";

            if (txtUsuario.Text == "")
            {
                Msg = "El campo usuario no puede estar vacío \n";
            }
            
            if (txtPassword.Text == "")
                    {
                Msg = Msg + "El campo Password no puede estar vacío \n";

            }


            // 1.) Validaciones de negocio

            // que el usuario exista en el archivo csv
            // que el password exista en el archivo csv y se corresponda con el pass
            // PersistenciaUtils

            //public List<String> LeerRegistro(String nombreArchivo)
            
            //PUSE EL LISTADO EN UNA VARIABLE PARA PODER HACER LA VALIDACION

            bool existeUsuario = false;
            DateTime fechaUltimoCambio = DateTime.Now;
            foreach (String linea in listado)
            {
                String[] columna = linea.Split(';');
                // columna[0] = usuario
                // columna[1] = password
                  if (columna[0].Equals(txtUsuario.Text) && columna[1].Equals(txtPassword.Text))
                  {
                    existeUsuario = true;
                    fechaUltimoCambio = DateTime.Parse(columna[2]);
                    // si existe el usuario y el password, entonces se loguea
                    // y se redirige al menu
                    // FormMenu formMenu = new FormMenu();
                    // formMenu.ShowDialog();
                    break;
                }
            }

            if (!existeUsuario)
            {
                Msg = Msg + "El usuario o password son incorrectos \n";
                
            }



            // 1.1) Longitud de usuario (mayor igual a 6)

            if (txtUsuario.Text.Length < 6)
            {
                Msg = Msg + "El campo usuario debe tener al menos 6 caracteres \n";
            }


            // 1.2) Longitud de password (mayor igual a 6)
            if (txtPassword.Text.Length < 6)
            {
                Msg = Msg + "El campo password debe tener al menos 6 caracteres \n";
            }

            // 1.3) Primero Login? -> Cambio password
            // 1.3) la primera vez que se loguea la una persona, crea usuario y password
            if (txtPassword.Text.Equals("123456"))
            {
                Msg= Msg + "Debe actualizar su password \n";
            }
            
            // 1.4) Expira password?
            // cada 3 meses

           if (fechaUltimoCambio.AddMonths(3) < DateTime.Now)
            {
                Msg = Msg + "Su password ha expirado, debe actualizarlo \n";
            }


            if (!string.IsNullOrEmpty(Msg)) 
            {
                MessageBox.Show(Msg);
                return;
            }

            // 2) Redirigir
            this.Hide();
            FormMenu formMenu = new FormMenu();
            formMenu.ShowDialog();
        }

        private List<String> obtenerUsuarios()
        {
            List<String> listado = persistenciaUtils.LeerRegistro("credenciales.csv");

            return listado;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormInicio_Load(object sender, EventArgs e)
        {
            txtUsuario.Text = "admin1";
            txtPassword.Text = "Abc123";
        }
    }
}
