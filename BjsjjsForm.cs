using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cjpc.utils;
using cjpc.utils.dbutils;

namespace cjpc
{
    public partial class BjsjjsForm : Form
    {
        DBClass dbclass = new DBClass();
        string bjjhsql = "select * from bjjh";
        string delbjjhsql = "delete from bjjh";
        string bjjhzdsql = "select * from zdgxb where bmc = 'bjjh' order by id asc";
        string jhbsql = "select * from jhb";
        string deletejhbsql = "delete from jhb";
        string sccjsql = "select * from SCCJ";
        string vcjinfo = "SELECT [CJ_YSCJ_NAME],[KC_NAME],[SJC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO]";


        string sjc;
        MainForm fm;

        public BjsjjsForm(MainForm _fm)
        {
            InitializeComponent();

            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
            sjc = labelsjc.Text;

            bindDGVZD();
            bindDGVZDJH();
        }

        private void bindDGVZD()
        {
            dataGridView1.DataSource = dbclass.GreatDs(bjjhsql + sjc).Tables[0];
            DataTable dtzd = new DataTable();
            dtzd = dbclass.GreatDs(bjjhzdsql.Replace("zdgxb", "zdgxb" + sjc).Replace("bjjh", "bjjh" + sjc)).Tables[0];

            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                for (int i = 0; i < dtzd.Rows.Count; i++)
                {
                    if (dataGridView1.Columns[j].HeaderText.Equals(dtzd.Rows[i]["yzdmc"].ToString()))
                    {
                        dataGridView1.Columns[j].HeaderText = dtzd.Rows[i]["szdmc"].ToString();
                        break;
                    }
                }
            }
        }

        private void bindDGVZDJH()
        {
            dataGridView2.DataSource = dbclass.GreatDs(jhbsql + sjc).Tables[0];
        }

