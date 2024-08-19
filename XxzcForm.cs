using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cjpc.utils;
using cjpc.utils.dbutils;
using cjpc.utils.menber;

namespace cjpc
{
    public partial class XxzcForm : Form
    {
        DBClass dbclass = new DBClass();

        Teacher teacher = new Teacher();

        MainForm fm;

        public XxzcForm(MainForm _fm)
        {
            InitializeComponent();
            fm = _fm;
        }

        private void btn_yz_Click(object sender, EventArgs e)
        {
            if (tb_yzm.Text == null || "".Equals(tb_yzm.Text.Trim()))
            {
                MessageBox.Show("请输入账号Please enter your account number！", "Error");
                return;
            }

            if (tb_yzsj.Text == null || "".Equals(tb_yzsj.Text.Trim()))
            {
                MessageBox.Show("请输入密码Please enter password！", "Error");
                return;
            }

            

            //账号 
            string inputyzm = tb_yzm.Text.Trim();


            //密码
            string inputyzsj = tb_yzsj.Text.Trim();


            string psw = Toolimp.get32MD5(inputyzsj);
            object um = teacher.Login(inputyzm, psw);


            if (um != null && !"".Equals(um))
            {
                

                if (fm.Controls.ContainsKey("menuStrip1")) 
                {
                    MenuStrip menuStrip1 = (MenuStrip)fm.Controls["menuStrip1"];
                    
                    int mItemCount = menuStrip1.Items.Count;

                     for (int i = 1; i < mItemCount; i++)
                     {
                          menuStrip1.Items[i].Visible = true;
                     }
                }

                if (fm.Controls.ContainsKey("label2"))
                {
                    Label label2 = (Label)fm.Controls["label2"];
                    label2.Text = inputyzm;
                }

                try
                {
                    string ipl = Toolimp.GetLocalIP();
                    string insertlogdl = "Insert into LOG_LOGIN_INFO(USER_CODE,USER_IP,USER_LOGIN_TIME) values ('"+inputyzm+"','"+ipl+"','"+DateTime.Now+"')";
                    dbclass.DoSql(insertlogdl);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("日志出错：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                //this.Hide();
                
                MessageBox.Show("用户登录成功User login successful！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("用户不存在或密码错误User does not exist or password is incorrect！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

          
       }

        private void XxzcForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult r = MessageBox.Show("确定要关闭此页Are you sure you want to close this page?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r != DialogResult.OK)
                {


                    e.Cancel = true;
                }
                else
                {
                    this.Hide();
                    //fm.ShowDialog();
                }
            }
        }
           
    }
}
