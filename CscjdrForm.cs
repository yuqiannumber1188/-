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
using cjpc.utils.dbutils;
using cjpc.utils;


namespace cjpc
{
    public partial class CscjdrForm : Form
    {
        DBClass dbclass = new DBClass();
        string yscjsql = "select * from yscj order by id asc";
        string jsbsql = "select * from jsb";
        string yscjcountsql = "select count(*) from yscj";
        string yscjzdsql = "select * from zdgxb where bmc = 'yscj' order by id asc";
        string jsbzdsql = "select * from zdgxb where bmc = 'jsb' order by id asc";
        string deletejsbsql = "delete from jsb";
        double minX = -2.5;
        double maxX = 2.5;
        double ydX = 3;
        string sjc;

        MainForm fm;

        public CscjdrForm(MainForm _fm)
        {
            InitializeComponent();
            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
            sjc = labelsjc.Text;

            dataGridView1.DataSource = dbclass.GreatDs(yscjsql.Replace("yscj", "YSCJ"+sjc)).Tables[0];
            dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            bindDGVZD();
        }

        private void bindDGVZD()
        {
            DataTable dtzd = new DataTable();
            dtzd = dbclass.GreatDs(yscjzdsql.Replace("zdgxb", "zdgxb" + sjc).Replace("yscj", "YSCJ" + sjc)).Tables[0];
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

            dtzd = dbclass.GreatDs(jsbzdsql.Replace("zdgxb", "zdgxb" + sjc).Replace("jsb", "jsb"+sjc)).Tables[0];
            for (int j = 0; j < dataGridView2.Columns.Count; j++)
            {
                for (int i = 0; i < dtzd.Rows.Count; i++)
                {
                    if (dataGridView2.Columns[j].HeaderText.Equals(dtzd.Rows[i]["yzdmc"].ToString()))
                    {
                        dataGridView2.Columns[j].HeaderText = dtzd.Rows[i]["szdmc"].ToString();
                        break;
                    }
                }
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

            try
            {
                cstart = Convert.ToInt32(tbstart.Text.Trim());
                cend = Convert.ToInt32(tbend.Text.Trim());
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

            DataTable dtcj = new DataTable();

            dtcj = dbclass.GreatDs(yscjsql.Replace("yscj", "YSCJ"+sjc)).Tables[0];

            try
            {
                dtcj = ExcelHelper.GetDataTable(tbfilepath.Text, cbSheetlist.Text, cstart, cend, dtcj);

                dbclass.UpdateAccess(dtcj, yscjsql.Replace("yscj", "YSCJ"+sjc));
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败！" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            dataGridView1.DataSource = dbclass.GreatDs(yscjsql.Replace("yscj", "YSCJ"+sjc)).Tables[0];
            bindDGVZD();

            string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导入成绩表','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
            dbclass.DoSql(sqlinsertlog);

            MessageBox.Show("导入成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnqksj_Click(object sender, EventArgs e)
        {
            string sql = "delete from yscj"+sjc;
            dbclass.DoSql(sql);

            dataGridView1.DataSource = dbclass.GreatDs(yscjsql.Replace("yscj", "YSCJ"+sjc)).Tables[0];
            bindDGVZD();
            string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "清空成绩表','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
            dbclass.DoSql(sqlinsertlog);
            MessageBox.Show("已清空数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnnd_Click(object sender, EventArgs e)
        {
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "1";
            avgz[1] = "难度";
            string sqlzf = "";
            string sqlavg = "";
            double nd = 0.0;

            DataTable dttemp = new DataTable();
            for (int j = 2; j < countc; j++)
            {
                string tempZf = "";
                if (jsbdt.Columns[j].ColumnName.Equals("df"))
                {
                    sqlzf = "select sum(fs) from ysffb"+sjc;
                }
                else
                {
                    sqlzf = "select A.fs from (SELECT zdgxb"+sjc+".yzdmc,zdgxb"+sjc+".szdmc,ysffb"+sjc+".fs,zdgxb"+sjc+".bmc FROM zdgxb"+sjc+" INNER JOIN ysffb"+sjc+" ON zdgxb"+sjc+".szdmc = ysffb"+sjc+".tm) as A where A.bmc = 'jsb"+sjc+"' and A.yzdmc = '" + jsbdt.Columns[j].ColumnName + "'";
                }
                tempZf = dbclass.GetOneValue(sqlzf);
                sqlavg = "select AVG(" + jsbdt.Columns[j].ColumnName + ")/" + tempZf + " from YSCJ"+sjc;
                avgz[j] = dbclass.GetOneValue(sqlavg);
            }

            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else
                {
                    if(i==2)
                    {
                        nd = 1 - double.Parse(avgz[i]);
                    }

                    dr[i] = 1 - double.Parse(avgz[i]);
                }
            }

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlnd = "insert into JDCL_INFO(CJ_CODE,CJ_LXZ,ND) values ('YSCJ" + sjc + "','10001'," + nd+")";
                dbclass.DoSql(sqlnd);

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "生成难度','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("难度生成成功Successfully generated difficulty！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("难度生成失败Difficulty generation failed！" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "生成难度失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                return;
            }
        }

        private void btnmc_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable jsbdt = new DataTable();
                jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
                int countc = jsbdt.Columns.Count;
                string[] sqlaltersmc = new string[countc - 2];
                string sqlmc = "SELECT px.xh,";
                string sqlmcw = " FROM YSCJ"+sjc+" AS px";
                string sqlmct = "insert into px" + sjc;
                string zdm = "mc,";

                for (int j = 2, i = 0; j < countc; j++, i++)
                {
                    if (jsbdt.Columns[j].ColumnName.Equals("df"))
                    {
                        sqlaltersmc[i] = "alter table px"+sjc+" add mc int";
                       // sqlmc[i] = "update yscj a Set mc=dcount(\"df\",\"yscj\",\"df > \" & a.df) +1";
                        sqlmc = sqlmc + "(SELECT   COUNT(*) AS mc FROM YSCJ" + sjc + " WHERE   (df > px.df)) + 1 AS mc,";
                    }
                    else
                    {
                        sqlaltersmc[i] = "alter table px"+sjc+" add mc" + jsbdt.Columns[j].ColumnName + " int";
                       // sqlmc[i] = "update yscj a Set mc" + jsbdt.Columns[j].ColumnName + "=dcount(\"" + jsbdt.Columns[j].ColumnName + "\",\"yscj\",\"" + jsbdt.Columns[j].ColumnName + " > \" & a." + jsbdt.Columns[j].ColumnName + ") +1";
                        sqlmc = sqlmc + "(SELECT   COUNT(*) AS mc FROM YSCJ" + sjc + " AS YSCJ" + sjc + i + " WHERE   (" + jsbdt.Columns[j].ColumnName + " > px" + jsbdt.Columns[j].ColumnName + "." + jsbdt.Columns[j].ColumnName + ")) + 1 AS mc" + jsbdt.Columns[j].ColumnName + ",";
                        sqlmcw = sqlmcw + "  INNER JOIN YSCJ" + sjc + " AS px" + jsbdt.Columns[j].ColumnName + " ON px.xh = px" + jsbdt.Columns[j].ColumnName + ".xh ";
                        zdm = zdm + "mc"+jsbdt.Columns[j].ColumnName + ",";
                    }
                }

                //sqlaltersmc[countc - 2] = "alter table YSCJ" + sjc + " add djf decimal(18,3)";
                sqlmc = sqlmct + " " + sqlmc.Substring(0, sqlmc.Length - 1) + sqlmcw + "ORDER BY px.xh";
                zdm = zdm.Substring(0, zdm.Length - 1);

                bool flagmc = false;
                string sqlcxzd = "select * from px"+sjc+ " where id = null";
                DataTable yscjcxzd = dbclass.GreatDs(sqlcxzd).Tables[0];
                for (int i = 0; i < yscjcxzd.Columns.Count;i++ )
                {
                    if(yscjcxzd.Columns[i].ColumnName.Contains("mc"))
                    {
                        flagmc = true;
                        break;
                    }
                }

                if (flagmc == false)
                {
                    dbclass.ExecNonQuerySW(sqlaltersmc);
                }
   
                dbclass.DoSql(sqlmc);
                

                dbclass.DoSql(DBUtils.GetVPXSql(sjc,zdm));
                string sqlinsertview = "Insert into CREATE_VIEW_ZDM(sjc,zdm,viewlx) values ('" + sjc + "','" + zdm +"','" + "YSCJ" + "')";
                dbclass.DoSql(sqlinsertview);

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "创建排序视图成功','" + "VRE_YSCJ_PX" + sjc + "_INFO" + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                dataGridView1.DataSource = dbclass.GreatDs(yscjsql.Replace("yscj", "YSCJ"+sjc)).Tables[0];
                bindDGVZD();

                sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "生成名次成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("名次生成成功Rank generation successful！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("名次生成失败Rank generation failed！" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "生成名次失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
            }
        }

        private void btnqfd_Click(object sender, EventArgs e)
        {
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "2";
            avgz[1] = "区分度";
            string sqlcount = "";
            string sqlavg = "";
            string sqlzf = "";
            double zqfd = 0.0;
            double zxxd = 0.0;
            DataTable dttemp = new DataTable();
            for (int j = 2; j < countc; j++)
            {
                try
                {
                    string tempCount = "";
                    string tempZf = "";
                    if (jsbdt.Columns[j].ColumnName.Equals("df"))
                    {
                        sqlzf = "select sum(fs) from ysffb"+sjc;
                    }
                    else
                    {
                        sqlzf = "select A.fs from (SELECT zdgxb"+sjc+".yzdmc,zdgxb"+sjc+".szdmc,ysffb"+sjc+".fs,zdgxb"+sjc+".bmc FROM zdgxb"+sjc+" INNER JOIN ysffb"+sjc+" ON zdgxb"+sjc+".szdmc = ysffb"+sjc+".tm) as A where A.bmc = 'jsb"+sjc+"' and A.yzdmc = '" + jsbdt.Columns[j].ColumnName + "'";
                    }
                    tempZf = dbclass.GetOneValue(sqlzf);

                    sqlcount = "select count(*) from YSCJ"+sjc;
                    tempCount = dbclass.GetOneValue(sqlcount);

                    sqlavg = "select " + jsbdt.Columns[j].ColumnName + " from YSCJ"+sjc;
                    dttemp = dbclass.GreatDs(sqlavg).Tables[0];

                    int cCount = dttemp.Rows.Count;
                    double[] tempmed = new double[cCount];
                    for (int c = 0; c < cCount; c++)
                    {
                        tempmed[c] = double.Parse(dttemp.Rows[c][0].ToString());
                    }
                    tempmed = Toolimp.Sort(tempmed);
                    double cd = Math.Ceiling(cCount * 0.27)-1;
                    double cg = Math.Ceiling(cCount * 0.73);
                    double ad = Toolimp.Avg(tempmed, 0, (int)cd);
                    double ag = Toolimp.Avg(tempmed, (int)cg, cCount);

                    double qfd = (ag - ad) / double.Parse(tempZf);

                    if(qfd > 1)
                    {
                         MessageBox.Show("区分度计算出现问题,请检查录入的原始分数的满分值There is an issue with the differentiation calculation. Please check the full score value of the original score entered", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "区分度计算出现问题,请检查录入的原始分数的满分值','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                         dbclass.DoSql(sqlinsertlog);
                         return;
                    }

                    avgz[j] = qfd.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("区分度计算出现问题Problem with discrimination calculation" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "区分度计算出现问题','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                    dbclass.DoSql(sqlinsertlog);
                    return;
                }

            }
            //效度计算
            double zxxdjs = 0;

            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else
                {
                    if(i==2)
                    {
                        zqfd = double.Parse(avgz[i]);
                    }
                    
                    dr[i] = double.Parse(avgz[i]);
                    zxxdjs = zxxdjs + double.Parse(avgz[i]);
                }
            }

            zxxd = zxxdjs / (countc - 2);

            tbxxd.Text = zxxd.ToString();

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlqfd = "UPDATE JDCL_INFO SET QFD = " + zqfd + ",XXD = " + zxxd + " WHERE (CJ_CODE = 'YSCJ" + sjc + "')";
                dbclass.DoSql(sqlqfd);

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "生成区分度、效度','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("区分度生成成功Differentiation generation successful！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("区分度生成失败Differentiation generation failed" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "区分度生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                return;
            }
        }

        private void btngjnlz_Click(object sender, EventArgs e)
        {
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "3";
            avgz[1] = "估计能力计算值";
            string sqlzf = "";
            string sqlavg = "";

            DataTable dttemp = new DataTable();
            for (int j = 2; j < countc; j++)
            {
                string tempZf = "";
                if (jsbdt.Columns[j].ColumnName.Equals("df"))
                {
                    sqlzf = "select sum(fs) from ysffb"+sjc;
                }
                else
                {
                    sqlzf = "select A.fs from (SELECT zdgxb"+sjc+".yzdmc,zdgxb"+sjc+".szdmc,ysffb"+sjc+".fs,zdgxb"+sjc+".bmc FROM zdgxb"+sjc+" INNER JOIN ysffb"+sjc+" ON zdgxb"+sjc+".szdmc = ysffb"+sjc+".tm) as A where A.bmc = 'jsb"+sjc+"' and A.yzdmc = '" + jsbdt.Columns[j].ColumnName + "'";
                }
                tempZf = dbclass.GetOneValue(sqlzf);
                sqlavg = "select AVG(" + jsbdt.Columns[j].ColumnName + ")/" + tempZf + " from yscj"+sjc;
                avgz[j] = dbclass.GetOneValue(sqlavg);
            }

            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else
                {
                    dr[i] = double.Parse(avgz[i]);
                }
            }

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "估计能力计算值生成成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("估计能力计算值生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "估计能力计算值生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("估计能力计算值生成失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btngjnl_Click(object sender, EventArgs e)
        {
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "4";
            avgz[1] = "估计能力值";
            string sqlavg = "";
            string sqlNd = "";
            string sqlQfd = "";
            string sqlNljs = "";
            string tempNd = "";
            string tempQfd = "";
            string tempNljs = "";

            DataTable dttemp = new DataTable();
            for (int j = 2; j < countc; j++)
            {
                sqlNd = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+" where 分项 = '难度'";
                sqlQfd = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+" where 分项 = '区分度'";
                sqlNljs = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+ " where 分项 = '估计能力计算值'";
                tempNd = dbclass.GetOneValue(sqlNd);
                tempQfd = dbclass.GetOneValue(sqlQfd);
                tempNljs = dbclass.GetOneValue(sqlNljs);

                if (double.Parse(tempQfd) == 0)
                {
                    if (double.Parse(tempNd) == 0 || double.Parse(tempNd) == 1)
                    {
                        avgz[j] = "0";
                    }
                    else
                    {
                        sqlavg = "select " + tempNd + " - LOG(1/" + tempNljs + "-1)/1.702" + " from jsb" + sjc;
                    }    
                }
                else
                {
                    sqlavg = "select " + tempNd + " - LOG(1/" + tempNljs + "-1)/(1.702*" + tempQfd + ")" + " from jsb" + sjc;
                }
               
                avgz[j] = dbclass.GetOneValue(sqlavg);
            }

            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else
                {
                    dr[i] = double.Parse(avgz[i]);
                }
            }

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "估计能力值生成成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("估计能力值生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "估计能力值生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("估计能力值生成失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnxznl_Click(object sender, EventArgs e)
        {
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "5";
            avgz[1] = "修正能力值";

            //string sqlNljs = "";
            string sqlNlgj = "";
            //string tempNljs = "";
            string tempNlgj = "";

            DataTable dttemp = new DataTable();
            for (int j = 2; j < countc; j++)
            {

                //sqlNljs = "select " + jsbdt.Columns[j].ColumnName + " from jsb where 分项 = '估计能力计算值'";
                sqlNlgj = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+" where 分项 = '估计能力值'";

                //tempNljs = dbclass.GetOneValue(sqlNljs);
                tempNlgj = dbclass.GetOneValue(sqlNlgj);

                //double fNljs = Double.Parse(tempNljs);
                double fNlgj = Double.Parse(tempNlgj);

                //avgz[j] = Math.Pow(0.5, Math.Abs(fNlgj)).ToString();

                if (fNlgj > maxX)
                {
                    avgz[j] = (maxX + ydX).ToString();
                }
                else if (fNlgj < minX)
                {
                    avgz[j] = (minX + ydX).ToString();
                }
                else
                {
                    avgz[j] = (fNlgj + ydX).ToString();
                }
            }

            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else
                {
                    dr[i] = double.Parse(avgz[i]);
                }
            }

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "修正能力值生成成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("修正能力值生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "修正能力值生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("修正能力值生成失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnxzfs_Click(object sender, EventArgs e)
        {
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "6";
            avgz[1] = "修正分数";

            string sqlNlxz = "";

            string tempNlxz = "";

            double fNlxz = 0;


            DataTable dttemp = new DataTable();
            for (int j = 3; j < countc; j++)
            {

                sqlNlxz = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+" where 分项 = '修正能力值'";

                tempNlxz = dbclass.GetOneValue(sqlNlxz);

                fNlxz = fNlxz + Double.Parse(tempNlxz);

            }

            string sqlzf = "select sum(fs) from ysffb"+sjc;
            string tempzf = dbclass.GetOneValue(sqlzf);
            double dzf = Double.Parse(tempzf);

            for (int j = 3; j < countc; j++)
            {

                sqlNlxz = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+" where 分项 = '修正能力值'";

                tempNlxz = dbclass.GetOneValue(sqlNlxz);

                avgz[j] = (dzf / fNlxz * Double.Parse(tempNlxz)).ToString();

            }



            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else if (i == 2)
                {
                    dr[i] = 0;
                }
                else
                {
                    dr[i] = double.Parse(avgz[i]);
                }
            }

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "修正分数生成成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("修正分数生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "修正分数生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("修正分数生成失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnxzdf_Click(object sender, EventArgs e)
        {
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "7";
            avgz[1] = "修正单分数";

            string sqlNlxz = "";

            string tempNlxz = "";

            string sqlzf = "";

            string tempzf = "";

            DataTable dttemp = new DataTable();
            for (int j = 3; j < countc; j++)
            {

                sqlNlxz = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+" where 分项 = '修正分数'";

                tempNlxz = dbclass.GetOneValue(sqlNlxz);

                sqlzf = "select A.fs from (SELECT zdgxb"+sjc+".yzdmc,zdgxb"+sjc+".szdmc,ysffb"+sjc+".fs,zdgxb"+sjc+".bmc FROM zdgxb"+sjc+" INNER JOIN ysffb"+sjc+" ON zdgxb"+sjc+".szdmc = ysffb"+sjc+".tm) as A where A.bmc = 'jsb"+sjc+"' and A.yzdmc = '" + jsbdt.Columns[j].ColumnName + "'";

                tempzf = dbclass.GetOneValue(sqlzf);

                avgz[j] = (Double.Parse(tempNlxz) / Double.Parse(tempzf)).ToString();

            }


            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else if (i == 2)
                {
                    dr[i] = 0;
                }
                else
                {
                    dr[i] = double.Parse(avgz[i]);
                }
            }

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "修正单分数生成成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("修正单分数生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "修正单分数生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("修正单分数生成失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnavg_Click(object sender, EventArgs e)
        {
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "8";
            avgz[1] = "平均值";

            string sqlavg = "";

            DataTable dttemp = new DataTable();
            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select AVG(" + jsbdt.Columns[j].ColumnName + ")" + " from yscj"+sjc;
                avgz[j] = dbclass.GetOneValue(sqlavg);
            }

            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else
                {
                    dr[i] = double.Parse(avgz[i]);
                }
            }

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "平均值生成成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("平均值生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "平均值生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("平均值生成失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnfc_Click(object sender, EventArgs e)
        {
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "9";
            avgz[1] = "方差";

            string sqlavg = "";

            DataTable dttemp = new DataTable();
            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select VarP(" + jsbdt.Columns[j].ColumnName + ")" + " from yscj"+sjc;
                avgz[j] = dbclass.GetOneValue(sqlavg);
            }

            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else
                {
                    dr[i] = double.Parse(avgz[i]);
                }
            }

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "方差生成成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("方差生成成功Variance generation successful！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "方差生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("方差生成失败Variance generation failed" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnbzc_Click(object sender, EventArgs e)
        {
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "10";
            avgz[1] = "标准差";

            string sqlavg = "";

            DataTable dttemp = new DataTable();
            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select StDevP(" + jsbdt.Columns[j].ColumnName + ")" + " from yscj"+sjc;
                avgz[j] = dbclass.GetOneValue(sqlavg);
            }

            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else
                {
                    dr[i] = double.Parse(avgz[i]);
                }
            }

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "标准差生成成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("标准差生成成功Standard deviation generation！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "标准差生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("标准差生成失败Standard deviation generation failed" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnqkjs_Click(object sender, EventArgs e)
        {
            string sql = "delete from jsb"+sjc;
            dbclass.DoSql(sql);

            dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            bindDGVZD();
            MessageBox.Show("已清空数据Data cleared！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnsc_Click(object sender, EventArgs e)
        {
            try
            {
                btnnd_Click(sender, e);
                btnmc_Click(sender, e);
                btnqfd_Click(sender, e);
                //btnscp_Click(sender, e);
               // btngjnlz_Click(sender, e);
               // btngjnl_Click(sender, e);
               // btnxznl_Click(sender, e);
               // btnxzfs_Click(sender, e);
               // btnxzdf_Click(sender, e);
                btnavg_Click(sender, e);
                btnfc_Click(sender, e);
                btnbzc_Click(sender, e);
                button1_Click(sender, e);
                btndjf_Click(sender, e);
                MessageBox.Show("数据生成data generator！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show("错误Error：" + ex.Message);
            }
           

        }

        private void btnscp_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable jsbdt = new DataTable();
                jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
                int countc = jsbdt.Columns.Count;
                string[] sqlaltersp = new string[countc - 2];
                string[] sqlaltersq = new string[countc - 2];
                string[] sqlaltersqf = new string[countc - 2];

                //人员数
                DataTable yscjdt = new DataTable();
                yscjdt = dbclass.GreatDs(yscjsql.Replace("yscj", "YSCJ"+sjc)).Tables[0];
                int countyscj = yscjdt.Rows.Count;
                Dictionary<string, List<string>> xhpqf = new Dictionary<string, List<string>>(); //学号，相关分数
                Dictionary<string, double> xhqf = new Dictionary<string, double>(); //学号，q值
                List<string> temppqf = null;
                //string[,] sjlp = new string[countc - 1,countyscj];
                //string[,] sjlq = new string[countc - 1, countyscj];

                double[] sjq = new double[countyscj];
                string[] sqlpqf = new string[countyscj];
                
                //选择顺序位
                int ids = int.Parse(yscjdt.Rows[0][0].ToString());

                string sqlsxw = "";
                string sqlsxwcount = ""; 

                double data = 0;
                string sdata = "";
                int sxwcount = 0;
                double pdata = 0;

                string sqlxh = "";
                string xh = "";

                //计算能力值
                string sqlNd = "";
                string sqlQfd = "";

                string tempNd = "";
                string tempQfd = "";
               
                double ftempNd=0;
                double ftempQfd=0;
                double qdata = 0;

                //计算能力分数
                string sqlzf = "";
                string tempZf = "";
                double tempZfdata = 0.0;
                double[] qf = new double[countyscj];


                xhpqf.Clear();


                double maxqdata = 0;
                string sqlinsertqb = "INSERT INTO pqf"+sjc+" (xh,";
                string sqlinsertzb = ") VALUES (";

                for (int j = 2, i = 0; j < countc; j++, i++)
                {
                    temppqf = null;
                    if (jsbdt.Columns[j].ColumnName.Equals("df"))
                    {
                        sqlaltersp[i] = "alter table pqf" + sjc + " add p decimal(18,3)";
                        sqlaltersq[i] = "alter table pqf" + sjc + " add q decimal(18,3)";
                        sqlaltersqf[i] = "alter table pqf" + sjc + " add qf decimal(18,3)";
                        sqlinsertqb = sqlinsertqb + "p,q,qf,"; 

                        sqlzf = "select sum(fs) from ysffb"+sjc;
                        tempZf = dbclass.GetOneValue(sqlzf);
                        tempZfdata = double.Parse(tempZf);
                        xhqf.Clear();
                        maxqdata = 0;
                        for (int a = 0; a < countyscj; a++ )
                        {
                            //学号
                            sqlxh = "select xh from YSCJ" + sjc + " where id = " + (a + ids);
                            xh = dbclass.GetOneValue(sqlxh);

                            if (xhpqf.ContainsKey(xh))
                            {
                                temppqf = xhpqf[xh];
                            }
                            else
                            {
                                temppqf = new List<string>();
                            }
                            
                            //计算p
                            sqlsxw = "select df from YSCJ"+ sjc +" where id = " + (a + ids);
                            sdata = dbclass.GetOneValue(sqlsxw);
                            data = double.Parse(sdata);
                            sqlsxwcount = "select count(*) from YSCJ"+sjc+" where df <= " + data;
                            sxwcount = int.Parse(dbclass.GetOneValue(sqlsxwcount));
                            pdata = double.Parse(((double)sxwcount / countyscj).ToString("#0.00"));
                            //sqlp[i, a] = "UPDATE YSCJ" + sjc + " Set p = " + pdata + " where id=" + (a + ids);
                           

                            //计算q
                            sqlNd = "select df from jsb"+sjc+" where 分项 = '难度'";
                            sqlQfd = "select df from jsb" + sjc + " where 分项 = '区分度'";
                            tempNd = dbclass.GetOneValue(sqlNd);
                            tempQfd = dbclass.GetOneValue(sqlQfd);
                            ftempNd = double.Parse(tempNd);
                            ftempQfd = double.Parse(tempQfd);
                            if (ftempQfd == 0)
                            {
                                qdata = double.Parse((ftempNd - Math.Log(1 / pdata - 1) / 1.702).ToString("#0.00"));
                            }
                            else
                            {
                                qdata = double.Parse((ftempNd - Math.Log(1 / pdata - 1) / (1.702 * ftempQfd)).ToString("#0.00"));
                            }

                            if (qdata > 2.5)
                            {
                                qdata = 2.5;
                            }
                            else if (qdata < -2.5)
                            {
                                qdata = -2.5;
                            }
                           //计算qf
                            if (qdata > maxqdata)
                                maxqdata = qdata;

                            xhqf.Add(xh, qdata);

                            temppqf.Add(pdata.ToString());
                            temppqf.Add(qdata.ToString());
                            if (xhpqf.ContainsKey(xh))
                            {
                                xhpqf[xh] = temppqf;
                            }
                            else
                            {
                                xhpqf.Add(xh, temppqf);
                            }
                            // sqlq[i, a] = "update YSCJ"+sjc+" Set q = " + qdata + " where id=" + (a + ids);
                        }

                       // double maxqdata = Toolimp.MaxNum(sjq);
                        double temfs = double.Parse((tempZfdata / (maxqdata + 2.5)).ToString("#0.00"));
                        //遍历key
                        foreach (string key in xhqf.Keys)
                        {
                            double tempqf = (xhqf[key] + 2.5) * temfs;

                            temppqf = xhpqf[key];

                            temppqf.Add(tempqf.ToString());

                            xhpqf[key] = temppqf; 
                        }
                          
                    }
                    else
                    {
                        sqlaltersp[i] = "alter table pqf" + sjc + " add p" + jsbdt.Columns[j].ColumnName + " decimal(18,3)";
                        sqlaltersq[i] = "alter table pqf" + sjc + " add q" + jsbdt.Columns[j].ColumnName + " decimal(18,3)";
                        sqlaltersqf[i] = "alter table pqf" + sjc + " add qf" + jsbdt.Columns[j].ColumnName + " decimal(18,3)";
                        sqlinsertqb = sqlinsertqb + "p" + jsbdt.Columns[j].ColumnName + ",q" + jsbdt.Columns[j].ColumnName + ",qf" + jsbdt.Columns[j].ColumnName + ",";

                        sqlzf = "select A.fs from (SELECT zdgxb"+sjc+".yzdmc,zdgxb"+sjc+".szdmc,ysffb"+sjc+".fs,zdgxb"+sjc+".bmc FROM zdgxb"+sjc+" INNER JOIN ysffb"+sjc+" ON zdgxb"+sjc+".szdmc = ysffb"+sjc+".tm) as A where A.bmc = 'jsb"+sjc+"' and A.yzdmc = '" + jsbdt.Columns[j].ColumnName + "'";
                
                        tempZf = dbclass.GetOneValue(sqlzf);
                        tempZfdata = double.Parse(tempZf);
                        xhqf.Clear();
                        maxqdata = 0;
                        for (int a = 0; a < countyscj; a++)
                        {
                            //学号
                            sqlxh = "select xh from YSCJ" + sjc + " where id = " + (a + ids);
                            xh = dbclass.GetOneValue(sqlxh);

                            if (xhpqf.ContainsKey(xh))
                            {
                                temppqf = xhpqf[xh];
                            }
                            else
                            {
                                temppqf = new List<string>();
                            }
                            
                            sqlsxw = "select " + jsbdt.Columns[j].ColumnName + " from YSCJ"+ sjc + " where id = " + (a + ids);
                            sdata = dbclass.GetOneValue(sqlsxw);
                            data = double.Parse(sdata);
                            sqlsxwcount = "select count(*) from YSCJ"+sjc+" where " + jsbdt.Columns[j].ColumnName + " <= " + data;
                            sxwcount = int.Parse(dbclass.GetOneValue(sqlsxwcount));
                            pdata = double.Parse(((double)sxwcount / countyscj).ToString("#0.00"));
                          //  sqlp[i, a] = "update YSCJ" + sjc + " Set p" + jsbdt.Columns[j].ColumnName + " = " + pdata + " where id=" + (a + ids);

                            sqlNd = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+" where 分项 = '难度'";
                            sqlQfd = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc +" where 分项 = '区分度'";
                            tempNd = dbclass.GetOneValue(sqlNd);
                            tempQfd = dbclass.GetOneValue(sqlQfd);
                            ftempNd = double.Parse(tempNd);
                            ftempQfd = double.Parse(tempQfd);
                            if (ftempQfd == 0)
                            {
                                qdata = double.Parse((ftempNd - Math.Log(1 / pdata - 1) / 1.702).ToString("#0.00"));
                            }
                            else
                            {
                                qdata = double.Parse((ftempNd - Math.Log(1 / pdata - 1) / (1.702 * ftempQfd)).ToString("#0.00"));
                            }

                            if (qdata > 2.5)
                            {
                                qdata = 2.5;
                            }
                            else if (qdata < -2.5)
                            {
                                qdata = -2.5;
                            }

                            //计算qf
                            //sjq[a] = qdata;
                            if (qdata > maxqdata)
                                maxqdata = qdata;

                            xhqf.Add(xh, qdata);

                            temppqf.Add(pdata.ToString());
                            temppqf.Add(qdata.ToString());
                            if (xhpqf.ContainsKey(xh))
                            {
                                xhpqf[xh] = temppqf;
                            }
                            else
                            {
                                xhpqf.Add(xh, temppqf);
                            }

                           // sqlq[i, a] = "update YSCJ"+sjc+" Set q" + jsbdt.Columns[j].ColumnName + " = " + qdata + " where id=" + (a + ids);
                        }

                        //double maxqdata = Toolimp.MaxNum(sjq);
                        double temfs = double.Parse((tempZfdata / (maxqdata + 2.5)).ToString("#0.00"));
                        //遍历key
                        foreach (string key in xhqf.Keys)
                        {
                            double tempqf = (xhqf[key] + 2.5) * temfs;

                            temppqf = xhpqf[key];

                            temppqf.Add(tempqf.ToString());

                            xhpqf[key] = temppqf;
                        }
                          
                    }
                }

                sqlinsertqb = sqlinsertqb.Substring(0, sqlinsertqb.Length - 1) + sqlinsertzb;

                 //遍历key
                int t = 0;
                
                foreach (string key in xhpqf.Keys)
                {
                    temppqf = xhpqf[key];

                    sqlpqf[t] = sqlinsertqb + key + "," + string.Join(",", temppqf.ToArray()) + ")";

                    t++;
                }

                dbclass.ExecNonQuerySW(sqlaltersp);
                dbclass.ExecNonQuerySW(sqlaltersq);
                dbclass.ExecNonQuerySW(sqlaltersqf);
                dbclass.ExecNonQuerySW(sqlpqf);
                
                dataGridView1.DataSource = dbclass.GreatDs(yscjsql.Replace("yscj", "YSCJ"+sjc)).Tables[0];
                bindDGVZD();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "PQ能力分生成成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("PQ生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "PQ能力分生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("PQ生成失败！" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];
            //dataGridView2.DataSource = jhbdt;
            DataRow dr = jsbdt.NewRow();
            int countc = jsbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "11";
            avgz[1] = "信度与效度";

            //总分的方差、标准差、平均值
            string vardfs = "select df from jsb"+sjc+" where 分项 = '方差'";
            string varzdfs = dbclass.GetOneValue(vardfs);
            double vardf = double.Parse(varzdfs);

            string stddfs = "select df from jsb"+sjc+" where 分项 = '标准差'";
            string stdzdfs = dbclass.GetOneValue(stddfs);
            double stdevdf = double.Parse(stdzdfs);

            string avgdfs = "select df from jsb"+sjc+" where 分项 = '平均值'";
            string avgzdfs = dbclass.GetOneValue(avgdfs);
            double avgdf = double.Parse(avgzdfs);

            string sqldf = "select df from yscj"+sjc;
            DataTable dttempdf = dbclass.GreatDs(sqldf).Tables[0];
            double[] arraydf = new double[dttempdf.Rows.Count];
            for (int c = 0; c < dttempdf.Rows.Count; c++)
            {
                arraydf[c] = double.Parse(dttempdf.Rows[c][0].ToString());
            }

            //各分项的方差、标准差、平均值
            string varargs = "";
            string varzargs = "";
            double vararg = 0;

            double fxvarsum = 0;
            

            string sdtargs = "";
            string stdzargs = "";
            double stdevarg = 0;

            string avgargs = "";
            string avgzargs = "";
            double avgarg = 0;

            string sqlarg = "";
            DataTable dttemparg = new DataTable();
            double[] arrayarg = new double[dttempdf.Rows.Count];

            double sumzf = 0;


            //效度
            for (int j = 3; j < countc; j++)
            {
                //信度计算
                varargs = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+" where 分项 = '方差'";
                varzargs = dbclass.GetOneValue(varargs);
                vararg = double.Parse(varzargs);
                fxvarsum = fxvarsum + vararg;
                
                //效度计算
                sdtargs = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+" where 分项 = '标准差'";
                stdzargs = dbclass.GetOneValue(sdtargs);
                stdevarg = double.Parse(stdzargs);

                avgargs = "select " + jsbdt.Columns[j].ColumnName + " from jsb"+sjc+" where 分项 = '平均值'";
                avgzargs = dbclass.GetOneValue(avgargs);
                avgarg = double.Parse(avgzargs);

                sqlarg = "select " + jsbdt.Columns[j].ColumnName + " from yscj"+ sjc;
                dttemparg = dbclass.GreatDs(sqlarg).Tables[0];

                for (int c = 0; c < dttemparg.Rows.Count; c++)
                {
                    arrayarg[c] = double.Parse(dttemparg.Rows[c][0].ToString());
                }

                sumzf = 0;
                for (int i = 0; i < arraydf.Length && i < arrayarg.Length; i++)
                {
                    sumzf = sumzf + ((arrayarg[i] - avgdf) / stdevdf) * ((arraydf[i] - avgarg) / stdevarg);
                }

                if (stdevarg == 0)
                {
                    avgz[j] = "0";
                }
                else
                {
                    avgz[j] = Math.Round(sumzf / arraydf.Length, 2).ToString();
                }                
            }
            //信度计算
            int nt = countc - 3;
            int nt_1 = nt -1;

            for (int i = 0; i < countc; i++)
            {
                if (i == 0)
                {
                    dr[i] = Convert.ToDecimal(avgz[i]);
                }
                else if (i == 1)
                {
                    dr[i] = avgz[i];
                }
                else if(i==2)
                {
                    dr[i] = (nt /nt_1)*(1 - fxvarsum/vardf);
                    tbxd.Text = dr[i].ToString();
                    string sqlupdatexd = "update JDCL_INFO set XD = "+dr[i] + " where CJ_CODE='YSCJ"+sjc+"'";
                    dbclass.DoSql(sqlupdatexd);
                }
                else
                {
                    dr[i] = double.Parse(avgz[i]);
                }
            }

            jsbdt.Rows.Add(dr);

            try
            {
                dbclass.DoSql(deletejsbsql.Replace("jsb", "jsb"+sjc));

                dbclass.UpdateAccess(jsbdt, jsbsql.Replace("jsb", "jsb" + sjc));

                dataGridView2.DataSource = dbclass.GreatDs(jsbsql.Replace("jsb", "jsb" + sjc)).Tables[0];

                bindDGVZD();

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "信度与效度生成成功','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("信度与效度生成成功Success in generating reliability and validity！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "信度与效度生成失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("信度与效度生成失败Reliability and validity generation failure!" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


        }

        private void btndjf_Click(object sender, EventArgs e)
        {
            try
            {
                //添加等级分字段
                string sqlaltersdjf = "alter table YSCJ" + sjc + " add djf decimal(18,3)";

                //获取总人数
                string sqlzrs = "select count(*) from YSCJ" + sjc;
                int tempZrs = int.Parse(dbclass.GetOneValue(sqlzrs));

                //满分值
                string sqlzf = "select sum(fs) from ysffb" + sjc;
                double mfz = double.Parse(dbclass.GetOneValue(sqlzf));

                //统计
                int ca = 0;
                //修改总数
                string[] sqlxgdjf = new string[tempZrs]; 

                //取值范围
                FWClass<double>[] blfws = new FWClass<double>[8];
                FWClass<int>[] qzfws = new FWClass<int>[8];
                FWClass<double>[] zsfws = new FWClass<double>[8];
                blfws[0] = new FWClass<double>(0, 0.03);
                blfws[1] = new FWClass<double>(0.03, 0.10);
                blfws[2] = new FWClass<double>(0.10, 0.26);
                blfws[3] = new FWClass<double>(0.26, 0.50);
                blfws[4] = new FWClass<double>(0.50, 0.74);
                blfws[5] = new FWClass<double>(0.74, 0.90);
                blfws[6] = new FWClass<double>(0.90, 0.97);
                blfws[7] = new FWClass<double>(0.97, 1);

                qzfws[0] = new FWClass<int>(91, 100);
                qzfws[1] = new FWClass<int>(81, 90);
                qzfws[2] = new FWClass<int>(71, 80);
                qzfws[3] = new FWClass<int>(61, 70);
                qzfws[4] = new FWClass<int>(51, 60);
                qzfws[5] = new FWClass<int>(41, 50);
                qzfws[6] = new FWClass<int>(31, 40);
                qzfws[7] = new FWClass<int>(21, 30);

                //取值语句
                DataTable[] fwdt = new DataTable[8];
                string[] sqlfw = new string[8];
                string[] sqlmaxfw = new string[8];
                string[] sqlminfw = new string[8];

                //相关取值并赋值语句
                for(int i = 0; i < 8; i++)
                {
                    if (ca == tempZrs)
                    {
                        break;
                    }
                    
                    fwdt[i] = new DataTable();
                    // 真实值取值范围
                    sqlmaxfw[i] = "SELECT max(df) from [xyfxxt].[dbo].[VRE_YSCJ_PX" + sjc + "_INFO]" + " where mc > CEILING(" + tempZrs + " * " + blfws[i].Minqz + ") and mc <= CEILING(" + tempZrs + " * " + blfws[i].Maxqz+")";
                    sqlminfw[i] = "SELECT min(df) from [xyfxxt].[dbo].[VRE_YSCJ_PX" + sjc + "_INFO]" + " where mc > CEILING(" + tempZrs + " * " + blfws[i].Minqz + ") and mc <= CEILING(" + tempZrs + " * " + blfws[i].Maxqz+")";
                    if (dbclass.GetOneValue(sqlminfw[i]) != null && !"".Equals( dbclass.GetOneValue(sqlminfw[i])))
                    {
                        zsfws[i] = new FWClass<double>(double.Parse(dbclass.GetOneValue(sqlminfw[i]))/mfz * 100, double.Parse(dbclass.GetOneValue(sqlmaxfw[i]))/mfz * 100);

                        sqlfw[i] = "SELECT id,df from [xyfxxt].[dbo].[VRE_YSCJ_PX" + sjc + "_INFO]" + " where mc > CEILING(" + tempZrs + " * " + blfws[i].Minqz + ") and mc <= CEILING(" + tempZrs + " * " + blfws[i].Maxqz + ")";
                        fwdt[i] = dbclass.GreatDs(sqlfw[i]).Tables[0];
                        for (int j = 0; j < fwdt[i].Rows.Count; j++)
                        {
                            DJFClass djfclass = new DJFClass();
                            djfclass.Stuid = int.Parse(fwdt[i].Rows[j]["id"].ToString());
                            djfclass.Zscj = double.Parse(fwdt[i].Rows[j]["df"].ToString()) / mfz * 100;
                            djfclass.Maxdjffw = qzfws[i].Maxqz;
                            djfclass.Mindjffw = qzfws[i].Minqz;
                            djfclass.Maxzsfw = zsfws[i].Maxqz;
                            djfclass.Minzsfw = zsfws[i].Minqz;

                            sqlxgdjf[ca] = "update YSCJ" + sjc + " Set djf = " + djfclass.Jsdjf() + " where id = " + djfclass.Stuid;

                            if (ca != tempZrs - 1)
                            {
                                ca++;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                }

                bool flagdjf = false;
                string sqlcxzd = "select * from YSCJ" + sjc+" where id is null";
                DataTable yscjcxzd = dbclass.GreatDs(sqlcxzd).Tables[0];
                for (int i = 0; i < yscjcxzd.Columns.Count;i++ )
                {
                    if(yscjcxzd.Columns[i].ColumnName.Equals("djf"))
                    {
                        flagdjf = true;
                        break;
                    }
                }

                if (flagdjf == false)
                {
                    dbclass.DoSql(sqlaltersdjf);
                    string sqldropv = "Drop view VRE_YSCJ_PX" + sjc + "_INFO";
                    dbclass.DoSql(sqldropv);
                    string sqlselectcv = "select zdm from CREATE_VIEW_ZDM where sjc='" + sjc + "' and viewlx = 'YSCJ'";
                    string zdm = dbclass.GetOneValue(sqlselectcv);
                    dbclass.DoSql(DBUtils.GetVPXSql(sjc, zdm));
                }
                
                dbclass.ExecNonQuerySW(sqlxgdjf);
                dataGridView1.DataSource = dbclass.GreatDs(yscjsql.Replace("yscj", "YSCJ"+sjc)).Tables[0];
                bindDGVZD();

                MessageBox.Show("等级分生成成功Level score generation successful！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "生成等级分','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
            }
            catch (Exception ex)
            {
                MessageBox.Show("等级分生成失败Grade score generation failed！" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "生成等级分失败','" + "YSCJ" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ExportDataToExcel(dataGridView1, true);//导出数据
                MessageBox.Show("导出成功Export successful!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败Export failed" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public void ExportDataToExcel(DataGridView dgv, bool isShowExcel)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("数据表无数据No data in the data table", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("保存成功Save successful", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存失败Save failed" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btn_xxd_Click(object sender, EventArgs e)
        {
            //信度
            string sqlxd = "SELECT round(df,2) from jsb where 分项='信度与效度'";
            tbxd.Text = dbclass.GetOneValue(sqlxd);

            //效度
        }

        private void CscjdrForm_FormClosing(object sender, FormClosingEventArgs e)
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
                  //  fm.ShowDialog();
                }
            }
        }
    }
}