        private void BjsjjsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult r = MessageBox.Show("退出本页面?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r != DialogResult.OK)
                {
                    e.Cancel = true;
                }
                else
                {

                    fm.labelsjc.Text = this.labelsjc.Text;
                    this.Hide();
              //      fm.ShowDialog();
                }
            }
        }

        private void btnkcjh_Click(object sender, EventArgs e)
        {
            
            try
            {
                 DataTable cskcdt = new DataTable();
                 string cskc = vcjinfo + " where SJC='" + sjc + "'";
                 cskcdt = dbclass.GreatDs(cskc).Tables[0];
                 string kcname = cskcdt.Rows[0]["KC_NAME"].ToString();
                 string csname = cskcdt.Rows[0]["CJ_YSCJ_NAME"].ToString();

                 if(csname.Contains("输入表"))
                 {
                     csname= csname.Replace("输入表", "");
                 }

                 if (csname.Contains("输出表"))
                 {
                     csname= csname.Replace("输出表", "");
                 }

                 string rs = dbclass.GetOneValue("SELECT   COUNT(*) FROM SCCJ" + sjc);
                 string rq = DateTime.Now.ToString("D");

                 DataTable jhbdt = new DataTable();
                 jhbdt = dbclass.GreatDs(jhbsql + sjc).Tables[0];
                 int countc = jhbdt.Columns.Count;
                 string[] altercolsql = new string[countc - 1];

                 string insertsqlbjjhkt = "insert into bjjh" + sjc + "(分项,";
                 string insertsqlbjjhzj = ") select SUBSTRING(分项, CHARINDEX('.', 分项) + 1, LEN(分项)),";
                 altercolsql[0] = "alter table bjjh" + sjc + " add 分项 varchar(50)";
                 for (int j = 2; j < countc; j++)
                {
                     altercolsql[j-1] = "alter table bjjh" + sjc + " add " + dataGridView2.Columns[j].HeaderText + " decimal(18, 3)";
                     insertsqlbjjhkt = insertsqlbjjhkt + dataGridView2.Columns[j].HeaderText + ",";
                     insertsqlbjjhzj = insertsqlbjjhzj + dataGridView2.Columns[j].HeaderText + ",";
                 }
                 insertsqlbjjhkt= insertsqlbjjhkt.Substring(0, insertsqlbjjhkt.Length - 1);
                 insertsqlbjjhzj= insertsqlbjjhzj.Substring(0, insertsqlbjjhzj.Length - 1);
                 insertsqlbjjhkt = insertsqlbjjhkt + insertsqlbjjhzj + " from jhb" + sjc + " WHERE (分项 LIKE '总数%')";

                 

                 string[] upsql = new string[7];
                 upsql[0] = "UPDATE bjjh" + sjc + " SET csmc = '" + csname + "'";
                 upsql[1] = "UPDATE bjjh" + sjc + " SET xx = '0'";
                 upsql[2] = "UPDATE bjjh" + sjc + " SET kc = '" + kcname + "'";
                 upsql[3] = "UPDATE bjjh" + sjc + " SET bj = '0'";
                 upsql[4] = "UPDATE bjjh" + sjc + " SET rs = '" + rs + "'";
                 upsql[5] = "UPDATE bjjh" + sjc + " SET rq = '" + rq + "'";
                 upsql[6] = "UPDATE bjjh" + sjc + " SET zy = '0'";


                 dbclass.ExecNonQuerySW(altercolsql);
                 dbclass.DoSql(insertsqlbjjhkt);
                 dbclass.ExecNonQuerySW(upsql);
                 bindDGVZD();
                 bindDGVZDJH();

                 string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "课程聚合生成成功" + "','" + "bjjh" + sjc + "','" + DateTime.Now + "')";
                 dbclass.DoSql(sqlinsertlogs);

                 MessageBox.Show("课程聚合生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "课程聚合生成失败" + "','" + "bjjh" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("课程聚合生成失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable cskcdt = new DataTable();
                string cskc = vcjinfo + " where SJC='" + sjc + "'";
                cskcdt = dbclass.GreatDs(cskc).Tables[0];
                string kcname = cskcdt.Rows[0]["KC_NAME"].ToString();
                string csname = cskcdt.Rows[0]["CJ_YSCJ_NAME"].ToString();

                if (csname.Contains("输入表"))
                {
                    csname = csname.Replace("输入表", "");
                }

                if (csname.Contains("输出表"))
                {
                    csname = csname.Replace("输出表", "");
                }

                string rs = "";
                string rq = DateTime.Now.ToString("D");

                DataTable jhbdt = new DataTable();
                jhbdt = dbclass.GreatDs(jhbsql + sjc).Tables[0];
                int countc = jhbdt.Columns.Count;
                string insertsqlbjjhkt = "insert into bjjh" + sjc + "(分项,";
                string insertsqlbjjhzj = ") select 分项,";
                string[] upsql = new string[6];

                //班级
                DataTable bjmcdt = new DataTable();
                string bjmcsql = "SELECT DISTINCT bj FROM SCCJ" + sjc;
                bjmcdt = dbclass.GreatDs(bjmcsql).Tables[0];
                for (int i = 0; i < bjmcdt.Rows.Count; i++)
                {
                    insertsqlbjjhkt = "insert into bjjh" + sjc + "(bj,分项,";
                    insertsqlbjjhzj = ") select  SUBSTRING(分项, 0, CHARINDEX('.', 分项)), SUBSTRING(分项, CHARINDEX('.', 分项) + 1, LEN(分项)),";
                    for (int j = 2; j < countc-1; j++)
                    {
                       
                        insertsqlbjjhkt = insertsqlbjjhkt + dataGridView2.Columns[j].HeaderText + ",";
                        insertsqlbjjhzj = insertsqlbjjhzj + dataGridView2.Columns[j].HeaderText + ",";
                    }
                    insertsqlbjjhkt = insertsqlbjjhkt.Substring(0, insertsqlbjjhkt.Length - 1);
                    insertsqlbjjhzj = insertsqlbjjhzj.Substring(0, insertsqlbjjhzj.Length - 1);
                    insertsqlbjjhkt = insertsqlbjjhkt + insertsqlbjjhzj + " from jhb" + sjc + " WHERE (分项 LIKE '" + bjmcdt.Rows[i]["bj"].ToString() + "%')";

                    rs = dbclass.GetOneValue("SELECT   COUNT(*) FROM SCCJ" + sjc + " where bj='" + bjmcdt.Rows[i]["bj"].ToString()+"'");
                    upsql[0] = "UPDATE bjjh" + sjc + " SET csmc = '" + csname + "' where bj='"+bjmcdt.Rows[i]["bj"].ToString()+"'";
                    upsql[1] = "UPDATE bjjh" + sjc + " SET xx = '0' where bj='" + bjmcdt.Rows[i]["bj"].ToString() + "'";
                    upsql[5] = "UPDATE bjjh" + sjc + " SET zy = '0' where bj='" + bjmcdt.Rows[i]["bj"].ToString() + "'";
                    upsql[2] = "UPDATE bjjh" + sjc + " SET kc = '" + kcname + "' where bj='" + bjmcdt.Rows[i]["bj"].ToString() + "'";
                    upsql[3] = "UPDATE bjjh" + sjc + " SET rs = '" + rs + "' where bj='" + bjmcdt.Rows[i]["bj"].ToString() + "'";
                    upsql[4] = "UPDATE bjjh" + sjc + " SET rq = '" + rq + "' where bj='" + bjmcdt.Rows[i]["bj"].ToString() + "'";
                    dbclass.DoSql(insertsqlbjjhkt);
                    dbclass.ExecNonQuerySW(upsql);
                }

                DataTable zymcdt = new DataTable();
               string zymcsql = "SELECT DISTINCT zy FROM SCCJ" + sjc;
            zymcdt = dbclass.GreatDs(zymcsql).Tables[0];

            for (int i = 0; i < zymcdt.Rows.Count; i++)
            {
                insertsqlbjjhkt = "insert into bjjh" + sjc + "(zy,分项,";
                insertsqlbjjhzj = ") select  SUBSTRING(分项, 0, CHARINDEX('.', 分项)), SUBSTRING(分项, CHARINDEX('.', 分项) + 1, LEN(分项)),";
                for (int j = 2; j < countc - 1; j++)
                {

                    insertsqlbjjhkt = insertsqlbjjhkt + dataGridView2.Columns[j].HeaderText + ",";
                    insertsqlbjjhzj = insertsqlbjjhzj + dataGridView2.Columns[j].HeaderText + ",";
                }
                insertsqlbjjhkt = insertsqlbjjhkt.Substring(0, insertsqlbjjhkt.Length - 1);
                insertsqlbjjhzj = insertsqlbjjhzj.Substring(0, insertsqlbjjhzj.Length - 1);
                insertsqlbjjhkt = insertsqlbjjhkt + insertsqlbjjhzj + " from jhb" + sjc + " WHERE (分项 LIKE '" + zymcdt.Rows[i]["zy"].ToString() + "%')";

                rs = dbclass.GetOneValue("SELECT   COUNT(*) FROM SCCJ" + sjc + " where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'");
                upsql[0] = "UPDATE bjjh" + sjc + " SET csmc = '" + csname + "' where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'";
                upsql[1] = "UPDATE bjjh" + sjc + " SET xx = '0' where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'";
                upsql[5] = "UPDATE bjjh" + sjc + " SET bj = '0' where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'";
                upsql[2] = "UPDATE bjjh" + sjc + " SET kc = '" + kcname + "' where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'";
                upsql[3] = "UPDATE bjjh" + sjc + " SET rs = '" + rs + "' where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'";
                upsql[4] = "UPDATE bjjh" + sjc + " SET rq = '" + rq + "' where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'";
                dbclass.DoSql(insertsqlbjjhkt);
                dbclass.ExecNonQuerySW(upsql);
            }
               


               
                
                bindDGVZD();
                bindDGVZDJH();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "班级专业聚合生成成功" + "','" + "bjjh" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("班级专业聚合生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "班级专业聚合生成失败" + "','" + "bjjh" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("班级专业聚合生成失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btncsh_Click(object sender, EventArgs e)
        {
            try{

                string deletebbjh = delbjjhsql + sjc;
                dbclass.DoSql(deletebbjh);

                string dropbbjh = DBUtils.DropBJJHSql(sjc);
                dbclass.DoSql(dropbbjh);

                string createbbjh = DBUtils.GetBJJHSql(sjc);
                dbclass.DoSql(createbbjh);
                
                bindDGVZD();
                bindDGVZDJH();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化聚合生成成功" + "','" + "bjjh" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("初始化聚合生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化聚合生成失败" + "','" + "bjjh" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("初始化聚合生成失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
    }
}
