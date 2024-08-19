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
    public partial class SjjsForm : Form
    {

        DBClass dbclass = new DBClass();
        string sccjsql = "select * from SCCJ";
        string delsccjsql = "delete from SCCJ";
        string sccjzdsql = "select * from zdgxb where bmc = 'sccj' order by id asc";
        string xsbsql = "select * from xsb";
        string yscjsql = "select * from YSCJ";
        string jhbsql = "select * from jhb";
        string deletejhbsql = "delete from jhb";
        string ysffbsql = "select * from ysffb";

        string sjc;
        MainForm fm;

        public SjjsForm(MainForm _fm)
        {
            InitializeComponent();

            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
            sjc = labelsjc.Text;

            bindDGVZD();
            bindDGVZDJH();
            //bindCBYS();

            string tempcjgs = "";
            
            string knowlegexs = dbclass.GetOneValue("SELECT KNOWLEDGE FROM TE_W_INFO WHERE(Code = '" + sjc + "')");
            string skillxs = dbclass.GetOneValue("SELECT SKILL FROM TE_W_INFO WHERE(Code = '" + sjc + "')");
            string abilityxs = dbclass.GetOneValue("SELECT ABILITY FROM TE_W_INFO WHERE(Code = '" + sjc + "')");
            tempcjgs = "知识*" + knowlegexs + "+技能*" + skillxs + "+能力*" + abilityxs;
            lbgs.Text = tempcjgs;
        }

        private DataTable bindDGVZD()
        {
            dataGridView1.DataSource = dbclass.GreatDs(sccjsql+sjc).Tables[0];
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

            DataTable xsbdt = new DataTable();
            xsbdt = dbclass.GreatDs(xsbsql+sjc).Tables[0];
            tbpks.Text = (10 + (xsbdt.Columns.Count - 2) + 1).ToString();

            //for (int i = 0; i < dtzd.Rows.Count; i++)
            //{
            //    dataGridView1.Columns[i].HeaderText = dtzd.Rows[i]["szdmc"].ToString();
            //}

            return dtzd;
        }

        private void bindDGVZDJH()
        {
            dataGridView2.DataSource = dbclass.GreatDs(jhbsql+sjc).Tables[0];
        }

        private void btnjc_Click(object sender, EventArgs e)
        {
            DataTable yscjdt = new DataTable();
            yscjdt = dbclass.GreatDs(yscjsql+sjc).Tables[0];

            DataTable xsbdt = new DataTable();
            xsbdt = dbclass.GreatDs(xsbsql+sjc).Tables[0];

            DataTable sccjdt = new DataTable();
            sccjdt = dbclass.GreatDs(sccjsql+sjc).Tables[0];


            int countrowsccj = yscjdt.Rows.Count;
            int countcolumnssccj = sccjdt.Columns.Count;

            //知识个数
            string sqlzscount = "SELECT  COUNT(*) FROM FXCLASS WHERE(PCLASSID = 10001) AND(SJC = '" + sjc + "')";
            int zscount = int.Parse(dbclass.GetOneValue(sqlzscount));

            //技能个数
            string sqljncount = "SELECT  COUNT(*) FROM FXCLASS WHERE(PCLASSID = 10002) AND(SJC = '" + sjc + "')";
            int jncount = int.Parse(dbclass.GetOneValue(sqljncount));
            //能力个数
            string sqlnlcount = "SELECT  COUNT(*) FROM FXCLASS WHERE(PCLASSID = 10003) AND(SJC = '" + sjc + "')";
            int nlcount = int.Parse(dbclass.GetOneValue(sqlnlcount));

            for (int i = 0; i < countrowsccj; i++)
            {
                DataRow dr = sccjdt.NewRow();
                for (int j = 0; j < countcolumnssccj; j++)
                {
                    if (j < 9)
                    {
                        dr[j] = yscjdt.Rows[i][j];
                    }

                    if (j > 9 && j < 10 + (xsbdt.Columns.Count - 2))
                    {
                        double tempjg = 0;
                        for (int x = 9, y = j - 8, z = 0; z < xsbdt.Rows.Count; x++, z++)
                        {
                            tempjg = tempjg + double.Parse(yscjdt.Rows[i][x].ToString()) * double.Parse(xsbdt.Rows[z][y].ToString());
                        }
                        dr[j] = tempjg;
                    }
                }

                double tempzs = 0;
                for(int t = 0; t < zscount; t++)
                {
                    tempzs = tempzs + double.Parse(dr[13 + t].ToString());
                }
                if(double.Parse(dr[10].ToString()) < tempzs)
                {
                    dr[10] = tempzs;
                }

                tempzs = 0;
                for (int t = 0; t < jncount; t++)
                {
                    tempzs = tempzs + double.Parse(dr[13 + zscount + t].ToString());
                }
                if (double.Parse(dr[11].ToString()) < tempzs)
                {
                    dr[11] = tempzs;
                }

                tempzs = 0;
                for (int t = 0; t < nlcount; t++)
                {
                    tempzs = tempzs + double.Parse(dr[13 + zscount + jncount + t].ToString());
                }
                if (double.Parse(dr[12].ToString()) < tempzs)
                {
                    dr[12] = tempzs;
                }

                sccjdt.Rows.Add(dr);
            }

            try
            {
                dbclass.DoSql(delsccjsql+sjc);

                dbclass.UpdateAccess(sccjdt, sccjsql+sjc);

                string sqlxh = "INSERT INTO scpx"+sjc+"(xh) SELECT xh FROM  YSCJ"+sjc;

                dbclass.DoSql(sqlxh);

                dataGridView1.DataSource = sccjdt;
                bindDGVZDJH();
               // bindCBYS();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "基础计算生成成功" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("生成成功Successfully generated！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "基础计算生成失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                
                MessageBox.Show("生成失败Generation failed！" + ex.ToString(), "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btncsh_Click(object sender, EventArgs e)
        {
            try
            {
                dbclass.DoSql(delsccjsql+sjc);

                string tempstr = "";
                string alterdropsql = "";
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    tempstr = dataGridView1.Columns[j].HeaderText;
                    if (tempstr.Contains("名次"))
                    {
                        alterdropsql = "alter table sccj" + sjc + " drop COLUMN " + tempstr;
                        dbclass.DoSql(alterdropsql);
                    }
                }

                bindDGVZD();
                bindDGVZDJH();
                //bindCBYS();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "输出成绩表初始化成功" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("初始化成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "输出成绩表初始化失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                
                MessageBox.Show("初始化失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }   
        }

        private void btnjs_Click(object sender, EventArgs e)
        {

            //if (cbys1.Text != "" && tbxs1.Text != "")
            //{
            //    tempcjgs = tempcjgs + cbys1.Text + "*" + tbxs1.Text;
            //}
            //else
            //{
            //    MessageBox.Show("请选择相关元素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //if (cbys2.Text != "" && tbxs2.Text != "")
            //{
            //    tempcjgs = tempcjgs + "+" + cbys2.Text + "*" + tbxs2.Text;
            //}

            //if (cbys3.Text != "" && tbxs3.Text != "")
            //{
            //    tempcjgs = tempcjgs + "+" + cbys3.Text + "*" + tbxs3.Text;
            //}

            //if (cbys4.Text != "" && tbxs4.Text != "")
            //{
            //    tempcjgs = tempcjgs + "+" + cbys4.Text + "*" + tbxs4.Text;
            //}

            //if (cbys5.Text != "" && tbxs5.Text != "")
            //{
            //    tempcjgs = tempcjgs + "+" + cbys5.Text + "*" + tbxs5.Text;
            //}
            string updatezcj = "";

            updatezcj = "update SCCJ" + sjc + " Set " + "总成绩" + "=round((" + lbgs.Text + "),1)";


            try
            {
                dbclass.DoSql(updatezcj);
                bindDGVZD();
                bindDGVZDJH();
                //bindCBYS();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "总成绩生成成功" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("总成绩生成成功Total score generated successfully", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "总成绩生成失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("总成绩生成失败Total score generation failed" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }  
        }

        private void btnpxd_Click(object sender, EventArgs e)
        {
            int cks = 0;
            int cjs = 0;
            int cpks = 0;
            int cpjs = 0;
            try
            {
                cks = Convert.ToInt32(tbks.Text);
                cjs = Convert.ToInt32(tbjs.Text);
                cpks = Convert.ToInt32(tbpks.Text);
                cpjs = Convert.ToInt32(tbpjs.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("类型转换失败" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int count = cjs - cks + 1;
            int pcount = cpjs - cpks + 1;
            if (count <= 0 || pcount <= 0 || count != pcount)
            {
                MessageBox.Show("结束值应大于开始值且两个值之差相等！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] sqlalterscpxmc = new string[count];
            string[] sqlalterscjmc = new string[count];
            string[] sqlscpxmc = new string[count];
            string[] sqlscmc = new string[count];
            
            string[] sqlpxd = new string[count];
            string[] zdmc = new string[count];
            string[] zdpxmc = new string[count];
            string[] zdlx = new string[count];
            string[] zddx = new string[count];

            DataTable sccjdt = new DataTable();
            sccjdt = dbclass.GreatDs(sccjsql+sjc).Tables[0];
            string tempzd = "";

            //string sqlmc = "SELECT scpx.xh,";
            //string sqlmcw = " FROM SCCJ" + sjc + " AS scpx";
            //string sqlmct = "insert into scpx" + sjc;
            //string zdm = "";

            for (int i = cks - 1, j = cpks - 1; i < cjs; i++, j++)
            {
                zdmc[i - (cks - 1)] = dataGridView1.Columns[i].HeaderText + "名次";
                zdpxmc[i - (cks - 1)] = dataGridView1.Columns[i].HeaderText + "mmc";
                zdlx[i - (cks - 1)] = "int";
                zddx[i - (cks - 1)] = "0";

                tempzd = dataGridView1.Columns[i].HeaderText;


                sqlscpxmc[i - (cks - 1)] = "UPDATE  scpx" + sjc + " SET " + zdpxmc[i - (cks - 1)] + " = a." + zdmc[i - (cks - 1)] + " FROM (SELECT xh,(SELECT   COUNT(*) AS " + dataGridView1.Columns[i].HeaderText + "mc FROM SCCJ" + sjc + " AS SCCJ" + sjc + "mc WHERE (" + dataGridView1.Columns[i].HeaderText + " > " + dataGridView1.Columns[i].HeaderText + "px." + dataGridView1.Columns[i].HeaderText + ")) + 1 AS " + zdmc[i - (cks - 1)] + " FROM SCCJ" + sjc + " AS " + dataGridView1.Columns[i].HeaderText + "px) AS a INNER JOIN scpx" + sjc + " ON a.xh = scpx" + sjc + ".xh";
                sqlscmc[i - (cks - 1)] = "UPDATE  SCCJ" + sjc + " SET  " + zdmc[i - (cks - 1)] + " = scpx" + sjc + "." + zdpxmc[i - (cks - 1)] + " FROM  scpx"+sjc+" INNER JOIN SCCJ"+sjc+" ON scpx"+sjc+".xh = SCCJ"+sjc+".xh";

                //sqlscmc[i - (cks - 1)] = "update sccj a Set " + zdmc[i - (cks - 1)] + "=dcount(\"" + tempzd + "\",\"sccj\",\"" + tempzd + " > \" & a." + tempzd + ") +1";
                //sqlmc = sqlmc + "(SELECT   COUNT(*) AS mc FROM SCCJ" + sjc + " AS SCCJ" + sjc + i + " WHERE   (" + dataGridView1.Columns[i].HeaderText + " > scpx" + dataGridView1.Columns[i].HeaderText + "." + dataGridView1.Columns[i].HeaderText + ")) + 1 AS mc" + dataGridView1.Columns[i].HeaderText + ",";
                //sqlmcw = sqlmcw + "  INNER JOIN SCCJ" + sjc + " AS scpx" + dataGridView1.Columns[i].HeaderText + " ON scpx.xh = scpx" + dataGridView1.Columns[i].HeaderText + ".xh ";
                //zdm = zdm + dataGridView1.Columns[i].HeaderText + "名次" + ",";
                sqlpxd[i - (cks - 1)] = "update sccj"+sjc+" Set " + dataGridView1.Columns[j].HeaderText + "=(100-(100*" + zdmc[i - (cks - 1)] + "-50)/" + sccjdt.Rows.Count + ")";
            }

            sqlalterscpxmc = DBUtils.GetAlterTableSql("scpx" + sjc, count, zdpxmc, zdlx, zddx);
            sqlalterscjmc = DBUtils.GetAlterTableSql("SCCJ" + sjc, count, zdmc, zdlx, zddx);
            try
            {
                
                DataTable sqlsccjdt = new DataTable();
                sqlsccjdt = dbclass.GreatDs(sccjsql + sjc + " WHERE (id = NULL)").Tables[0];
                bool sccjflag = false;

                for (int x = 0; x < sqlsccjdt.Columns.Count;x++ )
                {
                    if(zdmc.Contains(sqlsccjdt.Columns[x].ColumnName))
                    {
                        sccjflag = true;
                        break;
                    }
                }

                if (sccjflag == false)
                {
                    dbclass.ExecNonQuerySW(sqlalterscjmc);
                }

                DataTable sqlscpxdt = new DataTable();
                sqlscpxdt = dbclass.GreatDs("select * from scpx" + sjc +" WHERE (id = NULL)").Tables[0];
                bool scpxflag = false;

                for (int x = 0; x < sqlscpxdt.Columns.Count; x++)
                {
                    if (zdpxmc.Contains(sqlscpxdt.Columns[x].ColumnName))
                    {
                        scpxflag = true;
                        break;
                    }
                }

                if (scpxflag == false)
                {
                    dbclass.ExecNonQuerySW(sqlalterscpxmc);
                }

                dbclass.ExecNonQuerySW(sqlscpxmc);
                dbclass.ExecNonQuerySW(sqlscmc);
                dbclass.ExecNonQuerySW(sqlpxd);
                bindDGVZD();
                bindDGVZDJH();
               // bindCBYS();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "p修订值生成成功" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("p修订值生成成功P-value generated successfully", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "p修订值生成失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("p修订值生成失败P-value generation failed" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }  
        }

        private void btnpjs_Click(object sender, EventArgs e)
        {
            DataTable jhbdt = new DataTable();
            jhbdt = dbclass.GreatDs(jhbsql+sjc).Tables[0];
            dataGridView2.DataSource = jhbdt;

            //总数平均分
            DataRow dr = jhbdt.NewRow();
            int countc = jhbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "1";
            avgz[1] = "总数.平均值";
            string sqlavg = "";

            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select AVG(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ"+sjc;

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

            jhbdt.Rows.Add(dr);

            //班级平均分 
            DataTable bjmcdt = new DataTable();
            string bjmcsql = "SELECT DISTINCT bj FROM SCCJ" + sjc;
            bjmcdt = dbclass.GreatDs(bjmcsql).Tables[0];

            for (int i = 0; i < bjmcdt.Rows.Count; i++)
            {
                DataRow drbj = jhbdt.NewRow();
                
                avgz[0] = "1";
                avgz[1] = bjmcdt.Rows[i]["bj"].ToString() + ".平均值";
                sqlavg = "";

                for (int j = 2; j < countc; j++)
                {
                    sqlavg = "select AVG(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ" + sjc + " where bj='" + bjmcdt.Rows[i]["bj"].ToString() + "'";

                    avgz[j] = dbclass.GetOneValue(sqlavg);
                }

                for (int z = 0; z < countc; z++)
                {
                    if (z == 0)
                    {
                        drbj[z] = Convert.ToDecimal(avgz[z]);
                    }
                    else if (z == 1)
                    {
                        drbj[z] = avgz[z];
                    }
                    else
                    {
                        drbj[z] = double.Parse(avgz[z]);
                    }
                }

                jhbdt.Rows.Add(drbj);
            }

            //专业平均分 
            DataTable zymcdt = new DataTable();
            string zymcsql = "SELECT DISTINCT zy FROM SCCJ" + sjc;
            zymcdt = dbclass.GreatDs(zymcsql).Tables[0];

            try
            {
                for (int i = 0; i < zymcdt.Rows.Count; i++)
                {
                    DataRow drzy = jhbdt.NewRow();

                    avgz[0] = "1";
                    avgz[1] = zymcdt.Rows[i]["zy"].ToString() + ".平均值";
                    sqlavg = "";

                    for (int j = 2; j < countc; j++)
                    {
                        sqlavg = "select AVG(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ" + sjc + " where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'";

                        avgz[j] = dbclass.GetOneValue(sqlavg);
                    }

                    for (int z = 0; z < countc; z++)
                    {
                        if (z == 0)
                        {
                            drzy[z] = Convert.ToDecimal(avgz[z]);
                        }
                        else if (z == 1)
                        {
                            drzy[z] = avgz[z];
                        }
                        else
                        {
                            drzy[z] = double.Parse(avgz[z]);
                        }
                    }

                    jhbdt.Rows.Add(drzy);
                }
            }
            catch
            {

            }

            



            try
            {
                dbclass.DoSql(deletejhbsql+sjc);

                dbclass.UpdateAccess(jhbdt, jhbsql+sjc);

                bindDGVZDJH();
                //bindCBYS();
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "平均数生成成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("平均数生成成功Average generated successfully！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "平均数生成失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                
                MessageBox.Show("平均数生成成功Average generated successfully！" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnfc_Click(object sender, EventArgs e)
        {
            DataTable jhbdt = new DataTable();
            jhbdt = dbclass.GreatDs(jhbsql+sjc).Tables[0];
            dataGridView2.DataSource = jhbdt;
            DataRow dr = jhbdt.NewRow();
            int countc = jhbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "2";
            avgz[1] = "总数.方差";
            string sqlavg = "";

            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select VarP(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ"+sjc;

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

            jhbdt.Rows.Add(dr);

            //班级平均分 
            DataTable bjmcdt = new DataTable();
            string bjmcsql = "SELECT DISTINCT bj FROM SCCJ" + sjc;
            bjmcdt = dbclass.GreatDs(bjmcsql).Tables[0];

            for (int i = 0; i < bjmcdt.Rows.Count; i++)
            {
                DataRow drbj = jhbdt.NewRow();

                avgz[0] = "2";
                avgz[1] = bjmcdt.Rows[i]["bj"].ToString() + ".方差";
                sqlavg = "";

                for (int j = 2; j < countc; j++)
                {
                    sqlavg = "select VarP(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ" + sjc + " where bj='" + bjmcdt.Rows[i]["bj"].ToString() + "'";

                    avgz[j] = dbclass.GetOneValue(sqlavg);
                }

                for (int z = 0; z < countc; z++)
                {
                    if (z == 0)
                    {
                        drbj[z] = Convert.ToDecimal(avgz[z]);
                    }
                    else if (z == 1)
                    {
                        drbj[z] = avgz[z];
                    }
                    else
                    {
                        drbj[z] = double.Parse(avgz[z]);
                    }
                }

                jhbdt.Rows.Add(drbj);
            }

            //专业平均分 
            DataTable zymcdt = new DataTable();
            string zymcsql = "SELECT DISTINCT zy FROM SCCJ" + sjc;
            zymcdt = dbclass.GreatDs(zymcsql).Tables[0];

            for (int i = 0; i < zymcdt.Rows.Count; i++)
            {
                DataRow drzy = jhbdt.NewRow();

                avgz[0] = "2";
                avgz[1] = zymcdt.Rows[i]["zy"].ToString() + ".方差";
                sqlavg = "";

                for (int j = 2; j < countc; j++)
                {
                    sqlavg = "select VarP(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ" + sjc + " where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'";

                    avgz[j] = dbclass.GetOneValue(sqlavg);
                }

                for (int z = 0; z < countc; z++)
                {
                    if (z == 0)
                    {
                        drzy[z] = Convert.ToDecimal(avgz[z]);
                    }
                    else if (z == 1)
                    {
                        drzy[z] = avgz[z];
                    }
                    else
                    {
                        drzy[z] = double.Parse(avgz[z]);
                    }
                }

                jhbdt.Rows.Add(drzy);
            }


            try
            {
                dbclass.DoSql(deletejhbsql+sjc);

                dbclass.UpdateAccess(jhbdt, jhbsql+sjc);

                bindDGVZDJH();
               // bindCBYS();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "方差生成成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("方差生成成功Variance generation successful！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "方差生成失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                
                MessageBox.Show("方差生成失败Variance generation failed" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnbzc_Click(object sender, EventArgs e)
        {
            DataTable jhbdt = new DataTable();
            jhbdt = dbclass.GreatDs(jhbsql+sjc).Tables[0];
            dataGridView2.DataSource = jhbdt;
            DataRow dr = jhbdt.NewRow();
            int countc = jhbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "3";
            avgz[1] = "总数.标准差";
            string sqlavg = "";

            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select StDevP(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ"+sjc;

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

            jhbdt.Rows.Add(dr);


            //班级平均分 
            DataTable bjmcdt = new DataTable();
            string bjmcsql = "SELECT DISTINCT bj FROM SCCJ" + sjc;
            bjmcdt = dbclass.GreatDs(bjmcsql).Tables[0];

            for (int i = 0; i < bjmcdt.Rows.Count; i++)
            {
                DataRow drbj = jhbdt.NewRow();

                avgz[0] = "3";
                avgz[1] = bjmcdt.Rows[i]["bj"].ToString() + ".标准差";
                sqlavg = "";

                for (int j = 2; j < countc; j++)
                {
                    sqlavg = "select StDevP(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ" + sjc + " where bj='" + bjmcdt.Rows[i]["bj"].ToString() + "'";

                    avgz[j] = dbclass.GetOneValue(sqlavg);
                }

                for (int z = 0; z < countc; z++)
                {
                    if (z == 0)
                    {
                        drbj[z] = Convert.ToDecimal(avgz[z]);
                    }
                    else if (z == 1)
                    {
                        drbj[z] = avgz[z];
                    }
                    else
                    {
                        drbj[z] = double.Parse(avgz[z]);
                    }
                }

                jhbdt.Rows.Add(drbj);
            }

            //专业平均分 
            DataTable zymcdt = new DataTable();
            string zymcsql = "SELECT DISTINCT zy FROM SCCJ" + sjc;
            zymcdt = dbclass.GreatDs(zymcsql).Tables[0];

            for (int i = 0; i < zymcdt.Rows.Count; i++)
            {
                DataRow drzy = jhbdt.NewRow();

                avgz[0] = "3";
                avgz[1] = zymcdt.Rows[i]["zy"].ToString() + ".标准差";
                sqlavg = "";

                for (int j = 2; j < countc; j++)
                {
                    sqlavg = "select StDevP(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ" + sjc + " where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'";

                    avgz[j] = dbclass.GetOneValue(sqlavg);
                }

                for (int z = 0; z < countc; z++)
                {
                    if (z == 0)
                    {
                        drzy[z] = Convert.ToDecimal(avgz[z]);
                    }
                    else if (z == 1)
                    {
                        drzy[z] = avgz[z];
                    }
                    else
                    {
                        drzy[z] = double.Parse(avgz[z]);
                    }
                }

                jhbdt.Rows.Add(drzy);
            }

            try
            {
                dbclass.DoSql(deletejhbsql+sjc);

                dbclass.UpdateAccess(jhbdt, jhbsql+sjc);

                bindDGVZDJH();
                //bindCBYS();
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "标准差生成成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("标准差生成成功Standard deviation generated successfully！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "标准差生成失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("标准差生成失败Standard deviation generation failed" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnzws_Click(object sender, EventArgs e)
        {
            DataTable jhbdt = new DataTable();
            jhbdt = dbclass.GreatDs(jhbsql+sjc).Tables[0];
            dataGridView2.DataSource = jhbdt;
            DataRow dr = jhbdt.NewRow();
            int countc = jhbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "4";
            avgz[1] = "总数.中位数";
            string sqlavg = "";

            DataTable dttemp = new DataTable();
            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from SCCJ"+sjc;
                dttemp = dbclass.GreatDs(sqlavg).Tables[0];
                double[] tempmed = new double[dttemp.Rows.Count];
                for (int c = 0; c < dttemp.Rows.Count; c++)
                {
                    tempmed[c] = double.Parse(dttemp.Rows[c][0].ToString());
                }
                avgz[j] = Toolimp.Median(tempmed).ToString();
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

            jhbdt.Rows.Add(dr);

            //班级中位数 
            DataTable bjmcdt = new DataTable();
            string bjmcsql = "SELECT DISTINCT bj FROM SCCJ" + sjc;
            bjmcdt = dbclass.GreatDs(bjmcsql).Tables[0];

            for (int i = 0; i < bjmcdt.Rows.Count; i++)
            {
                DataRow drbj = jhbdt.NewRow();

                avgz[0] = "4";
                avgz[1] = bjmcdt.Rows[i]["bj"].ToString() + ".中位数";
                sqlavg = "";

                for (int j = 2; j < countc; j++)
                {
                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from SCCJ" + sjc + " where bj='" + bjmcdt.Rows[i]["bj"].ToString() + "'";
                    dttemp = dbclass.GreatDs(sqlavg).Tables[0];
                    double[] tempmed = new double[dttemp.Rows.Count];
                    for (int c = 0; c < dttemp.Rows.Count; c++)
                    {
                        tempmed[c] = double.Parse(dttemp.Rows[c][0].ToString());
                    }
                    avgz[j] = Toolimp.Median(tempmed).ToString();
                }

                for (int z = 0; z < countc; z++)
                {
                    if (z == 0)
                    {
                        drbj[z] = Convert.ToDecimal(avgz[z]);
                    }
                    else if (z == 1)
                    {
                        drbj[z] = avgz[z];
                    }
                    else
                    {
                        drbj[z] = double.Parse(avgz[z]);
                    }
                }

                jhbdt.Rows.Add(drbj);
            }

            //专业中位数
            DataTable zymcdt = new DataTable();
            string zymcsql = "SELECT DISTINCT zy FROM SCCJ" + sjc;
            zymcdt = dbclass.GreatDs(zymcsql).Tables[0];

            for (int i = 0; i < zymcdt.Rows.Count; i++)
            {
                DataRow drzy = jhbdt.NewRow();

                avgz[0] = "4";
                avgz[1] = zymcdt.Rows[i]["zy"].ToString() + ".中位数";
                sqlavg = "";

                for (int j = 2; j < countc; j++)
                {

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from SCCJ" + sjc + " where zy='" + zymcdt.Rows[i]["zy"].ToString() + "'";
                    dttemp = dbclass.GreatDs(sqlavg).Tables[0];
                    double[] tempmed = new double[dttemp.Rows.Count];
                    for (int c = 0; c < dttemp.Rows.Count; c++)
                    {
                        tempmed[c] = double.Parse(dttemp.Rows[c][0].ToString());
                    }
                    avgz[j] = Toolimp.Median(tempmed).ToString();
                }

                for (int z = 0; z < countc; z++)
                {
                    if (z == 0)
                    {
                        drzy[z] = Convert.ToDecimal(avgz[z]);
                    }
                    else if (z == 1)
                    {
                        drzy[z] = avgz[z];
                    }
                    else
                    {
                        drzy[z] = double.Parse(avgz[z]);
                    }
                }

                jhbdt.Rows.Add(drzy);
            }

            try
            {
                dbclass.DoSql(deletejhbsql+sjc);

                dbclass.UpdateAccess(jhbdt, jhbsql+sjc);

                bindDGVZDJH();
               // bindCBYS();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "中位数生成成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("中位数生成成功Median generated successfully！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "中位数生成失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                
                MessageBox.Show("中位数生成失败Median generation failed!" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnmf_Click(object sender, EventArgs e)
        {
            DataTable jhbdt = new DataTable();
            jhbdt = dbclass.GreatDs(jhbsql+sjc).Tables[0];
            dataGridView2.DataSource = jhbdt;
            DataRow dr = jhbdt.NewRow();
            int countc = jhbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "5";
            avgz[1] = "满分";

            //知识个数
            string sqlzscount = "SELECT  COUNT(*) FROM FXCLASS WHERE(PCLASSID = 10001) AND(SJC = '" + sjc + "')";
            int zscount = int.Parse(dbclass.GetOneValue(sqlzscount));

            //技能个数
            string sqljncount = "SELECT  COUNT(*) FROM FXCLASS WHERE(PCLASSID = 10002) AND(SJC = '" + sjc + "')";
            int jncount = int.Parse(dbclass.GetOneValue(sqljncount));
            //能力个数
            string sqlnlcount = "SELECT  COUNT(*) FROM FXCLASS WHERE(PCLASSID = 10003) AND(SJC = '" + sjc + "')";
            int nlcount = int.Parse(dbclass.GetOneValue(sqlnlcount));

            double temp = 0;
            DataTable dtysff = new DataTable();
            dtysff = dbclass.GreatDs(ysffbsql+sjc).Tables[0];
            DataTable dtxsb = new DataTable();
            dtxsb = dbclass.GreatDs(xsbsql+sjc).Tables[0];
            for (int j = 3; j < countc; j++)
            {
                temp = 0;

                for (int c = 0; c < dtysff.Rows.Count; c++)
                {
                    temp = temp + double.Parse(dtysff.Rows[c][2].ToString()) * double.Parse(dtxsb.Rows[c][j - 1].ToString());
                }

                avgz[j] = temp.ToString();
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

            double tempzs = 0;
            for (int t = 0; t < zscount; t++)
            {
                tempzs = tempzs + double.Parse(dr[6 + t].ToString());
            }
            if (double.Parse(dr[3].ToString()) < tempzs)
            {
                dr[3] = Math.Round(tempzs, 0);
            }
            else
            {
                dr[3] = Math.Round(double.Parse(dr[3].ToString()), 0);
            }

            tempzs = 0;
            for (int t = 0; t < jncount; t++)
            {
                tempzs = tempzs + double.Parse(dr[6 + zscount + t].ToString());
            }
            if (double.Parse(dr[4].ToString()) < tempzs)
            {
                dr[4] = Math.Round(tempzs, 0);
            }
            else
            {
                dr[4] = Math.Round(double.Parse(dr[4].ToString()), 0);
            }

            tempzs = 0;
            for (int t = 0; t < nlcount; t++)
            {
                tempzs = tempzs + double.Parse(dr[6 + zscount + jncount + t].ToString());
            }
            if (double.Parse(dr[5].ToString()) < tempzs)
            {
                dr[5] = Math.Round(tempzs, 0);
            }
            else
            {
                dr[5] = Math.Round(double.Parse(dr[5].ToString()), 0);
            }

            jhbdt.Rows.Add(dr);


            try
            {
                if (lbgs.Text == "")
                {
                    MessageBox.Show("满分生成失败，请输入总成绩计算公式！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dbclass.DoSql(deletejhbsql+sjc);

                dbclass.UpdateAccess(jhbdt, jhbsql+sjc);

                string sqlupzf = "update jhb"+sjc+" Set 总成绩 =(" + lbgs.Text + ") where 分项='满分'";
                dbclass.DoSql(sqlupzf);
                bindDGVZDJH();
                //bindCBYS();
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "满分生成成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("满分生成成功Full score generated successfully！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "满分生成失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("满分生成失败Full score generation failed!" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnjhcsh_Click(object sender, EventArgs e)
        {
            try
            {
                dbclass.DoSql(deletejhbsql+sjc);
                bindDGVZDJH();
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "聚合表初始化成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("初始化成功The initialization is successful", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "聚合表初始化失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("初始化失败Initialization failed" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }   
        }

        private void btnjhjs_Click(object sender, EventArgs e)
        {
            try
            {
                btnpjs_Click(sender, e);
                btnfc_Click(sender, e);
                btnbzc_Click(sender, e);
                btnzws_Click(sender, e);
                btnmf_Click(sender, e);

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "聚合计算成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("聚合计算成功Aggregation calculation successful！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "聚合计算失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("聚合计算失败错误Aggregation calculation failure error：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           
        }

        private void btngfz_Click(object sender, EventArgs e)
        {

        }

        private void SjjsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult r = MessageBox.Show("退出本页面Exit this page?", "操作提示Tips", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r != DialogResult.OK)
                {
                    e.Cancel = true;
                }
                else
                {

                    fm.labelsjc.Text = this.labelsjc.Text;
                    this.Hide();
                  //  fm.ShowDialog();
                }
            }
        }

        private void btndfz_Click(object sender, EventArgs e)
        {

        }
    }
}
