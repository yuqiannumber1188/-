using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
//using Microsoft.Office.Core;
using cjpc.utils;

namespace cjpc.utils.dbutils
{
    class DBClass
    {
        SqlConnection conn = null;  //连接数据库的对象
        private string M_ConnectionKey = "cjpc.Properties.Settings.conn";

        private String getSqlConnStr()
        {
            string temp = ConfigurationManager.ConnectionStrings[M_ConnectionKey].ConnectionString + ";Password=wfyxy";

            return temp;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns>返回SqlConnection对象</returns>
        public SqlConnection GetConnection()
        {
            SqlConnection myConn = new SqlConnection(getSqlConnStr());
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }
            return myConn;
        }

        /// <summary>
        /// 执行SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="sql">执行SQL语句命令</param>
        public DataSet GreatDs(string sql)
        {
            try
            {
                SqlConnection myConnection = new SqlConnection(getSqlConnStr());
                //myConnection.Open();
                if (myConnection.State == ConnectionState.Closed)
                {
                    myConnection.Open();
                }
                SqlDataAdapter Dar = new SqlDataAdapter(sql, myConnection);
                DataSet ds = new DataSet();

                Dar.Fill(ds);
                //myConnection.Close();
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        /// <summary>
        /// 执行SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="myCmd">执行SQL语句命令的SqlCommand对象</param>
        public void ExecNonQuery(SqlCommand myCmd)
        {
            try
            {
                if (myCmd.Connection.State != ConnectionState.Open)
                {
                    myCmd.Connection.Open(); //打开与数据库的连接
                }
                //使用SqlCommand对象的ExecuteNonQuery方法执行SQL语句，并返回受影响的行数
                myCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (myCmd.Connection.State == ConnectionState.Open)
                {
                    myCmd.Connection.Close(); //关闭与数据库的连接
                }
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">执行SQL语句命令</param>
        public void DoSql(string sql)
        {
            SqlConnection myConnection = new SqlConnection(getSqlConnStr());
            SqlCommand cmd = new SqlCommand(sql, myConnection);
            cmd.CommandTimeout = 90;
            try
            {

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open(); //打开与数据库的连接
                }
                //使用SqlCommand对象的ExecuteNonQuery方法执行SQL语句，并返回受影响的行数
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close(); //关闭与数据库的连接
                }
                myConnection.Close();//关闭数据库
            }

        }

        /// <summary>
        /// 执行SQL语句,返回首行第一个元素，用于返回统计值
        /// </summary>
        /// <param name="sql">执行SQL语句命令</param>
        public string GetOneValue(string sql)
        {
            DataSet dsFullName = GreatDs(sql);
            if (dsFullName.Tables.Count == 0 || dsFullName.Tables[0].Rows.Count == 0)
            {
                return "";//strQrget;
            }
            else
            {
                return dsFullName.Tables[0].Rows[0][0].ToString();
            }
        }


        // 关闭数据库连接
        private void closeConn()
        {
            if (conn.State == ConnectionState.Open)
            {//判断数据库的连接状态，如果状态是打开的话就将它关闭

                conn.Close();
            }
        }

        /// <summary>
        /// 执行SQL事务语句
        /// </summary>
        /// <param name="strSqls[]">执行的sql语句</param>
        public void ExecNonQuerySW(string[] strSqls)
        {
            SqlConnection sqlConn = new SqlConnection(getSqlConnStr());
            SqlCommand cmd = new SqlCommand();
            try
            {
                sqlConn.Open();
                cmd.Connection = sqlConn;
                cmd.Transaction = sqlConn.BeginTransaction();
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.Text;

                foreach (string strSql in strSqls)
                {
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();
                }

                cmd.Transaction.Commit(); ;//事务提交   
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();//事务回滚   
                throw new Exception(ex.Message, ex);

            }
            finally
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed)
                    sqlConn.Close();
            }
        }

