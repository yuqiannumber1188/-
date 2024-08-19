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
using cjpc.utils;
using cjpc.utils.dbutils;
using System.Text.RegularExpressions;

namespace cjpc
{
    public partial class SysfxForm : Form
    {
        DBClass dbclass = new DBClass();
        List<string> treeNodes = new List<string>();
        string xsbsql = "select * from xsb";

        string sjc;
        MainForm fm;

        public SysfxForm(MainForm _fm)
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

            dataGridView1.DataSource = dbclass.GreatDs(xsbsql + sjc).Tables[0];
            InitTree("all");
         //   InitSxList("all");
        }

        private void InitTree(string tj)
        {
            treeView1.Nodes.Clear();
           string sqltree_Department = "select classid,classname,pclassid from fxclass where fxtype='all' or (fxtype='" + tj + "' and sjc='"+ labelsjc.Text+"')";
            // string sqltree_Department = "select classid,classname,pclassid from fxclass where (fxtype='all' or fxtype='" + tj + "')";

            DataTable dt = dbclass.GreatDs(sqltree_Department).Tables[0];
            DataView dv = new DataView(dt);
            dv.RowFilter = "pclassid=0";
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

        private void InitSxList(string tj)
        {

            cbdsx.DataSource = null;
            cbdsx.Items.Clear();
            string sqlsx = "SELECT '' as classid,'' as classname from fxclass union select classid,classname from fxclass where pclassid=0 and (fxtype='all' or fxtype='" + tj + "')";
           

            DataTable dt = dbclass.GreatDs(sqlsx).Tables[0];

            //List<ListItem> list = new List<ListItem>();

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    list.Add(new ListItem(dt.Rows[i]["classid"].ToString(), dt.Rows[i]["classname"].ToString()));
            //}

            cbdsx.DataSource = dt;
            cbdsx.DisplayMember = "classname";
            cbdsx.ValueMember = "classid";  
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
               // lbjdmc.Text = treeView1.SelectedNode.Text;
                lbjdsx.Text = treeView1.SelectedNode.Text;
              //  lbjdbh.Text = treeView1.SelectedNode.Tag.ToString();
                lbjdbm.Text = treeView1.SelectedNode.Tag.ToString();
            }
        }

