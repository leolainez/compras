using BL.Rentas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win.Rentas
{
    public partial class FormProveedores : Form
    {
        proveedoresBL ProveedoresBL;

        public FormProveedores()
        {
            InitializeComponent();
            ProveedoresBL = new proveedoresBL();
            listaProveedoresBindingSource.DataSource = ProveedoresBL.ObtenerProveedores();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            ProveedoresBL.AgregarProveedores();
            listaProveedoresBindingSource.MoveLast();

            DeshabilitarHabilitarBotones(false);
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (idTextBox.Text != "")
            {
                var resultado = MessageBox.Show("Desea eliminar este registro?", "Eliminar", MessageBoxButtons.YesNo);
                if (resultado == DialogResult.Yes)
                {
                    var id = Convert.ToInt32(idTextBox.Text);
                    Eliminar(id);
                }
            }

        }

        private void listaProveedoresBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            listaProveedoresBindingSource.EndEdit();
            var proveedorer = (Proveerdor)listaProveedoresBindingSource.Current;

            var resultado = ProveedoresBL.GuardarProveedores(proveedorer);

            if (resultado.Exitoso == true)
            {
                listaProveedoresBindingSource.ResetBindings(false);
                DeshabilitarHabilitarBotones(true);
                MessageBox.Show("proveedorer guardado");
            }
            else
            {
                MessageBox.Show(resultado.Mensaje);
            }
        }
        private void DeshabilitarHabilitarBotones(bool valor)
        {
            bindingNavigatorMoveFirstItem.Enabled = valor;
            bindingNavigatorMoveLastItem.Enabled = valor;
            bindingNavigatorMovePreviousItem.Enabled = valor;
            bindingNavigatorMoveNextItem.Enabled = valor;
            bindingNavigatorPositionItem.Enabled = valor;

            bindingNavigatorAddNewItem.Enabled = valor;
            bindingNavigatorDeleteItem.Enabled = valor;
            toolStripButtonCancelar.Visible = !valor;
        }

        private void Eliminar(int id)
        {
            var resultado = ProveedoresBL.EliminarCliente(id);

            if (resultado == true)
            {
                listaProveedoresBindingSource.ResetBindings(false);
            }
            else
            {
                MessageBox.Show("Ocurrio un error al eliminar el Cliente");
            }
        }

        private void toolStripButtonCancelar_Click(object sender, EventArgs e)
        {

            ProveedoresBL.CancelarCambios();
            DeshabilitarHabilitarBotones(true);
        }
    }

}
