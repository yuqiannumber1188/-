using System;
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
    public partial class MainForm : Form
    {
        DBClass dbclass = new DBClass();
        
        public MainForm()
        {
            InitializeComponent();

            int mItemCount = menuStrip1.Items.Count;

            for (int i = 1; i < mItemCount; i++)
            {
                menuStrip1.Items[i].Visible = false;
            }
        }

        private void 输入注册码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XxzcForm xxzcForm = new XxzcForm(this);
            xxzcForm.Show();
            //this.Hide();
        }

        private void 初始化数据表ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                string[] strSqls = new string[15];
                strSqls[0] = DBUtils.DropSCCJSql();
                strSqls[1] = DBUtils.GetSCCJSql();
                strSqls[2] = DBUtils.DropSCFXGSSql();
                strSqls[3] = DBUtils.GetSCFXGSSql();
                strSqls[4] = DBUtils.DropYSCJSql();
                strSqls[5] = DBUtils.GetYSCJSql();
                strSqls[6] = DBUtils.DropYSFFBSql();
                strSqls[7] = DBUtils.GetYSFFBSql();
                strSqls[8] = DBUtils.DeleteZDGXB();
                strSqls[9] = DBUtils.DropXSBSql();
                strSqls[10] = DBUtils.GetXSBSql();
                strSqls[11] = DBUtils.DropJHBSql();
                strSqls[12] = DBUtils.GetJHBSql();
                strSqls[13] = DBUtils.DropJSBSql();
                strSqls[14] = DBUtils.GetJSBSql();

                dbclass.ExecNonQuerySW(strSqls);

                string[] insertSqls = new string[24];
                insertSqls = DBUtils.InsertZDGXBYS();

                dbclass.ExecNonQuerySW(insertSqls);

                MessageBox.Show("初始化成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败！" + ex.ToString(), "Error");
                return;
            }
        }

        private void 生成成绩输入表ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SccjsrForm sccjsrForm = new SccjsrForm(this);
            sccjsrForm.Show();
           // this.Hide();
        }

        private void 初始成绩导入ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            CscjdrForm cscjdrForm = new CscjdrForm(this);
            cscjdrForm.Show();
            //this.Hide();
        }

        private void 三元素分项生成ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SysfxForm sysfxForm = new SysfxForm(this);
            sysfxForm.Show();
            //this.Hide();
        }

        private void 分类表导入ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            FlForm fbForm = new FlForm(this);
            fbForm.Show();
           // this.Hide();
        }

        private void 输出表计算ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ScbscForm scbscForm = new ScbscForm(this);
            scbscForm.Show();
            //this.Hide();
        }

        private void 数据计算ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SjjsForm sjjsForm = new SjjsForm(this);
            sjjsForm.Show();
           // this.Hide();
        }

        private void 扩展数据计算ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            FzsjjsForm kzsjjsForm = new FzsjjsForm(this);
            kzsjjsForm.Show();
          //  this.Hide();
        }

        private void 输出表输出ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ScbscjsForm scbscjsForm = new ScbscjsForm(this);
            scbscjsForm.Show();
          //  this.Hide();
        }

        private void 输出表赋值ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ScbfzForm scbfzForm = new ScbfzForm(this);
            scbfzForm.Show();
          //  this.Hide();
        }

        private void toolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            CjbjlForm cjbjlForm = new CjbjlForm(this);
            //if (cjbjlForm.Controls.ContainsKey("label2"))
            //{
            //    Label label2 = (Label)cjbjlForm.Controls["label2"];
            //    label2.Text = this.label2.Text;
            //}
            cjbjlForm.Show();
          //  this.Hide();
        }

        private void 导出dbfToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            DataTable dt = new DataTable();// 这里是取数据的步骤，  
            string sqlxx = "select * from LOG_LOGIN_INFO";
            dt = dbclass.GreatDs(sqlxx).Tables[0];
            dt.TableName = "ceshi";// 必须有表名  
            DbfExportHelper helper = new DbfExportHelper(@"E:\download");
            helper.CreateNewTable(dt);
            helper.fillData(dt);  
        }

        private void 班级数据计算ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            BjsjjsForm bjsjjsForm = new BjsjjsForm(this);
            bjsjjsForm.Show();
           // this.Hide();
        }

        private void 生成成绩单ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string sqlxh = "select xh from SCCJ" + labelsjc.Text;
            DataTable xhdt = new DataTable();
            xhdt = dbclass.GreatDs(sqlxh).Tables[0];
            int count = xhdt.Rows.Count;

            string[] strSqls = new string[count];

            //STUDENT_CJB_INFO 是否存在
            string sqlstucjbinfo = "select * from STUDENT_CJB_INFO where STUDENT_CODE='" + xhdt.Rows[0][0].ToString() + "' AND  TIMESTAMP='" + labelsjc.Text + "'";
            DataTable stucjbinfodt = new DataTable();
            stucjbinfodt = dbclass.GreatDs(sqlstucjbinfo).Tables[0];
            if(stucjbinfodt.Rows.Count > 0)
            {
                MessageBox.Show("成绩单已生成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                strSqls[i] = "insert into STUDENT_CJB_INFO (STUDENT_CODE, TIMESTAMP) VALUES ('" + xhdt.Rows[i][0].ToString() + "','" + labelsjc.Text + "')";
            }
            try
            {   
                dbclass.ExecNonQuerySW(strSqls);
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "成绩单生成成功" + "','" + "jhb" + labelsjc.Text + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("成绩单生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                string sqlinsertlogs = "Insert into LOG_OPERATION_INFO(USER_CODE,USER_OPERATION,USER_OPERATION_CONTENT,USER_OPERATION_TIME) values ('" + label2.Text + "','" + "成绩单生成失败" + "','" + "jhb" + labelsjc.Text + "','" + DateTime.Now + "')";
                dbclass.DoSql(sqlinsertlogs);
                MessageBox.Show("成绩单生成失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AHP_THREE_Form sysfxForm = new AHP_THREE_Form(this);
            sysfxForm.Show();
        }


        private void AHP_Knowledge_Click(object sender, EventArgs e)
        {
            KnowledgeForm klform = new KnowledgeForm(this);
            klform.Show();
        }

        private void AHP_Skill_Click(object sender, EventArgs e)
        {
            SkillForm skillform = new SkillForm(this);
            skillform.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            AbilityForm abilityform = new AbilityForm(this);
            abilityform.Show();
        }

        private void Questionclassification_Click(object sender, EventArgs e)
        {
            QcForm qcform = new QcForm(this);
            qcform.Show();
        }
    }
}
