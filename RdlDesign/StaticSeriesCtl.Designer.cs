namespace fyiReporting.RdlDesign
{
    partial class StaticSeriesCtl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StaticSeriesCtl));
			this.label1 = new System.Windows.Forms.Label();
			this.lbDataSeries = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.chkShowLabels = new System.Windows.Forms.CheckBox();
			this.txtSeriesName = new System.Windows.Forms.TextBox();
			this.txtLabelValue = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDel = new System.Windows.Forms.Button();
			this.btnLabelValue = new System.Windows.Forms.Button();
			this.btnDataValue = new System.Windows.Forms.Button();
			this.btnSeriesName = new System.Windows.Forms.Button();
			this.txtDataValue = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbPlotType = new System.Windows.Forms.ComboBox();
			this.chkLeft = new System.Windows.Forms.RadioButton();
			this.chkRight = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.btnUp = new System.Windows.Forms.Button();
			this.btnDown = new System.Windows.Forms.Button();
			this.txtX = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnX = new System.Windows.Forms.Button();
			this.chkMarker = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cbLine = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.colorPicker1 = new fyiReporting.RdlDesign.ColorPicker();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// lbDataSeries
			// 
			resources.ApplyResources(this.lbDataSeries, "lbDataSeries");
			this.lbDataSeries.FormattingEnabled = true;
			this.lbDataSeries.Name = "lbDataSeries";
			this.lbDataSeries.SelectedIndexChanged += new System.EventHandler(this.lbDataSeries_SelectedIndexChanged);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// chkShowLabels
			// 
			resources.ApplyResources(this.chkShowLabels, "chkShowLabels");
			this.chkShowLabels.Name = "chkShowLabels";
			this.chkShowLabels.UseVisualStyleBackColor = true;
			this.chkShowLabels.CheckedChanged += new System.EventHandler(this.chkShowLabels_CheckedChanged);
			// 
			// txtSeriesName
			// 
			resources.ApplyResources(this.txtSeriesName, "txtSeriesName");
			this.txtSeriesName.Name = "txtSeriesName";
			this.txtSeriesName.TextChanged += new System.EventHandler(this.txtSeriesName_TextChanged);
			// 
			// txtLabelValue
			// 
			resources.ApplyResources(this.txtLabelValue, "txtLabelValue");
			this.txtLabelValue.Name = "txtLabelValue";
			this.txtLabelValue.TextChanged += new System.EventHandler(this.txtLabelValue_TextChanged);
			// 
			// btnAdd
			// 
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDel
			// 
			resources.ApplyResources(this.btnDel, "btnDel");
			this.btnDel.Name = "btnDel";
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
			// 
			// btnLabelValue
			// 
			resources.ApplyResources(this.btnLabelValue, "btnLabelValue");
			this.btnLabelValue.Name = "btnLabelValue";
			this.btnLabelValue.UseVisualStyleBackColor = true;
			this.btnLabelValue.Click += new System.EventHandler(this.FunctionButtonClick);
			// 
			// btnDataValue
			// 
			resources.ApplyResources(this.btnDataValue, "btnDataValue");
			this.btnDataValue.Name = "btnDataValue";
			this.btnDataValue.UseVisualStyleBackColor = true;
			this.btnDataValue.Click += new System.EventHandler(this.FunctionButtonClick);
			// 
			// btnSeriesName
			// 
			resources.ApplyResources(this.btnSeriesName, "btnSeriesName");
			this.btnSeriesName.Name = "btnSeriesName";
			this.btnSeriesName.UseVisualStyleBackColor = true;
			this.btnSeriesName.Click += new System.EventHandler(this.FunctionButtonClick);
			// 
			// txtDataValue
			// 
			resources.ApplyResources(this.txtDataValue, "txtDataValue");
			this.txtDataValue.Name = "txtDataValue";
			this.txtDataValue.TextChanged += new System.EventHandler(this.txtDataValue_TextChanged);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// cbPlotType
			// 
			resources.ApplyResources(this.cbPlotType, "cbPlotType");
			this.cbPlotType.FormattingEnabled = true;
			this.cbPlotType.Items.AddRange(new object[] {
            resources.GetString("cbPlotType.Items"),
            resources.GetString("cbPlotType.Items1")});
			this.cbPlotType.Name = "cbPlotType";
			this.cbPlotType.SelectedIndexChanged += new System.EventHandler(this.cbPlotType_SelectedIndexChanged);
			// 
			// chkLeft
			// 
			resources.ApplyResources(this.chkLeft, "chkLeft");
			this.chkLeft.Name = "chkLeft";
			this.chkLeft.TabStop = true;
			this.chkLeft.UseVisualStyleBackColor = true;
			this.chkLeft.CheckedChanged += new System.EventHandler(this.chkLeft_CheckedChanged);
			// 
			// chkRight
			// 
			resources.ApplyResources(this.chkRight, "chkRight");
			this.chkRight.Name = "chkRight";
			this.chkRight.TabStop = true;
			this.chkRight.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// btnUp
			// 
			resources.ApplyResources(this.btnUp, "btnUp");
			this.btnUp.Name = "btnUp";
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// btnDown
			// 
			resources.ApplyResources(this.btnDown, "btnDown");
			this.btnDown.Name = "btnDown";
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// txtX
			// 
			resources.ApplyResources(this.txtX, "txtX");
			this.txtX.Name = "txtX";
			this.txtX.TextChanged += new System.EventHandler(this.txtX_TextChanged);
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// btnX
			// 
			resources.ApplyResources(this.btnX, "btnX");
			this.btnX.Name = "btnX";
			this.btnX.UseVisualStyleBackColor = true;
			this.btnX.Click += new System.EventHandler(this.FunctionButtonClick);
			// 
			// chkMarker
			// 
			resources.ApplyResources(this.chkMarker, "chkMarker");
			this.chkMarker.Name = "chkMarker";
			this.chkMarker.UseVisualStyleBackColor = true;
			this.chkMarker.CheckedChanged += new System.EventHandler(this.chkMarker_CheckedChanged);
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// cbLine
			// 
			resources.ApplyResources(this.cbLine, "cbLine");
			this.cbLine.FormattingEnabled = true;
			this.cbLine.Items.AddRange(new object[] {
            resources.GetString("cbLine.Items"),
            resources.GetString("cbLine.Items1"),
            resources.GetString("cbLine.Items2"),
            resources.GetString("cbLine.Items3"),
            resources.GetString("cbLine.Items4")});
			this.cbLine.Name = "cbLine";
			this.cbLine.SelectedIndexChanged += new System.EventHandler(this.cbLine_SelectedIndexChanged);
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// colorPicker1
			// 
			resources.ApplyResources(this.colorPicker1, "colorPicker1");
			this.colorPicker1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.colorPicker1.DropDownHeight = 1;
			this.colorPicker1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.colorPicker1.FormattingEnabled = true;
			this.colorPicker1.Items.AddRange(new object[] {
            resources.GetString("colorPicker1.Items"),
            resources.GetString("colorPicker1.Items1"),
            resources.GetString("colorPicker1.Items2"),
            resources.GetString("colorPicker1.Items3"),
            resources.GetString("colorPicker1.Items4"),
            resources.GetString("colorPicker1.Items5"),
            resources.GetString("colorPicker1.Items6"),
            resources.GetString("colorPicker1.Items7"),
            resources.GetString("colorPicker1.Items8"),
            resources.GetString("colorPicker1.Items9"),
            resources.GetString("colorPicker1.Items10"),
            resources.GetString("colorPicker1.Items11"),
            resources.GetString("colorPicker1.Items12"),
            resources.GetString("colorPicker1.Items13"),
            resources.GetString("colorPicker1.Items14"),
            resources.GetString("colorPicker1.Items15"),
            resources.GetString("colorPicker1.Items16"),
            resources.GetString("colorPicker1.Items17"),
            resources.GetString("colorPicker1.Items18"),
            resources.GetString("colorPicker1.Items19"),
            resources.GetString("colorPicker1.Items20"),
            resources.GetString("colorPicker1.Items21"),
            resources.GetString("colorPicker1.Items22"),
            resources.GetString("colorPicker1.Items23"),
            resources.GetString("colorPicker1.Items24"),
            resources.GetString("colorPicker1.Items25"),
            resources.GetString("colorPicker1.Items26"),
            resources.GetString("colorPicker1.Items27"),
            resources.GetString("colorPicker1.Items28"),
            resources.GetString("colorPicker1.Items29"),
            resources.GetString("colorPicker1.Items30"),
            resources.GetString("colorPicker1.Items31"),
            resources.GetString("colorPicker1.Items32"),
            resources.GetString("colorPicker1.Items33"),
            resources.GetString("colorPicker1.Items34"),
            resources.GetString("colorPicker1.Items35"),
            resources.GetString("colorPicker1.Items36"),
            resources.GetString("colorPicker1.Items37"),
            resources.GetString("colorPicker1.Items38"),
            resources.GetString("colorPicker1.Items39"),
            resources.GetString("colorPicker1.Items40"),
            resources.GetString("colorPicker1.Items41"),
            resources.GetString("colorPicker1.Items42"),
            resources.GetString("colorPicker1.Items43"),
            resources.GetString("colorPicker1.Items44"),
            resources.GetString("colorPicker1.Items45"),
            resources.GetString("colorPicker1.Items46"),
            resources.GetString("colorPicker1.Items47"),
            resources.GetString("colorPicker1.Items48"),
            resources.GetString("colorPicker1.Items49"),
            resources.GetString("colorPicker1.Items50"),
            resources.GetString("colorPicker1.Items51"),
            resources.GetString("colorPicker1.Items52"),
            resources.GetString("colorPicker1.Items53"),
            resources.GetString("colorPicker1.Items54"),
            resources.GetString("colorPicker1.Items55"),
            resources.GetString("colorPicker1.Items56"),
            resources.GetString("colorPicker1.Items57"),
            resources.GetString("colorPicker1.Items58"),
            resources.GetString("colorPicker1.Items59"),
            resources.GetString("colorPicker1.Items60"),
            resources.GetString("colorPicker1.Items61"),
            resources.GetString("colorPicker1.Items62"),
            resources.GetString("colorPicker1.Items63"),
            resources.GetString("colorPicker1.Items64"),
            resources.GetString("colorPicker1.Items65"),
            resources.GetString("colorPicker1.Items66"),
            resources.GetString("colorPicker1.Items67"),
            resources.GetString("colorPicker1.Items68"),
            resources.GetString("colorPicker1.Items69"),
            resources.GetString("colorPicker1.Items70"),
            resources.GetString("colorPicker1.Items71"),
            resources.GetString("colorPicker1.Items72"),
            resources.GetString("colorPicker1.Items73"),
            resources.GetString("colorPicker1.Items74"),
            resources.GetString("colorPicker1.Items75"),
            resources.GetString("colorPicker1.Items76"),
            resources.GetString("colorPicker1.Items77"),
            resources.GetString("colorPicker1.Items78"),
            resources.GetString("colorPicker1.Items79"),
            resources.GetString("colorPicker1.Items80"),
            resources.GetString("colorPicker1.Items81"),
            resources.GetString("colorPicker1.Items82"),
            resources.GetString("colorPicker1.Items83"),
            resources.GetString("colorPicker1.Items84"),
            resources.GetString("colorPicker1.Items85"),
            resources.GetString("colorPicker1.Items86"),
            resources.GetString("colorPicker1.Items87"),
            resources.GetString("colorPicker1.Items88"),
            resources.GetString("colorPicker1.Items89"),
            resources.GetString("colorPicker1.Items90"),
            resources.GetString("colorPicker1.Items91"),
            resources.GetString("colorPicker1.Items92"),
            resources.GetString("colorPicker1.Items93"),
            resources.GetString("colorPicker1.Items94"),
            resources.GetString("colorPicker1.Items95"),
            resources.GetString("colorPicker1.Items96"),
            resources.GetString("colorPicker1.Items97"),
            resources.GetString("colorPicker1.Items98"),
            resources.GetString("colorPicker1.Items99"),
            resources.GetString("colorPicker1.Items100"),
            resources.GetString("colorPicker1.Items101"),
            resources.GetString("colorPicker1.Items102"),
            resources.GetString("colorPicker1.Items103"),
            resources.GetString("colorPicker1.Items104"),
            resources.GetString("colorPicker1.Items105"),
            resources.GetString("colorPicker1.Items106"),
            resources.GetString("colorPicker1.Items107"),
            resources.GetString("colorPicker1.Items108"),
            resources.GetString("colorPicker1.Items109"),
            resources.GetString("colorPicker1.Items110"),
            resources.GetString("colorPicker1.Items111"),
            resources.GetString("colorPicker1.Items112"),
            resources.GetString("colorPicker1.Items113"),
            resources.GetString("colorPicker1.Items114"),
            resources.GetString("colorPicker1.Items115"),
            resources.GetString("colorPicker1.Items116"),
            resources.GetString("colorPicker1.Items117"),
            resources.GetString("colorPicker1.Items118"),
            resources.GetString("colorPicker1.Items119"),
            resources.GetString("colorPicker1.Items120"),
            resources.GetString("colorPicker1.Items121"),
            resources.GetString("colorPicker1.Items122"),
            resources.GetString("colorPicker1.Items123"),
            resources.GetString("colorPicker1.Items124"),
            resources.GetString("colorPicker1.Items125"),
            resources.GetString("colorPicker1.Items126"),
            resources.GetString("colorPicker1.Items127"),
            resources.GetString("colorPicker1.Items128"),
            resources.GetString("colorPicker1.Items129"),
            resources.GetString("colorPicker1.Items130"),
            resources.GetString("colorPicker1.Items131"),
            resources.GetString("colorPicker1.Items132"),
            resources.GetString("colorPicker1.Items133"),
            resources.GetString("colorPicker1.Items134"),
            resources.GetString("colorPicker1.Items135"),
            resources.GetString("colorPicker1.Items136"),
            resources.GetString("colorPicker1.Items137"),
            resources.GetString("colorPicker1.Items138"),
            resources.GetString("colorPicker1.Items139"),
            resources.GetString("colorPicker1.Items140"),
            resources.GetString("colorPicker1.Items141"),
            resources.GetString("colorPicker1.Items142"),
            resources.GetString("colorPicker1.Items143"),
            resources.GetString("colorPicker1.Items144"),
            resources.GetString("colorPicker1.Items145"),
            resources.GetString("colorPicker1.Items146"),
            resources.GetString("colorPicker1.Items147"),
            resources.GetString("colorPicker1.Items148"),
            resources.GetString("colorPicker1.Items149"),
            resources.GetString("colorPicker1.Items150"),
            resources.GetString("colorPicker1.Items151"),
            resources.GetString("colorPicker1.Items152"),
            resources.GetString("colorPicker1.Items153"),
            resources.GetString("colorPicker1.Items154"),
            resources.GetString("colorPicker1.Items155"),
            resources.GetString("colorPicker1.Items156"),
            resources.GetString("colorPicker1.Items157"),
            resources.GetString("colorPicker1.Items158"),
            resources.GetString("colorPicker1.Items159"),
            resources.GetString("colorPicker1.Items160"),
            resources.GetString("colorPicker1.Items161"),
            resources.GetString("colorPicker1.Items162"),
            resources.GetString("colorPicker1.Items163"),
            resources.GetString("colorPicker1.Items164"),
            resources.GetString("colorPicker1.Items165"),
            resources.GetString("colorPicker1.Items166"),
            resources.GetString("colorPicker1.Items167"),
            resources.GetString("colorPicker1.Items168"),
            resources.GetString("colorPicker1.Items169"),
            resources.GetString("colorPicker1.Items170"),
            resources.GetString("colorPicker1.Items171"),
            resources.GetString("colorPicker1.Items172"),
            resources.GetString("colorPicker1.Items173"),
            resources.GetString("colorPicker1.Items174"),
            resources.GetString("colorPicker1.Items175"),
            resources.GetString("colorPicker1.Items176"),
            resources.GetString("colorPicker1.Items177"),
            resources.GetString("colorPicker1.Items178"),
            resources.GetString("colorPicker1.Items179"),
            resources.GetString("colorPicker1.Items180"),
            resources.GetString("colorPicker1.Items181"),
            resources.GetString("colorPicker1.Items182"),
            resources.GetString("colorPicker1.Items183"),
            resources.GetString("colorPicker1.Items184"),
            resources.GetString("colorPicker1.Items185"),
            resources.GetString("colorPicker1.Items186"),
            resources.GetString("colorPicker1.Items187"),
            resources.GetString("colorPicker1.Items188"),
            resources.GetString("colorPicker1.Items189"),
            resources.GetString("colorPicker1.Items190"),
            resources.GetString("colorPicker1.Items191"),
            resources.GetString("colorPicker1.Items192"),
            resources.GetString("colorPicker1.Items193"),
            resources.GetString("colorPicker1.Items194"),
            resources.GetString("colorPicker1.Items195"),
            resources.GetString("colorPicker1.Items196"),
            resources.GetString("colorPicker1.Items197"),
            resources.GetString("colorPicker1.Items198"),
            resources.GetString("colorPicker1.Items199"),
            resources.GetString("colorPicker1.Items200"),
            resources.GetString("colorPicker1.Items201"),
            resources.GetString("colorPicker1.Items202"),
            resources.GetString("colorPicker1.Items203"),
            resources.GetString("colorPicker1.Items204"),
            resources.GetString("colorPicker1.Items205"),
            resources.GetString("colorPicker1.Items206"),
            resources.GetString("colorPicker1.Items207"),
            resources.GetString("colorPicker1.Items208"),
            resources.GetString("colorPicker1.Items209"),
            resources.GetString("colorPicker1.Items210"),
            resources.GetString("colorPicker1.Items211"),
            resources.GetString("colorPicker1.Items212"),
            resources.GetString("colorPicker1.Items213"),
            resources.GetString("colorPicker1.Items214"),
            resources.GetString("colorPicker1.Items215"),
            resources.GetString("colorPicker1.Items216"),
            resources.GetString("colorPicker1.Items217"),
            resources.GetString("colorPicker1.Items218"),
            resources.GetString("colorPicker1.Items219"),
            resources.GetString("colorPicker1.Items220"),
            resources.GetString("colorPicker1.Items221"),
            resources.GetString("colorPicker1.Items222"),
            resources.GetString("colorPicker1.Items223"),
            resources.GetString("colorPicker1.Items224"),
            resources.GetString("colorPicker1.Items225"),
            resources.GetString("colorPicker1.Items226"),
            resources.GetString("colorPicker1.Items227"),
            resources.GetString("colorPicker1.Items228"),
            resources.GetString("colorPicker1.Items229"),
            resources.GetString("colorPicker1.Items230"),
            resources.GetString("colorPicker1.Items231"),
            resources.GetString("colorPicker1.Items232"),
            resources.GetString("colorPicker1.Items233"),
            resources.GetString("colorPicker1.Items234"),
            resources.GetString("colorPicker1.Items235"),
            resources.GetString("colorPicker1.Items236"),
            resources.GetString("colorPicker1.Items237"),
            resources.GetString("colorPicker1.Items238"),
            resources.GetString("colorPicker1.Items239"),
            resources.GetString("colorPicker1.Items240"),
            resources.GetString("colorPicker1.Items241"),
            resources.GetString("colorPicker1.Items242"),
            resources.GetString("colorPicker1.Items243"),
            resources.GetString("colorPicker1.Items244"),
            resources.GetString("colorPicker1.Items245"),
            resources.GetString("colorPicker1.Items246"),
            resources.GetString("colorPicker1.Items247"),
            resources.GetString("colorPicker1.Items248"),
            resources.GetString("colorPicker1.Items249"),
            resources.GetString("colorPicker1.Items250"),
            resources.GetString("colorPicker1.Items251"),
            resources.GetString("colorPicker1.Items252"),
            resources.GetString("colorPicker1.Items253"),
            resources.GetString("colorPicker1.Items254"),
            resources.GetString("colorPicker1.Items255"),
            resources.GetString("colorPicker1.Items256"),
            resources.GetString("colorPicker1.Items257"),
            resources.GetString("colorPicker1.Items258"),
            resources.GetString("colorPicker1.Items259"),
            resources.GetString("colorPicker1.Items260"),
            resources.GetString("colorPicker1.Items261"),
            resources.GetString("colorPicker1.Items262"),
            resources.GetString("colorPicker1.Items263"),
            resources.GetString("colorPicker1.Items264"),
            resources.GetString("colorPicker1.Items265"),
            resources.GetString("colorPicker1.Items266"),
            resources.GetString("colorPicker1.Items267"),
            resources.GetString("colorPicker1.Items268"),
            resources.GetString("colorPicker1.Items269"),
            resources.GetString("colorPicker1.Items270"),
            resources.GetString("colorPicker1.Items271"),
            resources.GetString("colorPicker1.Items272"),
            resources.GetString("colorPicker1.Items273"),
            resources.GetString("colorPicker1.Items274"),
            resources.GetString("colorPicker1.Items275"),
            resources.GetString("colorPicker1.Items276"),
            resources.GetString("colorPicker1.Items277"),
            resources.GetString("colorPicker1.Items278"),
            resources.GetString("colorPicker1.Items279"),
            resources.GetString("colorPicker1.Items280"),
            resources.GetString("colorPicker1.Items281"),
            resources.GetString("colorPicker1.Items282"),
            resources.GetString("colorPicker1.Items283"),
            resources.GetString("colorPicker1.Items284"),
            resources.GetString("colorPicker1.Items285"),
            resources.GetString("colorPicker1.Items286"),
            resources.GetString("colorPicker1.Items287"),
            resources.GetString("colorPicker1.Items288"),
            resources.GetString("colorPicker1.Items289"),
            resources.GetString("colorPicker1.Items290"),
            resources.GetString("colorPicker1.Items291"),
            resources.GetString("colorPicker1.Items292"),
            resources.GetString("colorPicker1.Items293"),
            resources.GetString("colorPicker1.Items294"),
            resources.GetString("colorPicker1.Items295"),
            resources.GetString("colorPicker1.Items296"),
            resources.GetString("colorPicker1.Items297"),
            resources.GetString("colorPicker1.Items298"),
            resources.GetString("colorPicker1.Items299"),
            resources.GetString("colorPicker1.Items300"),
            resources.GetString("colorPicker1.Items301"),
            resources.GetString("colorPicker1.Items302"),
            resources.GetString("colorPicker1.Items303"),
            resources.GetString("colorPicker1.Items304"),
            resources.GetString("colorPicker1.Items305"),
            resources.GetString("colorPicker1.Items306"),
            resources.GetString("colorPicker1.Items307"),
            resources.GetString("colorPicker1.Items308"),
            resources.GetString("colorPicker1.Items309"),
            resources.GetString("colorPicker1.Items310"),
            resources.GetString("colorPicker1.Items311"),
            resources.GetString("colorPicker1.Items312"),
            resources.GetString("colorPicker1.Items313"),
            resources.GetString("colorPicker1.Items314"),
            resources.GetString("colorPicker1.Items315"),
            resources.GetString("colorPicker1.Items316"),
            resources.GetString("colorPicker1.Items317"),
            resources.GetString("colorPicker1.Items318"),
            resources.GetString("colorPicker1.Items319"),
            resources.GetString("colorPicker1.Items320"),
            resources.GetString("colorPicker1.Items321"),
            resources.GetString("colorPicker1.Items322"),
            resources.GetString("colorPicker1.Items323"),
            resources.GetString("colorPicker1.Items324"),
            resources.GetString("colorPicker1.Items325"),
            resources.GetString("colorPicker1.Items326"),
            resources.GetString("colorPicker1.Items327"),
            resources.GetString("colorPicker1.Items328"),
            resources.GetString("colorPicker1.Items329"),
            resources.GetString("colorPicker1.Items330"),
            resources.GetString("colorPicker1.Items331"),
            resources.GetString("colorPicker1.Items332"),
            resources.GetString("colorPicker1.Items333"),
            resources.GetString("colorPicker1.Items334"),
            resources.GetString("colorPicker1.Items335"),
            resources.GetString("colorPicker1.Items336"),
            resources.GetString("colorPicker1.Items337"),
            resources.GetString("colorPicker1.Items338"),
            resources.GetString("colorPicker1.Items339"),
            resources.GetString("colorPicker1.Items340"),
            resources.GetString("colorPicker1.Items341"),
            resources.GetString("colorPicker1.Items342"),
            resources.GetString("colorPicker1.Items343"),
            resources.GetString("colorPicker1.Items344"),
            resources.GetString("colorPicker1.Items345"),
            resources.GetString("colorPicker1.Items346"),
            resources.GetString("colorPicker1.Items347"),
            resources.GetString("colorPicker1.Items348"),
            resources.GetString("colorPicker1.Items349"),
            resources.GetString("colorPicker1.Items350"),
            resources.GetString("colorPicker1.Items351"),
            resources.GetString("colorPicker1.Items352"),
            resources.GetString("colorPicker1.Items353"),
            resources.GetString("colorPicker1.Items354"),
            resources.GetString("colorPicker1.Items355"),
            resources.GetString("colorPicker1.Items356"),
            resources.GetString("colorPicker1.Items357"),
            resources.GetString("colorPicker1.Items358"),
            resources.GetString("colorPicker1.Items359"),
            resources.GetString("colorPicker1.Items360"),
            resources.GetString("colorPicker1.Items361"),
            resources.GetString("colorPicker1.Items362"),
            resources.GetString("colorPicker1.Items363"),
            resources.GetString("colorPicker1.Items364"),
            resources.GetString("colorPicker1.Items365"),
            resources.GetString("colorPicker1.Items366"),
            resources.GetString("colorPicker1.Items367"),
            resources.GetString("colorPicker1.Items368"),
            resources.GetString("colorPicker1.Items369"),
            resources.GetString("colorPicker1.Items370"),
            resources.GetString("colorPicker1.Items371"),
            resources.GetString("colorPicker1.Items372"),
            resources.GetString("colorPicker1.Items373"),
            resources.GetString("colorPicker1.Items374"),
            resources.GetString("colorPicker1.Items375"),
            resources.GetString("colorPicker1.Items376"),
            resources.GetString("colorPicker1.Items377"),
            resources.GetString("colorPicker1.Items378"),
            resources.GetString("colorPicker1.Items379"),
            resources.GetString("colorPicker1.Items380"),
            resources.GetString("colorPicker1.Items381"),
            resources.GetString("colorPicker1.Items382"),
            resources.GetString("colorPicker1.Items383"),
            resources.GetString("colorPicker1.Items384"),
            resources.GetString("colorPicker1.Items385"),
            resources.GetString("colorPicker1.Items386"),
            resources.GetString("colorPicker1.Items387"),
            resources.GetString("colorPicker1.Items388"),
            resources.GetString("colorPicker1.Items389"),
            resources.GetString("colorPicker1.Items390"),
            resources.GetString("colorPicker1.Items391"),
            resources.GetString("colorPicker1.Items392"),
            resources.GetString("colorPicker1.Items393"),
            resources.GetString("colorPicker1.Items394"),
            resources.GetString("colorPicker1.Items395"),
            resources.GetString("colorPicker1.Items396"),
            resources.GetString("colorPicker1.Items397"),
            resources.GetString("colorPicker1.Items398"),
            resources.GetString("colorPicker1.Items399"),
            resources.GetString("colorPicker1.Items400"),
            resources.GetString("colorPicker1.Items401"),
            resources.GetString("colorPicker1.Items402"),
            resources.GetString("colorPicker1.Items403"),
            resources.GetString("colorPicker1.Items404"),
            resources.GetString("colorPicker1.Items405"),
            resources.GetString("colorPicker1.Items406"),
            resources.GetString("colorPicker1.Items407"),
            resources.GetString("colorPicker1.Items408"),
            resources.GetString("colorPicker1.Items409"),
            resources.GetString("colorPicker1.Items410"),
            resources.GetString("colorPicker1.Items411"),
            resources.GetString("colorPicker1.Items412"),
            resources.GetString("colorPicker1.Items413"),
            resources.GetString("colorPicker1.Items414"),
            resources.GetString("colorPicker1.Items415"),
            resources.GetString("colorPicker1.Items416"),
            resources.GetString("colorPicker1.Items417"),
            resources.GetString("colorPicker1.Items418"),
            resources.GetString("colorPicker1.Items419"),
            resources.GetString("colorPicker1.Items420"),
            resources.GetString("colorPicker1.Items421"),
            resources.GetString("colorPicker1.Items422"),
            resources.GetString("colorPicker1.Items423"),
            resources.GetString("colorPicker1.Items424"),
            resources.GetString("colorPicker1.Items425"),
            resources.GetString("colorPicker1.Items426"),
            resources.GetString("colorPicker1.Items427"),
            resources.GetString("colorPicker1.Items428"),
            resources.GetString("colorPicker1.Items429"),
            resources.GetString("colorPicker1.Items430"),
            resources.GetString("colorPicker1.Items431"),
            resources.GetString("colorPicker1.Items432"),
            resources.GetString("colorPicker1.Items433"),
            resources.GetString("colorPicker1.Items434"),
            resources.GetString("colorPicker1.Items435"),
            resources.GetString("colorPicker1.Items436"),
            resources.GetString("colorPicker1.Items437"),
            resources.GetString("colorPicker1.Items438"),
            resources.GetString("colorPicker1.Items439"),
            resources.GetString("colorPicker1.Items440"),
            resources.GetString("colorPicker1.Items441"),
            resources.GetString("colorPicker1.Items442"),
            resources.GetString("colorPicker1.Items443"),
            resources.GetString("colorPicker1.Items444"),
            resources.GetString("colorPicker1.Items445"),
            resources.GetString("colorPicker1.Items446"),
            resources.GetString("colorPicker1.Items447"),
            resources.GetString("colorPicker1.Items448"),
            resources.GetString("colorPicker1.Items449"),
            resources.GetString("colorPicker1.Items450"),
            resources.GetString("colorPicker1.Items451"),
            resources.GetString("colorPicker1.Items452"),
            resources.GetString("colorPicker1.Items453"),
            resources.GetString("colorPicker1.Items454"),
            resources.GetString("colorPicker1.Items455"),
            resources.GetString("colorPicker1.Items456"),
            resources.GetString("colorPicker1.Items457"),
            resources.GetString("colorPicker1.Items458"),
            resources.GetString("colorPicker1.Items459"),
            resources.GetString("colorPicker1.Items460"),
            resources.GetString("colorPicker1.Items461"),
            resources.GetString("colorPicker1.Items462"),
            resources.GetString("colorPicker1.Items463"),
            resources.GetString("colorPicker1.Items464"),
            resources.GetString("colorPicker1.Items465"),
            resources.GetString("colorPicker1.Items466"),
            resources.GetString("colorPicker1.Items467"),
            resources.GetString("colorPicker1.Items468"),
            resources.GetString("colorPicker1.Items469"),
            resources.GetString("colorPicker1.Items470"),
            resources.GetString("colorPicker1.Items471"),
            resources.GetString("colorPicker1.Items472"),
            resources.GetString("colorPicker1.Items473"),
            resources.GetString("colorPicker1.Items474"),
            resources.GetString("colorPicker1.Items475"),
            resources.GetString("colorPicker1.Items476"),
            resources.GetString("colorPicker1.Items477"),
            resources.GetString("colorPicker1.Items478"),
            resources.GetString("colorPicker1.Items479"),
            resources.GetString("colorPicker1.Items480"),
            resources.GetString("colorPicker1.Items481"),
            resources.GetString("colorPicker1.Items482"),
            resources.GetString("colorPicker1.Items483"),
            resources.GetString("colorPicker1.Items484"),
            resources.GetString("colorPicker1.Items485"),
            resources.GetString("colorPicker1.Items486"),
            resources.GetString("colorPicker1.Items487"),
            resources.GetString("colorPicker1.Items488"),
            resources.GetString("colorPicker1.Items489"),
            resources.GetString("colorPicker1.Items490"),
            resources.GetString("colorPicker1.Items491"),
            resources.GetString("colorPicker1.Items492"),
            resources.GetString("colorPicker1.Items493"),
            resources.GetString("colorPicker1.Items494"),
            resources.GetString("colorPicker1.Items495"),
            resources.GetString("colorPicker1.Items496"),
            resources.GetString("colorPicker1.Items497"),
            resources.GetString("colorPicker1.Items498"),
            resources.GetString("colorPicker1.Items499"),
            resources.GetString("colorPicker1.Items500"),
            resources.GetString("colorPicker1.Items501"),
            resources.GetString("colorPicker1.Items502"),
            resources.GetString("colorPicker1.Items503"),
            resources.GetString("colorPicker1.Items504"),
            resources.GetString("colorPicker1.Items505"),
            resources.GetString("colorPicker1.Items506"),
            resources.GetString("colorPicker1.Items507"),
            resources.GetString("colorPicker1.Items508"),
            resources.GetString("colorPicker1.Items509"),
            resources.GetString("colorPicker1.Items510"),
            resources.GetString("colorPicker1.Items511"),
            resources.GetString("colorPicker1.Items512"),
            resources.GetString("colorPicker1.Items513"),
            resources.GetString("colorPicker1.Items514"),
            resources.GetString("colorPicker1.Items515"),
            resources.GetString("colorPicker1.Items516"),
            resources.GetString("colorPicker1.Items517"),
            resources.GetString("colorPicker1.Items518"),
            resources.GetString("colorPicker1.Items519"),
            resources.GetString("colorPicker1.Items520"),
            resources.GetString("colorPicker1.Items521"),
            resources.GetString("colorPicker1.Items522"),
            resources.GetString("colorPicker1.Items523"),
            resources.GetString("colorPicker1.Items524"),
            resources.GetString("colorPicker1.Items525"),
            resources.GetString("colorPicker1.Items526"),
            resources.GetString("colorPicker1.Items527"),
            resources.GetString("colorPicker1.Items528"),
            resources.GetString("colorPicker1.Items529"),
            resources.GetString("colorPicker1.Items530"),
            resources.GetString("colorPicker1.Items531"),
            resources.GetString("colorPicker1.Items532"),
            resources.GetString("colorPicker1.Items533"),
            resources.GetString("colorPicker1.Items534"),
            resources.GetString("colorPicker1.Items535"),
            resources.GetString("colorPicker1.Items536"),
            resources.GetString("colorPicker1.Items537"),
            resources.GetString("colorPicker1.Items538"),
            resources.GetString("colorPicker1.Items539"),
            resources.GetString("colorPicker1.Items540"),
            resources.GetString("colorPicker1.Items541"),
            resources.GetString("colorPicker1.Items542"),
            resources.GetString("colorPicker1.Items543"),
            resources.GetString("colorPicker1.Items544"),
            resources.GetString("colorPicker1.Items545"),
            resources.GetString("colorPicker1.Items546"),
            resources.GetString("colorPicker1.Items547"),
            resources.GetString("colorPicker1.Items548"),
            resources.GetString("colorPicker1.Items549"),
            resources.GetString("colorPicker1.Items550"),
            resources.GetString("colorPicker1.Items551"),
            resources.GetString("colorPicker1.Items552"),
            resources.GetString("colorPicker1.Items553"),
            resources.GetString("colorPicker1.Items554"),
            resources.GetString("colorPicker1.Items555"),
            resources.GetString("colorPicker1.Items556"),
            resources.GetString("colorPicker1.Items557"),
            resources.GetString("colorPicker1.Items558"),
            resources.GetString("colorPicker1.Items559"),
            resources.GetString("colorPicker1.Items560"),
            resources.GetString("colorPicker1.Items561"),
            resources.GetString("colorPicker1.Items562"),
            resources.GetString("colorPicker1.Items563"),
            resources.GetString("colorPicker1.Items564"),
            resources.GetString("colorPicker1.Items565"),
            resources.GetString("colorPicker1.Items566"),
            resources.GetString("colorPicker1.Items567"),
            resources.GetString("colorPicker1.Items568"),
            resources.GetString("colorPicker1.Items569"),
            resources.GetString("colorPicker1.Items570"),
            resources.GetString("colorPicker1.Items571"),
            resources.GetString("colorPicker1.Items572"),
            resources.GetString("colorPicker1.Items573"),
            resources.GetString("colorPicker1.Items574"),
            resources.GetString("colorPicker1.Items575"),
            resources.GetString("colorPicker1.Items576"),
            resources.GetString("colorPicker1.Items577"),
            resources.GetString("colorPicker1.Items578"),
            resources.GetString("colorPicker1.Items579"),
            resources.GetString("colorPicker1.Items580"),
            resources.GetString("colorPicker1.Items581"),
            resources.GetString("colorPicker1.Items582"),
            resources.GetString("colorPicker1.Items583"),
            resources.GetString("colorPicker1.Items584"),
            resources.GetString("colorPicker1.Items585"),
            resources.GetString("colorPicker1.Items586"),
            resources.GetString("colorPicker1.Items587"),
            resources.GetString("colorPicker1.Items588"),
            resources.GetString("colorPicker1.Items589"),
            resources.GetString("colorPicker1.Items590"),
            resources.GetString("colorPicker1.Items591"),
            resources.GetString("colorPicker1.Items592"),
            resources.GetString("colorPicker1.Items593"),
            resources.GetString("colorPicker1.Items594"),
            resources.GetString("colorPicker1.Items595"),
            resources.GetString("colorPicker1.Items596"),
            resources.GetString("colorPicker1.Items597"),
            resources.GetString("colorPicker1.Items598"),
            resources.GetString("colorPicker1.Items599"),
            resources.GetString("colorPicker1.Items600"),
            resources.GetString("colorPicker1.Items601"),
            resources.GetString("colorPicker1.Items602"),
            resources.GetString("colorPicker1.Items603"),
            resources.GetString("colorPicker1.Items604"),
            resources.GetString("colorPicker1.Items605"),
            resources.GetString("colorPicker1.Items606"),
            resources.GetString("colorPicker1.Items607"),
            resources.GetString("colorPicker1.Items608"),
            resources.GetString("colorPicker1.Items609"),
            resources.GetString("colorPicker1.Items610"),
            resources.GetString("colorPicker1.Items611"),
            resources.GetString("colorPicker1.Items612"),
            resources.GetString("colorPicker1.Items613"),
            resources.GetString("colorPicker1.Items614"),
            resources.GetString("colorPicker1.Items615"),
            resources.GetString("colorPicker1.Items616"),
            resources.GetString("colorPicker1.Items617"),
            resources.GetString("colorPicker1.Items618"),
            resources.GetString("colorPicker1.Items619"),
            resources.GetString("colorPicker1.Items620"),
            resources.GetString("colorPicker1.Items621"),
            resources.GetString("colorPicker1.Items622"),
            resources.GetString("colorPicker1.Items623"),
            resources.GetString("colorPicker1.Items624"),
            resources.GetString("colorPicker1.Items625"),
            resources.GetString("colorPicker1.Items626"),
            resources.GetString("colorPicker1.Items627"),
            resources.GetString("colorPicker1.Items628"),
            resources.GetString("colorPicker1.Items629"),
            resources.GetString("colorPicker1.Items630"),
            resources.GetString("colorPicker1.Items631"),
            resources.GetString("colorPicker1.Items632"),
            resources.GetString("colorPicker1.Items633"),
            resources.GetString("colorPicker1.Items634"),
            resources.GetString("colorPicker1.Items635"),
            resources.GetString("colorPicker1.Items636"),
            resources.GetString("colorPicker1.Items637"),
            resources.GetString("colorPicker1.Items638"),
            resources.GetString("colorPicker1.Items639"),
            resources.GetString("colorPicker1.Items640"),
            resources.GetString("colorPicker1.Items641"),
            resources.GetString("colorPicker1.Items642"),
            resources.GetString("colorPicker1.Items643"),
            resources.GetString("colorPicker1.Items644"),
            resources.GetString("colorPicker1.Items645"),
            resources.GetString("colorPicker1.Items646"),
            resources.GetString("colorPicker1.Items647"),
            resources.GetString("colorPicker1.Items648"),
            resources.GetString("colorPicker1.Items649"),
            resources.GetString("colorPicker1.Items650"),
            resources.GetString("colorPicker1.Items651"),
            resources.GetString("colorPicker1.Items652"),
            resources.GetString("colorPicker1.Items653"),
            resources.GetString("colorPicker1.Items654"),
            resources.GetString("colorPicker1.Items655"),
            resources.GetString("colorPicker1.Items656"),
            resources.GetString("colorPicker1.Items657"),
            resources.GetString("colorPicker1.Items658"),
            resources.GetString("colorPicker1.Items659"),
            resources.GetString("colorPicker1.Items660"),
            resources.GetString("colorPicker1.Items661"),
            resources.GetString("colorPicker1.Items662"),
            resources.GetString("colorPicker1.Items663"),
            resources.GetString("colorPicker1.Items664"),
            resources.GetString("colorPicker1.Items665"),
            resources.GetString("colorPicker1.Items666"),
            resources.GetString("colorPicker1.Items667"),
            resources.GetString("colorPicker1.Items668"),
            resources.GetString("colorPicker1.Items669"),
            resources.GetString("colorPicker1.Items670"),
            resources.GetString("colorPicker1.Items671"),
            resources.GetString("colorPicker1.Items672"),
            resources.GetString("colorPicker1.Items673"),
            resources.GetString("colorPicker1.Items674"),
            resources.GetString("colorPicker1.Items675"),
            resources.GetString("colorPicker1.Items676"),
            resources.GetString("colorPicker1.Items677"),
            resources.GetString("colorPicker1.Items678"),
            resources.GetString("colorPicker1.Items679"),
            resources.GetString("colorPicker1.Items680"),
            resources.GetString("colorPicker1.Items681"),
            resources.GetString("colorPicker1.Items682"),
            resources.GetString("colorPicker1.Items683"),
            resources.GetString("colorPicker1.Items684"),
            resources.GetString("colorPicker1.Items685"),
            resources.GetString("colorPicker1.Items686"),
            resources.GetString("colorPicker1.Items687"),
            resources.GetString("colorPicker1.Items688"),
            resources.GetString("colorPicker1.Items689"),
            resources.GetString("colorPicker1.Items690"),
            resources.GetString("colorPicker1.Items691"),
            resources.GetString("colorPicker1.Items692"),
            resources.GetString("colorPicker1.Items693"),
            resources.GetString("colorPicker1.Items694"),
            resources.GetString("colorPicker1.Items695"),
            resources.GetString("colorPicker1.Items696"),
            resources.GetString("colorPicker1.Items697"),
            resources.GetString("colorPicker1.Items698"),
            resources.GetString("colorPicker1.Items699"),
            resources.GetString("colorPicker1.Items700"),
            resources.GetString("colorPicker1.Items701"),
            resources.GetString("colorPicker1.Items702"),
            resources.GetString("colorPicker1.Items703"),
            resources.GetString("colorPicker1.Items704"),
            resources.GetString("colorPicker1.Items705"),
            resources.GetString("colorPicker1.Items706"),
            resources.GetString("colorPicker1.Items707"),
            resources.GetString("colorPicker1.Items708"),
            resources.GetString("colorPicker1.Items709"),
            resources.GetString("colorPicker1.Items710"),
            resources.GetString("colorPicker1.Items711"),
            resources.GetString("colorPicker1.Items712"),
            resources.GetString("colorPicker1.Items713"),
            resources.GetString("colorPicker1.Items714"),
            resources.GetString("colorPicker1.Items715"),
            resources.GetString("colorPicker1.Items716"),
            resources.GetString("colorPicker1.Items717"),
            resources.GetString("colorPicker1.Items718"),
            resources.GetString("colorPicker1.Items719"),
            resources.GetString("colorPicker1.Items720"),
            resources.GetString("colorPicker1.Items721"),
            resources.GetString("colorPicker1.Items722"),
            resources.GetString("colorPicker1.Items723"),
            resources.GetString("colorPicker1.Items724"),
            resources.GetString("colorPicker1.Items725"),
            resources.GetString("colorPicker1.Items726"),
            resources.GetString("colorPicker1.Items727"),
            resources.GetString("colorPicker1.Items728"),
            resources.GetString("colorPicker1.Items729"),
            resources.GetString("colorPicker1.Items730"),
            resources.GetString("colorPicker1.Items731"),
            resources.GetString("colorPicker1.Items732"),
            resources.GetString("colorPicker1.Items733"),
            resources.GetString("colorPicker1.Items734"),
            resources.GetString("colorPicker1.Items735"),
            resources.GetString("colorPicker1.Items736"),
            resources.GetString("colorPicker1.Items737"),
            resources.GetString("colorPicker1.Items738"),
            resources.GetString("colorPicker1.Items739"),
            resources.GetString("colorPicker1.Items740"),
            resources.GetString("colorPicker1.Items741"),
            resources.GetString("colorPicker1.Items742"),
            resources.GetString("colorPicker1.Items743"),
            resources.GetString("colorPicker1.Items744"),
            resources.GetString("colorPicker1.Items745"),
            resources.GetString("colorPicker1.Items746"),
            resources.GetString("colorPicker1.Items747"),
            resources.GetString("colorPicker1.Items748"),
            resources.GetString("colorPicker1.Items749"),
            resources.GetString("colorPicker1.Items750"),
            resources.GetString("colorPicker1.Items751"),
            resources.GetString("colorPicker1.Items752"),
            resources.GetString("colorPicker1.Items753"),
            resources.GetString("colorPicker1.Items754"),
            resources.GetString("colorPicker1.Items755"),
            resources.GetString("colorPicker1.Items756"),
            resources.GetString("colorPicker1.Items757"),
            resources.GetString("colorPicker1.Items758"),
            resources.GetString("colorPicker1.Items759"),
            resources.GetString("colorPicker1.Items760"),
            resources.GetString("colorPicker1.Items761"),
            resources.GetString("colorPicker1.Items762"),
            resources.GetString("colorPicker1.Items763"),
            resources.GetString("colorPicker1.Items764"),
            resources.GetString("colorPicker1.Items765"),
            resources.GetString("colorPicker1.Items766"),
            resources.GetString("colorPicker1.Items767"),
            resources.GetString("colorPicker1.Items768"),
            resources.GetString("colorPicker1.Items769"),
            resources.GetString("colorPicker1.Items770"),
            resources.GetString("colorPicker1.Items771"),
            resources.GetString("colorPicker1.Items772"),
            resources.GetString("colorPicker1.Items773"),
            resources.GetString("colorPicker1.Items774"),
            resources.GetString("colorPicker1.Items775"),
            resources.GetString("colorPicker1.Items776"),
            resources.GetString("colorPicker1.Items777"),
            resources.GetString("colorPicker1.Items778"),
            resources.GetString("colorPicker1.Items779"),
            resources.GetString("colorPicker1.Items780"),
            resources.GetString("colorPicker1.Items781"),
            resources.GetString("colorPicker1.Items782"),
            resources.GetString("colorPicker1.Items783"),
            resources.GetString("colorPicker1.Items784"),
            resources.GetString("colorPicker1.Items785"),
            resources.GetString("colorPicker1.Items786"),
            resources.GetString("colorPicker1.Items787"),
            resources.GetString("colorPicker1.Items788"),
            resources.GetString("colorPicker1.Items789"),
            resources.GetString("colorPicker1.Items790"),
            resources.GetString("colorPicker1.Items791"),
            resources.GetString("colorPicker1.Items792"),
            resources.GetString("colorPicker1.Items793"),
            resources.GetString("colorPicker1.Items794"),
            resources.GetString("colorPicker1.Items795"),
            resources.GetString("colorPicker1.Items796"),
            resources.GetString("colorPicker1.Items797"),
            resources.GetString("colorPicker1.Items798"),
            resources.GetString("colorPicker1.Items799"),
            resources.GetString("colorPicker1.Items800"),
            resources.GetString("colorPicker1.Items801"),
            resources.GetString("colorPicker1.Items802"),
            resources.GetString("colorPicker1.Items803"),
            resources.GetString("colorPicker1.Items804"),
            resources.GetString("colorPicker1.Items805"),
            resources.GetString("colorPicker1.Items806"),
            resources.GetString("colorPicker1.Items807"),
            resources.GetString("colorPicker1.Items808"),
            resources.GetString("colorPicker1.Items809"),
            resources.GetString("colorPicker1.Items810"),
            resources.GetString("colorPicker1.Items811"),
            resources.GetString("colorPicker1.Items812"),
            resources.GetString("colorPicker1.Items813"),
            resources.GetString("colorPicker1.Items814"),
            resources.GetString("colorPicker1.Items815"),
            resources.GetString("colorPicker1.Items816"),
            resources.GetString("colorPicker1.Items817"),
            resources.GetString("colorPicker1.Items818"),
            resources.GetString("colorPicker1.Items819"),
            resources.GetString("colorPicker1.Items820"),
            resources.GetString("colorPicker1.Items821"),
            resources.GetString("colorPicker1.Items822"),
            resources.GetString("colorPicker1.Items823"),
            resources.GetString("colorPicker1.Items824"),
            resources.GetString("colorPicker1.Items825"),
            resources.GetString("colorPicker1.Items826"),
            resources.GetString("colorPicker1.Items827"),
            resources.GetString("colorPicker1.Items828"),
            resources.GetString("colorPicker1.Items829"),
            resources.GetString("colorPicker1.Items830"),
            resources.GetString("colorPicker1.Items831"),
            resources.GetString("colorPicker1.Items832"),
            resources.GetString("colorPicker1.Items833"),
            resources.GetString("colorPicker1.Items834"),
            resources.GetString("colorPicker1.Items835"),
            resources.GetString("colorPicker1.Items836"),
            resources.GetString("colorPicker1.Items837"),
            resources.GetString("colorPicker1.Items838"),
            resources.GetString("colorPicker1.Items839"),
            resources.GetString("colorPicker1.Items840"),
            resources.GetString("colorPicker1.Items841")});
			this.colorPicker1.Name = "colorPicker1";
			this.colorPicker1.SelectedIndexChanged += new System.EventHandler(this.colorPicker1_SelectedIndexChanged);
			// 
			// StaticSeriesCtl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label8);
			this.Controls.Add(this.colorPicker1);
			this.Controls.Add(this.cbLine);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.chkMarker);
			this.Controls.Add(this.btnX);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtX);
			this.Controls.Add(this.btnDown);
			this.Controls.Add(this.btnUp);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.chkRight);
			this.Controls.Add(this.chkLeft);
			this.Controls.Add(this.cbPlotType);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtDataValue);
			this.Controls.Add(this.btnSeriesName);
			this.Controls.Add(this.btnDataValue);
			this.Controls.Add(this.btnLabelValue);
			this.Controls.Add(this.btnDel);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.txtLabelValue);
			this.Controls.Add(this.txtSeriesName);
			this.Controls.Add(this.chkShowLabels);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lbDataSeries);
			this.Controls.Add(this.label1);
			this.Name = "StaticSeriesCtl";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbDataSeries;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkShowLabels;
        private System.Windows.Forms.TextBox txtSeriesName;
        private System.Windows.Forms.TextBox txtLabelValue;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnLabelValue;
        private System.Windows.Forms.Button btnDataValue;
        private System.Windows.Forms.Button btnSeriesName;
        private System.Windows.Forms.TextBox txtDataValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbPlotType;
        private System.Windows.Forms.RadioButton chkLeft;
        private System.Windows.Forms.RadioButton chkRight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnX;
        private System.Windows.Forms.CheckBox chkMarker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbLine;
        private ColorPicker colorPicker1;
        private System.Windows.Forms.Label label8;
    }
}
