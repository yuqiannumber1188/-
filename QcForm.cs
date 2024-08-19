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
using System.Web;
using cjpc.utils;
using cjpc.utils.dbutils;
using System.Text.RegularExpressions;

namespace cjpc
{
    public partial class QcForm : Form
    {
        DBClass dbclass = new DBClass();
        List<string> treeNodes = new List<string>();
        string xsbsql = "select * from xsb";
        string delxsbsql = "delete from xsb";

        string ewt = "select CLASSID,CLASSNAME,SJC,yzdmc,szdmc,SWEIGHT,smf from ELEMENT_WEIGHT_INFO";

        string sjc;
        MainForm fm;

        public QcForm(MainForm _fm)
        {
            InitializeComponent();
            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
            sjc = labelsjc.Text;

            //课程
            string sqlfx = "SELECT '' AS [KC_CODE], '' AS [KC_NAME] UNION  SELECT distinct([KC_CODE]),[KC_NAME] FROM [xyfxxt].[dbo].[VRE_TE_KC_INFO] WHERE [TEACHER_CODE]='" + label2.Text + "' ORDER BY KC_CODE ASC";    //课程
            DataTable dtfx = new DataTable();
            dtfx = dbclass.GreatDs(sqlfx).Tables[0];

            cbkm.DataSource = dtfx;
            cbkm.DisplayMember = "KC_NAME";
            cbkm.ValueMember = "KC_CODE";

            dataGridView1.DataSource = dbclass.GreatDs(ewt).Tables[0];
            dataGridView1.Columns[0].HeaderText = "节点编号number";
            dataGridView1.Columns[1].HeaderText = "节点名称node name";
            dataGridView1.Columns[2].HeaderText = "试卷编号Exam paper number";
            dataGridView1.Columns[3].HeaderText = "试题标号Test question identification";
            dataGridView1.Columns[4].HeaderText = "试题题号Test question number";
            dataGridView1.Columns[5].HeaderText = "熵权法权重EWM weight";
            dataGridView1.Columns[6].HeaderText = "层次分析-熵权法满分值AHP-EWM full score";

            InitTree("all");
            InitListBox();
        }

        private void InitListBox()
        {
            lBtmlist.Items.Clear();
            string listxssql = "select yzdmc,szdmc from zdgxb" + sjc + " where bmc='jsb" + sjc + "'and yzdmc like 'm%'";
            DataTable dt = dbclass.GreatDs(listxssql).Tables[0];
            lBtmlist.DisplayMember = "Text";

            lBtmlist.ValueMember = "Value";

            List<ListItem> list = new List<ListItem>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(new ListItem(dt.Rows[i]["szdmc"].ToString(), dt.Rows[i]["yzdmc"].ToString()));
            }

            lBtmlist.DataSource = list;
            //lBtmlist.DataSource = dt;
            //lBtmlist.DisplayMember = "szdmc";
            //lBtmlist.ValueMember = "yzdmc";
        }

        private void InitTree(string tj)
        {
            treeView1.Nodes.Clear();
            string sqltree_Department = "select classid,classname,pclassid from fxclass where fxtype='" + tj + "' and sjc='" + labelsjc.Text + "'";
            DataTable dt = dbclass.GreatDs(sqltree_Department).Tables[0];
            DataView dv = new DataView(dt);
            // dv.RowFilter = "pclassid=0";
            foreach (DataRowView drv in dv)
            {
                TreeNode node = new TreeNode();
                node.Text = drv["classname"].ToString();
                node.Tag = drv["classid"].ToString();
                node.Expand();
                treeView1.Nodes.Add(node);
                AddReplies(dt, node);
            }
            treeView1.ExpandAll();
        }

