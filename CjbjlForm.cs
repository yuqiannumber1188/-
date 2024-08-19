using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cjpc.utils.dbutils;
using cjpc.utils;

namespace cjpc
{
    public partial class CjbjlForm : Form
    {
        DBClass dbclass = new DBClass();
        MainForm fm;

        public CjbjlForm(MainForm _fm)
        {
            InitializeComponent();

            fm = _fm;
            label2.Text = fm.label2.Text;
            labelsjc.Text = fm.labelsjc.Text;
           // sjc = labelsjc.Text;
           
           

            //创建表年度
            string sqlnd = "SELECT [ND_CODE],[ND_NAME]+[ND_XQ] as ND_MC FROM [xyfxxt].[dbo].[ND_INFO] ORDER BY ND_CODE DESC";    //年度分类
            DataTable dtnd = new DataTable();
            dtnd = dbclass.GreatDs(sqlnd).Tables[0];
            cbnd_c.DataSource = dtnd;
            cbnd_c.DisplayMember = "ND_MC";
            cbnd_c.ValueMember = "ND_CODE";

            //创建表课程
            string sqlkc = "SELECT [KC_CODE],[KC_NAME] FROM [xyfxxt].[dbo].[VRE_TE_KC_INFO] WHERE [TEACHER_CODE] = '" + label2.Text + "'";    //年度分类
            DataTable dtkc = new DataTable();
            dtkc = dbclass.GreatDs(sqlkc).Tables[0];
            cbkc_c.DataSource = dtkc;
            cbkc_c.DisplayMember = "KC_NAME";
            cbkc_c.ValueMember = "KC_CODE";


            
        }

        private void CjbjlForm_Load(object sender, EventArgs e)
        {
            // TODO:  这行代码将数据加载到表“xyfxxtDataSet.ND_INFO”中。您可以根据需要移动或删除它。
            //this.nD_INFOTableAdapter.Fill(this.xyfxxtDataSet.ND_INFO);
            // TODO:  这行代码将数据加载到表“xyfxxtDataSet1.XY_INFO”中。您可以根据需要移动或删除它。
            // this.xY_INFOTableAdapter.Fill(this.xyfxxtDataSet1.XY_INFO);

            //修改表

           
            cbnd_x.DataSource = null;
            cbkc_x.DataSource = null;
            cbsj_x.DataSource = null;

           

            //年度
            string sqlnd = "SELECT '' AS [ND_CODE], '' AS [ND_MC] UNION SELECT distinct([ND_CODE]),[ND_NAME] + [ND_XQ] as [ND_MC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [TEACHER_CODE] = '" + label2.Text + "' ORDER BY ND_CODE ASC ";    //年度分类
            DataTable dtnd = new DataTable();
            dtnd = dbclass.GreatDs(sqlnd).Tables[0];
            cbnd_x.DataSource = dtnd;
            cbnd_x.DisplayMember = "ND_MC";
            cbnd_x.ValueMember = "ND_CODE";

            //课程
            string sqlkc = "SELECT '' AS [KC_CODE], '' AS [KC_NAME] UNION SELECT distinct([KC_CODE]),[KC_NAME] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY KC_CODE ASC ";    //课程分类
            DataTable dtkc = new DataTable();
            dtkc = dbclass.GreatDs(sqlkc).Tables[0];
            cbkc_x.DataSource = dtkc;
            cbkc_x.DisplayMember = "KC_NAME";
            cbkc_x.ValueMember = "KC_CODE";

            //试卷
            string sqlsj = "SELECT '' AS [CJ_YSCJ_NAME], '' AS [SJC] UNION SELECT distinct([CJ_YSCJ_NAME]),[SJC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [KC_CODE] = '" + cbkc_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY SJC ASC ";    //试卷名称
            DataTable dtsj = new DataTable();
            dtsj = dbclass.GreatDs(sqlsj).Tables[0];
            cbsj_x.DataSource = dtsj;
            cbsj_x.DisplayMember = "CJ_YSCJ_NAME";
            cbsj_x.ValueMember = "SJC";

        }

       