        /// <summary>
        /// 执行SQL事务语句
        /// </summary>
        /// <param name="strSqls[]">执行的sql语句</param>
        public void ExecNonQuerySWS(string[,] strSqls)
        {
            SqlConnection sqlConn = new SqlConnection(getSqlConnStr());
            SqlCommand cmd = new SqlCommand();
            try
            {
                sqlConn.Open();
                cmd.Connection = sqlConn;
                cmd.Transaction = sqlConn.BeginTransaction();
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.Text;

                foreach(string strSql in strSqls)
                {
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();
                }

                cmd.Transaction.Commit(); ;//事务提交   
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();//事务回滚   
                throw new Exception(ex.Message, ex);

            }
            finally
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed)
                    sqlConn.Close();
            }
        }

        //public void SqlBulkCopyTD(string tablename,DataTable dt)
        //{
        //    SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(getSqlConnStr(), SqlBulkCopyOptions.UseInternalTransaction);
        //    sqlbulkcopy.DestinationTableName = tablename;//数据库中的表名
        //    sqlbulkcopy.WriteToServer(dt);
        //}

        public void UpdateAccess(DataTable temp, string sqlcx)
        {
            SqlConnection con = GetConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataAdapter Bada = new SqlDataAdapter(sqlcx, con);//建立一个DataAdapter对象
                SqlCommandBuilder cb = new SqlCommandBuilder(Bada);//这里的CommandBuilder对象一定不要忘了,一般就是写在DataAdapter定义的后面
                cb.QuotePrefix = "[";
                cb.QuoteSuffix = "]";
                DataSet ds = new DataSet();//建立DataSet对象
                Bada.Fill(ds, "demo");//填充DataSet
                foreach (DataRow tempRow in temp.Rows)
                {
                    DataRow dr = ds.Tables["demo"].NewRow();
                    dr.ItemArray = tempRow.ItemArray;//行复制
                    ds.Tables["demo"].Rows.Add(dr);
                }
                Bada.Update(ds, "demo");//用DataAdapter的Update()方法进行数据库的更新
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
        }

        /// <summary>
        /// dt批量导入数据库
        /// </summary>
        /// <param name="source">插入的dt</param>
        /// <param name="tablename">目的表</param>
        /// <returns></returns>
        public bool AddDataTableToDB(DataTable source, string tablename)
        {
            SqlTransaction tran = null;//声明一个事务对象  
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();//打开链接  
                    using (tran = conn.BeginTransaction())
                    {
                        using (SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran))
                        {
                            copy.DestinationTableName = tablename;  //指定服务器上目标表的名称  
                            copy.WriteToServer(source);                      //执行把DataTable中的数据写入DB  
                            tran.Commit();                                      //提交事务  
                            return true;                                        //返回True 执行成功！  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string ab = ex.Message.ToString();
                if (null != tran)
                    tran.Rollback();
                //LogHelper.Add(ex);  
                return false;//返回False 执行失败！  
            }
        }

        /// <summary>
        /// 执行查询语句，返回sqlCommand类对象
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <returns>返回sqlCommand类对象</returns>
        public SqlCommand GetCommandStr(string strSql)
        {
            //创建数据库连接对象
            SqlConnection myConn = GetConnection();
            //创建命令对象
            SqlCommand myCmd = new SqlCommand();
            //连接数据库
            myCmd.Connection = myConn;
            //执行定义的SQL语句
            myCmd.CommandText = strSql;
            //获取命令对象执行的类型
            myCmd.CommandType = CommandType.Text;
            return myCmd;
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。 
        /// </summary>
        /// <param name="myCmd"></param>
        /// <returns>执行SQL语句命令的SqlCommand对象</returns>
        public string ExecScalar(SqlCommand myCmd)
        {
            string strSql;
            try
            {
                if (myCmd.Connection.State == ConnectionState.Closed)
                {
                    myCmd.Connection.Open(); //打开与数据库的连接
                }
                //使用SqlCommand对象的ExecuteScalar方法执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。 
                strSql = Convert.ToString(myCmd.ExecuteScalar());
                return strSql;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (myCmd.Connection.State == ConnectionState.Open)
                {
                    myCmd.Connection.Close();//关闭与数据库的连接
                }
            }
        }

        /// <summary>
        /// 说  明：  返回数据集的表的集合
        ///	返回值：  数据源的数据表
        ///	参  数：  myCmd 执行SQL语句命令的SqlCommand对象，TableName 数据表名称
        /// </summary>
        public DataSet GetDataSet(SqlCommand myCmd, string TableName)
        {
            SqlDataAdapter adapt;
            DataSet ds = new DataSet();
            try
            {
                if (myCmd.Connection.State != ConnectionState.Open)
                {
                    myCmd.Connection.Open();
                }
                adapt = new SqlDataAdapter(myCmd);
                adapt.Fill(ds, TableName);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (myCmd.Connection.State == ConnectionState.Open)
                {
                    myCmd.Connection.Close();

                }
            }
        }
    }
}
