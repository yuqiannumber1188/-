using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using cjpc.utils;
using cjpc.utils.dbutils;

namespace cjpc
{
    public partial class ScbfzForm : Form
    {
        DBClass dbclass = new DBClass();

        string sccjsql = "select * from sccj";
        string delsccjsql = "delete from sccj";
        string sccjzdsql = "select * from zdgxb where bmc = 'sccj' order by id asc";
        string xsbsql = "select * from xsb";
        string yscjsql = "select * from yscj";
        string jhbsql = "select * from jhb";
        string deletejhbsql = "delete from jhb";
        string ysffbsql = "select * from ysffb";


        string sjc;
        MainForm fm;
        public ScbfzForm(MainForm _fm)
        {
            InitializeComponent();

            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
            sjc = labelsjc.Text;

            bindDGVZD();
            bindDGVZDJH();
            bindCBYS();
        }

        private DataTable bindDGVZD()
        {
            dataGridView1.DataSource = dbclass.GreatDs(sccjsql).Tables[0];
            DataTable dtzd = new DataTable();
            dtzd = dbclass.GreatDs(sccjzdsql.Replace("zdgxb", "zdgxb" + sjc).Replace("sccj", "sccj"+sjc)).Tables[0];

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

            return dtzd;
        }
        private DataTable bindDGVZDSQL(string sqltj)
        {
            dataGridView1.DataSource = dbclass.GreatDs(sqltj).Tables[0];
            DataTable dtzd = new DataTable();
            dtzd = dbclass.GreatDs(sccjzdsql).Tables[0];

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

            return dtzd;
        }

        private void bindDGVZDJH()
        {
            dataGridView2.DataSource = dbclass.GreatDs(jhbsql+sjc).Tables[0];
        }

        private void bindCBYS()
        {
            cbxx.Items.Clear();
            cbxy.Items.Clear();
            cbzy.Items.Clear();
            cbbj.Items.Clear();


            cbxx.Items.Insert(0, "");
            cbxy.Items.Insert(0, "");
            cbzy.Items.Insert(0, "");
            cbbj.Items.Insert(0, "");


            string sqlxx = "select distinct xx from SCCJ"+sjc;
            DataTable dtxx = dbclass.GreatDs(sqlxx).Tables[0];
            for (int i = 0; i < dtxx.Rows.Count; i++)
            {
                cbxx.Items.Insert(i + 1, dtxx.Rows[i][0].ToString());
            }

            string sqlxy = "select distinct xy from SCCJ"+sjc;
            DataTable dtxy = dbclass.GreatDs(sqlxy).Tables[0];
            for (int i = 0; i < dtxy.Rows.Count; i++)
            {
                cbxy.Items.Insert(i + 1, dtxy.Rows[i][0].ToString());
            }

            string sqlzy = "select distinct zy from SCCJ"+sjc;
            DataTable dtzy = dbclass.GreatDs(sqlzy).Tables[0];
            for (int i = 0; i < dtzy.Rows.Count; i++)
            {
                cbzy.Items.Insert(i + 1, dtzy.Rows[i][0].ToString());
            }

            string sqlbj = "select distinct bj from SCCJ"+sjc;
            DataTable dtbj = dbclass.GreatDs(sqlbj).Tables[0];
            for (int i = 0; i < dtbj.Rows.Count; i++)
            {
                cbbj.Items.Insert(i + 1, dtbj.Rows[i][0].ToString());
            }
        }

