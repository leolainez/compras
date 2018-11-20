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
    public partial class FormCompras : Form
    {
        ComprasBL _compraBL;
        proveedoresBL _proveedoresBL;
        ProductosBL _productosBL;
        


        public FormCompras()
        {
            InitializeComponent();
            _compraBL = new ComprasBL();
            listaCompraBindingSource.DataSource = _compraBL.ObtenerCompras();

            _proveedoresBL = new proveedoresBL();
            listaProveedoresBindingSource.DataSource = _proveedoresBL.ObtenerProveedores();

            _productosBL = new ProductosBL();
            listaProductosBindingSource.DataSource = _productosBL.ObtenerProductos();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            _compraBL.AgregarCompra();
            listaCompraBindingSource.MoveLast();

            DeshabilitarHabilitarBotones(false);
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
            toolStripButton1.Visible = !valor;
        }

        private void listaCompraBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            listaCompraBindingSource.EndEdit();

            var compra = (Compra)listaCompraBindingSource.Current;
            var resultado = _compraBL.GuardarCopmra(compra);

            if (resultado.Exitoso == true)
            {
                listaCompraBindingSource.ResetBindings(false);
                DeshabilitarHabilitarBotones(true);
                MessageBox.Show("Compra Guardada");
            }
            else
            {
                MessageBox.Show(resultado.Mensaje);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DeshabilitarHabilitarBotones(true);
            _compraBL.CancelarCambios();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var compra = (Compra)listaCompraBindingSource.Current;

            _compraBL.AgregarCompraDetalle(compra);

            DeshabilitarHabilitarBotones(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var compra = (Compra)listaCompraBindingSource.Current;
            var compraDetalle = (compraDetalle)compraDetalleBindingSource.Current;

            _compraBL.RemoverComprasDetalle(compra, compraDetalle);

            DeshabilitarHabilitarBotones(false);
        }

        private void compraDetalleDataGridView_DataError_1(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void compraDetalleDataGridView_CellEndEdit_1(object sender, DataGridViewCellEventArgs e)
        {
            var compra = (Compra)listaCompraBindingSource.Current;
            _compraBL.CalcularCompra(compra);

            listaCompraBindingSource.ResetBindings(false);
        }


        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (idTextBox.Text != "")
            {
                var resultado = MessageBox.Show("Desea anular esta compra?", "Anular", MessageBoxButtons.YesNo);
                if (resultado == DialogResult.Yes)
                {
                    var id = Convert.ToInt32(idTextBox.Text);
                    Anular(id);
                }
            }
        }

        private void Anular(int id)
        {
            var resultado = _compraBL.AnularCompra(id);

            if (resultado == true)
            {
                listaCompraBindingSource.ResetBindings(false);
            }
            else
            {
                MessageBox.Show("Ocurrio un error al anular la Compra");
            }
        }
                

        private void listaCompraBindingSource_CurrentChanged_1(object sender, EventArgs e)
        {
            var compra = (Compra)listaCompraBindingSource.Current;

            if (compra != null && compra.Id != 0 && compra.Activo == false)
            {
                label1.Visible = true;
            }
            else
            {
                label1.Visible = false;
            }
        }
    }
}
