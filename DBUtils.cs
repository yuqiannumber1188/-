using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cjpc.utils.dbutils
{
    class DBUtils
    {
        public static DBClass dbclass = new DBClass();

        public DBUtils()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        //创建数据表语句
        static public string GetCreateTableSql(string tabName, int count, string[] zdmc, string[] zdlx, string[] zddx)
        {
            //声明要创建的表名，你也可以改为从textbox中获取；
            string sqlStr = "create table ";
            sqlStr += tabName + "( ";
            sqlStr += "id int identity(1,1) primary key,";
            //identity(1,1)是标记递增种子
            //primary key定义主键
            for (int i = 0; i < count; i++)
            {
                if (zdlx[i].Equals("NVARCHAR"))
                {
                    sqlStr += (zdmc[i] + " NVARCHAR(" + zddx[i] + "),");
                }
                else
                {
                    sqlStr += (zdmc[i] + " " + zdlx[i] + ",");
                }
            }
            sqlStr += " )";
            return sqlStr;
        }

        //增加数据表字段
        static public string[] GetAlterTableSql(string tabName, int count, string[] zdmc, string[] zdlx, string[] zddx)
        {
            string sqlStr = "alter table " + tabName + " add ";
            string[] sqlalters = new string[count];
            for (int i = 0; i < count; i++)
            {
                if (zdlx[i].Equals("NVARCHAR"))
                {
                    sqlalters[i] = (sqlStr + zdmc[i] + " NVARCHAR(" + zddx[i] + ")");
                }
                else
                {
                    sqlalters[i] = (sqlStr + zdmc[i] + " " + zdlx[i]);
                }
            }
            return sqlalters;
        }

        //更改值为0
        static public string[] UpdateTableSql(string tabName, int count, string[] zdmc)
        {
            string sqlStr = "update "+ tabName + " set ";
            string[] sqlupdate = new string[count];
            for (int i = 0; i < count; i++)
            {

                sqlupdate[i] = (sqlStr + zdmc[i] + " = "+0.0);     
               
            }
            return sqlupdate;
        }



        //创建原始数据输入表
        static public string GetYSCJSql()
        {

            //声明要创建的表名，你也可以改为从textbox中获取；
            string sqlStr = "create table yscj ";
            sqlStr += "( ";
            //sqlStr += "IDY Counter(1,1) primary key,";
            sqlStr += "id long primary key,";
            sqlStr += "xx varchar(100),";
            sqlStr += "xy varchar(100),";
            sqlStr += "zy varchar(50),";
            sqlStr += "bj varchar(50),";
            sqlStr += "xm varchar(50),";
            sqlStr += "xh varchar(50),";
            sqlStr += "xxbm varchar(50),";
            sqlStr += "df decimal(18,3)";
            sqlStr += " )";
            return sqlStr;
        }

        static public string GetYSCJSql(string yscjtable)
        {

            //声明要创建的表名，你也可以改为从textbox中获取；
            string sqlStr = "create table " + yscjtable;
            sqlStr += "( ";
            //sqlStr += "IDY Counter(1,1) primary key,";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "xx varchar(100),";
            sqlStr += "xy varchar(100),";
            sqlStr += "zy varchar(50),";
            sqlStr += "bj varchar(50),";
            sqlStr += "xm varchar(50),";
            sqlStr += "xh varchar(50),";
            sqlStr += "xxbm varchar(50),";
            sqlStr += "df decimal(18,3)";
            sqlStr += ")";
            return sqlStr;
        }

        static public string DropYSCJSql()
        {
            return "Drop table yscj";
        }

        static public string DropYSCJSql(string tablename)
        {
            return "Drop table " + tablename;
        }

        //创建系数表
        static public string GetXSBSql()
        {
            string sqlStr = "create table xsb ";
            sqlStr += "( ";
            sqlStr += "id Counter(1,1) primary key,";
            sqlStr += "题号 varchar(50)";
            sqlStr += " )";
            return sqlStr;
        }

        static public string GetXSBSql(string sjc)
        {
            string xsb = "xsb" + sjc;
            string sqlStr = "create table " + xsb;
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "题号 varchar(50)";
            sqlStr += ")";
            return sqlStr;
        }

        static public string DropXSBSql()
        {
            return "Drop table xsb";
        }

        static public string DropXSBSql(string sjc)
        {
            return "Drop table xsb" + sjc;
        }

        //创建聚合表
        static public string GetJHBSql()
        {
            string sqlStr = "create table jhb ";
            sqlStr += "( ";
            sqlStr += "id Counter(1,1) primary key,";
            sqlStr += "分项 varchar(50),";
            sqlStr += "总成绩 decimal(18,3)";
            sqlStr += ")";
            return sqlStr;
        }

        static public string GetJHBSql(string sjc)
        {
            string jhb = "jhb" + sjc;
            string sqlStr = "create table " + jhb;
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "分项 varchar(50),";
            sqlStr += "总成绩 decimal(18,3)";
            sqlStr += ")";
            return sqlStr;
        }

        static public string DropJHBSql()
        {
            return "Drop table jhb";
        }

        static public string DropJHBSql(string sjc)
        {
            return "Drop table jhb" + sjc;
        }

        static public string GetJSBSql()
        {
            string sqlStr = "create table jsb ";
            sqlStr += "( ";
            sqlStr += "id Counter(1,1) primary key,";
            sqlStr += "分项 varchar(50),";
            sqlStr += "df decimal(18,3)";
            sqlStr += ")";
            return sqlStr;
        }

        static public string GetJSBSql(string sjc)
        {
            string jsb = "jsb" + sjc;
            string sqlStr = "create table " + jsb;
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "分项 varchar(50),";
            sqlStr += "df decimal(18,3)";
            sqlStr += ")";
            return sqlStr;
        }

        static public string DropJSBSql()
        {
            return "Drop table jsb";
        }

        static public string DropJSBSql(string sjc)
        {
            return "Drop table jsb" + sjc;
        }

        //创建原始数据输出表
        static public string GetSCCJSql()
        {
            //声明要创建的表名，你也可以改为从textbox中获取；
            string sqlStr = "create table sccj ";
            sqlStr += "( ";
            //sqlStr += "id Counter(1,1) primary key,";
            sqlStr += "id long primary key,";
            sqlStr += "xx varchar(100),";
            sqlStr += "xy varchar(100),";
            sqlStr += "zy varchar(50),";
            sqlStr += "bj varchar(50),";
            sqlStr += "xm varchar(50),";
            sqlStr += "xh varchar(50),";
            sqlStr += "xxbm varchar(50),";
            sqlStr += "df decimal(18,3),";
            sqlStr += "总成绩 decimal(18,3)";
            sqlStr += " )";
            return sqlStr;
        }

        //创建原始数据输出表
        static public string GetSCCJSql(string tablename)
        {
            //声明要创建的表名，你也可以改为从textbox中获取；
            string sqlStr = "create table " + tablename;
            sqlStr += "( ";
            //sqlStr += "id Counter(1,1) primary key,";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "xx varchar(100),";
            sqlStr += "xy varchar(100),";
            sqlStr += "zy varchar(50),";
            sqlStr += "bj varchar(50),";
            sqlStr += "xm varchar(50),";
            sqlStr += "xh varchar(50),";
            sqlStr += "xxbm varchar(50),";
            sqlStr += "df decimal(18,3),";
            sqlStr += "总成绩 decimal(18,3)";
            sqlStr += ")";
            return sqlStr;
        }

        static public string DropSCCJSql()
        {
            return "Drop table sccj";
        }

        static public string DropSCCJSql(string tablename)
        {
            return "Drop table " + tablename;
        }

        //创建原始数据成绩表
        static public string GetYSFFBSql()
        {
            //声明要创建的表名，你也可以改为从textbox中获取；
            string sqlStr = "create table ysffb ";
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "tm varchar(50),";
            sqlStr += "fs decimal(18,3)";
            sqlStr += ")";
            return sqlStr;
        }

        static public string GetYSFFBSql(string sjc)
        {
            //声明要创建的表名，你也可以改为从textbox中获取；
            string ysffb = "ysffb" + sjc;
            string sqlStr = "create table " + ysffb;
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "tm varchar(50),";
            sqlStr += "fs decimal(18,3)";
            sqlStr += ")";
            return sqlStr;
        }

        static public string DropYSFFBSql()
        {
            return "Drop table ysffb";
        }

        static public string DropYSFFBSql(string sjc)
        {
            return "Drop table ysffb" + sjc;
        }

        //创建原始数据输出表
        static public string GetSCFXGSSql()
        {
            //声明要创建的表名，你也可以改为从textbox中获取；
            string sqlStr = "create table scfxgs ";
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "fxmc varchar(50),";  //分项名称
            sqlStr += "mfs decimal(18,3),";         //分项值
            sqlStr += "qz decimal(18,3),";          //权重
            sqlStr += "jsgs varchar(255),"; //计算公式
            sqlStr += "pfxmc varchar(50)";  //
            sqlStr += " )";
            return sqlStr;
        }

        static public string GetSCFXGSSql(string sjc)
        {
            //声明要创建的表名，你也可以改为从textbox中获取；
            string scfxgs = "scfxgs" + sjc;
            string sqlStr = "create table " + scfxgs;
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "fxmc varchar(50),";  //分项名称
            sqlStr += "mfs decimal(18,3),";         //分项值
            sqlStr += "qz decimal(18,3),";          //权重
            sqlStr += "jsgs varchar(255),"; //计算公式
            sqlStr += "pfxmc varchar(50)";  //
            sqlStr += ")";
            return sqlStr;
        }

        //排序表
        static public string GetPXSql(string sjc)
        {
            string px = "px" + sjc;
            string sqlStr = "create table " + px;
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "xh varchar(50),";  //学号
            sqlStr += ")";
            return sqlStr;
        }
        //输出排序表
        static public string GetSCPXSql(string sjc)
        {
            string scpx = "scpx" + sjc;
            string sqlStr = "create table " + scpx;
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "xh varchar(50),";  //学号
            sqlStr += ")";
            return sqlStr;
        }
        //班级聚合表
        static public string GetBJJHSql(string sjc)
        {
            string bjjh = "bjjh" + sjc;
            string sqlStr = "create table " + bjjh;
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "csmc varchar(100),";  //测试名称
            sqlStr += "xx varchar(100),";  //学校
            sqlStr += "kc varchar(100),";  //课程
            sqlStr += "zy varchar(100),";  //专业
            sqlStr += "bj varchar(100),";  //班级
            sqlStr += "rs varchar(100),";  //人数
            sqlStr += "rq varchar(100),";  //日期
            sqlStr += ")";
            return sqlStr;
        }

        static public string DropPXSql(string sjc)
        {
            return "Drop table px"+sjc;
        }

        static public string DropSCPXSql(string sjc)
        {
            return "Drop table scpx" + sjc;
        }

        static public string DropBJJHSql(string sjc)
        {
            return "Drop table bjjh" + sjc;
        }

        //排序视图
        static public string GetVPXSql(string sjc,string zdm)
        {
            string px = "VRE_YSCJ_PX" + sjc+"_INFO";
            string sqlStr = "IF NOT EXISTS (SELECT * FROM sys.views WHERE name = '"+ px + "') create VIEW " + px;
            sqlStr += " AS ";
            sqlStr += "select YSCJ"+sjc+".*," + zdm + " ";
            sqlStr += "FROM px" + sjc + " INNER JOIN YSCJ" + sjc + " ON px" + sjc + ".xh = YSCJ"+sjc + ".xh";  
            return sqlStr;
        }

        static public string DropSCFXGSSql()
        {
            return "Drop table scfxgs";
        }

        static public string DropSCFXGSSql(string sjc)
        {
            return "Drop table scfxgs" + sjc;
        }

        static public string GetDtYscjSql()
        {
            return "Select * from yscj";
        }

        //创建字段关系表
        static public string GetZDGXBSql(string sjc)
        {
            //声明要创建的表名，你也可以改为从textbox中获取；
            string zdgxb = "zdgxb" + sjc;
            string sqlStr = "create table " + zdgxb;
            sqlStr += "( ";
            sqlStr += "id int identity(1,1) primary key,";
            sqlStr += "yzdmc varchar(200),";  //原始字段
            sqlStr += "szdmc varchar(200),"; //输出公式
            sqlStr += "bmc varchar(200)";  //表名称
            sqlStr += ")";
            return sqlStr;
        }


        //清空对应关系表
        static public string DeleteZDGXB()
        {
            return "delete from zdgxb";
        }

        static public string DeleteZDGXB(string sjc)
        {
            return "delete from zdgxb" + sjc;
        }

        static public string[] InsertZDGXBYS()
        {
            string[] insertzdgxsqls = new string[23];

            insertzdgxsqls[0] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "id", "序号", "yscj");
            insertzdgxsqls[1] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "dq", "地区", "yscj");
            insertzdgxsqls[2] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xx", "学校", "yscj");
            insertzdgxsqls[3] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "nj", "年级", "yscj");
            insertzdgxsqls[4] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "bj", "班级", "yscj");
            insertzdgxsqls[5] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xm", "姓名", "yscj");
            insertzdgxsqls[6] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xh", "学号", "yscj");
            insertzdgxsqls[7] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xxbm", "学校编码", "yscj");
            insertzdgxsqls[8] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "df", "得分", "yscj");
            insertzdgxsqls[22] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "djf", "等级分", "yscj");

            insertzdgxsqls[9] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "id", "序号", "sccj");
            insertzdgxsqls[10] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "dq", "地区", "sccj");
            insertzdgxsqls[11] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xx", "学校", "sccj");
            insertzdgxsqls[12] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "nj", "年级", "sccj");
            insertzdgxsqls[13] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "bj", "班级", "sccj");
            insertzdgxsqls[14] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xm", "姓名", "sccj");
            insertzdgxsqls[15] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xh", "学号", "sccj");
            insertzdgxsqls[16] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xxbm", "学校编码", "sccj");
            insertzdgxsqls[17] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "df", "得分", "sccj");
            insertzdgxsqls[18] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "总成绩", "总成绩", "sccj");

            insertzdgxsqls[19] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "mc", "名次", "yscj");

            insertzdgxsqls[20] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "id", "序号", "jsb");
            insertzdgxsqls[21] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "df", "得分", "jsb");

            return insertzdgxsqls;
        }

        static public string[] InsertZDGXBYS(string sjc)
        {
            string zdgxb = "zdgxb" + sjc;
            string[] insertzdgxsqls = new string[31];

            insertzdgxsqls[0] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "id", "序号", "yscj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[1] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xx", "学校", "yscj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[2] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xy", "院系", "yscj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[3] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "zy", "专业", "yscj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[4] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "bj", "班级", "yscj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[5] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xm", "姓名", "yscj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[6] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xh", "学号", "yscj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[7] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xxbm", "学校编码", "yscj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[8] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "df", "得分", "yscj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[22] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "djf", "等级分", "yscj" + sjc).Replace("zdgxb", zdgxb);

            insertzdgxsqls[9] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "id", "序号", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[10] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xx", "学校", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[11] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xy", "院系", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[12] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "zy", "专业", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[13] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "bj", "班级", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[14] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xm", "姓名", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[15] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xh", "学号", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[16] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xxbm", "学校编码", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[17] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "df", "得分", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[18] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "总成绩", "总成绩", "sccj" + sjc).Replace("zdgxb", zdgxb);

            insertzdgxsqls[19] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "mc", "名次", "yscj" + sjc).Replace("zdgxb", zdgxb);

            insertzdgxsqls[20] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "id", "序号", "jsb" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[21] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "df", "得分", "jsb" + sjc).Replace("zdgxb", zdgxb);

            insertzdgxsqls[23] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "id", "序号", "bjjh" + sjc).Replace("zdgxb", zdgxb);
            
            insertzdgxsqls[24] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "csmc", "测试名称", "bjjh" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[25] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xx", "学校", "bjjh" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[26] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "kc", "课程", "bjjh" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[27] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "zy", "专业", "bjjh" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[28] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "bj", "班级", "bjjh" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[29] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "rs", "人数", "bjjh" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[30] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "rq", "日期", "bjjh" + sjc).Replace("zdgxb", zdgxb);

            return insertzdgxsqls;
        }

        static public string[] InsertZDGXSCYS(string sjc)
        {
            string zdgxb = "zdgxb" + sjc;
            string[] insertzdgxsqls = new string[10];
            insertzdgxsqls[0] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "id", "序号", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[1] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xx", "学校", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[2] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xy", "院系", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[3] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "zy", "专业", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[4] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "bj", "班级", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[5] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xm", "姓名", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[6] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xh", "学号", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[7] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "xxbm", "学校编码", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[8] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "df", "得分", "sccj" + sjc).Replace("zdgxb", zdgxb);
            insertzdgxsqls[9] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", "总成绩", "总成绩", "sccj" + sjc).Replace("zdgxb", zdgxb);
            return insertzdgxsqls;
        }

        static public string[] InsertZDGXZD(int count, string[] yzdmcs, string[] szdmcs, string bmc,string sjc)
        {
            string zdgxb = "zdgxb" + sjc;
            string[] insertzdgxzdsqls = new string[count];
            for (int i = 0; i < count; i++)
            {
                insertzdgxzdsqls[i] = String.Format("Insert into zdgxb(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", yzdmcs[i], szdmcs[i], bmc).Replace("zdgxb", zdgxb);
            }

            return insertzdgxzdsqls;
        }

        //获得fxclass父节点
        static public int PClassID(int bclassid)
        {
            string sql = "SELECT pclassid FROM fxclass WHERE classid=" + bclassid.ToString();
            string pids = dbclass.GetOneValue(sql);
            int pidi = 0;
            try
            {
                pidi = Convert.ToInt32(pids);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return pidi;
        }
    }
}
