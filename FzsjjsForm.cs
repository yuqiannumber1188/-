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
    public partial class FzsjjsForm : Form
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

        public FzsjjsForm(MainForm _fm)
        {
            InitializeComponent();

            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
            sjc = labelsjc.Text;

            bindDGVZD();
            bindDGVZDJH();
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
            tbpks.Text = (10 + (xsbdt.Columns.Count - 2) + 4 + 1).ToString();

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

        private void btnbzcjs_Click(object sender, EventArgs e)
        {
            int cks = 0;
            int cjs = 0;
            int cpks = 0;
            int cpjs = 0;
            float a = 0;
            float b = 0;
            try
            {
                cks = Convert.ToInt32(tbks.Text);
                cjs = Convert.ToInt32(tbjs.Text);
                cpks = Convert.ToInt32(tbpks.Text);
                cpjs = Convert.ToInt32(tbpjs.Text);
                a = float.Parse(tbpj.Text);
                b = float.Parse(tbbc.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("类型转换失败Type conversion failed" + ex.ToString(), "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int count = cjs - cks + 1;
            int pcount = cpjs - cpks + 1;
            if (count <= 0 || pcount <= 0 || count != pcount)
            {
                MessageBox.Show("结束值应大于开始值且两个值之差相等！The end value should be greater than the start value and the difference between the two values should be equal!", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            string[] sqlpbzc = new string[count];
            string[] zdmc = new string[count];
            string[] zdlx = new string[count];
            string[] zddx = new string[count];

            DataTable sccjdt = new DataTable();
            sccjdt = dbclass.GreatDs(sccjsql+sjc).Tables[0];
            string jzsql = "";
            string jzstr = "";
            string bzcstr = "";
            string bzcsql = "";
            for (int i = cks - 1, j = cpks - 1; i < cjs; i++, j++)
            {
                jzsql = "select " + dataGridView1.Columns[i].HeaderText + " from jhb"+sjc+" where 分项='总数.平均值'";
                jzstr = dbclass.GetOneValue(jzsql);
                bzcsql = "select " + dataGridView1.Columns[i].HeaderText + " from jhb" + sjc + " where 分项='总数.标准差'";
                bzcstr = dbclass.GetOneValue(bzcsql);
                sqlpbzc[i - (cks - 1)] = "update sccj"+sjc+" Set " + dataGridView1.Columns[j].HeaderText + "=(" + a + "+" + b + "*((" + dataGridView1.Columns[i].HeaderText + "-" + jzstr + ")/" + bzcstr + "))";
            }
            try
            {
                dbclass.ExecNonQuerySW(sqlpbzc);
                bindDGVZD();
                bindDGVZDJH();
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "标准分生成成功" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                //  bindCBYS();
                MessageBox.Show("标准分生成成功Standard scores are generated", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "标准分生成失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                
                MessageBox.Show("标准分生成失败Failed to generate standard scores" + ex.ToString(), "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            } 
        }

        private void btnzdjf_Click(object sender, EventArgs e)
        {
            try
            {
               

                //获取总人数
                string sqlzrs = "select count(*) from sccj"+sjc;
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
                for (int i = 0; i < 8; i++)
                {
                    if (ca == tempZrs)
                    {
                        break;
                    }
                    
                    fwdt[i] = new DataTable();
                    // 真实值取值范围
                    sqlmaxfw[i] = "SELECT max(总成绩) from SCCJ" + sjc + " where 总成绩名次 > CEILING(" + tempZrs + " * " + blfws[i].Minqz + ") and 总成绩名次 <= CEILING(" + tempZrs + " * " + blfws[i].Maxqz+")";
                    sqlminfw[i] = "SELECT min(总成绩) from SCCJ" + sjc + " where 总成绩名次 > CEILING(" + tempZrs + " * " + blfws[i].Minqz + ") and 总成绩名次 <= CEILING(" + tempZrs + " * " + blfws[i].Maxqz+")";
                  

                    if (dbclass.GetOneValue(sqlminfw[i]) != null && !"".Equals(dbclass.GetOneValue(sqlminfw[i])))
                    {
                        zsfws[i] = new FWClass<double>(double.Parse(dbclass.GetOneValue(sqlminfw[i])) / mfz * 100, double.Parse(dbclass.GetOneValue(sqlmaxfw[i])) / mfz * 100);
                        sqlfw[i] = "SELECT id,总成绩 from SCCJ" + sjc + " where 总成绩名次 > CEILING(" + tempZrs + " * " + blfws[i].Minqz + ") and 总成绩名次 <= CEILING(" + tempZrs + " * " + blfws[i].Maxqz + ")";
                        fwdt[i] = dbclass.GreatDs(sqlfw[i]).Tables[0];
                        for (int j = 0; j < fwdt[i].Rows.Count; j++)
                        {
                            DJFClass djfclass = new DJFClass();
                            djfclass.Stuid = int.Parse(fwdt[i].Rows[j]["id"].ToString());
                            djfclass.Zscj = double.Parse(fwdt[i].Rows[j]["总成绩"].ToString()) / mfz * 100;
                            djfclass.Maxdjffw = qzfws[i].Maxqz;
                            djfclass.Mindjffw = qzfws[i].Minqz;
                            djfclass.Maxzsfw = zsfws[i].Maxqz;
                            djfclass.Minzsfw = zsfws[i].Minqz;

                            sqlxgdjf[ca] = "update SCCJ" + sjc + " Set 总成绩等级分 = " + djfclass.Jsdjf() + " where id = " + djfclass.Stuid;

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

                dbclass.ExecNonQuerySW(sqlxgdjf);
                dataGridView1.DataSource = dbclass.GreatDs(sccjsql+sjc).Tables[0];
                bindDGVZD();


                string sqlzcjdjf = "alter table jhb" + sjc + " add 总成绩等级分 decimal(18, 3)";
                dbclass.DoSql(sqlzcjdjf);

                string[] tempzcj = new string[5];
                
                string[] zcjdjf = new string[5];
                tempzcj[0] = dbclass.GetOneValue("select AVG(总成绩等级分) from SCCJ"+sjc);
                zcjdjf[0] = "UPDATE jhb" + sjc + " set 总成绩等级分 = " + tempzcj[0] + " where 分项='总数.平均值'";
                tempzcj[1] = dbclass.GetOneValue("select VarP(总成绩等级分) from SCCJ" + sjc);
                zcjdjf[1] = "UPDATE jhb" + sjc + " set 总成绩等级分 = " + tempzcj[1] + " where 分项='总数.方差'";
                tempzcj[2] = dbclass.GetOneValue("select StDevP(总成绩等级分) from SCCJ" + sjc);
                zcjdjf[2] = "UPDATE jhb" + sjc + " set 总成绩等级分 = " + tempzcj[2] + " where 分项='总数.标准差'";


                string sqlavg = "select 总成绩等级分 from SCCJ" + sjc;
                DataTable dttemp = dbclass.GreatDs(sqlavg).Tables[0];
                double[] tempmed = new double[dttemp.Rows.Count];
                for (int c = 0; c < dttemp.Rows.Count; c++)
                {
                    tempmed[c] = double.Parse(dttemp.Rows[c][0].ToString());
                }
                tempzcj[3] = Toolimp.Median(tempmed).ToString();
                zcjdjf[3] = "UPDATE jhb" + sjc + " set 总成绩等级分 = " + tempzcj[3] + " where 分项='总数.中位数'";
                tempzcj[4] = dbclass.GetOneValue("select AVG(总成绩等级分) from SCCJ" + sjc);
                zcjdjf[4] = "UPDATE jhb" + sjc + " set 总成绩等级分 = " + 100 + " where 分项='满分'";
                dbclass.ExecNonQuerySW(zcjdjf);
                bindDGVZDJH();
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "总成绩等级分生成成功" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("总成绩等级分生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "总成绩等级分生成失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                
                MessageBox.Show("总成绩等级分生成失败The overall grade score generation failed！" + ex.ToString(), "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void FzsjjsForm_FormClosing(object sender, FormClosingEventArgs e)
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
                //    fm.ShowDialog();
                }
            }
        }

        private void btnpjdfl_Click(object sender, EventArgs e)
        {
            string temp1 = "";
            string temp2 = "";

            double dotemp1 = 0;
            double dotemp2 = 0;
            DataTable jhbdt = new DataTable();
            jhbdt = dbclass.GreatDs(jhbsql + sjc).Tables[0];
            dataGridView2.DataSource = jhbdt;
            DataRow dr = jhbdt.NewRow();
            int countc = jhbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "6";
            avgz[1] = "总数.平均值得分率";
            string sqlavg = "";

            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='总数.平均值'";

                temp1 = dbclass.GetOneValue(sqlavg);
                dotemp1 = double.Parse(temp1);

                sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='满分'";

                temp2 = dbclass.GetOneValue(sqlavg);
                dotemp2 = Toolimp.Round(double.Parse(temp2),0);

                avgz[j] = ((dotemp1 / dotemp2)*100).ToString();
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

            //班级平均得分率 
            DataTable bjmcdt = new DataTable();
            string bjmcsql = "SELECT DISTINCT bj FROM SCCJ" + sjc;
            bjmcdt = dbclass.GreatDs(bjmcsql).Tables[0];

            for (int i = 0; i < bjmcdt.Rows.Count; i++)
            {
                DataRow drbj = jhbdt.NewRow();

                avgz[0] = "6";
                avgz[1] = bjmcdt.Rows[i]["bj"].ToString() + ".平均值得分率";
                sqlavg = "";

                for (int j = 2; j < countc-1; j++)
                {

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + bjmcdt.Rows[i]["bj"].ToString() + ".平均值'";

                    temp1 = dbclass.GetOneValue(sqlavg);
                    dotemp1 = double.Parse(temp1);

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='满分'";

                    temp2 = dbclass.GetOneValue(sqlavg);
                    dotemp2 = Toolimp.Round(double.Parse(temp2), 0);

                    avgz[j] = ((dotemp1 / dotemp2) * 100).ToString();
                }

                for (int z = 0; z < countc-1; z++)
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

            //专业平均得分率
            DataTable zymcdt = new DataTable();
            string zymcsql = "SELECT DISTINCT zy FROM SCCJ" + sjc;
            zymcdt = dbclass.GreatDs(zymcsql).Tables[0];

            for (int i = 0; i < zymcdt.Rows.Count; i++)
            {
                DataRow drzy = jhbdt.NewRow();

                avgz[0] = "6";
                avgz[1] = zymcdt.Rows[i]["zy"].ToString() + ".平均值得分率";
                sqlavg = "";

                for (int j = 2; j < countc-1; j++)
                {
                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + zymcdt.Rows[i]["zy"].ToString() + ".平均值'";

                    temp1 = dbclass.GetOneValue(sqlavg);
                    dotemp1 = double.Parse(temp1);

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='满分'";

                    temp2 = dbclass.GetOneValue(sqlavg);
                    dotemp2 = Toolimp.Round(double.Parse(temp2), 0);

                    avgz[j] = ((dotemp1 / dotemp2) * 100).ToString();
                }

                for (int z = 0; z < countc-1; z++)
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
                dbclass.DoSql(deletejhbsql + sjc);

                dbclass.UpdateAccess(jhbdt, jhbsql + sjc);

                bindDGVZDJH();
                
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "平均得分率生成成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("平均得分率生成成功The average score rate was generated successfully！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "平均得分率生成失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("平均得分率生成失败Average Score Rate generation failed" + ex.ToString(), "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnzpcz_Click(object sender, EventArgs e)
        {
            string temp1 = "";
            string temp2 = "";

            double dotemp1 = 0;
            double dotemp2 = 0;
            DataTable jhbdt = new DataTable();
            jhbdt = dbclass.GreatDs(jhbsql + sjc).Tables[0];
            dataGridView2.DataSource = jhbdt;
            DataRow dr = jhbdt.NewRow();
            int countc = jhbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "9";
            avgz[1] = "总数.中平差值";
            string sqlavg = "";

            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='总数.中位数得分率'";

                temp1 = dbclass.GetOneValue(sqlavg);
                dotemp1 = double.Parse(temp1);

                sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='总数.平均值得分率'";

                temp2 = dbclass.GetOneValue(sqlavg);
                dotemp2 = double.Parse(temp2);

                avgz[j] = (dotemp1 - dotemp2).ToString();
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

            //班级中平差值 
            DataTable bjmcdt = new DataTable();
            string bjmcsql = "SELECT DISTINCT bj FROM SCCJ" + sjc;
            bjmcdt = dbclass.GreatDs(bjmcsql).Tables[0];

            for (int i = 0; i < bjmcdt.Rows.Count; i++)
            {
                DataRow drbj = jhbdt.NewRow();

                avgz[0] = "9";
                avgz[1] = bjmcdt.Rows[i]["bj"].ToString() + ".中平差值";
                sqlavg = "";

                for (int j = 2; j < countc-1; j++)
                {

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + bjmcdt.Rows[i]["bj"].ToString() + ".中位数得分率'";

                    temp1 = dbclass.GetOneValue(sqlavg);
                    dotemp1 = double.Parse(temp1);

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + bjmcdt.Rows[i]["bj"].ToString() + ".平均值得分率'";

                    temp2 = dbclass.GetOneValue(sqlavg);
                    dotemp2 = double.Parse(temp2);

                    avgz[j] = (dotemp1 - dotemp2).ToString();
                }

                for (int z = 0; z < countc-1; z++)
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

            //专业分化程度
            DataTable zymcdt = new DataTable();
            string zymcsql = "SELECT DISTINCT zy FROM SCCJ" + sjc;
            zymcdt = dbclass.GreatDs(zymcsql).Tables[0];

            for (int i = 0; i < zymcdt.Rows.Count; i++)
            {
                DataRow drzy = jhbdt.NewRow();

                avgz[0] = "9";
                avgz[1] = zymcdt.Rows[i]["zy"].ToString() + ".中平差值";
                sqlavg = "";

                for (int j = 2; j < countc-1; j++)
                {

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + zymcdt.Rows[i]["zy"].ToString() + ".中位数得分率'";

                    temp1 = dbclass.GetOneValue(sqlavg);
                    dotemp1 = double.Parse(temp1);

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + zymcdt.Rows[i]["zy"].ToString() + ".平均值得分率'";

                    temp2 = dbclass.GetOneValue(sqlavg);
                    dotemp2 = double.Parse(temp2);

                    avgz[j] = (dotemp1 - dotemp2).ToString();
                }

                for (int z = 0; z < countc-1; z++)
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
                dbclass.DoSql(deletejhbsql + sjc);

                dbclass.UpdateAccess(jhbdt, jhbsql + sjc);

                bindDGVZDJH();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "中平差值生成成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("中平差值生成成功The adjustment is generated successfully！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "中平差值生成失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("中平差值生成失败Failed to generate the medium adjustment" + ex.ToString(), "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnzwsdfl_Click(object sender, EventArgs e)
        {
            string temp1 = "";
            string temp2 = "";

            double dotemp1 = 0;
            double dotemp2 = 0;
            DataTable jhbdt = new DataTable();
            jhbdt = dbclass.GreatDs(jhbsql + sjc).Tables[0];
            dataGridView2.DataSource = jhbdt;
            DataRow dr = jhbdt.NewRow();
            int countc = jhbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "7";
            avgz[1] = "总数.中位数得分率";
            string sqlavg = "";

            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='总数.中位数'";

                temp1 = dbclass.GetOneValue(sqlavg);
                dotemp1 = double.Parse(temp1);

                sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='满分'";

                temp2 = dbclass.GetOneValue(sqlavg);
                dotemp2 = Toolimp.Round(double.Parse(temp2),0);

                avgz[j] = ((dotemp1 / dotemp2)*100).ToString();
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

            //班级中位数得分率 
            DataTable bjmcdt = new DataTable();
            string bjmcsql = "SELECT DISTINCT bj FROM SCCJ" + sjc;
            bjmcdt = dbclass.GreatDs(bjmcsql).Tables[0];

            for (int i = 0; i < bjmcdt.Rows.Count; i++)
            {
                DataRow drbj = jhbdt.NewRow();

                avgz[0] = "7";
                avgz[1] = bjmcdt.Rows[i]["bj"].ToString() + ".中位数得分率";
                sqlavg = "";

                for (int j = 2; j < countc-1; j++)
                {

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + bjmcdt.Rows[i]["bj"].ToString() + ".中位数'";

                    temp1 = dbclass.GetOneValue(sqlavg);
                    dotemp1 = double.Parse(temp1);

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='满分'";

                    temp2 = dbclass.GetOneValue(sqlavg);
                    dotemp2 = Toolimp.Round(double.Parse(temp2), 0);

                    avgz[j] = ((dotemp1 / dotemp2) * 100).ToString();
                }

                for (int z = 0; z < countc-1; z++)
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

            //专业中位数得分率
            DataTable zymcdt = new DataTable();
            string zymcsql = "SELECT DISTINCT zy FROM SCCJ" + sjc;
            zymcdt = dbclass.GreatDs(zymcsql).Tables[0];

            for (int i = 0; i < zymcdt.Rows.Count; i++)
            {
                DataRow drzy = jhbdt.NewRow();

                avgz[0] = "7";
                avgz[1] = zymcdt.Rows[i]["zy"].ToString() + ".中位数得分率";
                sqlavg = "";

                for (int j = 2; j < countc-1; j++)
                {
                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + zymcdt.Rows[i]["zy"].ToString() + ".中位数'";

                    temp1 = dbclass.GetOneValue(sqlavg);
                    dotemp1 = double.Parse(temp1);

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='满分'";

                    temp2 = dbclass.GetOneValue(sqlavg);
                    dotemp2 = Toolimp.Round(double.Parse(temp2), 0);

                    avgz[j] = ((dotemp1 / dotemp2) * 100).ToString();
                }

                for (int z = 0; z < countc-1; z++)
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
                dbclass.DoSql(deletejhbsql + sjc);

                dbclass.UpdateAccess(jhbdt, jhbsql + sjc);

                bindDGVZDJH();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "中位数得分率生成成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("中位数得分率生成成功The median score rate was generated successfully！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "中位数得分率生成失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("中位数得分率生成失败The median score rate failed to be generated" + ex.ToString(), "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnfhcd_Click(object sender, EventArgs e)
        {
            string temp1 = "";
            string temp2 = "";

            double dotemp1 = 0;
            double dotemp2 = 0;
            DataTable jhbdt = new DataTable();
            jhbdt = dbclass.GreatDs(jhbsql + sjc).Tables[0];
            dataGridView2.DataSource = jhbdt;
            DataRow dr = jhbdt.NewRow();
            int countc = jhbdt.Columns.Count;
            string[] avgz = new string[countc];
            avgz[0] = "8";
            avgz[1] = "总数.分化程度";
            string sqlavg = "";

            for (int j = 2; j < countc; j++)
            {
                sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='总数.标准差'";

                temp1 = dbclass.GetOneValue(sqlavg);
                dotemp1 = double.Parse(temp1);

                sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='总数.平均值'";

                temp2 = dbclass.GetOneValue(sqlavg);
                dotemp2 = double.Parse(temp2);

                avgz[j] = ((dotemp1 / dotemp2)*100).ToString();
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

            //班级分化程度 
            DataTable bjmcdt = new DataTable();
            string bjmcsql = "SELECT DISTINCT bj FROM SCCJ" + sjc;
            bjmcdt = dbclass.GreatDs(bjmcsql).Tables[0];

            for (int i = 0; i < bjmcdt.Rows.Count; i++)
            {
                DataRow drbj = jhbdt.NewRow();

                avgz[0] = "8";
                avgz[1] = bjmcdt.Rows[i]["bj"].ToString() + ".分化程度";
                sqlavg = "";

                for (int j = 2; j < countc-1; j++)
                {

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + bjmcdt.Rows[i]["bj"].ToString() + ".标准差'";

                    temp1 = dbclass.GetOneValue(sqlavg);
                    dotemp1 = double.Parse(temp1);

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + bjmcdt.Rows[i]["bj"].ToString() + ".平均值'";

                    temp2 = dbclass.GetOneValue(sqlavg);
                    dotemp2 = double.Parse(temp2);

                    avgz[j] = ((dotemp1 / dotemp2) * 100).ToString();
                }

                for (int z = 0; z < countc-1; z++)
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

            //专业分化程度
            DataTable zymcdt = new DataTable();
            string zymcsql = "SELECT DISTINCT zy FROM SCCJ" + sjc;
            zymcdt = dbclass.GreatDs(zymcsql).Tables[0];

            for (int i = 0; i < zymcdt.Rows.Count; i++)
            {
                DataRow drzy = jhbdt.NewRow();

                avgz[0] = "8";
                avgz[1] = zymcdt.Rows[i]["zy"].ToString() + ".中位数得分率";
                sqlavg = "";

                for (int j = 2; j < countc-1; j++)
                {
                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + zymcdt.Rows[i]["zy"].ToString() + ".标准差'";

                    temp1 = dbclass.GetOneValue(sqlavg);
                    dotemp1 = double.Parse(temp1);

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='" + zymcdt.Rows[i]["zy"].ToString() + ".平均值'";

                    temp2 = dbclass.GetOneValue(sqlavg);
                    dotemp2 = double.Parse(temp2);

                    avgz[j] = ((dotemp1 / dotemp2) * 100).ToString();
                }

                for (int z = 0; z < countc-1; z++)
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
                dbclass.DoSql(deletejhbsql + sjc);

                dbclass.UpdateAccess(jhbdt, jhbsql + sjc);

                bindDGVZDJH();

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "分化程度生成成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("分化程度生成成功The degree of differentiation was generated successfully！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "分化程度生成失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("分化程度生成失败Degree of differentiation generation failed" + ex.ToString(), "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btndj_Click(object sender, EventArgs e)
        {
            DataTable jhbdt = new DataTable();
            jhbdt = dbclass.GreatDs(jhbsql + sjc).Tables[0];
            dataGridView2.DataSource = jhbdt;

            for(int x=0;x < 4;x++)
            {
                DataRow dr = jhbdt.NewRow();
                int countc = jhbdt.Columns.Count;
                string[] avgz = new string[countc];
                avgz[0] = (10+x).ToString();
                char tempchar =(char)('A'+x);
                avgz[1] = tempchar.ToString();
                string sqlavg = "";
                string sqlwhere = "";
                string temp2 = "";
                double dotemp2 = 0;

                for (int j = 2; j < countc; j++)
                {

                    sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='满分'";

                    temp2 = dbclass.GetOneValue(sqlavg);
                    dotemp2 = Toolimp.Round(double.Parse(temp2),0);
                    
                    if (avgz[1].Equals("A"))
                    {
                        sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " >= " + dotemp2*0.85;
                    }
                    else if (avgz[1].Equals("B"))
                    {
                        sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " >= " + dotemp2 * 0.70 + " AND " + dataGridView2.Columns[j].HeaderText + " < " + dotemp2*0.85;
                    }
                    else if (avgz[1].Equals("C"))
                    {
                        sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " >= " + dotemp2 * 0.60 + " AND " + dataGridView2.Columns[j].HeaderText + " < " + dotemp2 * 0.70;
                    }
                    else if (avgz[1].Equals("D"))
                    {
                        sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " < " + dotemp2*0.60;
                    }

                    sqlavg = "select COUNT(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ" + sjc + sqlwhere;

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
                        dr[i] = "总数."+avgz[i];
                    }
                    else
                    {
                        dr[i] = double.Parse(avgz[i]);
                    }
                }

                jhbdt.Rows.Add(dr);
            }

            //班级
            DataTable bjmcdt = new DataTable();
            string bjmcsql = "SELECT DISTINCT bj FROM SCCJ" + sjc;
            bjmcdt = dbclass.GreatDs(bjmcsql).Tables[0];

            for (int i = 0; i < bjmcdt.Rows.Count; i++)
            {
                for (int x = 0; x < 4; x++)
                {
                    DataRow dr = jhbdt.NewRow();
                    int countc = jhbdt.Columns.Count;
                    string[] avgz = new string[countc];
                    avgz[0] = (10 + x).ToString();
                    char tempchar = (char)('A' + x);
                    avgz[1] = tempchar.ToString();
                    string sqlavg = "";
                    string sqlwhere = "";
                    string temp2 = "";
                    double dotemp2 = 0;

                    for (int j = 2; j < countc-1; j++)
                    {

                        sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='满分'";

                        temp2 = dbclass.GetOneValue(sqlavg);
                        dotemp2 = Toolimp.Round(double.Parse(temp2), 0);

                        if (avgz[1].Equals("A"))
                        {
                            sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " >= " + dotemp2 * 0.85 + " AND bj = '" + bjmcdt.Rows[i]["bj"].ToString() + "'";
                        }
                        else if (avgz[1].Equals("B"))
                        {
                            sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " >= " + dotemp2 * 0.70 + " AND " + dataGridView2.Columns[j].HeaderText + " < " + dotemp2 * 0.85 + " AND bj = '" + bjmcdt.Rows[i]["bj"].ToString() + "'";
                        }
                        else if (avgz[1].Equals("C"))
                        {
                            sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " >= " + dotemp2 * 0.60 + " AND " + dataGridView2.Columns[j].HeaderText + " < " + dotemp2 * 0.70 + " AND bj = '" + bjmcdt.Rows[i]["bj"].ToString() + "'";
                        }
                        else if (avgz[1].Equals("D"))
                        {
                            sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " < " + dotemp2 * 0.60 + " AND bj = '" + bjmcdt.Rows[i]["bj"].ToString() + "'";
                        }

                        sqlavg = "select COUNT(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ" + sjc + sqlwhere;

                        avgz[j] = dbclass.GetOneValue(sqlavg);
                    }

                    for (int z = 0; z < countc-1; z++)
                    {
                        if (z == 0)
                        {
                            dr[z] = Convert.ToDecimal(avgz[z]);
                        }
                        else if (z == 1)
                        {
                            dr[z] = bjmcdt.Rows[i]["bj"].ToString() + "." + avgz[z];
                        }
                        else
                        {
                            dr[z] = double.Parse(avgz[z]);
                        }
                    }

                    jhbdt.Rows.Add(dr);
                }
            }


            //专业
            DataTable zymcdt = new DataTable();
            string zymcsql = "SELECT DISTINCT zy FROM SCCJ" + sjc;
            zymcdt = dbclass.GreatDs(zymcsql).Tables[0];

            for (int i = 0; i < zymcdt.Rows.Count; i++)
            {
                for (int x = 0; x < 4; x++)
                {
                    DataRow dr = jhbdt.NewRow();
                    int countc = jhbdt.Columns.Count;
                    string[] avgz = new string[countc];
                    avgz[0] = (10 + x).ToString();
                    char tempchar = (char)('A' + x);
                    avgz[1] = tempchar.ToString();
                    string sqlavg = "";
                    string sqlwhere = "";
                    string temp2 = "";
                    double dotemp2 = 0;

                    for (int j = 2; j < countc-1; j++)
                    {

                        sqlavg = "select " + dataGridView2.Columns[j].HeaderText + " from jhb" + sjc + " where 分项='满分'";

                        temp2 = dbclass.GetOneValue(sqlavg);
                        dotemp2 = Toolimp.Round(double.Parse(temp2), 0);

                        if (avgz[1].Equals("A"))
                        {
                            sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " >= " + dotemp2 * 0.85 + " AND zy = '" + zymcdt.Rows[i]["zy"].ToString() + "'";
                        }
                        else if (avgz[1].Equals("B"))
                        {
                            sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " >= " + dotemp2 * 0.70 + " AND " + dataGridView2.Columns[j].HeaderText + " < " + dotemp2 * 0.85 + " AND zy = '" + zymcdt.Rows[i]["zy"].ToString() + "'";
                        }
                        else if (avgz[1].Equals("C"))
                        {
                            sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " >= " + dotemp2 * 0.60 + " AND " + dataGridView2.Columns[j].HeaderText + " < " + dotemp2 * 0.70 + " AND zy = '" + zymcdt.Rows[i]["zy"].ToString() + "'";
                        }
                        else if (avgz[1].Equals("D"))
                        {
                            sqlwhere = " where " + dataGridView2.Columns[j].HeaderText + " < " + dotemp2 * 0.60 + " AND zy = '" + zymcdt.Rows[i]["zy"].ToString() + "'";
                        }

                        sqlavg = "select COUNT(" + dataGridView2.Columns[j].HeaderText + ") from SCCJ" + sjc + sqlwhere;

                        avgz[j] = dbclass.GetOneValue(sqlavg);
                    }

                    for (int z = 0; z < countc-1; z++)
                    {
                        if (z == 0)
                        {
                            dr[z] = Convert.ToDecimal(avgz[z]);
                        }
                        else if (z == 1)
                        {
                            dr[z] = zymcdt.Rows[i]["zy"].ToString() + "." + avgz[z];
                        }
                        else
                        {
                            dr[z] = double.Parse(avgz[z]);
                        }
                    }

                    jhbdt.Rows.Add(dr);
                }
            }
           

            try
            {
                dbclass.DoSql(deletejhbsql + sjc);

                dbclass.UpdateAccess(jhbdt, jhbsql + sjc);

                bindDGVZDJH();
              
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "等级生成成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("等级生成成功The level is generated！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "等级生成失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("等级生成失败Level generation failed" + ex.ToString(), "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnjhjs_Click(object sender, EventArgs e)
        {

            try
            {
                 btnpjdfl_Click(sender, e);
                 btnzwsdfl_Click(sender, e);
                 btnfhcd_Click(sender, e);
                 btnzpcz_Click(sender, e);
                 btndj_Click(sender, e);
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "聚合计算成功" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("聚合计算成功The aggregate calculation succeeded！", "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "聚合计算失败" + "','" + "jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("聚合计算失败Aggregate computation failed：" + ex.Message, "提示prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           
        }
       
    }
}
