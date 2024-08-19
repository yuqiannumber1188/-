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

namespace cjpc
{
    public partial class FlForm : Form
    {
        DBClass dbclass = new DBClass();
        List<string> treeNodes = new List<string>();
        string xsbsql = "select * from xsb";
        string delxsbsql = "delete from xsb";

        string sjc;
        MainForm fm;

        public FlForm(MainForm _fm)
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

            dataGridView1.DataSource = dbclass.GreatDs(xsbsql+sjc).Tables[0];
            InitTree("all");
            InitListBox();
        }

        private void InitListBox()
        {
            lBtmlist.Items.Clear();
            string listxssql = "select yzdmc,szdmc from zdgxb"+sjc+" where bmc='jsb"+sjc+"'and yzdmc like 'm%'";
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
            string sqltree_Department = "select classid,classname,pclassid from fxclass where fxtype='" + tj + "' and sjc='"+labelsjc.Text + "'";
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

        private void cbkm_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitTree(cbkm.SelectedValue.ToString());
        }

        private void btnzjzs_Click(object sender, EventArgs e)
        {
            List<int> indexsel = new List<int>();

            foreach (object item in lBtmlist.SelectedItems)
            {
                indexsel.Add(lBtmlist.Items.IndexOf(item));
            }

            string[] sqlupdate = new string[lBtmlist.Items.Count];

            for (int i = 0; i < lBtmlist.Items.Count; i++)
            {
                string temptm = ((ListItem)lBtmlist.Items[i]).Value.ToString();
                if (indexsel.Contains(i))
                {
                    string sqlvalue = "select A.fs from (SELECT zdgxb"+sjc+".yzdmc,zdgxb"+sjc+".szdmc,ysffb"+sjc+".fs,zdgxb"+sjc+".bmc FROM zdgxb"+sjc+" INNER JOIN ysffb"+sjc+" ON zdgxb"+sjc+".szdmc = ysffb"+sjc+".tm) as A where A.bmc = 'jsb"+sjc+"' and A.yzdmc = '" + temptm + "'";
                    string tempvalue = dbclass.GetOneValue(sqlvalue);
                    sqlupdate[i] = "update xsb"+sjc+" set " + lbjdmc.Text + " = " + tempvalue + " where 题号 = '"+temptm+"'";
                }
                else
                {
                    sqlupdate[i] = "update xsb"+sjc+" set " + lbjdmc.Text + " = " + 0 + " where 题号 = '"+temptm+"'";
                }
            }

            try
            {
                dbclass.ExecNonQuerySW(sqlupdate);
                dataGridView1.DataSource = dbclass.GreatDs(xsbsql+sjc).Tables[0];

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "知识节点修改成功"  + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "知识节点修改失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("修改失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnzj_Click(object sender, EventArgs e)
        {
            List<int> indexsel = new List<int>();

            foreach (object item in lBtmlist.SelectedItems)
            {
                indexsel.Add(lBtmlist.Items.IndexOf(item));
            }

            string[] sqlupdate = new string[lBtmlist.Items.Count];

            for (int i = 0; i < lBtmlist.Items.Count; i++)
            {
                string temptm = ((ListItem)lBtmlist.Items[i]).Value.ToString();
                if (indexsel.Contains(i))
                {
                    string sqlvalue = "select " + temptm + " from jsb"+sjc+" where 分项='修正能力值'";
                    string tempvalue = dbclass.GetOneValue(sqlvalue);
                    sqlupdate[i] = "update xsb"+sjc+" set " + lbjdmc.Text + "= "+tempvalue+"  where 题号 = '"+temptm+"'";
                }
                else
                {
                    sqlupdate[i] = "update xsb" + sjc + " set " + lbjdmc.Text + "= " + 0 + "  where 题号 = '" + temptm + "'";
                }
            }

            try
            {
                dbclass.ExecNonQuerySW(sqlupdate);
                dataGridView1.DataSource = dbclass.GreatDs(xsbsql+sjc).Tables[0];
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "其它节点修改成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "其它节点修改失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("修改失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnhbzs_Click(object sender, EventArgs e)
        {
            if (cbkm.SelectedValue.ToString().Equals(""))
            {
                MessageBox.Show("请选择分类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sqlzs = "select classname from fxclass where pclassid = 10001 and fxtype='" + cbkm.SelectedValue.ToString() + "' and sjc='"+labelsjc.Text + "'";
            DataTable dtzs = dbclass.GreatDs(sqlzs).Tables[0];
            string[] sqlupdate = new string[lBtmlist.Items.Count];
            for (int i = 0; i < lBtmlist.Items.Count; i++)
            {
                double dsum = 0;
                string temptm = ((ListItem)lBtmlist.Items[i]).Value.ToString();
                string tempfs = "";
                string sqlls = "";
                for (int j = 0; j < dtzs.Rows.Count; j++)
                {
                    sqlls = "select " + dtzs.Rows[j][0].ToString() + " from xsb"+sjc+" where 题号='" + temptm + "'";
                    tempfs = dbclass.GetOneValue(sqlls);

                    dsum = dsum + Double.Parse(tempfs);

                }
                sqlupdate[i] = "update xsb"+sjc+" set 知识 = "+dsum+"  where 题号 = '"+temptm+"'";
            }

            try
            {
                dbclass.ExecNonQuerySW(sqlupdate);
                dataGridView1.DataSource = dbclass.GreatDs(xsbsql+sjc).Tables[0];
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "知识合并成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("知识合并成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "知识合并失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                
                MessageBox.Show("知识合并失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnhbjn_Click(object sender, EventArgs e)
        {
            if (cbkm.SelectedValue.ToString().Equals(""))
            {
                MessageBox.Show("请选择分类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sqlzs = "select classname from fxclass where pclassid = 10002 and fxtype='" + cbkm.SelectedValue.ToString() + "' and sjc='"+labelsjc.Text + "'";
            DataTable dtzs = dbclass.GreatDs(sqlzs).Tables[0];
            string[] sqlupdate = new string[lBtmlist.Items.Count];
            for (int i = 0; i < lBtmlist.Items.Count; i++)
            {
                double dsum = 0;
                string temptm = ((ListItem)lBtmlist.Items[i]).Value.ToString();
                string tempfs = "";
                string sqlls = "";
                for (int j = 0; j < dtzs.Rows.Count; j++)
                {
                    sqlls = "select " + dtzs.Rows[j][0].ToString() + " from xsb"+sjc+" where 题号='" + temptm + "'";
                    tempfs = dbclass.GetOneValue(sqlls);

                    dsum = dsum + Double.Parse(tempfs);

                }
                sqlupdate[i] = "update xsb"+sjc+" set 技能 = "+dsum+"  where 题号 = '"+temptm+"'";
            }

            try
            {
                dbclass.ExecNonQuerySW(sqlupdate);
                dataGridView1.DataSource = dbclass.GreatDs(xsbsql+sjc).Tables[0];
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "技能合并成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("技能合并成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "技能合并失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                
                MessageBox.Show("技能合并失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnhbnl_Click(object sender, EventArgs e)
        {
            if (cbkm.SelectedValue.ToString().Equals(""))
            {
                MessageBox.Show("请选择分类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sqlzs = "select classname from fxclass where pclassid = 10003 and fxtype='" + cbkm.SelectedValue.ToString() + "' and sjc='"+labelsjc.Text + "'";
            DataTable dtzs = dbclass.GreatDs(sqlzs).Tables[0];
            string[] sqlupdate = new string[lBtmlist.Items.Count];
            for (int i = 0; i < lBtmlist.Items.Count; i++)
            {
                double dsum = 0;
                string temptm = ((ListItem)lBtmlist.Items[i]).Value.ToString();
                string tempfs = "";
                string sqlls = "";
                for (int j = 0; j < dtzs.Rows.Count; j++)
                {
                    sqlls = "select " + dtzs.Rows[j][0].ToString() + " from xsb"+sjc+" where 题号='" + temptm + "'";
                    tempfs = dbclass.GetOneValue(sqlls);

                    dsum = dsum + Double.Parse(tempfs);

                }
                sqlupdate[i] = "update xsb"+sjc+" set 能力 = "+dsum+"  where 题号 = '"+temptm+"'";
            }

            try
            {
                dbclass.ExecNonQuerySW(sqlupdate);
                dataGridView1.DataSource = dbclass.GreatDs(xsbsql+sjc).Tables[0];
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "能力合并成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("能力合并成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "能力合并失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("能力合并失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnzsxs_Click(object sender, EventArgs e)
        {
            if (cbkm.SelectedValue.ToString().Equals(""))
            {
                MessageBox.Show("请选择分类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sqlzs = "select classname from fxclass where pclassid = 10001 and fxtype='" + cbkm.SelectedValue.ToString() + "' and sjc='"+labelsjc.Text + "'";
            DataTable dtzs = dbclass.GreatDs(sqlzs).Tables[0];
            int dtCout = dtzs.Rows.Count;
            string[,] sqlupdatezs = new string[dtCout + 1, lBtmlist.Items.Count];

            //原始总分
            string sqlzf = "select sum(fs) from ysffb"+sjc;
            string tempzf = dbclass.GetOneValue(sqlzf);
            double dzf = Double.Parse(tempzf);

            //现在总分
            string sqlzfzs = "select sum(知识) from xsb"+sjc;
            string tempzfzs = dbclass.GetOneValue(sqlzfzs);
            double dzfzs = Double.Parse(tempzfzs);

            string sqlzdfs = "";
            string tempzdfs = "";
            double zdfx = 0;
            string temptm = "";
            string sqlvalue = "";
            string tempvalue = "";

            for (int i = 0; i < lBtmlist.Items.Count; i++)
            {
                //知识字段
                temptm = ((ListItem)lBtmlist.Items[i]).Value.ToString();
                sqlzdfs = "select 知识" + " from xsb"+sjc+" where 题号='" + temptm + "'";
                tempzdfs = dbclass.GetOneValue(sqlzdfs);
                zdfx = Math.Round(Double.Parse(tempzdfs) * dzf / dzfzs,2);
                sqlvalue = "select A.fs from (SELECT zdgxb"+sjc+".yzdmc,zdgxb"+sjc+".szdmc,ysffb"+sjc+".fs,zdgxb"+sjc+".bmc FROM zdgxb"+sjc+" INNER JOIN ysffb"+sjc+" ON zdgxb"+sjc+".szdmc = ysffb"+sjc+".tm) as A where A.bmc = 'jsb"+sjc+"' and A.yzdmc = '" + temptm + "'";
                tempvalue = dbclass.GetOneValue(sqlvalue);
                if (zdfx != 0)
                {
                    zdfx = Math.Round(zdfx / Double.Parse(tempvalue),2);
                }
                sqlupdatezs[dtCout, i] = "update xsb"+sjc+" set 知识 = "+zdfx+"  where 题号 = '"+temptm+"'";
                //知识子字段
                for (int j = 0; j < dtCout; j++)
                {
                    sqlzdfs = "select " + dtzs.Rows[j][0].ToString() + " from xsb"+sjc+" where 题号='" + temptm + "'";
                    tempzdfs = dbclass.GetOneValue(sqlzdfs);
                    zdfx = Math.Round(Double.Parse(tempzdfs) * dzf / dzfzs,2);

                    zdfx = Math.Round(zdfx / Double.Parse(tempvalue),2);
                    sqlupdatezs[j, i] = "update xsb"+sjc+" set " + dtzs.Rows[j][0].ToString() + " = "+ zdfx +" where 题号 = '"+temptm+"'";
                }

            }

            try
            {
                string[] sqlupdate = new string[lBtmlist.Items.Count];
                for (int i = 0; i < dtCout + 1; i++)
                {
                    for (int j = 0; j < lBtmlist.Items.Count; j++)
                    {
                        sqlupdate[j] = sqlupdatezs[i, j];
                    }
                    dbclass.ExecNonQuerySW(sqlupdate);
                }
                dataGridView1.DataSource = dbclass.GreatDs(xsbsql+sjc).Tables[0];
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "知识系数成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("知识系数成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "知识系数失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("知识系数失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnjnxg_Click(object sender, EventArgs e)
        {
            if (cbkm.SelectedValue.ToString().Equals(""))
            {
                MessageBox.Show("请选择分类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sqlzs = "select classname from fxclass where pclassid = 10002 and fxtype='" + cbkm.SelectedValue.ToString() + "' and sjc='"+labelsjc.Text + "'";
            DataTable dtzs = dbclass.GreatDs(sqlzs).Tables[0];
            int dtCout = dtzs.Rows.Count;
            string[,] sqlupdatezs = new string[dtCout + 1, lBtmlist.Items.Count];

            //原始总分
            string sqlzf = "select sum(fs) from ysffb"+sjc;
            string tempzf = dbclass.GetOneValue(sqlzf);
            double dzf = Double.Parse(tempzf);

            //现在总分
            string sqlzfzs = "select sum(技能) from xsb"+sjc;
            string tempzfzs = dbclass.GetOneValue(sqlzfzs);
            double dzfzs = Double.Parse(tempzfzs);

            string sqlzdfs = "";
            string tempzdfs = "";
            double dtempzdfs = 0;
            double zdfx = 0;
            string temptm = "";
            string sqlvalue = "";
            string tempvalue = "";
            double dtempvalue = 0;

            for (int i = 0; i < lBtmlist.Items.Count; i++)
            {
                //技能字段
                temptm = ((ListItem)lBtmlist.Items[i]).Value.ToString();
                sqlzdfs = "select 技能" + " from xsb"+sjc+" where 题号='" + temptm + "'";
                tempzdfs = dbclass.GetOneValue(sqlzdfs);
                dtempzdfs = Double.Parse(tempzdfs);
                
                sqlvalue = "select A.fs from (SELECT zdgxb"+sjc+".yzdmc,zdgxb"+sjc+".szdmc,ysffb"+sjc+".fs,zdgxb"+sjc+".bmc FROM zdgxb"+sjc+" INNER JOIN ysffb"+sjc+" ON zdgxb"+sjc+".szdmc = ysffb"+sjc+".tm) as A where A.bmc = 'jsb"+sjc+"' and A.yzdmc = '" + temptm + "'";
                tempvalue = dbclass.GetOneValue(sqlvalue);
                dtempvalue = Double.Parse(tempvalue);

                if (dtempzdfs == 0)
                {
                    zdfx = 0;
                }
                else
                {
                    zdfx = Math.Round(dtempzdfs * dzf / dzfzs, 2);
                }
                
                if (zdfx != 0)
                {
                    zdfx = Math.Round(zdfx / dtempvalue, 2);
                }
                sqlupdatezs[dtCout, i] = "update xsb"+sjc+" set 技能 = "+zdfx+"  where 题号 = '"+temptm+"'";
                //技能子字段
                for (int j = 0; j < dtCout; j++)
                {
                    sqlzdfs = "select " + dtzs.Rows[j][0].ToString() + " from xsb"+sjc+" where 题号='" + temptm + "'";
                    tempzdfs = dbclass.GetOneValue(sqlzdfs);
                    zdfx = Math.Round(Double.Parse(tempzdfs) * dzf / dzfzs,2);

                    if (zdfx != 0)
                    {
                        zdfx = Math.Round(zdfx / Double.Parse(tempvalue),2);
                    }
                    sqlupdatezs[j, i] = "update xsb"+sjc+" set " + dtzs.Rows[j][0].ToString() + " = "+zdfx+" where 题号 = '"+temptm+"'";
                }

            }

            try
            {
                string[] sqlupdate = new string[lBtmlist.Items.Count];
                for (int i = 0; i < dtCout + 1; i++)
                {
                    for (int j = 0; j < lBtmlist.Items.Count; j++)
                    {
                        sqlupdate[j] = sqlupdatezs[i, j];
                    }
                    dbclass.ExecNonQuerySW(sqlupdate);
                }
                dataGridView1.DataSource = dbclass.GreatDs(xsbsql+sjc).Tables[0];
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "技能系数成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("技能系数成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "技能系数失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("技能系数失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnnlxs_Click(object sender, EventArgs e)
        {
            if (cbkm.SelectedValue.ToString().Equals(""))
            {
                MessageBox.Show("请选择分类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sqlzs = "select classname from fxclass where pclassid = 10003 and fxtype='" + cbkm.SelectedValue.ToString() + "' and sjc='"+labelsjc.Text + "'";
            DataTable dtzs = dbclass.GreatDs(sqlzs).Tables[0];
            int dtCout = dtzs.Rows.Count;
            string[,] sqlupdatezs = new string[dtCout + 1, lBtmlist.Items.Count];

            //原始总分
            string sqlzf = "select sum(fs) from ysffb"+sjc;
            string tempzf = dbclass.GetOneValue(sqlzf);
            double dzf = Double.Parse(tempzf);

            //现在总分
            string sqlzfzs = "select sum(能力) from xsb"+sjc;
            string tempzfzs = dbclass.GetOneValue(sqlzfzs);
            double dzfzs = Double.Parse(tempzfzs);

            string sqlzdfs = "";
            string tempzdfs = "";
            double zdfx = 0;
            string temptm = "";
            string sqlvalue = "";
            string tempvalue = "";
            double dtempvalue = 0;

            for (int i = 0; i < lBtmlist.Items.Count; i++)
            {
                //技能字段
                temptm = ((ListItem)lBtmlist.Items[i]).Value.ToString();
                sqlzdfs = "select 能力" + " from xsb"+sjc+" where 题号='" + temptm + "'";
                tempzdfs = dbclass.GetOneValue(sqlzdfs);
                
                sqlvalue = "select A.fs from (SELECT zdgxb"+sjc+".yzdmc,zdgxb"+sjc+".szdmc,ysffb"+sjc+".fs,zdgxb"+sjc+".bmc FROM zdgxb"+sjc+" INNER JOIN ysffb"+sjc+" ON zdgxb"+sjc+".szdmc = ysffb"+sjc+".tm) as A where A.bmc = 'jsb"+sjc+"' and A.yzdmc = '" + temptm + "'";
                tempvalue = dbclass.GetOneValue(sqlvalue);
                dtempvalue = Double.Parse(tempvalue);

                zdfx = Math.Round(Double.Parse(tempzdfs) * dzf / dzfzs, 2);

                if (zdfx != 0)
                {
                    zdfx = Math.Round(zdfx / dtempvalue, 2);
                }
                sqlupdatezs[dtCout, i] = "update xsb"+sjc+" set 能力 = "+zdfx+" where 题号 = '"+temptm+"'";
                //技能子字段
                for (int j = 0; j < dtCout; j++)
                {
                    sqlzdfs = "select " + dtzs.Rows[j][0].ToString() + " from xsb"+sjc+" where 题号='" + temptm + "'";
                    tempzdfs = dbclass.GetOneValue(sqlzdfs);
                    zdfx = Math.Round(Double.Parse(tempzdfs) * dzf / dzfzs,2);

                    if (zdfx != 0)
                    {
                        zdfx = Math.Round(zdfx / Double.Parse(tempvalue),2);
                    }
                    sqlupdatezs[j, i] = "update xsb"+sjc+" set " + dtzs.Rows[j][0].ToString() + " = "+zdfx+" where 题号 = '"+temptm+"'";
                }

            }

            try
            {
                string[] sqlupdate = new string[lBtmlist.Items.Count];
                for (int i = 0; i < dtCout + 1; i++)
                {
                    for (int j = 0; j < lBtmlist.Items.Count; j++)
                    {
                        sqlupdate[j] = sqlupdatezs[i, j];
                    }
                    dbclass.ExecNonQuerySW(sqlupdate);
                }
                dataGridView1.DataSource = dbclass.GreatDs(xsbsql+sjc).Tables[0];
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "能力系数成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("能力系数成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "能力系数失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("能力系数失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                lbjdmc.Text = treeView1.SelectedNode.Text;
                lbjdbh.Text = treeView1.SelectedNode.Tag.ToString();
            }
        }

        private void btnxsdc_Click(object sender, EventArgs e)
        {
            try
            {
                ExportDataToExcel(dataGridView1, true);//导出数据
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "系数导出成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "系数导出失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
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

        private void btn_open_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbfilepath.Text = openFileDialog1.FileName;
                ArrayList nameList = GetExcelTables(tbfilepath.Text.Trim());
                ArrayList nameList2 = GetExcelTables(tbfilepath.Text.Trim());
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

        private void btnxsfg_Click(object sender, EventArgs e)
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

            

            try
            {
                dbclass.DoSql(delxsbsql+sjc);

                dtcj = dbclass.GreatDs(xsbsql + sjc).Tables[0];

                dtcj = ExcelHelper.GetDataTable(tbfilepath.Text, cbSheetlist.Text, cstart, cend, dtcj);

                dbclass.UpdateAccess(dtcj, xsbsql + sjc);

            }
            catch (Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "系数覆盖失败" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("覆盖失败！" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            dataGridView1.DataSource = dbclass.GreatDs(xsbsql + sjc).Tables[0];
            string sqlinsertlogsa = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "系数覆盖成功" + "','" + "xsb" + sjc + "','" + DateTime.Now + "')";
            dbclass.DoSql(sqlinsertlogsa);
            MessageBox.Show("覆盖成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FlForm_FormClosing(object sender, FormClosingEventArgs e)
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
              //      fm.ShowDialog();
                }
            }
        }
    }
}
