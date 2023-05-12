namespace RTI_Tools
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageCommunications = new System.Windows.Forms.TabPage();
            this.groupBox69 = new System.Windows.Forms.GroupBox();
            this.label51 = new System.Windows.Forms.Label();
            this.buttonUDP = new System.Windows.Forms.Button();
            this.textBoxUDPstate = new System.Windows.Forms.TextBox();
            this.textBoxUDPport = new System.Windows.Forms.TextBox();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.radioButtonBinary = new System.Windows.Forms.RadioButton();
            this.radioButtonASCII = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label50 = new System.Windows.Forms.Label();
            this.textBoxEMACA = new System.Windows.Forms.TextBox();
            this.textBoxEMACD = new System.Windows.Forms.TextBox();
            this.textBoxEMACB = new System.Windows.Forms.TextBox();
            this.textBoxEMACC = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox52 = new System.Windows.Forms.GroupBox();
            this.groupBox73 = new System.Windows.Forms.GroupBox();
            this.buttonCommsSetMainPortStopbits = new System.Windows.Forms.Button();
            this.textBoxCommsMainPortManStopBits = new System.Windows.Forms.TextBox();
            this.listBoxMainPortStopBits = new System.Windows.Forms.ListBox();
            this.groupBox72 = new System.Windows.Forms.GroupBox();
            this.buttonCommsSetMainPortBits = new System.Windows.Forms.Button();
            this.textBoxCommsMainPortManBits = new System.Windows.Forms.TextBox();
            this.listBoxMainPortBits = new System.Windows.Forms.ListBox();
            this.groupBox71 = new System.Windows.Forms.GroupBox();
            this.buttonCommsSetMainPortParity = new System.Windows.Forms.Button();
            this.textBoxCommsMainPortManParity = new System.Windows.Forms.TextBox();
            this.listBoxMainPortParity = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonCommsSetMainPortBaud = new System.Windows.Forms.Button();
            this.textBoxCommsMainPortManBaud = new System.Windows.Forms.TextBox();
            this.listBoxMainPortBaud = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonCommsDiconnectMainPort = new System.Windows.Forms.Button();
            this.listBoxAvailableMainPorts = new System.Windows.Forms.ListBox();
            this.btnScanForMainPorts = new System.Windows.Forms.Button();
            this.txtMainPort = new System.Windows.Forms.TextBox();
            this.tabPageFirmware = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.labelFirmwareUpdate = new System.Windows.Forms.Label();
            this.buttonFirmwareCurrentVersion = new System.Windows.Forms.Button();
            this.buttonFirmwareUpdate = new System.Windows.Forms.Button();
            this.textBoxFirmware = new System.Windows.Forms.TextBox();
            this.tabPageTerminal = new System.Windows.Forms.TabPage();
            this.labelTime = new System.Windows.Forms.Label();
            this.textBoxCaptureStatus = new System.Windows.Forms.TextBox();
            this.textBoxForceBaudTime = new System.Windows.Forms.TextBox();
            this.buttonForceBaud = new System.Windows.Forms.Button();
            this.buttonTerminalSetTime = new System.Windows.Forms.Button();
            this.buttonXmodemCancel = new System.Windows.Forms.Button();
            this.groupBoxADCPControl = new System.Windows.Forms.GroupBox();
            this.textBoxDataSize = new System.Windows.Forms.TextBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.textBoxCurrentCommand = new System.Windows.Forms.TextBox();
            this.buttonTerminalDeploy = new System.Windows.Forms.Button();
            this.buttonTerminalSTOP = new System.Windows.Forms.Button();
            this.buttonTerminalSTART = new System.Windows.Forms.Button();
            this.buttonTerminalBREAK = new System.Windows.Forms.Button();
            this.buttonTerminalSLEEP = new System.Windows.Forms.Button();
            this.groupBoxFile = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFileSDcard = new System.Windows.Forms.TextBox();
            this.buttonFileErase = new System.Windows.Forms.Button();
            this.buttonFileUpload = new System.Windows.Forms.Button();
            this.buttonXModemDownload = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.tabPageSystem = new System.Windows.Forms.TabPage();
            this.tabPageBackScatter = new System.Windows.Forms.TabPage();
            this.textBoxBSprofile = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxBSbeam = new System.Windows.Forms.TextBox();
            this.radioButtonBSprofile = new System.Windows.Forms.RadioButton();
            this.radioButtonBSleaders = new System.Windows.Forms.RadioButton();
            this.radioButtonBSdata = new System.Windows.Forms.RadioButton();
            this.radioButtonBSsystem = new System.Windows.Forms.RadioButton();
            this.textBoxBSleaders = new System.Windows.Forms.TextBox();
            this.textBoxBSdata = new System.Windows.Forms.TextBox();
            this.textBoxBSsystem = new System.Windows.Forms.TextBox();
            this.tabPageRiver = new System.Windows.Forms.TabPage();
            this.textBoxRiverBeam = new System.Windows.Forms.TextBox();
            this.textBoxRiverBT2 = new System.Windows.Forms.TextBox();
            this.textBoxRiverBT = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.buttonRiverStop = new System.Windows.Forms.Button();
            this.buttonRiverMinus = new System.Windows.Forms.Button();
            this.buttonRiverPlayback = new System.Windows.Forms.Button();
            this.buttonRiverPause = new System.Windows.Forms.Button();
            this.buttonRiverPlus = new System.Windows.Forms.Button();
            this.buttonRiverGo = new System.Windows.Forms.Button();
            this.textBoxRiverNMEA = new System.Windows.Forms.TextBox();
            this.tabPageProfilePlot = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonEnsembleSubMinus = new System.Windows.Forms.Button();
            this.buttonEnsembleSubPlus = new System.Windows.Forms.Button();
            this.textBoxEnsembleSub = new System.Windows.Forms.TextBox();
            this.checkBoxBTNAVRecalc = new System.Windows.Forms.CheckBox();
            this.checkBoxBTNAVuseZ = new System.Windows.Forms.CheckBox();
            this.textBoxBTNavBinScale = new System.Windows.Forms.TextBox();
            this.buttonStatisticsClear = new System.Windows.Forms.Button();
            this.checkBoxBTNAVshowalways = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.radioButtonStatisticsSD = new System.Windows.Forms.RadioButton();
            this.radioButtonStatisticsAVG = new System.Windows.Forms.RadioButton();
            this.radioButtonStatisticsNone = new System.Windows.Forms.RadioButton();
            this.label37 = new System.Windows.Forms.Label();
            this.checkBoxProfStatPeaks = new System.Windows.Forms.CheckBox();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.buttonBinMinus = new System.Windows.Forms.Button();
            this.textBoxFirstBin = new System.Windows.Forms.TextBox();
            this.buttonBinPlus = new System.Windows.Forms.Button();
            this.buttonCoordinate = new System.Windows.Forms.Button();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.radioButtonProfileDisplayBottomTrack = new System.Windows.Forms.RadioButton();
            this.radioButtonProfileDisplayText = new System.Windows.Forms.RadioButton();
            this.radioButtonProfileDisplayGraph = new System.Windows.Forms.RadioButton();
            this.buttonBTnavBinMinus = new System.Windows.Forms.Button();
            this.buttonBTnavBinPlus = new System.Windows.Forms.Button();
            this.textBoxBTNavBin = new System.Windows.Forms.TextBox();
            this.groupBoxADCPControl2 = new System.Windows.Forms.GroupBox();
            this.buttonSTOP = new System.Windows.Forms.Button();
            this.buttonSTART = new System.Windows.Forms.Button();
            this.buttonBreak = new System.Windows.Forms.Button();
            this.buttonSleep = new System.Windows.Forms.Button();
            this.groupBoxPlayback = new System.Windows.Forms.GroupBox();
            this.buttonPlaybackStop = new System.Windows.Forms.Button();
            this.buttonPlaybackStepBack = new System.Windows.Forms.Button();
            this.buttonPLAYBACK = new System.Windows.Forms.Button();
            this.buttonPlaybackPause = new System.Windows.Forms.Button();
            this.buttonPlaybackStep = new System.Windows.Forms.Button();
            this.buttonPlayBackGo = new System.Windows.Forms.Button();
            this.buttonMinusProfileScale = new System.Windows.Forms.Button();
            this.buttonPlusProfileScale = new System.Windows.Forms.Button();
            this.pictureBoxProfile = new System.Windows.Forms.PictureBox();
            this.textBoxProfile = new System.Windows.Forms.TextBox();
            this.tabPageSeriesPlot = new System.Windows.Forms.TabPage();
            this.groupBox47 = new System.Windows.Forms.GroupBox();
            this.buttonSeriesSubPlus = new System.Windows.Forms.Button();
            this.buttonSeriesSubMinus = new System.Windows.Forms.Button();
            this.textBoxSeriesSub = new System.Windows.Forms.TextBox();
            this.groupBoxPlaybackSeries = new System.Windows.Forms.GroupBox();
            this.buttonPlaybackStopSeries = new System.Windows.Forms.Button();
            this.buttonPlaybackStepBackSeries = new System.Windows.Forms.Button();
            this.buttonPlaybackSeries = new System.Windows.Forms.Button();
            this.buttonPlaybackPauseSeries = new System.Windows.Forms.Button();
            this.buttonPlaybackStepSeries = new System.Windows.Forms.Button();
            this.buttonPlaybackGoSeries = new System.Windows.Forms.Button();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.radioButtonSeriesCoordENU = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesCoordBeam = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesCoordXYZ = new System.Windows.Forms.RadioButton();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.radioButtonSeriesBTmag = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesBTrange = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesBTsnr = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesBTcor = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesBTvel = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesBTamp = new System.Windows.Forms.RadioButton();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.radioButtonSeriesWTcor = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesWTvel = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesWTamp = new System.Windows.Forms.RadioButton();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.radioButtonSeriesWPRT = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesAncillaryProfile = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesAncillaryBT = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesWT = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesProfile = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesBT = new System.Windows.Forms.RadioButton();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.buttonSeriesBinPlus = new System.Windows.Forms.Button();
            this.buttonSeriesBinMinus = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSeriesBin = new System.Windows.Forms.TextBox();
            this.radioButtonSeriesProfileCor = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesProfileVel = new System.Windows.Forms.RadioButton();
            this.radioButtonSeriesProfileAmp = new System.Windows.Forms.RadioButton();
            this.buttonClearSeries = new System.Windows.Forms.Button();
            this.buttonSeriesPlus = new System.Windows.Forms.Button();
            this.buttonSeriesMinus = new System.Windows.Forms.Button();
            this.pictureBoxSeries = new System.Windows.Forms.PictureBox();
            this.tabPageNMEA = new System.Windows.Forms.TabPage();
            this.checkBoxNMEA_ASCII_Input = new System.Windows.Forms.CheckBox();
            this.groupBoxPlaybackNMEA = new System.Windows.Forms.GroupBox();
            this.buttonPlaybackStopNMEA = new System.Windows.Forms.Button();
            this.buttonPlaybackStepBackNMEA = new System.Windows.Forms.Button();
            this.buttonPlaybackNMEA = new System.Windows.Forms.Button();
            this.buttonPlaybackPauseNMEA = new System.Windows.Forms.Button();
            this.buttonPlaybackStepNMEA = new System.Windows.Forms.Button();
            this.buttonPlaybackGoNMEA = new System.Windows.Forms.Button();
            this.textBoxDecoded = new System.Windows.Forms.TextBox();
            this.textBoxNavigation = new System.Windows.Forms.TextBox();
            this.textBoxCapturedNMEA = new System.Windows.Forms.TextBox();
            this.tabDownload = new System.Windows.Forms.TabPage();
            this.textBoxDownloadBPS = new System.Windows.Forms.TextBox();
            this.label63 = new System.Windows.Forms.Label();
            this.textBoxDownloadPercent = new System.Windows.Forms.TextBox();
            this.label62 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.textBoxDownloadSeconds = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.textBoxDownloadBytes = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.textBoxDownloadTries = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.textBoxDownloadRetries = new System.Windows.Forms.TextBox();
            this.groupBox34 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFirstFile = new System.Windows.Forms.TextBox();
            this.buttonRecoverDownloadCancel = new System.Windows.Forms.Button();
            this.buttonRecoverDownload = new System.Windows.Forms.Button();
            this.buttonRecoverDir = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.radioButtonTxtDownload = new System.Windows.Forms.RadioButton();
            this.radioButtonRawDownload = new System.Windows.Forms.RadioButton();
            this.radioButtonAllDownLoad = new System.Windows.Forms.RadioButton();
            this.radioButtonBurstDownLoad = new System.Windows.Forms.RadioButton();
            this.radioButtonProfileDownLoad = new System.Windows.Forms.RadioButton();
            this.textBoxWavesRecoverDownload = new System.Windows.Forms.TextBox();
            this.textBoxWavesRecover = new System.Windows.Forms.TextBox();
            this.tabPageFileConvert = new System.Windows.Forms.TabPage();
            this.groupBox44 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxMergeTotalBytes = new System.Windows.Forms.TextBox();
            this.textBoxMergeFilesInc = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.buttonMergeFiles = new System.Windows.Forms.Button();
            this.groupBox74 = new System.Windows.Forms.GroupBox();
            this.button1PTICPD13toCSV = new System.Windows.Forms.Button();
            this.groupBox70 = new System.Windows.Forms.GroupBox();
            this.buttonPD3toCSV = new System.Windows.Forms.Button();
            this.groupBox51 = new System.Windows.Forms.GroupBox();
            this.buttonExtractVTGbottomnav = new System.Windows.Forms.Button();
            this.groupBox50 = new System.Windows.Forms.GroupBox();
            this.buttonExtractADCP1raw = new System.Windows.Forms.Button();
            this.buttonExtractADCP0raw = new System.Windows.Forms.Button();
            this.groupBox41 = new System.Windows.Forms.GroupBox();
            this.label44 = new System.Windows.Forms.Label();
            this.textBoxExtractMatlabSubSys = new System.Windows.Forms.TextBox();
            this.buttonExtractMatlab = new System.Windows.Forms.Button();
            this.groupBox40 = new System.Windows.Forms.GroupBox();
            this.groupBox43 = new System.Windows.Forms.GroupBox();
            this.label45 = new System.Windows.Forms.Label();
            this.textBoxExtractSeriesSubSys = new System.Windows.Forms.TextBox();
            this.buttonExtractSeries = new System.Windows.Forms.Button();
            this.groupBox42 = new System.Windows.Forms.GroupBox();
            this.buttonExtractProfile = new System.Windows.Forms.Button();
            this.textBoxExtractProfileEnsembleNumber = new System.Windows.Forms.TextBox();
            this.textBoxExtract = new System.Windows.Forms.TextBox();
            this.txtUserCommand = new System.Windows.Forms.TextBox();
            this.btnSendCom = new System.Windows.Forms.Button();
            this.mnuMain = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuHelp = new System.Windows.Forms.MenuItem();
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.buttonSendCom1 = new System.Windows.Forms.Button();
            this.toolTipWavesWaterDepth = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipWavesPressureHeight = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxShowVTGspeed = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.textBoxVTGspeedLimit = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageCommunications.SuspendLayout();
            this.groupBox69.SuspendLayout();
            this.groupBox26.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox52.SuspendLayout();
            this.groupBox73.SuspendLayout();
            this.groupBox72.SuspendLayout();
            this.groupBox71.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPageFirmware.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPageTerminal.SuspendLayout();
            this.groupBoxADCPControl.SuspendLayout();
            this.groupBoxFile.SuspendLayout();
            this.tabPageBackScatter.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabPageRiver.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPageProfilePlot.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox23.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.groupBoxADCPControl2.SuspendLayout();
            this.groupBoxPlayback.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfile)).BeginInit();
            this.tabPageSeriesPlot.SuspendLayout();
            this.groupBox47.SuspendLayout();
            this.groupBoxPlaybackSeries.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSeries)).BeginInit();
            this.tabPageNMEA.SuspendLayout();
            this.groupBoxPlaybackNMEA.SuspendLayout();
            this.tabDownload.SuspendLayout();
            this.groupBox34.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPageFileConvert.SuspendLayout();
            this.groupBox44.SuspendLayout();
            this.groupBox74.SuspendLayout();
            this.groupBox70.SuspendLayout();
            this.groupBox51.SuspendLayout();
            this.groupBox50.SuspendLayout();
            this.groupBox41.SuspendLayout();
            this.groupBox40.SuspendLayout();
            this.groupBox43.SuspendLayout();
            this.groupBox42.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPageCommunications);
            this.tabControl1.Controls.Add(this.tabPageFirmware);
            this.tabControl1.Controls.Add(this.tabPageTerminal);
            this.tabControl1.Controls.Add(this.tabPageSystem);
            this.tabControl1.Controls.Add(this.tabPageBackScatter);
            this.tabControl1.Controls.Add(this.tabPageRiver);
            this.tabControl1.Controls.Add(this.tabPageProfilePlot);
            this.tabControl1.Controls.Add(this.tabPageSeriesPlot);
            this.tabControl1.Controls.Add(this.tabPageNMEA);
            this.tabControl1.Controls.Add(this.tabDownload);
            this.tabControl1.Controls.Add(this.tabPageFileConvert);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPageCommunications
            // 
            this.tabPageCommunications.BackColor = System.Drawing.Color.Transparent;
            this.tabPageCommunications.Controls.Add(this.groupBox69);
            this.tabPageCommunications.Controls.Add(this.groupBox26);
            this.tabPageCommunications.Controls.Add(this.groupBox3);
            this.tabPageCommunications.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPageCommunications, "tabPageCommunications");
            this.tabPageCommunications.Name = "tabPageCommunications";
            this.tabPageCommunications.UseVisualStyleBackColor = true;
            // 
            // groupBox69
            // 
            this.groupBox69.Controls.Add(this.label51);
            this.groupBox69.Controls.Add(this.buttonUDP);
            this.groupBox69.Controls.Add(this.textBoxUDPstate);
            this.groupBox69.Controls.Add(this.textBoxUDPport);
            resources.ApplyResources(this.groupBox69, "groupBox69");
            this.groupBox69.Name = "groupBox69";
            this.groupBox69.TabStop = false;
            // 
            // label51
            // 
            resources.ApplyResources(this.label51, "label51");
            this.label51.Name = "label51";
            // 
            // buttonUDP
            // 
            resources.ApplyResources(this.buttonUDP, "buttonUDP");
            this.buttonUDP.Name = "buttonUDP";
            this.buttonUDP.UseVisualStyleBackColor = true;
            // 
            // textBoxUDPstate
            // 
            resources.ApplyResources(this.textBoxUDPstate, "textBoxUDPstate");
            this.textBoxUDPstate.Name = "textBoxUDPstate";
            this.textBoxUDPstate.TabStop = false;
            // 
            // textBoxUDPport
            // 
            resources.ApplyResources(this.textBoxUDPport, "textBoxUDPport");
            this.textBoxUDPport.Name = "textBoxUDPport";
            this.textBoxUDPport.TabStop = false;
            // 
            // groupBox26
            // 
            this.groupBox26.Controls.Add(this.radioButtonBinary);
            this.groupBox26.Controls.Add(this.radioButtonASCII);
            resources.ApplyResources(this.groupBox26, "groupBox26");
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.TabStop = false;
            // 
            // radioButtonBinary
            // 
            resources.ApplyResources(this.radioButtonBinary, "radioButtonBinary");
            this.radioButtonBinary.Checked = true;
            this.radioButtonBinary.Name = "radioButtonBinary";
            this.radioButtonBinary.TabStop = true;
            this.radioButtonBinary.UseVisualStyleBackColor = true;
            // 
            // radioButtonASCII
            // 
            resources.ApplyResources(this.radioButtonASCII, "radioButtonASCII");
            this.radioButtonASCII.Name = "radioButtonASCII";
            this.radioButtonASCII.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label50);
            this.groupBox3.Controls.Add(this.textBoxEMACA);
            this.groupBox3.Controls.Add(this.textBoxEMACD);
            this.groupBox3.Controls.Add(this.textBoxEMACB);
            this.groupBox3.Controls.Add(this.textBoxEMACC);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label50
            // 
            resources.ApplyResources(this.label50, "label50");
            this.label50.Name = "label50";
            // 
            // textBoxEMACA
            // 
            resources.ApplyResources(this.textBoxEMACA, "textBoxEMACA");
            this.textBoxEMACA.Name = "textBoxEMACA";
            this.textBoxEMACA.TabStop = false;
            // 
            // textBoxEMACD
            // 
            resources.ApplyResources(this.textBoxEMACD, "textBoxEMACD");
            this.textBoxEMACD.Name = "textBoxEMACD";
            this.textBoxEMACD.TabStop = false;
            // 
            // textBoxEMACB
            // 
            resources.ApplyResources(this.textBoxEMACB, "textBoxEMACB");
            this.textBoxEMACB.Name = "textBoxEMACB";
            this.textBoxEMACB.TabStop = false;
            // 
            // textBoxEMACC
            // 
            resources.ApplyResources(this.textBoxEMACC, "textBoxEMACC");
            this.textBoxEMACC.Name = "textBoxEMACC";
            this.textBoxEMACC.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox52);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox52
            // 
            this.groupBox52.Controls.Add(this.groupBox73);
            this.groupBox52.Controls.Add(this.groupBox72);
            this.groupBox52.Controls.Add(this.groupBox71);
            this.groupBox52.Controls.Add(this.groupBox2);
            this.groupBox52.Controls.Add(this.groupBox4);
            resources.ApplyResources(this.groupBox52, "groupBox52");
            this.groupBox52.Name = "groupBox52";
            this.groupBox52.TabStop = false;
            // 
            // groupBox73
            // 
            this.groupBox73.Controls.Add(this.buttonCommsSetMainPortStopbits);
            this.groupBox73.Controls.Add(this.textBoxCommsMainPortManStopBits);
            this.groupBox73.Controls.Add(this.listBoxMainPortStopBits);
            resources.ApplyResources(this.groupBox73, "groupBox73");
            this.groupBox73.Name = "groupBox73";
            this.groupBox73.TabStop = false;
            // 
            // buttonCommsSetMainPortStopbits
            // 
            resources.ApplyResources(this.buttonCommsSetMainPortStopbits, "buttonCommsSetMainPortStopbits");
            this.buttonCommsSetMainPortStopbits.Name = "buttonCommsSetMainPortStopbits";
            this.buttonCommsSetMainPortStopbits.UseVisualStyleBackColor = true;
            this.buttonCommsSetMainPortStopbits.Click += new System.EventHandler(this.buttonCommsSetMainPortStopbits_Click);
            // 
            // textBoxCommsMainPortManStopBits
            // 
            this.textBoxCommsMainPortManStopBits.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxCommsMainPortManStopBits, "textBoxCommsMainPortManStopBits");
            this.textBoxCommsMainPortManStopBits.CausesValidation = false;
            this.textBoxCommsMainPortManStopBits.Name = "textBoxCommsMainPortManStopBits";
            // 
            // listBoxMainPortStopBits
            // 
            resources.ApplyResources(this.listBoxMainPortStopBits, "listBoxMainPortStopBits");
            this.listBoxMainPortStopBits.FormattingEnabled = true;
            this.listBoxMainPortStopBits.Items.AddRange(new object[] {
            resources.GetString("listBoxMainPortStopBits.Items"),
            resources.GetString("listBoxMainPortStopBits.Items1")});
            this.listBoxMainPortStopBits.Name = "listBoxMainPortStopBits";
            this.listBoxMainPortStopBits.SelectedIndexChanged += new System.EventHandler(this.listBoxMainPortStopBits_SelectedIndexChanged);
            // 
            // groupBox72
            // 
            this.groupBox72.Controls.Add(this.buttonCommsSetMainPortBits);
            this.groupBox72.Controls.Add(this.textBoxCommsMainPortManBits);
            this.groupBox72.Controls.Add(this.listBoxMainPortBits);
            resources.ApplyResources(this.groupBox72, "groupBox72");
            this.groupBox72.Name = "groupBox72";
            this.groupBox72.TabStop = false;
            // 
            // buttonCommsSetMainPortBits
            // 
            resources.ApplyResources(this.buttonCommsSetMainPortBits, "buttonCommsSetMainPortBits");
            this.buttonCommsSetMainPortBits.Name = "buttonCommsSetMainPortBits";
            this.buttonCommsSetMainPortBits.UseVisualStyleBackColor = true;
            this.buttonCommsSetMainPortBits.Click += new System.EventHandler(this.buttonCommsSetMainPortBits_Click);
            // 
            // textBoxCommsMainPortManBits
            // 
            this.textBoxCommsMainPortManBits.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxCommsMainPortManBits, "textBoxCommsMainPortManBits");
            this.textBoxCommsMainPortManBits.CausesValidation = false;
            this.textBoxCommsMainPortManBits.Name = "textBoxCommsMainPortManBits";
            // 
            // listBoxMainPortBits
            // 
            resources.ApplyResources(this.listBoxMainPortBits, "listBoxMainPortBits");
            this.listBoxMainPortBits.FormattingEnabled = true;
            this.listBoxMainPortBits.Items.AddRange(new object[] {
            resources.GetString("listBoxMainPortBits.Items"),
            resources.GetString("listBoxMainPortBits.Items1")});
            this.listBoxMainPortBits.Name = "listBoxMainPortBits";
            this.listBoxMainPortBits.SelectedIndexChanged += new System.EventHandler(this.listBoxMainPortBits_SelectedIndexChanged);
            // 
            // groupBox71
            // 
            this.groupBox71.Controls.Add(this.buttonCommsSetMainPortParity);
            this.groupBox71.Controls.Add(this.textBoxCommsMainPortManParity);
            this.groupBox71.Controls.Add(this.listBoxMainPortParity);
            resources.ApplyResources(this.groupBox71, "groupBox71");
            this.groupBox71.Name = "groupBox71";
            this.groupBox71.TabStop = false;
            // 
            // buttonCommsSetMainPortParity
            // 
            resources.ApplyResources(this.buttonCommsSetMainPortParity, "buttonCommsSetMainPortParity");
            this.buttonCommsSetMainPortParity.Name = "buttonCommsSetMainPortParity";
            this.buttonCommsSetMainPortParity.UseVisualStyleBackColor = true;
            this.buttonCommsSetMainPortParity.Click += new System.EventHandler(this.buttonCommsSetMainPortParity_Click);
            // 
            // textBoxCommsMainPortManParity
            // 
            this.textBoxCommsMainPortManParity.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxCommsMainPortManParity, "textBoxCommsMainPortManParity");
            this.textBoxCommsMainPortManParity.CausesValidation = false;
            this.textBoxCommsMainPortManParity.Name = "textBoxCommsMainPortManParity";
            // 
            // listBoxMainPortParity
            // 
            resources.ApplyResources(this.listBoxMainPortParity, "listBoxMainPortParity");
            this.listBoxMainPortParity.FormattingEnabled = true;
            this.listBoxMainPortParity.Items.AddRange(new object[] {
            resources.GetString("listBoxMainPortParity.Items"),
            resources.GetString("listBoxMainPortParity.Items1"),
            resources.GetString("listBoxMainPortParity.Items2")});
            this.listBoxMainPortParity.Name = "listBoxMainPortParity";
            this.listBoxMainPortParity.SelectedIndexChanged += new System.EventHandler(this.listBoxMainPortParity_SelectedIndexChanged_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonCommsSetMainPortBaud);
            this.groupBox2.Controls.Add(this.textBoxCommsMainPortManBaud);
            this.groupBox2.Controls.Add(this.listBoxMainPortBaud);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // buttonCommsSetMainPortBaud
            // 
            resources.ApplyResources(this.buttonCommsSetMainPortBaud, "buttonCommsSetMainPortBaud");
            this.buttonCommsSetMainPortBaud.Name = "buttonCommsSetMainPortBaud";
            this.buttonCommsSetMainPortBaud.UseVisualStyleBackColor = true;
            this.buttonCommsSetMainPortBaud.Click += new System.EventHandler(this.buttonCommsSetBaud_Click);
            // 
            // textBoxCommsMainPortManBaud
            // 
            this.textBoxCommsMainPortManBaud.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxCommsMainPortManBaud, "textBoxCommsMainPortManBaud");
            this.textBoxCommsMainPortManBaud.CausesValidation = false;
            this.textBoxCommsMainPortManBaud.Name = "textBoxCommsMainPortManBaud";
            // 
            // listBoxMainPortBaud
            // 
            resources.ApplyResources(this.listBoxMainPortBaud, "listBoxMainPortBaud");
            this.listBoxMainPortBaud.FormattingEnabled = true;
            this.listBoxMainPortBaud.Items.AddRange(new object[] {
            resources.GetString("listBoxMainPortBaud.Items"),
            resources.GetString("listBoxMainPortBaud.Items1"),
            resources.GetString("listBoxMainPortBaud.Items2"),
            resources.GetString("listBoxMainPortBaud.Items3"),
            resources.GetString("listBoxMainPortBaud.Items4"),
            resources.GetString("listBoxMainPortBaud.Items5"),
            resources.GetString("listBoxMainPortBaud.Items6"),
            resources.GetString("listBoxMainPortBaud.Items7"),
            resources.GetString("listBoxMainPortBaud.Items8"),
            resources.GetString("listBoxMainPortBaud.Items9"),
            resources.GetString("listBoxMainPortBaud.Items10"),
            resources.GetString("listBoxMainPortBaud.Items11")});
            this.listBoxMainPortBaud.Name = "listBoxMainPortBaud";
            this.listBoxMainPortBaud.SelectedIndexChanged += new System.EventHandler(this.listBoxBaud_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonCommsDiconnectMainPort);
            this.groupBox4.Controls.Add(this.listBoxAvailableMainPorts);
            this.groupBox4.Controls.Add(this.btnScanForMainPorts);
            this.groupBox4.Controls.Add(this.txtMainPort);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // buttonCommsDiconnectMainPort
            // 
            resources.ApplyResources(this.buttonCommsDiconnectMainPort, "buttonCommsDiconnectMainPort");
            this.buttonCommsDiconnectMainPort.Name = "buttonCommsDiconnectMainPort";
            this.buttonCommsDiconnectMainPort.Click += new System.EventHandler(this.buttonCommsDiconnect_Click);
            // 
            // listBoxAvailableMainPorts
            // 
            resources.ApplyResources(this.listBoxAvailableMainPorts, "listBoxAvailableMainPorts");
            this.listBoxAvailableMainPorts.FormattingEnabled = true;
            this.listBoxAvailableMainPorts.Items.AddRange(new object[] {
            resources.GetString("listBoxAvailableMainPorts.Items")});
            this.listBoxAvailableMainPorts.Name = "listBoxAvailableMainPorts";
            this.listBoxAvailableMainPorts.SelectedIndexChanged += new System.EventHandler(this.listBoxAvailablePorts_SelectedIndexChanged);
            // 
            // btnScanForMainPorts
            // 
            resources.ApplyResources(this.btnScanForMainPorts, "btnScanForMainPorts");
            this.btnScanForMainPorts.Name = "btnScanForMainPorts";
            this.btnScanForMainPorts.Click += new System.EventHandler(this.btnCheckForPorts_Click);
            // 
            // txtMainPort
            // 
            resources.ApplyResources(this.txtMainPort, "txtMainPort");
            this.txtMainPort.Name = "txtMainPort";
            this.txtMainPort.TabStop = false;
            // 
            // tabPageFirmware
            // 
            this.tabPageFirmware.Controls.Add(this.groupBox6);
            this.tabPageFirmware.Controls.Add(this.textBoxFirmware);
            resources.ApplyResources(this.tabPageFirmware, "tabPageFirmware");
            this.tabPageFirmware.Name = "tabPageFirmware";
            this.tabPageFirmware.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.labelFirmwareUpdate);
            this.groupBox6.Controls.Add(this.buttonFirmwareCurrentVersion);
            this.groupBox6.Controls.Add(this.buttonFirmwareUpdate);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // labelFirmwareUpdate
            // 
            resources.ApplyResources(this.labelFirmwareUpdate, "labelFirmwareUpdate");
            this.labelFirmwareUpdate.Name = "labelFirmwareUpdate";
            // 
            // buttonFirmwareCurrentVersion
            // 
            resources.ApplyResources(this.buttonFirmwareCurrentVersion, "buttonFirmwareCurrentVersion");
            this.buttonFirmwareCurrentVersion.Name = "buttonFirmwareCurrentVersion";
            this.buttonFirmwareCurrentVersion.UseVisualStyleBackColor = true;
            this.buttonFirmwareCurrentVersion.Click += new System.EventHandler(this.buttonFirmwareCurrentVersion_Click);
            // 
            // buttonFirmwareUpdate
            // 
            resources.ApplyResources(this.buttonFirmwareUpdate, "buttonFirmwareUpdate");
            this.buttonFirmwareUpdate.Name = "buttonFirmwareUpdate";
            this.buttonFirmwareUpdate.UseVisualStyleBackColor = true;
            this.buttonFirmwareUpdate.Click += new System.EventHandler(this.buttonFirmwareUpdate_Click);
            // 
            // textBoxFirmware
            // 
            resources.ApplyResources(this.textBoxFirmware, "textBoxFirmware");
            this.textBoxFirmware.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxFirmware.Name = "textBoxFirmware";
            this.textBoxFirmware.ReadOnly = true;
            this.textBoxFirmware.TabStop = false;
            // 
            // tabPageTerminal
            // 
            this.tabPageTerminal.BackColor = System.Drawing.Color.Transparent;
            this.tabPageTerminal.Controls.Add(this.groupBox9);
            this.tabPageTerminal.Controls.Add(this.labelTime);
            this.tabPageTerminal.Controls.Add(this.textBoxCaptureStatus);
            this.tabPageTerminal.Controls.Add(this.textBoxForceBaudTime);
            this.tabPageTerminal.Controls.Add(this.buttonForceBaud);
            this.tabPageTerminal.Controls.Add(this.buttonTerminalSetTime);
            this.tabPageTerminal.Controls.Add(this.buttonXmodemCancel);
            this.tabPageTerminal.Controls.Add(this.groupBoxADCPControl);
            this.tabPageTerminal.Controls.Add(this.groupBoxFile);
            this.tabPageTerminal.Controls.Add(this.btnClear);
            this.tabPageTerminal.Controls.Add(this.txtSerial);
            resources.ApplyResources(this.tabPageTerminal, "tabPageTerminal");
            this.tabPageTerminal.Name = "tabPageTerminal";
            this.tabPageTerminal.UseVisualStyleBackColor = true;
            // 
            // labelTime
            // 
            resources.ApplyResources(this.labelTime, "labelTime");
            this.labelTime.Name = "labelTime";
            // 
            // textBoxCaptureStatus
            // 
            resources.ApplyResources(this.textBoxCaptureStatus, "textBoxCaptureStatus");
            this.textBoxCaptureStatus.Name = "textBoxCaptureStatus";
            this.textBoxCaptureStatus.TabStop = false;
            // 
            // textBoxForceBaudTime
            // 
            resources.ApplyResources(this.textBoxForceBaudTime, "textBoxForceBaudTime");
            this.textBoxForceBaudTime.Name = "textBoxForceBaudTime";
            this.textBoxForceBaudTime.TabStop = false;
            // 
            // buttonForceBaud
            // 
            resources.ApplyResources(this.buttonForceBaud, "buttonForceBaud");
            this.buttonForceBaud.Name = "buttonForceBaud";
            this.buttonForceBaud.UseVisualStyleBackColor = true;
            this.buttonForceBaud.Click += new System.EventHandler(this.buttonForceBaud_Click);
            // 
            // buttonTerminalSetTime
            // 
            resources.ApplyResources(this.buttonTerminalSetTime, "buttonTerminalSetTime");
            this.buttonTerminalSetTime.Name = "buttonTerminalSetTime";
            this.buttonTerminalSetTime.UseVisualStyleBackColor = true;
            this.buttonTerminalSetTime.Click += new System.EventHandler(this.buttonTerminalSetTime_Click);
            // 
            // buttonXmodemCancel
            // 
            resources.ApplyResources(this.buttonXmodemCancel, "buttonXmodemCancel");
            this.buttonXmodemCancel.Name = "buttonXmodemCancel";
            this.buttonXmodemCancel.UseVisualStyleBackColor = true;
            this.buttonXmodemCancel.Click += new System.EventHandler(this.buttonXmodemCancel_Click);
            // 
            // groupBoxADCPControl
            // 
            this.groupBoxADCPControl.Controls.Add(this.textBoxDataSize);
            this.groupBoxADCPControl.Controls.Add(this.btnCapture);
            this.groupBoxADCPControl.Controls.Add(this.textBoxCurrentCommand);
            this.groupBoxADCPControl.Controls.Add(this.buttonTerminalDeploy);
            this.groupBoxADCPControl.Controls.Add(this.buttonTerminalSTOP);
            this.groupBoxADCPControl.Controls.Add(this.buttonTerminalSTART);
            this.groupBoxADCPControl.Controls.Add(this.buttonTerminalBREAK);
            this.groupBoxADCPControl.Controls.Add(this.buttonTerminalSLEEP);
            resources.ApplyResources(this.groupBoxADCPControl, "groupBoxADCPControl");
            this.groupBoxADCPControl.Name = "groupBoxADCPControl";
            this.groupBoxADCPControl.TabStop = false;
            // 
            // textBoxDataSize
            // 
            resources.ApplyResources(this.textBoxDataSize, "textBoxDataSize");
            this.textBoxDataSize.Name = "textBoxDataSize";
            this.textBoxDataSize.TabStop = false;
            // 
            // btnCapture
            // 
            resources.ApplyResources(this.btnCapture, "btnCapture");
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // textBoxCurrentCommand
            // 
            resources.ApplyResources(this.textBoxCurrentCommand, "textBoxCurrentCommand");
            this.textBoxCurrentCommand.Name = "textBoxCurrentCommand";
            this.textBoxCurrentCommand.TabStop = false;
            // 
            // buttonTerminalDeploy
            // 
            resources.ApplyResources(this.buttonTerminalDeploy, "buttonTerminalDeploy");
            this.buttonTerminalDeploy.Name = "buttonTerminalDeploy";
            this.buttonTerminalDeploy.UseVisualStyleBackColor = true;
            this.buttonTerminalDeploy.Click += new System.EventHandler(this.buttonTerminalDeploy_Click);
            // 
            // buttonTerminalSTOP
            // 
            resources.ApplyResources(this.buttonTerminalSTOP, "buttonTerminalSTOP");
            this.buttonTerminalSTOP.Name = "buttonTerminalSTOP";
            this.buttonTerminalSTOP.UseVisualStyleBackColor = true;
            this.buttonTerminalSTOP.Click += new System.EventHandler(this.buttonTerminalSTOP_Click);
            // 
            // buttonTerminalSTART
            // 
            resources.ApplyResources(this.buttonTerminalSTART, "buttonTerminalSTART");
            this.buttonTerminalSTART.Name = "buttonTerminalSTART";
            this.buttonTerminalSTART.UseVisualStyleBackColor = true;
            this.buttonTerminalSTART.Click += new System.EventHandler(this.buttonTerminalSTART_Click);
            // 
            // buttonTerminalBREAK
            // 
            resources.ApplyResources(this.buttonTerminalBREAK, "buttonTerminalBREAK");
            this.buttonTerminalBREAK.Name = "buttonTerminalBREAK";
            this.buttonTerminalBREAK.UseVisualStyleBackColor = true;
            this.buttonTerminalBREAK.Click += new System.EventHandler(this.buttonTerminalBREAK_Click);
            // 
            // buttonTerminalSLEEP
            // 
            resources.ApplyResources(this.buttonTerminalSLEEP, "buttonTerminalSLEEP");
            this.buttonTerminalSLEEP.Name = "buttonTerminalSLEEP";
            this.buttonTerminalSLEEP.UseVisualStyleBackColor = true;
            this.buttonTerminalSLEEP.Click += new System.EventHandler(this.buttonTerminalSLEEP_Click);
            // 
            // groupBoxFile
            // 
            this.groupBoxFile.Controls.Add(this.label3);
            this.groupBoxFile.Controls.Add(this.textBoxFileSDcard);
            this.groupBoxFile.Controls.Add(this.buttonFileErase);
            this.groupBoxFile.Controls.Add(this.buttonFileUpload);
            this.groupBoxFile.Controls.Add(this.buttonXModemDownload);
            resources.ApplyResources(this.groupBoxFile, "groupBoxFile");
            this.groupBoxFile.Name = "groupBoxFile";
            this.groupBoxFile.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textBoxFileSDcard
            // 
            resources.ApplyResources(this.textBoxFileSDcard, "textBoxFileSDcard");
            this.textBoxFileSDcard.Name = "textBoxFileSDcard";
            this.textBoxFileSDcard.TabStop = false;
            // 
            // buttonFileErase
            // 
            resources.ApplyResources(this.buttonFileErase, "buttonFileErase");
            this.buttonFileErase.Name = "buttonFileErase";
            this.buttonFileErase.UseVisualStyleBackColor = true;
            this.buttonFileErase.Click += new System.EventHandler(this.buttonFileErase_Click);
            // 
            // buttonFileUpload
            // 
            resources.ApplyResources(this.buttonFileUpload, "buttonFileUpload");
            this.buttonFileUpload.Name = "buttonFileUpload";
            this.buttonFileUpload.UseVisualStyleBackColor = true;
            this.buttonFileUpload.Click += new System.EventHandler(this.buttonFileUpload_Click);
            // 
            // buttonXModemDownload
            // 
            resources.ApplyResources(this.buttonXModemDownload, "buttonXModemDownload");
            this.buttonXModemDownload.Name = "buttonXModemDownload";
            this.buttonXModemDownload.UseVisualStyleBackColor = true;
            this.buttonXModemDownload.Click += new System.EventHandler(this.buttonXModemDownload_Click);
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtSerial
            // 
            resources.ApplyResources(this.txtSerial, "txtSerial");
            this.txtSerial.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.ReadOnly = true;
            this.txtSerial.TabStop = false;
            // 
            // tabPageSystem
            // 
            resources.ApplyResources(this.tabPageSystem, "tabPageSystem");
            this.tabPageSystem.Name = "tabPageSystem";
            this.tabPageSystem.UseVisualStyleBackColor = true;
            // 
            // tabPageBackScatter
            // 
            this.tabPageBackScatter.Controls.Add(this.textBoxBSprofile);
            this.tabPageBackScatter.Controls.Add(this.groupBox8);
            this.tabPageBackScatter.Controls.Add(this.textBoxBSleaders);
            this.tabPageBackScatter.Controls.Add(this.textBoxBSdata);
            this.tabPageBackScatter.Controls.Add(this.textBoxBSsystem);
            resources.ApplyResources(this.tabPageBackScatter, "tabPageBackScatter");
            this.tabPageBackScatter.Name = "tabPageBackScatter";
            this.tabPageBackScatter.UseVisualStyleBackColor = true;
            // 
            // textBoxBSprofile
            // 
            resources.ApplyResources(this.textBoxBSprofile, "textBoxBSprofile");
            this.textBoxBSprofile.Name = "textBoxBSprofile";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label5);
            this.groupBox8.Controls.Add(this.textBoxBSbeam);
            this.groupBox8.Controls.Add(this.radioButtonBSprofile);
            this.groupBox8.Controls.Add(this.radioButtonBSleaders);
            this.groupBox8.Controls.Add(this.radioButtonBSdata);
            this.groupBox8.Controls.Add(this.radioButtonBSsystem);
            resources.ApplyResources(this.groupBox8, "groupBox8");
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // textBoxBSbeam
            // 
            resources.ApplyResources(this.textBoxBSbeam, "textBoxBSbeam");
            this.textBoxBSbeam.Name = "textBoxBSbeam";
            this.textBoxBSbeam.TabStop = false;
            this.textBoxBSbeam.TextChanged += new System.EventHandler(this.textBoxBSbeam_TextChanged);
            // 
            // radioButtonBSprofile
            // 
            resources.ApplyResources(this.radioButtonBSprofile, "radioButtonBSprofile");
            this.radioButtonBSprofile.Name = "radioButtonBSprofile";
            this.radioButtonBSprofile.UseVisualStyleBackColor = true;
            this.radioButtonBSprofile.CheckedChanged += new System.EventHandler(this.radioButtonBSprofile_CheckedChanged);
            // 
            // radioButtonBSleaders
            // 
            resources.ApplyResources(this.radioButtonBSleaders, "radioButtonBSleaders");
            this.radioButtonBSleaders.Name = "radioButtonBSleaders";
            this.radioButtonBSleaders.UseVisualStyleBackColor = true;
            this.radioButtonBSleaders.CheckedChanged += new System.EventHandler(this.radioButtonBSleaders_CheckedChanged);
            // 
            // radioButtonBSdata
            // 
            resources.ApplyResources(this.radioButtonBSdata, "radioButtonBSdata");
            this.radioButtonBSdata.Name = "radioButtonBSdata";
            this.radioButtonBSdata.UseVisualStyleBackColor = true;
            this.radioButtonBSdata.CheckedChanged += new System.EventHandler(this.radioButtonBSdata_CheckedChanged);
            // 
            // radioButtonBSsystem
            // 
            resources.ApplyResources(this.radioButtonBSsystem, "radioButtonBSsystem");
            this.radioButtonBSsystem.Checked = true;
            this.radioButtonBSsystem.Name = "radioButtonBSsystem";
            this.radioButtonBSsystem.TabStop = true;
            this.radioButtonBSsystem.UseVisualStyleBackColor = true;
            this.radioButtonBSsystem.CheckedChanged += new System.EventHandler(this.radioButtonBSsystem_CheckedChanged);
            // 
            // textBoxBSleaders
            // 
            resources.ApplyResources(this.textBoxBSleaders, "textBoxBSleaders");
            this.textBoxBSleaders.Name = "textBoxBSleaders";
            // 
            // textBoxBSdata
            // 
            resources.ApplyResources(this.textBoxBSdata, "textBoxBSdata");
            this.textBoxBSdata.Name = "textBoxBSdata";
            // 
            // textBoxBSsystem
            // 
            resources.ApplyResources(this.textBoxBSsystem, "textBoxBSsystem");
            this.textBoxBSsystem.Name = "textBoxBSsystem";
            // 
            // tabPageRiver
            // 
            this.tabPageRiver.Controls.Add(this.textBoxRiverBeam);
            this.tabPageRiver.Controls.Add(this.textBoxRiverBT2);
            this.tabPageRiver.Controls.Add(this.textBoxRiverBT);
            this.tabPageRiver.Controls.Add(this.groupBox5);
            this.tabPageRiver.Controls.Add(this.textBoxRiverNMEA);
            resources.ApplyResources(this.tabPageRiver, "tabPageRiver");
            this.tabPageRiver.Name = "tabPageRiver";
            this.tabPageRiver.UseVisualStyleBackColor = true;
            // 
            // textBoxRiverBeam
            // 
            resources.ApplyResources(this.textBoxRiverBeam, "textBoxRiverBeam");
            this.textBoxRiverBeam.Name = "textBoxRiverBeam";
            this.textBoxRiverBeam.TabStop = false;
            // 
            // textBoxRiverBT2
            // 
            resources.ApplyResources(this.textBoxRiverBT2, "textBoxRiverBT2");
            this.textBoxRiverBT2.Name = "textBoxRiverBT2";
            // 
            // textBoxRiverBT
            // 
            resources.ApplyResources(this.textBoxRiverBT, "textBoxRiverBT");
            this.textBoxRiverBT.Name = "textBoxRiverBT";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.buttonRiverStop);
            this.groupBox5.Controls.Add(this.buttonRiverMinus);
            this.groupBox5.Controls.Add(this.buttonRiverPlayback);
            this.groupBox5.Controls.Add(this.buttonRiverPause);
            this.groupBox5.Controls.Add(this.buttonRiverPlus);
            this.groupBox5.Controls.Add(this.buttonRiverGo);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // buttonRiverStop
            // 
            resources.ApplyResources(this.buttonRiverStop, "buttonRiverStop");
            this.buttonRiverStop.Name = "buttonRiverStop";
            this.buttonRiverStop.UseVisualStyleBackColor = true;
            this.buttonRiverStop.Click += new System.EventHandler(this.buttonRiverStop_Click);
            // 
            // buttonRiverMinus
            // 
            resources.ApplyResources(this.buttonRiverMinus, "buttonRiverMinus");
            this.buttonRiverMinus.Name = "buttonRiverMinus";
            this.buttonRiverMinus.UseVisualStyleBackColor = true;
            this.buttonRiverMinus.Click += new System.EventHandler(this.buttonRiverMinus_Click);
            // 
            // buttonRiverPlayback
            // 
            resources.ApplyResources(this.buttonRiverPlayback, "buttonRiverPlayback");
            this.buttonRiverPlayback.Name = "buttonRiverPlayback";
            this.buttonRiverPlayback.UseVisualStyleBackColor = true;
            this.buttonRiverPlayback.Click += new System.EventHandler(this.buttonRiverPlayback_Click);
            // 
            // buttonRiverPause
            // 
            resources.ApplyResources(this.buttonRiverPause, "buttonRiverPause");
            this.buttonRiverPause.Name = "buttonRiverPause";
            this.buttonRiverPause.UseVisualStyleBackColor = true;
            this.buttonRiverPause.Click += new System.EventHandler(this.buttonRiverPause_Click);
            // 
            // buttonRiverPlus
            // 
            resources.ApplyResources(this.buttonRiverPlus, "buttonRiverPlus");
            this.buttonRiverPlus.Name = "buttonRiverPlus";
            this.buttonRiverPlus.UseVisualStyleBackColor = true;
            this.buttonRiverPlus.Click += new System.EventHandler(this.buttonRiverPlus_Click);
            // 
            // buttonRiverGo
            // 
            resources.ApplyResources(this.buttonRiverGo, "buttonRiverGo");
            this.buttonRiverGo.Name = "buttonRiverGo";
            this.buttonRiverGo.UseVisualStyleBackColor = true;
            this.buttonRiverGo.Click += new System.EventHandler(this.buttonRiverGo_Click);
            // 
            // textBoxRiverNMEA
            // 
            resources.ApplyResources(this.textBoxRiverNMEA, "textBoxRiverNMEA");
            this.textBoxRiverNMEA.Name = "textBoxRiverNMEA";
            // 
            // tabPageProfilePlot
            // 
            this.tabPageProfilePlot.BackColor = System.Drawing.Color.Transparent;
            this.tabPageProfilePlot.Controls.Add(this.label4);
            this.tabPageProfilePlot.Controls.Add(this.buttonEnsembleSubMinus);
            this.tabPageProfilePlot.Controls.Add(this.buttonEnsembleSubPlus);
            this.tabPageProfilePlot.Controls.Add(this.textBoxEnsembleSub);
            this.tabPageProfilePlot.Controls.Add(this.checkBoxBTNAVRecalc);
            this.tabPageProfilePlot.Controls.Add(this.checkBoxBTNAVuseZ);
            this.tabPageProfilePlot.Controls.Add(this.textBoxBTNavBinScale);
            this.tabPageProfilePlot.Controls.Add(this.buttonStatisticsClear);
            this.tabPageProfilePlot.Controls.Add(this.checkBoxBTNAVshowalways);
            this.tabPageProfilePlot.Controls.Add(this.groupBox10);
            this.tabPageProfilePlot.Controls.Add(this.groupBox21);
            this.tabPageProfilePlot.Controls.Add(this.buttonBTnavBinMinus);
            this.tabPageProfilePlot.Controls.Add(this.buttonBTnavBinPlus);
            this.tabPageProfilePlot.Controls.Add(this.textBoxBTNavBin);
            this.tabPageProfilePlot.Controls.Add(this.groupBoxADCPControl2);
            this.tabPageProfilePlot.Controls.Add(this.groupBoxPlayback);
            this.tabPageProfilePlot.Controls.Add(this.buttonMinusProfileScale);
            this.tabPageProfilePlot.Controls.Add(this.buttonPlusProfileScale);
            this.tabPageProfilePlot.Controls.Add(this.pictureBoxProfile);
            this.tabPageProfilePlot.Controls.Add(this.textBoxProfile);
            resources.ApplyResources(this.tabPageProfilePlot, "tabPageProfilePlot");
            this.tabPageProfilePlot.Name = "tabPageProfilePlot";
            this.tabPageProfilePlot.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // buttonEnsembleSubMinus
            // 
            resources.ApplyResources(this.buttonEnsembleSubMinus, "buttonEnsembleSubMinus");
            this.buttonEnsembleSubMinus.Name = "buttonEnsembleSubMinus";
            this.buttonEnsembleSubMinus.UseVisualStyleBackColor = true;
            this.buttonEnsembleSubMinus.Click += new System.EventHandler(this.buttonEnsembleSubMinus_Click);
            // 
            // buttonEnsembleSubPlus
            // 
            resources.ApplyResources(this.buttonEnsembleSubPlus, "buttonEnsembleSubPlus");
            this.buttonEnsembleSubPlus.Name = "buttonEnsembleSubPlus";
            this.buttonEnsembleSubPlus.UseVisualStyleBackColor = true;
            this.buttonEnsembleSubPlus.Click += new System.EventHandler(this.buttonEnsembleSubPlus_Click);
            // 
            // textBoxEnsembleSub
            // 
            this.textBoxEnsembleSub.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxEnsembleSub, "textBoxEnsembleSub");
            this.textBoxEnsembleSub.Name = "textBoxEnsembleSub";
            this.textBoxEnsembleSub.TextChanged += new System.EventHandler(this.textBoxEnsembleSub_TextChanged);
            // 
            // checkBoxBTNAVRecalc
            // 
            resources.ApplyResources(this.checkBoxBTNAVRecalc, "checkBoxBTNAVRecalc");
            this.checkBoxBTNAVRecalc.Name = "checkBoxBTNAVRecalc";
            this.checkBoxBTNAVRecalc.UseVisualStyleBackColor = true;
            this.checkBoxBTNAVRecalc.CheckedChanged += new System.EventHandler(this.checkBoxBTNAVRecalc_CheckedChanged);
            // 
            // checkBoxBTNAVuseZ
            // 
            resources.ApplyResources(this.checkBoxBTNAVuseZ, "checkBoxBTNAVuseZ");
            this.checkBoxBTNAVuseZ.Name = "checkBoxBTNAVuseZ";
            this.checkBoxBTNAVuseZ.UseVisualStyleBackColor = true;
            this.checkBoxBTNAVuseZ.CheckedChanged += new System.EventHandler(this.checkBoxBTNAVuseZ_CheckedChanged);
            // 
            // textBoxBTNavBinScale
            // 
            this.textBoxBTNavBinScale.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxBTNavBinScale, "textBoxBTNavBinScale");
            this.textBoxBTNavBinScale.Name = "textBoxBTNavBinScale";
            // 
            // buttonStatisticsClear
            // 
            resources.ApplyResources(this.buttonStatisticsClear, "buttonStatisticsClear");
            this.buttonStatisticsClear.Name = "buttonStatisticsClear";
            this.buttonStatisticsClear.UseVisualStyleBackColor = true;
            this.buttonStatisticsClear.Click += new System.EventHandler(this.buttonClearAverage_Click);
            // 
            // checkBoxBTNAVshowalways
            // 
            resources.ApplyResources(this.checkBoxBTNAVshowalways, "checkBoxBTNAVshowalways");
            this.checkBoxBTNAVshowalways.Name = "checkBoxBTNAVshowalways";
            this.checkBoxBTNAVshowalways.UseVisualStyleBackColor = true;
            this.checkBoxBTNAVshowalways.CheckedChanged += new System.EventHandler(this.checkBoxBTNAVshowalways_CheckedChanged);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.groupBox23);
            this.groupBox10.Controls.Add(this.groupBox22);
            this.groupBox10.Controls.Add(this.buttonCoordinate);
            resources.ApplyResources(this.groupBox10, "groupBox10");
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.TabStop = false;
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.radioButtonStatisticsSD);
            this.groupBox23.Controls.Add(this.radioButtonStatisticsAVG);
            this.groupBox23.Controls.Add(this.radioButtonStatisticsNone);
            this.groupBox23.Controls.Add(this.label37);
            this.groupBox23.Controls.Add(this.checkBoxProfStatPeaks);
            resources.ApplyResources(this.groupBox23, "groupBox23");
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.TabStop = false;
            // 
            // radioButtonStatisticsSD
            // 
            resources.ApplyResources(this.radioButtonStatisticsSD, "radioButtonStatisticsSD");
            this.radioButtonStatisticsSD.Name = "radioButtonStatisticsSD";
            this.radioButtonStatisticsSD.UseVisualStyleBackColor = true;
            this.radioButtonStatisticsSD.CheckedChanged += new System.EventHandler(this.radioButtonStatisticsSD_CheckedChanged);
            // 
            // radioButtonStatisticsAVG
            // 
            resources.ApplyResources(this.radioButtonStatisticsAVG, "radioButtonStatisticsAVG");
            this.radioButtonStatisticsAVG.Name = "radioButtonStatisticsAVG";
            this.radioButtonStatisticsAVG.UseVisualStyleBackColor = true;
            this.radioButtonStatisticsAVG.CheckedChanged += new System.EventHandler(this.radioButtonStatisticsAVG_CheckedChanged);
            // 
            // radioButtonStatisticsNone
            // 
            resources.ApplyResources(this.radioButtonStatisticsNone, "radioButtonStatisticsNone");
            this.radioButtonStatisticsNone.Checked = true;
            this.radioButtonStatisticsNone.Name = "radioButtonStatisticsNone";
            this.radioButtonStatisticsNone.TabStop = true;
            this.radioButtonStatisticsNone.UseVisualStyleBackColor = true;
            this.radioButtonStatisticsNone.CheckedChanged += new System.EventHandler(this.radioButtonStatisticsNone_CheckedChanged);
            // 
            // label37
            // 
            resources.ApplyResources(this.label37, "label37");
            this.label37.Name = "label37";
            // 
            // checkBoxProfStatPeaks
            // 
            resources.ApplyResources(this.checkBoxProfStatPeaks, "checkBoxProfStatPeaks");
            this.checkBoxProfStatPeaks.Checked = true;
            this.checkBoxProfStatPeaks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxProfStatPeaks.Name = "checkBoxProfStatPeaks";
            this.checkBoxProfStatPeaks.UseVisualStyleBackColor = true;
            this.checkBoxProfStatPeaks.CheckedChanged += new System.EventHandler(this.checkBoxProfStatPeaks_CheckedChanged);
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.buttonBinMinus);
            this.groupBox22.Controls.Add(this.textBoxFirstBin);
            this.groupBox22.Controls.Add(this.buttonBinPlus);
            resources.ApplyResources(this.groupBox22, "groupBox22");
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.TabStop = false;
            // 
            // buttonBinMinus
            // 
            resources.ApplyResources(this.buttonBinMinus, "buttonBinMinus");
            this.buttonBinMinus.Name = "buttonBinMinus";
            this.buttonBinMinus.UseVisualStyleBackColor = true;
            this.buttonBinMinus.Click += new System.EventHandler(this.buttonBinMinus_Click);
            // 
            // textBoxFirstBin
            // 
            this.textBoxFirstBin.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxFirstBin, "textBoxFirstBin");
            this.textBoxFirstBin.Name = "textBoxFirstBin";
            this.textBoxFirstBin.TextChanged += new System.EventHandler(this.textBoxFirstBin_TextChanged);
            // 
            // buttonBinPlus
            // 
            resources.ApplyResources(this.buttonBinPlus, "buttonBinPlus");
            this.buttonBinPlus.Name = "buttonBinPlus";
            this.buttonBinPlus.UseVisualStyleBackColor = true;
            this.buttonBinPlus.Click += new System.EventHandler(this.buttonBinPlus_Click);
            // 
            // buttonCoordinate
            // 
            resources.ApplyResources(this.buttonCoordinate, "buttonCoordinate");
            this.buttonCoordinate.Name = "buttonCoordinate";
            this.buttonCoordinate.UseVisualStyleBackColor = true;
            this.buttonCoordinate.Click += new System.EventHandler(this.buttonCoordinate_Click);
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.radioButtonProfileDisplayBottomTrack);
            this.groupBox21.Controls.Add(this.radioButtonProfileDisplayText);
            this.groupBox21.Controls.Add(this.radioButtonProfileDisplayGraph);
            resources.ApplyResources(this.groupBox21, "groupBox21");
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.TabStop = false;
            // 
            // radioButtonProfileDisplayBottomTrack
            // 
            resources.ApplyResources(this.radioButtonProfileDisplayBottomTrack, "radioButtonProfileDisplayBottomTrack");
            this.radioButtonProfileDisplayBottomTrack.Name = "radioButtonProfileDisplayBottomTrack";
            this.radioButtonProfileDisplayBottomTrack.UseVisualStyleBackColor = true;
            this.radioButtonProfileDisplayBottomTrack.CheckedChanged += new System.EventHandler(this.radioButtonProfileDisplayBottomTrack_CheckedChanged);
            // 
            // radioButtonProfileDisplayText
            // 
            resources.ApplyResources(this.radioButtonProfileDisplayText, "radioButtonProfileDisplayText");
            this.radioButtonProfileDisplayText.Name = "radioButtonProfileDisplayText";
            this.radioButtonProfileDisplayText.UseVisualStyleBackColor = true;
            this.radioButtonProfileDisplayText.CheckedChanged += new System.EventHandler(this.radioButtonProfileDisplayText_CheckedChanged);
            // 
            // radioButtonProfileDisplayGraph
            // 
            resources.ApplyResources(this.radioButtonProfileDisplayGraph, "radioButtonProfileDisplayGraph");
            this.radioButtonProfileDisplayGraph.Checked = true;
            this.radioButtonProfileDisplayGraph.Name = "radioButtonProfileDisplayGraph";
            this.radioButtonProfileDisplayGraph.TabStop = true;
            this.radioButtonProfileDisplayGraph.UseVisualStyleBackColor = true;
            this.radioButtonProfileDisplayGraph.CheckedChanged += new System.EventHandler(this.radioButtonProfileDisplayGraph_CheckedChanged);
            // 
            // buttonBTnavBinMinus
            // 
            resources.ApplyResources(this.buttonBTnavBinMinus, "buttonBTnavBinMinus");
            this.buttonBTnavBinMinus.Name = "buttonBTnavBinMinus";
            this.buttonBTnavBinMinus.UseVisualStyleBackColor = true;
            this.buttonBTnavBinMinus.Click += new System.EventHandler(this.buttonBTnavBinMinus_Click);
            // 
            // buttonBTnavBinPlus
            // 
            resources.ApplyResources(this.buttonBTnavBinPlus, "buttonBTnavBinPlus");
            this.buttonBTnavBinPlus.Name = "buttonBTnavBinPlus";
            this.buttonBTnavBinPlus.UseVisualStyleBackColor = true;
            this.buttonBTnavBinPlus.Click += new System.EventHandler(this.buttonBTnavBinPlus_Click);
            // 
            // textBoxBTNavBin
            // 
            this.textBoxBTNavBin.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxBTNavBin, "textBoxBTNavBin");
            this.textBoxBTNavBin.Name = "textBoxBTNavBin";
            // 
            // groupBoxADCPControl2
            // 
            this.groupBoxADCPControl2.Controls.Add(this.buttonSTOP);
            this.groupBoxADCPControl2.Controls.Add(this.buttonSTART);
            this.groupBoxADCPControl2.Controls.Add(this.buttonBreak);
            this.groupBoxADCPControl2.Controls.Add(this.buttonSleep);
            resources.ApplyResources(this.groupBoxADCPControl2, "groupBoxADCPControl2");
            this.groupBoxADCPControl2.Name = "groupBoxADCPControl2";
            this.groupBoxADCPControl2.TabStop = false;
            // 
            // buttonSTOP
            // 
            resources.ApplyResources(this.buttonSTOP, "buttonSTOP");
            this.buttonSTOP.Name = "buttonSTOP";
            this.buttonSTOP.UseVisualStyleBackColor = true;
            this.buttonSTOP.Click += new System.EventHandler(this.buttonSTOP_Click);
            // 
            // buttonSTART
            // 
            resources.ApplyResources(this.buttonSTART, "buttonSTART");
            this.buttonSTART.Name = "buttonSTART";
            this.buttonSTART.UseVisualStyleBackColor = true;
            this.buttonSTART.Click += new System.EventHandler(this.buttonSTART_Click);
            // 
            // buttonBreak
            // 
            resources.ApplyResources(this.buttonBreak, "buttonBreak");
            this.buttonBreak.Name = "buttonBreak";
            this.buttonBreak.UseVisualStyleBackColor = true;
            this.buttonBreak.Click += new System.EventHandler(this.buttonBreak_Click);
            // 
            // buttonSleep
            // 
            resources.ApplyResources(this.buttonSleep, "buttonSleep");
            this.buttonSleep.Name = "buttonSleep";
            this.buttonSleep.UseVisualStyleBackColor = true;
            this.buttonSleep.Click += new System.EventHandler(this.buttonSleep_Click);
            // 
            // groupBoxPlayback
            // 
            this.groupBoxPlayback.Controls.Add(this.buttonPlaybackStop);
            this.groupBoxPlayback.Controls.Add(this.buttonPlaybackStepBack);
            this.groupBoxPlayback.Controls.Add(this.buttonPLAYBACK);
            this.groupBoxPlayback.Controls.Add(this.buttonPlaybackPause);
            this.groupBoxPlayback.Controls.Add(this.buttonPlaybackStep);
            this.groupBoxPlayback.Controls.Add(this.buttonPlayBackGo);
            resources.ApplyResources(this.groupBoxPlayback, "groupBoxPlayback");
            this.groupBoxPlayback.Name = "groupBoxPlayback";
            this.groupBoxPlayback.TabStop = false;
            // 
            // buttonPlaybackStop
            // 
            resources.ApplyResources(this.buttonPlaybackStop, "buttonPlaybackStop");
            this.buttonPlaybackStop.Name = "buttonPlaybackStop";
            this.buttonPlaybackStop.UseVisualStyleBackColor = true;
            this.buttonPlaybackStop.Click += new System.EventHandler(this.buttonPlaybackStop_Click);
            // 
            // buttonPlaybackStepBack
            // 
            resources.ApplyResources(this.buttonPlaybackStepBack, "buttonPlaybackStepBack");
            this.buttonPlaybackStepBack.Name = "buttonPlaybackStepBack";
            this.buttonPlaybackStepBack.UseVisualStyleBackColor = true;
            this.buttonPlaybackStepBack.Click += new System.EventHandler(this.buttonPlaybackStepBack_Click);
            // 
            // buttonPLAYBACK
            // 
            resources.ApplyResources(this.buttonPLAYBACK, "buttonPLAYBACK");
            this.buttonPLAYBACK.Name = "buttonPLAYBACK";
            this.buttonPLAYBACK.UseVisualStyleBackColor = true;
            this.buttonPLAYBACK.Click += new System.EventHandler(this.buttonPLAYBACK_Click);
            // 
            // buttonPlaybackPause
            // 
            resources.ApplyResources(this.buttonPlaybackPause, "buttonPlaybackPause");
            this.buttonPlaybackPause.Name = "buttonPlaybackPause";
            this.buttonPlaybackPause.UseVisualStyleBackColor = true;
            this.buttonPlaybackPause.Click += new System.EventHandler(this.buttonPlaybackPause_Click);
            // 
            // buttonPlaybackStep
            // 
            resources.ApplyResources(this.buttonPlaybackStep, "buttonPlaybackStep");
            this.buttonPlaybackStep.Name = "buttonPlaybackStep";
            this.buttonPlaybackStep.UseVisualStyleBackColor = true;
            this.buttonPlaybackStep.Click += new System.EventHandler(this.buttonPlaybackStep_Click);
            // 
            // buttonPlayBackGo
            // 
            resources.ApplyResources(this.buttonPlayBackGo, "buttonPlayBackGo");
            this.buttonPlayBackGo.Name = "buttonPlayBackGo";
            this.buttonPlayBackGo.UseVisualStyleBackColor = true;
            this.buttonPlayBackGo.Click += new System.EventHandler(this.buttonPlayBackGo_Click);
            // 
            // buttonMinusProfileScale
            // 
            resources.ApplyResources(this.buttonMinusProfileScale, "buttonMinusProfileScale");
            this.buttonMinusProfileScale.Name = "buttonMinusProfileScale";
            this.buttonMinusProfileScale.UseVisualStyleBackColor = true;
            this.buttonMinusProfileScale.Click += new System.EventHandler(this.buttonMinusProfileScale_Click);
            // 
            // buttonPlusProfileScale
            // 
            resources.ApplyResources(this.buttonPlusProfileScale, "buttonPlusProfileScale");
            this.buttonPlusProfileScale.Name = "buttonPlusProfileScale";
            this.buttonPlusProfileScale.UseVisualStyleBackColor = true;
            this.buttonPlusProfileScale.Click += new System.EventHandler(this.buttonPlusProfileScale_Click);
            // 
            // pictureBoxProfile
            // 
            resources.ApplyResources(this.pictureBoxProfile, "pictureBoxProfile");
            this.pictureBoxProfile.Name = "pictureBoxProfile";
            this.pictureBoxProfile.TabStop = false;
            // 
            // textBoxProfile
            // 
            resources.ApplyResources(this.textBoxProfile, "textBoxProfile");
            this.textBoxProfile.Name = "textBoxProfile";
            // 
            // tabPageSeriesPlot
            // 
            this.tabPageSeriesPlot.Controls.Add(this.groupBox47);
            this.tabPageSeriesPlot.Controls.Add(this.groupBoxPlaybackSeries);
            this.tabPageSeriesPlot.Controls.Add(this.groupBox15);
            this.tabPageSeriesPlot.Controls.Add(this.groupBox14);
            this.tabPageSeriesPlot.Controls.Add(this.groupBox13);
            this.tabPageSeriesPlot.Controls.Add(this.groupBox12);
            this.tabPageSeriesPlot.Controls.Add(this.groupBox11);
            this.tabPageSeriesPlot.Controls.Add(this.buttonClearSeries);
            this.tabPageSeriesPlot.Controls.Add(this.buttonSeriesPlus);
            this.tabPageSeriesPlot.Controls.Add(this.buttonSeriesMinus);
            this.tabPageSeriesPlot.Controls.Add(this.pictureBoxSeries);
            resources.ApplyResources(this.tabPageSeriesPlot, "tabPageSeriesPlot");
            this.tabPageSeriesPlot.Name = "tabPageSeriesPlot";
            this.tabPageSeriesPlot.UseVisualStyleBackColor = true;
            // 
            // groupBox47
            // 
            resources.ApplyResources(this.groupBox47, "groupBox47");
            this.groupBox47.Controls.Add(this.buttonSeriesSubPlus);
            this.groupBox47.Controls.Add(this.buttonSeriesSubMinus);
            this.groupBox47.Controls.Add(this.textBoxSeriesSub);
            this.groupBox47.Name = "groupBox47";
            this.groupBox47.TabStop = false;
            // 
            // buttonSeriesSubPlus
            // 
            resources.ApplyResources(this.buttonSeriesSubPlus, "buttonSeriesSubPlus");
            this.buttonSeriesSubPlus.Name = "buttonSeriesSubPlus";
            this.buttonSeriesSubPlus.UseVisualStyleBackColor = true;
            this.buttonSeriesSubPlus.Click += new System.EventHandler(this.buttonSeriesSubPlus_Click);
            // 
            // buttonSeriesSubMinus
            // 
            resources.ApplyResources(this.buttonSeriesSubMinus, "buttonSeriesSubMinus");
            this.buttonSeriesSubMinus.Name = "buttonSeriesSubMinus";
            this.buttonSeriesSubMinus.UseVisualStyleBackColor = true;
            this.buttonSeriesSubMinus.Click += new System.EventHandler(this.buttonSeriesSubMinus_Click);
            // 
            // textBoxSeriesSub
            // 
            this.textBoxSeriesSub.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxSeriesSub, "textBoxSeriesSub");
            this.textBoxSeriesSub.Name = "textBoxSeriesSub";
            this.textBoxSeriesSub.TextChanged += new System.EventHandler(this.textBoxSeriesSub_TextChanged);
            // 
            // groupBoxPlaybackSeries
            // 
            this.groupBoxPlaybackSeries.Controls.Add(this.buttonPlaybackStopSeries);
            this.groupBoxPlaybackSeries.Controls.Add(this.buttonPlaybackStepBackSeries);
            this.groupBoxPlaybackSeries.Controls.Add(this.buttonPlaybackSeries);
            this.groupBoxPlaybackSeries.Controls.Add(this.buttonPlaybackPauseSeries);
            this.groupBoxPlaybackSeries.Controls.Add(this.buttonPlaybackStepSeries);
            this.groupBoxPlaybackSeries.Controls.Add(this.buttonPlaybackGoSeries);
            resources.ApplyResources(this.groupBoxPlaybackSeries, "groupBoxPlaybackSeries");
            this.groupBoxPlaybackSeries.Name = "groupBoxPlaybackSeries";
            this.groupBoxPlaybackSeries.TabStop = false;
            // 
            // buttonPlaybackStopSeries
            // 
            resources.ApplyResources(this.buttonPlaybackStopSeries, "buttonPlaybackStopSeries");
            this.buttonPlaybackStopSeries.Name = "buttonPlaybackStopSeries";
            this.buttonPlaybackStopSeries.UseVisualStyleBackColor = true;
            this.buttonPlaybackStopSeries.Click += new System.EventHandler(this.buttonPlaybackStopSeries_Click);
            // 
            // buttonPlaybackStepBackSeries
            // 
            resources.ApplyResources(this.buttonPlaybackStepBackSeries, "buttonPlaybackStepBackSeries");
            this.buttonPlaybackStepBackSeries.Name = "buttonPlaybackStepBackSeries";
            this.buttonPlaybackStepBackSeries.UseVisualStyleBackColor = true;
            this.buttonPlaybackStepBackSeries.Click += new System.EventHandler(this.buttonPlaybackStepBackSeries_Click);
            // 
            // buttonPlaybackSeries
            // 
            resources.ApplyResources(this.buttonPlaybackSeries, "buttonPlaybackSeries");
            this.buttonPlaybackSeries.Name = "buttonPlaybackSeries";
            this.buttonPlaybackSeries.UseVisualStyleBackColor = true;
            this.buttonPlaybackSeries.Click += new System.EventHandler(this.buttonPlaybackSeries_Click);
            // 
            // buttonPlaybackPauseSeries
            // 
            resources.ApplyResources(this.buttonPlaybackPauseSeries, "buttonPlaybackPauseSeries");
            this.buttonPlaybackPauseSeries.Name = "buttonPlaybackPauseSeries";
            this.buttonPlaybackPauseSeries.UseVisualStyleBackColor = true;
            this.buttonPlaybackPauseSeries.Click += new System.EventHandler(this.buttonPlaybackPauseSeries_Click);
            // 
            // buttonPlaybackStepSeries
            // 
            resources.ApplyResources(this.buttonPlaybackStepSeries, "buttonPlaybackStepSeries");
            this.buttonPlaybackStepSeries.Name = "buttonPlaybackStepSeries";
            this.buttonPlaybackStepSeries.UseVisualStyleBackColor = true;
            this.buttonPlaybackStepSeries.Click += new System.EventHandler(this.buttonPlaybackStepSeries_Click);
            // 
            // buttonPlaybackGoSeries
            // 
            resources.ApplyResources(this.buttonPlaybackGoSeries, "buttonPlaybackGoSeries");
            this.buttonPlaybackGoSeries.Name = "buttonPlaybackGoSeries";
            this.buttonPlaybackGoSeries.UseVisualStyleBackColor = true;
            this.buttonPlaybackGoSeries.Click += new System.EventHandler(this.buttonPlaybackGoSeries_Click);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.radioButtonSeriesCoordENU);
            this.groupBox15.Controls.Add(this.radioButtonSeriesCoordBeam);
            this.groupBox15.Controls.Add(this.radioButtonSeriesCoordXYZ);
            resources.ApplyResources(this.groupBox15, "groupBox15");
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.TabStop = false;
            // 
            // radioButtonSeriesCoordENU
            // 
            resources.ApplyResources(this.radioButtonSeriesCoordENU, "radioButtonSeriesCoordENU");
            this.radioButtonSeriesCoordENU.Checked = true;
            this.radioButtonSeriesCoordENU.Name = "radioButtonSeriesCoordENU";
            this.radioButtonSeriesCoordENU.TabStop = true;
            this.radioButtonSeriesCoordENU.UseVisualStyleBackColor = true;
            this.radioButtonSeriesCoordENU.CheckedChanged += new System.EventHandler(this.radioButtonSeriesCoordENU_CheckedChanged);
            // 
            // radioButtonSeriesCoordBeam
            // 
            resources.ApplyResources(this.radioButtonSeriesCoordBeam, "radioButtonSeriesCoordBeam");
            this.radioButtonSeriesCoordBeam.Name = "radioButtonSeriesCoordBeam";
            this.radioButtonSeriesCoordBeam.UseVisualStyleBackColor = true;
            this.radioButtonSeriesCoordBeam.CheckedChanged += new System.EventHandler(this.radioButtonSeriesCoordBeam_CheckedChanged);
            // 
            // radioButtonSeriesCoordXYZ
            // 
            resources.ApplyResources(this.radioButtonSeriesCoordXYZ, "radioButtonSeriesCoordXYZ");
            this.radioButtonSeriesCoordXYZ.Name = "radioButtonSeriesCoordXYZ";
            this.radioButtonSeriesCoordXYZ.UseVisualStyleBackColor = true;
            this.radioButtonSeriesCoordXYZ.CheckedChanged += new System.EventHandler(this.radioButtonSeriesCoordXYZ_CheckedChanged);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.radioButtonSeriesBTmag);
            this.groupBox14.Controls.Add(this.radioButtonSeriesBTrange);
            this.groupBox14.Controls.Add(this.radioButtonSeriesBTsnr);
            this.groupBox14.Controls.Add(this.radioButtonSeriesBTcor);
            this.groupBox14.Controls.Add(this.radioButtonSeriesBTvel);
            this.groupBox14.Controls.Add(this.radioButtonSeriesBTamp);
            resources.ApplyResources(this.groupBox14, "groupBox14");
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.TabStop = false;
            // 
            // radioButtonSeriesBTmag
            // 
            resources.ApplyResources(this.radioButtonSeriesBTmag, "radioButtonSeriesBTmag");
            this.radioButtonSeriesBTmag.Name = "radioButtonSeriesBTmag";
            this.radioButtonSeriesBTmag.TabStop = true;
            this.radioButtonSeriesBTmag.UseVisualStyleBackColor = true;
            this.radioButtonSeriesBTmag.CheckedChanged += new System.EventHandler(this.radioButtonSeriesBTmag_CheckedChanged);
            // 
            // radioButtonSeriesBTrange
            // 
            resources.ApplyResources(this.radioButtonSeriesBTrange, "radioButtonSeriesBTrange");
            this.radioButtonSeriesBTrange.Name = "radioButtonSeriesBTrange";
            this.radioButtonSeriesBTrange.TabStop = true;
            this.radioButtonSeriesBTrange.UseVisualStyleBackColor = true;
            this.radioButtonSeriesBTrange.CheckedChanged += new System.EventHandler(this.radioButtonSeriesBTrange_CheckedChanged);
            // 
            // radioButtonSeriesBTsnr
            // 
            resources.ApplyResources(this.radioButtonSeriesBTsnr, "radioButtonSeriesBTsnr");
            this.radioButtonSeriesBTsnr.Name = "radioButtonSeriesBTsnr";
            this.radioButtonSeriesBTsnr.TabStop = true;
            this.radioButtonSeriesBTsnr.UseVisualStyleBackColor = true;
            this.radioButtonSeriesBTsnr.CheckedChanged += new System.EventHandler(this.radioButtonSeriesBTsnr_CheckedChanged);
            // 
            // radioButtonSeriesBTcor
            // 
            resources.ApplyResources(this.radioButtonSeriesBTcor, "radioButtonSeriesBTcor");
            this.radioButtonSeriesBTcor.Name = "radioButtonSeriesBTcor";
            this.radioButtonSeriesBTcor.TabStop = true;
            this.radioButtonSeriesBTcor.UseVisualStyleBackColor = true;
            this.radioButtonSeriesBTcor.CheckedChanged += new System.EventHandler(this.radioButtonSeriesBTcor_CheckedChanged);
            // 
            // radioButtonSeriesBTvel
            // 
            resources.ApplyResources(this.radioButtonSeriesBTvel, "radioButtonSeriesBTvel");
            this.radioButtonSeriesBTvel.Checked = true;
            this.radioButtonSeriesBTvel.Name = "radioButtonSeriesBTvel";
            this.radioButtonSeriesBTvel.TabStop = true;
            this.radioButtonSeriesBTvel.UseVisualStyleBackColor = true;
            this.radioButtonSeriesBTvel.CheckedChanged += new System.EventHandler(this.radioButtonSeriesBTvel_CheckedChanged);
            // 
            // radioButtonSeriesBTamp
            // 
            resources.ApplyResources(this.radioButtonSeriesBTamp, "radioButtonSeriesBTamp");
            this.radioButtonSeriesBTamp.Name = "radioButtonSeriesBTamp";
            this.radioButtonSeriesBTamp.TabStop = true;
            this.radioButtonSeriesBTamp.UseVisualStyleBackColor = true;
            this.radioButtonSeriesBTamp.CheckedChanged += new System.EventHandler(this.radioButtonSeriesBTamp_CheckedChanged);
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.radioButtonSeriesWTcor);
            this.groupBox13.Controls.Add(this.radioButtonSeriesWTvel);
            this.groupBox13.Controls.Add(this.radioButtonSeriesWTamp);
            resources.ApplyResources(this.groupBox13, "groupBox13");
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.TabStop = false;
            // 
            // radioButtonSeriesWTcor
            // 
            resources.ApplyResources(this.radioButtonSeriesWTcor, "radioButtonSeriesWTcor");
            this.radioButtonSeriesWTcor.Name = "radioButtonSeriesWTcor";
            this.radioButtonSeriesWTcor.TabStop = true;
            this.radioButtonSeriesWTcor.UseVisualStyleBackColor = true;
            // 
            // radioButtonSeriesWTvel
            // 
            resources.ApplyResources(this.radioButtonSeriesWTvel, "radioButtonSeriesWTvel");
            this.radioButtonSeriesWTvel.Checked = true;
            this.radioButtonSeriesWTvel.Name = "radioButtonSeriesWTvel";
            this.radioButtonSeriesWTvel.TabStop = true;
            this.radioButtonSeriesWTvel.UseVisualStyleBackColor = true;
            // 
            // radioButtonSeriesWTamp
            // 
            resources.ApplyResources(this.radioButtonSeriesWTamp, "radioButtonSeriesWTamp");
            this.radioButtonSeriesWTamp.Name = "radioButtonSeriesWTamp";
            this.radioButtonSeriesWTamp.TabStop = true;
            this.radioButtonSeriesWTamp.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.radioButtonSeriesWPRT);
            this.groupBox12.Controls.Add(this.radioButtonSeriesAncillaryProfile);
            this.groupBox12.Controls.Add(this.radioButtonSeriesAncillaryBT);
            this.groupBox12.Controls.Add(this.radioButtonSeriesWT);
            this.groupBox12.Controls.Add(this.radioButtonSeriesProfile);
            this.groupBox12.Controls.Add(this.radioButtonSeriesBT);
            resources.ApplyResources(this.groupBox12, "groupBox12");
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.TabStop = false;
            // 
            // radioButtonSeriesWPRT
            // 
            resources.ApplyResources(this.radioButtonSeriesWPRT, "radioButtonSeriesWPRT");
            this.radioButtonSeriesWPRT.Name = "radioButtonSeriesWPRT";
            this.radioButtonSeriesWPRT.UseVisualStyleBackColor = true;
            this.radioButtonSeriesWPRT.CheckedChanged += new System.EventHandler(this.radioButtonSeriesWPBT_CheckedChanged);
            // 
            // radioButtonSeriesAncillaryProfile
            // 
            resources.ApplyResources(this.radioButtonSeriesAncillaryProfile, "radioButtonSeriesAncillaryProfile");
            this.radioButtonSeriesAncillaryProfile.Name = "radioButtonSeriesAncillaryProfile";
            this.radioButtonSeriesAncillaryProfile.UseVisualStyleBackColor = true;
            this.radioButtonSeriesAncillaryProfile.CheckedChanged += new System.EventHandler(this.radioButtonSeriesAncillaryProfile_CheckedChanged);
            // 
            // radioButtonSeriesAncillaryBT
            // 
            resources.ApplyResources(this.radioButtonSeriesAncillaryBT, "radioButtonSeriesAncillaryBT");
            this.radioButtonSeriesAncillaryBT.Name = "radioButtonSeriesAncillaryBT";
            this.radioButtonSeriesAncillaryBT.UseVisualStyleBackColor = true;
            this.radioButtonSeriesAncillaryBT.CheckedChanged += new System.EventHandler(this.radioButtonSeriesAncillaryBT_CheckedChanged);
            // 
            // radioButtonSeriesWT
            // 
            resources.ApplyResources(this.radioButtonSeriesWT, "radioButtonSeriesWT");
            this.radioButtonSeriesWT.Name = "radioButtonSeriesWT";
            this.radioButtonSeriesWT.UseVisualStyleBackColor = true;
            this.radioButtonSeriesWT.CheckedChanged += new System.EventHandler(this.radioButtonSeriesWT_CheckedChanged);
            // 
            // radioButtonSeriesProfile
            // 
            resources.ApplyResources(this.radioButtonSeriesProfile, "radioButtonSeriesProfile");
            this.radioButtonSeriesProfile.Checked = true;
            this.radioButtonSeriesProfile.Name = "radioButtonSeriesProfile";
            this.radioButtonSeriesProfile.TabStop = true;
            this.radioButtonSeriesProfile.UseVisualStyleBackColor = true;
            this.radioButtonSeriesProfile.CheckedChanged += new System.EventHandler(this.radioButtonSeriesProfile_CheckedChanged);
            // 
            // radioButtonSeriesBT
            // 
            resources.ApplyResources(this.radioButtonSeriesBT, "radioButtonSeriesBT");
            this.radioButtonSeriesBT.Name = "radioButtonSeriesBT";
            this.radioButtonSeriesBT.UseVisualStyleBackColor = true;
            this.radioButtonSeriesBT.CheckedChanged += new System.EventHandler(this.radioButtonSeriesBT_CheckedChanged);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.buttonSeriesBinPlus);
            this.groupBox11.Controls.Add(this.buttonSeriesBinMinus);
            this.groupBox11.Controls.Add(this.label2);
            this.groupBox11.Controls.Add(this.textBoxSeriesBin);
            this.groupBox11.Controls.Add(this.radioButtonSeriesProfileCor);
            this.groupBox11.Controls.Add(this.radioButtonSeriesProfileVel);
            this.groupBox11.Controls.Add(this.radioButtonSeriesProfileAmp);
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // buttonSeriesBinPlus
            // 
            resources.ApplyResources(this.buttonSeriesBinPlus, "buttonSeriesBinPlus");
            this.buttonSeriesBinPlus.Name = "buttonSeriesBinPlus";
            this.buttonSeriesBinPlus.UseVisualStyleBackColor = true;
            this.buttonSeriesBinPlus.Click += new System.EventHandler(this.buttonSeriesBinPlus_Click);
            // 
            // buttonSeriesBinMinus
            // 
            resources.ApplyResources(this.buttonSeriesBinMinus, "buttonSeriesBinMinus");
            this.buttonSeriesBinMinus.Name = "buttonSeriesBinMinus";
            this.buttonSeriesBinMinus.UseVisualStyleBackColor = true;
            this.buttonSeriesBinMinus.Click += new System.EventHandler(this.buttonSeriesBinMinus_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBoxSeriesBin
            // 
            this.textBoxSeriesBin.AcceptsReturn = true;
            resources.ApplyResources(this.textBoxSeriesBin, "textBoxSeriesBin");
            this.textBoxSeriesBin.Name = "textBoxSeriesBin";
            this.textBoxSeriesBin.TextChanged += new System.EventHandler(this.textBoxSeriesBin_TextChanged);
            // 
            // radioButtonSeriesProfileCor
            // 
            resources.ApplyResources(this.radioButtonSeriesProfileCor, "radioButtonSeriesProfileCor");
            this.radioButtonSeriesProfileCor.Name = "radioButtonSeriesProfileCor";
            this.radioButtonSeriesProfileCor.TabStop = true;
            this.radioButtonSeriesProfileCor.UseVisualStyleBackColor = true;
            this.radioButtonSeriesProfileCor.CheckedChanged += new System.EventHandler(this.radioButtonSeriesProfileCor_CheckedChanged);
            // 
            // radioButtonSeriesProfileVel
            // 
            resources.ApplyResources(this.radioButtonSeriesProfileVel, "radioButtonSeriesProfileVel");
            this.radioButtonSeriesProfileVel.Checked = true;
            this.radioButtonSeriesProfileVel.Name = "radioButtonSeriesProfileVel";
            this.radioButtonSeriesProfileVel.TabStop = true;
            this.radioButtonSeriesProfileVel.UseVisualStyleBackColor = true;
            this.radioButtonSeriesProfileVel.CheckedChanged += new System.EventHandler(this.radioButtonSeriesProfileVel_CheckedChanged);
            // 
            // radioButtonSeriesProfileAmp
            // 
            resources.ApplyResources(this.radioButtonSeriesProfileAmp, "radioButtonSeriesProfileAmp");
            this.radioButtonSeriesProfileAmp.Name = "radioButtonSeriesProfileAmp";
            this.radioButtonSeriesProfileAmp.TabStop = true;
            this.radioButtonSeriesProfileAmp.UseVisualStyleBackColor = true;
            this.radioButtonSeriesProfileAmp.CheckedChanged += new System.EventHandler(this.radioButtonSeriesProfileAmp_CheckedChanged);
            // 
            // buttonClearSeries
            // 
            resources.ApplyResources(this.buttonClearSeries, "buttonClearSeries");
            this.buttonClearSeries.Name = "buttonClearSeries";
            this.buttonClearSeries.UseVisualStyleBackColor = true;
            this.buttonClearSeries.Click += new System.EventHandler(this.buttonClearSeries_Click);
            // 
            // buttonSeriesPlus
            // 
            resources.ApplyResources(this.buttonSeriesPlus, "buttonSeriesPlus");
            this.buttonSeriesPlus.Name = "buttonSeriesPlus";
            this.buttonSeriesPlus.UseVisualStyleBackColor = true;
            this.buttonSeriesPlus.Click += new System.EventHandler(this.buttonSeriesPlus_Click);
            // 
            // buttonSeriesMinus
            // 
            resources.ApplyResources(this.buttonSeriesMinus, "buttonSeriesMinus");
            this.buttonSeriesMinus.Name = "buttonSeriesMinus";
            this.buttonSeriesMinus.UseVisualStyleBackColor = true;
            this.buttonSeriesMinus.Click += new System.EventHandler(this.buttonSeriesMinus_Click);
            // 
            // pictureBoxSeries
            // 
            resources.ApplyResources(this.pictureBoxSeries, "pictureBoxSeries");
            this.pictureBoxSeries.Name = "pictureBoxSeries";
            this.pictureBoxSeries.TabStop = false;
            // 
            // tabPageNMEA
            // 
            this.tabPageNMEA.BackColor = System.Drawing.Color.Transparent;
            this.tabPageNMEA.Controls.Add(this.checkBoxNMEA_ASCII_Input);
            this.tabPageNMEA.Controls.Add(this.groupBoxPlaybackNMEA);
            this.tabPageNMEA.Controls.Add(this.textBoxDecoded);
            this.tabPageNMEA.Controls.Add(this.textBoxNavigation);
            this.tabPageNMEA.Controls.Add(this.textBoxCapturedNMEA);
            resources.ApplyResources(this.tabPageNMEA, "tabPageNMEA");
            this.tabPageNMEA.Name = "tabPageNMEA";
            this.tabPageNMEA.UseVisualStyleBackColor = true;
            // 
            // checkBoxNMEA_ASCII_Input
            // 
            resources.ApplyResources(this.checkBoxNMEA_ASCII_Input, "checkBoxNMEA_ASCII_Input");
            this.checkBoxNMEA_ASCII_Input.Name = "checkBoxNMEA_ASCII_Input";
            this.checkBoxNMEA_ASCII_Input.UseVisualStyleBackColor = true;
            this.checkBoxNMEA_ASCII_Input.CheckedChanged += new System.EventHandler(this.checkBoxNMEA_ASCII_Input_CheckedChanged);
            // 
            // groupBoxPlaybackNMEA
            // 
            this.groupBoxPlaybackNMEA.Controls.Add(this.buttonPlaybackStopNMEA);
            this.groupBoxPlaybackNMEA.Controls.Add(this.buttonPlaybackStepBackNMEA);
            this.groupBoxPlaybackNMEA.Controls.Add(this.buttonPlaybackNMEA);
            this.groupBoxPlaybackNMEA.Controls.Add(this.buttonPlaybackPauseNMEA);
            this.groupBoxPlaybackNMEA.Controls.Add(this.buttonPlaybackStepNMEA);
            this.groupBoxPlaybackNMEA.Controls.Add(this.buttonPlaybackGoNMEA);
            resources.ApplyResources(this.groupBoxPlaybackNMEA, "groupBoxPlaybackNMEA");
            this.groupBoxPlaybackNMEA.Name = "groupBoxPlaybackNMEA";
            this.groupBoxPlaybackNMEA.TabStop = false;
            // 
            // buttonPlaybackStopNMEA
            // 
            resources.ApplyResources(this.buttonPlaybackStopNMEA, "buttonPlaybackStopNMEA");
            this.buttonPlaybackStopNMEA.Name = "buttonPlaybackStopNMEA";
            this.buttonPlaybackStopNMEA.UseVisualStyleBackColor = true;
            this.buttonPlaybackStopNMEA.Click += new System.EventHandler(this.buttonPlaybackStopNMEA_Click);
            // 
            // buttonPlaybackStepBackNMEA
            // 
            resources.ApplyResources(this.buttonPlaybackStepBackNMEA, "buttonPlaybackStepBackNMEA");
            this.buttonPlaybackStepBackNMEA.Name = "buttonPlaybackStepBackNMEA";
            this.buttonPlaybackStepBackNMEA.UseVisualStyleBackColor = true;
            this.buttonPlaybackStepBackNMEA.Click += new System.EventHandler(this.buttonPlaybackStepBackNMEA_Click);
            // 
            // buttonPlaybackNMEA
            // 
            resources.ApplyResources(this.buttonPlaybackNMEA, "buttonPlaybackNMEA");
            this.buttonPlaybackNMEA.Name = "buttonPlaybackNMEA";
            this.buttonPlaybackNMEA.UseVisualStyleBackColor = true;
            this.buttonPlaybackNMEA.Click += new System.EventHandler(this.buttonPlaybackNMEA_Click);
            // 
            // buttonPlaybackPauseNMEA
            // 
            resources.ApplyResources(this.buttonPlaybackPauseNMEA, "buttonPlaybackPauseNMEA");
            this.buttonPlaybackPauseNMEA.Name = "buttonPlaybackPauseNMEA";
            this.buttonPlaybackPauseNMEA.UseVisualStyleBackColor = true;
            this.buttonPlaybackPauseNMEA.Click += new System.EventHandler(this.buttonPlaybackPauseNMEA_Click);
            // 
            // buttonPlaybackStepNMEA
            // 
            resources.ApplyResources(this.buttonPlaybackStepNMEA, "buttonPlaybackStepNMEA");
            this.buttonPlaybackStepNMEA.Name = "buttonPlaybackStepNMEA";
            this.buttonPlaybackStepNMEA.UseVisualStyleBackColor = true;
            this.buttonPlaybackStepNMEA.Click += new System.EventHandler(this.buttonPlaybackStepNMEA_Click);
            // 
            // buttonPlaybackGoNMEA
            // 
            resources.ApplyResources(this.buttonPlaybackGoNMEA, "buttonPlaybackGoNMEA");
            this.buttonPlaybackGoNMEA.Name = "buttonPlaybackGoNMEA";
            this.buttonPlaybackGoNMEA.UseVisualStyleBackColor = true;
            this.buttonPlaybackGoNMEA.Click += new System.EventHandler(this.buttonPlaybackGoNMEA_Click);
            // 
            // textBoxDecoded
            // 
            resources.ApplyResources(this.textBoxDecoded, "textBoxDecoded");
            this.textBoxDecoded.Name = "textBoxDecoded";
            // 
            // textBoxNavigation
            // 
            resources.ApplyResources(this.textBoxNavigation, "textBoxNavigation");
            this.textBoxNavigation.Name = "textBoxNavigation";
            // 
            // textBoxCapturedNMEA
            // 
            resources.ApplyResources(this.textBoxCapturedNMEA, "textBoxCapturedNMEA");
            this.textBoxCapturedNMEA.Name = "textBoxCapturedNMEA";
            // 
            // tabDownload
            // 
            this.tabDownload.Controls.Add(this.textBoxDownloadBPS);
            this.tabDownload.Controls.Add(this.label63);
            this.tabDownload.Controls.Add(this.textBoxDownloadPercent);
            this.tabDownload.Controls.Add(this.label62);
            this.tabDownload.Controls.Add(this.label56);
            this.tabDownload.Controls.Add(this.textBoxDownloadSeconds);
            this.tabDownload.Controls.Add(this.label49);
            this.tabDownload.Controls.Add(this.textBoxDownloadBytes);
            this.tabDownload.Controls.Add(this.label48);
            this.tabDownload.Controls.Add(this.textBoxDownloadTries);
            this.tabDownload.Controls.Add(this.label43);
            this.tabDownload.Controls.Add(this.textBoxDownloadRetries);
            this.tabDownload.Controls.Add(this.groupBox34);
            this.tabDownload.Controls.Add(this.textBoxWavesRecoverDownload);
            this.tabDownload.Controls.Add(this.textBoxWavesRecover);
            resources.ApplyResources(this.tabDownload, "tabDownload");
            this.tabDownload.Name = "tabDownload";
            this.tabDownload.UseVisualStyleBackColor = true;
            // 
            // textBoxDownloadBPS
            // 
            resources.ApplyResources(this.textBoxDownloadBPS, "textBoxDownloadBPS");
            this.textBoxDownloadBPS.Name = "textBoxDownloadBPS";
            // 
            // label63
            // 
            resources.ApplyResources(this.label63, "label63");
            this.label63.Name = "label63";
            // 
            // textBoxDownloadPercent
            // 
            resources.ApplyResources(this.textBoxDownloadPercent, "textBoxDownloadPercent");
            this.textBoxDownloadPercent.Name = "textBoxDownloadPercent";
            // 
            // label62
            // 
            resources.ApplyResources(this.label62, "label62");
            this.label62.Name = "label62";
            // 
            // label56
            // 
            resources.ApplyResources(this.label56, "label56");
            this.label56.Name = "label56";
            // 
            // textBoxDownloadSeconds
            // 
            resources.ApplyResources(this.textBoxDownloadSeconds, "textBoxDownloadSeconds");
            this.textBoxDownloadSeconds.Name = "textBoxDownloadSeconds";
            // 
            // label49
            // 
            resources.ApplyResources(this.label49, "label49");
            this.label49.Name = "label49";
            // 
            // textBoxDownloadBytes
            // 
            resources.ApplyResources(this.textBoxDownloadBytes, "textBoxDownloadBytes");
            this.textBoxDownloadBytes.Name = "textBoxDownloadBytes";
            // 
            // label48
            // 
            resources.ApplyResources(this.label48, "label48");
            this.label48.Name = "label48";
            // 
            // textBoxDownloadTries
            // 
            resources.ApplyResources(this.textBoxDownloadTries, "textBoxDownloadTries");
            this.textBoxDownloadTries.Name = "textBoxDownloadTries";
            // 
            // label43
            // 
            resources.ApplyResources(this.label43, "label43");
            this.label43.Name = "label43";
            // 
            // textBoxDownloadRetries
            // 
            resources.ApplyResources(this.textBoxDownloadRetries, "textBoxDownloadRetries");
            this.textBoxDownloadRetries.Name = "textBoxDownloadRetries";
            // 
            // groupBox34
            // 
            this.groupBox34.Controls.Add(this.label1);
            this.groupBox34.Controls.Add(this.textBoxFirstFile);
            this.groupBox34.Controls.Add(this.buttonRecoverDownloadCancel);
            this.groupBox34.Controls.Add(this.buttonRecoverDownload);
            this.groupBox34.Controls.Add(this.buttonRecoverDir);
            this.groupBox34.Controls.Add(this.groupBox7);
            resources.ApplyResources(this.groupBox34, "groupBox34");
            this.groupBox34.Name = "groupBox34";
            this.groupBox34.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBoxFirstFile
            // 
            resources.ApplyResources(this.textBoxFirstFile, "textBoxFirstFile");
            this.textBoxFirstFile.Name = "textBoxFirstFile";
            // 
            // buttonRecoverDownloadCancel
            // 
            resources.ApplyResources(this.buttonRecoverDownloadCancel, "buttonRecoverDownloadCancel");
            this.buttonRecoverDownloadCancel.Name = "buttonRecoverDownloadCancel";
            this.buttonRecoverDownloadCancel.UseVisualStyleBackColor = true;
            this.buttonRecoverDownloadCancel.Click += new System.EventHandler(this.buttonRecoverDownloadCancel_Click);
            // 
            // buttonRecoverDownload
            // 
            resources.ApplyResources(this.buttonRecoverDownload, "buttonRecoverDownload");
            this.buttonRecoverDownload.Name = "buttonRecoverDownload";
            this.buttonRecoverDownload.UseVisualStyleBackColor = true;
            this.buttonRecoverDownload.Click += new System.EventHandler(this.buttonRecoverDownload_Click);
            // 
            // buttonRecoverDir
            // 
            resources.ApplyResources(this.buttonRecoverDir, "buttonRecoverDir");
            this.buttonRecoverDir.Name = "buttonRecoverDir";
            this.buttonRecoverDir.UseVisualStyleBackColor = true;
            this.buttonRecoverDir.Click += new System.EventHandler(this.buttonRecoverDir_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.radioButtonTxtDownload);
            this.groupBox7.Controls.Add(this.radioButtonRawDownload);
            this.groupBox7.Controls.Add(this.radioButtonAllDownLoad);
            this.groupBox7.Controls.Add(this.radioButtonBurstDownLoad);
            this.groupBox7.Controls.Add(this.radioButtonProfileDownLoad);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // radioButtonTxtDownload
            // 
            resources.ApplyResources(this.radioButtonTxtDownload, "radioButtonTxtDownload");
            this.radioButtonTxtDownload.Name = "radioButtonTxtDownload";
            this.radioButtonTxtDownload.UseVisualStyleBackColor = true;
            // 
            // radioButtonRawDownload
            // 
            resources.ApplyResources(this.radioButtonRawDownload, "radioButtonRawDownload");
            this.radioButtonRawDownload.Name = "radioButtonRawDownload";
            this.radioButtonRawDownload.UseVisualStyleBackColor = true;
            // 
            // radioButtonAllDownLoad
            // 
            resources.ApplyResources(this.radioButtonAllDownLoad, "radioButtonAllDownLoad");
            this.radioButtonAllDownLoad.Checked = true;
            this.radioButtonAllDownLoad.Name = "radioButtonAllDownLoad";
            this.radioButtonAllDownLoad.TabStop = true;
            this.radioButtonAllDownLoad.UseVisualStyleBackColor = true;
            // 
            // radioButtonBurstDownLoad
            // 
            resources.ApplyResources(this.radioButtonBurstDownLoad, "radioButtonBurstDownLoad");
            this.radioButtonBurstDownLoad.Name = "radioButtonBurstDownLoad";
            this.radioButtonBurstDownLoad.UseVisualStyleBackColor = true;
            // 
            // radioButtonProfileDownLoad
            // 
            resources.ApplyResources(this.radioButtonProfileDownLoad, "radioButtonProfileDownLoad");
            this.radioButtonProfileDownLoad.Name = "radioButtonProfileDownLoad";
            this.radioButtonProfileDownLoad.UseVisualStyleBackColor = true;
            // 
            // textBoxWavesRecoverDownload
            // 
            resources.ApplyResources(this.textBoxWavesRecoverDownload, "textBoxWavesRecoverDownload");
            this.textBoxWavesRecoverDownload.Name = "textBoxWavesRecoverDownload";
            this.textBoxWavesRecoverDownload.TabStop = false;
            // 
            // textBoxWavesRecover
            // 
            resources.ApplyResources(this.textBoxWavesRecover, "textBoxWavesRecover");
            this.textBoxWavesRecover.Name = "textBoxWavesRecover";
            this.textBoxWavesRecover.TabStop = false;
            // 
            // tabPageFileConvert
            // 
            this.tabPageFileConvert.Controls.Add(this.groupBox44);
            this.tabPageFileConvert.Controls.Add(this.groupBox74);
            this.tabPageFileConvert.Controls.Add(this.groupBox70);
            this.tabPageFileConvert.Controls.Add(this.groupBox51);
            this.tabPageFileConvert.Controls.Add(this.groupBox50);
            this.tabPageFileConvert.Controls.Add(this.groupBox41);
            this.tabPageFileConvert.Controls.Add(this.groupBox40);
            this.tabPageFileConvert.Controls.Add(this.textBoxExtract);
            resources.ApplyResources(this.tabPageFileConvert, "tabPageFileConvert");
            this.tabPageFileConvert.Name = "tabPageFileConvert";
            this.tabPageFileConvert.UseVisualStyleBackColor = true;
            // 
            // groupBox44
            // 
            this.groupBox44.Controls.Add(this.label6);
            this.groupBox44.Controls.Add(this.textBoxMergeTotalBytes);
            this.groupBox44.Controls.Add(this.textBoxMergeFilesInc);
            this.groupBox44.Controls.Add(this.label28);
            this.groupBox44.Controls.Add(this.buttonMergeFiles);
            resources.ApplyResources(this.groupBox44, "groupBox44");
            this.groupBox44.Name = "groupBox44";
            this.groupBox44.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // textBoxMergeTotalBytes
            // 
            resources.ApplyResources(this.textBoxMergeTotalBytes, "textBoxMergeTotalBytes");
            this.textBoxMergeTotalBytes.Name = "textBoxMergeTotalBytes";
            // 
            // textBoxMergeFilesInc
            // 
            resources.ApplyResources(this.textBoxMergeFilesInc, "textBoxMergeFilesInc");
            this.textBoxMergeFilesInc.Name = "textBoxMergeFilesInc";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // buttonMergeFiles
            // 
            resources.ApplyResources(this.buttonMergeFiles, "buttonMergeFiles");
            this.buttonMergeFiles.Name = "buttonMergeFiles";
            this.buttonMergeFiles.UseVisualStyleBackColor = true;
            this.buttonMergeFiles.Click += new System.EventHandler(this.buttonMergeFiles_Click);
            // 
            // groupBox74
            // 
            this.groupBox74.Controls.Add(this.button1PTICPD13toCSV);
            resources.ApplyResources(this.groupBox74, "groupBox74");
            this.groupBox74.Name = "groupBox74";
            this.groupBox74.TabStop = false;
            // 
            // button1PTICPD13toCSV
            // 
            resources.ApplyResources(this.button1PTICPD13toCSV, "button1PTICPD13toCSV");
            this.button1PTICPD13toCSV.Name = "button1PTICPD13toCSV";
            this.button1PTICPD13toCSV.UseVisualStyleBackColor = true;
            this.button1PTICPD13toCSV.Click += new System.EventHandler(this.button1PTICPD13toCSV_Click);
            // 
            // groupBox70
            // 
            this.groupBox70.Controls.Add(this.buttonPD3toCSV);
            resources.ApplyResources(this.groupBox70, "groupBox70");
            this.groupBox70.Name = "groupBox70";
            this.groupBox70.TabStop = false;
            // 
            // buttonPD3toCSV
            // 
            resources.ApplyResources(this.buttonPD3toCSV, "buttonPD3toCSV");
            this.buttonPD3toCSV.Name = "buttonPD3toCSV";
            this.buttonPD3toCSV.UseVisualStyleBackColor = true;
            this.buttonPD3toCSV.Click += new System.EventHandler(this.buttonPD3toCSV_Click);
            // 
            // groupBox51
            // 
            this.groupBox51.Controls.Add(this.buttonExtractVTGbottomnav);
            resources.ApplyResources(this.groupBox51, "groupBox51");
            this.groupBox51.Name = "groupBox51";
            this.groupBox51.TabStop = false;
            // 
            // buttonExtractVTGbottomnav
            // 
            resources.ApplyResources(this.buttonExtractVTGbottomnav, "buttonExtractVTGbottomnav");
            this.buttonExtractVTGbottomnav.Name = "buttonExtractVTGbottomnav";
            this.buttonExtractVTGbottomnav.UseVisualStyleBackColor = true;
            this.buttonExtractVTGbottomnav.Click += new System.EventHandler(this.buttonExtractVTGbottomnav_Click);
            // 
            // groupBox50
            // 
            this.groupBox50.Controls.Add(this.buttonExtractADCP1raw);
            this.groupBox50.Controls.Add(this.buttonExtractADCP0raw);
            resources.ApplyResources(this.groupBox50, "groupBox50");
            this.groupBox50.Name = "groupBox50";
            this.groupBox50.TabStop = false;
            // 
            // buttonExtractADCP1raw
            // 
            resources.ApplyResources(this.buttonExtractADCP1raw, "buttonExtractADCP1raw");
            this.buttonExtractADCP1raw.Name = "buttonExtractADCP1raw";
            this.buttonExtractADCP1raw.UseVisualStyleBackColor = true;
            this.buttonExtractADCP1raw.Click += new System.EventHandler(this.buttonExtractADCP1raw_Click);
            // 
            // buttonExtractADCP0raw
            // 
            resources.ApplyResources(this.buttonExtractADCP0raw, "buttonExtractADCP0raw");
            this.buttonExtractADCP0raw.Name = "buttonExtractADCP0raw";
            this.buttonExtractADCP0raw.UseVisualStyleBackColor = true;
            this.buttonExtractADCP0raw.Click += new System.EventHandler(this.buttonExtractADCP0raw_Click);
            // 
            // groupBox41
            // 
            this.groupBox41.Controls.Add(this.label44);
            this.groupBox41.Controls.Add(this.textBoxExtractMatlabSubSys);
            this.groupBox41.Controls.Add(this.buttonExtractMatlab);
            resources.ApplyResources(this.groupBox41, "groupBox41");
            this.groupBox41.Name = "groupBox41";
            this.groupBox41.TabStop = false;
            // 
            // label44
            // 
            resources.ApplyResources(this.label44, "label44");
            this.label44.Name = "label44";
            // 
            // textBoxExtractMatlabSubSys
            // 
            resources.ApplyResources(this.textBoxExtractMatlabSubSys, "textBoxExtractMatlabSubSys");
            this.textBoxExtractMatlabSubSys.Name = "textBoxExtractMatlabSubSys";
            // 
            // buttonExtractMatlab
            // 
            resources.ApplyResources(this.buttonExtractMatlab, "buttonExtractMatlab");
            this.buttonExtractMatlab.Name = "buttonExtractMatlab";
            this.buttonExtractMatlab.UseVisualStyleBackColor = true;
            this.buttonExtractMatlab.Click += new System.EventHandler(this.buttonExtractMatlab_Click);
            // 
            // groupBox40
            // 
            this.groupBox40.Controls.Add(this.groupBox43);
            this.groupBox40.Controls.Add(this.groupBox42);
            resources.ApplyResources(this.groupBox40, "groupBox40");
            this.groupBox40.Name = "groupBox40";
            this.groupBox40.TabStop = false;
            // 
            // groupBox43
            // 
            this.groupBox43.Controls.Add(this.label45);
            this.groupBox43.Controls.Add(this.textBoxExtractSeriesSubSys);
            this.groupBox43.Controls.Add(this.buttonExtractSeries);
            resources.ApplyResources(this.groupBox43, "groupBox43");
            this.groupBox43.Name = "groupBox43";
            this.groupBox43.TabStop = false;
            // 
            // label45
            // 
            resources.ApplyResources(this.label45, "label45");
            this.label45.Name = "label45";
            // 
            // textBoxExtractSeriesSubSys
            // 
            resources.ApplyResources(this.textBoxExtractSeriesSubSys, "textBoxExtractSeriesSubSys");
            this.textBoxExtractSeriesSubSys.Name = "textBoxExtractSeriesSubSys";
            // 
            // buttonExtractSeries
            // 
            resources.ApplyResources(this.buttonExtractSeries, "buttonExtractSeries");
            this.buttonExtractSeries.Name = "buttonExtractSeries";
            this.buttonExtractSeries.UseVisualStyleBackColor = true;
            this.buttonExtractSeries.Click += new System.EventHandler(this.buttonExtractSeries_Click);
            // 
            // groupBox42
            // 
            this.groupBox42.Controls.Add(this.buttonExtractProfile);
            this.groupBox42.Controls.Add(this.textBoxExtractProfileEnsembleNumber);
            resources.ApplyResources(this.groupBox42, "groupBox42");
            this.groupBox42.Name = "groupBox42";
            this.groupBox42.TabStop = false;
            // 
            // buttonExtractProfile
            // 
            resources.ApplyResources(this.buttonExtractProfile, "buttonExtractProfile");
            this.buttonExtractProfile.Name = "buttonExtractProfile";
            this.buttonExtractProfile.UseVisualStyleBackColor = true;
            this.buttonExtractProfile.Click += new System.EventHandler(this.buttonExtractProfile_Click);
            // 
            // textBoxExtractProfileEnsembleNumber
            // 
            resources.ApplyResources(this.textBoxExtractProfileEnsembleNumber, "textBoxExtractProfileEnsembleNumber");
            this.textBoxExtractProfileEnsembleNumber.Name = "textBoxExtractProfileEnsembleNumber";
            // 
            // textBoxExtract
            // 
            resources.ApplyResources(this.textBoxExtract, "textBoxExtract");
            this.textBoxExtract.Name = "textBoxExtract";
            // 
            // txtUserCommand
            // 
            this.txtUserCommand.AcceptsReturn = true;
            resources.ApplyResources(this.txtUserCommand, "txtUserCommand");
            this.txtUserCommand.CausesValidation = false;
            this.txtUserCommand.Name = "txtUserCommand";
            this.txtUserCommand.TextChanged += new System.EventHandler(this.txtUserCommand_TextChanged);
            // 
            // btnSendCom
            // 
            resources.ApplyResources(this.btnSendCom, "btnSendCom");
            this.btnSendCom.AllowDrop = true;
            this.btnSendCom.Name = "btnSendCom";
            this.btnSendCom.Click += new System.EventHandler(this.btnSendCom_Click);
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuHelp});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuExit});
            resources.ApplyResources(this.mnuFile, "mnuFile");
            // 
            // mnuExit
            // 
            this.mnuExit.Index = 0;
            resources.ApplyResources(this.mnuExit, "mnuExit");
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Index = 1;
            this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAbout});
            resources.ApplyResources(this.mnuHelp, "mnuHelp");
            // 
            // mnuAbout
            // 
            this.mnuAbout.Index = 0;
            resources.ApplyResources(this.mnuAbout, "mnuAbout");
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // comboBox1
            // 
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            resources.GetString("comboBox1.Items"),
            resources.GetString("comboBox1.Items1"),
            resources.GetString("comboBox1.Items2"),
            resources.GetString("comboBox1.Items3"),
            resources.GetString("comboBox1.Items4"),
            resources.GetString("comboBox1.Items5"),
            resources.GetString("comboBox1.Items6")});
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // buttonSendCom1
            // 
            resources.ApplyResources(this.buttonSendCom1, "buttonSendCom1");
            this.buttonSendCom1.AllowDrop = true;
            this.buttonSendCom1.Name = "buttonSendCom1";
            this.buttonSendCom1.Click += new System.EventHandler(this.buttonSendCom1_Click);
            // 
            // checkBoxShowVTGspeed
            // 
            resources.ApplyResources(this.checkBoxShowVTGspeed, "checkBoxShowVTGspeed");
            this.checkBoxShowVTGspeed.Name = "checkBoxShowVTGspeed";
            this.checkBoxShowVTGspeed.UseVisualStyleBackColor = true;
            this.checkBoxShowVTGspeed.CheckedChanged += new System.EventHandler(this.checkBoxShowVTGspeed_CheckedChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label7);
            this.groupBox9.Controls.Add(this.textBoxVTGspeedLimit);
            this.groupBox9.Controls.Add(this.checkBoxShowVTGspeed);
            resources.ApplyResources(this.groupBox9, "groupBox9");
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.TabStop = false;
            // 
            // textBoxVTGspeedLimit
            // 
            resources.ApplyResources(this.textBoxVTGspeedLimit, "textBoxVTGspeedLimit");
            this.textBoxVTGspeedLimit.Name = "textBoxVTGspeedLimit";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.buttonSendCom1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txtUserCommand);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnSendCom);
            this.HelpButton = true;
            this.Menu = this.mnuMain;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_ResizeEnd);
            this.tabControl1.ResumeLayout(false);
            this.tabPageCommunications.ResumeLayout(false);
            this.groupBox69.ResumeLayout(false);
            this.groupBox69.PerformLayout();
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox52.ResumeLayout(false);
            this.groupBox73.ResumeLayout(false);
            this.groupBox73.PerformLayout();
            this.groupBox72.ResumeLayout(false);
            this.groupBox72.PerformLayout();
            this.groupBox71.ResumeLayout(false);
            this.groupBox71.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPageFirmware.ResumeLayout(false);
            this.tabPageFirmware.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPageTerminal.ResumeLayout(false);
            this.tabPageTerminal.PerformLayout();
            this.groupBoxADCPControl.ResumeLayout(false);
            this.groupBoxADCPControl.PerformLayout();
            this.groupBoxFile.ResumeLayout(false);
            this.groupBoxFile.PerformLayout();
            this.tabPageBackScatter.ResumeLayout(false);
            this.tabPageBackScatter.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabPageRiver.ResumeLayout(false);
            this.tabPageRiver.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.tabPageProfilePlot.ResumeLayout(false);
            this.tabPageProfilePlot.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.groupBoxADCPControl2.ResumeLayout(false);
            this.groupBoxPlayback.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfile)).EndInit();
            this.tabPageSeriesPlot.ResumeLayout(false);
            this.groupBox47.ResumeLayout(false);
            this.groupBox47.PerformLayout();
            this.groupBoxPlaybackSeries.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSeries)).EndInit();
            this.tabPageNMEA.ResumeLayout(false);
            this.tabPageNMEA.PerformLayout();
            this.groupBoxPlaybackNMEA.ResumeLayout(false);
            this.tabDownload.ResumeLayout(false);
            this.tabDownload.PerformLayout();
            this.groupBox34.ResumeLayout(false);
            this.groupBox34.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabPageFileConvert.ResumeLayout(false);
            this.tabPageFileConvert.PerformLayout();
            this.groupBox44.ResumeLayout(false);
            this.groupBox44.PerformLayout();
            this.groupBox74.ResumeLayout(false);
            this.groupBox70.ResumeLayout(false);
            this.groupBox51.ResumeLayout(false);
            this.groupBox50.ResumeLayout(false);
            this.groupBox41.ResumeLayout(false);
            this.groupBox41.PerformLayout();
            this.groupBox40.ResumeLayout(false);
            this.groupBox43.ResumeLayout(false);
            this.groupBox43.PerformLayout();
            this.groupBox42.ResumeLayout(false);
            this.groupBox42.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageCommunications;
        private System.Windows.Forms.TabPage tabPageTerminal;
        private System.Windows.Forms.TabPage tabPageProfilePlot;
        private System.Windows.Forms.TabPage tabPageFileConvert;
        private System.Windows.Forms.ListBox listBoxMainPortBaud;
        //private System.IO.Ports.SerialPort _serialPort;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtUserCommand;
        private System.Windows.Forms.Button btnSendCom;
        private System.Windows.Forms.ListBox listBoxAvailableMainPorts;
        private System.Windows.Forms.Button btnScanForMainPorts;
        private System.Windows.Forms.TextBox txtMainPort;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.TextBox textBoxCaptureStatus;
        private System.Windows.Forms.TextBox textBoxProfile;
        private System.Windows.Forms.Button buttonCoordinate;
        private System.Windows.Forms.Button buttonExtractProfile;
        private System.Windows.Forms.Button buttonFileUpload;
        private System.Windows.Forms.Button buttonXModemDownload;
        private System.Windows.Forms.Button buttonXmodemCancel;
        private System.Windows.Forms.GroupBox groupBoxFile;
        private System.Windows.Forms.TabPage tabPageNMEA;
        private System.Windows.Forms.TextBox textBoxCapturedNMEA;
        private System.Windows.Forms.TextBox textBoxNavigation;
        private System.Windows.Forms.TextBox textBoxDecoded;
        private System.Windows.Forms.MainMenu mnuMain;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem mnuHelp;
        private System.Windows.Forms.MenuItem mnuAbout;
        private System.Windows.Forms.Button buttonStatisticsClear;
        private System.Windows.Forms.PictureBox pictureBoxProfile;
        private System.Windows.Forms.Button buttonPlusProfileScale;
        private System.Windows.Forms.Button buttonMinusProfileScale;
        private System.Windows.Forms.TabPage tabPageSeriesPlot;
        private System.Windows.Forms.PictureBox pictureBoxSeries;
        private System.Windows.Forms.Button buttonSeriesPlus;
        private System.Windows.Forms.Button buttonSeriesMinus;
        private System.Windows.Forms.Button buttonClearSeries;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.RadioButton radioButtonSeriesProfileCor;
        private System.Windows.Forms.RadioButton radioButtonSeriesProfileVel;
        private System.Windows.Forms.RadioButton radioButtonSeriesProfileAmp;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.RadioButton radioButtonSeriesWT;
        private System.Windows.Forms.RadioButton radioButtonSeriesProfile;
        private System.Windows.Forms.RadioButton radioButtonSeriesBT;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.RadioButton radioButtonSeriesBTcor;
        private System.Windows.Forms.RadioButton radioButtonSeriesBTvel;
        private System.Windows.Forms.RadioButton radioButtonSeriesBTamp;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.RadioButton radioButtonSeriesWTcor;
        private System.Windows.Forms.RadioButton radioButtonSeriesWTvel;
        private System.Windows.Forms.RadioButton radioButtonSeriesWTamp;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.RadioButton radioButtonSeriesCoordENU;
        private System.Windows.Forms.RadioButton radioButtonSeriesCoordBeam;
        private System.Windows.Forms.RadioButton radioButtonSeriesCoordXYZ;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSeriesBin;
        private System.Windows.Forms.Button buttonSeriesBinPlus;
        private System.Windows.Forms.Button buttonSeriesBinMinus;
        private System.Windows.Forms.RadioButton radioButtonSeriesBTrange;
        private System.Windows.Forms.RadioButton radioButtonSeriesBTsnr;
        private System.Windows.Forms.RadioButton radioButtonSeriesAncillaryProfile;
        private System.Windows.Forms.RadioButton radioButtonSeriesAncillaryBT;
        private System.Windows.Forms.Button buttonFileErase;
        private System.Windows.Forms.TextBox textBoxExtract;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button buttonSendCom1;
        private System.Windows.Forms.Button buttonBinMinus;
        private System.Windows.Forms.Button buttonBinPlus;
        private System.Windows.Forms.TextBox textBoxFirstBin;
        private System.Windows.Forms.Button buttonExtractMatlab;
        private System.Windows.Forms.TabPage tabDownload;
        private System.Windows.Forms.TextBox textBoxWavesRecover;
        private System.Windows.Forms.CheckBox checkBoxProfStatPeaks;
        private System.Windows.Forms.TextBox textBoxWavesRecoverDownload;
        private System.Windows.Forms.GroupBox groupBox34;
        private System.Windows.Forms.ToolTip toolTipWavesWaterDepth;
        private System.Windows.Forms.ToolTip toolTipWavesPressureHeight;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxEMACA;
        private System.Windows.Forms.TextBox textBoxEMACD;
        private System.Windows.Forms.TextBox textBoxEMACB;
        private System.Windows.Forms.TextBox textBoxEMACC;
        private System.Windows.Forms.RadioButton radioButtonProfileDownLoad;
        private System.Windows.Forms.RadioButton radioButtonBurstDownLoad;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton radioButtonAllDownLoad;
        private System.Windows.Forms.GroupBox groupBoxPlayback;
        private System.Windows.Forms.Button buttonPlaybackStepBack;
        private System.Windows.Forms.Button buttonPLAYBACK;
        private System.Windows.Forms.Button buttonPlaybackPause;
        private System.Windows.Forms.Button buttonPlaybackStep;
        private System.Windows.Forms.Button buttonPlayBackGo;
        private System.Windows.Forms.GroupBox groupBoxADCPControl2;
        private System.Windows.Forms.Button buttonSTOP;
        private System.Windows.Forms.Button buttonSTART;
        private System.Windows.Forms.Button buttonBreak;
        private System.Windows.Forms.Button buttonSleep;
        private System.Windows.Forms.GroupBox groupBoxADCPControl;
        private System.Windows.Forms.Button buttonTerminalSTOP;
        private System.Windows.Forms.Button buttonTerminalSTART;
        private System.Windows.Forms.Button buttonTerminalSetTime;
        private System.Windows.Forms.Button buttonTerminalBREAK;
        private System.Windows.Forms.Button buttonTerminalSLEEP;
        private System.Windows.Forms.TextBox textBoxBTNavBin;
        private System.Windows.Forms.Button buttonBTnavBinMinus;
        private System.Windows.Forms.Button buttonBTnavBinPlus;
        private System.Windows.Forms.Button buttonPlaybackStop;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.RadioButton radioButtonProfileDisplayText;
        private System.Windows.Forms.RadioButton radioButtonProfileDisplayGraph;
        private System.Windows.Forms.RadioButton radioButtonProfileDisplayBottomTrack;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.RadioButton radioButtonStatisticsSD;
        private System.Windows.Forms.RadioButton radioButtonStatisticsAVG;
        private System.Windows.Forms.RadioButton radioButtonStatisticsNone;
        private System.Windows.Forms.GroupBox groupBoxPlaybackSeries;
        private System.Windows.Forms.Button buttonPlaybackStopSeries;
        private System.Windows.Forms.Button buttonPlaybackStepBackSeries;
        private System.Windows.Forms.Button buttonPlaybackSeries;
        private System.Windows.Forms.Button buttonPlaybackPauseSeries;
        private System.Windows.Forms.Button buttonPlaybackStepSeries;
        private System.Windows.Forms.Button buttonPlaybackGoSeries;
        private System.Windows.Forms.GroupBox groupBox41;
        private System.Windows.Forms.GroupBox groupBox40;
        private System.Windows.Forms.TextBox textBoxExtractProfileEnsembleNumber;
        private System.Windows.Forms.GroupBox groupBox42;
        private System.Windows.Forms.GroupBox groupBox43;
        private System.Windows.Forms.Button buttonExtractSeries;
        private System.Windows.Forms.GroupBox groupBoxPlaybackNMEA;
        private System.Windows.Forms.Button buttonPlaybackStopNMEA;
        private System.Windows.Forms.Button buttonPlaybackStepBackNMEA;
        private System.Windows.Forms.Button buttonPlaybackNMEA;
        private System.Windows.Forms.Button buttonPlaybackPauseNMEA;
        private System.Windows.Forms.Button buttonPlaybackStepNMEA;
        private System.Windows.Forms.Button buttonPlaybackGoNMEA;
        private System.Windows.Forms.TextBox textBoxEnsembleSub;
        private System.Windows.Forms.Button buttonEnsembleSubMinus;
        private System.Windows.Forms.Button buttonEnsembleSubPlus;
        private System.Windows.Forms.GroupBox groupBox47;
        private System.Windows.Forms.Button buttonSeriesSubPlus;
        private System.Windows.Forms.Button buttonSeriesSubMinus;
        private System.Windows.Forms.TextBox textBoxSeriesSub;
        private System.Windows.Forms.RadioButton radioButtonSeriesWPRT;
        private System.Windows.Forms.CheckBox checkBoxNMEA_ASCII_Input;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox textBoxDownloadRetries;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox textBoxExtractMatlabSubSys;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox textBoxExtractSeriesSubSys;
        private System.Windows.Forms.Button buttonCommsSetMainPortBaud;
        private System.Windows.Forms.TextBox textBoxCommsMainPortManBaud;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox textBoxDownloadTries;
        private System.Windows.Forms.TextBox textBoxDownloadSeconds;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox textBoxDownloadBytes;
        private System.Windows.Forms.CheckBox checkBoxBTNAVshowalways;
        private System.Windows.Forms.GroupBox groupBox50;
        private System.Windows.Forms.Button buttonExtractADCP0raw;
        private System.Windows.Forms.GroupBox groupBox51;
        private System.Windows.Forms.Button buttonExtractVTGbottomnav;
        private System.Windows.Forms.Button buttonCommsDiconnectMainPort;
        private System.Windows.Forms.GroupBox groupBox52;
        private System.Windows.Forms.Button buttonForceBaud;
        private System.Windows.Forms.TextBox textBoxForceBaudTime;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.GroupBox groupBox70;
        private System.Windows.Forms.Button buttonPD3toCSV;
        private System.Windows.Forms.TextBox textBoxDataSize;
        private System.Windows.Forms.RadioButton radioButtonRawDownload;
        private System.Windows.Forms.Button buttonExtractADCP1raw;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.GroupBox groupBox71;
        private System.Windows.Forms.Button buttonCommsSetMainPortParity;
        private System.Windows.Forms.TextBox textBoxCommsMainPortManParity;
        private System.Windows.Forms.ListBox listBoxMainPortParity;
        private System.Windows.Forms.GroupBox groupBox73;
        private System.Windows.Forms.Button buttonCommsSetMainPortStopbits;
        private System.Windows.Forms.TextBox textBoxCommsMainPortManStopBits;
        private System.Windows.Forms.ListBox listBoxMainPortStopBits;
        private System.Windows.Forms.GroupBox groupBox72;
        private System.Windows.Forms.Button buttonCommsSetMainPortBits;
        private System.Windows.Forms.TextBox textBoxCommsMainPortManBits;
        private System.Windows.Forms.ListBox listBoxMainPortBits;
        private System.Windows.Forms.RadioButton radioButtonSeriesBTmag;
        private System.Windows.Forms.GroupBox groupBox74;
        private System.Windows.Forms.Button button1PTICPD13toCSV;
        private System.Windows.Forms.RadioButton radioButtonTxtDownload;
        private System.Windows.Forms.TextBox textBoxDownloadPercent;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.TextBox textBoxDownloadBPS;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Button buttonRecoverDir;
        private System.Windows.Forms.Button buttonRecoverDownload;
        private System.Windows.Forms.Button buttonRecoverDownloadCancel;
        private System.Windows.Forms.GroupBox groupBox26;
        private System.Windows.Forms.RadioButton radioButtonBinary;
        private System.Windows.Forms.RadioButton radioButtonASCII;
        private System.Windows.Forms.Button buttonTerminalDeploy;
        private System.Windows.Forms.TextBox textBoxBTNavBinScale;
        private System.Windows.Forms.CheckBox checkBoxBTNAVuseZ;
        private System.Windows.Forms.CheckBox checkBoxBTNAVRecalc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFirstFile;
        private System.Windows.Forms.GroupBox groupBox69;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Button buttonUDP;
        private System.Windows.Forms.TextBox textBoxUDPstate;
        private System.Windows.Forms.TextBox textBoxUDPport;
        private System.Windows.Forms.GroupBox groupBox44;
        private System.Windows.Forms.TextBox textBoxMergeFilesInc;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button buttonMergeFiles;
        private System.Windows.Forms.TextBox textBoxCurrentCommand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFileSDcard;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPageRiver;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button buttonRiverStop;
        private System.Windows.Forms.Button buttonRiverMinus;
        private System.Windows.Forms.Button buttonRiverPlayback;
        private System.Windows.Forms.Button buttonRiverPause;
        private System.Windows.Forms.Button buttonRiverPlus;
        private System.Windows.Forms.Button buttonRiverGo;
        private System.Windows.Forms.TextBox textBoxRiverNMEA;
        private System.Windows.Forms.TextBox textBoxRiverBT;
        private System.Windows.Forms.TextBox textBoxRiverBT2;
        private System.Windows.Forms.TabPage tabPageFirmware;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label labelFirmwareUpdate;
        private System.Windows.Forms.Button buttonFirmwareCurrentVersion;
        private System.Windows.Forms.Button buttonFirmwareUpdate;
        private System.Windows.Forms.TextBox textBoxFirmware;
        private System.Windows.Forms.TextBox textBoxRiverBeam;
        private System.Windows.Forms.TabPage tabPageBackScatter;
        private System.Windows.Forms.TextBox textBoxBSleaders;
        private System.Windows.Forms.TextBox textBoxBSdata;
        private System.Windows.Forms.TextBox textBoxBSsystem;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton radioButtonBSprofile;
        private System.Windows.Forms.RadioButton radioButtonBSleaders;
        private System.Windows.Forms.RadioButton radioButtonBSdata;
        private System.Windows.Forms.RadioButton radioButtonBSsystem;
        private System.Windows.Forms.TextBox textBoxBSprofile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxBSbeam;
        private System.Windows.Forms.TabPage tabPageSystem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxMergeTotalBytes;
        private System.Windows.Forms.CheckBox checkBoxShowVTGspeed;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxVTGspeedLimit;
    }
}

