using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Taoqi
{
    public class DAL
    {
        /// <summary>
        /// 获取表和视图数据
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="ht"></param>
        /// <param name="total"></param>
        /// <param name="orderBy"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static DataTable GetTable(string viewName,
            Hashtable ht = null,
            int total = 0,
            string orderBy = null,
            string fields = "*",
            int startIndex = 0,
            int endIndex = 0)
        {
            DataTable dt = new DataTable();

            try
            {
                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();

                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        if (total > 0)
                        {
                            cmd.CommandText = string.Format("select top {0} {1} from {2} where 1=1 ", total, fields, viewName);
                        }
                        else
                        {
                            cmd.CommandText = string.Format("select {0} from {1} where 1=1 ", fields, viewName);
                        }

                        if (total == 0 && endIndex > 0 && orderBy != null) //分页查询
                        {
                            cmd.CommandText = string.Format(@"WITH cte AS( 
                                    select ROW_NUMBER() over(order by {0}) as RowIndex, 
                                    {1} 
                                    from {2} 
                                    where 1=1 ", orderBy, fields, viewName);
                        }

                        if (ht != null)
                        {
                            foreach (DictionaryEntry dic in ht)
                            {
                                Sql.AppendParameter(cmd, Convert.ToString(dic.Value), Convert.ToString(dic.Key));
                            }
                        }

                        if (total == 0 && endIndex > 0 && orderBy != null) //分页查询
                        {
                            cmd.CommandText = cmd.CommandText + string.Format(@" ) 
                                select * from cte where RowIndex between {0} and {1} ", startIndex, endIndex);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(orderBy)) cmd.CommandText = cmd.CommandText + "order by " + orderBy;
                        }

                        using (DbDataAdapter da = dbf.CreateDataAdapter())
                        {
                            ((IDbDataAdapter)da).SelectCommand = cmd;
                            da.Fill(dt);
                        }
                    }
                }
                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetTable(string viewName, string where, string orderBy = null, string fields = "*")
        {
            DataTable dt = new DataTable();

            try
            {
                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();

                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = string.Format("select {0} from {1} where 1=1 ", fields, viewName);

                        if (!string.IsNullOrEmpty(where)) cmd.CommandText += "and " + where;
                        if (!string.IsNullOrEmpty(orderBy)) cmd.CommandText += "order by " + orderBy;

                        using (DbDataAdapter da = dbf.CreateDataAdapter())
                        {
                            ((IDbDataAdapter)da).SelectCommand = cmd;
                            da.Fill(dt);
                        }
                    }
                }
                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static DataRow GetRow(string viewName, Hashtable ht = null, string fields = "*")
        {
            DataRow row = null;
            DataTable dt = GetTable(viewName, ht, 1, null, fields);
            if (dt.Rows.Count > 0) row = dt.Rows[0];
            return row;
        }

        /// <summary>
        /// 获取查询结果数目
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static int GetTotalByViewName(string viewName, Hashtable ht = null)
        {
            int total = 0;

            try
            {

                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();

                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = string.Format("select count(*) from {0} where 1=1", viewName);

                        if (ht != null)
                        {
                            foreach (DictionaryEntry dic in ht)
                            {
                                Sql.AppendParameter(cmd, Convert.ToString(dic.Value), Convert.ToString(dic.Key));
                            }
                        }

                        total = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return total;
        }

        public static object ExecuteScalar(string viewName, Hashtable ht = null, string select = "*", string orderBy = null)
        {
            object result = null;
            try
            {
                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();

                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = string.Format("select {0} from {1} where 1=1", select, viewName);

                        if (ht != null)
                        {
                            foreach (DictionaryEntry dic in ht)
                            {
                                Sql.AppendParameter(cmd, Convert.ToString(dic.Value), Convert.ToString(dic.Key));
                            }
                        }

                        if (orderBy != null) cmd.CommandText = cmd.CommandText + " order by " + orderBy;

                        result = cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// 获取存储过程数据
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="ht"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static DataTable GetDatatableByProcedure(string proName, Hashtable ht = null, string orderBy = null)
        {
            DataTable dt = new DataTable();
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbCommand cmd = con.CreateCommand())
                {

                    cmd.CommandText = proName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (ht != null)
                    {
                        foreach (DictionaryEntry dic in ht)
                        {
                            Sql.AddParameter(cmd, Convert.ToString(dic.Key), Convert.ToString(dic.Value));
                        }
                    }

                    if (orderBy != null)
                    {
                        cmd.CommandText = cmd.CommandText + " "
                            + orderBy;
                    }
                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;

                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 目前值针对 用户上传使用
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strColumnName"></param>
        /// <param name="strColumnValue"></param>
        /// <returns></returns>
        public static bool UpdateTableByColumn(string strTableName, string strColumnName, string strColumnValue, string strwhere, string strwhereValue)
        {
            try
            {
                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();

                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = string.Format(@"update {0} set {1}='{2}' where {3}='{4}'", strTableName, strColumnName, strColumnValue, strwhere, strwhereValue);

                        using (DbDataAdapter da = dbf.CreateDataAdapter())
                        {
                            ((IDbDataAdapter)da).SelectCommand = cmd;
                            cmd.ExecuteScalar();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return false;
        }
        /*
        /// <summary>
        /// 执行指定名称的存储过程
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="ht"></param>
        public static void UpdateByProcedure(string proName, Hashtable ht = null)
        {
            DataTable dt = new DataTable();
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = proName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (ht != null)
                    {
                        foreach (DictionaryEntry dic in ht)
                        {

                            Sql.AddParameter(cmd, Convert.ToString(dic.Key), Convert.ToString(dic.Value));
                        }
                    }

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).UpdateCommand = cmd;
                    }
                }
            }
        }
        */

        /*
        /// <summary>
        /// 核心系统生成的该存储过程会出现“从数据类型 decimal 转换为 numeric 时出错”的异常,防止被下次生成自动覆盖，使用以下代码
        /// </summary>
        /// <param name="gC_contractId"></param>
        /// <param name="flC_BatchesAmount"></param>
        public static void spTaoqi_CountDeptAmountByContractID(Guid gC_contractId, ref decimal flC_BatchesAmount)
        {
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbTransaction trn = Sql.BeginTransaction(con))
                {
                    try
                    {
                        using (IDbCommand cmd = con.CreateCommand())
                        {
                            cmd.Transaction = trn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (Sql.IsOracle(cmd))
                                cmd.CommandText = "spTaoqi_CountDeptAmountByCont";
                            else
                                cmd.CommandText = "spTaoqi_CountDeptAmountByContractID";
                            IDbDataParameter parC_contractId = Sql.AddParameter(cmd, "@C_contractId", gC_contractId);
                            IDbDataParameter parC_BatchesAmount = Sql.AddParameter(cmd, "@C_BatchesAmount", flC_BatchesAmount);

                            //2015.4.29 解决“从数据类型 decimal 转换为 numeric 时出错”的异常
                            parC_BatchesAmount.Precision = 38;
                            parC_BatchesAmount.Scale = 2;
                            //---

                            parC_BatchesAmount.Direction = ParameterDirection.InputOutput;
                            cmd.ExecuteNonQuery();
                            flC_BatchesAmount = Sql.ToDecimal(parC_BatchesAmount.Value);
                        }
                        trn.Commit();
                    }
                    catch
                    {
                        trn.Rollback();
                        throw;
                    }
                }
            }
        }
        */

    }
}