using CRM.Common.EntityDomain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace CRM.Business.Views.BaseViews
{
    [Serializable]
    public abstract class BaseView : EntityTypedId<int>
    {
        private string VIEW_NAME;
        private string SP_NAME;
        private DataTable _Datos;
        private Boolean IS_STOREPROCDURE = false;
        private SqlParameter[] SP_PARAMS;
        protected IList<Entities.UsuarioRolCliente> _usuarioClientes;


        public  dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            return new DynObject(properties);
        }


        public BaseView(string viewName, IList<vEmpleadosEnIonics> usuarioClientes)
        {
            this.VIEW_NAME = viewName;
        }

        public BaseView(string viewName, IList<Entities.UsuarioRolCliente> usuarioClientes)
        {
            this.VIEW_NAME = viewName;
            this._usuarioClientes = usuarioClientes;
        }

        public BaseView(string spName, bool isSP, params SqlParameter[] sp_Param)
        {
            this.SP_NAME = spName;
            this.IS_STOREPROCDURE = true;
            this.SP_PARAMS = sp_Param;
        }

        protected BaseView(string vIEW_NAME)
        {
            VIEW_NAME = vIEW_NAME;
        }

        public IEnumerable<DynObject> GetDynamicCollectionList(DataTable datos)
        {
            Dictionary<string, object> propertie ;
            List<DynObject> list = new List<DynObject>();



            foreach (DataRow dr in datos.Rows)
            {
                propertie= new Dictionary<string,object>();
                foreach (DataColumn dc in datos.Columns)
                {
                    propertie.Add(dc.ColumnName, dr[dc]);
                }
                list.Add(GetDynamicObject(propertie));
            }

            return list;
        }

        public DataTable Datos
        {
            get
            {
                if (_Datos == null)
                {
                    _Datos = this.GetEmpyViewModel();
                    return _Datos;
                }
                return _Datos;
            }
            set
            {
                _Datos = value;
            }
        }

        public DataTable GetViewModel()
        {
            DataTable datos = null;
            try
            {

                if (this.IS_STOREPROCDURE)
                {
                    datos = DataService.SQLSrv.DataAccess.GetDataSet( this.SP_NAME, this.SP_PARAMS).Tables[0];
                }
                else
                {

                    switch (VIEW_NAME)
                    {
                        case vClientes.VIEW_NAME:
                            {
                                if (_usuarioClientes != null)
                                {
                                    string queryFilter = " ID in ( ";
                                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                    sb.Append(queryFilter);

                                    foreach (var clienteRol in this._usuarioClientes)
                                    {
                                        sb.Append(clienteRol.Cliente_Id.ToString());
                                        sb.Append(",");

                                    }

                                    sb.Remove(sb.Length - 1, 1);
                                    sb.Append(") ");
                                    datos = GetByFilter(sb.ToString());
                                }
                                else
                                {
                                    datos = DataService.SQLSrv.DataAccess.ExecuteQuerry("SELECT * FROM " + this.VIEW_NAME).Tables[0];
                                }

                                break;
                            }
                        default:
                            {
                                datos = DataService.SQLSrv.DataAccess.ExecuteQuerry("SELECT * FROM " + this.VIEW_NAME).Tables[0];
                                break;
                            }
                    }
                 
                 
                }
                return datos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public DataTable GetViewModel_SP(params SqlParameter[] sp_param)
        {
            DataTable datos = null;
            try
            {

                if (this.IS_STOREPROCDURE)
                {
                    datos = DataService.SQLSrv.DataAccess.GetDataSet(this.SP_NAME, sp_param).Tables[0];
                }
                
                return datos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public DataTable GetEmpyViewModel()
        {

            try
            {
                DataTable datos = DataService.SQLSrv.DataAccess.ExecuteQuerry("SELECT  * FROM " + this.VIEW_NAME  + " WHERE ID = -99").Tables[0];

                return datos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public virtual DataTable GetByID(int Id)
        {
            DataTable datos = null;
           
            try
            {
                if (this.IS_STOREPROCDURE)
                {
                    datos = DataService.SQLSrv.DataAccess.GetDataSet(this.SP_NAME, this.SP_PARAMS).Tables[0];
                }
                else
                {
                    datos = DataService.SQLSrv.DataAccess.ExecuteQuerry("SELECT * FROM " + this.VIEW_NAME + " WHERE ID =" + Id).Tables[0];

                }
                return datos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public DataTable GetByFilter(string queryFilter)
        {
            DataTable datos = null;
            try
            {
                if (!this.IS_STOREPROCDURE)
                {
                    datos = DataService.SQLSrv.DataAccess.ExecuteQuerry("SELECT * FROM " + this.VIEW_NAME + " WHERE  " + queryFilter).Tables[0];
                }
               
                return datos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public sealed class DynObject : DynamicObject
        {
            private readonly Dictionary<string, object> _properties;

            public DynObject(Dictionary<string, object> properties)
            {
                _properties = properties;
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return _properties.Keys;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    result = _properties[binder.Name];
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    _properties[binder.Name] = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }


        }
    }
}