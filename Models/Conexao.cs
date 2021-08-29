using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace LoginUsuario.Models
{
    public class Conexao : IDisposable
    {
        private SqlConnection SqlConn;
        public SqlTransaction SqlTrans;
        public SqlCommand SqlComm = new SqlCommand();
        private DataTable dtReturn = new DataTable();
        public String strReturn;
        private Boolean blnReturn;
        private DataTable dtReturnTrans = new DataTable();

        #region [ Properties ]

        public String textoRetorno
        {
            //Retorna o texto da procedure para saber qual foi a mensagem de erro ou de sucesso
            get
            {
                return strReturn;
            }
        }

        public Boolean resultadoTrans
        {
            //Retorna se a transação foi executada ou não com sucesso
            get
            {
                return blnReturn;
            }
        }

        public DataTable dataTableTrans
        {
            //Retorna o recordset obtido da transação pois para algumas telas é necessário pegar o código de retorno
            get
            {
                return dtReturnTrans;
            }
        }



        #endregion 

        public void OpenConnection(String pstrConnection = "Login")
        {
            SqlConn = new SqlConnection();
            SqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings[pstrConnection].ToString();
            SqlConn.Open();
            BeginTrans();
        }
        public void createParameter(String pStrName, String pstrValue)
        {
            if (pstrValue != null)
            {
                pstrValue = pstrValue.Replace("'", "´");
            }

            SqlComm.Parameters.AddWithValue(pStrName, pstrValue).Direction = ParameterDirection.Input;
        }
        public DataTable executeSelect(String pstrCommand,
                                         string pstrConnection = "Login")
        {
            DataTable dtReturn = new DataTable();
            SqlDataAdapter MyAdapter = new SqlDataAdapter();

            try
            {
                SqlConnection SqlConn = new SqlConnection();
                SqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings[pstrConnection].ToString();
                SqlComm.Connection = SqlConn;

                SqlComm.CommandType = CommandType.Text;
                SqlComm.CommandText = pstrCommand;
                MyAdapter.SelectCommand = SqlComm;
                MyAdapter.Fill(dtReturn);

                blnReturn = true;
            }
            catch (Exception ex)
            {
                blnReturn = false;
                strReturn = ex.Message.ToString();
            }

            return dtReturn;

        }
        public void closeConnection()
        {
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn = null;
            }
        }
        public void BeginTrans()
        {
            SqlTrans = SqlConn.BeginTransaction();
            SqlComm.Transaction = SqlTrans;
            blnReturn = true;
        }
        public void CommitTrans()
        {
            SqlTrans.Commit();
            closeConnection();
        }
        public void RollBackTrans()
        {
            if (SqlConn != null && SqlConn.State == ConnectionState.Open)
            {
                SqlTrans.Rollback();
                closeConnection();
            }
        }

        public List<dynamic> DataTableToListDynamic(DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }

        public List<dynamic> executeSelectList(String pstrCommand, string pstrConnection = "Login")
        {
            List<dynamic> retorno = new List<dynamic>();
            DataTable dtReturn = new DataTable();
            SqlDataAdapter MyAdapter = new SqlDataAdapter();

            try
            {
                SqlConnection SqlConn = new SqlConnection();
                SqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings[pstrConnection].ToString();
                SqlComm.Connection = SqlConn;

                SqlComm.CommandType = CommandType.Text;
                SqlComm.CommandText = pstrCommand;
                MyAdapter.SelectCommand = SqlComm;
                MyAdapter.Fill(dtReturn);

                retorno = DataTableToListDynamic(dtReturn);
                blnReturn = true;
            }
            catch (Exception ex)
            {
                blnReturn = false;
                strReturn = ex.Message.ToString();
                throw ex;
            }

            return retorno;
        }


        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {

            Dispose(true);
        }
        #endregion
    }
}
