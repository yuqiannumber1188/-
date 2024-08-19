namespace cjpc
{
    partial class ScbscForm
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
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.plz = new System.Windows.Forms.Panel();
            this.btncsh = new System.Windows.Forms.Button();
            this.btnscbzz = new System.Windows.Forms.Button();
            this.pladd = new System.Windows.Forms.Panel();
            this.pl1 = new System.Windows.Forms.Panel();
            this.tbcd1 = new System.Windows.Forms.TextBox();
            this.lbcd1 = new System.Windows.Forms.Label();
            this.cblx1 = new System.Windows.Forms.ComboBox();
            this.lxh1 = new System.Windows.Forms.Label();
            this.lblx1 = new System.Windows.Forms.Label();
            this.tbtm1 = new System.Windows.Forms.TextBox();
            this.lbtm1 = new System.Windows.Forms.Label();
            this.btnscb = new System.Windows.Forms.Button();
            this.lbzf = new System.Windows.Forms.Label();
            this.tbadd = new System.Windows.Forms.TextBox();
            this.btnadd = new System.Windows.Forms.Button();
            this.labelsjc = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.plz.SuspendLayout();
            this.pladd.SuspendLayout();
            this.pl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(34, 804);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(1098, 21);
            this.dataGridView2.TabIndex = 13;
            this.dataGridView2.Visible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dataGridView1);
            this.groupBox4.Location = new System.Drawing.Point(34, 645);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(1107, 152);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "生成输出表";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(10, 32);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1088, 92);
            this.dataGridView1.TabIndex = 0;
            // 
            // plz
            // 
            this.plz.AutoScroll = true;
            this.plz.AutoSize = true;
            this.plz.Controls.Add(this.btncsh);
            this.plz.Controls.Add(this.btnscbzz);
            this.plz.Controls.Add(this.pladd);
            this.plz.Controls.Add(this.btnscb);
            this.plz.Controls.Add(this.lbzf);
            this.plz.Controls.Add(this.tbadd);
            this.plz.Controls.Add(this.btnadd);
            this.plz.Location = new System.Drawing.Point(34, 20);
            this.plz.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.plz.Name = "plz";
            this.plz.Size = new System.Drawing.Size(1107, 616);
            this.plz.TabIndex = 11;
            // 
            // btncsh
            // 
            this.btncsh.Location = new System.Drawing.Point(900, 14);
            this.btncsh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btncsh.Name = "btncsh";
            this.btncsh.Size = new System.Drawing.Size(168, 34);
            this.btncsh.TabIndex = 7;
            this.btncsh.Text = "初始化输出表";
            this.btncsh.UseVisualStyleBackColor = true;
            this.btncsh.Click += new System.EventHandler(this.btncsh_Click);
            // 
            // btnscbzz
            // 
            this.btnscbzz.Location = new System.Drawing.Point(704, 14);
            this.btnscbzz.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnscbzz.Name = "btnscbzz";
            this.btnscbzz.Size = new System.Drawing.Size(168, 34);
            this.btnscbzz.TabIndex = 6;
            this.btnscbzz.Text = "生成最终输出表";
            this.btnscbzz.UseVisualStyleBackColor = true;
            this.btnscbzz.Click += new System.EventHandler(this.btnscbzz_Click);
            // 
            // pladd
            // 
            this.pladd.AutoScroll = true;
            this.pladd.Controls.Add(this.pl1);
            this.pladd.Location = new System.Drawing.Point(52, 60);
            this.pladd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pladd.Name = "pladd";
            this.pladd.Size = new System.Drawing.Size(1016, 516);
            this.pladd.TabIndex = 5;
            // 
            // pl1
            // 
            this.pl1.Controls.Add(this.tbcd1);
            this.pl1.Controls.Add(this.lbcd1);
            this.pl1.Controls.Add(this.cblx1);
            this.pl1.Controls.Add(this.lxh1);
            this.pl1.Controls.Add(this.lblx1);
            this.pl1.Controls.Add(this.tbtm1);
            this.pl1.Controls.Add(this.lbtm1);
            this.pl1.Location = new System.Drawing.Point(60, 15);
            this.pl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pl1.Name = "pl1";
            this.pl1.Size = new System.Drawing.Size(900, 68);
            this.pl1.TabIndex = 0;
            // 
            // tbcd1
            // 
            this.tbcd1.Location = new System.Drawing.Point(681, 24);
            this.tbcd1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbcd1.Name = "tbcd1";
            this.tbcd1.Size = new System.Drawing.Size(40, 28);
            this.tbcd1.TabIndex = 7;
            // 
            // lbcd1
            // 
            this.lbcd1.AutoSize = true;
            this.lbcd1.Location = new System.Drawing.Point(591, 30);
            this.lbcd1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbcd1.Name = "lbcd1";
            this.lbcd1.Size = new System.Drawing.Size(62, 18);
            this.lbcd1.TabIndex = 6;
            this.lbcd1.Text = "长度：";
            // 
            // cblx1
            // 
            this.cblx1.FormattingEnabled = true;
            this.cblx1.Items.AddRange(new object[] {
            "float",
            "varchar"});
            this.cblx1.Location = new System.Drawing.Point(428, 26);
            this.cblx1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cblx1.Name = "cblx1";
            this.cblx1.Size = new System.Drawing.Size(122, 26);
            this.cblx1.TabIndex = 5;
            // 
            // lxh1
            // 
            this.lxh1.AutoSize = true;
            this.lxh1.Location = new System.Drawing.Point(822, 30);
            this.lxh1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lxh1.Name = "lxh1";
            this.lxh1.Size = new System.Drawing.Size(17, 18);
            this.lxh1.TabIndex = 4;
            this.lxh1.Text = "1";
            // 
            // lblx1
            // 
            this.lblx1.AutoSize = true;
            this.lblx1.Location = new System.Drawing.Point(360, 30);
            this.lblx1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblx1.Name = "lblx1";
            this.lblx1.Size = new System.Drawing.Size(62, 18);
            this.lblx1.TabIndex = 2;
            this.lblx1.Text = "类型：";
            // 
            // tbtm1
            // 
            this.tbtm1.Location = new System.Drawing.Point(165, 24);
            this.tbtm1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbtm1.Name = "tbtm1";
            this.tbtm1.Size = new System.Drawing.Size(148, 28);
            this.tbtm1.TabIndex = 1;
            // 
            // lbtm1
            // 
            this.lbtm1.AutoSize = true;
            this.lbtm1.Location = new System.Drawing.Point(45, 30);
            this.lbtm1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbtm1.Name = "lbtm1";
            this.lbtm1.Size = new System.Drawing.Size(98, 18);
            this.lbtm1.TabIndex = 0;
            this.lbtm1.Text = "字段名称：";
            // 
            // btnscb
            // 
            this.btnscb.Location = new System.Drawing.Point(508, 14);
            this.btnscb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnscb.Name = "btnscb";
            this.btnscb.Size = new System.Drawing.Size(168, 34);
            this.btnscb.TabIndex = 2;
            this.btnscb.Text = "生成原始输出表";
            this.btnscb.UseVisualStyleBackColor = true;
            this.btnscb.Click += new System.EventHandler(this.btnscb_Click);
            // 
            // lbzf
            // 
            this.lbzf.AutoSize = true;
            this.lbzf.Location = new System.Drawing.Point(94, 22);
            this.lbzf.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbzf.Name = "lbzf";
            this.lbzf.Size = new System.Drawing.Size(62, 18);
            this.lbzf.TabIndex = 3;
            this.lbzf.Text = "个数：";
            // 
            // tbadd
            // 
            this.tbadd.Location = new System.Drawing.Point(174, 16);
            this.tbadd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbadd.Name = "tbadd";
            this.tbadd.Size = new System.Drawing.Size(110, 28);
            this.tbadd.TabIndex = 2;
            // 
            // btnadd
            // 
            this.btnadd.Location = new System.Drawing.Point(332, 14);
            this.btnadd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnadd.Name = "btnadd";
            this.btnadd.Size = new System.Drawing.Size(112, 34);
            this.btnadd.TabIndex = 1;
            this.btnadd.Text = "增加字段";
            this.btnadd.UseVisualStyleBackColor = true;
            this.btnadd.Click += new System.EventHandler(this.btnadd_Click);
            // 
            // labelsjc
            // 
            this.labelsjc.AutoSize = true;
            this.labelsjc.Location = new System.Drawing.Point(970, 3);
            this.labelsjc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelsjc.Name = "labelsjc";
            this.labelsjc.Size = new System.Drawing.Size(80, 18);
            this.labelsjc.TabIndex = 28;
            this.labelsjc.Text = "labelsjc";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(880, 2);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 29;
            this.label2.Text = "10114";
            // 
            // ScbscForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 843);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.plz);
            this.Controls.Add(this.labelsjc);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "ScbscForm";
            this.Text = "输出表生成Output table generation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScbscForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.plz.ResumeLayout(false);
            this.plz.PerformLayout();
            this.pladd.ResumeLayout(false);
            this.pl1.ResumeLayout(false);
            this.pl1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel plz;
        private System.Windows.Forms.Button btncsh;
        private System.Windows.Forms.Button btnscbzz;
        private System.Windows.Forms.Panel pladd;
        private System.Windows.Forms.Panel pl1;
        private System.Windows.Forms.TextBox tbcd1;
        private System.Windows.Forms.Label lbcd1;
        private System.Windows.Forms.ComboBox cblx1;
        private System.Windows.Forms.Label lxh1;
        private System.Windows.Forms.Label lblx1;
        private System.Windows.Forms.TextBox tbtm1;
        private System.Windows.Forms.Label lbtm1;
        private System.Windows.Forms.Button btnscb;
        private System.Windows.Forms.Label lbzf;
        private System.Windows.Forms.TextBox tbadd;
        private System.Windows.Forms.Button btnadd;
        private System.Windows.Forms.Label labelsjc;
        private System.Windows.Forms.Label label2;

    }
}