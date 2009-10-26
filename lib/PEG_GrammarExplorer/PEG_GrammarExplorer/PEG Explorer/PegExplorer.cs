/*Author:Martin.Holzherr;Date:20080922;Context:"PEG Support for C#";Licence:CPOL
 * <<History>> 
 *  20080922;V1.0 created
 * <</History>>
*/
using System;
using System.Text;
using System.Windows.Forms;

using Peg.Base;
using Peg.Samples;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PEG_Explorer
{
    public partial class PegExplorer : Form
    {
        #region Data Members
        SamplesCollection samples;
        #endregion Data Members
        #region Constructors
        public PegExplorer()
        {
            samples = new SamplesCollection();
            InitializeComponent();
            cboGrammar.DisplayMember = "grammarName";
            cboGrammar.DataSource = samples.sampleGrammars;
            LoadSourceFile("");
        } 
        #endregion
        #region Helper methods
        #region LoadSourceFile and helper methods
        private string BuildHexView(byte[] src)
        {
            const int nWidthInBytes = 16;
            StringBuilder sb = new StringBuilder(src.Length * 4, src.Length * 16);
            for (uint i = 0; i < src.Length; ++i)
            {
                if (i % nWidthInBytes == 0)
                {
                    if (i != 0){
                        for (uint j = i - nWidthInBytes; j < i; ++j)
                        {
                            if (src[j] <= 0x7F && !char.IsControl(((char)src[j])))
                            {
                                sb.Append((char)src[j]);
                            }
                            else
                            {
                                sb.Append('.');
                            }
                        }
                        sb.Append("\r\n");
                    }
                    sb.Append(i.ToString("X8"));
                }
                if (i % 8 == 0) sb.Append("  ");
                sb.Append(src[i].ToString("X2"));
                sb.Append("  ");
            }
            return sb.ToString();
        }
        void DisplaySrc(string src, string path, int len)
        {
            string[] lines = src.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            txtSource.Lines = lines;
            txtSource.Select(0, 0);
            char[] anyOf = { '\\', ':' };
            int pos = path.LastIndexOfAny(anyOf);
            Output.Text = (pos == -1 ? path : path.Substring(pos + 1)) + ": " + len / 1000 + "KByte";
        }
        void LoadSourceFile(string path)
        {
            txtSource.Tag = null;
            txtSource.ReadOnly = false;
            tvParseTree.Nodes.Clear();
            HidePostProcessButtons();
            cmbPostProcess.DataSource = null;

            SampleInfo selectedSample;
            if (path.Length == 0)
            {
                selectedSample = new SampleInfo();
                txtSource.Tag = new SourcefileInfo(path, EncodingClass.ascii, "");
            }else
            {
                if (cboGrammar.SelectedIndex < 0)
                {
                    MessageBox.Show("Grammar must be selected first");
                    return;
                }
                this.Text = "Parsing Expression Grammar Explorer (" + path.Substring(path.LastIndexOf('\\') + 1) + ")";
                selectedSample = (SampleInfo)cboGrammar.Items[cboGrammar.SelectedIndex];
                try
                {
                    FileLoader loader = new FileLoader(selectedSample.GetEncodingClass(), selectedSample.GetUnicodeDetection(), path);
                    if (loader.IsBinaryFile())
                    {
                        byte[] bytes;
                        loader.LoadFile(out bytes);
                        txtSource.Tag = new SourcefileInfo(path, selectedSample.GetEncodingClass(), bytes);
                        string src = BuildHexView(bytes);
                        DisplaySrc(src, path, src.Length);
                        txtSource.ReadOnly = true;
                    }
                    else
                    {
                        string src;
                        loader.LoadFile(out src);
                        txtSource.Tag = new SourcefileInfo(path, selectedSample.GetEncodingClass(), src);
                        DisplaySrc(src, path, src.Length);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        #endregion LoadSourceFile helper methods
        
        #endregion Helper methods
        #region Helper Classes
        class SourcefileInfo
        {
           
            internal SourcefileInfo(string sourcePath, EncodingClass encodingClass, byte[] sourceBytes)
            {
                sourcePath_ = sourcePath;
                encodingClass_ = encodingClass;
                sourceBytes_ = sourceBytes;
            }
            internal SourcefileInfo(string sourcePath, EncodingClass encodingClass, string sourceChars)
            {
                sourcePath_ = sourcePath;
                encodingClass_ = encodingClass;
                sourceChars_ = sourceChars;
            }
            internal string SourcePath { get { return sourcePath_; } }
            internal string SourceChars { get { return sourceChars_; } }
            internal byte[] SourceBytes { get { return sourceBytes_; } }
            internal EncodingClass EncodingClass { get { return encodingClass_; } }
            string sourcePath_;
            EncodingClass encodingClass_;
            byte[] sourceBytes_;  //only valid if encodingClass==eBinary
            string sourceChars_;  //only valid if encodingClass!=eBinary
        }
        public sealed class TextBoxWriter : TextWriter
        {
            internal TextBoxWriter(TextBox textBox)
                : base()
            {
                textBox_ = textBox;
            }

            private TextBox textBox_;


            public override Encoding Encoding
            {
                get { return Encoding.Default; }
            }


            // when the Console's Out property is set to other than the default,
            // the Console class will create a synchronized, thread-safe TextWriter
            // so we don't need to perform the otherwise required calls to
            // .InvokeRequired and .Invoke() on the TextBox.

            public override void Write(string value)
            {
                textBox_.AppendText(value.Replace("\n", base.NewLine));
            }


            public override void WriteLine(string value)
            {
                this.Write(value);
                // WriteLine() needs to append and additional line break.
                textBox_.AppendText(base.NewLine);
            }
        }
        #endregion Helper Classes
        #region EventHandlers
        #region Event handler helper methods
        void ShowLocation(PegBegEnd stretch)
        {
            if (stretch.posBeg_ < 0) return;
            if (txtSource.Tag != null && (txtSource.Tag as SourcefileInfo).EncodingClass == EncodingClass.binary)//hex view present in txtSource
            {
                TranslateToHexViewPosition(ref stretch);
            }
            txtSource.Select(stretch.posBeg_, stretch.posEnd_ - stretch.posBeg_);
            txtSource.ScrollToCaret();
        }
        private void HidePostProcessButtons()
        {
            cmbPostProcess.Visible = false;
            btnPostProcess.Visible = false;
            if (cboGrammar.SelectedIndex >= 0)
            {
                SampleInfo selectedSample = (SampleInfo)cboGrammar.Items[cboGrammar.SelectedIndex];
                grammarDescription.Text = selectedSample.grammarDescription_;
            }
        }
        #endregion
        private void btnFileOpen_Click(object sender, EventArgs e)
        {
            string proposedDirectory;
            if (dlgOpenFile == null) dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            if (ProposeSampleInputDirectory(out proposedDirectory))
            {
                dlgOpenFile.InitialDirectory = proposedDirectory;
            }
            if (dlgOpenFile.ShowDialog() != DialogResult.OK) return;
            LoadSourceFile(dlgOpenFile.FileName);

        }
        #region btnFileOpen_Click helper functions
        private bool ProposeSampleInputDirectory(out string proposedDir)
        {
            proposedDir = "";
            if (cboGrammar.SelectedIndex < 0) return false;
            var selectedSample = (SampleInfo)cboGrammar.Items[cboGrammar.SelectedIndex];
            string dir = Environment.CurrentDirectory;
            while (Directory.Exists(dir))
            {
                if (Directory.Exists(dir + "\\" + selectedSample.samplesDirectory))
                {
                    proposedDir = dir + "\\" + selectedSample.samplesDirectory;
                    return true;
                }
                int pos = dir.LastIndexOf(@"\");
                if (pos == -1) return false;
                dir = dir.Substring(0, pos);
            }
            return false;
        }
        #endregion
        private void btnParse_Click(object sender, EventArgs e)
        {
            Output.Text = "";
            HidePostProcessButtons();
            Console.SetOut(new TextBoxWriter(Output));
            var textBoxWriter = new TextBoxWriter(Output);
            if (cboGrammar.SelectedIndex < 0) return;
            cmbPostProcess.DataSource = null;
            SampleInfo selectedSample = (SampleInfo)cboGrammar.Items[cboGrammar.SelectedIndex];
            double elapsedTime;
            PegNode root;
            bool bMatches;

            if (selectedSample.GetEncodingClass() == EncodingClass.binary && (txtSource.Tag as SourcefileInfo).EncodingClass != EncodingClass.binary
              || selectedSample.GetEncodingClass() != EncodingClass.binary && (txtSource.Tag as SourcefileInfo).EncodingClass == EncodingClass.binary)
            {
                MessageBox.Show("Input text has wrong format for parsing");
                return;
            }
            else if (selectedSample.GetEncodingClass() == EncodingClass.binary)
            {
                bMatches = samples.Run(selectedSample.grammarId, (txtSource.Tag as SourcefileInfo).SourceBytes, textBoxWriter, out elapsedTime, out root);
            }
            else
            {
                bMatches = samples.Run(selectedSample.grammarId, txtSource.Text, textBoxWriter, out elapsedTime, out root);
            }
            Output.Text += (bMatches ? "Success." : "Failure.") + " Elapsed Time: " + elapsedTime.ToString("N03") + "ms";
            int srcLen = txtSource.Text.Length;
            if (srcLen > 2000)
            {
                Output.Text += "\tKByte/s:" + ((srcLen / 1024) / (elapsedTime/1000)).ToString("N03");
            }
            Output.Text += "\r\n";
            if (selectedSample.startRule.Target is PegCharParser)
            {
                BuildTree(root, ((PegCharParser)selectedSample.startRule.Target).TreeNodeToString);
            }
            else if (selectedSample.startRule.Target is PegByteParser)
            {
                BuildTree(root, ((PegByteParser)selectedSample.startRule.Target).TreeNodeToString);
            }

            if (bMatches && selectedSample.postProcessors_ != null)
            {
                cmbPostProcess.Visible = true;
                btnPostProcess.Visible = true;
                cmbPostProcess.DisplayMember = "ShortDesc";
                cmbPostProcess.DataSource = selectedSample.postProcessors_;
            }
            if (!bMatches)
            {
                Output_Click(sender,e);
            }
        }
        #region btnParse_Click helper methods
        private void ExpandTop(TreeNode node, ref int nofElemsToExpand)
        {
            if (node == null || nofElemsToExpand <= 0) return;
            node.Expand();
            if (--nofElemsToExpand <= 0) return;
            ExpandTop(node.NextNode, ref nofElemsToExpand);
            ExpandTop(node.FirstNode, ref nofElemsToExpand);

        }
        private void ExpandTop(int nofElemsToExpand)
        {
            foreach (TreeNode node in tvParseTree.Nodes)
            {
                ExpandTop(node, ref nofElemsToExpand);
                tvParseTree.SelectedNode = node;
            }
        }
        delegate string NodeToString(PegNode node);
        private void BuildTree(PegNode root, NodeToString nodeToString)
        {
            tvParseTree.Nodes.Clear();
            AddTreeNode(null, root, nodeToString);
            ExpandTop(10);
        }
        private void AddTreeNode(TreeNode parent, PegNode node, NodeToString nodeToString)
        {
            if (node == null) return;
            string txt = nodeToString(node);
            if (node.parent_ == null) txt = "^" + txt;
            TreeNode tn = (parent == null ? tvParseTree.Nodes.Add(txt) : parent.Nodes.Add(txt));
            tn.Tag = node;
            AddTreeNode(tn, node.child_, nodeToString);
            AddTreeNode(parent, node.next_, nodeToString);
        }
        #endregion btnParse_Click helper methods
        private void cmbPostProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPostProcess.SelectedIndex < 0 || cboGrammar.SelectedIndex < 0) return;
            SampleInfo selectedSample = (SampleInfo)cboGrammar.Items[cboGrammar.SelectedIndex];
            var selectedPostProcessor = (IParserPostProcessor)cmbPostProcess.Items[cmbPostProcess.SelectedIndex];
            if (selectedPostProcessor != null)
            {
                btnPostProcess.Visible = true;
                grammarDescription.Text= selectedPostProcessor.DetailDesc;
                btnPostProcess.Text= selectedPostProcessor.ShortestDesc;
            }
        }
        #region cmbPostProcess_SelectedIndexChanged helper methods
        void GetGrammarFileNameAndSource(PegCharParser parser, int ruleId, out string grammarFileName, out string src)
        {
            grammarFileName = parser.GetRuleNameFromId(ruleId) + ".cs";
            src = parser.GetSource();
        }
        void GetGrammarFileNameAndSource(PegByteParser parser, int ruleId, out string grammarFileName, out byte[] src)
        {
            grammarFileName = parser.GetRuleNameFromId(ruleId);
            src = parser.GetSource();
        }
        string GetSourceFileTitle()
        {
            Debug.Assert(txtSource.Tag != null);
            string path = (txtSource.Tag as SourcefileInfo).SourcePath;
            int pos = path.LastIndexOfAny(new char[] { '\\', '/', ':' });
            return pos == -1 ? "Test" : path.Substring(pos + 1);
        }
        string GetOutputDirectory()
        {
            Debug.Assert(txtSource.Tag != null);
            string path = (txtSource.Tag as SourcefileInfo).SourcePath;
            int pos = path.LastIndexOfAny(new char[] { '\\', '/', ':' });
            string sGenDirectory = pos == -1 ? Directory.GetCurrentDirectory() : path.Substring(0, pos + 1);
            return sGenDirectory;
        }
        #endregion cmbPostProcess_SelectedIndexChanged
        private void cboGrammar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGrammar.SelectedIndex < 0) return;
            if (dlgOpenFile != null)
            {
                dlgOpenFile.Dispose();
                dlgOpenFile = null;
            }
            tvParseTree.Nodes.Clear();
            HidePostProcessButtons();
            cmbPostProcess.DataSource = null;
        }
        private void tvParseTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowNode(tvParseTree.SelectedNode);
        }
        #region tvParseTree_AfterSelect helper methods
        void TranslateToHexViewPosition(ref int pos)
        {
            const int hexViewLineLength = 8 + 2 + (16 * 4 + 2) + 16 + 2;
            int line = pos / 16;
            int col = pos % 16;
            int linePos = 8 + 2;
            linePos += col * 4;
            if (col >= 8) linePos += 2;
            pos = line * hexViewLineLength + linePos;
        }
        void TranslateToHexViewPosition(ref PegBegEnd stretch)
        {
            int len = stretch.Length;
            TranslateToHexViewPosition(ref stretch.posBeg_);
            if (len > 0) --stretch.posEnd_;
            TranslateToHexViewPosition(ref stretch.posEnd_);
            stretch.posEnd_ += 2;
        }
      
        void ShowNode(TreeNode node)
        {
            if (node == null) return;
            PegNode pegNode = node.Tag as PegNode;
            if (pegNode == null) return;
            ShowLocation(pegNode.match_);
        }
        #endregion tvParseTree_AfterSelect helper methods
        private void Output_Click(object sender, EventArgs e)
        {
            #region Select Text Source Error Posiion
            Regex regex = new Regex("<([0-9]+),([0-9]+)>");
            Match m = regex.Match(Output.Text);
            if (m.Success )
            {

                int errLine, errCol;
                Int32.TryParse(Output.Text.Substring(m.Groups[1].Index, m.Groups[1].Length), out errLine);
                Int32.TryParse(Output.Text.Substring(m.Groups[2].Index, m.Groups[2].Length), out errCol);
                int selBeg = (errLine == -1 ? 1 : txtSource.Text.NthIndexOf('\n', 0, errLine - 1)) + errCol;
                if (selBeg >= txtSource.Text.Length)
                {
                    selBeg = txtSource.Text.Length - 1;
                }
                int selEnd = selBeg + 1;
                ShowLocation(new PegBegEnd() { posBeg_ = selBeg, posEnd_ = selEnd });
            } 
            #endregion
            #region Select Binary Source Error Position
            else
            {
                regex = new Regex("<([0-9]+)>");
                m = regex.Match(Output.Text);
                if (m.Success)
                {
                    int errOffset;
                    Int32.TryParse(Output.Text.Substring(m.Groups[1].Index, m.Groups[1].Length), out errOffset);
                    ShowLocation(new PegBegEnd() { posBeg_ = errOffset, posEnd_ = errOffset + 1 });
                }
                
            } 
            #endregion
            
        }
        private void btnPostProcess_Click(object sender, EventArgs e)
        {
            if (cmbPostProcess.SelectedIndex < 0 || cboGrammar.SelectedIndex < 0) return;
            SampleInfo selectedSample = (SampleInfo)cboGrammar.Items[cboGrammar.SelectedIndex];
            PegNode root = (PegNode)(tvParseTree.Nodes.Count == 0 ? null : tvParseTree.Nodes[0].Tag);
            TextBoxWriter errOut = new TextBoxWriter(Output);
            ParserPostProcessParams postProcParams;
            if (selectedSample.startRule.Target is PegCharParser)
            {
                string grammarFileName, src;
                GetGrammarFileNameAndSource((PegCharParser)selectedSample.startRule.Target, root.id_, out grammarFileName, out src);
                postProcParams = new ParserPostProcessParams(GetOutputDirectory(),GetSourceFileTitle(), grammarFileName, root, src, errOut);
            }
            else
            {
                string grammarFileName;
                byte[] src;
                GetGrammarFileNameAndSource((PegByteParser)selectedSample.startRule.Target, root.id_, out grammarFileName, out src);
                postProcParams = new ParserPostProcessParams(GetOutputDirectory(), GetSourceFileTitle(), grammarFileName, root, src, errOut);
            }
            var selectedPostProcessor = (IParserPostProcessor)cmbPostProcess.Items[cmbPostProcess.SelectedIndex];
            if (selectedPostProcessor != null)
            {
                selectedPostProcessor.Postprocess(postProcParams);
            }
        }
        
        #endregion EventHandlers

    }
    public static class StringExtension
    {
        public static int NthIndexOf(this string s, char c, int startIndex, int n)
        {
            --startIndex;
            for (; n > 0; --n)
            {
                startIndex = s.IndexOf(c, startIndex + 1);
                if (startIndex == -1) return -1;
            }
            return startIndex;
        }
    }
}


