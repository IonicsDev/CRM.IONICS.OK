using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Website.Security.Infrastructure;
using System.Data;
using CRM.Business.Views;

namespace CRM.Website.Controllers
{
    public class RolEmpresaController : GenericController<RolEmpresa>
    {
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        RolRepository rolRepository = new RolRepository();

        RolEmpresaRepository rolEmpresaRepository = new RolEmpresaRepository();
        UsuarioRolClienteRepository usuarioRolClienteRepository = new UsuarioRolClienteRepository();
        CRM.Business.Views.vClientes vClientes = new Business.Views.vClientes();

        public ActionResult Index()
        {
            ViewBag.Usuarios = usuarioRepository.GetFiltered(x => x.Estado == true).ToList();
            RolEmpresa rolEmpresa = new RolEmpresa();
            rolEmpresa.Roles = new List<Rol>();
            rolEmpresa.Clientes = new Business.Views.vClientes();
          
           
            return View("Index", rolEmpresa);
        }

        public ActionResult GridViewRoles()
        {
            List<Rol> rolesUsuario = (List<Rol>)Session["RolesUsuarioSeleccionado"];
            return PartialView("_GridViewRoles", rolesUsuario.ToList());
        }

        public ActionResult GridViewModalRoles()
        {
            List<Rol> rolesNoUsuario = (List<Rol>)Session["RolesNoUsuario"];
            return PartialView("_GridViewModalRoles", rolesNoUsuario.ToList());
        }

        public ActionResult GridViewClientes()
        {
            vClientes.Datos = (DataTable)Session["ClientesRolUsuario"];
            return PartialView("_ListClientes", vClientes);
        }

        public ActionResult GridViewModalClientes()
        {
            vClientes.Datos = ((DataTable)Session["ClientesNoRol"]);
            return PartialView("_GridViewClientes", vClientes);
        }

