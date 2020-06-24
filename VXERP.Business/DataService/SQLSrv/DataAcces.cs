using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CRM.Business.DataService.Secutity;

namespace CRM.Business.DataService.SQLSrv
{
    public class DataAccess
    {
        /// <summary>
        /// Obtiene un dataset.
        /// </summary>
        /// <param name="sp_Name">Nombre del store procedure.</param>
        ///  <returns>El resultado del store procedure ejecutado.</returns>
        public static DataSet GetDataSet(string storedProcedureName)
        {
            return GetDataSet(storedProcedureName, null);
        }

        /// <summary>
        /// Obtiene un dataset.
        /// </summary>
        /// <param name="sp_Name">Nombre del store procedure.</param>
        ///  <param name="sp_Param">Nombre los parámetros requeridos por el stored procedure.</param>
        ///  <returns>El resultado del store procedure ejecutado.</returns>
        public static DataSet GetDataSet(string sp_Name, params SqlParameter[] sp_Param)
        {
            SqlCommand _Command = new SqlCommand(sp_Name, new SqlConnection(AppSettings.ConnectionString));
            SqlDataAdapter _Adapter = new SqlDataAdapter(_Command);
            DataSet ds = new DataSet();

            try
            {
                _Command.CommandTimeout = 4000;
                _Command.CommandType = CommandType.StoredProcedure;
                if (sp_Param == null)
                {
                    _Adapter.Fill(ds);
                }
                else
                {
                    foreach (SqlParameter Param in sp_Param)
                    {
                        _Command.Parameters.Add(Param);
                    }

                    _Adapter.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _Command.Connection.Close();
                _Command.Parameters.Clear();
                _Command.Dispose();
                _Adapter.Dispose();
            }
        }

        /// <summary>
        /// Obtiene el primer elemento de un datarow.
        /// </summary>
        /// <param name="storedProcedureName">Nombre del store procedure.</param>
        /// <returns>El primer registro del stored procedure ejecutado</returns>
        public static DataRow GetDataSetFirstRow(string storedProcedureName)
        {
            return GetDataSetFirstRow(storedProcedureName, null);
        }

        /// <summary>
        /// Ejecuta un Querry y retorna la primera fila
        /// </summary>
        /// <param name="storedProcedureName">Nombre del store procedure.</param>
        /// <param name="StoredProcedureParameters">Nombre los parámetros requeridos por el stored procedure.</param>
        public static object ExecuteQuerryFirstRow(string Querry)
        {
            SqlCommand _Command = new SqlCommand(Querry, new SqlConnection(AppSettings.ConnectionString));
            object objectReturn;
            try
            {
                _Command.CommandType = CommandType.Text;
                _Command.Connection.Open();
                objectReturn = _Command.ExecuteScalar();
                _Command.Connection.Close();
                return objectReturn;
            }
            catch (SqlException ex)
            {
                _Command.Connection.Close();
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Obtiene el primer elemento de un dataset.
        /// </summary>
        /// <param name="storedProcedureName">Nombre del store procedure.</param>
        /// <param name="storedProcedureParameters">Nombre los parámetros requeridos por el stored procedure.</param>
        /// <returns>El resultado del store procedure ejecutado.</returns>
        public static DataRow GetDataSetFirstRow(string storedProcedureName, params SqlParameter[] storedProcedureParameters)
        {
            DataSet _DataSet = GetDataSet(storedProcedureName, storedProcedureParameters);
            //DataRow Dr;

            if (_DataSet.Tables[0].Rows.Count > 0)
            {
                return _DataSet.Tables[0].Rows[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene el Id del elemento default de un dataset.
        /// </summary>
        /// <param name="DataSetToProcess">Nombre del store procedure.</param>
        /// <param name="IdColumnName">Id del campo requerido.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetDataSetDefaultRowId(DataSet DataSetToProcess, string IdColumnName)
        {
            foreach (DataRowView dr in DataSetToProcess.Tables[0].Rows)
            {
                if (Convert.ToBoolean((dr.Row["MarcaDefault"])) == true)
                {
                    return dr.Row[IdColumnName].ToString();
                }
            }
            return "0";
        }

        /// <summary>
        /// Obtiene un elemento de un dataset.
        /// </summary>
        /// <param name="dataSetToProcess">Nombre del store procedure.</param>
        /// <param name="idColumnName">Nombre la columna.</param>
        /// <param name="idValue">Valor Id de la columna.</param>
        /// <returns></returns>
        public static DataRow GetDataSetRow(DataSet dataSetToProcess, string idColumnName, string idValue)
        {
            foreach (DataRowView _DataRow in dataSetToProcess.Tables[0].Rows)
            {
                if (Convert.ToString(_DataRow.Row[idColumnName]) == idValue.ToString())
                {
                    return _DataRow.Row;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene el paràmetro de salida de un stored procedure ejecutado.
        /// </summary>
        /// <param name="storedProcedureName">Nombre del store procedure.</param>
        /// <param name="storedProcedureOutputParameter">Nombre del paràmetro de salida.</param>
        /// <param name="storedProcedureParameters">Nombre los parámetros requeridos por el stored procedure.</param>
        /// <returns>Un objeto devuelto por el stored procedure.</returns>
        public static object GetStoredProcedureOutputParameter(string storedProcedureName, SqlParameter storedProcedureOutputParameter, params SqlParameter[] storedProcedureParameters)
        {
            SqlCommand _Command = new SqlCommand(storedProcedureName, new SqlConnection(AppSettings.ConnectionString));

            try
            {
                _Command.CommandType = CommandType.StoredProcedure;
                storedProcedureOutputParameter.Direction = ParameterDirection.Output;
                if ((storedProcedureParameters == null) == false)
                {
                    foreach (SqlParameter _Parameter in storedProcedureParameters)
                    {
                        _Command.Parameters.Add(_Parameter);
                    }
                }

                _Command.Parameters.Add(storedProcedureOutputParameter);
                _Command.Connection.Open();
                _Command.ExecuteNonQuery();
                _Command.Connection.Close();

                return storedProcedureOutputParameter.Value;
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Ejecuta un stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Nombre del store procedure.</param>
        /// <param name="StoredProcedureParameters">Nombre los parámetros requeridos por el stored procedure.</param>
        public static void ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] storedProcedureParameters)
        {
            SqlCommand _Command = new SqlCommand(storedProcedureName, new SqlConnection(AppSettings.ConnectionString));

            try
            {
                _Command.CommandTimeout = 400;
                _Command.CommandType = CommandType.StoredProcedure;
                if ((Convert.ToBoolean(storedProcedureParameters == null)) == false)
                {
                    foreach (SqlParameter _Parameter in storedProcedureParameters)
                    {
                        _Command.Parameters.Add(_Parameter);
                    }
                }
                _Command.Connection.Open();
                _Command.ExecuteNonQuery();
                _Command.Connection.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _Command.Connection.Close();
                _Command.Dispose();
            }

        }

        /// <summary>
        /// Ejecuta un Querry 
        /// </summary>
        /// <param name="storedProcedureName">Nombre del store procedure.</param>
        /// <param name="StoredProcedureParameters">Nombre los parámetros requeridos por el stored procedure.</param>
        public static DataSet ExecuteQuerry(string Querry)
        {
            SqlCommand _Command = new SqlCommand(Querry, new SqlConnection(AppSettings.ConnectionString));
            SqlDataAdapter _Adapter = new SqlDataAdapter(_Command);
            DataSet _Datos = new DataSet();
            try
            {
                _Command.CommandTimeout = 300;
                _Command.CommandType = CommandType.Text;
                _Command.Connection.Open();
                _Command.ExecuteNonQuery();
                _Command.Connection.Close();
                _Adapter.Fill(_Datos);
                return _Datos;
            }
            catch (SqlException ex)
            {
                _Command.Connection.Close();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Ejecuta un Querry y lo almacena en un DataSet
        /// </summary>
        /// <param name="storedProcedureName">Nombre del store procedure.</param>
        /// <param name="StoredProcedureParameters">Nombre los parámetros requeridos por el stored procedure.</param>
        public static void ExecuteNodeQuerry(string Querry)
        {
            SqlCommand _Command = new SqlCommand(Querry, new SqlConnection(AppSettings.ConnectionString));
            DataSet _Datos = new DataSet();
            try
            {
                _Command.CommandType = CommandType.Text;
                _Command.Connection.Open();
                _Command.ExecuteNonQuery();
                _Command.Connection.Close();
            }
            catch (SqlException ex)
            {
                _Command.Connection.Close();
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Crea un dataset.
        /// </summary>
        /// <param name="idColumnName">Id de la columna creada.</param>
        /// <param name="valueColumnName">Valor de la columna creada.</param>
        /// <param name="listToProcess">Lista de elementos a procesar.</param>
        /// <param name="defaultValue">Valor por default de la lista.</param>
        /// <returns>El dataset alterado.</returns>
        public static DataSet CreateDataset(string idColumnName, string valueColumnName, SortedList listToProcess, string defaultValue)
        {

            DataSet _DataSet = new DataSet();
            DataTable _DataTable = new DataTable();
            DataColumn _Id = new DataColumn(idColumnName);
            DataColumn _Value = new DataColumn(valueColumnName);

            DataRow _DataRow;

            _DataTable.Columns.Add(_Id);
            _DataTable.Columns.Add(_Value);

            _Id.Unique = true;
            _Value.Unique = true;

            foreach (DictionaryEntry _dictionaryEntry in listToProcess)
            {
                _DataRow = _DataTable.NewRow();
                _DataRow[idColumnName] = _dictionaryEntry.Key;
                _DataRow[valueColumnName] = _dictionaryEntry.Value;

                _DataTable.Rows.Add(_DataRow);
            }
            _DataSet.Tables.Add(_DataTable);

            return _DataSet;
        }

    }
}
