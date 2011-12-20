/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

   This file is part of the fyiReporting RDL project.
	
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;
using System.IO;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// Summary description for DialogAbout.
    /// </summary>
    public partial class DialogToolOptions 
    {

        bool bDesktop = false;
        bool bToolbar = false;
        bool bMaps = false;

        // Desktop server configuration
        XmlDocument _DesktopDocument;
        XmlNode _DesktopPort;
        XmlNode _DesktopDirectory;
        XmlNode _DesktopLocal;

        public DialogToolOptions(RdlDesigner rdl)
        {
            _RdlDesigner = rdl;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            Init();
            return;
        }

        private void Init()
        {
            this.tbRecentFilesMax.Text = _RdlDesigner.RecentFilesMax.ToString();
            this.tbHelpUrl.Text = _RdlDesigner.HelpUrl;

            // init the toolbar

            // list of items in current toolbar
            foreach (string ti in _RdlDesigner.Toolbar)
            {
                this.lbToolbar.Items.Add(ti);
            }

            this.cbEditLines.Checked = _RdlDesigner.ShowEditLines;
            this.cbOutline.Checked = _RdlDesigner.ShowReportItemOutline;
            this.cbTabInterface.Checked = _RdlDesigner.ShowTabbedInterface;
            chkPBAutoHide.Checked = _RdlDesigner.PropertiesAutoHide;
            this.cbShowReportWaitDialog.Checked = _RdlDesigner.ShowPreviewWaitDialog;

            switch (_RdlDesigner.PropertiesLocation)
            {
                case DockStyle.Top:
                    this.rbPBTop.Checked = true;
                    break;
                case DockStyle.Bottom:
                    this.rbPBBottom.Checked = true;
                    break;
                case DockStyle.Right:
                    this.rbPBRight.Checked = true;
                    break;
                case DockStyle.Left:
                default:
                    this.rbPBLeft.Checked = true;
                    break;
            }


            InitOperations();

            InitDesktop();

            InitMaps();
            bDesktop = bToolbar = bMaps = false;			// start with no changes
        }

        private void InitDesktop()
        {
            string optFileName = AppDomain.CurrentDomain.BaseDirectory + "config.xml";

            try
            {
                XmlDocument xDoc = _DesktopDocument = new XmlDocument();
                xDoc.PreserveWhitespace = true;
                xDoc.Load(optFileName);
                XmlNode xNode;
                xNode = xDoc.SelectSingleNode("//config");

                // Loop thru all the child nodes
                foreach (XmlNode xNodeLoop in xNode.ChildNodes)
                {
                    if (xNodeLoop.NodeType != XmlNodeType.Element)
                        continue;
                    switch (xNodeLoop.Name.ToLower())
                    {
                        case "port":
                            this.tbPort.Text = xNodeLoop.InnerText;
                            _DesktopPort = xNodeLoop;
                            break;
                        case "localhostonly":
                            string tf = xNodeLoop.InnerText.ToLower();
                            this.ckLocal.Checked = !(tf == "false");
                            _DesktopLocal = xNodeLoop;
                            break;
                        case "serverroot":
                            this.tbDirectory.Text = xNodeLoop.InnerText;
                            _DesktopDirectory = xNodeLoop;
                            break;
                        case "cachedirectory":
                            // wd = xNodeLoop.InnerText;
                            break;
                        case "tracelevel":
                            break;
                        case "maxreadcache":
                            break;
                        case "maxreadcacheentrysize":
                            break;
                        case "mimetypes":
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {		// Didn't sucessfully get the startup state: use defaults
                MessageBox.Show(string.Format("Error processing Desktop Configuration; using defaults.\n{0}", ex.Message), "Options");
                this.tbPort.Text = "8080";
                this.ckLocal.Checked = true;
                this.tbDirectory.Text = "Examples";
            }

        }

        private void InitMaps()
        {
            lbMaps.Items.Clear();
            lbMaps.Items.AddRange(RdlDesigner.MapSubtypes);
        }

        private void InitOperations()
        {
            // list of operations; 
            lbOperation.Items.Clear();

            List<string> dups = _RdlDesigner.ToolbarAllowDups;
            foreach (string ti in _RdlDesigner.ToolbarOperations)
            {
                // if item is allowed to be duplicated or if
                //   item has not already been used we add to operation list
                if (dups.Contains(ti) || !lbToolbar.Items.Contains(ti))
                    this.lbOperation.Items.Add(ti);
            }
        }

        private bool Verify()
        {
            try
            {
                int i = Convert.ToInt32(this.tbRecentFilesMax.Text);
                return (i >= 1 || i <= 50);
            }
            catch
            {
                MessageBox.Show("Recent files maximum must be an integer between 1 and 50", "Options");
                return false;
            }
        }

        private void bOK_Click(object sender, System.EventArgs e)
        {
            if (DoApply())
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool DoApply()
        {
            lock (this)
            {
                try
                {
                    if (!Verify())
                        return false;
                    HandleRecentFilesMax();
                    _RdlDesigner.HelpUrl = this.tbHelpUrl.Text;
                    HandleShows();
                    HandleProperties();
                    if (bToolbar)
                        HandleToolbar();
                    if (bDesktop)
                        HandleDesktop();
                    if (bMaps)
                        HandleMaps();
                    bToolbar = bDesktop = false;		// no changes now
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Options");
                    return false;
                }
            }
        }

        private void HandleProperties()
        {
            DockStyle ds = DockStyle.Right;
            if (this.rbPBTop.Checked)
                ds = DockStyle.Top;
            else if (this.rbPBBottom.Checked)
                ds = DockStyle.Bottom;
            else if (this.rbPBLeft.Checked)
                ds = DockStyle.Left;

            _RdlDesigner.PropertiesLocation = ds;
            _RdlDesigner.PropertiesAutoHide = chkPBAutoHide.Checked;
        }

        private void HandleDesktop()
        {
            if (_DesktopDocument == null)
            {
                _DesktopDocument = new XmlDocument();
                XmlProcessingInstruction xPI;
                xPI = _DesktopDocument.CreateProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                _DesktopDocument.AppendChild(xPI);
            }

            if (_DesktopPort == null)
            {
                _DesktopPort = _DesktopDocument.CreateElement("port");
                _DesktopDocument.AppendChild(_DesktopPort);
            }
            _DesktopPort.InnerText = this.tbPort.Text;

            if (_DesktopDirectory == null)
            {
                _DesktopDirectory = _DesktopDocument.CreateElement("serverroot");
                _DesktopDocument.AppendChild(_DesktopDirectory);
            }
            _DesktopDirectory.InnerText = this.tbDirectory.Text;

            if (_DesktopLocal == null)
            {
                _DesktopLocal = _DesktopDocument.CreateElement("localhostonly");
                _DesktopDocument.AppendChild(_DesktopLocal);
            }
            _DesktopLocal.InnerText = this.ckLocal.Checked ? "true" : "false";

            string optFileName = AppDomain.CurrentDomain.BaseDirectory + "config.xml";

            _DesktopDocument.Save(optFileName);
            this._RdlDesigner.menuToolsCloseProcess(false);		// close the server
        }

        private void HandleMaps()
        {
            string[] maps = new string[lbMaps.Items.Count];
            for (int i = 0; i < lbMaps.Items.Count; i++)
            {
                maps[i] = lbMaps.Items[i] as string;
            }
            RdlDesigner.MapSubtypes = maps;
        }

        private void HandleRecentFilesMax()
        {
            // Handle the RecentFilesMax
            int i = Convert.ToInt32(this.tbRecentFilesMax.Text);
            if (i < 1 || i > 50)
                throw new Exception("Recent files maximum must be an integer between 1 and 50");
            if (this._RdlDesigner.RecentFilesMax == i)	// if not different we don't need to do anything
                return;

            this._RdlDesigner.RecentFilesMax = i;

            // Make the list match the maximum size
            bool bChangeMenu = false;
            while (_RdlDesigner.RecentFiles.Count > _RdlDesigner.RecentFilesMax)
            {
                _RdlDesigner.RecentFiles.RemoveAt(0);	// remove the first entry
                bChangeMenu = true;
            }

            if (bChangeMenu)
                _RdlDesigner.RecentFilesMenu();			// reset the menu since the list changed
            return;
        }

        private void HandleToolbar()
        {
            List<string> ar = new List<string>();
            foreach (string item in this.lbToolbar.Items)
                ar.Add(item);
            this._RdlDesigner.Toolbar = ar;
        }

        private void HandleShows()
        {
            _RdlDesigner.ShowEditLines = this.cbEditLines.Checked;
            _RdlDesigner.ShowReportItemOutline = this.cbOutline.Checked;
            _RdlDesigner.ShowTabbedInterface = this.cbTabInterface.Checked;
            _RdlDesigner.ShowPreviewWaitDialog = this.cbShowReportWaitDialog.Checked;

            foreach (MDIChild mc in _RdlDesigner.MdiChildren)
            {
                mc.ShowEditLines(this.cbEditLines.Checked);
                mc.ShowReportItemOutline = this.cbOutline.Checked;
                mc.ShowPreviewWaitDialog(this.cbShowReportWaitDialog.Checked);
            }

        }

        private void bCopyItem_Click(object sender, System.EventArgs e)
        {
            bToolbar = true;
            int i = this.lbOperation.SelectedIndex;
            if (i < 0)
                return;
            string itm = lbOperation.Items[i] as String;
            lbToolbar.Items.Add(itm);
            // Remove from list if not allowed to be duplicated in toolbar
            if (!_RdlDesigner.ToolbarAllowDups.Contains(itm))
                lbOperation.Items.RemoveAt(i);
        }

        private void bRemove_Click(object sender, System.EventArgs e)
        {
            bToolbar = true;
            int i = this.lbToolbar.SelectedIndex;
            if (i < 0)
                return;
            string itm = lbToolbar.Items[i] as String;
            if (itm == "Newline" || itm == "Space")
            { }
            else
                lbOperation.Items.Add(itm);

            lbToolbar.Items.RemoveAt(i);
        }

        private void bUp_Click(object sender, System.EventArgs e)
        {
            int i = this.lbToolbar.SelectedIndex;
            if (i <= 0)
                return;

            Swap(i - 1, i);
        }

        private void bDown_Click(object sender, System.EventArgs e)
        {
            int i = this.lbToolbar.SelectedIndex;
            if (i < 0 || i == lbToolbar.Items.Count - 1)
                return;

            Swap(i, i + 1);
        }

        /// <summary>
        /// Swap items in the toolbar listbox.  i1 should always be less than i2
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        private void Swap(int i1, int i2)
        {
            bToolbar = true;
            bool b1 = (i1 == lbToolbar.SelectedIndex);

            string s1 = lbToolbar.Items[i1] as string;
            string s2 = lbToolbar.Items[i2] as string;
            lbToolbar.SuspendLayout();
            lbToolbar.Items.RemoveAt(i2);
            lbToolbar.Items.RemoveAt(i1);
            lbToolbar.Items.Insert(i1, s2);
            lbToolbar.Items.Insert(i2, s1);
            lbToolbar.SelectedIndex = b1 ? i2 : i1;
            lbToolbar.ResumeLayout(true);
        }

        private void bReset_Click(object sender, System.EventArgs e)
        {
            bToolbar = true;

            this.lbToolbar.Items.Clear();
            List<string> ar = this._RdlDesigner.ToolbarDefault;
            foreach (string itm in ar)
                this.lbToolbar.Items.Add(itm);

            InitOperations();
        }

        private void bApply_Click(object sender, System.EventArgs e)
        {
            DoApply();
        }

        private void Desktop_Changed(object sender, System.EventArgs e)
        {
            bDesktop = true;
        }

        private void bBrowse_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            // Set the help text description for the FolderBrowserDialog.
            fbd.Description =
                "Select the directory that will contain reports.";

            // Do not allow the user to create new files via the FolderBrowserDialog.
            fbd.ShowNewFolderButton = false;
            //			fbd.RootFolder = System.Environment.SpecialFolder.MyComputer;
            fbd.SelectedPath = this.tbDirectory.Text.Length == 0 ?
                "Examples" : tbDirectory.Text;

            try
            {
                if (fbd.ShowDialog(this) == DialogResult.Cancel)
                    return;

                tbDirectory.Text = fbd.SelectedPath;
                bDesktop = true;		// we modified Desktop settings

            }
            finally
            {
                fbd.Dispose();
            }

            return;
        }

        static internal DesktopConfig DesktopConfiguration
        {
            get
            {
                string optFileName = AppDomain.CurrentDomain.BaseDirectory + "config.xml";

                DesktopConfig dc = new DesktopConfig();
                try
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(optFileName);
                    XmlNode xNode;
                    xNode = xDoc.SelectSingleNode("//config");

                    // Loop thru all the child nodes
                    foreach (XmlNode xNodeLoop in xNode.ChildNodes)
                    {
                        if (xNodeLoop.NodeType != XmlNodeType.Element)
                            continue;
                        switch (xNodeLoop.Name.ToLower())
                        {
                            case "serverroot":
                                dc.Directory = xNodeLoop.InnerText;
                                break;
                            case "port":
                                dc.Port = xNodeLoop.InnerText;
                                break;
                        }
                    }
                    return dc;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Unable to obtain Desktop config information.\n{0}", ex.Message));
                }
            }
        }

        private void cbTabInterface_CheckedChanged(object sender, EventArgs e)
        {
            this.bToolbar = true;   // tabbed interface is part of the toolbar
        }

        private void bAddMap_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            ofd.DefaultExt = "rdl";
            ofd.Filter = "Map files (*.xml)|*.xml|" +
                "All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.CheckFileExists = true;
            ofd.Multiselect = true;
            try
            {
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        string nm = Path.GetFileNameWithoutExtension(file);
                        if (!lbMaps.Items.Contains(nm))
                        {
                            lbMaps.Items.Add(nm);
                            bMaps = true;
                        }
                    }
                }
            }
            finally
            {
                ofd.Dispose();
            }
        }

        private void bRemoveMap_Click(object sender, EventArgs e)
        {
            if (lbMaps.SelectedIndex < 0)
                return;
            lbMaps.Items.RemoveAt(lbMaps.SelectedIndex);
            bMaps = true;
            return;
        }
    }

    internal class DesktopConfig
    {
        internal string Directory;
        internal string Port;
    }
}