        [HttpPost]
        public ActionResult Index(int? UsuarioId, RolEmpresa rolEmpresa, string btnRoles, string btnClientes, string btnGuardar,  
            string selectedClientes, string selectedRol, string selectedModalRoles)
        {
            ViewBag.Usuarios = usuarioRepository.GetAll().ToList();
            ViewBag.IdUsuario = UsuarioId;

            try
            {
                if(UsuarioId == null)
                    throw new Exception(" Debe seleccionar un Usuario");

                var selectedUser = usuarioRepository.GetFiltered(s => s.Id == UsuarioId.Value).FirstOrDefault();

                if (selectedUser == null)
                    throw new Exception(" No existe el usuario seleccionado");

                if (btnRoles != null)
                {
                    if (selectedModalRoles == null || selectedModalRoles.Trim() == string.Empty)
                        throw new Exception(" Debe seleccionar al menos un Rol");

                    var rolesSeleccionados = GetRolesFromArray(selectedModalRoles);

                    foreach (var rol in rolesSeleccionados)
                    {
                        if (rolEmpresaRepository.GetFiltered(s => s.Rol_Id == rol.Id && s.Usuario_Id == selectedUser.Id).Any())
                            continue;

                        var newRolEmpresa = new RolEmpresa();
                        newRolEmpresa.Usuario_Id = selectedUser.Id;
                        newRolEmpresa.Rol_Id = rol.Id;
                        rolEmpresaRepository.Add(newRolEmpresa, this.User.UserID);

                        if (rol.Id == 26)
                        {
                            var rolesclientesAdministracion = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Usuario_Id == 1028 && s.Estado == true, s => s.UsuarioRol).ToList();

                            foreach (var rolscli in rolesclientesAdministracion)
                            {
                                UsuarioRolCliente urc = new UsuarioRolCliente();
                                List<RolEmpresa> rcli = rolEmpresaRepository.GetFiltered(u => u.Usuario.Id == selectedUser.Id).ToList();
                                var rolempresa = rcli[0];
                                urc.UsuarioRol_Id = rolempresa.Id;
                                urc.Cliente_Id = rolscli.Cliente_Id;
                                urc.FechaCreacion = rolscli.FechaCreacion;
                                urc.FechaActualizacion = rolscli.FechaActualizacion;
                                urc.Estado = rolscli.Estado;
                                urc.idUsuario = selectedUser.Id;
                                usuarioRolClienteRepository.Add(urc, selectedUser.Id);
                            }


                        }
                    }
                 
                    rolEmpresa.Roles = rolEmpresaRepository.GetFiltered(s => s.Usuario_Id == selectedUser.Id).Select(s => s.Rol).ToList();
                    rolEmpresa.Usuario = selectedUser;
                    rolEmpresa.Clientes = new Business.Views.vClientes();

                    SetMessage(SUCCESS, "Se han guardado correctamente los Roles.");
                    return View(rolEmpresa);
                }

                if (btnClientes != null)
                {
                    if (selectedRol == null || selectedRol.Trim() == string.Empty)
                        throw new Exception(" Debe seleccionar al menos un Rol");

                    if (selectedClientes == null ||selectedClientes.Trim()  == string.Empty)
                        throw new Exception(" Debe seleccionar al menos un Cliente");

                    //Solamente debe ser UNO
                    var rolesSeleccionados = GetRolesFromArray(selectedRol).FirstOrDefault();

                    var clientesSeleccionados = GetClientesFromArray(selectedClientes);

                    var rolesEmpresa = rolEmpresaRepository.GetFiltered(s=>s.Usuario_Id == selectedUser.Id && s.Rol_Id == rolesSeleccionados.Id).ToList();


                    foreach (var re in rolesEmpresa)
                    {
                        foreach (var cliente in clientesSeleccionados)
                        {
                            if (usuarioRolClienteRepository.GetFiltered(s => s.Cliente_Id == cliente.Id && s.UsuarioRol_Id == re.Id).Any())
                                continue;

                            var usuarioRolCliente = new UsuarioRolCliente();
                            usuarioRolCliente.UsuarioRol_Id = re.Id;
                            usuarioRolCliente.Cliente_Id = cliente.Id;

                            usuarioRolClienteRepository.Add(usuarioRolCliente, this.User.UserID);

                        }
                    }

                    rolEmpresa.Clientes = new Business.Views.vClientes();

                    rolEmpresa.Roles = rolEmpresaRepository.GetFiltered(s => s.Usuario_Id == selectedUser.Id).Select(s => s.Rol).ToList();
                    rolEmpresa.Usuario = selectedUser;
                
                    //TO DO
                    var userRolClient = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Rol_Id == rolesSeleccionados.Id && s.UsuarioRol.Usuario_Id == selectedUser.Id).ToList();

                    foreach (var item in userRolClient)
                    {
                        var listRow = (new CRM.Business.Views.vClientes()).GetViewModel().AsEnumerable()
                                .Where(r => r.Field<int>("ID") == item.Cliente_Id).ToList();
                        foreach (DataRow dr in listRow)
                            rolEmpresa.Clientes.Datos.Rows.Add(dr.ItemArray);
                    }

                    //rolEmpresa.Clientes = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Rol_Id == rolesSeleccionados.Id && s.UsuarioRol.Usuario_Id == selectedUser.Id).Select(s => s.Cliente).ToList();

                    ViewBag.IdRol = rolesSeleccionados.Id; //Lo recojo cuando se carga la grilla

                    SetMessage(SUCCESS, " Se han guardado correctamente los Clientes.");
                    return View(rolEmpresa);

                }

            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);

                List<Rol> rolesUsuario = rolEmpresaRepository.GetFiltered(s => s.Usuario_Id == UsuarioId.Value).Select(s => s.Rol).ToList();
                if (rolesUsuario == null)
                {
                    rolEmpresa.Roles = new List<Rol>();
                }
                else
                {
                    rolEmpresa.Roles = rolesUsuario;
                }

