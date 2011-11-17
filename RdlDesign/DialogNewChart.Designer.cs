using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    internal partial class DialogNewChart : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private DesignXmlDraw _Draw;
private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bCancel;
private System.Windows.Forms.Label label1;
private System.Windows.Forms.ComboBox cbDataSets;
private System.Windows.Forms.Label label2;
private System.Windows.Forms.Label label3;
private System.Windows.Forms.ListBox lbFields;
private System.Windows.Forms.ListBox lbChartCategories;
private System.Windows.Forms.Button bCategoryUp;
private System.Windows.Forms.Button bCategoryDown;
private System.Windows.Forms.Button bCategory;
private System.Windows.Forms.Button bSeries;
private System.Windows.Forms.ListBox lbChartSeries;
private System.Windows.Forms.Button bCategoryDelete;
private System.Windows.Forms.Button bSeriesDelete;
private System.Windows.Forms.Button bSeriesDown;
private System.Windows.Forms.Button bSeriesUp;
private System.Windows.Forms.Label label4;
private System.Windows.Forms.Label lChartData;
private System.Windows.Forms.ComboBox cbChartData;
private System.Windows.Forms.Label label6;
private System.Windows.Forms.ComboBox cbSubType;
private System.Windows.Forms.ComboBox cbChartType;
private System.Windows.Forms.Label label7;
private ComboBox cbChartData2;
private Label lChartData2;
private ComboBox cbChartData3;
private Label lChartData3;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDataSets = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbFields = new System.Windows.Forms.ListBox();
            this.lbChartCategories = new System.Windows.Forms.ListBox();
            this.bCategoryUp = new System.Windows.Forms.Button();
            this.bCategoryDown = new System.Windows.Forms.Button();
            this.bCategory = new System.Windows.Forms.Button();
            this.bSeries = new System.Windows.Forms.Button();
            this.lbChartSeries = new System.Windows.Forms.ListBox();
            this.bCategoryDelete = new System.Windows.Forms.Button();
            this.bSeriesDelete = new System.Windows.Forms.Button();
            this.bSeriesDown = new System.Windows.Forms.Button();
            this.bSeriesUp = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lChartData = new System.Windows.Forms.Label();
            this.cbChartData = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbSubType = new System.Windows.Forms.ComboBox();
            this.cbChartType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbChartData2 = new System.Windows.Forms.ComboBox();
            this.lChartData2 = new System.Windows.Forms.Label();
            this.cbChartData3 = new System.Windows.Forms.ComboBox();
            this.lChartData3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(326, 452);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(90, 27);
            this.bOK.TabIndex = 16;
            this.bOK.Text = "OK";
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(442, 452);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(90, 27);
            this.bCancel.TabIndex = 17;
            this.bCancel.Text = "Cancel";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 27);
            this.label1.TabIndex = 11;
            this.label1.Text = "DataSet";
            // 
            // cbDataSets
            // 
            this.cbDataSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataSets.Location = new System.Drawing.Point(96, 18);
            this.cbDataSets.Name = "cbDataSets";
            this.cbDataSets.Size = new System.Drawing.Size(432, 24);
            this.cbDataSets.TabIndex = 0;
            this.cbDataSets.SelectedIndexChanged += new System.EventHandler(this.cbDataSets_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 27);
            this.label2.TabIndex = 13;
            this.label2.Text = "DataSet Fields";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(269, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 27);
            this.label3.TabIndex = 14;
            this.label3.Text = "Chart Categories (X) Groupings";
            // 
            // lbFields
            // 
            this.lbFields.ItemHeight = 16;
            this.lbFields.Location = new System.Drawing.Point(19, 120);
            this.lbFields.Name = "lbFields";
            this.lbFields.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFields.Size = new System.Drawing.Size(183, 196);
            this.lbFields.TabIndex = 3;
            // 
            // lbChartCategories
            // 
            this.lbChartCategories.ItemHeight = 16;
            this.lbChartCategories.Location = new System.Drawing.Point(278, 120);
            this.lbChartCategories.Name = "lbChartCategories";
            this.lbChartCategories.Size = new System.Drawing.Size(183, 84);
            this.lbChartCategories.TabIndex = 4;
            // 
            // bCategoryUp
            // 
            this.bCategoryUp.Location = new System.Drawing.Point(478, 109);
            this.bCategoryUp.Name = "bCategoryUp";
            this.bCategoryUp.Size = new System.Drawing.Size(58, 28);
            this.bCategoryUp.TabIndex = 5;
            this.bCategoryUp.Text = "Up";
            this.bCategoryUp.Click += new System.EventHandler(this.bCategoryUp_Click);
            // 
            // bCategoryDown
            // 
            this.bCategoryDown.Location = new System.Drawing.Point(478, 143);
            this.bCategoryDown.Name = "bCategoryDown";
            this.bCategoryDown.Size = new System.Drawing.Size(58, 28);
            this.bCategoryDown.TabIndex = 6;
            this.bCategoryDown.Text = "Down";
            this.bCategoryDown.Click += new System.EventHandler(this.bCategoryDown_Click);
            // 
            // bCategory
            // 
            this.bCategory.Location = new System.Drawing.Point(221, 129);
            this.bCategory.Name = "bCategory";
            this.bCategory.Size = new System.Drawing.Size(38, 28);
            this.bCategory.TabIndex = 2;
            this.bCategory.Text = ">";
            this.bCategory.Click += new System.EventHandler(this.bCategory_Click);
            // 
            // bSeries
            // 
            this.bSeries.Location = new System.Drawing.Point(222, 230);
            this.bSeries.Name = "bSeries";
            this.bSeries.Size = new System.Drawing.Size(38, 28);
            this.bSeries.TabIndex = 8;
            this.bSeries.Text = ">";
            this.bSeries.Click += new System.EventHandler(this.bSeries_Click);
            // 
            // lbChartSeries
            // 
            this.lbChartSeries.ItemHeight = 16;
            this.lbChartSeries.Location = new System.Drawing.Point(278, 232);
            this.lbChartSeries.Name = "lbChartSeries";
            this.lbChartSeries.Size = new System.Drawing.Size(183, 84);
            this.lbChartSeries.TabIndex = 9;
            // 
            // bCategoryDelete
            // 
            this.bCategoryDelete.Location = new System.Drawing.Point(478, 176);
            this.bCategoryDelete.Name = "bCategoryDelete";
            this.bCategoryDelete.Size = new System.Drawing.Size(58, 28);
            this.bCategoryDelete.TabIndex = 7;
            this.bCategoryDelete.Text = "Delete";
            this.bCategoryDelete.Click += new System.EventHandler(this.bCategoryDelete_Click);
            // 
            // bSeriesDelete
            // 
            this.bSeriesDelete.Location = new System.Drawing.Point(478, 298);
            this.bSeriesDelete.Name = "bSeriesDelete";
            this.bSeriesDelete.Size = new System.Drawing.Size(58, 27);
            this.bSeriesDelete.TabIndex = 12;
            this.bSeriesDelete.Text = "Delete";
            this.bSeriesDelete.Click += new System.EventHandler(this.bSeriesDelete_Click);
            // 
            // bSeriesDown
            // 
            this.bSeriesDown.Location = new System.Drawing.Point(478, 264);
            this.bSeriesDown.Name = "bSeriesDown";
            this.bSeriesDown.Size = new System.Drawing.Size(58, 28);
            this.bSeriesDown.TabIndex = 11;
            this.bSeriesDown.Text = "Down";
            this.bSeriesDown.Click += new System.EventHandler(this.bSeriesDown_Click);
            // 
            // bSeriesUp
            // 
            this.bSeriesUp.Location = new System.Drawing.Point(478, 230);
            this.bSeriesUp.Name = "bSeriesUp";
            this.bSeriesUp.Size = new System.Drawing.Size(58, 28);
            this.bSeriesUp.TabIndex = 10;
            this.bSeriesUp.Text = "Up";
            this.bSeriesUp.Click += new System.EventHandler(this.bSeriesUp_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(269, 204);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(194, 16);
            this.label4.TabIndex = 31;
            this.label4.Text = "Chart Series";
            // 
            // lChartData
            // 
            this.lChartData.Location = new System.Drawing.Point(19, 363);
            this.lChartData.Name = "lChartData";
            this.lChartData.Size = new System.Drawing.Size(135, 27);
            this.lChartData.TabIndex = 32;
            this.lChartData.Text = "Data Expression";
            // 
            // cbChartData
            // 
            this.cbChartData.Location = new System.Drawing.Point(164, 360);
            this.cbChartData.Name = "cbChartData";
            this.cbChartData.Size = new System.Drawing.Size(297, 24);
            this.cbChartData.TabIndex = 13;
            this.cbChartData.Enter += new System.EventHandler(this.cbChartData_Enter);
            this.cbChartData.TextChanged += new System.EventHandler(this.cbChartData_TextChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(278, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 27);
            this.label6.TabIndex = 36;
            this.label6.Text = "Sub-type";
            // 
            // cbSubType
            // 
            this.cbSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubType.Location = new System.Drawing.Point(374, 55);
            this.cbSubType.Name = "cbSubType";
            this.cbSubType.Size = new System.Drawing.Size(96, 24);
            this.cbSubType.TabIndex = 2;
            // 
            // cbChartType
            // 
            this.cbChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChartType.Items.AddRange(new object[] {
            "Area",
            "Bar",
            "Column",
            "Doughnut",
            "Line",
            "Pie",
            "Bubble",
            "Scatter"});
            this.cbChartType.Location = new System.Drawing.Point(115, 55);
            this.cbChartType.Name = "cbChartType";
            this.cbChartType.Size = new System.Drawing.Size(145, 24);
            this.cbChartType.TabIndex = 1;
            this.cbChartType.SelectedIndexChanged += new System.EventHandler(this.cbChartType_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(19, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 19);
            this.label7.TabIndex = 33;
            this.label7.Text = "Chart Type";
            // 
            // cbChartData2
            // 
            this.cbChartData2.Location = new System.Drawing.Point(164, 391);
            this.cbChartData2.Name = "cbChartData2";
            this.cbChartData2.Size = new System.Drawing.Size(297, 24);
            this.cbChartData2.TabIndex = 14;
            this.cbChartData2.Enter += new System.EventHandler(this.cbChartData_Enter);
            this.cbChartData2.TextChanged += new System.EventHandler(this.cbChartData_TextChanged);
            // 
            // lChartData2
            // 
            this.lChartData2.Location = new System.Drawing.Point(22, 395);
            this.lChartData2.Name = "lChartData2";
            this.lChartData2.Size = new System.Drawing.Size(134, 26);
            this.lChartData2.TabIndex = 38;
            this.lChartData2.Text = "Y Coordinate";
            // 
            // cbChartData3
            // 
            this.cbChartData3.Location = new System.Drawing.Point(164, 422);
            this.cbChartData3.Name = "cbChartData3";
            this.cbChartData3.Size = new System.Drawing.Size(297, 24);
            this.cbChartData3.TabIndex = 15;
            this.cbChartData3.Enter += new System.EventHandler(this.cbChartData_Enter);
            this.cbChartData3.TextChanged += new System.EventHandler(this.cbChartData_TextChanged);
            // 
            // lChartData3
            // 
            this.lChartData3.Location = new System.Drawing.Point(22, 426);
            this.lChartData3.Name = "lChartData3";
            this.lChartData3.Size = new System.Drawing.Size(134, 26);
            this.lChartData3.TabIndex = 40;
            this.lChartData3.Text = "Bubble Size";
            // 
            // DialogNewChart
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(544, 486);
            this.Controls.Add(this.cbChartData3);
            this.Controls.Add(this.lChartData3);
            this.Controls.Add(this.cbChartData2);
            this.Controls.Add(this.lChartData2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbSubType);
            this.Controls.Add(this.cbChartType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbChartData);
            this.Controls.Add(this.lChartData);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bSeriesDelete);
            this.Controls.Add(this.bSeriesDown);
            this.Controls.Add(this.bSeriesUp);
            this.Controls.Add(this.bCategoryDelete);
            this.Controls.Add(this.lbChartSeries);
            this.Controls.Add(this.bSeries);
            this.Controls.Add(this.bCategory);
            this.Controls.Add(this.bCategoryDown);
            this.Controls.Add(this.bCategoryUp);
            this.Controls.Add(this.lbChartCategories);
            this.Controls.Add(this.lbFields);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbDataSets);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogNewChart";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Chart";
            this.ResumeLayout(false);

		}
		#endregion

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
	}
}
