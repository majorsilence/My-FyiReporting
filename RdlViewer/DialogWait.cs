/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
 * Copyright 2011 rcmillard31 (http://www.fyireporting.com/forum/viewtopic.php?t=1103&highlight=threadabortexception)

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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace fyiReporting.RdlViewer
{
    public partial class DialogWait : Form
    {
        private DateTime Started;
        private RdlViewer _viewer;

        public delegate bool CheckStopWaitDialog();
        private CheckStopWaitDialog _checkStopFunction;

        public DialogWait(RdlViewer viewer, CheckStopWaitDialog checkStopFunction)
        {
            InitializeComponent();
            _checkStopFunction = checkStopFunction; 
            _viewer = viewer;
            Started = DateTime.Now;
            timer1.Interval = 1000;
            timer1_Tick(null,null);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan time = DateTime.Now - Started;
            lblTimeTaken.Text = (((time.Days * 24 + time.Hours) * 60) + time.Minutes) + " Minutes " + time.Seconds + " Seconds";
           
            if (_checkStopFunction())
            {
                Close();
            } 
            Application.DoEvents();
        }
    }
}