        private void btncj_Click(object sender, System.EventArgs e)
        {
            string sjc = "_" + Toolimp.GetTimeStamp();
            labelsjc.Text = sjc;
            string yscjtable = "YSCJ" + sjc;
            string yscjname = tbsjmc.Text;
            string sccjtable = "SCCJ" + sjc;
            string sccjname = tbsjmc.Text;
            
            try
            {

                string[] strSqls = new string[11];
                strSqls[0] = DBUtils.GetSCCJSql(sccjtable);
                strSqls[1] = DBUtils.GetSCFXGSSql(sjc);
                strSqls[2] = DBUtils.GetYSCJSql(yscjtable);
                strSqls[3] = DBUtils.GetYSFFBSql(sjc);
                strSqls[4] = DBUtils.GetZDGXBSql(sjc);
                //strSqls[5] = DBUtils.DeleteZDGXB(sjc);
                strSqls[5] = DBUtils.GetXSBSql(sjc);
                strSqls[6] = DBUtils.GetJHBSql(sjc);
                strSqls[7] = DBUtils.GetJSBSql(sjc);
                strSqls[8] = DBUtils.GetPXSql(sjc);
                strSqls[9] = DBUtils.GetSCPXSql(sjc);
                strSqls[10] = DBUtils.GetBJJHSql(sjc);

                dbclass.ExecNonQuerySW(strSqls);

                string[] insertSqls = new string[30];
                insertSqls = DBUtils.InsertZDGXBYS(sjc);

                dbclass.ExecNonQuerySW(insertSqls);

                string sqlinsertreteclcj = "Insert into RE_TE_CL_CJ_INFO(TEACHER_CODE,ND_CODE,KC_CODE,CJ_YSCJ,CJ_SCCJ,SJC) values ('" + label2.Text + "','" + cbnd_c.SelectedValue.ToString() + "','" + cbkc_c.SelectedValue.ToString() + "','" + yscjtable + "','" + sccjtable + "','" + labelsjc.Text + "')";
                string sqlinsertyscj = "Insert into CJ_INFO(CJ_CODE,CJ_LX,CJ_NAME) values ('" + yscjtable + "','" + "输入表" + "','" + yscjname + "')";
                string sqlinsertsccj = "Insert into CJ_INFO(CJ_CODE,CJ_LX,CJ_NAME) values ('" + sccjtable + "','" + "输出表" + "','" + sccjname + "')";
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "创建成绩表','" + yscjtable + "','" + DateTime.Now + "')";

                string[] insertsj = new string[4];
                insertsj[0] = sqlinsertreteclcj;
                insertsj[1] = sqlinsertyscj;
                insertsj[2] = sqlinsertsccj;
                insertsj[3] = sqlinsertlog;

                dbclass.ExecNonQuerySW(insertsj);

                MessageBox.Show("创建成功Created successfully！");
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "创建成绩表失败','" + yscjtable + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                MessageBox.Show("创建失败Creationfailed！" + ex.ToString(), "Error");
                return;
            }
        }

        private void cbxy_x_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            

            //年度
            string sqlnd = "SELECT '' AS [ND_CODE], '' AS [ND_MC] UNION SELECT distinct([ND_CODE]),[ND_NAME] + [ND_XQ] AS [ND_MC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [TEACHER_CODE] = '" + label2.Text + "' ORDER BY ND_CODE ASC ";    //年度分类
            DataTable dtnd = new DataTable();
            dtnd = dbclass.GreatDs(sqlnd).Tables[0];
            cbnd_x.DataSource = dtnd;
            cbnd_x.DisplayMember = "ND_MC";
            cbnd_x.ValueMember = "ND_CODE";

            //课程
            string sqlkc = "SELECT '' AS [KC_CODE], '' AS [KC_NAME] UNION SELECT distinct([KC_CODE]),[KC_NAME] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY KC_CODE ASC ";    //课程分类
            DataTable dtkc = new DataTable();
            dtkc = dbclass.GreatDs(sqlkc).Tables[0];
            cbkc_x.DataSource = dtkc;
            cbkc_x.DisplayMember = "KC_NAME";
            cbkc_x.ValueMember = "KC_CODE";

