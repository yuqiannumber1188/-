namespace cjpc
{
    partial class BjsjjsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btncsh = new System.Windows.Forms.Button();
            this.btn = new System.Windows.Forms.Button();
            this.btnkcjh = new System.Windows.Forms.Button();
            this.labelsjc = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btncsh);
            this.groupBox1.Controls.Add(this.btn);
            this.groupBox1.Controls.Add(this.btnkcjh);
            this.groupBox1.Controls.Add(this.labelsjc);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(22, 16);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1131, 168);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "班级数据计算（ClassDataCalculation）";
            // 
            // btncsh
            // 
            this.btncsh.Location = new System.Drawing.Point(688, 62);
            this.btncsh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btncsh.Name = "btncsh";
            this.btncsh.Size = new System.Drawing.Size(295, 67);
            this.btncsh.TabIndex = 30;
            this.btncsh.Text = "初始化聚合计算表\r\nInitialize aggregation calculation table";
            this.btncsh.UseVisualStyleBackColor = true;
            this.btncsh.Click += new System.EventHandler(this.btncsh_Click);
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(277, 62);
            this.btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(387, 67);
            this.btn.TabIndex = 29;
            this.btn.Text = "班级专业数据集合计算（可选）\r\nCalculation of class professional data set (optional)";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnkcjh
            // 
            this.btnkcjh.Location = new System.Drawing.Point(21, 62);
            this.btnkcjh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnkcjh.Name = "btnkcjh";
            this.btnkcjh.Size = new System.Drawing.Size(238, 67);
            this.btnkcjh.TabIndex = 28;
            this.btnkcjh.Text = "课程数据聚合计算\r\nCourse data aggregation calculation";
            this.btnkcjh.UseVisualStyleBackColor = true;
            this.btnkcjh.Click += new System.EventHandler(this.btnkcjh_Click);
            // 
            // labelsjc
            // 
            this.labelsjc.AutoSize = true;
            this.labelsjc.Location = new System.Drawing.Point(1041, 20);
            this.labelsjc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelsjc.Name = "labelsjc";
            this.labelsjc.Size = new System.Drawing.Size(80, 18);
            this.labelsjc.TabIndex = 27;
            this.labelsjc.Text = "labelsjc";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(915, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 26;
            this.label2.Text = "10114";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dataGridView1);
            this.groupBox4.Location = new System.Drawing.Point(18, 200);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(1140, 474);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "基础数据输出（BasicDataOutput）";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(10, 32);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1120, 428);
            this.dataGridView1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView2);
            this.groupBox3.Location = new System.Drawing.Point(18, 687);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(1140, 158);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "聚合数据输出（AggregatedDataOutput）";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(10, 33);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(1120, 104);
            this.dataGridView2.TabIndex = 0;
            // 
            // BjsjjsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 842);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "BjsjjsForm";
            this.Text = "课程数据聚合计算（Coursedataaggregationcalculation）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BjsjjsForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelsjc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Button btnkcjh;
        private System.Windows.Forms.Button btncsh;
    }
}