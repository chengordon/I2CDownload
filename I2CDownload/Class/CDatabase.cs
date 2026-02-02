using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
//using System.IO;

namespace I2CDownload
{
    public class CDatabase
    {
        //private System.Windows.Forms.DataGridView dgv_Cfg;
        //CDatabase DatabaseCls_Obj = new CDatabase();
        //DatabaseCls_Obj.DatabaseParam.DBName = Application.StartupPath + "\\CfgDatabase.accdb";
        //DatabaseCls_Obj.DatabaseParam.TabName = "CfgTab";
        //DatabaseCls_Obj.DatabaseParam.SqlStr = "select * from " + DatabaseCls_Obj.DatabaseParam.TabName;
        //DataSet ds = DatabaseCls_Obj.getDataSet(DatabaseCls_Obj.DatabaseParam.SqlStr, DatabaseCls_Obj.DatabaseParam.TabName, DatabaseCls_Obj.DatabaseParam.DBName);
        //dgv_Cfg.DataSource = ds.Tables[0];

        public struct DatabaseParamStruct
        {
            public string DBName;
            public string TabName;
            public string[] ColumnName;
            public string SqlStr;
            public string TablePassword;
        }
        public  DatabaseParamStruct DatabaseParam;
        public void DBParamInit()
        {
            DatabaseParam.DBName = "";
            DatabaseParam.TabName = "";
            DatabaseParam.ColumnName = new string[1];
            DatabaseParam.SqlStr = "";
            DatabaseParam.TablePassword = "Wandtec888";
        }

        #region  全局变量
        public static OleDbConnection Result_OLEDB_Con;
        #endregion

        #region  建立数据库连接
        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回SqlConnection对象</returns>
        public  OleDbConnection getcon()
        {
            //连接字符串
            string conn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DatabaseParam.DBName + ";Jet OLEDB:Database Password=" + DatabaseParam.TablePassword + ";Jet OLEDB:Engine Type=5";

            if (!System.IO.File.Exists(DatabaseParam.DBName))
            {
                string dirName = System.IO.Path.GetDirectoryName(DatabaseParam.DBName);
                if(!System.IO.Directory.Exists(dirName))
                {
                    System.IO.Directory.CreateDirectory(dirName);
                }

                ADOX.CatalogClass cat = new ADOX.CatalogClass();
                cat.Create(conn + ";");
                cat = null;
                TableCreate(DatabaseParam.TabName, DatabaseParam.ColumnName);
            }
            //建立连接,注意数据表应放在DEBUG文件下
            Result_OLEDB_Con = new OleDbConnection(conn);                           //用oledbConnection对象与指定的数据库相连接
            Result_OLEDB_Con.Open();                                                        //打开数据库连接
            return Result_OLEDB_Con;                                                        //返回SqlConnection对象的信息
        }
        #endregion        

        #region  关闭数据库连接
        /// <summary>
        /// 关闭于数据库的连接.
        /// </summary>
        public void con_close()
        {
            if (Result_OLEDB_Con.State == ConnectionState.Open)                             //判断是否打开与数据库的连接
            {
                Result_OLEDB_Con.Close();                                                   //关闭数据库的连接
                Result_OLEDB_Con.Dispose();                                                 //释放My_con变量的所有空间
            }
        }
        #endregion

        #region  读取指定表中的信息
        /// <summary>
        /// 读取指定表中的信息.
        /// </summary>
        /// <param name="SQLstr">SQL语句</param>
        /// <returns>返回bool型</returns>
        public OleDbDataReader GetReader()
        {
            getcon();                                                           //打开与数据库的连接
            OleDbCommand My_com = Result_OLEDB_Con.CreateCommand();                         //创建一个SqlCommand对象，用于执行SQL语句
            My_com.CommandText = DatabaseParam.SqlStr;                                                    //获取指定的SQL语句
            OleDbDataReader My_read = My_com.ExecuteReader();                               //执行SQL语名句，生成一个SqlDataReader对象
            return My_read;
        }
        #endregion

        #region 执行SqlCommand命令
        /// <summary>
        /// 执行SqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public void GetQuery()
        {
            getcon();                                                           //打开与数据库的连接
            OleDbCommand SQLcom = new OleDbCommand(DatabaseParam.SqlStr, Result_OLEDB_Con); //创建一个SqlCommand对象，用于执行SQL语句
            SQLcom.ExecuteNonQuery();                                                       //执行SQL语句
            SQLcom.Dispose();                                                               //释放所有空间
            con_close();                                                                    //调用con_close()方法，关闭与数据库的连接
        }
        #endregion