            //试卷
            string sqlsj = "SELECT '' AS [CJ_YSCJ_NAME], '' AS [SJC] UNION SELECT distinct([CJ_YSCJ_NAME]),[SJC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [KC_CODE] = '" + cbkc_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY SJC ASC ";    //试卷名称
            DataTable dtsj = new DataTable();
            dtsj = dbclass.GreatDs(sqlsj).Tables[0];
            cbsj_x.DataSource = dtsj;
            cbsj_x.DisplayMember = "CJ_YSCJ_NAME";
            cbsj_x.ValueMember = "SJC";
        }

        private void cbbj_x_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string sqlclass = "SELECT '' AS [ND_CODE], '' AS [ND_MC] UNION SELECT distinct([ND_CODE]),[ND_NAME] + [ND_XQ] AS[ND_MC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [TEACHER_CODE] = '" + label2.Text + "' ORDER BY ND_CODE ASC ";    //年度分类
            DataTable dtclass = new DataTable();
            dtclass = dbclass.GreatDs(sqlclass).Tables[0];
            cbnd_x.DataSource = dtclass;
            cbnd_x.DisplayMember = "ND_MC";
            cbnd_x.ValueMember = "ND_CODE";

            //课程
            string sqlkc = "SELECT '' AS [KC_CODE], '' AS [KC_NAME] UNION SELECT distinct([KC_CODE]),[KC_NAME] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY KC_CODE ASC ";    //课程分类
            DataTable dtkc = new DataTable();
            dtkc = dbclass.GreatDs(sqlkc).Tables[0];
            cbkc_x.DataSource = dtkc;
            cbkc_x.DisplayMember = "KC_NAME";
            cbkc_x.ValueMember = "KC_CODE";

