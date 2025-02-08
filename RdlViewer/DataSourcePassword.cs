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
using System.ComponentModel;
using System.Windows.Forms;
using Majorsilence.Reporting.Rdl;
using Majorsilence.Reporting.RdlViewer;

namespace Majorsilence.Reporting.RdlViewer
{
    /// <summary>
    /// Summary description for ZoomTo.
    /// </summary>
    public partial class DataSourcePassword 
    {

        public DataSourcePassword()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

        }

        public string PassPhrase
        {
            get { return tbPassPhrase.Text; }
        }
    }
}
