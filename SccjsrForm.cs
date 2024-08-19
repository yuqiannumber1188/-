using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cjpc.utils.dbutils;
using cjpc.utils;

namespace cjpc
{
    public partial class SccjsrForm : Form
    {
        DBClass dbclass = new DBClass();

        MainForm fm;

        public SccjsrForm(MainForm _fm)
        {
            InitializeComponent();
            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
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
                panel.Location = new System.Drawing.Point(0, 45 * (i - 1) + i * 10);

                Label lbfs = new Label();
                lbfs.Name = "lbfs" + i;
                lbfs.AutoSize = true;
                lbfs.Location = new System.Drawing.Point(220, 20);
                lbfs.Size = new System.Drawing.Size(41, 12);
                lbfs.Text = "分数：";

                TextBox tbfs = new TextBox();
                tbfs.Name = "tbfs" + i;
                tbfs.Size = new System.Drawing.Size(120, 21);
                tbfs.Location = new System.Drawing.Point(260, 16);

                Label lbtm = new Label();
                lbtm.AutoSize = true;
                lbtm.Location = new System.Drawing.Point(30, 20);
                lbtm.Name = "lbtm" + i;
                lbtm.Size = new System.Drawing.Size(41, 12);
                lbtm.Text = "题目：";

                TextBox tbtm = new TextBox();
                tbtm.Location = new System.Drawing.Point(70, 16);
                tbtm.Name = "tbtm" + i;
                tbtm.Size = new System.Drawing.Size(120, 21);
                tbtm.Text = i.ToString();

                Label lblxh = new Label();
                lblxh.AutoSize = true;
                lblxh.Location = new System.Drawing.Point(513, 20);
                lblxh.Name = "lxh" + i;
                lblxh.Size = new System.Drawing.Size(11, 12);
                lblxh.Text = i.ToString();

                panel.Controls.Add(lblxh);
                panel.Controls.Add(tbfs);
                panel.Controls.Add(lbfs);
                panel.Controls.Add(tbtm);
                panel.Controls.Add(lbtm);

                this.pladd.Controls.Add(panel);
            }
        }

