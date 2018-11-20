using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace BL.Rentas
{
    

  public  class proveedoresBL
    {
        Contexto _contexto;

        public BindingList<Proveerdor> ListaProveedores { get; set; }

        public proveedoresBL()
        {
            _contexto = new Contexto();
            ListaProveedores = new BindingList<Proveerdor>();

        }
        public BindingList<Proveerdor> ObtenerProveedores()
        {
            _contexto.Proveerdores.Load();
            ListaProveedores = _contexto.Proveerdores.Local.ToBindingList();

            return ListaProveedores;
        }

        public void CancelarCambios()
        {
            foreach (var item in _contexto.ChangeTracker.Entries())
            {
                item.State = EntityState.Unchanged;
                item.Reload();
            }
        }
        public Resultado GuardarProveedores(Proveerdor Proveedores)
        {

            var resultado = Validar(Proveedores);
            if (resultado.Exitoso == false)
            {
                return resultado;
            }

            _contexto.SaveChanges();
            resultado.Exitoso = true;
            return resultado;
        }

        public void AgregarProveedores()
        {
            var nuevoProveedor = new Proveerdor();
            _contexto.Proveerdores.Add(nuevoProveedor);
        }

        public bool EliminarCliente(int id)
        {
            foreach (var proveedores in ListaProveedores.ToList())
            {
                if (proveedores.Id == id)
                {
                    ListaProveedores.Remove(proveedores);
                    _contexto.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        private Resultado Validar(Proveerdor Proveedores)
        {
            var resultado = new Resultado();
            resultado.Exitoso = true;

            if (Proveedores == null)
            {
                resultado.Mensaje = "Agregue un Proveedores valido";
                resultado.Exitoso = false;

                return resultado;
            }

            if (string.IsNullOrEmpty(Proveedores.Nombre) == true)
            {
                resultado.Mensaje = "Ingrese el nombre del Proveedores";
                resultado.Exitoso = false;
            }

            if (string.IsNullOrEmpty(Proveedores.Direccion) == true)
            {
                resultado.Mensaje = "Ingrese el Direccion del Proveedores";
                resultado.Exitoso = false;
            }

            if ((Proveedores.Telefono.Length != 8) == true)
            {
                resultado.Mensaje = "Ingrese el Telefono de 8 Digitos del Proveedores";
                resultado.Exitoso = false;
            }

            if (string.IsNullOrEmpty(Proveedores.CorreoElectronico) == true)
            {
                resultado.Mensaje = "Ingrese el Correo Electronico del Proveedores";
                resultado.Exitoso = false;
            }

            return resultado;
        }

    }

    public class Proveerdor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public bool Activo { get; set; }

        public Proveerdor()
        {
            Activo = true;
        }

    }
}
