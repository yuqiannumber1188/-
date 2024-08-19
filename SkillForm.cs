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
using System.Text.RegularExpressions;

namespace cjpc
{
    public partial class SkillForm : Form
    {
        DBClass dbclass = new DBClass();
        string xsbsql = "select * from xsb";
        string sccjsql = "select * from SCCJ";
        string sccjzdsql = "select * from zdgxb where bmc = 'sccj' order by id asc";
        string deletesccjsql = "delete from zdgxb where bmc = 'sccj'";

        string sjc;
        MainForm fm;

        int skillcount = 0;
        DataTable dtskill = new DataTable();

        System.Windows.Forms.TextBox[,] tbA1 = new System.Windows.Forms.TextBox[9, 9];
        System.Windows.Forms.Label[] lbAT = new System.Windows.Forms.Label[9];
        System.Windows.Forms.Label[] lbAL = new System.Windows.Forms.Label[9];
        System.Windows.Forms.Label[] lbWA = new System.Windows.Forms.Label[9];


        public SkillForm(MainForm _fm)
        {
            InitializeComponent();

            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
            sjc = labelsjc.Text;



            tbA1[0, 0] = tbA11;
            tbA1[0, 1] = tbA12;
            tbA1[0, 2] = tbA13;
            tbA1[0, 3] = tbA14;
            tbA1[0, 4] = tbA15;
            tbA1[0, 5] = tbA16;
            tbA1[0, 6] = tbA17;
            tbA1[0, 7] = tbA18;
            tbA1[0, 8] = tbA19;

            tbA1[1, 0] = tbA21;
            tbA1[1, 1] = tbA22;
            tbA1[1, 2] = tbA23;
            tbA1[1, 3] = tbA24;
            tbA1[1, 4] = tbA25;
            tbA1[1, 5] = tbA26;
            tbA1[1, 6] = tbA27;
            tbA1[1, 7] = tbA28;
            tbA1[1, 8] = tbA29;

            tbA1[2, 0] = tbA31;
            tbA1[2, 1] = tbA32;
            tbA1[2, 2] = tbA33;
            tbA1[2, 3] = tbA34;
            tbA1[2, 4] = tbA35;
            tbA1[2, 5] = tbA36;
            tbA1[2, 6] = tbA37;
            tbA1[2, 7] = tbA38;
            tbA1[2, 8] = tbA39;

            tbA1[3, 0] = tbA41;
            tbA1[3, 1] = tbA42;
            tbA1[3, 2] = tbA43;
            tbA1[3, 3] = tbA44;
            tbA1[3, 4] = tbA45;
            tbA1[3, 5] = tbA46;
            tbA1[3, 6] = tbA47;
            tbA1[3, 7] = tbA48;
            tbA1[3, 8] = tbA49;

            tbA1[4, 0] = tbA51;
            tbA1[4, 1] = tbA52;
            tbA1[4, 2] = tbA53;
            tbA1[4, 3] = tbA54;
            tbA1[4, 4] = tbA55;
            tbA1[4, 5] = tbA56;
            tbA1[4, 6] = tbA57;
            tbA1[4, 7] = tbA58;
            tbA1[4, 8] = tbA59;

            tbA1[5, 0] = tbA61;
            tbA1[5, 1] = tbA62;
            tbA1[5, 2] = tbA63;
            tbA1[5, 3] = tbA64;
            tbA1[5, 4] = tbA65;
            tbA1[5, 5] = tbA66;
            tbA1[5, 6] = tbA67;
            tbA1[5, 7] = tbA68;
            tbA1[5, 8] = tbA69;

            tbA1[6, 0] = tbA71;
            tbA1[6, 1] = tbA72;
            tbA1[6, 2] = tbA73;
            tbA1[6, 3] = tbA74;
            tbA1[6, 4] = tbA75;
            tbA1[6, 5] = tbA76;
            tbA1[6, 6] = tbA77;
            tbA1[6, 7] = tbA78;
            tbA1[6, 8] = tbA79;

            tbA1[7, 0] = tbA81;
            tbA1[7, 1] = tbA82;
            tbA1[7, 2] = tbA83;
            tbA1[7, 3] = tbA84;
            tbA1[7, 4] = tbA85;
            tbA1[7, 5] = tbA86;
            tbA1[7, 6] = tbA87;
            tbA1[7, 7] = tbA88;
            tbA1[7, 8] = tbA89;

            tbA1[8, 0] = tbA91;
            tbA1[8, 1] = tbA92;
            tbA1[8, 2] = tbA93;
            tbA1[8, 3] = tbA94;
            tbA1[8, 4] = tbA95;
            tbA1[8, 5] = tbA96;
            tbA1[8, 6] = tbA97;
            tbA1[8, 7] = tbA98;
            tbA1[8, 8] = tbA99;

            lbAT[0] = lbA1T;
            lbAT[1] = lbA2T;
            lbAT[2] = lbA3T;
            lbAT[3] = lbA4T;
            lbAT[4] = lbA5T;
            lbAT[5] = lbA6T;
            lbAT[6] = lbA7T;
            lbAT[7] = lbA8T;
            lbAT[8] = lbA9T;

            lbAL[0] = lbA1l;
            lbAL[1] = lbA2l;
            lbAL[2] = lbA3l;
            lbAL[3] = lbA4l;
            lbAL[4] = lbA5l;
            lbAL[5] = lbA6l;
            lbAL[6] = lbA7l;
            lbAL[7] = lbA8l;
            lbAL[8] = lbA9l;

            lbWA[0] = WA1;
            lbWA[1] = WA2;
            lbWA[2] = WA3;
            lbWA[3] = WA4;
            lbWA[4] = WA5;
            lbWA[5] = WA6;
            lbWA[6] = WA7;
            lbWA[7] = WA8;
            lbWA[8] = WA9;


            string sqlskill = "select CLASSNAME,CLASSID from FXCLASS where PCLASSID = 10002 and SJC='" + labelsjc.Text + "'";


            dtskill = dbclass.GreatDs(sqlskill).Tables[0];

            skillcount = dtskill.Rows.Count;

            string temp = "";
            string pattern1 = @"[\u4e00-\u9fa5]+";
            string pattern2 = @"[a-zA-Z0-9\s]+";
            string[] result1;
            string[] result2;

            for (int i = 0; i < skillcount; i++)
            {
                temp = dtskill.Rows[i][0].ToString();
                result1 = Regex.Split(temp, pattern1);
                result2 = Regex.Split(temp, pattern2);
                lbAT[i].Text = result1[1];
                lbAL[i].Text = result2[0];
            }

            for (int i = skillcount; i < 9; i++)
            {
                lbAT[i].Visible = false;
                lbAL[i].Visible = false;
                lbWA[i].Visible = false;
            }

            for (int i = 0; i < 9; i++) // 外层循环遍历行
            {
                for (int j = 0; j < 9; j++) // 内层循环遍历列
                {
                    if (i >= skillcount || j >= skillcount)
                    {
                        tbA1[i, j].Visible = false;
                    }
                }
            }
        }

