using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Rentas
{
    public class ComprasBL
    {
        Contexto _contexto;

        public BindingList<Compra> ListaCompra { get; set; }

        public ComprasBL()
        {
            _contexto = new Contexto();
        }
        public BindingList<Compra> ObtenerCompras()
        {
            _contexto.compras.Include("CompraDetalle").Load();
            ListaCompra = _contexto.compras.Local.ToBindingList();

            return ListaCompra;
        }

        public void AgregarCompra()
        {
            var nuevaCompra = new Compra();
            _contexto.compras.Add(nuevaCompra);
        }

        public void AgregarCompraDetalle(Compra compra)
        {
            if (compra != null)
            {
                var CompraDetalle = new compraDetalle();
                compra.compraDetalle.Add(CompraDetalle);
            }
        }

        public void RemoverComprasDetalle(Compra compra, compraDetalle comprasDetalle)
        {
            if (compra != null && comprasDetalle != null)
            {
                compra.compraDetalle.Remove(comprasDetalle);
            }
        }

        public void CancelarCambios()
        {
            foreach (var item in _contexto.ChangeTracker.Entries())
            {
                item.State = EntityState.Unchanged;
                item.Reload();
            }
        }

        public Resultado GuardarCopmra(Compra compra)
        {
            var resultado = Validar(compra);
            if (resultado.Exitoso == false)
            {
                return resultado;
            }

            CalcularExistencia(compra);

            _contexto.SaveChanges();
            resultado.Exitoso = true;
            return resultado;
        }

        private void CalcularExistencia(Compra compra)
        {
            foreach (var detalle in compra.compraDetalle)
            {
                var producto = _contexto.Productos.Find(detalle.ProductoId);
                if (producto != null)
                {
                    if (compra.Activo == true)
                    {
                        producto.Existencia = producto.Existencia + detalle.Cantidad;
                    }
                    else
                    {
                        producto.Existencia = producto.Existencia - detalle.Cantidad;
                    }
                }
            }
        }

        private Resultado Validar(Compra compra)
        {
            var resultado = new Resultado();
            resultado.Exitoso = true;

            if (compra == null)
            {
                resultado.Mensaje = "Agregue una Compra para poderla salvar";
                resultado.Exitoso = false;

                return resultado;
            }

            if (compra.Id != 0 && compra.Activo == true)
            {
                resultado.Mensaje = "La compra ya fue emitida y no se pueden realizar cambios en ella";
                resultado.Exitoso = false;
            }

            if (compra.Activo == false)
            {
                resultado.Mensaje = "La factura esta anulada y no se pueden realizar cambios en ella";
                resultado.Exitoso = false;
            }

            if (compra.proveedorId == 0)
            {
                resultado.Mensaje = "Seleccione un Proveedor";
                resultado.Exitoso = false;
            }

            if (compra.compraDetalle.Count == 0)
            {
                resultado.Mensaje = "Agregue productos a la factura";
                resultado.Exitoso = false;
            }

            foreach (var detalle in compra.compraDetalle)
            {
                if (detalle.ProductoId == 0)
                {
                    resultado.Mensaje = "Seleccione productos validos";
                    resultado.Exitoso = false;
                }
            }


            return resultado;
        }

        public void CalcularCompra(Compra compra)
        {
            if (compra != null)
            {
                double subtotal = 0;

                foreach (var detalle in compra.compraDetalle)
                {
                    var producto = _contexto.Productos.Find(detalle.ProductoId);
                    if (producto != null)
                    {
                        detalle.Precio = producto.Precio;
                        detalle.Total = detalle.Cantidad * producto.Precio;

                        subtotal += detalle.Total;
                    }
                }

                compra.Subtotal = subtotal;
                compra.Impuesto = subtotal * 0.15;
                compra.Total = subtotal + compra.Impuesto;
            }
        }

        public bool AnularCompra(int id)
        {
            foreach (var compra in ListaCompra)
            {
                if (compra.Id == id)
                {
                    compra.Activo = false;

                    CalcularExistencia(compra);

                    _contexto.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }




public class Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int proveedorId { get; set; }
        public Proveerdor proveedor { get; set; }
        public BindingList<compraDetalle> compraDetalle { get; set; }
        public double Subtotal { get; set; }
        public double Impuesto { get; set; }
        public double Total { get; set; }
        public bool Activo { get; set; }

        public Compra()
        {
            Fecha = DateTime.Now;
            compraDetalle = new BindingList<compraDetalle>();
            Activo = true;
        }

       
    }

    public class compraDetalle
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double Total { get; set; }

        public compraDetalle()
        {
            Cantidad = 1;
        }
    }

}
