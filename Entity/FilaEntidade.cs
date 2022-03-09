using DataBaseAccessLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Ks.MonitorCPI.Entity
{
    [Serializable()]
    public class FilaEntidade
    {
        public FilaEntidade()
        {
        }

        public string tabelaId { get; set; }
        public string procId { get; set; }
        public string nvc1 { get; set; }
        public string nvc2 { get; set; }
        public string nvc3 { get; set; }
        public string nvc4 { get; set; }
        public string nvc5 { get; set; }
        public string nvc6 { get; set; }
        public string nvc7 { get; set; }
        public string nvc8 { get; set; }
        public string nvc9 { get; set; }
        public string nvc10 { get; set; }

        public DataTable Listar()
        {
            SQLAutomator sa = new SQLAutomator(this, "TB_INTEGRADOR_CONTROLE_ALTERACOES_SALESFORCE", "sequenciaId", "tabelaId", "sequenciaId");

            DataBaseAccess da = new DataBaseAccess();

            try
            {
                if (!da.open())
                    throw new Exception(da.LastMessage);

                string sSQL = sa.getSelectAllSQL("tabelaId");

                DataTable dt = da.getDataTable(sSQL, this);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.close();
            }
        }

        public bool Incluir()
        {

            if (!string.IsNullOrEmpty(nvc1))
            {
                DataBaseAccess da = new DataBaseAccess();
                try
                {
                    if (!da.open())
                        throw new Exception(da.LastMessage);

                    string sSQL = string.Format(@"insert into TB_INTEGRADOR_CONTROLE_ALTERACOES_SALESFORCE (nvc1, nvc2, tabelaId) values ('{0}','{1}','{2}')",
                        nvc1,
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        tabelaId);

                    if (!da.executeNonQuery(sSQL, this))
                        throw new Exception(da.LastMessage);

                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    da.close();
                }
            }
            return false;
        }


    }
}