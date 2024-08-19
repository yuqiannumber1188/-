namespace cjpc
{
    partial class FlForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cbkm = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnnlxs = new System.Windows.Forms.Button();
            this.btnjnxg = new System.Windows.Forms.Button();
            this.btnzsxs = new System.Windows.Forms.Button();
            this.btnhbnl = new System.Windows.Forms.Button();
            this.btnhbjn = new System.Windows.Forms.Button();
            this.btnhbzs = new System.Windows.Forms.Button();
            this.btnzjzs = new System.Windows.Forms.Button();
            this.lbjdbh = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbjdmc = new System.Windows.Forms.Label();
            this.btnzj = new System.Windows.Forms.Button();
            this.lBtmlist = new System.Windows.Forms.ListBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbdc = new System.Windows.Forms.TextBox();
            this.btnxsdc = new System.Windows.Forms.Button();
            this.btnxsfg = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbend = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbstart = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSheetlist = new System.Windows.Forms.ComboBox();
            this.btn_open = new System.Windows.Forms.Button();
            this.tbfilepath = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelsjc = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(8, 599);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(1252, 420);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "系数表";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(10, 32);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1611, 506);
            this.dataGridView1.TabIndex = 0;
            // 
            // cbkm
            // 
            this.cbkm.FormattingEnabled = true;
            this.cbkm.Location = new System.Drawing.Point(186, 24);
            this.cbkm.Margin = new System.Windows.Forms.Padding(4);
            this.cbkm.Name = "cbkm";
            this.cbkm.Size = new System.Drawing.Size(180, 26);
            this.cbkm.TabIndex = 20;
            this.cbkm.SelectedIndexChanged += new System.EventHandler(this.cbkm_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(66, 30);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 18);
            this.label7.TabIndex = 19;
            this.label7.Text = "选择科目：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnnlxs);
            this.groupBox1.Controls.Add(this.btnjnxg);
            this.groupBox1.Controls.Add(this.btnzsxs);
            this.groupBox1.Controls.Add(this.btnhbnl);
            this.groupBox1.Controls.Add(this.btnhbjn);
            this.groupBox1.Controls.Add(this.btnhbzs);
            this.groupBox1.Controls.Add(this.btnzjzs);
            this.groupBox1.Controls.Add(this.lbjdbh);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbjdmc);
            this.groupBox1.Controls.Add(this.btnzj);
            this.groupBox1.Controls.Add(this.lBtmlist);
            this.groupBox1.Location = new System.Drawing.Point(506, 66);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(606, 450);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择题目";
            // 
            // btnnlxs
            // 
            this.btnnlxs.Location = new System.Drawing.Point(441, 399);
            this.btnnlxs.Margin = new System.Windows.Forms.Padding(4);
            this.btnnlxs.Name = "btnnlxs";
            this.btnnlxs.Size = new System.Drawing.Size(134, 34);
            this.btnnlxs.TabIndex = 15;
            this.btnnlxs.Text = "能力相关系数";
            this.btnnlxs.UseVisualStyleBackColor = true;
            this.btnnlxs.Click += new System.EventHandler(this.btnnlxs_Click);
            // 
            // btnjnxg
            // 
            this.btnjnxg.Location = new System.Drawing.Point(438, 362);
            this.btnjnxg.Margin = new System.Windows.Forms.Padding(4);
            this.btnjnxg.Name = "btnjnxg";
            this.btnjnxg.Size = new System.Drawing.Size(134, 34);
            this.btnjnxg.TabIndex = 14;
            this.btnjnxg.Text = "技能相关系数";
            this.btnjnxg.UseVisualStyleBackColor = true;
            this.btnjnxg.Click += new System.EventHandler(this.btnjnxg_Click);
            // 
            // btnzsxs
            // 
            this.btnzsxs.Location = new System.Drawing.Point(438, 318);
            this.btnzsxs.Margin = new System.Windows.Forms.Padding(4);
            this.btnzsxs.Name = "btnzsxs";
            this.btnzsxs.Size = new System.Drawing.Size(134, 34);
            this.btnzsxs.TabIndex = 13;
            this.btnzsxs.Text = "知识相关系数";
            this.btnzsxs.UseVisualStyleBackColor = true;
            this.btnzsxs.Click += new System.EventHandler(this.btnzsxs_Click);
            // 
            // btnhbnl
            // 
            this.btnhbnl.Location = new System.Drawing.Point(441, 258);
            this.btnhbnl.Margin = new System.Windows.Forms.Padding(4);
            this.btnhbnl.Name = "btnhbnl";
            this.btnhbnl.Size = new System.Drawing.Size(134, 34);
            this.btnhbnl.TabIndex = 12;
            this.btnhbnl.Text = "合并能力";
            this.btnhbnl.UseVisualStyleBackColor = true;
            this.btnhbnl.Click += new System.EventHandler(this.btnhbnl_Click);
            // 
            // btnhbjn
            // 
            this.btnhbjn.Location = new System.Drawing.Point(441, 214);
            this.btnhbjn.Margin = new System.Windows.Forms.Padding(4);
            this.btnhbjn.Name = "btnhbjn";
            this.btnhbjn.Size = new System.Drawing.Size(134, 34);
            this.btnhbjn.TabIndex = 11;
            this.btnhbjn.Text = "合并技能";
            this.btnhbjn.UseVisualStyleBackColor = true;
            this.btnhbjn.Click += new System.EventHandler(this.btnhbjn_Click);
            // 
            // btnhbzs
            // 
            this.btnhbzs.Location = new System.Drawing.Point(441, 172);
            this.btnhbzs.Margin = new System.Windows.Forms.Padding(4);
            this.btnhbzs.Name = "btnhbzs";
            this.btnhbzs.Size = new System.Drawing.Size(134, 34);
            this.btnhbzs.TabIndex = 10;
            this.btnhbzs.Text = "合并知识";
            this.btnhbzs.UseVisualStyleBackColor = true;
            this.btnhbzs.Click += new System.EventHandler(this.btnhbzs_Click);
            // 
            // btnzjzs
            // 
            this.btnzjzs.Location = new System.Drawing.Point(441, 68);
            this.btnzjzs.Margin = new System.Windows.Forms.Padding(4);
            this.btnzjzs.Name = "btnzjzs";
            this.btnzjzs.Size = new System.Drawing.Size(134, 34);
            this.btnzjzs.TabIndex = 9;
            this.btnzjzs.Text = "增加知识系列";
            this.btnzjzs.UseVisualStyleBackColor = true;
            this.btnzjzs.Click += new System.EventHandler(this.btnzjzs_Click);
            // 
            // lbjdbh
            // 
            this.lbjdbh.AutoSize = true;
            this.lbjdbh.Location = new System.Drawing.Point(438, 26);
            this.lbjdbh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbjdbh.Name = "lbjdbh";
            this.lbjdbh.Size = new System.Drawing.Size(116, 18);
            this.lbjdbh.TabIndex = 8;
            this.lbjdbh.Text = "选中节点编号";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(327, 26);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(98, 18);
            this.label12.TabIndex = 7;
            this.label12.Text = "节点编号：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "节点名称：";
            // 
            // lbjdmc
            // 
            this.lbjdmc.AutoSize = true;
            this.lbjdmc.Location = new System.Drawing.Point(162, 26);
            this.lbjdmc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbjdmc.Name = "lbjdmc";
            this.lbjdmc.Size = new System.Drawing.Size(116, 18);
            this.lbjdmc.TabIndex = 5;
            this.lbjdmc.Text = "选中节点名称";
            // 
            // btnzj
            // 
            this.btnzj.Location = new System.Drawing.Point(441, 111);
            this.btnzj.Margin = new System.Windows.Forms.Padding(4);
            this.btnzj.Name = "btnzj";
            this.btnzj.Size = new System.Drawing.Size(134, 34);
            this.btnzj.TabIndex = 1;
            this.btnzj.Text = "增加其它系列";
            this.btnzj.UseVisualStyleBackColor = true;
            this.btnzj.Click += new System.EventHandler(this.btnzj_Click);
            // 
            // lBtmlist
            // 
            this.lBtmlist.FormattingEnabled = true;
            this.lBtmlist.ItemHeight = 18;
            this.lBtmlist.Location = new System.Drawing.Point(34, 68);
            this.lBtmlist.Margin = new System.Windows.Forms.Padding(4);
            this.lBtmlist.Name = "lBtmlist";
            this.lBtmlist.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lBtmlist.Size = new System.Drawing.Size(392, 364);
            this.lBtmlist.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(69, 66);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(366, 448);
            this.treeView1.TabIndex = 17;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbdc);
            this.groupBox3.Controls.Add(this.btnxsdc);
            this.groupBox3.Location = new System.Drawing.Point(1173, 87);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(561, 124);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "系数导出";
            // 
            // tbdc
            // 
            this.tbdc.Location = new System.Drawing.Point(22, 33);
            this.tbdc.Margin = new System.Windows.Forms.Padding(4);
            this.tbdc.Name = "tbdc";
            this.tbdc.ReadOnly = true;
            this.tbdc.Size = new System.Drawing.Size(376, 28);
            this.tbdc.TabIndex = 14;
            // 
            // btnxsdc
            // 
            this.btnxsdc.Location = new System.Drawing.Point(414, 30);
            this.btnxsdc.Margin = new System.Windows.Forms.Padding(4);
            this.btnxsdc.Name = "btnxsdc";
            this.btnxsdc.Size = new System.Drawing.Size(123, 34);
            this.btnxsdc.TabIndex = 0;
            this.btnxsdc.Text = "系数导出";
            this.btnxsdc.UseVisualStyleBackColor = true;
            this.btnxsdc.Click += new System.EventHandler(this.btnxsdc_Click);
            // 
            // btnxsfg
            // 
            this.btnxsfg.Location = new System.Drawing.Point(417, 220);
            this.btnxsfg.Margin = new System.Windows.Forms.Padding(4);
            this.btnxsfg.Name = "btnxsfg";
            this.btnxsfg.Size = new System.Drawing.Size(117, 34);
            this.btnxsfg.TabIndex = 1;
            this.btnxsfg.Text = "系数覆盖";
            this.btnxsfg.UseVisualStyleBackColor = true;
            this.btnxsfg.Click += new System.EventHandler(this.btnxsfg_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbend);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.tbstart);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.cbSheetlist);
            this.groupBox4.Controls.Add(this.btn_open);
            this.groupBox4.Controls.Add(this.tbfilepath);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.btnxsfg);
            this.groupBox4.Location = new System.Drawing.Point(1173, 238);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(561, 278);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "系数导入";
            // 
            // tbend
            // 
            this.tbend.Location = new System.Drawing.Point(464, 156);
            this.tbend.Margin = new System.Windows.Forms.Padding(4);
            this.tbend.Name = "tbend";
            this.tbend.Size = new System.Drawing.Size(48, 28);
            this.tbend.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(342, 162);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 18);
            this.label4.TabIndex = 25;
            this.label4.Text = "结束的行数：";
            // 
            // tbstart
            // 
            this.tbstart.Location = new System.Drawing.Point(238, 156);
            this.tbstart.Margin = new System.Windows.Forms.Padding(4);
            this.tbstart.Name = "tbstart";
            this.tbstart.Size = new System.Drawing.Size(48, 28);
            this.tbstart.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 164);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 18);
            this.label3.TabIndex = 23;
            this.label3.Text = "选择导入开始的行数：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(24, 105);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(125, 18);
            this.label10.TabIndex = 22;
            this.label10.Text = "选择sheet表：";
            // 
            // cbSheetlist
            // 
            this.cbSheetlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSheetlist.FormattingEnabled = true;
            this.cbSheetlist.Location = new System.Drawing.Point(159, 98);
            this.cbSheetlist.Margin = new System.Windows.Forms.Padding(4);
            this.cbSheetlist.Name = "cbSheetlist";
            this.cbSheetlist.Size = new System.Drawing.Size(175, 26);
            this.cbSheetlist.TabIndex = 21;
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(460, 40);
            this.btn_open.Margin = new System.Windows.Forms.Padding(4);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(90, 32);
            this.btn_open.TabIndex = 20;
            this.btn_open.Text = "打开";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // tbfilepath
            // 
            this.tbfilepath.Location = new System.Drawing.Point(140, 42);
            this.tbfilepath.Margin = new System.Windows.Forms.Padding(4);
            this.tbfilepath.Name = "tbfilepath";
            this.tbfilepath.ReadOnly = true;
            this.tbfilepath.Size = new System.Drawing.Size(296, 28);
            this.tbfilepath.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(18, 46);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 21);
            this.label9.TabIndex = 18;
            this.label9.Text = "导入模板：";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // labelsjc
            // 
            this.labelsjc.AutoSize = true;
            this.labelsjc.Location = new System.Drawing.Point(1608, 33);
            this.labelsjc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelsjc.Name = "labelsjc";
            this.labelsjc.Size = new System.Drawing.Size(80, 18);
            this.labelsjc.TabIndex = 25;
            this.labelsjc.Text = "labelsjc";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1482, 33);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 24;
            this.label2.Text = "10114";
            // 
            // FlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1776, 1050);
            this.Controls.Add(this.labelsjc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbkm);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.treeView1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FlForm";
            this.Text = "分类表导入";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlForm_FormClosing);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cbkm;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnnlxs;
        private System.Windows.Forms.Button btnjnxg;
        private System.Windows.Forms.Button btnzsxs;
        private System.Windows.Forms.Button btnhbnl;
        private System.Windows.Forms.Button btnhbjn;
        private System.Windows.Forms.Button btnhbzs;
        private System.Windows.Forms.Button btnzjzs;
        private System.Windows.Forms.Label lbjdbh;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbjdmc;
        private System.Windows.Forms.Button btnzj;
        private System.Windows.Forms.ListBox lBtmlist;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnxsdc;
        private System.Windows.Forms.Button btnxsfg;
        private System.Windows.Forms.TextBox tbdc;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbfilepath;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbSheetlist;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox tbend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbstart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelsjc;
        private System.Windows.Forms.Label label2;
    }
}