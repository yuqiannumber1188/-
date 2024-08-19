namespace cjpc
{
    partial class XxzcForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_yz = new System.Windows.Forms.Button();
            this.tb_yzsj = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_yzm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_yz
            // 
            this.btn_yz.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_yz.Location = new System.Drawing.Point(672, 513);
            this.btn_yz.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_yz.Name = "btn_yz";
            this.btn_yz.Size = new System.Drawing.Size(307, 75);
            this.btn_yz.TabIndex = 9;
            this.btn_yz.Text = "登陆（Logon）";
            this.btn_yz.UseVisualStyleBackColor = true;
            this.btn_yz.Click += new System.EventHandler(this.btn_yz_Click);
            // 
            // tb_yzsj
            // 
            this.tb_yzsj.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_yzsj.Location = new System.Drawing.Point(646, 286);
            this.tb_yzsj.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tb_yzsj.Name = "tb_yzsj";
            this.tb_yzsj.PasswordChar = '*';
            this.tb_yzsj.Size = new System.Drawing.Size(332, 53);
            this.tb_yzsj.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(117, 302);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(377, 40);
            this.label2.TabIndex = 7;
            this.label2.Text = "密码（password）：";
            // 
            // tb_yzm
            // 
            this.tb_yzm.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_yzm.Location = new System.Drawing.Point(646, 102);
            this.tb_yzm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tb_yzm.Name = "tb_yzm";
            this.tb_yzm.Size = new System.Drawing.Size(333, 53);
            this.tb_yzm.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 115);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(597, 40);
            this.label1.TabIndex = 5;
            this.label1.Text = "教师账号（Teacher account）：";
            // 
            // XxzcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 693);
            this.Controls.Add(this.btn_yz);
            this.Controls.Add(this.tb_yzsj);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_yzm);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "XxzcForm";
            this.Text = "账号登陆（AccountLlogin）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.XxzcForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_yz;
        private System.Windows.Forms.TextBox tb_yzsj;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_yzm;
        private System.Windows.Forms.Label label1;
    }
}