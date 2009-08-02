namespace PEG_Explorer
{
    partial class PegExplorer
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboGrammar = new System.Windows.Forms.ComboBox();
            this.grammarDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbPostProcess = new System.Windows.Forms.ComboBox();
            this.btnFileOpen = new System.Windows.Forms.Button();
            this.btnParse = new System.Windows.Forms.Button();
            this.Output = new System.Windows.Forms.TextBox();
            this.tvParseTree = new System.Windows.Forms.TreeView();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(676, 457);
            this.splitContainer1.SplitterDistance = 29;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboGrammar);
            this.panel1.Controls.Add(this.grammarDescription);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(676, 29);
            this.panel1.TabIndex = 0;
            // 
            // cboGrammar
            // 
            this.cboGrammar.DropDownHeight = 200;
            this.cboGrammar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGrammar.FormattingEnabled = true;
            this.cboGrammar.IntegralHeight = false;
            this.cboGrammar.Location = new System.Drawing.Point(71, 6);
            this.cboGrammar.Name = "cboGrammar";
            this.cboGrammar.Size = new System.Drawing.Size(189, 21);
            this.cboGrammar.TabIndex = 7;
            this.cboGrammar.SelectedIndexChanged += new System.EventHandler(this.cboGrammar_SelectedIndexChanged);
            // 
            // grammarDescription
            // 
            this.grammarDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grammarDescription.Location = new System.Drawing.Point(277, 3);
            this.grammarDescription.Name = "grammarDescription";
            this.grammarDescription.ReadOnly = true;
            this.grammarDescription.Size = new System.Drawing.Size(396, 20);
            this.grammarDescription.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Grammar:";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tvParseTree);
            this.splitContainer2.Size = new System.Drawing.Size(676, 424);
            this.splitContainer2.SplitterDistance = 499;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.txtSource);
            this.splitContainer3.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.Output);
            this.splitContainer3.Size = new System.Drawing.Size(499, 424);
            this.splitContainer3.SplitterDistance = 360;
            this.splitContainer3.TabIndex = 0;
            // 
            // txtSource
            // 
            this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSource.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSource.HideSelection = false;
            this.txtSource.Location = new System.Drawing.Point(-3, 30);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSource.Size = new System.Drawing.Size(505, 326);
            this.txtSource.TabIndex = 18;
            this.txtSource.WordWrap = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbPostProcess);
            this.panel2.Controls.Add(this.btnFileOpen);
            this.panel2.Controls.Add(this.btnParse);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(499, 29);
            this.panel2.TabIndex = 17;
            // 
            // cmbPostProcess
            // 
            this.cmbPostProcess.DropDownHeight = 100;
            this.cmbPostProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPostProcess.FormattingEnabled = true;
            this.cmbPostProcess.IntegralHeight = false;
            this.cmbPostProcess.Location = new System.Drawing.Point(200, 3);
            this.cmbPostProcess.Name = "cmbPostProcess";
            this.cmbPostProcess.Size = new System.Drawing.Size(177, 21);
            this.cmbPostProcess.TabIndex = 10;
            this.cmbPostProcess.SelectedIndexChanged += new System.EventHandler(this.cmbPostProcess_SelectedIndexChanged);
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.Location = new System.Drawing.Point(14, 3);
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(88, 23);
            this.btnFileOpen.TabIndex = 8;
            this.btnFileOpen.Text = "Load ...";
            this.btnFileOpen.UseVisualStyleBackColor = true;
            this.btnFileOpen.Click += new System.EventHandler(this.btnFileOpen_Click);
            // 
            // btnParse
            // 
            this.btnParse.Location = new System.Drawing.Point(108, 3);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(86, 23);
            this.btnParse.TabIndex = 7;
            this.btnParse.Text = "Parse";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // Output
            // 
            this.Output.AcceptsReturn = true;
            this.Output.AcceptsTab = true;
            this.Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Output.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Output.Location = new System.Drawing.Point(0, 0);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.ReadOnly = true;
            this.Output.Size = new System.Drawing.Size(499, 60);
            this.Output.TabIndex = 1;
            this.Output.WordWrap = false;
            // 
            // tvParseTree
            // 
            this.tvParseTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvParseTree.Location = new System.Drawing.Point(0, 0);
            this.tvParseTree.Name = "tvParseTree";
            this.tvParseTree.Size = new System.Drawing.Size(173, 424);
            this.tvParseTree.TabIndex = 1;
            // 
            // PegExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 457);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PegExplorer";
            this.Text = "Parsing Expression Grammar Explorer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            this.splitContainer3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox grammarDescription;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbPostProcess;
        private System.Windows.Forms.Button btnFileOpen;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.TextBox Output;
        private System.Windows.Forms.ComboBox cboGrammar;
        private System.Windows.Forms.TreeView tvParseTree;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
    }
}