        private void btnscb_Click(object sender, EventArgs e)
        {
            string sjc = labelsjc.Text;
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
            string[] zdmcjsb = new string[count];
            string[] szdmc = new string[count];
            string[] zdlx = new string[count];
            string[] zddx = new string[count];
            string[] zdfs = new string[count];
            string[] zdtm = new string[count];
            double[] zdfsf = new double[count];
            string[] sqlalters = new string[count];
            string[] sqljsbalters = new string[count];
            string[] sqlinnsert = new string[count];
            string[] sqlinsertzdgx = new string[count];

            string[] sqljsbinsertzdgx = new string[count];

            try
            {
                for (int i = 1; i < count + 1; i++)
                {
                    Panel panel = (Panel)(pladd.Controls.Find("pl" + i, false)[0]);

                    TextBox txt = (TextBox)(panel.Controls.Find("tbtm" + i, false)[0]);

                    TextBox txt2 = (TextBox)(panel.Controls.Find("tbfs" + i, false)[0]);

                    Label lxh = (Label)(panel.Controls.Find("lxh" + i, false)[0]);

                    zdmc[i - 1] = "m" + lxh.Text;

                    zdmcjsb[i - 1] = zdmc[i - 1];

                    szdmc[i - 1] = txt.Text;

                    zdlx[i - 1] = "decimal(18,3)";

                    zddx[i - 1] = "0";

                    zdtm[i - 1] = txt.Text;
                    zdfs[i - 1] = txt2.Text;
                }

                //zdtm[0] = "zf";
                //zdfs[0] = tbzf.Text.Trim();

                try
                {
                  

                    double zf = double.Parse(tbzf.Text.Trim());
                    double tempzf = 0;
                    for (int a = 0; a < count; a++)
                    {
                        zdfsf[a] = double.Parse(zdfs[a]);
                        tempzf = tempzf + zdfsf[a];
                        sqlinnsert[a] = String.Format("Insert into ysffb" + sjc + "(tm,fs) values ('{0}',{1})", zdtm[a], zdfsf[a]);
                        sqlinsertzdgx[a] = String.Format("Insert into zdgxb"+sjc+"(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", zdmc[a], zdtm[a], "yscj"+sjc);
                        sqljsbinsertzdgx[a] = String.Format("Insert into zdgxb"+ sjc +"(yzdmc,szdmc,bmc) values ('{0}','{1}','{2}')", zdmcjsb[a], zdtm[a], "jsb" + sjc);
                    }

                    if (zf != tempzf)
                    {
                        MessageBox.Show("各分项赋分值之和不等于总分，请检查各分项值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("得分的类型为数字！" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                sqlalters = DBUtils.GetAlterTableSql("YSCJ" + sjc, count, zdmc, zdlx, zddx);
                sqljsbalters = DBUtils.GetAlterTableSql("jsb" + sjc, count, zdmcjsb, zdlx, zddx);
                //string sqlaltersmc = "alter table yscj add mc int";
                dbclass.ExecNonQuerySW(sqlalters);
                dbclass.ExecNonQuerySW(sqljsbalters);
                //dbclass.DoSql(sqlaltersmc);

                dbclass.ExecNonQuerySW(sqlinnsert);

                dbclass.ExecNonQuerySW(sqlinsertzdgx);

                dbclass.ExecNonQuerySW(sqljsbinsertzdgx);

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "生成新的成绩表','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("创建成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex1)
            {
                MessageBox.Show("请添加相关题目名称！" + ex1.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbfilepath.Text = openFileDialog1.FileName;
                ArrayList nameList = GetExcelTables(tbfilepath.Text.Trim());
                if (nameList == null || nameList.Count <= 0)
                {
                    MessageBox.Show("输入的文件不正确，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                this.cbSheetlist.BeginUpdate();
                this.cbSheetlist.DataSource = nameList;
                this.cbSheetlist.EndUpdate();
                this.cbSheetlist.Show();
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
                MessageBox.Show("请输入要导入的Excel文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);//弹出提示对话框
                return null;
            }
        }

        private void btndr_Click(object sender, EventArgs e)
        {
            int cstart = 0;
            int cend = 0;
            int lstart = 0;
            int lend = 0;

            try
            {
                cstart = Convert.ToInt32(tbstart.Text.Trim());
                cend = Convert.ToInt32(tbend.Text.Trim());
                lstart = Convert.ToInt32(tblstart.Text.Trim());
                lend = Convert.ToInt32(tblend.Text.Trim());
            }
            catch
            {
                MessageBox.Show("请输入整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cstart >= cend)
            {
                MessageBox.Show("结束行数应大于起始行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lstart >= lend)
            {
                MessageBox.Show("结束列数应大于起始列！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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

            DataTable dtcj = new DataTable();

            try
            {
                dtcj = ExcelHelper.GetTitleScoreDataTable(tbfilepath.Text, cbSheetlist.Text, cstart, cend, lstart, lend);

                for (int i = 0; i < count; i++)
                {
                    Panel panel = (Panel)(pladd.Controls.Find("pl" + (i+1), false)[0]);

                    TextBox txt = (TextBox)(panel.Controls.Find("tbtm" + (i + 1), false)[0]);

                    TextBox txt2 = (TextBox)(panel.Controls.Find("tbfs" + (i + 1), false)[0]);

                    txt.Text = dtcj.Rows[0][i].ToString();

                    txt2.Text = dtcj.Rows[1][i].ToString();
                }

                MessageBox.Show("导入成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败！" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }    
        }

        private void SccjsrForm_FormClosing(object sender, FormClosingEventArgs e)
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
                    this.Hide();
                   // fm.ShowDialog();
                }
            }
        }
    }
}
