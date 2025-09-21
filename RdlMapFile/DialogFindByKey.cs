
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

namespace Majorsilence.Reporting.RdlMapFile
{
    /// <summary>
    /// Summary description for DialogFindByKey.
    /// </summary>
    public partial class DialogFindByKey 
    {

        internal DialogFindByKey(DesignXmlDraw dxd)
        {
            _Draw = dxd;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // populate the keys
            SortedList<string, string> keys = _Draw.GetAllKeys();
            foreach (string key in keys.Keys)
                lbKeyList.Items.Add(key);
            return;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            List<string> select = new List<string>(lbKeyList.SelectedIndices.Count);
            foreach (int si in lbKeyList.SelectedIndices)
            {
                select.Add(lbKeyList.Items[si].ToString());
            }

            _Draw.SelectByKey(select);

            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