                if (selectedRol == null || selectedRol.Trim() == string.Empty)
                {
                    rolEmpresa.Clientes = new Business.Views.vClientes();
                }
                else
                {
                    var rolesSeleccionados = GetRolesFromArray(selectedRol).FirstOrDefault();
                    var userRolClient = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Rol_Id == rolesSeleccionados.Id
                                                                                && s.UsuarioRol.Usuario_Id == UsuarioId.Value && s.Estado == true).ToList();

                   if(userRolClient == null || userRolClient.Count == 0)
                       rolEmpresa.Clientes = new Business.Views.vClientes();

                   if (rolEmpresa.Clientes == null)
                       rolEmpresa.Clientes = new Business.Views.vClientes();

                    foreach (var item in userRolClient)
                    {
                    var listRow =  (new CRM.Business.Views.vClientes()).GetViewModel().AsEnumerable()
                            .Where(r => r.Field<int>("ID") == item.Cliente_Id).ToList();
                        foreach(DataRow dr in listRow)
                            rolEmpresa.Clientes.Datos.Rows.Add(dr.ItemArray);
                    }
                    //TO DO
                    //rolEmpresa.Clientes = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Rol_Id == rolesSeleccionados.Id && s.UsuarioRol.Usuario_Id == UsuarioId.Value).Select(s => s.Cliente).ToList();

