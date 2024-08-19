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
    public partial class ScbscForm : Form
    {
        DBClass dbclass = new DBClass();
        string xsbsql = "select * from xsb";
        string sccjsql = "select * from SCCJ";
        string sccjzdsql = "select * from zdgxb where bmc = 'sccj' order by id asc";
        string deletesccjsql = "delete from zdgxb where bmc = 'sccj'";

        string sjc;
        MainForm fm;

        public ScbscForm(MainForm _fm)
        {
            InitializeComponent();

            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
            sjc = labelsjc.Text;

            dataGridView2.DataSource = dbclass.GreatDs(xsbsql+sjc).Tables[0];
            bindDGVZD();
        }

        private void bindDGVZD()
        {
            dataGridView1.DataSource = dbclass.GreatDs(sccjsql+sjc).Tables[0];
            DataTable dtzd = new DataTable();
            dtzd = dbclass.GreatDs(sccjzdsql.Replace("zdgxb", "zdgxb" + sjc).Replace("sccj", "sccj" + sjc)).Tables[0];

            for (int i = 0; i < dtzd.Rows.Count; i++)
            {
                dataGridView1.Columns[i].HeaderText = dtzd.Rows[i]["szdmc"].ToString();
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            string c = tbadd.Text;
            int count = 0;
            try
            {
                count = Convert.ToInt32(c);
            }
            catch
            {
                MessageBox.Show("增加条数为整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.pladd.Controls.Clear();

            for (int i = 1; i < count + 1; i++)
            {
                Panel panel = new Panel();
                panel.Name = "pl" + i;
                panel.Size = new System.Drawing.Size(600, 45);
                panel.Location = new System.Drawing.Point(40, 45 * (i - 1) + i * 10);

                Label lbtm = new Label();
                lbtm.AutoSize = true;
                lbtm.Location = new System.Drawing.Point(30, 20);
                lbtm.Name = "lbtm" + i;
                lbtm.Size = new System.Drawing.Size(65, 12);
                lbtm.Text = "字段名称：";

                TextBox tbtm = new TextBox();
                tbtm.Location = new System.Drawing.Point(110, 16);
                tbtm.Name = "tbtm" + i;
                tbtm.Size = new System.Drawing.Size(100, 20);

                Label lblx = new Label();
                lblx.AutoSize = true;
                lblx.Location = new System.Drawing.Point(240, 20);
                lblx.Name = "lblx" + i;
                lblx.Size = new System.Drawing.Size(41, 12);
                lblx.Text = "类型：";

                ComboBox cblx = new ComboBox();
                cblx.FormattingEnabled = true;
                cblx.Items.AddRange(new object[] {
            "decimal(18,3)",
            "varchar"});
                cblx.Location = new System.Drawing.Point(285, 17);
                cblx.Name = "cblx" + i;
                cblx.Size = new System.Drawing.Size(83, 20);

                Label lbcd = new Label();
                lbcd.AutoSize = true;
                lbcd.Location = new System.Drawing.Point(394, 20);
                lbcd.Name = "lbcd" + i;
                lbcd.Size = new System.Drawing.Size(41, 12);
                lbcd.Text = "长度：";

                TextBox tbcd = new TextBox();
                tbcd.Location = new System.Drawing.Point(454, 16);
                tbcd.Name = "tbcd" + i;
                tbcd.Size = new System.Drawing.Size(28, 21);
                tbcd.Text = "0";

                Label lblxh = new Label();
                lblxh.AutoSize = true;
                lblxh.Location = new System.Drawing.Point(548, 20);
                lblxh.Name = "lxh" + i;
                lblxh.Size = new System.Drawing.Size(11, 12);
                lblxh.Text = i.ToString();

                panel.Controls.Add(lbtm);
                panel.Controls.Add(tbtm);
                panel.Controls.Add(lblx);
                panel.Controls.Add(cblx);
                panel.Controls.Add(lbcd);
                panel.Controls.Add(tbcd);
                panel.Controls.Add(lblxh);

                this.pladd.Controls.Add(panel);
            }
        }

        private void btnscb_Click(object sender, EventArgs e)
        {
            int zdcount = dataGridView2.ColumnCount - 2;
            string[] zdmc = new string[zdcount];
            string[] zdlx = new string[zdcount];
            string[] zddx = new string[zdcount];
            string[] sqlalters = new string[zdcount];
            string[] sqlinsertzdgx = new string[zdcount];

            for (int i = 2; i < dataGridView2.ColumnCount; i++)
            {
                zdmc[i - 2] = dataGridView2.Columns[i].HeaderText;
                zdlx[i - 2] = "decimal(18,3)";
                zddx[i - 2] = "0";
            }

            sqlalters = DBUtils.GetAlterTableSql("sccj"+sjc, zdcount, zdmc, zdlx, zddx);
            sqlinsertzdgx = DBUtils.InsertZDGXZD(zdcount, zdmc, zdmc, "sccj"+sjc,sjc);
            try
            {
                dbclass.ExecNonQuerySW(sqlalters);
                dbclass.ExecNonQuerySW(sqlinsertzdgx);
                bindDGVZD();
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "原始输出表创建成功" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("创建成功！");
            }
            catch
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "原始输出表创建失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("创建失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnscbzz_Click(object sender, EventArgs e)
        {
            string c = tbadd.Text;
            int count = 0;
            try
            {
                count = Convert.ToInt32(c);
            }
            catch
            {
                MessageBox.Show("增加条数为整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] zdmc = new string[count];
            string[] zdlx = new string[count];
            string[] zddx = new string[count];
            string[] sqlalters = new string[count];
            string[] sqlinsertzdgx = new string[count];
            try
            {
                for (int i = 1; i < count + 1; i++)
                {
                    Panel panel = (Panel)(pladd.Controls.Find("pl" + i, false)[0]);

                    TextBox txttm = (TextBox)(panel.Controls.Find("tbtm" + i, false)[0]);

                    ComboBox cbblx = (ComboBox)(panel.Controls.Find("cblx" + i, false)[0]);

                    TextBox txtcd = (TextBox)(panel.Controls.Find("tbcd" + i, false)[0]);

                    zdmc[i - 1] = txttm.Text;

                    zdlx[i - 1] = cbblx.Text;

                    zddx[i - 1] = txtcd.Text;
                }

                sqlalters = DBUtils.GetAlterTableSql("sccj"+sjc, count, zdmc, zdlx, zddx);

                sqlinsertzdgx = DBUtils.InsertZDGXZD(count, zdmc, zdmc, "sccj"+sjc,sjc);

                dbclass.ExecNonQuerySW(sqlalters);

                dbclass.ExecNonQuerySW(sqlinsertzdgx);

                bindDGVZD();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "原始输出表创建失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("创建成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建失败！" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btncsh_Click(object sender, EventArgs e)
        {
            try
            {
                string[] strSqls = new string[3];
                strSqls[0] = DBUtils.DropSCCJSql(sjc);
                strSqls[1] = DBUtils.GetSCCJSql(sjc);
                strSqls[2] = deletesccjsql.Replace("zdgxb", "zdgxb" + sjc).Replace("sccj", "sccj" + sjc);
                dbclass.ExecNonQuerySW(strSqls);

                string[] insertSqls = new string[10];
                insertSqls = DBUtils.InsertZDGXSCYS(sjc);
                dbclass.ExecNonQuerySW(insertSqls);
                bindDGVZD();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化成功" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("初始化成功！");
            }
            catch
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                
                MessageBox.Show("初始化失败！", "Error");
                return;
            }
        }

        private void ScbscForm_FormClosing(object sender, FormClosingEventArgs e)
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
                 //   fm.ShowDialog();
                }
            }
        }
    }
}
