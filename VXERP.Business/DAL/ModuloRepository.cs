using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Common.EntityDomain;

namespace CRM.Business.DAL
{

    public class ModuloRepository : DelRepository<Modulo, int>
    {
        public ModuloRepository()
            : base(new ConfigurationContext())
        {

        }

        /// <summary>
        /// Obtiene los Modulos Permitidos de un Perfil
        /// </summary>
        /// <param name="RolesEmpresa"></param>
        /// <returns></returns>
        public List<Modulo> GetModulosUser(ICollection<RolEmpresa> RolesEmpresa, ICollection<Modulo> Modulos)
        {
            RolEmpresa re = null;
            //if (perfilUsuario != null)
                re = RolesEmpresa.First();

            if (re == null && RolesEmpresa.Count() > 0)
            {
                re = re = RolesEmpresa.First();
            }
            List<Modulo> listResult = new List<Modulo>();
            try
            {
                foreach (var ob2 in re.Rol.ModulosPermiso)
                {
                    Modulo modulo2 = Modulos.Where(f => f.Id == 111  && f.Visible).FirstOrDefault();
                    Modulo modulo = Modulos.Where(f => f.Id == ob2.Modulo_Id && f.Visible).FirstOrDefault();
                    if (modulo != null)
                    {
                        if(modulo.Id == 111)
                        {

                        }

                        var itemAdd = listResult.FirstOrDefault(f => f.Id == modulo.Id);
                        if (itemAdd == null)
                            listResult.Add(modulo);


                        var parents = GetParentsList(modulo, Modulos);

                        foreach (var parent in parents)
                        {
                            var item = listResult.FirstOrDefault(f => f.Id == parent.Id);
                            if (item == null)
                                listResult.Add(parent);

                        }

                    }
                }

          
            }
            catch (Exception)
            {
            }

            return listResult;
        }

        /// <summary>
        /// Obtiene los hijos de un modulo
        /// </summary>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public List<Modulo> GetChildsList(Modulo modulo, ICollection<Modulo> Modulos)
        {
            List<Modulo> listChildsResult = new List<Modulo>();

            var result = Modulos.Where(f => f.Parent_Id == modulo.Id).ToList();
            foreach (var item in result)
            {
                listChildsResult.Add(item);
                var childs =GetChildsList(item, Modulos);
                listChildsResult.AddRange(childs);
            }

            return listChildsResult;
        }

        /// <summary>
        /// Obtiene los Padres de un Modulo
        /// </summary>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public List<Modulo> GetParentsList(Modulo modulo, ICollection<Modulo> Modulos)
        {
            List<Modulo> listParentResult = new List<Modulo>();
            if (modulo.Parent_Id != null)
            {
                Modulo parent;
                if (modulo.Parent == null)
                    parent = Modulos.Where(p => p.Id == modulo.Parent_Id.Value && p.Estado == true).SingleOrDefault(); //parent = (new ModuloRepository()).Get(modulo.Parent_Id.Value, p => p.Parent).SingleOrDefault();
                else
                    parent = modulo.Parent;

                listParentResult.Add(parent);

                if (parent.Parent_Id != null)
                {
                    Modulo newParent;
                    if (parent.Parent == null)
                        newParent = Modulos.Where(s => s.Id == parent.Parent_Id.Value).SingleOrDefault(); //  newParent = (new ModuloRepository()).Get(parent.Parent_Id.Value, p => p.Parent).SingleOrDefault();
                    else
                        newParent = parent.Parent;

                    listParentResult.Add(newParent);
                    GetParent(newParent, ref listParentResult, Modulos);
                }

            }
            return listParentResult;
        }

        private void GetParent(Modulo modulo, ref List<Modulo> listParentResult, ICollection<Modulo> Modulos)
        {
            if (modulo.Parent_Id != null)
            {
                Modulo parent;
                if (modulo.Parent == null)
                    parent = Modulos.Where(p => p.Id == modulo.Parent_Id.Value).SingleOrDefault();//  parent = (new ModuloRepository()).Get(modulo.Parent_Id.Value, p => p.Parent).SingleOrDefault();
                else
                    parent = modulo.Parent;

                listParentResult.Add(parent);

                if (parent.Parent_Id != null)
                {
                    Modulo newParent;
                    if (parent.Parent == null)
                        newParent = Modulos.Where(p => p.Id == parent.Parent_Id.Value).SingleOrDefault();// newParent = (new ModuloRepository()).Get(parent.Parent_Id.Value, p => p.Parent).SingleOrDefault();
                    else
                        newParent = parent.Parent;

                    GetParent(newParent, ref listParentResult,Modulos);
                }
            }

        }

     
    }
}
