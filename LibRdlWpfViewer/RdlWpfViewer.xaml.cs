﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibRdlWpfViewer
{
    /// <summary>
    /// Interaction logic for RdlWpfViewer.xaml
    /// </summary>
    public partial class RdlWpfViewer : UserControl
    {
        public RdlWpfViewer()
        {
            InitializeComponent();
        }

        public async Task Rebuild()
        {
            await this.reportViewer.Rebuild();
        }

        public async Task SaveAs(string FileName, Majorsilence.Reporting.Rdl.OutputPresentationType type)
        {
            await this.reportViewer.SaveAs(FileName, type);
        }

        public Uri SourceFile
        {
            get
            {
                return this.reportViewer.SourceFile;
            }
        }

        public async Task SetSourceFile(Uri value)
        {
            await this.reportViewer.SetSourceFile(value);
        }

        public string SourceRdl
        {
            get
            {
                return this.reportViewer.SourceRdl;
            }
        }

        public async Task SetSourceRdl(string value)
        {
            await this.reportViewer.SetSourceRdl(value);
        }

        public string Parameters
        {
            get
            {
                return this.reportViewer.Parameters;
            }
            set
            {
                this.reportViewer.Parameters = value;
            }
        }

        public async Task<Majorsilence.Reporting.Rdl.Report> Report()
        {
            return await this.reportViewer.Report();       
        }

    }

}