        private void AddReplies(DataTable dt, TreeNode node)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = "pclassid=" + node.Tag;
            foreach (DataRowView row in dv)
            {
                TreeNode replyNode = new TreeNode();
                replyNode.Text = row["classname"].ToString();
                replyNode.Tag = row["classid"].ToString();
                replyNode.Expand();
                node.Nodes.Add(replyNode);
                AddReplies(dt, replyNode);
            }
        }

        private void btnzjzs_Click(object sender, EventArgs e)
        {
            //判断是否熵权法插入
            string selectContain = "select ID FROM ELEMENT_WEIGHT_INFO where CLASSID = " + lbjdbh.Text + " and SJC = '" + sjc + "'";
            string czone = dbclass.GetOneValue(selectContain);
            if (!czone.Equals(""))
            {
                MessageBox.Show("请先删除Please delete first " + lbjdmc.Text + " 题目 topic！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            List<int> indexsel = new List<int>();

            string[] sqlinsdate = new string[lBtmlist.SelectedItems.Count];

            int i = 0;
            foreach (object item in lBtmlist.SelectedItems)
            {
                //indexsel.Add(lBtmlist.Items.IndexOf(item));
                string temptm = ((ListItem)item).Value.ToString();
                string tempvalue = ((ListItem)item).Text.ToString();
                sqlinsdate[i] = String.Format("Insert into ELEMENT_WEIGHT_INFO(CLASSID,CLASSNAME,SJC,yzdmc,szdmc) values ('{0}','{1}','{2}','{3}','{4}')", int.Parse(lbjdbh.Text), lbjdmc.Text, labelsjc.Text, temptm, tempvalue);
                i++;
            }

            try
            {
                dbclass.ExecNonQuerySW(sqlinsdate);
                dataGridView1.DataSource = dbclass.GreatDs(ewt).Tables[0];
                dataGridView1.Columns[0].HeaderText = "节点编号number";
                dataGridView1.Columns[1].HeaderText = "节点名称node name";
                dataGridView1.Columns[2].HeaderText = "试卷编号Exam paper number";
                dataGridView1.Columns[3].HeaderText = "试题标号Test question identification";
                dataGridView1.Columns[4].HeaderText = "试题题号Test question number";
                dataGridView1.Columns[5].HeaderText = "熵权法权重EWM weight";
                dataGridView1.Columns[6].HeaderText = "层次分析-熵权法满分值AHP-EWM full score";

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "新增题目成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("新增题目成功Successfully added a new question！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "新增题目失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("新增题目失败Failed to add a new question！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void cbkm_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitTree(cbkm.SelectedValue.ToString());
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                lbjdmc.Text = treeView1.SelectedNode.Text;
                lbjdbh.Text = treeView1.SelectedNode.Tag.ToString();
            }
        }

        private void btnhbzs_Click(object sender, EventArgs e)
        {
            string selectdate = "select yzdmc from ELEMENT_WEIGHT_INFO where SJC = '" + sjc + "' and CLASSID = " + lbjdbh.Text;
            DataTable dtEW = dbclass.GreatDs(selectdate).Tables[0];
            if(dtEW.Rows.Count == 0)
            {
                MessageBox.Show("请先选择Please choose first " + lbjdmc.Text + " 题目 topic，并按增加按钮and press the Add button", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectE = "";
            for(int i = 0;i < dtEW.Rows.Count; i++)
            {
                selectE = selectE + dtEW.Rows[i][0].ToString() + ",";
            }
            selectE = selectE.Remove(selectE.Length - 1);

            string selectEt = "select " + selectE + " from YSCJ" + sjc;
            DataTable dtEt = dbclass.GreatDs(selectEt).Tables[0];
            double[,] data = new double[dtEt.Rows.Count, dtEt.Columns.Count];
            for(int i =0;i< dtEt.Rows.Count;i++)
            {
                for (int j = 0; j < dtEt.Columns.Count; j++)
                {
                    data[i, j] = Double.Parse(dtEt.Rows[i][j].ToString());
                }
            }

            // 计算权重
            List<double> weights = EntropyWeightMethod.CalculateWeights(data);

            //更新权重
            string[] updateEW = new string[dtEW.Rows.Count];
            for(int i = 0;i< dtEW.Rows.Count; i++)
            {
                updateEW[i] = "update ELEMENT_WEIGHT_INFO set SWEIGHT = " + weights[i] + " where SJC = '" + sjc + "' and CLASSID = " + lbjdbh.Text + " and yzdmc = '" + dtEW.Rows[i][0].ToString() + "'";
            }

            try
            {
                dbclass.ExecNonQuerySW(updateEW);
                dataGridView1.DataSource = dbclass.GreatDs(ewt).Tables[0];
                dataGridView1.Columns[0].HeaderText = "节点编号number";
                dataGridView1.Columns[1].HeaderText = "节点名称node name";
                dataGridView1.Columns[2].HeaderText = "试卷编号Exam paper number";
                dataGridView1.Columns[3].HeaderText = "试题标号Test question identification";
                dataGridView1.Columns[4].HeaderText = "试题题号Test question number";
                dataGridView1.Columns[5].HeaderText = "熵权法权重EWM weight";
                dataGridView1.Columns[6].HeaderText = "层次分析-熵权法满分值AHP-EWM full score";

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "熵权法计算权重成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("熵权法计算权重成功The entropy-weight method is successful in calculating the weights！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "熵权法计算权重失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("熵权法计算权重失败The entropy weight method failed to calculate the weights！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

        }

        private void btnzj_Click(object sender, EventArgs e)
        {
            string delEW = "delete from ELEMENT_WEIGHT_INFO where SJC = '" + sjc + "' and CLASSID = " + lbjdbh.Text;
            try
            {
                dbclass.DoSql(delEW);
                dataGridView1.DataSource = dbclass.GreatDs(ewt).Tables[0];
                dataGridView1.Columns[0].HeaderText = "节点编号number";
                dataGridView1.Columns[1].HeaderText = "节点名称node name";
                dataGridView1.Columns[2].HeaderText = "试卷编号Exam paper number";
                dataGridView1.Columns[3].HeaderText = "试题标号Test question identification";
                dataGridView1.Columns[4].HeaderText = "试题题号Test question number";
                dataGridView1.Columns[5].HeaderText = "熵权法权重EWM weight";
                dataGridView1.Columns[6].HeaderText = "层次分析-熵权法满分值AHP-EWM full score";

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "删除题目成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("删除题目成功The deletion of the question is successful！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "删除题目失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("删除题目失败Failed to delete the question！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnmfz_Click(object sender, EventArgs e)
        {
            double zmf = 0;
            string selmf = "select fs,tm from ysffb"+sjc;
            DataTable dtMf = dbclass.GreatDs(selmf).Tables[0];
            int coutMf = dtMf.Rows.Count;
            for(int i =0;i < coutMf; i++)
            {
                zmf = zmf + Double.Parse(dtMf.Rows[i][0].ToString());       
            }

            

            string selw = "SELECT CLASSID, PCLASSID, SJC, yzdmc, szdmc, SWEIGHT, WEIGHT FROM VRE_WEIGT WHERE SJC = '"+sjc+"'";
            DataTable dtW = dbclass.GreatDs(selw).Tables[0];
            int coutW = dtW.Rows.Count;
            //更新权重
            string[] updateMF = new string[coutW];
            double tempMF = 0;
            double tempmf = 0;
            string smf = "";
            for (int i = 0;i < coutW; i++)
            {
                smf = "select fs from ysffb" + sjc + " where tm = '" + dtW.Rows[i][4].ToString() + "'";
                tempmf = Double.Parse(dbclass.GetOneValue(smf));
                tempMF = zmf* Double.Parse(dtW.Rows[i][5].ToString()) * Double.Parse(dtW.Rows[i][6].ToString())/ tempmf;
                updateMF[i] = "update ELEMENT_WEIGHT_INFO set smf = " + tempMF + " where SJC = '" + sjc + "' and CLASSID = " + dtW.Rows[i][0].ToString() + " and yzdmc = '" + dtW.Rows[i][3].ToString() + "'";
            }


            try
            {
                dbclass.ExecNonQuerySW(updateMF);
                dataGridView1.DataSource = dbclass.GreatDs(ewt).Tables[0];
                dataGridView1.Columns[0].HeaderText = "节点编号number";
                dataGridView1.Columns[1].HeaderText = "节点名称node name";
                dataGridView1.Columns[2].HeaderText = "试卷编号Exam paper number";
                dataGridView1.Columns[3].HeaderText = "试题标号Test question identification";
                dataGridView1.Columns[4].HeaderText = "试题题号Test question number";
                dataGridView1.Columns[5].HeaderText = "熵权法权重EWM weight";
                dataGridView1.Columns[6].HeaderText = "层次分析-熵权法满分值AHP-EWM full score";

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "熵权法计算权重成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("熵权法计算权重成功The entropy-weight method is successful in calculating the weights！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "熵权法计算权重失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("熵权法计算权重失败The entropy weight method failed to calculate the weights！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //更新系数表
            string seldisw = "SELECT DISTINCT yzdmc FROM ELEMENT_WEIGHT_INFO";
            DataTable dtDisw = dbclass.GreatDs(seldisw).Tables[0];
            int coutDisw = dtDisw.Rows.Count;
            string[] updatetempxsb = new string[coutDisw];
            string[] updatexsb = null;
            DataTable tempdtRw = new DataTable();

            string temppid = "";
            string temppzdmc = "";
            string tempzdmc = "";
            string pattern2 = @"[a-zA-Z0-9\s]+";
            
            string selwu = "SELECT CLASSID, CLASSNAME, PCLASSID, SJC, yzdmc, szdmc, SWEIGHT, WEIGHT,smf  FROM VRE_WEIGT WHERE SJC = '" + sjc + "' and yzdmc = '";

            try
            {
                for (int i = 0; i < coutDisw; i++)
                {
                    updatetempxsb[i] = selwu + dtDisw.Rows[i][0].ToString() + "'";
                    tempdtRw = dbclass.GreatDs(updatetempxsb[i]).Tables[0];
                    updatexsb = new string[tempdtRw.Rows.Count];
                    for (int j = 0; j < tempdtRw.Rows.Count; j++)
                    {
                        tempzdmc = Regex.Split(tempdtRw.Rows[j][1].ToString(), pattern2)[0];
                        temppid = tempdtRw.Rows[j][2].ToString();
                        if (temppid.Equals("10001"))
                        {
                            temppzdmc = "知识";
                        }
                        else if (temppid.Equals("10002"))
                        {
                            temppzdmc = "技能";
                        }
                        else if (temppid.Equals("10003"))
                        {
                            temppzdmc = "能力";
                        }

                        updatexsb[j] = "UPDATE xsb" + sjc + " SET " + temppzdmc + " = " + tempdtRw.Rows[j][8].ToString() + ", " + tempzdmc + " = " + tempdtRw.Rows[j][8].ToString() + " WHERE 题号 = '" + dtDisw.Rows[i][0].ToString() + "'";
                    }
                    dbclass.ExecNonQuerySW(updatexsb);
                }

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "系数表修改成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("系数表修改成功The entropy-weight method is successful in calculating the weights！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "系数表修改失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("系数表修改失败The entropy weight method failed to calculate the weights！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            
            




        }
    }
}