                    ViewBag.IdRol = rolesSeleccionados.Id; //Lo recojo cuando se carga la grilla
                }
            }

            return View(rolEmpresa);
        }

        public List<CRM.Business.Views.vClientes> GetClientesFromArray(String clientesChequeados)
        {
            //Este if es para el modal cuando se agrega un rol al listado
            List<CRM.Business.Views.vClientes> clientes = new List<CRM.Business.Views.vClientes>();

            String[] arrayClientes = clientesChequeados.Split(',');

            foreach (String idCliente in arrayClientes)
            {
                if (idCliente != "")
                {
                    int idClienteInt = Int32.Parse(idCliente);
                    CRM.Business.Views.vClientes newCliente = (new CRM.Business.Views.vClientes()).GetById(idClienteInt);

                    if (!clientes.Any(x => x.Id == newCliente.Id))
                    {
                        clientes.Add(newCliente);
                    } 
                }
            }
            return clientes;
        }

        public List<Rol> GetRolesFromArray(String rolesChequeados)
        {
            //Este if es para el modal cuando se agrega un rol al listado
            List<Rol> roles = new List<Rol>();

            String[] arrayroles = rolesChequeados.Split(',');

            foreach (String idRol in arrayroles)
            {
                if (idRol != "")
                {
                    int idRolInt = Int32.Parse(idRol);
                    Rol newRol = rolRepository.GetFiltered(x => x.Id == idRolInt).SingleOrDefault();

                    if (!roles.Any(x => x.Id == newRol.Id))
                    {
                        roles.Add(newRol);
                    } 
                }
            }
            return roles;
        }

        public ActionResult GetRolesUsuario(string idUsuario) {
            RolEmpresa rolEmpresa = new RolEmpresa();
            ViewBag.Usuarios = usuarioRepository.GetFiltered(x => x.Estado == true).ToList();
            if (idUsuario != "")
            {
                int idUsuarioInt = Int32.Parse(idUsuario);
                ViewBag.IdUsuario = idUsuario;

                List<Rol> rolesUsuario = rolEmpresaRepository.GetFiltered(x => x.Usuario.Id == idUsuarioInt).Select(x => x.Rol).ToList();

                if (rolesUsuario.Count > 0)
                {
                    rolEmpresa.Roles = rolesUsuario;
                }
                else {
                    rolEmpresa.Roles = new List<Rol>();
                }
            }

            if (rolEmpresa.Roles == null)
                rolEmpresa.Roles = new List<Rol>();

            Session["RolesUsuarioSeleccionado"] = rolEmpresa.Roles.ToList(); 

            return PartialView("_GridViewRoles", rolEmpresa.Roles.ToList());
        }

        public ActionResult LimpiarTablaClientes()
        {
            return PartialView("_ListClientes", new vClientes());
        }

        public ActionResult GetClientesRolUsuario(string idRol, string idUsuario)
        {
            try
            {
                int rolId = 0;
                if (!int.TryParse(idRol, out rolId))
                    throw new Exception(" Debe seleccionar un rol");

                var Rol = rolRepository.Get(rolId).FirstOrDefault();

                if (Rol == null)
                    throw new Exception(" No se encontró el Rol en el sistema");

                int userId = 0;
                if (!int.TryParse(idUsuario, out userId))
                    throw new Exception(" Debe seleccionar un usuario");

                var Usuario = usuarioRepository.Get(userId).FirstOrDefault();

                if (Usuario == null)
                    throw new Exception(" No se encontró al usuario en el sistema");

                RolEmpresa rolEmpresa = new RolEmpresa();
                rolEmpresa.Clientes = new vClientes();
                ViewBag.Usuarios = usuarioRepository.GetAll().ToList();

                var userRolClient = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Rol_Id == Rol.Id && s.UsuarioRol.Usuario_Id == Usuario.Id && s.Estado == true).ToList();

                var tablaClientesFull = (new CRM.Business.Views.vClientes()).GetViewModel().AsEnumerable();

                //foreach (var item in userRolClient)
                //{
                //    var listRow = (new CRM.Business.Views.vClientes()).GetViewModel().AsEnumerable()
                //            .Where(r => r.Field<int>("ID") == item.Cliente_Id).ToList();
                //    foreach (DataRow dr in listRow)
                //        rolEmpresa.Clientes.Datos.Rows.Add(dr.ItemArray);

                //}

                //foreach (var item in userRolClient)
                //{
                //    var listRow = (new CRM.Business.Views.vClientes()).GetViewModel().AsEnumerable()
                //            .Where(r => r.Field<int>("ID") == item.Cliente_Id).ToList();
                //    foreach (DataRow dr in listRow)
                //        rolEmpresa.Clientes.Datos.Rows.Add(dr.ItemArray);

                //}

               
              //  List<Cliente> clientesRolUser = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Rol_Id == Rol.Id && s.UsuarioRol.Usuario_Id == Usuario.Id).Select(s => s.Cliente).ToList();

                if (userRolClient.Count > 0)
                {
                    rolEmpresa.Clientes = rolEmpresa.Clientes;
                }
                else
                {
                    rolEmpresa.Clientes =  new vClientes();
                }

                if (rolEmpresa.Clientes == null)
                    rolEmpresa.Clientes = new vClientes(); 

                Session["ClientesRolUsuario"] = rolEmpresa.Clientes.Datos;

                return PartialView("_ListClientes", rolEmpresa.Clientes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddRol(string UsuarioId)
        {
            List<Rol> rolesNoUsuario = rolRepository.GetFiltered(x => x.Estado == true).ToList();

            ViewBag.Usuarios = usuarioRepository.GetFiltered(x => x.Estado == true).ToList();
            if (UsuarioId != "" && UsuarioId != null)
            {
                int idUsuarioInt = Int32.Parse(UsuarioId);
                ViewBag.IdUsuario = UsuarioId;

                List<Rol> rolesUsuario = rolEmpresaRepository.GetFiltered(x => x.Usuario.Id == idUsuarioInt).Select(x => x.Rol).ToList();

                rolesNoUsuario = rolesNoUsuario.Where(x => !rolesUsuario.Any(y => y.Id == x.Id)).ToList();
            }
            Session["RolesNoUsuario"] = rolesNoUsuario.ToList();
            return PartialView("_Roles", rolesNoUsuario); 
        }

        public ActionResult AddCliente(string selectedRol, string IdUsuario)
        {
            int userId = 0;
            if (!int.TryParse(IdUsuario, out userId))
                throw new Exception(" No existe el usuario seleccionado");

            int rolId = 0;
            if (!int.TryParse(selectedRol, out rolId))
                throw new Exception(" No existe el rol seleccionado");

            vClientes clientes = new Business.Views.vClientes();
            var clientesNoRol = clientes.GetViewModel().AsEnumerable();

            //List<Cliente> clientesNoRol = clienteRepository.GetAll().ToList();

            RolEmpresa usuarioRol = rolEmpresaRepository.GetFiltered(x => x.Rol_Id == rolId && x.Usuario_Id == userId).FirstOrDefault();

            var clientesRolUsuario = usuarioRolClienteRepository.GetFiltered(x => x.UsuarioRol_Id == usuarioRol.Id && x.Estado == true).ToList();

           // List<Cliente> clientesRolUsuario = usuarioRolClienteRepository.GetFiltered(x => x.UsuarioRol_Id == usuarioRol.Id).Select(x => x.Cliente).ToList();

            clientesNoRol = clientesNoRol.Where(x => !clientesRolUsuario.Any(y => y.Cliente_Id == x.Field<Int32>("ID")));
            clientes.Datos = clientesNoRol.CopyToDataTable();
            Session["ClientesNoRol"] = clientesNoRol.CopyToDataTable();
            return PartialView("_Clientes", clientes);
        }

        public ActionResult DeleteCliente(int Id, string UsuarioId, string RolId)
        {
            int ClienteId = Id;
            int userId = 0;
            int rolId = 0;

            ViewBag.Usuarios = usuarioRepository.GetAll().ToList();

            try
            {
                if (!int.TryParse(UsuarioId, out userId))
                    throw new Exception(" No se pudo eliminar el Cliente");
                
                if (!int.TryParse(RolId, out rolId))
                    throw new Exception(" No se pudo eliminar el Cliente");

                RolEmpresa usuarioRol = rolEmpresaRepository.GetFiltered(x => x.Rol_Id == rolId && x.Usuario_Id == userId).FirstOrDefault();

                if (usuarioRol == null)
                    throw new Exception(" No se encontró al rol en el sistema");

                List<UsuarioRolCliente> usuarioRolClientes = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Id == usuarioRol.Id && s.UsuarioRol.Usuario_Id == userId
                                                                                                    && s.Estado == true).ToList();

                if (usuarioRolClientes == null)
                    throw new Exception(" No se encontraron clientes relacionados al rol seleccionado");

                UsuarioRolCliente usuarioRolClienteSeleccionado = usuarioRolClientes.Where(x => x.Cliente_Id == ClienteId).FirstOrDefault();
                usuarioRolClienteRepository.RemoveFromDataBase(usuarioRolClienteSeleccionado, userId);

                RolEmpresa rolEmpresa = new RolEmpresa();
                rolEmpresa.Roles = rolEmpresaRepository.GetFiltered(x => x.Usuario_Id == userId).Select(x => x.Rol).ToList();

                RolEmpresa rolUsuario = rolEmpresaRepository.GetFiltered(x => x.Rol_Id == rolId && x.Usuario_Id == userId).FirstOrDefault();

                if (rolUsuario == null)
                    throw new Exception(" No se encontró al rol en el sistema");

                //TODO
                //rolEmpresa.Clientes = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Id == rolUsuario.Id && s.UsuarioRol.Usuario_Id == userId).Select(s => s.Cliente).ToList();

                rolEmpresa.Clientes = new Business.Views.vClientes();
                var userRolClient = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Id == rolUsuario.Id && s.UsuarioRol.Usuario_Id == userId).ToList();

                foreach (var item in userRolClient)
                {
                    var listRow = (new CRM.Business.Views.vClientes()).GetViewModel().AsEnumerable()
                            .Where(r => r.Field<int>("ID") == item.Cliente_Id).ToList();
                    foreach (DataRow dr in listRow)
                        rolEmpresa.Clientes.Datos.Rows.Add(dr.ItemArray);

                }

             

                ViewBag.IdRol = rolId; //Lo recojo cuando se carga la grilla

                SetMessage(SUCCESS, " El Cliente se eliminó correctamente");
                
                return View("Index", rolEmpresa);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                RolEmpresa rolEmpresa = new RolEmpresa();
                rolEmpresa.Roles = rolEmpresaRepository.GetFiltered(x => x.Usuario_Id == userId).Select(x => x.Rol).ToList();

                RolEmpresa rolUsuario = rolEmpresaRepository.GetFiltered(x => x.Rol_Id == rolId && x.Usuario_Id == userId).FirstOrDefault();

                if (rolUsuario == null)
                {
                    rolEmpresa.Clientes = new vClientes();
                }
                else
                {
                  //  rolEmpresa.Clientes = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Id == rolUsuario.Id && s.UsuarioRol.Usuario_Id == userId).Select(s => s.Cliente).ToList();
                    //TO DO

                    rolEmpresa.Clientes = new Business.Views.vClientes();
                    var userRolClient = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Id == rolUsuario.Id && s.UsuarioRol.Usuario_Id == userId && s.Estado == true).ToList();

                    foreach (var item in userRolClient)
                    {
                        var listRow = (new CRM.Business.Views.vClientes()).GetViewModel().AsEnumerable()
                                .Where(r => r.Field<int>("ID") == item.Cliente_Id).ToList();
                        foreach (DataRow dr in listRow)
                            rolEmpresa.Clientes.Datos.Rows.Add(dr.ItemArray);

                    }
                }
                ViewBag.IdRol = rolId; //Lo recojo cuando se carga la grilla

                return View("Index", rolEmpresa);
            }
        }

        public ActionResult DeleteRol(int Id, string UsuarioId)
        {
            ViewBag.Usuarios = usuarioRepository.GetFiltered(x => x.Estado == true).ToList();
            RolEmpresa rolEmpresa = new RolEmpresa();

            int userId = 0;

            try 
	        {
                if (!int.TryParse(UsuarioId, out userId))
                    throw new Exception(" No se pudo eliminar el Cliente");

                RolEmpresa rolUsuario = rolEmpresaRepository.GetFiltered(x => x.Usuario_Id == userId && x.Rol_Id == Id).FirstOrDefault();

                if (rolUsuario == null)
                    throw new Exception(" No se encontró al rol en el sistema");

                List<UsuarioRolCliente> clientesRolUsuario = usuarioRolClienteRepository.GetFiltered(x => x.UsuarioRol_Id == rolUsuario.Id && x.Estado == true).ToList();
                if (clientesRolUsuario != null)
                {
                    foreach (UsuarioRolCliente clienteRolUsuario in clientesRolUsuario)
                    {
                        usuarioRolClienteRepository.RemoveFromDataBase(clienteRolUsuario, this.User.UserID);
                    }
                }

                rolEmpresaRepository.RemoveFromDataBase(rolUsuario, this.User.UserID);
                SetMessage(SUCCESS, " Se eliminó correctamente el Rol");
	        

                List<Rol> rolesUsuario = rolEmpresaRepository.GetFiltered(x => x.Usuario.Id == userId).Select(x => x.Rol).ToList();

                if (rolesUsuario.Count > 0)
                {
                    rolEmpresa.Roles = rolesUsuario;
                }
                else
                {
                    rolEmpresa.Roles = new List<Rol>();
                }

                if (rolEmpresa.Roles == null)
                    rolEmpresa.Roles = new List<Rol>();

                rolEmpresa.Clientes = new vClientes();

                return View("Index", rolEmpresa);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);

                List<Rol> rolesUsuario = rolEmpresaRepository.GetFiltered(x => x.Usuario.Id == userId).Select(x => x.Rol).ToList();

                if (rolesUsuario.Count > 0)
                {
                    rolEmpresa.Roles = rolesUsuario;
                }
                else
                {
                    rolEmpresa.Roles = new List<Rol>();
                }

                if (rolEmpresa.Roles == null)
                    rolEmpresa.Roles = new List<Rol>();

                rolEmpresa.Clientes = new vClientes();

                return View("Index", rolEmpresa);
            }
        }
    }
}
