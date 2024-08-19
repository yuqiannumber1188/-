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
    public partial class AHP_THREE_Form : Form
    {
        DBClass dbclass = new DBClass();
        string xsbsql = "select * from xsb";
        string sccjsql = "select * from SCCJ";
        string sccjzdsql = "select * from zdgxb where bmc = 'sccj' order by id asc";
        string deletesccjsql = "delete from zdgxb where bmc = 'sccj'";

        string sjc;
        MainForm fm;

        public AHP_THREE_Form(MainForm _fm)
        {
            InitializeComponent();

            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
            sjc = labelsjc.Text;

            
        }

        private void btnweight_Click(object sender, EventArgs e)
        {
            string textA33 = tbA33.Text.Trim().ToString();
            string textA32 = tbA32.Text.Trim().ToString();
            string textA31 = tbA31.Text.Trim().ToString();
            string textA23 = tbA23.Text.Trim().ToString();
            string textA22 = tbA22.Text.Trim().ToString();
            string textA21 = tbA21.Text.Trim().ToString();
            string textA13 = tbA13.Text.Trim().ToString();
            string textA12 = tbA12.Text.Trim().ToString();
            string textA11 = tbA11.Text.Trim().ToString();
            double numberA33 =0;
            double numberA32 =0;
            double numberA31 = 0;
            double numberA23 =0 ;
            double numberA22=0;
            double numberA21=0;
            double numberA13=0;
            double numberA12=0;
            double numberA11=0;
            try
            {
                if (textA33.Contains("/"))
                {
                    string[] numbers = textA33.Split('/');
                    double num1 = double.Parse(numbers[0]);
                    double num2 = double.Parse(numbers[1]);
                    numberA33 = num1 / num2;
                }
                else
                {
                    numberA33 = Double.Parse(textA33);
                }
                if (textA32.Contains("/"))
                {
                    string[] numbers = textA32.Split('/');
                    double num1 = double.Parse(numbers[0]);
                    double num2 = double.Parse(numbers[1]);
                    numberA32 = num1 / num2;
                }
                else
                {
                    numberA32 = Double.Parse(textA32);
                }
                if (textA31.Contains("/"))
                {
                    string[] numbers = textA31.Split('/');
                    double num1 = double.Parse(numbers[0]);
                    double num2 = double.Parse(numbers[1]);
                    numberA31 = num1 / num2;
                }
                else
                {
                    numberA31 = Double.Parse(textA31);
                }


                if (textA23.Contains("/"))
                {
                    string[] numbers = textA23.Split('/');
                    double num1 = double.Parse(numbers[0]);
                    double num2 = double.Parse(numbers[1]);
                    numberA23 = num1 / num2;
                }
                else
                {
                    numberA23 = Double.Parse(textA23);
                }
                if (textA22.Contains("/"))
                {
                    string[] numbers = textA22.Split('/');
                    double num1 = double.Parse(numbers[0]);
                    double num2 = double.Parse(numbers[1]);
                    numberA22 = num1 / num2;
                }
                else
                {
                    numberA22 = Double.Parse(textA22);
                }
                if (textA21.Contains("/"))
                {
                    string[] numbers = textA21.Split('/');
                    double num1 = double.Parse(numbers[0]);
                    double num2 = double.Parse(numbers[1]);
                    numberA21 = num1 / num2;
                }
                else
                {
                    numberA21 = Double.Parse(textA21);
                }

                if (textA13.Contains("/"))
                {
                    string[] numbers = textA13.Split('/');
                    double num1 = double.Parse(numbers[0]);
                    double num2 = double.Parse(numbers[1]);
                    numberA13 = num1 / num2;
                }
                else
                {
                    numberA13 = Double.Parse(textA13);
                }
                if (textA12.Contains("/"))
                {
                    string[] numbers = textA12.Split('/');
                    double num1 = double.Parse(numbers[0]);
                    double num2 = double.Parse(numbers[1]);
                    numberA12 = num1 / num2;
                }
                else
                {
                    numberA12 = Double.Parse(textA12);
                }
                if (textA11.Contains("/"))
                {
                    string[] numbers = textA11.Split('/');
                    double num1 = double.Parse(numbers[0]);
                    double num2 = double.Parse(numbers[1]);
                    numberA11 = num1 / num2;
                }
                else
                {
                    numberA11 = Double.Parse(textA11);
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("无法将文本转换为双精度浮点数Unable to convert text to double precision floating-point number！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            // 1. 构建成对比较矩阵
            double[,] pairwiseMatrix = new double[3, 3];
            pairwiseMatrix[0, 0] = numberA33;
            pairwiseMatrix[0, 1] = numberA32;
            pairwiseMatrix[0, 2] = numberA31;

            pairwiseMatrix[1, 0] = numberA23;
            pairwiseMatrix[1, 1] = numberA22;
            pairwiseMatrix[1, 2] = numberA21;

            pairwiseMatrix[2, 0] = numberA13;
            pairwiseMatrix[2, 1] = numberA12;
            pairwiseMatrix[2, 2] = numberA11;

            // 2. 计算权重向量
            double[] weights = HierarchicalAnalysis.CalculateWeights(pairwiseMatrix);

            // 3. 计算最大特征值根
            double maxEigenvalue = HierarchicalAnalysis.CalculateMaxEigenvalue(pairwiseMatrix, weights);

            // 4. 计算一致性指标和一致性比率
            int n = pairwiseMatrix.GetLength(0);
            double randomIndex = HierarchicalAnalysis.GetRandomIndex(n);
            double consistencyIndex = (maxEigenvalue - n) / (n - 1);
            double consistencyRatio = consistencyIndex / randomIndex;

            W3.Text = weights[0].ToString();
            W2.Text = weights[1].ToString();
            W1.Text = weights[2].ToString();

            lbtzz.Text = maxEigenvalue.ToString();
            lbci.Text = consistencyIndex.ToString();
            lbcr.Text = consistencyRatio.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectthw = "select Code from TE_W_INFO where Code='" + labelsjc.Text + "'";
            string getdata = dbclass.GetOneValue(selectthw);

            if (getdata.Equals(""))
            {
                string insertthw = null;
                try
                {
                    insertthw = String.Format("Insert into TE_W_INFO(Code,KNOWLEDGE,SKILL,ABILITY) values ('{0}','{1}','{2}','{3}')", labelsjc.Text, Double.Parse(W1.Text), Double.Parse(W2.Text), Double.Parse(W3.Text));
                }
                catch (FormatException)
                {
                    MessageBox.Show("无法将文本转换为双精度浮点数Unable to convert text to double precision floating-point number！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                try
                {
                    dbclass.DoSql(insertthw);
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
                string updatethw = null;
                try
                {
                    updatethw = "update TE_W_INFO set KNOWLEDGE = '"+ Double.Parse(W1.Text) + "',SKILL = '"+ Double.Parse(W2.Text) + "',ABILITY = '"+ Double.Parse(W3.Text) + "' where Code = '"+ labelsjc.Text + "'";

                }
                catch (FormatException)
                {
                    MessageBox.Show("无法将文本转换为双精度浮点数Unable to convert text to double precision floating-point number！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                try
                {
                    dbclass.DoSql(updatethw);
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
            tbA11.Text = null;
            tbA12.Text = null;
            tbA13.Text = null;

            tbA21.Text = null;
            tbA22.Text = null;
            tbA23.Text = null;

            tbA31.Text = null;
            tbA32.Text = null;
            tbA33.Text = null;

            W1.Text = null;
            W2.Text = null;
            W3.Text = null;

            lbci.Text = null;
            lbcr.Text = null;
            lbtzz.Text = null;

            string delthw = "delete from TE_W_INFO where Code = '" + labelsjc.Text + "'";

            try
            {
                dbclass.DoSql(delthw);
                MessageBox.Show("初始化成功Initialized successfully！");
            }
            catch
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化失败" + "','" + "sccj" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                MessageBox.Show("初始化失败initialization failed！", "Error");
                return;
            }
        }
    }
}