        private void btnweight_Click(object sender, EventArgs e)
        {
            double[,] data = new double[skillcount, skillcount];

            for (int i = 0; i < skillcount; i++) // 外层循环遍历行
            {
                for (int j = 0; j < skillcount; j++) // 内层循环遍历列
                {
                    data[i, j] = 0;
                }
            }

            try
            {
                for (int i = 0; i < skillcount; i++) // 外层循环遍历行
                {
                    for (int j = 0; j < skillcount; j++) // 内层循环遍历列
                    {
                        if (tbA1[i, j].Text.Trim().Contains("/"))
                        {
                            string[] numbers = tbA1[i, j].Text.Trim().Split('/');
                            double num1 = double.Parse(numbers[0]);
                            double num2 = double.Parse(numbers[1]);
                            data[i, j] = num1 / num2;
                        }
                        else
                        {
                            data[i, j] = Double.Parse(tbA1[i, j].Text.Trim());
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("无法将文本转换为双精度浮点数Unable to convert text to double precision floating-point number！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // 1. 构建成对比较矩阵



            // 2. 计算权重向量
            double[] weights = HierarchicalAnalysis.CalculateWeights(data);

            // 3. 计算最大特征值根
            double maxEigenvalue = HierarchicalAnalysis.CalculateMaxEigenvalue(data, weights);

            // 4. 计算一致性指标和一致性比率
            int n = data.GetLength(0);
            double randomIndex = HierarchicalAnalysis.GetRandomIndex(n);
            double consistencyIndex = (maxEigenvalue - n) / (n - 1);
            double consistencyRatio = consistencyIndex / randomIndex;

            for (int i = 0; i < skillcount; i++)
            {
                lbWA[i].Text = weights[i].ToString();
            }

            lbtzz.Text = maxEigenvalue.ToString();
            lbci.Text = consistencyIndex.ToString();
            lbcr.Text = consistencyRatio.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selecthtrw = "select SJC from Three_Weight_INFO where SJC='" + labelsjc.Text + "' and PCLASSID = 10002";
            string getdata = dbclass.GetOneValue(selecthtrw);

            if (getdata.Equals(""))
            {
                string[] insertthw = new string[skillcount];
                try
                {
                    for (int i = 0; i < skillcount; i++)
                    {
                        insertthw[i] = String.Format("Insert into Three_Weight_INFO(CLASSID,CLASSNAME,PCLASSID,SJC,WEIGHT) values ('{0}','{1}','{2}','{3}',{4})", int.Parse(dtskill.Rows[i][1].ToString()), dtskill.Rows[i][0].ToString(), 10002, labelsjc.Text, Double.Parse(lbWA[i].Text));
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("无法将文本转换为双精度浮点数Unable to convert text to double precision floating-point number！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                try
                {
                    dbclass.ExecNonQuerySW(insertthw);
                    MessageBox.Show("保存成功Saved successfully！");
                }
                catch
                {
                    string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                    dbclass.DoSql(sqlinsertlogs);

                    MessageBox.Show("保存失败Save failed！", "Error");
                    return;
                }
            }
            else
            {
                string[] updatethw = new string[skillcount];
                try
                {
                    for (int i = 0; i < skillcount; i++)
                    {
                        updatethw[i] = "update Three_Weight_INFO set WEIGHT = " + Double.Parse(lbWA[i].Text) + " where CLASSID = " + int.Parse(dtskill.Rows[i][1].ToString()) + " and PCLASSID = 10002 and  SJC = '" + labelsjc.Text + "'";
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("无法将文本转换为双精度浮点数Unable to convert text to double precision floating-point number！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                try
                {
                    dbclass.ExecNonQuerySW(updatethw);
                    MessageBox.Show("保存成功Saved successfully！");
                }
                catch
                {
                    string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                    dbclass.DoSql(sqlinsertlogs);

                    MessageBox.Show("保存失败Save failed！", "Error");
                    return;
                }

            }
        }

        private void btncsh_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < skillcount; i++) // 外层循环遍历行
            {
                for (int j = 0; j < skillcount; j++) // 内层循环遍历列
                {
                    tbA1[i, j].Text = "";
                }
            }

            for (int i = 0; i < skillcount; i++)
            {
                lbWA[i].Text = "";
            }

            lbci.Text = null;
            lbcr.Text = null;
            lbtzz.Text = null;
        }
    }
}