            //试卷
            string sqlsj = "SELECT '' AS [CJ_YSCJ_NAME], '' AS [SJC] UNION SELECT distinct([CJ_YSCJ_NAME]),[SJC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [KC_CODE] = '" + cbkc_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY SJC ASC ";    //试卷名称
            DataTable dtsj = new DataTable();
            dtsj = dbclass.GreatDs(sqlsj).Tables[0];
            cbsj_x.DataSource = dtsj;
            cbsj_x.DisplayMember = "CJ_YSCJ_NAME";
            cbsj_x.ValueMember = "SJC";
        }

        private void cbzy_x_SelectedIndexChanged(object sender, System.EventArgs e)
        {
           

            //年度
            string sqlnd = "SELECT '' AS [ND_CODE], '' AS [ND_MC] UNION SELECT distinct([ND_CODE]), [ND_NAME] + [ND_XQ] AS[ND_MC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [TEACHER_CODE] = '" + label2.Text + "' ORDER BY ND_CODE ASC ";    //年度分类
            DataTable dtnd = new DataTable();
            dtnd = dbclass.GreatDs(sqlnd).Tables[0];
            cbnd_x.DataSource = dtnd;
            cbnd_x.DisplayMember = "ND_MC";
            cbnd_x.ValueMember = "ND_CODE";

            //课程
            string sqlkc = "SELECT '' AS [KC_CODE], '' AS [KC_NAME] UNION SELECT distinct([KC_CODE]),[KC_NAME] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY KC_CODE ASC ";    //课程分类
            DataTable dtkc = new DataTable();
            dtkc = dbclass.GreatDs(sqlkc).Tables[0];
            cbkc_x.DataSource = dtkc;
            cbkc_x.DisplayMember = "KC_NAME";
            cbkc_x.ValueMember = "KC_CODE";

            //试卷
            string sqlsj = "SELECT '' AS [CJ_YSCJ_NAME], '' AS [SJC] UNION SELECT distinct([CJ_YSCJ_NAME]),[SJC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [KC_CODE] = '" + cbkc_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY SJC ASC ";    //试卷名称
            DataTable dtsj = new DataTable();
            dtsj = dbclass.GreatDs(sqlsj).Tables[0];
            cbsj_x.DataSource = dtsj;
            cbsj_x.DisplayMember = "CJ_YSCJ_NAME";
            cbsj_x.ValueMember = "SJC";
        }

        private void cbnd_x_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string sqlclass = "SELECT '' AS [KC_CODE], '' AS [KC_NAME] UNION SELECT distinct([KC_CODE]),[KC_NAME] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY KC_CODE ASC ";    //课程分类
            DataTable dtclass = new DataTable();
            dtclass = dbclass.GreatDs(sqlclass).Tables[0];
            cbkc_x.DataSource = dtclass;
            cbkc_x.DisplayMember = "KC_NAME";
            cbkc_x.ValueMember = "KC_CODE";

            //试卷
            string sqlsj = "SELECT '' AS [CJ_YSCJ_NAME], '' AS [SJC] UNION SELECT distinct([CJ_YSCJ_NAME]),[SJC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE  [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [KC_CODE] = '" + cbkc_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY SJC ASC ";    //试卷名称
            DataTable dtsj = new DataTable();
            dtsj = dbclass.GreatDs(sqlsj).Tables[0];
            cbsj_x.DataSource = dtsj;
            cbsj_x.DisplayMember = "CJ_YSCJ_NAME";
            cbsj_x.ValueMember = "SJC";
        }

        private void cbkc_x_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string sqlclass = "SELECT '' AS [CJ_YSCJ_NAME], '' AS [SJC] UNION SELECT distinct([CJ_YSCJ_NAME]),[SJC] FROM [xyfxxt].[dbo].[VRE_TE_CL_CJ_YSSC_INFO] WHERE [ND_CODE] = '" + cbnd_x.SelectedValue.ToString() + "' AND [KC_CODE] = '" + cbkc_x.SelectedValue.ToString() + "' AND [TEACHER_CODE] = '" + label2.Text + "' ORDER BY SJC ASC ";    //试卷名称
            DataTable dtclass = new DataTable();
            dtclass = dbclass.GreatDs(sqlclass).Tables[0];
            cbsj_x.DataSource = dtclass;
            cbsj_x.DisplayMember = "CJ_YSCJ_NAME";
            cbsj_x.ValueMember = "SJC";
        }

        private void cbsj_x_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            labelsjc.Text = cbsj_x.SelectedValue.ToString();
        }

        private void bntxg_Click(object sender, System.EventArgs e)
        {

            string sjc = labelsjc.Text;
            string yscjtable = "YSCJ" + sjc;
            string sccjtable = "SCCJ" + sjc;
            try
            {
                
                string[] strSqls = new string[21];
                strSqls[0] = DBUtils.DropSCCJSql(sccjtable);
                strSqls[1] = DBUtils.GetSCCJSql(sccjtable);
                strSqls[2] = DBUtils.DropSCFXGSSql(sjc);
                strSqls[3] = DBUtils.GetSCFXGSSql(sjc);
                strSqls[4] = DBUtils.DropYSCJSql(yscjtable);
                strSqls[5] = DBUtils.GetYSCJSql(yscjtable);
                strSqls[6] = DBUtils.DropYSFFBSql(sjc);
                strSqls[7] = DBUtils.GetYSFFBSql(sjc);
                strSqls[8] = DBUtils.DeleteZDGXB(sjc);
                strSqls[9] = DBUtils.DropXSBSql(sjc);
                strSqls[10] = DBUtils.GetXSBSql(sjc);
                strSqls[11] = DBUtils.DropJHBSql(sjc);
                strSqls[12] = DBUtils.GetJHBSql(sjc);
                strSqls[13] = DBUtils.DropJSBSql(sjc);
                strSqls[14] = DBUtils.GetJSBSql(sjc);
                strSqls[15] = DBUtils.DropPXSql(sjc);
                strSqls[16] = DBUtils.GetPXSql(sjc);
                strSqls[17] = DBUtils.DropSCPXSql(sjc);
                strSqls[18] = DBUtils.GetSCPXSql(sjc);
                strSqls[19] = DBUtils.DropBJJHSql(sjc);
                strSqls[20] = DBUtils.GetBJJHSql(sjc);

                dbclass.ExecNonQuerySW(strSqls);

                string[] insertSqls = new string[30];
                insertSqls = DBUtils.InsertZDGXBYS(sjc);
                dbclass.ExecNonQuerySW(insertSqls);

                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化成绩表','" + yscjtable + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);

                MessageBox.Show("初始化成功InitializedSuccessfully！");
            }
            catch (Exception ex)
            {
                string sqlinsertlog = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "初始化成绩表失败','" + yscjtable + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlog);
                
                MessageBox.Show("初始化失败InitializationFailed！" + ex.ToString(), "Error");
                return;
            }
        }

        private void CjbjlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult r = MessageBox.Show("退出本页面(ExitThisPage)?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