        private void btncx_Click(object sender, EventArgs e)
        {
            string sqlcx = "select * from SCCJ"+sjc+" where 1=1 ";

            string wheresql = "";

            if (cbxx.Text != "")
            {
                wheresql = wheresql + " and xx='" + cbxx.Text + "'";
            }

            if (cbxy.Text != "")
            {
                wheresql = wheresql + " and xy='" + cbxy.Text + "'";
            }

            if (cbzy.Text != "")
            {
                wheresql = wheresql + " and zy='" + cbzy.Text + "'";
            }

            if (cbbj.Text != "")
            {
                wheresql = wheresql + " and bj='" + cbbj.Text + "'";
            }

            sqlcx = sqlcx + wheresql;

            bindDGVZDSQL(sqlcx);
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbfilepath.Text = openFileDialog1.FileName;
                ArrayList nameList = GetExcelTables(tbfilepath.Text.Trim());
                ArrayList nameList2 = GetExcelTables(tbfilepath.Text.Trim());
                if (nameList == null || nameList.Count <= 0)
                {
                    MessageBox.Show("输入的文件不正确，请重新输入The file you entered is incorrect, please re-enter it！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                this.cbSheetlist.BeginUpdate();
                this.cbSheetlist.DataSource = nameList;
                this.cbSheetlist.EndUpdate();
                this.cbSheetlist.Show();

                this.cbSheetlist2.BeginUpdate();
                this.cbSheetlist2.DataSource = nameList2;
                this.cbSheetlist2.EndUpdate();
                this.cbSheetlist2.Show();
            }
        }

        //获取Excel表列表
        private ArrayList GetExcelTables(string FilePath)
        {
            DataTable dt = new DataTable();
            ArrayList TablesList = new ArrayList();

            if (!String.IsNullOrEmpty(FilePath))
            {
                TablesList = ExcelHelper.GetExcelSheet(FilePath);
                return TablesList;
            }
            else
            {
                MessageBox.Show("请输入要导入的Excel文件Please enter the Excel file you want to import！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);//弹出提示对话框
                return null;
            }
        }

        private void btnwr2bjdc_Click(object sender, EventArgs e)
        {
            string sqlbj = "select distinct bj from SCCJ"+sjc+" where 1=1";

            string wheresqldq = "";
            string wheresql = "";

            if (cbxx.Text != "")
            {
                wheresql = wheresql + " and xx='" + cbxx.Text + "'";
                wheresqldq = wheresql;
            }
            else
            {
                MessageBox.Show("请选择学校Please select a school！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbxy.Text != "")
            {
                wheresql = wheresql + " and xy='" + cbxy.Text + "'";
            }
            else
            {
                MessageBox.Show("请选择院系Please select a department！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbzy.Text != "")
            {
                wheresqldq = wheresqldq + " and zy='" + cbzy.Text + "'";
                wheresql = wheresql + " and zy='" + cbzy.Text + "'";
            }
            else
            {
                MessageBox.Show("请选择专业Please select a major！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dtbj = new DataTable();
            sqlbj = sqlbj + wheresql;
            dtbj = dbclass.GreatDs(sqlbj).Tables[0];
            string[] names = null;
            string[] sqlwhere = null;
            if (dtbj.Rows.Count == 0)
            {
                MessageBox.Show("无班级表导出There is no class table export！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int countr = dtbj.Rows.Count;

            sqlwhere = new string[countr + 2];
            names = new string[countr + 2];

            sqlwhere[0] = wheresqldq;
            names[0] = cbxx.Text + cbzy.Text;

            sqlwhere[1] = wheresql;
            names[1] = cbxy.Text + cbzy.Text;
            string temp = "";

            for (int i = 0; i < countr; i++)
            {
                temp = dtbj.Rows[i][0].ToString();
                names[i + 2] = temp + "班";
                sqlwhere[i + 2] = wheresql + " and bj='" + temp + "'";
            }

            ExcelHelper.ExcelSaveExcel(tbfilepath.Text, cbSheetlist.Text, sqlwhere, names, countr + 2);


            MessageBox.Show("数据生成成功The data was generated successfully！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlbj = "select distinct bj from SCCJ"+sjc+" where 1=1 ";

            string wheresqldq = "";
            string wheresql = "";

            if (cbxx.Text != "")
            {
                wheresql = wheresql + " and xx='" + cbxx.Text + "'";
                wheresqldq = wheresql;
            }
            else
            {
                MessageBox.Show("请选择学校Please select a school！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbxy.Text != "")
            {
                wheresql = wheresql + " and xy='" + cbxy.Text + "'";
            }
            else
            {
                MessageBox.Show("请选择院系Please select a department！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbzy.Text != "")
            {
                wheresqldq = wheresqldq + " and zy='" + cbzy.Text + "'";
                wheresql = wheresql + " and zy='" + cbzy.Text + "'";
            }
            else
            {
                MessageBox.Show("请选择专业Please select a major！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dtbj = new DataTable();
            sqlbj = sqlbj + wheresql;
            dtbj = dbclass.GreatDs(sqlbj).Tables[0];
            string[] names = null;
            string[] sqlwhere = null;
            if (dtbj.Rows.Count == 0)
            {
                MessageBox.Show("无班级表导出There is no class table export！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int countr = dtbj.Rows.Count;

            sqlwhere = new string[countr + 2];
            names = new string[countr + 2];

            sqlwhere[0] = wheresqldq;
            names[0] = cbxx.Text + cbzy.Text;

            sqlwhere[1] = wheresql;
            names[1] = cbxy.Text + cbzy.Text;
            string temp = "";

            for (int i = 0; i < countr; i++)
            {
                temp = dtbj.Rows[i][0].ToString();
                names[i + 2] = temp + "班";
                sqlwhere[i + 2] = wheresql + " and bj='" + temp + "'";
            }

            ExcelHelper.ExcelSaveExcelWithJH(tbfilepath.Text, cbSheetlist.Text, cbSheetlist2.Text, sqlwhere, names, countr + 2);


            MessageBox.Show("数据生成成功The data was generated successfully！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbdq_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxy.Items.Clear();
            cbzy.Items.Clear();
            cbbj.Items.Clear();

            cbxy.Items.Insert(0, "");
            cbzy.Items.Insert(0, "");
            cbbj.Items.Insert(0, "");

            string sqlxx = "select distinct xx from sccj"+sjc+" where dq = '" + cbxx.Text + "'";
            DataTable dtxx = dbclass.GreatDs(sqlxx).Tables[0];
            for (int i = 0; i < dtxx.Rows.Count; i++)
            {
                cbxy.Items.Insert(i + 1, dtxx.Rows[i][0].ToString());
            }

            //string sqlnj = "select distinct nj from sccj where dq = '" + cbdq.Text + "'" + " and xx = '" + cbxx.Text + "'";
            //DataTable dtnj = dbclass.GreatDs(sqlnj).Tables[0];
            //for (int i = 0; i < dtnj.Rows.Count; i++)
            //{
            //    cbnj.Items.Insert(i + 1, dtnj.Rows[i][0].ToString());
            //}

            //string sqlbj = "select distinct bj from sccj dq = '" + cbdq.Text + "'" + " and xx = '" + cbxx.Text + "' and nj = '" + cbnj.Text + "'";
            //DataTable dtbj = dbclass.GreatDs(sqlbj).Tables[0];
            //for (int i = 0; i < dtbj.Rows.Count; i++)
            //{
            //    cbbj.Items.Insert(i + 1, dtbj.Rows[i][0].ToString());
            //}
        }

        private void cbxx_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbzy.Items.Clear();
            cbbj.Items.Clear();


            cbzy.Items.Insert(0, "");
            cbbj.Items.Insert(0, "");


            string sqlzy = "select distinct zy from SCCJ"+sjc+" where xx = '" + cbxx.Text + "'" + " and xy = '" + cbxy.Text + "'";
            DataTable dtzy = dbclass.GreatDs(sqlzy).Tables[0];
            for (int i = 0; i < dtzy.Rows.Count; i++)
            {
                cbzy.Items.Insert(i + 1, dtzy.Rows[i][0].ToString());
            }

            //string sqlbj = "select distinct bj from sccj dq = '" + cbdq.Text + "'" + " and xx = '" + cbxx.Text + "' and nj = '" + cbnj.Text + "'";
            //DataTable dtbj = dbclass.GreatDs(sqlbj).Tables[0];
            //for (int i = 0; i < dtbj.Rows.Count; i++)
            //{
            //    cbbj.Items.Insert(i + 1, dtbj.Rows[i][0].ToString());
            //}
        }

        private void cbnj_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbj.Items.Clear();

            cbbj.Items.Insert(0, "");

            string sqlbj = "select distinct bj from SCCJ"+sjc+" where xx = '" + cbxx.Text + "'" + " and xy = '" + cbxy.Text + "' and zy = '" + cbzy.Text + "'";
            DataTable dtbj = dbclass.GreatDs(sqlbj).Tables[0];
            for (int i = 0; i < dtbj.Rows.Count; i++)
            {
                cbbj.Items.Insert(i + 1, dtbj.Rows[i][0].ToString());
            }
        }
    }
}
