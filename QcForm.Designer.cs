namespace cjpc
{
    partial class QcForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.labelsjc = new System.Windows.Forms.Label();
            this.cbkm = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnhbzs = new System.Windows.Forms.Button();
            this.btnzjzs = new System.Windows.Forms.Button();
            this.lbjdbh = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbjdmc = new System.Windows.Forms.Label();
            this.btnzj = new System.Windows.Forms.Button();
            this.lBtmlist = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnmfz = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(849, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 25;
            this.label2.Text = "10114";
            // 
            // labelsjc
            // 
            this.labelsjc.AutoSize = true;
            this.labelsjc.Location = new System.Drawing.Point(989, 9);
            this.labelsjc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelsjc.Name = "labelsjc";
            this.labelsjc.Size = new System.Drawing.Size(80, 18);
            this.labelsjc.TabIndex = 26;
            this.labelsjc.Text = "labelsjc";
            // 
            // cbkm
            // 
            this.cbkm.FormattingEnabled = true;
            this.cbkm.Location = new System.Drawing.Point(148, 47);
            this.cbkm.Margin = new System.Windows.Forms.Padding(4);
            this.cbkm.Name = "cbkm";
            this.cbkm.Size = new System.Drawing.Size(180, 26);
            this.cbkm.TabIndex = 29;
            this.cbkm.SelectedIndexChanged += new System.EventHandler(this.cbkm_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 37);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 36);
            this.label7.TabIndex = 28;
            this.label7.Text = "选择科目：\r\nSelect subject\r\n";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(31, 89);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(366, 448);
            this.treeView1.TabIndex = 27;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnhbzs);
            this.groupBox1.Controls.Add(this.btnzjzs);
            this.groupBox1.Controls.Add(this.lbjdbh);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbjdmc);
            this.groupBox1.Controls.Add(this.btnzj);
            this.groupBox1.Controls.Add(this.lBtmlist);
            this.groupBox1.Location = new System.Drawing.Point(452, 53);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(606, 418);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择题目Select topic";
            // 
            // btnhbzs
            // 
            this.btnhbzs.Location = new System.Drawing.Point(441, 336);
            this.btnhbzs.Margin = new System.Windows.Forms.Padding(4);
            this.btnhbzs.Name = "btnhbzs";
            this.btnhbzs.Size = new System.Drawing.Size(134, 34);
            this.btnhbzs.TabIndex = 10;
            this.btnhbzs.Text = "熵权法EWM";
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
            this.btnzjzs.Text = "增加add";
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
            this.label12.Size = new System.Drawing.Size(98, 36);
            this.label12.TabIndex = 7;
            this.label12.Text = "节点编号：\r\nnumber";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 36);
            this.label1.TabIndex = 6;
            this.label1.Text = "节点名称：\r\nnode name";
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
            this.btnzj.Text = "删除delete";
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
            this.lBtmlist.Size = new System.Drawing.Size(391, 328);
            this.lBtmlist.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(31, 557);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1092, 274);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "权重值WeightValue";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(32, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.Size = new System.Drawing.Size(1032, 216);
            this.dataGridView1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnmfz);
            this.groupBox3.Location = new System.Drawing.Point(443, 479);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(615, 88);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "生成层次分析-熵权法满分值系数Generate AHP-EWM full score coefficient";
            // 
            // btnmfz
            // 
            this.btnmfz.Location = new System.Drawing.Point(22, 40);
            this.btnmfz.Name = "btnmfz";
            this.btnmfz.Size = new System.Drawing.Size(580, 38);
            this.btnmfz.TabIndex = 0;
            this.btnmfz.Text = "层次分析-熵权法满分值系数AHP-EWM full score coefficient";
            this.btnmfz.UseVisualStyleBackColor = true;
            this.btnmfz.Click += new System.EventHandler(this.btnmfz_Click);
            // 
            // QcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 843);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbkm);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.labelsjc);
            this.Controls.Add(this.label2);
            this.Name = "QcForm";
            this.Text = "题目分类QuestionClassificationForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelsjc;
        private System.Windows.Forms.ComboBox cbkm;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnhbzs;
        private System.Windows.Forms.Button btnzjzs;
        private System.Windows.Forms.Label lbjdbh;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbjdmc;
        private System.Windows.Forms.Button btnzj;
        private System.Windows.Forms.ListBox lBtmlist;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnmfz;
    }
}