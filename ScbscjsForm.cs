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
    public partial class ScbscjsForm : Form
    {
        DBClass dbclass = new DBClass();
        string sccjsql = "select * from SCCJ";
        string delsccjsql = "delete from SCCJ";
        string sccjzdsql = "select * from zdgxb where bmc = 'sccj' order by id asc";
        string xsbsql = "select * from xsb";
        string yscjsql = "select * from yscj";
        string jhbsql = "select * from jhb";
        string deletejhbsql = "delete from jhb";
        string ysffbsql = "select * from ysffb";

        string sjc;
        MainForm fm;

        public ScbscjsForm(MainForm _fm)
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
            dataGridView1.DataSource = dbclass.GreatDs(sccjsql+sjc).Tables[0];
            DataTable dtzd = new DataTable();
            sccjzdsql = sccjzdsql.Replace("zdgxb", "zdgxb" + sjc).Replace("sccj", "sccj"+sjc);
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
            sccjzdsql = "select * from zdgxb where bmc = 'sccj' order by id asc";
            return dtzd;
        }
        private DataTable bindDGVZDSQL(string sqltj)
        {
            dataGridView1.DataSource = dbclass.GreatDs(sqltj).Tables[0];
            DataTable dtzd = new DataTable();
            dtzd = dbclass.GreatDs(sqltj).Tables[0];

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
            cbys.Items.Clear();
            cbxx.Items.Clear();
            cbxy.Items.Clear();
            cbzy.Items.Clear();
            cbbj.Items.Clear();

            cbys.Items.Insert(0, "");
            cbxx.Items.Insert(0, "");
            cbxy.Items.Insert(0, "");
            cbzy.Items.Insert(0, "");
            cbbj.Items.Insert(0, "");

            for (int j = 2; j < dataGridView2.Columns.Count; j++)
            {
                cbys.Items.Insert(j - 1, dataGridView2.Columns[j].HeaderText);
            }

            string sqldq = "select distinct xx from SCCJ"+sjc;
            DataTable dtdq = dbclass.GreatDs(sqldq).Tables[0];
            for (int i = 0; i < dtdq.Rows.Count; i++)
            {
                cbxx.Items.Insert(i + 1, dtdq.Rows[i][0].ToString());
            }

            string sqlxx = "select distinct xy from SCCJ" + sjc;
            DataTable dtxx = dbclass.GreatDs(sqlxx).Tables[0];
            for (int i = 0; i < dtxx.Rows.Count; i++)
            {
                cbxy.Items.Insert(i + 1, dtxx.Rows[i][0].ToString());
            }

            string sqlnj = "select distinct zy from SCCJ" + sjc;
            DataTable dtnj = dbclass.GreatDs(sqlnj).Tables[0];
            for (int i = 0; i < dtnj.Rows.Count; i++)
            {
                cbzy.Items.Insert(i + 1, dtnj.Rows[i][0].ToString());
            }

            string sqlbj = "select distinct bj from SCCJ" + sjc;
            DataTable dtbj = dbclass.GreatDs(sqlbj).Tables[0];
            for (int i = 0; i < dtbj.Rows.Count; i++)
            {
                cbbj.Items.Insert(i + 1, dtbj.Rows[i][0].ToString());
            }
        }

        private void btnbfh_Click(object sender, EventArgs e)
        {
            if (cbys.Text == "")
            {
                MessageBox.Show("请选择百分化的选项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                string sqlys = "select " + cbys.Text + " from jhb"+sjc+" where 分项='满分'";
                string mffx = dbclass.GetOneValue(sqlys);
                string sqlupdateys = "update SCCJ"+sjc+" Set " + cbys.Text + "=(" + cbys.Text + "*(100/" + mffx + "))";
                string sqlupdatemf = "update jhb"+sjc+" Set " + cbys.Text + "=round(" + cbys.Text + "*(100/" + mffx + "),2) where 分项='满分'";
                try
                {
                    dbclass.DoSql(sqlupdateys);
                    dbclass.DoSql(sqlupdatemf);
                    bindDGVZD();
                    bindDGVZDJH();
                    bindCBYS();

                    string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "百分化成功','" +"SCCJ"+sjc + "','" + DateTime.Now + "')";
                    dbclass.DoSql(sqlinsertlog);
                    MessageBox.Show(cbys.Text + "百分化成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "百分化失败','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                    dbclass.DoSql(sqlinsertlog);
                    
                    MessageBox.Show(cbys.Text + "百分化失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void btn1w_Click(object sender, EventArgs e)
        {
            int cks = 0;
            int cjs = 0;

            try
            {
                cks = Convert.ToInt32(tbks.Text);
                cjs = Convert.ToInt32(tbjs.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("类型转换失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int count = cjs - cks + 1;
            if (count <= 0)
            {
                MessageBox.Show("结束值应大于开始值且两个值之差相等！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] sqlpxd = new string[count];
            string tempzd = "";
            for (int i = cks - 1; i < cjs; i++)
            {
                tempzd = dataGridView1.Columns[i].HeaderText;
                sqlpxd[i - (cks - 1)] = "update SCCJ"+sjc+" Set " + tempzd + "= round(" + tempzd + ",1)";
            }

            try
            {
                dbclass.ExecNonQuerySW(sqlpxd);
                bindDGVZD();
                bindDGVZDJH();
                bindCBYS();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "1位小数转换成功','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("1位小数转换成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "1位小数转换失败','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("1位小数转换失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btn2w_Click(object sender, EventArgs e)
        {
            int cks = 0;
            int cjs = 0;

            try
            {
                cks = Convert.ToInt32(tbks.Text);
                cjs = Convert.ToInt32(tbjs.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("类型转换失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int count = cjs - cks + 1;
            if (count <= 0)
            {
                MessageBox.Show("结束值应大于开始值且两个值之差相等！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] sqlpxd = new string[count];
            string tempzd = "";
            for (int i = cks - 1; i < cjs; i++)
            {
                tempzd = dataGridView1.Columns[i].HeaderText;
                sqlpxd[i - (cks - 1)] = "update SCCJ"+sjc+" Set " + tempzd + "= round(" + tempzd + ",2)";
            }

            try
            {
                dbclass.ExecNonQuerySW(sqlpxd);
                bindDGVZD();
                bindDGVZDJH();
                bindCBYS();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "2位小数转换成功','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("2位小数转换成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "2位小数转换失败','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                
                MessageBox.Show("2位小数转换失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            } 
        }

        private void btncx_Click(object sender, EventArgs e)
        {
            string sqlcx = "select * from SCCJ" + sjc+" where 1=1 ";

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

        private void btndc_Click(object sender, EventArgs e)
        {
            try
            {
                ExportDataToExcel(dataGridView1, true);//导出数据
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导出成绩输出表成功','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导出成绩输出表失败','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("导出失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public void ExportDataToExcel(DataGridView dgv, bool isShowExcel)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("数据表无数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            saveFileDialog1.Filter = "Excel文件|*.xls;*.xlsx";
            String saveFileName = "";


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                saveFileName = saveFileDialog1.FileName;
                if (saveFileName.IndexOf(":") < 0) return;
                tbdc.Text = saveFileName;
                try
                {
                    ExcelHelper.SaveExcel(dataGridView1, tbdc.Text);
                    MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnwr2dc_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listDGV2 = new List<string>();
                for (int i = 2; i < dataGridView2.ColumnCount; i++)
                {
                    listDGV2.Add(dataGridView2.Columns[i].HeaderText);
                }

                string[] cellvalue = new string[dataGridView1.ColumnCount];

                string tempsql = "";
                string tempvalue = "";
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    if (listDGV2.Contains(dataGridView1.Columns[i].HeaderText))
                    {
                        tempsql = "select " + dataGridView1.Columns[i].HeaderText + " from jhb"+sjc+" where 分项='满分'";
                        tempvalue = dbclass.GetOneValue(tempsql);
                        cellvalue[i] = double.Parse(tempvalue).ToString("F2");
                    }
                    else
                    {
                        cellvalue[i] = "";
                    }
                }

                ExportDataToExcelWithRow2(dataGridView1, cellvalue, true);//导出数据

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导出带满分成绩输出表成功','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导出带满分成绩输出表失败','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("导出失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public void ExportDataToExcelWithRow2(DataGridView dgv, string[] cellvalue, bool isShowExcel)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("数据表无数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            saveFileDialog1.Filter = "Excel文件|*.xls;*.xlsx";
            String saveFileName = "";


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                saveFileName = saveFileDialog1.FileName;
                if (saveFileName.IndexOf(":") < 0) return;
                tbdc.Text = saveFileName;
                try
                {
                    ExcelHelper.SaveWithRow2Excel(dataGridView1, tbdc.Text, cellvalue);
                    MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnbcdc_Click(object sender, EventArgs e)
        {
            string sqlbj = "select distinct bj from SCCJ"+sjc;

            string sqlcx = "select * from SCCJ"+sjc+" where 1=1 ";

            string wheresql = "";

            if (cbxx.Text != "")
            {
                wheresql = wheresql + " and xx='" + cbxx.Text + "'";
            }
            else
            {
                MessageBox.Show("请选择学校！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbxy.Text != "")
            {
                wheresql = wheresql + " and xy='" + cbxy.Text + "'";
            }
            else
            {
                MessageBox.Show("请选择院系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbzy.Text != "")
            {
                wheresql = wheresql + " and zy='" + cbzy.Text + "'";
            }
            else
            {
                MessageBox.Show("请选择专业！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dtbj = new DataTable();
            dtbj = dbclass.GreatDs(sqlbj).Tables[0];
            string[] sqlscbjb = null;
            string[] names = null;
            DataTable[] dts = null;
            if (dtbj.Rows.Count == 0)
            {
                MessageBox.Show("无班级表导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int countr = dtbj.Rows.Count;

            sqlscbjb = new string[countr];
            names = new string[countr];
            dts = new DataTable[countr];

            for (int i = 0; i < countr; i++)
            {
                names[i] = dtbj.Rows[i][0].ToString();
                sqlscbjb[i] = sqlcx + wheresql + " and bj='" + names[i] + "'";
                dts[i] = dbclass.GreatDs(sqlscbjb[i]).Tables[0];
            }

            try
            {
                ExportDataToExcelBydts(dts, names, true);//导出数据
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导出班级成绩输出表成功','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导出班级成绩输出表失败','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("导出失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public void ExportDataToExcelBydts(DataTable[] dts, string[] names, bool isShowExcel)
        {
            if (dts.Length == 0)
            {
                MessageBox.Show("无数据导出表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            saveFileDialog1.Filter = "Excel文件|*.xls;*.xlsx";
            String saveFileName = "";


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                saveFileName = saveFileDialog1.FileName;
                if (saveFileName.IndexOf(":") < 0) return;
                tbdc.Text = saveFileName;
                try
                {
                    ExcelHelper.SaveDtExcel(dts, names, tbdc.Text);
                    MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnwr2bjdc_Click(object sender, EventArgs e)
        {
            string sqlbj = "select distinct bj from SCCJ"+sjc;

            string sqlcx = "select * from SCCJ"+sjc+" where 1=1 ";

            string wheresql = "";

            if (cbxx.Text != "")
            {
                wheresql = wheresql + " and xx='" + cbxx.Text + "'";
            }
            else
            {
                MessageBox.Show("请选择学校！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbxy.Text != "")
            {
                wheresql = wheresql + " and xy='" + cbxy.Text + "'";
            }
            else
            {
                MessageBox.Show("请选择院系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbzy.Text != "")
            {
                wheresql = wheresql + " and zy='" + cbzy.Text + "'";
            }
            else
            {
                MessageBox.Show("请选择专业！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dtbj = new DataTable();
            dtbj = dbclass.GreatDs(sqlbj).Tables[0];
            string[] sqlscbjb = null;
            string[] names = null;
            DataTable[] dts = null;
            if (dtbj.Rows.Count == 0)
            {
                MessageBox.Show("无班级表导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int countr = dtbj.Rows.Count;

            sqlscbjb = new string[countr];
            names = new string[countr];
            dts = new DataTable[countr];

            for (int i = 0; i < countr; i++)
            {
                names[i] = dtbj.Rows[i][0].ToString();
                sqlscbjb[i] = sqlcx + wheresql + " and bj='" + names[i] + "'";
                dts[i] = dbclass.GreatDs(sqlscbjb[i]).Tables[0];
            }



            try
            {
                List<string> listDGV2 = new List<string>();
                for (int i = 2; i < dataGridView2.ColumnCount; i++)
                {
                    listDGV2.Add(dataGridView2.Columns[i].HeaderText);
                }

                string[] cellvalue = new string[dataGridView1.ColumnCount];

                string tempsql = "";
                string tempvalue = "";
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    if (listDGV2.Contains(dataGridView1.Columns[i].HeaderText))
                    {
                        tempsql = "select " + dataGridView1.Columns[i].HeaderText + " from jhb"+sjc+" where 分项='满分'";
                        tempvalue = dbclass.GetOneValue(tempsql);
                        cellvalue[i] = double.Parse(tempvalue).ToString("F2");
                    }
                    else
                    {
                        cellvalue[i] = "";
                    }
                }

                ExportDataToExcelBydtsWithRow2(dts, names, cellvalue, true);//导出数据

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导出带满分班级成绩输出表成功','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导出带满分班级成绩输出表失败','" + "SCCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("导出失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public void ExportDataToExcelBydtsWithRow2(DataTable[] dts, string[] names, string[] cellvalue, bool isShowExcel)
        {
            if (dts.Length == 0)
            {
                MessageBox.Show("无数据导出表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            saveFileDialog1.Filter = "Excel文件|*.xls;*.xlsx";
            String saveFileName = "";


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                saveFileName = saveFileDialog1.FileName;
                if (saveFileName.IndexOf(":") < 0) return;
                tbdc.Text = saveFileName;
                try
                {
                    ExcelHelper.SaveDtWithRow2Excel(dts, names, tbdc.Text, cellvalue);
                    MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void cbdq_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxy.Items.Clear();
            cbzy.Items.Clear();
            cbbj.Items.Clear();

            cbxy.Items.Insert(0, "");
            cbzy.Items.Insert(0, "");
            cbbj.Items.Insert(0, "");

            string sqlxy = "select distinct xy from SCCJ"+sjc+" where xx = '" + cbxx.Text + "'";
            DataTable dtxy = dbclass.GreatDs(sqlxy).Tables[0];
            for (int i = 0; i < dtxy.Rows.Count; i++)
            {
                cbxy.Items.Insert(i + 1, dtxy.Rows[i][0].ToString());
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

            string sqlbj = "select distinct bj from SCCJ"+sjc+" where dq = '" + cbxx.Text + "'" + " and xx = '" + cbxy.Text + "' and nj = '" + cbzy.Text + "'";
            DataTable dtbj = dbclass.GreatDs(sqlbj).Tables[0];
            for (int i = 0; i < dtbj.Rows.Count; i++)
            {
                cbbj.Items.Insert(i + 1, dtbj.Rows[i][0].ToString());
            }
        }

        private void ScbscjsForm_FormClosing(object sender, FormClosingEventArgs e)
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
                   // fm.ShowDialog();
                }
            }
        }

    }
}