        #region  创建DataSet对象
        /// <summary>
        /// 创建一个DataSet对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <param name="M_str_table">表名</param>
        /// <returns>返回DataSet对象</returns>
        public DataSet getDataSet(string SQLstr, string tableName, string DatabaseName)
        {
            getcon();                                                           //打开与数据库的连接
            OleDbDataAdapter SQLda = new OleDbDataAdapter(DatabaseParam.SqlStr, Result_OLEDB_Con);        //创建一个SqlDataAdapter对象，并获取指定数据表的信息
            DataSet My_DataSet = new DataSet();                                             //创建DataSet对象
            SQLda.Fill(My_DataSet, DatabaseParam.TabName);                                   //通过SqlDataAdapter对象的Fill()方法，将数据表信息添加到DataSet对象中
            con_close();                                                                    //关闭数据库的连接
            SQLda.Dispose();
            return My_DataSet;                                                              //返回DataSet对象的信息
        }


        #endregion

        #region 获取数据库中所有的表的名称
        /// <summary>
        /// 获取数据库中所有的表的名称
        /// </summary>
        /// <returns></returns>
        public string[] getTableName()
        {
            getcon();
            DataTable shemaTable = Result_OLEDB_Con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });            
            string[] TableName = new string[shemaTable.Rows.Count];
            for (int i = 0; i < shemaTable.Rows.Count; i++)
            {
                DataRow m_DataRow = shemaTable.Rows[i];
                TableName[i] = m_DataRow["TABLE_NAME"].ToString();
                //TableName[i] = shemaTable.Rows[i]["TABLE_NAME"].ToString();
            }
            con_close();
            return TableName;
        }
        #endregion

        #region 获取数据库中表的列名称
        /// <summary>
        /// 获取数据库中表的列名称
        /// </summary>
        /// <returns></returns>
        public string[] getColumnName(string tableName)
        {
            getcon();
            DataTable shemaTable = Result_OLEDB_Con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
            con_close();
            string[] ColumnName = new string[shemaTable.Rows.Count];
            for (int i = 0; i < shemaTable.Rows.Count; i++)
            {
                DataRow m_DataRow = shemaTable.Rows[i];
                ColumnName[i] = m_DataRow["COLUMN_NAME"].ToString();
            }
            return ColumnName;
        }
        #endregion

        #region 获取数据库中表的列类型
        /// <summary>
        /// 获取数据库中表的列类型
        /// </summary>
        /// <returns></returns>
        public string[] getColumnType(string tableName)
        {
            getcon();
            DataTable shemaTable = Result_OLEDB_Con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
            con_close();
            string[] ColumnName = new string[shemaTable.Rows.Count];
            for (int i = 0; i < shemaTable.Rows.Count; i++)
            {
                DataRow m_DataRow = shemaTable.Rows[i];
                ColumnName[i] = m_DataRow["DATA_TYPE"].ToString();
            }
            return ColumnName;
        }
        #endregion

        #region 获取数据库中表的列最大长度
        /// <summary>
        /// 获取数据库中表的列最大长度
        /// </summary>
        /// <returns></returns>
        public string[] getColumnLength(string tableName)
        {
            getcon();
            DataTable shemaTable = Result_OLEDB_Con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
            con_close();
            string[] ColumnLength = new string[shemaTable.Rows.Count];
            for (int i = 0; i < shemaTable.Rows.Count; i++)
            {
                DataRow m_DataRow = shemaTable.Rows[i];
                ColumnLength[i] = m_DataRow["CHARACTER_MAXIMUM_LENGTH"].ToString();
            }
            return ColumnLength;
        }
        #endregion

        #region 新建数据表格
        /// <summary>
        /// 新建数据表格
        /// </summary>
        /// <param name="TableName"></param>
        public void TableCreate(string TableName,string[] str1)
        {
            ADOX.Catalog catalog = new ADOX.Catalog();
            ADODB.Connection cn = new ADODB.Connection();

            cn.Open("Provider=Microsoft.ACE.OleDb.12.0;Data Source=" + DatabaseParam.DBName + ";Jet OLEDB:Database Password=" + DatabaseParam.TablePassword + ";Jet OLEDB:Engine Type=5", null, null, -1);
            catalog.ActiveConnection = cn;

            ADOX.Table table = new ADOX.Table();
            table.Name = TableName;
            
            for (int i = 0; i < str1.Length; i++)
            {                
                if (str1[i] != null)
                {
                    if ((i + 1) == str1.Length)
                    {
                        table.Columns.Append(str1[i], ADOX.DataTypeEnum.adLongVarWChar, 1024); 
                    }
                    else
                    {
                        table.Columns.Append(str1[i], ADOX.DataTypeEnum.adVarWChar, 255); 
                    }
                    
                }
            }           
            catalog.Tables.Append(table);            
            cn.Close();
        }
        #endregion
    }
}