        private void cbkm_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitTree(cbkm.SelectedValue.ToString());
            InitSxList(cbkm.SelectedValue.ToString());
        }

        private void btncsh_Click(object sender, EventArgs e)
        {
            try
            {
                string[] strSqls = new string[4];
                strSqls[0] = DBUtils.DropXSBSql(sjc);
                strSqls[1] = DBUtils.GetXSBSql(sjc);
                strSqls[2] = DBUtils.DropJHBSql(sjc);
                strSqls[3] = DBUtils.GetJHBSql(sjc);

                dbclass.ExecNonQuerySW(strSqls);
                dataGridView1.DataSource = dbclass.GreatDs(xsbsql + sjc).Tables[0];

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化成功" +  "','" + "xsb、jhb" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("初始化成功！");
            }
            catch(Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化失败" + "','" + "xsb、jhb" + sjc +":"+ex.Message +"','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("初始化失败！"+ex.Message, "Error");
                return;
            }
        }

        private void btnscxsb_Click(object sender, EventArgs e)
        {
            #region 递归
            //1.获取TreeView的所有根节点
            treeNodes.Clear();

            foreach (TreeNode tn in treeView1.Nodes)
            {
                treeNodes.Add(tn.Text);
            }

            foreach (TreeNode tn in treeView1.Nodes)
            {
                DiGui(tn);
            }

            #endregion

            int count = treeNodes.Count;
            string[] zdmc = new string[count];
            string[] zdlx = new string[count];
            string[] zddx = new string[count];
            string[] sqlalters = new string[count];
            string[] sqlaltersjhb = new string[count];
            string[] sqlupdatexsb = new string[count];
            string pattern2 = @"[a-zA-Z0-9\s]+";

            for (int i = 0; i < treeNodes.Count; i++)
            {
                zdmc[i] = Regex.Split(treeNodes[i].ToString(), pattern2)[0]; 
                zdlx[i] = "decimal(18, 3)";
                zddx[i] = "0";
            }

            sqlalters = DBUtils.GetAlterTableSql("xsb"+sjc, count, zdmc, zdlx, zddx);
            sqlaltersjhb = DBUtils.GetAlterTableSql("jhb"+sjc, count, zdmc, zdlx, zddx);
            sqlupdatexsb = DBUtils.UpdateTableSql("xsb" + sjc, count, zdmc);

            //插入题目
            string jsbsql = "select * from jsb"+sjc;
            DataTable jsbdt = new DataTable();
            jsbdt = dbclass.GreatDs(jsbsql).Tables[0];
            int countc = jsbdt.Columns.Count;
            int countz = countc - 3;
            string[] sqlinsert = new string[countz];
            for (int i = 0; i < countz; i++)
            {
                sqlinsert[i] = "Insert into xsb"+sjc+"(题号) values ('"+jsbdt.Columns[i + 3].ColumnName+"')";
            }

            try
            {
                dbclass.ExecNonQuerySW(sqlalters);
                dbclass.ExecNonQuerySW(sqlaltersjhb);
                dbclass.ExecNonQuerySW(sqlinsert);
                dbclass.ExecNonQuerySW(sqlupdatexsb);


            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "创建失败" + "','" + "xsb、jhb" + sjc + ":" + ex.Message + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("创建失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dataGridView1.DataSource = dbclass.GreatDs(xsbsql+ sjc).Tables[0];

            string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "创建成功" + "','" + "xsb、jhb" + sjc + "','" + DateTime.Now + "')";
            dbclass.DoSql(sqlinsertlogs);

            MessageBox.Show("创建成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DiGui(TreeNode tn)
        {
            //1.将当前节点显示到lable上
            foreach (TreeNode tnSub in tn.Nodes)
            {
                DiGui(tnSub);
                treeNodes.Add(tnSub.Text);
            }
        }

        private void btnaddbr_Click(object sender, EventArgs e)
        {
            try
            {
                int bh = Convert.ToInt32(lbjdbh.Text);
                tbjdbh.Text = Toolimp.GetBrothersClassID(bh).ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("获取节点编号失败，请重新获取！"+ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnaddch_Click(object sender, EventArgs e)
        {
            try
            {
                int bh = Convert.ToInt32(lbjdbh.Text);
                tbjdbh.Text = Toolimp.GetChildClassID(bh).ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("获取节点编号失败，请重新获取！"+ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            string fxtype = "";
            int bh = 0;
            int pbh = 0;
            if ("".Equals(tbjdmc.Text.Trim()) || "".Equals(tbjdbh.Text.Trim()))
            {
                tbjdmc.Focus();
                MessageBox.Show("请填写节点名称和编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                if (lbjdbh.Text.Substring(0, 1).Equals(tbjdbh.Text.Substring(0, 1)))
                {
                    string pidsql = "select pclassid from fxzclass where classid = " + lbjdbh.Text.Trim();
                    string pids = dbclass.GetOneValue(pidsql);
                    try
                    {
                        bh = Convert.ToInt32(tbjdbh.Text);
                        pbh = Convert.ToInt32(pids);
                    }
                    catch
                    {
                        MessageBox.Show("类型转换错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        bh = Convert.ToInt32(tbjdbh.Text);
                        pbh = Convert.ToInt32(lbjdbh.Text);
                    }
                    catch
                    {
                        MessageBox.Show("类型转换错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if ("".Equals(cbkm.SelectedValue.ToString()))
                {
                    fxtype = "all";
                }
                else
                {
                    fxtype = cbkm.SelectedValue.ToString();
                }

                string sqlinsert = "Insert into fxzclass(classid,classname,pclassid,fxtype,jssm) values (" + bh + ",'" + tbjdmc.Text.Trim() + "'," + pbh + ",'" + fxtype + "','" + tbjssm.Text + "')";

                dbclass.DoSql(sqlinsert);

               // InitTree(fxtype);
                listsx.DataSource = null;
                listsx.Items.Clear();
                string tx = cbdsx.SelectedValue.ToString();
                InitListsx(tx);

                MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "节点保存成功" + bh.ToString() + "','" + "fxzclass" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                tbjdbh.Text = "";
                tbjdmc.Text = "";
            }
        }

        private void btnxg_Click(object sender, EventArgs e)
        {
            if ("".Equals(tbxjdmc.Text.Trim()) || "".Equals(lbjdbh.Text.Trim()))
            {
                tbxjdmc.Focus();
                MessageBox.Show("请选择要修改的节点，并填写新的节点名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                int bh = 0;
                try
                {
                    bh = Convert.ToInt32(lbjdbh.Text);
                }
                catch
                {
                    MessageBox.Show("类型转换错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string sqlupdate = "update fxzclass set classname = '"+tbxjdmc.Text.Trim()+"'  where classid = "+bh;
                dbclass.DoSql(sqlupdate);
                //InitTree(cbkm.Text);
                listsx.DataSource = null;
                listsx.Items.Clear();
                string tx = cbdsx.SelectedValue.ToString();
                InitListsx(tx);

                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "节点修改成功" + bh.ToString() + "','" + "fxzclass" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);

                tbxjdmc.Text = "";

            }
        }

        private void btnsc_Click(object sender, EventArgs e)
        {
            if ("".Equals(lbjdbh.Text.Trim()))
            {
                MessageBox.Show("请选择要删除的节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                int bh = 0;
                try
                {
                    bh = Convert.ToInt32(lbjdbh.Text);
                }
                catch
                {
                    MessageBox.Show("类型转换错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string sqldel = "delete from fxzclass where classid = "+ bh;
                dbclass.DoSql(sqldel);
               // InitTree(cbkm.Text);
                listsx.DataSource = null;
                listsx.Items.Clear();
                string tx = cbdsx.SelectedValue.ToString();
                InitListsx(tx);

                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "节点删除成功" + bh.ToString() + "','" + "fxzclass" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbdsx_SelectedIndexChanged(object sender, EventArgs e)
        {
            listsx.DataSource = null;
            listsx.Items.Clear();
            try
            {
                
                string tx = cbdsx.SelectedValue.ToString();
                string txt = cbdsx.Text;
                lbjdmc.Text = txt;
                lbjdbh.Text = tx;
                InitListsx(tx);
            }
           catch
            {
                return;
            }
        }

        private void InitListsx(string tx)
        {
            
            if (tx != null && !tx.Equals("") && !tx.Equals("System.Data.DataRowView"))
            {
                string fxt = cbkm.SelectedValue.ToString();
                string listxssql = "";
                if ("".Equals(fxt))
                {
                    listxssql = "select classid,classname from fxzclass where pclassid = " + tx + " and fxtype = 'all'";
                }
                else
                {
                    listxssql = "select classid,classname from fxzclass where pclassid = " + tx + " and fxtype = '" + fxt + "'";
                }

                DataTable dt = dbclass.GreatDs(listxssql).Tables[0];
                listsx.DisplayMember = "Text";

                listsx.ValueMember = "Value";

                List<ListItem> list = new List<ListItem>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new ListItem(dt.Rows[i]["classname"].ToString(), dt.Rows[i]["classid"].ToString()));
                }

                listsx.DataSource = list;
            }
        }

        private void btnscsx_Click(object sender, EventArgs e)
        {
            if ("".Equals(lbjdbm.Text.Trim()))
            {
                MessageBox.Show("请选择要删除的节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                int bh = 0;
                try
                {
                    bh = Convert.ToInt32(lbjdbh.Text);
                }
                catch(Exception ex)
                {
                    string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "类型转换错误" + "','" + "fxclass" + sjc + ex.Message + "','" + DateTime.Now + "')";
                    dbclass.DoSql(sqlinsertlog);
                    
                    MessageBox.Show("类型转换错误！"+ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string sqldel = "delete from fxclass where sjc='"+labelsjc.Text+"' and classid = "+ bh;
                dbclass.DoSql(sqldel);
                InitTree(cbkm.SelectedValue.ToString());
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "节点删除成功" + bh.ToString() + "','" + "fxclass" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnzjsx_Click(object sender, EventArgs e)
        {
            try
            {
                int bh = Convert.ToInt32(cbdsx.SelectedValue.ToString());
                string zbh = "";
                string cname = "";
                string sqlzr = "";
                string jssm = "";
                string fxtype = cbkm.SelectedValue.ToString();
             //   int[] id = new int[listsx.SelectedIndices.Count];

                for (int i = 0; i < listsx.SelectedItems.Count; i++)
                {
                   // id[i] = (int)listsx.SelectedIndices[i];
                    cname = ((ListItem)(listsx.SelectedItems[i])).Text;
                    zbh = Toolimp.GetChildClassID(bh).ToString();

                    jssm = dbclass.GetOneValue("select JSSM from fxzclass where classname='" + cname + "'");
                    sqlzr = "Insert into fxclass(classid,classname,pclassid,fxtype,jssm,sjc) values (" + zbh + ",'" + cname + "'," + bh + ",'" + fxtype + "','" + jssm + "','" + labelsjc.Text + "')";
                    dbclass.DoSql(sqlzr);
                }

                InitTree(fxtype);

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "添加节点成功" + cname + "','" + "fxclass" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
            }
            catch(Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "添加节点失败" + "','" + "fxclass" + sjc+"','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("获取节点编号失败，请重新获取！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnqksx_Click(object sender, EventArgs e)
        {
            try
            {
                int bh = Convert.ToInt32(cbdsx.SelectedValue.ToString());
                string sqlzr = "DELETE FROM fxclass where pclassid = "+ bh;
                string fxtype = cbkm.SelectedValue.ToString();
                dbclass.DoSql(sqlzr);
                InitTree(fxtype);
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "清空节点成功" + bh.ToString() + "','" + "fxclass" + sjc + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
            }
            catch(Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "清空节点失败" + "','" + "fxclass" + sjc + ex.Message + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("清空节点失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void listsx_MouseClick(object sender, MouseEventArgs e)
        {
            if (listsx.SelectedItems.Count != 0)
            {
                // lbjdmc.Text = treeView1.SelectedNode.Text;
                lbjdmc.Text = ((ListItem)(listsx.SelectedItem)).Text; 
                //  lbjdbh.Text = treeView1.SelectedNode.Tag.ToString();
                lbjdbh.Text = ((ListItem)(listsx.SelectedItem)).Value.ToString(); 
            }
        }

        private void btnmaxnum_Click(object sender, EventArgs e)
        {
            try
            {
                tbmaxnum.Text = Toolimp.GetMaxNum().ToString();
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "获取节点编号成功" + "','" + tbmaxnum.Text + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
            }
            catch(Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "获取节点编号失败" + "','"+ ex.Message + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("获取节点编号失败，请重新获取！"+ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string fxclasssql = "select * from fxclass";
            string delfxclasssql = "delete from fxclass";
            
            int cstart = 0;
            int cend = 0;

            try
            {
                cstart = Convert.ToInt32(tbstart.Text.Trim());
                cend = Convert.ToInt32(tbend.Text.Trim());
            }
            catch(Exception ex1)
            {
                MessageBox.Show("请输入整数！"+ex1.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cstart > cend)
            {
                MessageBox.Show("结束行数应大于起始行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dtcj = new DataTable();

             dtcj = dbclass.GreatDs(fxclasssql).Tables[0];

            try
            {
                dtcj = ExcelHelper.GetDataTable(tbfilepath.Text, cbSheetlist.Text, cstart, cend, dtcj);

                dbclass.DoSql(delfxclasssql);

                dbclass.UpdateAccess(dtcj, fxclasssql);
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导入失败" + "','" + "fxclass" + sjc + ex.Message + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("导入失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string fxtype = cbkm.SelectedValue.ToString();
            InitTree(fxtype);

            string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "导入成功" + "','" + "fxclass" + sjc + "','" + DateTime.Now + "')";
            dbclass.DoSql(sqlinsertlogs);

            MessageBox.Show("导入成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SysfxForm_FormClosing(object sender, FormClosingEventArgs e)
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
               //     fm.ShowDialog();
                    fm.labelsjc.Text = this.labelsjc.Text;
                }
            }
        }
    }
}
