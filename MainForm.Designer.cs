namespace cjpc
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.系统注册ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.输入注册码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.成绩输入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.生成成绩输入表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.初始成绩导入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系数计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.三元素分项生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AHP_Knowledge = new System.Windows.Forms.ToolStripMenuItem();
            this.AHP_Skill = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.Questionclassification = new System.Windows.Forms.ToolStripMenuItem();
            this.成绩计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.输出表计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.扩展数据计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.班级数据计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.成绩输出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.输出表输出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.输出表赋值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成成绩单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelsjc = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统注册ToolStripMenuItem,
            this.成绩输入ToolStripMenuItem,
            this.系数计算ToolStripMenuItem,
            this.成绩计算ToolStripMenuItem,
            this.成绩输出ToolStripMenuItem,
            this.生成成绩单ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1176, 34);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 系统注册ToolStripMenuItem
            // 
            this.系统注册ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.输入注册码ToolStripMenuItem});
            this.系统注册ToolStripMenuItem.Name = "系统注册ToolStripMenuItem";
            this.系统注册ToolStripMenuItem.Size = new System.Drawing.Size(94, 28);
            this.系统注册ToolStripMenuItem.Text = "系统登陆";
            // 
            // 输入注册码ToolStripMenuItem
            // 
            this.输入注册码ToolStripMenuItem.Name = "输入注册码ToolStripMenuItem";
            this.输入注册码ToolStripMenuItem.Size = new System.Drawing.Size(324, 30);
            this.输入注册码ToolStripMenuItem.Text = "账号登陆（Account Login）";
            this.输入注册码ToolStripMenuItem.Click += new System.EventHandler(this.输入注册码ToolStripMenuItem_Click);
            // 
            // 成绩输入ToolStripMenuItem
            // 
            this.成绩输入ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.生成成绩输入表ToolStripMenuItem,
            this.初始成绩导入ToolStripMenuItem});
            this.成绩输入ToolStripMenuItem.Name = "成绩输入ToolStripMenuItem";
            this.成绩输入ToolStripMenuItem.Size = new System.Drawing.Size(94, 28);
            this.成绩输入ToolStripMenuItem.Text = "成绩输入";
            this.成绩输入ToolStripMenuItem.Visible = false;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(432, 30);
            this.toolStripMenuItem1.Text = "建立成绩表（EstablishScoreSheet）";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 生成成绩输入表ToolStripMenuItem
            // 
            this.生成成绩输入表ToolStripMenuItem.Name = "生成成绩输入表ToolStripMenuItem";
            this.生成成绩输入表ToolStripMenuItem.Size = new System.Drawing.Size(432, 30);
            this.生成成绩输入表ToolStripMenuItem.Text = "生成成绩输入表（GenerateGradeSheet）";
            this.生成成绩输入表ToolStripMenuItem.Click += new System.EventHandler(this.生成成绩输入表ToolStripMenuItem_Click);
            // 
            // 初始成绩导入ToolStripMenuItem
            // 
            this.初始成绩导入ToolStripMenuItem.Name = "初始成绩导入ToolStripMenuItem";
            this.初始成绩导入ToolStripMenuItem.Size = new System.Drawing.Size(432, 30);
            this.初始成绩导入ToolStripMenuItem.Text = "初始成绩导入（InitialScoreImport）";
            this.初始成绩导入ToolStripMenuItem.Click += new System.EventHandler(this.初始成绩导入ToolStripMenuItem_Click);
            // 
            // 系数计算ToolStripMenuItem
            // 
            this.系数计算ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.三元素分项生成ToolStripMenuItem,
            this.AHP_Knowledge,
            this.AHP_Skill,
            this.toolStripMenuItem3,
            this.Questionclassification});
            this.系数计算ToolStripMenuItem.Name = "系数计算ToolStripMenuItem";
            this.系数计算ToolStripMenuItem.Size = new System.Drawing.Size(299, 28);
            this.系数计算ToolStripMenuItem.Text = "分类处理Classificationprocessing";
            this.系数计算ToolStripMenuItem.Visible = false;
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(709, 30);
            this.toolStripMenuItem2.Text = "层次分析法-三元素（Analytic Hierarchy Process - Three Elements）";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // 三元素分项生成ToolStripMenuItem
            // 
            this.三元素分项生成ToolStripMenuItem.Name = "三元素分项生成ToolStripMenuItem";
            this.三元素分项生成ToolStripMenuItem.Size = new System.Drawing.Size(709, 30);
            this.三元素分项生成ToolStripMenuItem.Text = "三元素分项生成(Three element sub item generation)";
            this.三元素分项生成ToolStripMenuItem.Click += new System.EventHandler(this.三元素分项生成ToolStripMenuItem_Click);
            // 
            // AHP_Knowledge
            // 
            this.AHP_Knowledge.Name = "AHP_Knowledge";
            this.AHP_Knowledge.Size = new System.Drawing.Size(709, 30);
            this.AHP_Knowledge.Text = "层次分析法-知识（AHP-Knowledge）";
            this.AHP_Knowledge.Click += new System.EventHandler(this.AHP_Knowledge_Click);
            // 
            // AHP_Skill
            // 
            this.AHP_Skill.Name = "AHP_Skill";
            this.AHP_Skill.Size = new System.Drawing.Size(709, 30);
            this.AHP_Skill.Text = "层次分析法-技能（AHP-Skill）";
            this.AHP_Skill.Click += new System.EventHandler(this.AHP_Skill_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(709, 30);
            this.toolStripMenuItem3.Text = "层次分析法-能力（AHP-Ability）";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // Questionclassification
            // 
            this.Questionclassification.Name = "Questionclassification";
            this.Questionclassification.Size = new System.Drawing.Size(709, 30);
            this.Questionclassification.Text = "熵权法-题目分类与求权（EWM - Question Classification and Weighting）";
            this.Questionclassification.Click += new System.EventHandler(this.Questionclassification_Click);
            // 
            // 成绩计算ToolStripMenuItem
            // 
            this.成绩计算ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.输出表计算ToolStripMenuItem,
            this.数据计算ToolStripMenuItem,
            this.扩展数据计算ToolStripMenuItem,
            this.班级数据计算ToolStripMenuItem});
            this.成绩计算ToolStripMenuItem.Name = "成绩计算ToolStripMenuItem";
            this.成绩计算ToolStripMenuItem.Size = new System.Drawing.Size(225, 28);
            this.成绩计算ToolStripMenuItem.Text = "成绩计算GradeCalculate";
            this.成绩计算ToolStripMenuItem.Visible = false;
            // 
            // 输出表计算ToolStripMenuItem
            // 
            this.输出表计算ToolStripMenuItem.Name = "输出表计算ToolStripMenuItem";
            this.输出表计算ToolStripMenuItem.Size = new System.Drawing.Size(430, 30);
            this.输出表计算ToolStripMenuItem.Text = "输出表计算(OutputTableCalculation)";
            this.输出表计算ToolStripMenuItem.Click += new System.EventHandler(this.输出表计算ToolStripMenuItem_Click);
            // 
            // 数据计算ToolStripMenuItem
            // 
            this.数据计算ToolStripMenuItem.Name = "数据计算ToolStripMenuItem";
            this.数据计算ToolStripMenuItem.Size = new System.Drawing.Size(430, 30);
            this.数据计算ToolStripMenuItem.Text = "数据计算(Computer)";
            this.数据计算ToolStripMenuItem.Click += new System.EventHandler(this.数据计算ToolStripMenuItem_Click);
            // 
            // 扩展数据计算ToolStripMenuItem
            // 
            this.扩展数据计算ToolStripMenuItem.Name = "扩展数据计算ToolStripMenuItem";
            this.扩展数据计算ToolStripMenuItem.Size = new System.Drawing.Size(430, 30);
            this.扩展数据计算ToolStripMenuItem.Text = "扩展数据计算(ExtendedDataCalculation)";
            this.扩展数据计算ToolStripMenuItem.Click += new System.EventHandler(this.扩展数据计算ToolStripMenuItem_Click);
            // 
            // 班级数据计算ToolStripMenuItem
            // 
            this.班级数据计算ToolStripMenuItem.Name = "班级数据计算ToolStripMenuItem";
            this.班级数据计算ToolStripMenuItem.Size = new System.Drawing.Size(430, 30);
            this.班级数据计算ToolStripMenuItem.Text = "课程数据聚合(CourseDataAggregation)";
            this.班级数据计算ToolStripMenuItem.Click += new System.EventHandler(this.班级数据计算ToolStripMenuItem_Click);
            // 
            // 成绩输出ToolStripMenuItem
            // 
            this.成绩输出ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.输出表输出ToolStripMenuItem,
            this.输出表赋值ToolStripMenuItem});
            this.成绩输出ToolStripMenuItem.Name = "成绩输出ToolStripMenuItem";
            this.成绩输出ToolStripMenuItem.Size = new System.Drawing.Size(94, 28);
            this.成绩输出ToolStripMenuItem.Text = "成绩输出";
            this.成绩输出ToolStripMenuItem.Visible = false;
            // 
            // 输出表输出ToolStripMenuItem
            // 
            this.输出表输出ToolStripMenuItem.Name = "输出表输出ToolStripMenuItem";
            this.输出表输出ToolStripMenuItem.Size = new System.Drawing.Size(406, 30);
            this.输出表输出ToolStripMenuItem.Text = "输出表输出(OutputTable)";
            this.输出表输出ToolStripMenuItem.Click += new System.EventHandler(this.输出表输出ToolStripMenuItem_Click);
            // 
            // 输出表赋值ToolStripMenuItem
            // 
            this.输出表赋值ToolStripMenuItem.Name = "输出表赋值ToolStripMenuItem";
            this.输出表赋值ToolStripMenuItem.Size = new System.Drawing.Size(406, 30);
            this.输出表赋值ToolStripMenuItem.Text = "输出表赋值(OutputTableAssignment)";
            this.输出表赋值ToolStripMenuItem.Visible = false;
            this.输出表赋值ToolStripMenuItem.Click += new System.EventHandler(this.输出表赋值ToolStripMenuItem_Click);
            // 
            // 生成成绩单ToolStripMenuItem
            // 
            this.生成成绩单ToolStripMenuItem.Name = "生成成绩单ToolStripMenuItem";
            this.生成成绩单ToolStripMenuItem.Size = new System.Drawing.Size(288, 28);
            this.生成成绩单ToolStripMenuItem.Text = "生成成绩单(GenerateTranscript)";
            this.生成成绩单ToolStripMenuItem.Visible = false;
            this.生成成绩单ToolStripMenuItem.Click += new System.EventHandler(this.生成成绩单ToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(980, 110);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "您好：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1012, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 18);
            this.label2.TabIndex = 3;
            // 
            // labelsjc
            // 
            this.labelsjc.AutoSize = true;
            this.labelsjc.Location = new System.Drawing.Point(740, 64);
            this.labelsjc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelsjc.Name = "labelsjc";
            this.labelsjc.Size = new System.Drawing.Size(0, 18);
            this.labelsjc.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 693);
            this.Controls.Add(this.labelsjc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "测评系统(AssessmentSystem)";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 系统注册ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 输入注册码ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 成绩输入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生成成绩输入表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 初始成绩导入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系数计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 三元素分项生成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 成绩计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 输出表计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 扩展数据计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 成绩输出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 输出表输出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 输出表赋值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label labelsjc;
        private System.Windows.Forms.ToolStripMenuItem 班级数据计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生成成绩单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem AHP_Knowledge;
        private System.Windows.Forms.ToolStripMenuItem AHP_Skill;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem Questionclassification;
    }
}

