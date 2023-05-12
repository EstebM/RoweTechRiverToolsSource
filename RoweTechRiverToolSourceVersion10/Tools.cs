using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
//using System.Numeric;
using System.Globalization;
using System.Net.Sockets;
using RoweTechBackScatter;
using System.Management;
using Microsoft.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;



#pragma warning disable IDE1006
#pragma warning disable IDE0051
#pragma warning disable IDE0054
#pragma warning disable IDE0044
#pragma warning disable IDE0018

namespace RTI_Tools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const int DefaultReadInterval = 200;
        private bool ClearTextScreen = true;

        private int MaxDataBuff = 1400000 - 1;
        private byte[] aBuff = new byte[1400000];
        //private byte[] aBuff2 = new byte[65536];
        private byte[] bBuff = new byte[1400000];
        //private byte[] bBuff2 = new byte[65536];
        //private byte[] cBuff = new byte[1400000];
        //private byte[] cBuff2 = new byte[1400000];
        //private byte[] nBuff =    new byte[1400000];
        private byte[] DataBuff = new byte[1400000];
        private int NmeaBuffReadIndex = 0;
        //private int CompassBuffReadIndex = 0;
        private int DataBuffReadIndexA = 0;
        private int DataBuffReadIndexC = 0;
        private int DataBuffReadIndex = 0;
        private int DataBuffWriteIndex = 0;

        private bool PlaybackPaused = false;
        private bool PlaybackStep = false;
        private bool PlaybackStepBack = false;
        private bool PlaybackEnabled = false;

        private const int ProfENU = 0;
        private const int ProfXYZ = 1;
        private const int ProfBeam = 2;
        private int ProfileCoordinateState = ProfENU;//ProfBeam;

        private const int SeriesENU = 0;
        private const int SeriesXYZ = 1;
        private const int SeriesBeam = 2;
        private int SeriesCoordinateState = SeriesENU;//ProfBeam;

        //static SerialPort _serialPortListen = new SerialPort();

        static SerialPort _serialPort = new SerialPort();

        static System.Windows.Forms.Timer tmrTerminal = new System.Windows.Forms.Timer();
        static System.Windows.Forms.Timer tmrTime = new System.Windows.Forms.Timer();
        static System.Windows.Forms.Timer tmrText = new System.Windows.Forms.Timer();

        private const int MaxFileBytes = 10000000;
        private long FileBytes = 0;
        private bool CaptureData = false;
        private long CapturedBytes = 0;
        
        private string CaptureFileName;
        //private string ExSTR = "";
        

        private byte[] PacketBuff = new byte[1000000];
        private int PacketSize = 0;

        private int DecodeState = 0;
        private const int GotHeader = 1;

        const int HDRLEN = 32;

        //long EnsembleNumber = 0;
        int EnsembleSize = 0;
        int [] LastEnsembleSize = new int[3];
        long EnsembleCheckSum = 0;
        int TempReadIndex = 0;

        //int nArray = 0;
        const int MaxArray = 21;
        const int cbins = 300;
        const int cbeams = 4;
        const int csubs = 12;

        int[] subbeams = new int[16];

        public class ArrayClass
        {
            public int[] Type = new int[MaxArray];
            public int[] Bins = new int[MaxArray];
            public int[] Beams = new int[MaxArray];
            public int[] Imag = new int[MaxArray];
            public int[] NameLen = new int[MaxArray];
            public string[] Name = new string[MaxArray];

            public bool VelocityAvailable;
            public bool InstrumentAvailable;
            public bool EarthAvailable;
            public bool AmplitudeAvailable;
            public bool CorrelationAvailable;

            public bool BeamNAvailable;
            public bool XfrmNAvailable;

            public bool EnsembleDataAvailable;
            public bool AncillaryAvailable;

            public bool EngProfileDataAvailable;

            public bool EngBottomTrackDataAvailable;
            public bool SystemSetupDataAvailable;
            public bool RTonWPAvailable;

            public bool BottomTrackAvailable;
            public bool NmeaAvailable;

            public bool GageAvailable;

            public float[,] Velocity = new float[cbeams, cbins];
            public float[,] Instrument = new float[cbeams, cbins];
            public float[,] Earth = new float[cbeams, cbins];
            public float[,] Amplitude = new float[cbeams, cbins];
            public float[,] Correlation = new float[cbeams, cbins];

            public int[ ,] BeamN = new int[cbeams,  cbins];
            public int[ ,] XfrmN = new int[cbeams, cbins];
            //EnsembleData
            public long E_EnsembleNumber;
            public long E_Cells;
            public long E_Beams;
            public long E_PingsInEnsemble;
            public long E_PingCount;
            public long E_Status;
            public long E_Year;
            public long E_Month;
            public long E_Day;
            public long E_Hour;
            public long E_Minute;
            public long E_Second;
            public long E_Hsec;
            public byte[] E_SN_Buffer = new byte[33];
            public byte[] E_FW_Vers = new byte[4];
            public long E_CurrentSystem;
            public long E_Status2;
            public long E_BurstIndex;
            //Ancillary
            public float A_FirstCellDepth;
            public float A_CellSize;
            public float A_FirstPingSeconds;
            public float A_LastPingSeconds;

            public float A_Heading;
            public float A_Pitch;
            public float A_Roll;
            public float A_WaterTemperature;
            public float A_BoardTemperature;
            public float A_Salinity;
            public float A_Pressure;
            public float A_Depth;
            public float A_SpeedOfSound;
            public float A_Mx;
            public float A_My;
            public float A_Mz;
            public float A_Gp;
            public float A_Gr;
            public float A_Gz;
            public float A_HS1Temperature;
            public float A_HS2Temperature;
            public float A_RCV1Temperature;
            public float A_RCV2Temperature;
            public float A_VINF;
            public float A_VG;
            public float A_VT;
            public float A_VTL;
            public float A_D3V3;
            public float A_SPARE;


            //WPBT

            //Bottom Track
            public float B_FirstPingSeconds;
            public float B_LastPingSeconds;

            public float B_Heading;
            public float B_Pitch;
            public float B_Roll;
            public float B_WaterTemperature;
            public float B_BoardTemperature;
            public float B_HS1Temperature;
            public float B_HS2Temperature;
            public float B_RCV1Temperature;
            public float B_RCV2Temperature;
            public float B_VINF;
            public float B_VG;
            public float B_VT;
            public float B_VTL;
            public float B_D3V3;
            public float B_SPARE;

            public float B_Salinity;
            public float B_Pressure;
            public float B_Depth;
            public float B_SpeedOfSound;

            public float B_Status;//12
            public float B_Beams;//13
            public float B_PingCount;//14

            public float[] B_Range = new float[4];//15-18
            public float[] B_SNR = new float[4];//19-21
            public float[] B_Amplitude = new float[4];//23-26
            public float[] B_Correlation = new float[4];//27-30
            public float[] B_Velocity = new float[4];//31-34
            public float[] B_BeamN = new float[4];//35-38

            public float[] B_Instrument = new float[4];//39-42
            public float[] B_XfrmN = new float[4];//43-46
            public float[] B_Earth = new float[4];//47-50
            public float[] B_EarthN = new float[4];//51-54

            public float[] B_SNRs = new float[4];//55-58
            public float[] B_AmpS = new float[4];//59-62
            public float[] B_VelS = new float[4];//63-66
            public float[] B_NoiseS = new float[4];//67-70
            public float[] B_CorS = new float[4];//71-74

            public float B_SounderRange;//75
            public float B_SounderSNR;//76
            public float B_SounderAmp;//77

            public byte[] NMEA_Buffer = new byte[8192];
            public int NMEA_Bytes = 0;

            public float[] Eng_ProfVel = new float[4];
            public float[] Eng_ProfCor = new float[4];
            public float[] Eng_ProfAmp = new float[4];
            public float Eng_SamplesPerSecond;
            public float Eng_SystemFrequency;
            public float Eng_LagSamples;
            public float Eng_CPCE;
            public float Eng_NCE;
            public float Eng_RepeatN;

            public float Eng_sGap;
            public float Eng_sCPCE;
            public float Eng_sNCE;
            public float Eng_sLagSamples;
            public float Eng_sRepeatN;
            public float Eng_RcvGain;

            public float EngBT_SamplesPerSecond;
            public float EngBT_SystemFrequency;

            public float EngBT_CPCE;
            public float EngBT_NCE;
            public float EngBT_RepeatN;
            public float EngBT_Spare;

            public float[] Eng_SLag = new float[4];
            public float[] Eng_SNoise = new float[4];
            public float[] Eng_SSNR = new float[4];
            public float[] Eng_SCor = new float[4];
            public float[] Eng_SAmp = new float[4];
            public float[] Eng_SVel = new float[4];
            public float[] Eng_SHz = new float[4];

            public float[] Eng_M1Lag = new float[4];
            public float[] Eng_M1Noise = new float[4];
            public float[] Eng_M1SNR = new float[4];
            public float[] Eng_M1Cor = new float[4];
            public float[] Eng_M1Amp = new float[4];
            public float[] Eng_M1Vel = new float[4];
            public float[] Eng_M1Hz = new float[4];

            public float[] Eng_M2Lag = new float[4];
            public float[] Eng_M2Noise = new float[4];
            public float[] Eng_M2SNR = new float[4];
            public float[] Eng_M2Cor = new float[4];
            public float[] Eng_M2Amp = new float[4];
            public float[] Eng_M2Vel = new float[4];
            public float[] Eng_M2Hz = new float[4];

            public float[] Eng_LLLag = new float[4];
            public float[] Eng_LLNoise = new float[4];
            public float[] Eng_LLSNR = new float[4];
            public float[] Eng_LLCor = new float[4];
            public float[] Eng_LLAmp = new float[4];
            public float[] Eng_LLVel = new float[4];
            public float[] Eng_LLHz = new float[4];

            public float[] EngBT_LagUsed = new float[4];
            public float[] Eng_SLSampleDepth = new float[4];
            public float[] Eng_M1SampleDepth = new float[4];
            public float[] Eng_M2SampleDepth = new float[4];
            public float[] Eng_LLSampleDepth = new float[4];

            public float[] Eng_AmbHz = new float[4];
            public float[] Eng_AmbVel = new float[4];
            public float[] Eng_AmbAmp = new float[4];
            public float[] Eng_AmbCor = new float[4];
            public float[] Eng_AmbSNR = new float[4];
            public float[] Eng_AmbLagSamp = new float[4];

            public float SystemSetup_BTSamplesPerSecond;
            public float SystemSetup_BTSystemFreqHz;
            public float SystemSetup_BTCPCE;
            public float SystemSetup_BTNCE;
            public float SystemSetup_BTRepeatN;

            public float SystemSetup_WPSamplesPerSecond;
            public float SystemSetup_WPSystemFreqHz;
            public float SystemSetup_WPCPCE;
            public float SystemSetup_WPLagSamples;
            public float SystemSetup_WPNCE;
            public float SystemSetup_WPRepeatN;
            public float SystemSetup_InputVoltage;
            public float SystemSetup_TransmitVoltage;

            public float SystemSetup_BTBB;
            public float SystemSetup_BTLL;
            public float SystemSetup_BTNB;
            public float SystemSetup_BTMUX;
            public float SystemSetup_WPBB;
            public float SystemSetup_WPLL;
            public float SystemSetup_WPXMTBW;
            public float SystemSetup_WPRCVBW;
            public float SystemSetup_TransmitVoltageMinus;
            public float SystemSetup_WPMUX;
            public float SystemSetup_BTscale;
            public float SystemSetup_WPscale;


            public float RTonWP_Beams;
            public float[] RTonWP_SNR   = new float[4];
            public float[] RTonWP_Range = new float[4];
            public float[] RTonWP_Pings = new float[4];
            public float[] RTonWP_Amp = new float[4];
            public float[] RTonWP_Cor = new float[4];
            public float[] RTonWP_Vel = new float[4];
            public float[] RTonWP_Ins = new float[4];
            public float[] RTonWP_Earth = new float[4];

            public float RGH_Status;//1
            public float RGH_AvgRange;//2
            public float RGH_sd;//3
            public float RGH_AvgSN;//4
            public float RGH_n;//5
            public float RGH_Salinity;//6
            public float RGH_Pressure;//7
            public float RGH_Depth;//8
            public float RGH_WaterTemperature;//9
            public float RGH_BackPlaneTemperature;//10
            public float RGH_SOS;//11
            public float RGH_Heading;//12
            public float RGH_Pitch;//13
            public float RGH_Roll;//14
            public float RGH_AvgS;//15
            public float RGH_AvgN1;//16
            public float RGH_AvgN2;//17
            public float RGH_GainFrac;//18

            public float RGH_Pings;//19
            public float RGH_SNthreshold;//20
            public float RGH_GainThres;//21
            public float RGH_StatThres;//22
            public float RGH_XmtCycles;//23
            public float RGH_DepthOffset;//24

        }

        ArrayClass Arr = new ArrayClass();

        public const int BTMAXSUBS = 10;
        public const int BTMAXBEAMS = 10;

        public const int BTHMAXBINS = 10;
        public const int BTHMAXBEAMS = 4;

        public class RiverBTHClass
        {
            public bool Available;

            public float Subs;
            public float[] Beams = new float[BTMAXSUBS];
            public float[] Bins = new float[BTMAXSUBS];
            public float[] BinSize = new float[BTMAXSUBS];

            public float[,,] Amp = new float[BTMAXSUBS,BTHMAXBEAMS,BTHMAXBINS];
            public float[,,] Cor = new float[BTMAXSUBS, BTHMAXBEAMS, BTHMAXBINS];
            public float[,,] Vel = new float[BTMAXSUBS, BTHMAXBEAMS, BTHMAXBINS];
        }
        RiverBTHClass RiverBTH = new RiverBTHClass();

        public class RiverTransectClass
        {
            public bool Available;
            public bool StationAvailable;

            public byte[] StationName = new byte[200];
            public byte[] StationNumber = new byte[100];

            public int LenName;
            public int LenNumber;

            public float TransectState;
            public float TransectNumber;
            public float TransectStatus;
            public float BottomStatus;
            public float ProfileStatus;
            public float MovingEnsembles;
            public float MovingBTEnsembles;
            public float MovingWPEnsembles;
            public float CurrentEdge;
            public float[] EdgeType = new float[2];
            public float[] EdgeDistance = new float[2];
            public float[] EdgeEnsembles = new float[2];
            public float[] EdgeStatus = new float[2];

        }
        RiverTransectClass RiverTran = new RiverTransectClass();

        public class RiverBTClass
        {
            public bool Available;
            
            public float Subs;
            public float[] PingCount = new float[BTMAXSUBS];
            public float[] Status = new float[BTMAXSUBS];
            public float[] Beams = new float[BTMAXSUBS];
            public float[] NCE = new float[BTMAXSUBS];
            public float[] RepeatN = new float[BTMAXSUBS];
            public float[] CPCE = new float[BTMAXSUBS];
            public float[] BB = new float[BTMAXSUBS];
            public float[] LL = new float[BTMAXSUBS];
            public float[] BTbeamMux = new float[BTMAXSUBS];
            public float[] NB = new float[BTMAXSUBS];
            public float[] PingSeconds = new float[BTMAXSUBS];
            public float[] Heading = new float[BTMAXSUBS];
            public float[] Pitch = new float[BTMAXSUBS];
            public float[] Roll = new float[BTMAXSUBS];
            public float[] WaterTemperature = new float[BTMAXSUBS];
            public float[] BackPlaneTemperature = new float[BTMAXSUBS];
            public float[] Salinity = new float[BTMAXSUBS];
            public float[] Pressure = new float[BTMAXSUBS];
            public float[] Depth = new float[BTMAXSUBS];
            public float[] SpeedOfSound = new float[BTMAXSUBS];
            public float[] Mx = new float[BTMAXSUBS];
            public float[] My = new float[BTMAXSUBS];
            public float[] Mz = new float[BTMAXSUBS];
            public float[] Gp = new float[BTMAXSUBS];
            public float[] Gr = new float[BTMAXSUBS];
            public float[] Gz = new float[BTMAXSUBS];
            public float[] SamplesPerSecond = new float[BTMAXSUBS];
            public float[] SystemFreqHz = new float[BTMAXSUBS];
            public float[,] Range = new float[BTMAXSUBS, BTMAXBEAMS];
            public float[,] SNR = new float[BTMAXSUBS, BTMAXBEAMS];
            public float[,] Amplitude = new float[BTMAXSUBS, BTMAXBEAMS];
            public float[,] NoiseAmpBackPorch = new float[BTMAXSUBS, BTMAXBEAMS];
            public float[,] NoiseAmpFrontPorch = new float[BTMAXSUBS, BTMAXBEAMS];
            public float[,] Correlation = new float[BTMAXSUBS, BTMAXBEAMS];
            public float[,] Velocity = new float[BTMAXSUBS, BTMAXBEAMS];
            public float[,] BeamN = new float[BTMAXSUBS, BTMAXBEAMS];
            //public float[,] Instrument = new float[BTMAXSUBS, BTMAXBEAMS];
            //public float[,] XfrmN = new float[BTMAXSUBS, BTMAXBEAMS];
            //public float[,] Earth = new float[BTMAXSUBS, BTMAXBEAMS];
            //public float[,] EarthN = new float[BTMAXSUBS, BTMAXBEAMS];
        }
        RiverBTClass RiverBT = new RiverBTClass();
        public class RiverTimeStampClass
        {
            public bool Available;
            public float TimeStampGGA;
            public float TimeStampVTG;
            public float TimeStampHDT;
            public float TimeStampDBT;
        }
        RiverTimeStampClass RiverTS = new RiverTimeStampClass();
        public class RiverNMEAClass
        {
            public bool Available;
            public byte GGAbytes;
            public byte VTGbytes;
            public byte HDTbytes;
            public byte DBTbytes;
            public byte[] GGA = new byte[200];
            public byte[] VTG = new byte[100];
            public byte[] HDT = new byte[100];
            public byte[] DBT = new byte[100];

        }
        RiverNMEAClass RiverNMEA = new RiverNMEAClass();


        public class PD3ArrayClass
        {
            public byte ID;
            public byte Data;
            public float[] BTvel = new float[3];
            public float[] WTvel = new float[3];
            public float[] BTrange = new float[4];
            public float BTavgRange;
            public byte[] Spare = new byte[16];
            public byte Sensor;
            public float Hour;
            public float Minute;
            public float Second;
            public float Heading;
            public float Pitch;
            public float Roll;
            public float Temperature;
            public ushort BIT;
            public ushort Checksum;
        }
        PD3ArrayClass PD3 = new PD3ArrayClass();

        public class ArrayClass2
        {
            public int[] beams = new int[csubs];
            public int[] bins = new int[csubs];
            public float[] firstbin = new float[csubs];
            public float[] binsize = new float[csubs];

            public float[, ,] VelSum = new float[csubs, cbeams, cbins];
            public float[, ,] InsSum = new float[csubs, cbeams, cbins];
            public float[, ,] EarSum = new float[csubs, cbeams, cbins];
            public float[, ,] AmpSum = new float[csubs, cbeams, cbins];
            public float[, ,] CorSum = new float[csubs, cbeams, cbins];

            public float[, ,] AmpMax = new float[csubs, cbeams, cbins];
            public float[, ,] CorMax = new float[csubs, cbeams, cbins];
            public float[, ,] AmpMin = new float[csubs, cbeams, cbins];
            public float[, ,] CorMin = new float[csubs, cbeams, cbins];

            public float[, ,] VelN = new float[csubs, cbeams, cbins];
            public float[, ,] InsN = new float[csubs, cbeams, cbins];
            public float[, ,] EarN = new float[csubs, cbeams, cbins];
            public float[, ,] AmpN = new float[csubs, cbeams, cbins];
            public float[, ,] CorN = new float[csubs, cbeams, cbins];

            public float[, ,] VelDiffSum = new float[csubs, cbeams, cbins];
            public float[, ,] VelDiffSumSqr = new float[csubs, cbeams, cbins];
            public float[, ,] VelDiffN = new float[csubs, cbeams, cbins];

            public float[, ,] VelSumI = new float[csubs, cbeams, cbins];
            public float[, ,] VelSumSqrI = new float[csubs, cbeams, cbins];
            public float[, ,] VelNI = new float[csubs, cbeams, cbins];

            public float[, ,] VelSumE = new float[csubs, cbeams, cbins];
            public float[, ,] VelSumSqrE = new float[csubs, cbeams, cbins];
            public float[, ,] VelNE = new float[csubs, cbeams, cbins];

            public float[,] PPVelSum = new float[csubs, cbeams];
            public float[,] PPVelSumSqr = new float[csubs, cbeams];
            public float[,] PPVelN = new float[csubs, cbeams];
            public float[,] PPVelSD = new float[csubs, cbeams];
        }
        ArrayClass2 Arr2 = new ArrayClass2();
        
        //Bitmap bmp1 = new Bitmap(2*2048, 4*768);
        Bitmap bmp2 = new Bitmap(3*800, 2*600);
        Bitmap bmp3 = new Bitmap(3*800, 2*600);

        //NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        #region " Standard Menu Code "

        // This code simply shows the About form.

        private void mnuAbout_Click(object sender, System.EventArgs e)
        {

            // Open the About form in Dialog Mode

            frmAbout frm = new frmAbout();

            frm.ShowDialog(this);

            frm.Dispose();

        }

        // This code will close the form.

        private void mnuExit_Click(object sender, System.EventArgs e)
        {

            // Close the current form

            this.Close();

        }

        #endregion

        string AddSpaces(string str, int n)
        {
            string s = str;

            for (int i = 0; i < n - str.Length; i++)
            {
                s = " " + s;
            }

            return s;
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {   
            try
            {
                PlaybackPaused = false;
                PlaybackEnabled = false;
                string s;
                if (listBoxAvailableMainPorts.SelectedItem != null)
                {
                    s = listBoxAvailableMainPorts.SelectedItem.ToString() + "\r\n";
                    s += listBoxMainPortBaud.SelectedItem.ToString() + "\r\n";
                }
                else
                {
                    s = "\r\n";
                    s += "\r\n";
                }
                s += tabControl1.SelectedIndex.ToString() + "\r\n";                
                s += ProfileGraph + "\r\n";
                s += ShowBottomTrack + "\r\n";
                s += textBoxBTNavBin.Text + "\r\n";                
                s += textBoxFirstBin.Text + "\r\n";

                s += textBoxEMACA.Text + "\r\n";
                s += textBoxEMACB.Text + "\r\n";
                s += textBoxEMACC.Text + "\r\n";
                s += textBoxEMACD.Text + "\r\n";

                //s += radioButtonEthernet.Checked + "\r\n";

                //ADCP Deploy TAB ping control timing
                
                
                //ADCP Deploy TAB predictions
                
                //more uart stuff
                s += listBoxMainPortBits.SelectedItem.ToString() + "\r\n";
                s += listBoxMainPortParity.SelectedItem.ToString() + "\r\n";
                s += listBoxMainPortStopBits.SelectedItem.ToString() + "\r\n";

                s += radioButtonBinary.Checked + "\r\n";
                s += radioButtonASCII.Checked + "\r\n";
                
                
                //here
                s += ProfileCoordinateState.ToString() + "\r\n";

                s += radioButtonProfileDisplayGraph.Checked + "\r\n";
                s += radioButtonProfileDisplayText.Checked + "\r\n";
                s += radioButtonProfileDisplayBottomTrack.Checked + "\r\n";

                s += radioButtonSeriesProfile.Checked + "\r\n";
                s += radioButtonSeriesBT.Checked + "\r\n";
                s += radioButtonSeriesWT.Checked + "\r\n";
                s += radioButtonSeriesAncillaryProfile.Checked + "\r\n";
                s += radioButtonSeriesAncillaryBT.Checked + "\r\n";
                s += radioButtonSeriesWPRT.Checked + "\r\n";

                s += radioButtonSeriesCoordBeam.Checked + "\r\n";
                s += radioButtonSeriesCoordXYZ.Checked + "\r\n";
                s += radioButtonSeriesCoordENU.Checked + "\r\n";

                s += radioButtonSeriesProfileVel.Checked + "\r\n";
                s += radioButtonSeriesProfileAmp.Checked + "\r\n";
                s += radioButtonSeriesProfileCor.Checked + "\r\n";

                s += textBoxSeriesBin.Text + "\r\n";

                s += radioButtonSeriesBTvel.Checked + "\r\n";
                s += radioButtonSeriesBTamp.Checked + "\r\n";
                s += radioButtonSeriesBTcor.Checked + "\r\n";
                s += radioButtonSeriesBTsnr.Checked + "\r\n";
                s += radioButtonSeriesBTrange.Checked + "\r\n";
                s += radioButtonSeriesBTmag.Checked + "\r\n";
                s += radioButtonSeriesWTvel.Checked + "\r\n";
                s += radioButtonSeriesWTamp.Checked + "\r\n";
                s += radioButtonSeriesWTcor.Checked + "\r\n";

                s += VSpanIndex.ToString() + "\r\n"; 
                s += PSpanSeriesIndex.ToString() + "\r\n";
                s += VSpanBTIndex.ToString() + "\r\n";
                s += RSpanBTIndex.ToString() + "\r\n";
                
                s += VSpanSeries.ToString() + "\r\n";
                s += PSpanSeries.ToString() + "\r\n";
                s += VSpanBT.ToString() + "\r\n";
                s += RSpanBT.ToString() + "\r\n";

                //DVL Data TAB
                s += textBoxUDPstate.Text + "\r\n";

                s+= textBoxUDPport.Text + "\r\n";

                s += checkBoxShowVTGspeed.Checked + "\r\n";
                s += textBoxVTGspeedLimit.Text + "\r\n";

                //end here



                //Save setup File
                string DirName = "c:\\RoweTechRiverTools";
                string FilName = "RoweTechRiverTools_Setup14.txt";
                //SaveTextFile(DirName, FilName, s, false);
                SaveTextFile(DirName, FilName, s, true, false);

                if (tmrTerminal.Enabled)
                {
                    tmrTerminal.Enabled = false;
                    //_serialPort.Close();
                    
                }

                if (tmrTime.Enabled)
                {
                    tmrTime.Enabled = false;
                }
                if (tmrText.Enabled)
                {
                    tmrText.Enabled = false;
                }

                if (XMODEM)
                    StopXmodem();

                //if (_serialPort != null)
                    _serialPort?.Close();

            }
            catch {}
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
           
            //nfi.NumberDecimalSeparator = ".";

            textBoxEMACA.Text = "192";
            textBoxEMACB.Text = "168";
            textBoxEMACC.Text = "1";
            textBoxEMACD.Text = "130";

            tmrTerminal.Tick += new System.EventHandler(tmrTerminal_Tick);
            tmrTerminal.Interval = DefaultReadInterval;

            tmrTime.Tick += new System.EventHandler(tmrTime_Tick);
            tmrTime.Interval = 1000;

            tmrText.Tick += new System.EventHandler(tmrText_Tick);
            tmrText.Interval = 100;

            txtMainPort.Text = "No Port Selected";
            
            listBoxMainPortBaud.SelectedIndex = 3;
            listBoxMainPortBits.SelectedIndex = 0;
            listBoxMainPortParity.SelectedIndex = 0;
            listBoxMainPortStopBits.SelectedIndex = 0;
            
            _serialPort.ReadBufferSize = 16 * 65536;
            _serialPort.BaudRate = 115200;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.Handshake = Handshake.None;//_serialPort.Handshake = Handshake.RequestToSend;
            _serialPort.DtrEnable = true;

            //Set the read/write timeouts
            _serialPort.ReadTimeout = 50;
            _serialPort.WriteTimeout = 500;
            
            btnCheckForPorts_Click(sender, e);
            //btnConnect_Click(sender, e);

            pictureBoxProfile.Image = bmp2;
            pictureBoxSeries.Image = bmp3;
            
            ClearSeries();

            //get last the user setup and selected screen
            try
            {
                string FilName = "c:\\RoweTechRiverTools\\RoweTechRiverTools_Setup14.txt";
                string s = ReadTextFile(FilName);
                int i = 0;
                int j = s.IndexOf('\r');
                string sPort = s.Substring(i,j);
                int n = listBoxAvailableMainPorts.FindStringExact(sPort);
                if (n != ListBox.NoMatches)
                {
                    listBoxAvailableMainPorts.SelectedIndex = n;
                    txtMainPort.Text = listBoxAvailableMainPorts.SelectedItem.ToString();
                }
                i = j + 2;
                j = s.IndexOf('\r',i);
                string sBaud = s.Substring(i,j-i);
                n = listBoxMainPortBaud.FindStringExact(sBaud);
                if (n != ListBox.NoMatches)
                    listBoxMainPortBaud.SelectedIndex = n;

                i = j + 2;
                j = s.IndexOf('\r', i);
                string sTab = s.Substring(i, j - i);
                tabControl1.SelectedIndex = Convert.ToInt32(sTab);

                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "False")
                    ProfileGraph = false;
                else
                    ProfileGraph = true;
                
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "False")
                    ShowBottomTrack = false;
                else
                    ShowBottomTrack = true;

                if(ShowBottomTrack)
                    radioButtonProfileDisplayBottomTrack.Checked = true;
                else
                    if(ProfileGraph)
                        radioButtonProfileDisplayGraph.Checked = true;
                    else
                        radioButtonProfileDisplayText.Checked = true;

                i = j + 2;
                j = s.IndexOf('\r', i);
                textBoxBTNavBin.Text = s.Substring(i, j - i);
                
                i = j + 2;
                j = s.IndexOf('\r', i);
                textBoxFirstBin.Text = s.Substring(i, j - i);

                FirstBin = Convert.ToInt32(textBoxFirstBin.Text);

                i = j + 2;
                j = s.IndexOf('\r', i);
                textBoxEMACA.Text = s.Substring(i, j - i);
                i = j + 2;
                j = s.IndexOf('\r', i);
                textBoxEMACB.Text = s.Substring(i, j - i);
                i = j + 2;
                j = s.IndexOf('\r', i);
                textBoxEMACC.Text = s.Substring(i, j - i);
                i = j + 2;
                j = s.IndexOf('\r', i);
                textBoxEMACD.Text = s.Substring(i, j - i);
                
                //more uart stuff
                i = j + 2;
                j = s.IndexOf('\r', i);
                string sBits = s.Substring(i, j - i);
                n = listBoxMainPortBits.FindStringExact(sBits);
                if (n != ListBox.NoMatches)
                    listBoxMainPortBits.SelectedIndex = n;

                i = j + 2;
                j = s.IndexOf('\r', i);
                string sParity = s.Substring(i, j - i);
                n = listBoxMainPortParity.FindStringExact(sParity);
                if (n != ListBox.NoMatches)
                    listBoxMainPortParity.SelectedIndex = n;

                i = j + 2;
                j = s.IndexOf('\r', i);
                string sStopBits = s.Substring(i, j - i);
                n = listBoxMainPortStopBits.FindStringExact(sStopBits);
                if (n != ListBox.NoMatches)
                    listBoxMainPortStopBits.SelectedIndex = n;

                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonBinary.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonASCII.Checked = true;
                }
                
                string ss;
                i = j + 2;
                j = s.IndexOf('\r', i);
                ss = s.Substring(i,j-i);

                ProfileCoordinateState = Convert.ToInt32(ss);

                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonProfileDisplayGraph.Checked = true;
                }
                                
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonProfileDisplayText.Checked = true;
                }
                
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonProfileDisplayBottomTrack.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesProfile.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesBT.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesWT.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesAncillaryProfile.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesAncillaryBT.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesWPRT.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesCoordBeam.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesCoordXYZ.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesCoordENU.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesProfileVel.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesProfileAmp.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesProfileCor.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                textBoxSeriesBin.Text = s.Substring(i, j - i);
                
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesBTvel.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesBTamp.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesBTcor.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesBTsnr.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesBTrange.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesBTmag.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesWTvel.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesWTamp.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    radioButtonSeriesWTcor.Checked = true;
                }

                i = j + 2;
                j = s.IndexOf('\r', i);
                ss = s.Substring(i, j - i);
                VSpanIndex = Convert.ToInt32(ss);

                i = j + 2;
                j = s.IndexOf('\r', i);
                ss = s.Substring(i, j - i);
                PSpanSeriesIndex = Convert.ToInt32(ss);

                
                i = j + 2;
                j = s.IndexOf('\r', i);
                ss = s.Substring(i, j - i);
                VSpanBTIndex = Convert.ToInt32(ss);
                
                i = j + 2;
                j = s.IndexOf('\r', i);
                ss = s.Substring(i, j - i);
                RSpanBTIndex = Convert.ToInt32(ss);

                i = j + 2;
                j = s.IndexOf('\r', i);
                ss = s.Substring(i, j - i);
                VSpanSeries = Convert.ToInt32(ss);
                i = j + 2;
                j = s.IndexOf('\r', i);
                ss = s.Substring(i, j - i);
                PSpanSeries = Convert.ToInt32(ss);
                i = j + 2;
                j = s.IndexOf('\r', i);
                ss = s.Substring(i, j - i);
                VSpanBT = Convert.ToInt32(ss);
                i = j + 2;
                j = s.IndexOf('\r', i);
                ss = s.Substring(i, j - i);
                RSpanBT = Convert.ToInt32(ss);

                //DVL Data TAB
                i = j + 2;
                j = s.IndexOf('\r', i);
                textBoxUDPstate.Text = s.Substring(i, j - i);

                i = j + 2;
                j = s.IndexOf('\r', i);
                textBoxUDPport.Text = s.Substring(i, j - i);

                i = j + 2;
                j = s.IndexOf('\r', i);
                if (s.Substring(i, j - i) == "True")
                {
                    checkBoxShowVTGspeed.Checked = true;
                }
                i = j + 2;
                j = s.IndexOf('\r', i);
                textBoxVTGspeedLimit.Text = s.Substring(i, j - i);

                
                


                UsingSerial = true;
                
                
                //end here
            }
            catch 
            {
                tabControl1.SelectedIndex = 9;//new user show comm ports
            }

            BTnavClr();

            //toolTipWavesWaterDepth.SetToolTip(textBoxDeployWaterDepth, "Water depth (meters) where the ADCP is to be deployed");
            //toolTipWavesPressureHeight.SetToolTip(textBoxDeployPsensHeight, "Pressure sensor height (meters) above the bottom");

            //txtSerial.Height = 540;
            
            btnConnect_Click(sender, e);
            txtUserCommand.Enabled = true;
            btnSendCom.Enabled = true;

            tmrTerminal.Enabled = true;
            tmrTerminal.Start();

            tmrTime.Enabled = true;
            tmrTime.Start();

            tmrText.Enabled = true;
            tmrText.Start();

            textBoxProfile.MouseWheel += new MouseEventHandler(TextBoxProfile_MouseWheel);

            textBoxBSsystem.BringToFront();

            FormLoading = false;

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            

        }

        private void TextBoxProfile_MouseWheel(object sender, MouseEventArgs e)
        {
            //int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;

            //WHEEL_DELTA

            int BinsToMove;// = -e.Delta / 120;

            if (e.Delta > 0)
            {
                BinsToMove = -1;
            }
            else
            {
                BinsToMove = 1;
            }
            
            int Bin = Convert.ToInt32(textBoxFirstBin.Text);

            Bin += BinsToMove;

            textBoxFirstBin.Text = Bin.ToString();

            if (!FormLoading)
                ShowEnsemble(Arr, Arr2);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {

            //tabControl1.Width = buttonSendCom1.Left + buttonSendCom1.Width - 10;
            //tabControl1.Height = buttonSendCom1.Top - tabControl1.Top - 6;
            //textBoxProfile.Width = tabControl1 .Width - textBoxProfile.Left - 10;
            //textBoxProfile.Height = tabControl1.Height - textBoxProfile.Top - 30;

            //pictureBoxProfile.Width = textBoxProfile.Width - 10;
            //pictureBoxProfile.Height = textBoxProfile.Height - pictureBoxProfile.Top - 10;

            //txtUserCommand.Width = buttonSendCom1.Left - txtUserCommand.Left - 6;

            //txtSerial.Width = tabControl1.Width - txtSerial.Left - 10;

            //txtSerial.Height = txtUserCommand.Top - txtSerial.Top - 40;

            //pictureBoxSeries.Width = tabControl1.Width - pictureBoxSeries.Left;

            if (!FormLoading)
            {
                ShowSeries(Arr, Arr2);
                ShowEnsemble(Arr, Arr2);
            }

        }

        string CaptureAppendPacket(byte[] buf, int offset, int bytes, string DirectoryName, string CaptureFileName, Boolean ForceCreateNew)
        {
            Boolean OK = true;            
            DirectoryInfo di = new DirectoryInfo(DirectoryName);

            try
            {
                // Determine whether the directory exists.
                if (di.Exists)
                {
                }
                else
                {
                    // Try to create the directory.
                    di.Create();
                }
                // Delete the directory.
                //di.Delete();
            }
            catch
            {
                OK = false;
                WriteMessageTxtSerial("Can't create directory", true);
            }

            string Path = "Empty File";

            if (OK)
            {
                Path = DirectoryName + "\\" + CaptureFileName;
                FileStream fs;

                if (ForceCreateNew)
                {
                    if (File.Exists(Path))
                    {
                        if (MessageBox.Show("Ok to overwrite " + Path + "?", "Erase File", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        {
                            OK = false;
                        }
                        else
                        {
                            fs = new FileStream(Path, FileMode.Create);
                            fs.Close();
                        }
                    }
                }

                if (OK)
                {
                    try
                    {
                        if (File.Exists(Path))
                        {
                            fs = new FileStream(Path, FileMode.Append);
                        }
                        else
                            fs = new FileStream(Path, FileMode.Create);

                        BinaryWriter w = new BinaryWriter(fs);

                        w.Write(buf, offset, bytes);
                        w.Close();
                        fs.Close();
                    }
                    catch (System.Exception ex)
                    {
                        string exceptionmessage = String.Format("caughtA: {0}", ex.GetType().ToString());
                        WriteMessageTxtSerial(exceptionmessage, true);
                    }
                }
            }
            return Path;
        }

        string SaveTextFile(string DirName, string FilName, string strData, bool ShowError, bool Append)
        {
            DirectoryInfo di = new DirectoryInfo(DirName);
            string str = "";

            try
            {
                if (di.Exists)// Determine whether the directory exists.
                {
                    //di.Delete();// Delete the directory.
                }
                else// Try to create the directory.
                {
                    di.Create();
                }
            }
            catch
            {
                if(ShowError)
                    MessageBox.Show("No Directory. Can't save Text data!");
            }
            finally
            {
                str = DirName + "\\" + FilName;
                try
                {
                    if(Append)                    
                        System.IO.File.AppendAllText(str, strData);
                    else
                        System.IO.File.WriteAllText(str, strData);
                }
                catch (System.Exception ex)
                {
                    if (ShowError)
                        MessageBox.Show("Can't save Text data! " + str + " " + String.Format("exception: {0}", ex.GetType().ToString()));
                }
            }
            return str;
        }
        string ReadTextFile(string FilName)
        {   
            String s = "";
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(FilName);                
                s = sr.ReadToEnd();
                //close the file
                sr.Close();
            }
            catch {}

            return s;
        }

        string GetADCPCommand(string s)//-1 indicates no match
        {
            int offset = s.IndexOf("[");
            if (offset == -1)
            {
                offset = s.IndexOf(" ");
                if (offset == -1)
                {
                    offset = s.IndexOf("0");
                    if (offset == -1)
                    {
                        string str;
                        for (int i = 48; i < 58; i++)
                        {
                            str = "" + Convert.ToChar(i);
                            offset = s.IndexOf(str);
                            if (offset != -1)
                                break;
                        }
                    }
                }
            }
            if (offset == -1)
                offset = s.Length;
            return s.Substring(0, offset);
        }

        void SendADCPCommandNoVerify(string txt, int wait)
        {
            string message1 = txt + '\r';
            //string message3;

            if (UsingSerial)
            {
                tmrTerminal.Enabled = true;
                tmrTerminal.Start();
                if (_serialPort.IsOpen)
                {
                    try
                    {
                        txtSerialStr = "";//.Text = "";
                        _serialPort.Write(message1);
                    }
                    catch //(System.Exception ex)
                    {
                        //message3 = String.Format("Send ADCP Command Error No Verify {0}", ex.GetType().ToString());
                    }
                }
            }
            System.Threading.Thread.Sleep(wait);
            Application.DoEvents();
        }
        bool SendADCPCommand(string txt)
        {   
            string message1 = txt + '\r';
            //string message3;

            if (UsingSerial)
            {
                tmrTerminal.Enabled = true;
                tmrTerminal.Start();
                if (_serialPort.IsOpen)
                {
                    try
                    {
                        txtSerialStr = "";
                        _serialPort.Write(message1);
                    }
                    catch //(System.Exception ex)
                    {
                        //message3 = String.Format("Send ADCP Command Error {0}", ex.GetType().ToString());
                    }
                }
            }
            System.Threading.Thread.Sleep(100);

            DateTime currentDate = DateTime.Now;
            int lastSec = currentDate.Second;

            int ElapsedSec;// = 0;
            
            while (txtSerialStr.Length < txt.Length + 1)
            {
                Application.DoEvents();

                currentDate = DateTime.Now;
                ElapsedSec = currentDate.Second - lastSec;
                if (ElapsedSec < 0)
                    ElapsedSec += 60;
                
                if (ElapsedSec > 10)//10 seconds
                    break;
            }
            Application.DoEvents();

            string s = txtSerialStr;//.Text;                
            if (s.Length > 0)
            {
                try
                {
                    int n = s.IndexOf(txt.ToUpper());
                    byte b = Convert.ToByte(s[n + txt.Length]);
                    if (b == 43)
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }

            }            
            return false;
        }
       

        [StructLayout(LayoutKind.Explicit)]
        struct TestUnion
        {
            [FieldOffset(0)]
            public byte A;
            [FieldOffset(1)]
            public byte B;
            [FieldOffset(2)]
            public byte C;
            [FieldOffset(3)]
            public byte D;
            [FieldOffset(4)]
            public byte E;
            [FieldOffset(5)]
            public byte F;
            [FieldOffset(6)]
            public byte G;
            [FieldOffset(7)]
            public byte H;
            [FieldOffset(0)]
            public float Float;
            [FieldOffset(0)]
            public int Int;
            [FieldOffset(0)]
            public double Double;
        }

        TestUnion ByteArrayToNumber;

        int PacketPointer = 0;
        int ByteArrayToInt(byte[] packet)
        {
            ByteArrayToNumber.A = packet[PacketPointer++];
            ByteArrayToNumber.B = packet[PacketPointer++];
            ByteArrayToNumber.C = packet[PacketPointer++];
            ByteArrayToNumber.D = packet[PacketPointer++];

            return ByteArrayToNumber.Int;
        }
        string ByteArrayToString(byte[] packet, int len)
        {
            string s = "";
            int i;
            for (i = 0; i < len; i++)
            {
                s += (char)packet[PacketPointer++];
            }
            return s;
        }
        float ByteArrayToFloat(byte[] packet)
        {
            ByteArrayToNumber.A = packet[PacketPointer++];
            ByteArrayToNumber.B = packet[PacketPointer++];
            ByteArrayToNumber.C = packet[PacketPointer++];
            ByteArrayToNumber.D = packet[PacketPointer++];

            return ByteArrayToNumber.Float;
        }
        double ByteArrayToDouble(byte[] packet)
        {
            ByteArrayToNumber.A = packet[PacketPointer++];
            ByteArrayToNumber.B = packet[PacketPointer++];
            ByteArrayToNumber.C = packet[PacketPointer++];
            ByteArrayToNumber.D = packet[PacketPointer++];

            ByteArrayToNumber.E = packet[PacketPointer++];
            ByteArrayToNumber.F = packet[PacketPointer++];
            ByteArrayToNumber.G = packet[PacketPointer++];
            ByteArrayToNumber.H = packet[PacketPointer++];

            return ByteArrayToNumber.Double;
        }

        string VelocityID = "E000001\0";
        string InstrumentID = "E000002\0";
        string EarthID = "E000003\0";
        string AmplitudeID = "E000004\0";
        string CorrelationID = "E000005\0";
        string BeamNID = "E000006\0";
        string XfrmNID = "E000007\0";
        string EnsembleDataID = "E000008\0";
        string AncillaryID = "E000009\0";
        string BottomTrackID = "E000010\0";
        string NMEAID = "E000011\0";
        string EngProfileDataID = "E000012\0";
        string EngBottomTrackDataID = "E000013\0";
        string SystemSetupDataID = "E000014\0";
        string RTonWPDataID = "E000015\0";
        string GageID = "E000016\0";
        string RiverBTID = "R000001\0";
        string RiverTimeStampID = "R000002\0";
        string RiverNMEAID = "R000003\0";
        string RiverBThump = "R000004\0";
        string RiverStationID = "R000005\0";
        string RiverTransectID = "R000006\0";
    
        bool GotData = false;

        bool ignorecsum = false;
        

        //BackScatter Ensemble -----------------------------------------------------------------
        private byte[] EnsBuf = new byte[1000000];

        //string SystemStringA = "";
        //string SystemStringB = "";
     
        void DecodeBackScatterEnsemble(int beam, BackScatter.EnsembleClass E)
        {
            BackScatter.DecodeEnsemble(EnsBuf, E);

            textBoxBSsystem.Text = BackScatter.GetSystemString(E);
            textBoxBSdata.Text = BackScatter.GetBsString(E);
            textBoxBSleaders.Text = BackScatter.GetHeaderString(E) + BackScatter.GetHeaderDataTypesString();

            textBoxBSprofile.Text = BackScatter.GetBsProfileString(beam,E); 
        }
        
        bool DecodeData(bool MatlabExtract, bool WavesExtract, bool SeriesExtract)
        {
            int stay = 1;
            int i, j, k;

            bool csumOK = false;

            //if (BackScatter.FindEnsemble(EnsBuf, DataBuff))
            //{
            //    DecodeBackScatterEnsemble(TheBSbeam, BackScatter.Ensemble);

            //    return false;
            //}
            //else//Standard Decode
            {
                while (stay == 1)
                {
                    int ByteCount = DataBuffWriteIndex - DataBuffReadIndex;
                    if (ByteCount < 0)
                        ByteCount += MaxDataBuff;

                    if (ByteCount <= 0)
                        break;

                    switch (DecodeState)
                    {
                        default:
                            while (ByteCount > 0)
                            {
                                if (DataBuff[DataBuffReadIndex] == 0x80)//standard binary
                                    break;
                                DataBuffReadIndex++;
                                if (DataBuffReadIndex > MaxDataBuff)
                                    DataBuffReadIndex = 0;

                                ByteCount--;
                            }
                            //standard binary
                            if (ByteCount > HDRLEN)
                            {
                                TempReadIndex = DataBuffReadIndex;
                                byte[] DBuff = new byte[HDRLEN];
                                for (i = 0; i < HDRLEN; i++)
                                {
                                    DBuff[i] = DataBuff[TempReadIndex];
                                    TempReadIndex++;
                                    if (TempReadIndex > MaxDataBuff)
                                        TempReadIndex = 0;
                                }
                                j = 0;
                                k = 0;
                                for (i = 0; i < 16; i++)
                                {
                                    if (DBuff[i] == 0x80)
                                        j++;
                                    else
                                        k++;
                                }
                                long EnsNum;
                                long NotEnsNum;
                                int EnsSiz;
                                long NotEnsSiz;
                                i = 16;
                                if (j == 16)
                                {
                                    EnsNum = DBuff[i++];
                                    EnsNum += DBuff[i++] << 8;
                                    EnsNum += DBuff[i++] << 16;
                                    EnsNum += DBuff[i++] << 24;

                                    NotEnsNum = DBuff[i++];
                                    NotEnsNum += DBuff[i++] << 8;
                                    NotEnsNum += DBuff[i++] << 16;
                                    NotEnsNum += DBuff[i++] << 24;
                                    NotEnsNum = ~NotEnsNum;

                                    EnsSiz = DBuff[i++];
                                    EnsSiz += DBuff[i++] << 8;
                                    EnsSiz += DBuff[i++] << 16;
                                    EnsSiz += DBuff[i++] << 24;

                                    NotEnsSiz = DBuff[i++];
                                    NotEnsSiz += DBuff[i++] << 8;
                                    NotEnsSiz += DBuff[i++] << 16;
                                    NotEnsSiz += DBuff[i++] << 24;
                                    NotEnsSiz = ~NotEnsSiz;

                                    TempReadIndex = DataBuffReadIndex;
                                    if (EnsNum == NotEnsNum && EnsSiz == NotEnsSiz)
                                    {
                                        for (i = 0; i < HDRLEN; i++)
                                        {
                                            PacketBuff[i] = DBuff[i];
                                        }
                                        TempReadIndex += HDRLEN;
                                        if (TempReadIndex > MaxDataBuff)
                                            TempReadIndex = (TempReadIndex - MaxDataBuff - 1);

                                        DecodeState = GotHeader;


                                        LastEnsembleSize[0] = LastEnsembleSize[1];
                                        LastEnsembleSize[1] = EnsembleSize;
                                        EnsembleSize = EnsSiz;
                                        LastEnsembleSize[2] = EnsembleSize;
                                        //EnsembleNumber = EnsNum;
                                    }
                                    else//point to next byte in the buffer
                                    {
                                        DataBuffReadIndex++;
                                        if (DataBuffReadIndex > MaxDataBuff)
                                            DataBuffReadIndex = 0;
                                    }
                                }
                                else
                                {
                                    if (j == 0)
                                        DataBuffReadIndex += 16;
                                    else
                                        DataBuffReadIndex++;
                                    if (DataBuffReadIndex > MaxDataBuff)
                                        DataBuffReadIndex = (DataBuffReadIndex - MaxDataBuff - 1);
                                    stay = 1;
                                }
                            }
                            else
                            {
                                stay = 0;
                            }
                            break;

                        case GotHeader:
                            if (ByteCount >= HDRLEN + EnsembleSize + 4)
                            {
                                long csum;
                                ushort crc = 0;
                                //CCITT 16 bit algorithm (X^16 + X^12 + X^5 + 1)
                                for (i = HDRLEN; i < EnsembleSize + HDRLEN; i++)
                                {
                                    PacketBuff[i] = DataBuff[TempReadIndex];
                                    crc = (ushort)((byte)(crc >> 8) | (crc << 8));
                                    crc ^= DataBuff[TempReadIndex];
                                    crc ^= (byte)((crc & 0xff) >> 4);
                                    crc ^= (ushort)((crc << 8) << 4);
                                    crc ^= (ushort)(((crc & 0xff) << 4) << 1);

                                    TempReadIndex++;
                                    if (TempReadIndex > MaxDataBuff)
                                        TempReadIndex = 0;
                                }
                                csum = crc;

                                if (ignorecsum)
                                {
                                    for (j = i; j < EnsembleSize + HDRLEN; j++)
                                    {
                                        PacketBuff[j] = DataBuff[TempReadIndex];
                                        TempReadIndex++;
                                        if (TempReadIndex > MaxDataBuff)
                                            TempReadIndex = 0;// (TempReadIndex - MaxDataBuff);
                                    }
                                    EnsembleCheckSum = csum;
                                }
                                else
                                {
                                    //read in one more for the checksum
                                    for (j = i; j < EnsembleSize + HDRLEN + 4; j++)
                                    {
                                        PacketBuff[j] = DataBuff[TempReadIndex];
                                        TempReadIndex++;
                                        if (TempReadIndex > MaxDataBuff)
                                            TempReadIndex = 0;// TempReadIndex - MaxDataBuff;
                                    }
                                    EnsembleCheckSum = PacketBuff[i];
                                    EnsembleCheckSum += PacketBuff[i + 1] << 8;
                                    EnsembleCheckSum += PacketBuff[i + 2] << 16;
                                    EnsembleCheckSum += PacketBuff[i + 3] << 24;
                                }
                                if (csum == EnsembleCheckSum)
                                {
                                    csumOK = true;
                                    GotData = true;
                                    DataBuffReadIndex = TempReadIndex;
                                    PacketSize = HDRLEN + EnsembleSize + 4;
                                    if (MatlabExtract)
                                    {
                                        DecodeEnsemble(PacketBuff, Arr);
                                        strMatlabFile = ExtractMatlab(PacketBuff, Arr);
                                    }
                                    else
                                    {
                                        if (WavesExtract)
                                        {
                                            DecodeEnsemble(PacketBuff, Arr);
                                        }
                                        else
                                        {
                                            if (SeriesExtract)
                                            {
                                                DecodeEnsemble(PacketBuff, Arr);

                                                //DecodeNmea((int)Arr.E_CurrentSystem);

                                                DecodeEnsembleNmea(Arr, (int)Arr.E_CurrentSystem >> 24);
                                                bool fresh = false;
                                                if (FreshGGA[(int)(Arr.E_CurrentSystem >> 24)])
                                                    fresh = true;
                                                DoAllNav(Arr, (int)Arr.E_CurrentSystem >> 24);

                                                int cs = (int)(Arr.E_CurrentSystem >> 24);
                                                if (cs < 0)
                                                    cs = 0;
                                                if (cs > csubs - 1)
                                                    cs = csubs - 1;

                                                float BTperr = 100;
                                                if (GGANAV[cs].DMG > 0)
                                                    BTperr = (float)(100.0 * (BTdisMag[cs] / GGANAV[cs].DMG - 1));

                                                int index = SeriesIndex[cs] - 1;
                                                if (index < 0)
                                                    index = MaxSeries - 1;
                                                
                                                if (NewGGA[cs] && FreshBT[cs])
                                                {
                                                    MagSeriesBT[cs, 5, index] = BTperr;
                                                }
                                                else
                                                {
                                                    MagSeriesBT[cs, 5, index] = 100;
                                                }

                                                if (Arr.NmeaAvailable)
                                                {
                                                    NmeaBuffReadIndex = 0;
                                                    textBoxCapturedNMEA.Clear();
                                                    DecodeNmea((int)Arr.E_CurrentSystem >> 24, Arr.NMEA_Buffer, Arr.NMEA_Bytes, false, true);
                                                }

                                                if (Arr.EnsembleDataAvailable)
                                                    ExtractSeries(Arr, true, fresh, false);
                                            }
                                            else
                                            {
                                                DecodeEnsemble(PacketBuff, Arr);
                                            }
                                        }
                                    }

                                    DecodeState = 0;
                                    stay = 0;
                                }
                                else//checksum failed
                                {
                                    //DataBuffReadIndex = TempReadIndex;//chuck the data
                                    DataBuffReadIndex += 32;//chuck the header
                                    DecodeState = 0;
                                    stay = 1;
                                }
                            }
                            else
                                stay = 0;
                            break;
                    }
                }
            }
            return csumOK;
        }

        bool DecodeNavData()
        {
            int stay = 1;
            int i, j, k;

            bool csumOK = false;

            while (stay == 1)
            {
                int ByteCount = DataBuffWriteIndex - DataBuffReadIndex;
                if (ByteCount < 0)
                    ByteCount += MaxDataBuff;

                if (ByteCount <= 0)
                    break;

                switch (DecodeState)
                {
                    default:
                        while (ByteCount > 0)
                        {
                            if (DataBuff[DataBuffReadIndex] == 0x80)//standard binary
                                break;

                            DataBuffReadIndex++;
                            if (DataBuffReadIndex > MaxDataBuff)
                                DataBuffReadIndex = 0;

                            ByteCount--;
                        }   
                        if (ByteCount > HDRLEN)
                        {
                            TempReadIndex = DataBuffReadIndex;
                            byte[] DBuff = new byte[HDRLEN];
                            for (i = 0; i < HDRLEN; i++)
                            {
                                DBuff[i] = DataBuff[TempReadIndex];
                                TempReadIndex++;
                                if (TempReadIndex > MaxDataBuff)
                                    TempReadIndex = 0;
                            }
                            j = 0;
                            k = 0;
                            for (i = 0; i < 16; i++)
                            {
                                if (DBuff[0] == 0x80)
                                    j++;
                                else
                                    k++;
                            }
                            long EnsNum;
                            long NotEnsNum;
                            int EnsSiz;
                            long NotEnsSiz;
                            i = 16;
                            if (j == 16)
                            {
                                EnsNum = DBuff[i++];
                                EnsNum += DBuff[i++] << 8;
                                EnsNum += DBuff[i++] << 16;
                                EnsNum += DBuff[i++] << 24;

                                NotEnsNum = DBuff[i++];
                                NotEnsNum += DBuff[i++] << 8;
                                NotEnsNum += DBuff[i++] << 16;
                                NotEnsNum += DBuff[i++] << 24;
                                NotEnsNum = ~NotEnsNum;

                                EnsSiz = DBuff[i++];
                                EnsSiz += DBuff[i++] << 8;
                                EnsSiz += DBuff[i++] << 16;
                                EnsSiz += DBuff[i++] << 24;

                                NotEnsSiz = DBuff[i++];
                                NotEnsSiz += DBuff[i++] << 8;
                                NotEnsSiz += DBuff[i++] << 16;
                                NotEnsSiz += DBuff[i++] << 24;
                                NotEnsSiz = ~NotEnsSiz;

                                TempReadIndex = DataBuffReadIndex;
                                if (EnsNum == NotEnsNum && EnsSiz == NotEnsSiz)
                                {
                                    for (i = 0; i < HDRLEN; i++)
                                    {
                                        PacketBuff[i] = DBuff[i];
                                    }
                                    TempReadIndex += HDRLEN;
                                    if (TempReadIndex > MaxDataBuff)
                                        TempReadIndex = (TempReadIndex - MaxDataBuff - 1);

                                    DecodeState = GotHeader;


                                    LastEnsembleSize[0] = LastEnsembleSize[1];
                                    LastEnsembleSize[1] = EnsembleSize;
                                    EnsembleSize = EnsSiz;
                                    LastEnsembleSize[2] = EnsembleSize;
                                    //EnsembleNumber = EnsNum;
                                }
                                else//point to next byte in the buffer
                                {
                                    DataBuffReadIndex++;
                                    if (DataBuffReadIndex > MaxDataBuff)
                                        DataBuffReadIndex = 0;
                                }
                            }
                            else
                            {
                                if (j == 0)
                                    DataBuffReadIndex += 16;
                                else
                                    DataBuffReadIndex++;
                                if (DataBuffReadIndex > MaxDataBuff)
                                    DataBuffReadIndex = (DataBuffReadIndex - MaxDataBuff - 1);
                                stay = 0;
                            }
                        }
                        else
                        {
                            stay = 0;
                        }
                            
                        
                        break;

                    case GotHeader:
                        if (ByteCount >= HDRLEN + EnsembleSize + 4)
                        {
                            long csum;// = 0;
                            ushort crc = 0;
                            //CCITT 16 bit algorithm (X^16 + X^12 + X^5 + 1)
                            for (i = HDRLEN; i < EnsembleSize + HDRLEN; i++)
                            {
                                PacketBuff[i] = DataBuff[TempReadIndex];
                                crc = (ushort)((byte)(crc >> 8) | (crc << 8));
                                crc ^= DataBuff[TempReadIndex];
                                crc ^= (byte)((crc & 0xff) >> 4);
                                crc ^= (ushort)((crc << 8) << 4);
                                crc ^= (ushort)(((crc & 0xff) << 4) << 1);

                                TempReadIndex++;
                                if (TempReadIndex > MaxDataBuff)
                                    TempReadIndex = 0;// TempReadIndex - MaxDataBuff;
                            }
                            csum = crc;
                            //read in one more for the checksum
                            for (j = i; j < EnsembleSize + HDRLEN + 4; j++)
                            {
                                PacketBuff[j] = DataBuff[TempReadIndex];
                                TempReadIndex++;
                                if (TempReadIndex > MaxDataBuff)
                                    TempReadIndex = 0;// TempReadIndex - MaxDataBuff;
                            }
                            EnsembleCheckSum = PacketBuff[i];
                            EnsembleCheckSum += PacketBuff[i + 1] << 8;
                            EnsembleCheckSum += PacketBuff[i + 2] << 16;
                            EnsembleCheckSum += PacketBuff[i + 3] << 24;

                            if (csum == EnsembleCheckSum)
                            {
                                csumOK = true;
                                GotData = true;
                                DataBuffReadIndex = TempReadIndex;
                                PacketSize = HDRLEN + EnsembleSize + 4;
                                
                                DecodeEnsemble(PacketBuff, Arr);
                                if (Arr.EnsembleDataAvailable)
                                    ExtractNavSeries(Arr);
                                DecodeState = 0;
                                stay = 0;
                            }
                            else//checksum failed
                            {
                                DataBuffReadIndex = TempReadIndex;//chuck the data
                                DecodeState = 0;
                                stay = 0;
                            }
                        }
                        else
                            stay = 0;

                        break;
                }
            }
            return csumOK;
        }
        
        bool DecodePD3Data( PD3ArrayClass PD)
        {
            int stay = 1;
            int i;

            bool csumOK = false;

            while (stay == 1)
            {
                int ByteCount = DataBuffWriteIndex - DataBuffReadIndex;
                if (ByteCount < 0)
                    ByteCount += MaxDataBuff;
               
                        while (ByteCount > 0)
                        {
                            if (DataBuff[DataBuffReadIndex] == 0x7E)//PD3 ID
                                break;

                            DataBuffReadIndex++;
                            if (DataBuffReadIndex > MaxDataBuff)
                                DataBuffReadIndex = 0;

                            ByteCount--;
                        }
                        if (ByteCount > 56)//PD3 length = 57
                        {
                            TempReadIndex = DataBuffReadIndex;
                            byte[] DBuff = new byte[57];
                            long sum = 0;
                            for (i = 0; i < 55; i++)
                            {
                                DBuff[i] = DataBuff[TempReadIndex];
                                sum += DBuff[i];
                                TempReadIndex++;
                                if (TempReadIndex > MaxDataBuff)
                                    TempReadIndex = 0;
                            }

                            ushort csum = DataBuff[TempReadIndex];
                            TempReadIndex++;
                            if (TempReadIndex > MaxDataBuff)
                                TempReadIndex = 0;

                            csum += (ushort)(DataBuff[TempReadIndex] << 8);
                            TempReadIndex++;
                            if (TempReadIndex > MaxDataBuff)
                                TempReadIndex = 0;

                            if (sum == csum)
                            {
                                csumOK = true;
                                DataBuffReadIndex = TempReadIndex;
                                stay = 0;
                                int n = 0;
                                PD.ID = DBuff[n++];
                                PD.Data = DBuff[n++];
                                float f;
                                for (i = 0; i < 3; i++)
                                {
                                    f = DBuff[n++];
                                    f += (float)(DBuff[n++] << 8);
                                    if (f > 32767)
                                        f -= 65536;
                                    PD.BTvel[i] = f / 1000;
                                }
                                for (i = 0; i < 3; i++)
                                {
                                    f = DBuff[n++];
                                    f += (float)(DBuff[n++] << 8);
                                    if (f > 32767)
                                        f -= 65536;
                                    PD.WTvel[i] = f / 1000;
                                }
                                for (i = 0; i < 4; i++)
                                {
                                    f = DBuff[n++];
                                    f += (float)(DBuff[n++] << 8);
                                    PD.BTrange[i] = f / 100;
                                }
                                f = DBuff[n++];
                                f += (float)(DBuff[n++] << 8);
                                PD.BTavgRange = f / 100;
                                for (i = 0; i < 16; i++)
                                    PD.Spare[i] = DBuff[n++];
                                PD.Sensor = DBuff[n++];
                                
                                PD.Hour = DBuff[n++];
                                PD.Minute = DBuff[n++];
                                PD.Second = DBuff[n++];
                                PD.Second += (float)DBuff[n++] / 100;

                                PD.Heading = DBuff[n++];
                                PD.Heading += DBuff[n++] << 8;
                                PD.Heading /= 100;
                                PD.Pitch = DBuff[n++];
                                PD.Pitch += DBuff[n++] << 8;
                                if (PD.Pitch > 32767)
                                    PD.Pitch -= 65536;
                                PD.Pitch /= 100;
                                PD.Roll = DBuff[n++];
                                PD.Roll += DBuff[n++] << 8;
                                if (PD.Roll > 32767)
                                    PD.Roll -= 65536;
                                PD.Roll /= 100;
                                PD.Temperature = DBuff[n++];
                                PD.Temperature += DBuff[n++] << 8;
                                if (PD.Temperature > 32767)
                                    PD.Temperature -= 65536;
                                PD.Temperature /= 100;
                                PD.BIT = DBuff[n++];
                                PD.BIT += (ushort)(DBuff[n++] << 8);
                                PD.Checksum = DBuff[n++];
                                PD.Checksum += (ushort)(DBuff[n++] << 8);
                            }
                            else
                            {
                                DataBuffReadIndex++;
                                if (DataBuffReadIndex > MaxDataBuff)
                                    DataBuffReadIndex = 0;
                            }
                        }
                        else
                        {
                            stay = 0;
                        }
                /*
                 * 
                   char ID;
                   public float[] BTvel = new float[3];
                   public float[] WTvel = new float[3];
                   public float[] BTrange = new float[4];
                   public float BTavgRange;
                   public char[] Spare = new char[16];
                   char Sensor;
                   char Hour;
                   char Minute;
                   char Second;
                   char Hundreth;
                   float Heading;
                   float Pitch;
                   float Roll;
                   float Temperature;
                   ushort BIT;
                   ushort Checksum;
                  
                   else//checksum failed
                       {
                           DataBuffReadIndex = TempReadIndex;//chuck the data
                           DecodeState = 0;
                           stay = 0;
                       }
                   }
                   else
                       stay = 0;
                 */
            }
            
            return csumOK;
        }
        
        float MaxTimeBetweenEnsembles = 0;
        float MinTimeBetweenEnsembles = 0;
        float LastEnsembleTime = 0;
        long LastEnsemble = 0;
        long MaxEnsembleSkip = 0;
        long MinEnsembleSkip = 0;

        long MaxErrorSkipEnsemble = 0;
        long MinErrorSkipEnsemble = 0;
        long MaxErrorTimeEnsemble = 0;
        long MinErrorTimeEnsemble = 0;

        long StatusErrorEnsemble = 0;
        long StatusError = 0;

        long LastStatus = 0;
        //void ProfileStats(ArrayClass m, ArrayClass2 m2,bool ShowPlaybackStats)
        void ProfileStats(ArrayClass m, bool ShowPlaybackStats)
        {
            //m.E_Year;            m.E_Month;            m.E_Day;
            float EnsembleTime = (float)(3600 * m.E_Hour + 60 * m.E_Minute + m.E_Second + m.E_Hsec / 100.0);

            if (LastEnsemble == 0)
            {
                StatusErrorEnsemble = 0;
                StatusError = 0;
                MaxErrorSkipEnsemble = 0;
                MaxErrorTimeEnsemble = 0;
                MinErrorSkipEnsemble = 0;
                MinErrorTimeEnsemble = 0;
                MaxEnsembleSkip = -1000000000;
                MinEnsembleSkip = 1000000000;
                MaxTimeBetweenEnsembles = -1000000000;
                MinTimeBetweenEnsembles = 1000000000;
            }
            else
            {
                if (m.E_Status > 255 && m.E_Status != LastStatus)
                {
                    LastStatus = m.E_Status;
                    StatusErrorEnsemble = m.E_EnsembleNumber;
                    StatusError = m.E_Status;
                    if (ShowPlaybackStats)
                        WriteMessageTxtSerial(StatusErrorEnsemble.ToString() + ",Status," + StatusError.ToString("X04"), true);
                }
                long EnsembleSkip = m.E_EnsembleNumber - LastEnsemble;

                if (EnsembleSkip != 1)
                {   
                    if (ShowPlaybackStats)
                        WriteMessageTxtSerial(EnsembleSkip.ToString() + ",Ensemble Skip," + m.E_EnsembleNumber.ToString(), true);
                }

                if (MaxEnsembleSkip < EnsembleSkip)
                {
                    MaxErrorSkipEnsemble = m.E_EnsembleNumber;
                    MaxEnsembleSkip = EnsembleSkip;
                    if (ShowPlaybackStats)
                        WriteMessageTxtSerial(MaxErrorSkipEnsemble.ToString() + ",Max Ensemble Skip," + MaxEnsembleSkip.ToString(), true);
                }
                if (MinEnsembleSkip > EnsembleSkip)
                {
                    MinErrorSkipEnsemble = m.E_EnsembleNumber;
                    MinEnsembleSkip = EnsembleSkip;
                    if (ShowPlaybackStats)
                        WriteMessageTxtSerial(MinErrorSkipEnsemble.ToString() + ",Min Ensemble Skip," + MinEnsembleSkip.ToString(), true);
                }

                float TBE = EnsembleTime - LastEnsembleTime;

                if (MaxTimeBetweenEnsembles < TBE)
                {
                    MaxErrorTimeEnsemble = m.E_EnsembleNumber;
                    MaxTimeBetweenEnsembles = TBE;
                    if (ShowPlaybackStats)
                        WriteMessageTxtSerial(MaxErrorTimeEnsemble.ToString() + ",Max Time Skip," + MaxTimeBetweenEnsembles.ToString("0.00"), true);
                }
                if (MinTimeBetweenEnsembles > TBE)
                {
                    MinErrorTimeEnsemble = m.E_EnsembleNumber;
                    MinTimeBetweenEnsembles = TBE;
                    if (ShowPlaybackStats)
                        WriteMessageTxtSerial(MinErrorTimeEnsemble.ToString() + ",Min Time Skip," + MinTimeBetweenEnsembles.ToString("0.00"), true);
                }
            }
            LastEnsembleTime = EnsembleTime;
            LastEnsemble = m.E_EnsembleNumber;
            //HEX
            //textBoxEnsembleErrorStatus.Text = StatusError.ToString("X04") + "," + StatusErrorEnsemble.ToString();
            //textBoxMinEnsembleDelta.Text = MinTimeBetweenEnsembles.ToString("0.00") + "," + MinErrorTimeEnsemble.ToString();
            //textBoxMaxEnsembleDelta.Text = MaxTimeBetweenEnsembles.ToString("0.00") + "," + MaxErrorTimeEnsemble.ToString();
            //textBoxMinEnsembleSkip.Text = MinEnsembleSkip.ToString() + "," + MinErrorSkipEnsemble.ToString();
            //textBoxMaxEnsembleSkip.Text = MaxEnsembleSkip.ToString() + "," + MaxErrorSkipEnsemble.ToString();

            int cs = (int)(Arr.E_CurrentSystem >> 24);
            if (cs > csubs - 1)
                cs = csubs - 1;
            if (m.VelocityAvailable)
            {
                for (int beam = 0; beam < m.E_Beams; beam++)
                {
                    for (int bin = 0; bin < 199; bin++)
                    {
                        if (m.Velocity[beam, bin] < 80 && m.Velocity[beam, bin + 1] < 80)
                        {
                            Arr2.VelDiffSum[cs, beam, bin] += m.Velocity[beam, bin + 1] - m.Velocity[beam, bin];
                            Arr2.VelDiffSumSqr[cs, beam, bin] += (m.Velocity[beam, bin + 1] - m.Velocity[beam, bin]) * (m.Velocity[beam, bin + 1] - m.Velocity[beam, bin]);
                            Arr2.VelDiffN[cs, beam, bin]++;
                        }
                    }
                    m.BeamN[beam, 199] = 0;
                    Arr2.VelDiffN[cs, beam, 199] = 0;
                }
            }
            if (m.InstrumentAvailable)
            {
                for (int beam = 0; beam < m.E_Beams; beam++)
                {
                    for (int bin = 0; bin < 199; bin++)
                    {
                        if (m.Instrument[beam, bin] < 80)//m.XfrmN[beam, bin] > 0)
                        {
                            Arr2.VelSumI[cs, beam, bin] += m.Instrument[beam, bin];
                            Arr2.VelSumSqrI[cs, beam, bin] += (m.Instrument[beam, bin] * m.Instrument[beam, bin]);
                            Arr2.VelNI[cs, beam, bin]++;
                        }
                    }
                    //m.XfrmN[beam, 199] = 0;
                }
            }
            if (m.EarthAvailable)
            {
                for (int beam = 0; beam < m.E_Beams; beam++)
                {
                    for (int bin = 0; bin < 199; bin++)
                    {
                        if (m.Earth[beam, bin] < 80)//m.XfrmN[beam, bin] > 0)
                        {
                            Arr2.VelSumE[cs, beam, bin] += m.Earth[beam, bin];
                            Arr2.VelSumSqrE[cs, beam, bin] += (m.Earth[beam, bin] * m.Earth[beam, bin]);
                            Arr2.VelNE[cs, beam, bin]++;
                        }
                    }
                    //m.XfrmN[beam, 199] = 0;
                }
            }

            if (m.EngProfileDataAvailable)
            {
                for (int beam = 0; beam < m.E_Beams; beam++)
                {
                    Arr2.PPVelSum[cs, beam] += m.Eng_ProfVel[beam];
                    Arr2.PPVelSumSqr[cs, beam] += m.Eng_ProfVel[beam] * m.Eng_ProfVel[beam];
                    Arr2.PPVelN[cs, beam]++;
                    if (Arr2.PPVelN[cs, beam] > 1)
                    {
                        Arr2.PPVelSD[cs, beam] = (float)(Math.Sqrt((Arr2.PPVelSumSqr[cs, beam] - ((Arr2.PPVelSum[cs, beam] * Arr2.PPVelSum[cs, beam]) / Arr2.PPVelN[cs, beam])) / (Arr2.PPVelN[cs, beam] - 1)));
                    }
                    else
                    {
                        Arr2.PPVelSD[cs, beam] = 0;
                    }
                }
            }
        }
        void AccumulateEnsemble(ArrayClass m, ArrayClass2 m2)
        {
            int cs = (int)(m.E_CurrentSystem >> 24);
            if (cs > csubs - 1)
                cs = csubs - 1;

            if (FindBTinProfile)
                BTPRstr = FindBTinProfileF();

            m2.beams[cs] = (int)m.E_Beams;
            m2.bins[cs] = (int)m.E_Cells;
            m2.firstbin[cs] = m.A_FirstCellDepth;
            m2.binsize[cs] = m.A_CellSize;
            for (int beam = 0; beam < m.E_Beams; beam++)
            {
                if (m.VelocityAvailable)
                {
                    for (int bin = 0; bin < 200; bin++)
                    {
                        if (m.Velocity[beam, bin] < 80)//m.BeamN[beam, bin] > 0)
                        {
                            m2.VelSum[cs, beam, bin] += m.Velocity[beam, bin];
                            m2.VelN[cs, beam, bin]++;
                        }
                    }
                }
                if (m.InstrumentAvailable)
                {
                    for (int bin = 0; bin < 200; bin++)
                    {
                        if (m.Instrument[beam, bin] < 80)
                        {
                            m2.InsSum[cs, beam, bin] += m.Instrument[beam, bin];
                            m2.InsN[cs, beam, bin]++;
                        }
                        else
                        {
                            m.Instrument[beam, bin] = m.Instrument[beam, bin];
                        }
                    }
                }
                if (m.EarthAvailable)
                {
                    for (int bin = 0; bin < 200; bin++)
                    {
                        if (m.Earth[beam, bin] < 80)//(m.XfrmN[beam, bin] > 0)
                        {
                            m2.EarSum[cs, beam, bin] += m.Earth[beam, bin];
                            m2.EarN[cs, beam, bin]++;
                        }
                    }
                }
                if (m.CorrelationAvailable)
                {
                    for (int bin = 0; bin < 200; bin++)
                    {
                        m2.CorSum[cs, beam, bin] += m.Correlation[beam, bin];
                        if (m2.CorMax[cs, beam, bin] < m.Correlation[beam, bin])
                            m2.CorMax[cs, beam, bin] = m.Correlation[beam, bin];
                        if (m2.CorMin[cs, beam, bin] > m.Correlation[beam, bin])
                            m2.CorMin[cs, beam, bin] = m.Correlation[beam, bin];

                        m2.CorN[cs, beam, bin]++;
                    }
                }
                if (m.CorrelationAvailable)
                {
                    for (int bin = 0; bin < 200; bin++)
                    {
                        m2.AmpSum[cs, beam, bin] += m.Amplitude[beam, bin];
                        m2.AmpN[cs, beam, bin]++;

                        if (m2.AmpMax[cs, beam, bin] < m.Amplitude[beam, bin])
                            m2.AmpMax[cs, beam, bin] = m.Amplitude[beam, bin];
                        if (m2.AmpMin[cs, beam, bin] > m.Amplitude[beam, bin])
                            m2.AmpMin[cs, beam, bin] = m.Amplitude[beam, bin];
                    }
                }
            }
        }
        void DecodeEnsemble(byte[] packet, ArrayClass m)
        {
            Array.Clear(m.BeamN, 0, m.BeamN.Length);
            Array.Clear(m.XfrmN, 0, m.XfrmN.Length);
            Array.Clear(m.B_BeamN, 0, m.B_BeamN.Length);
            Array.Clear(m.B_XfrmN, 0, m.B_XfrmN.Length);

            m.NMEA_Bytes = 0;

            int i,n;
            int SizeCount;
            int ArrayCount;

            PacketPointer = HDRLEN;

            RiverBT.Available = false;
            RiverTS.Available = false;
            RiverNMEA.Available = false;
            RiverBTH.Available = false;
            RiverTran.Available = false;
            RiverTran.StationAvailable = false;

            m.VelocityAvailable = false;
            m.InstrumentAvailable = false;
            m.EarthAvailable = false;
            m.AmplitudeAvailable = false;
            m.CorrelationAvailable = false;
            m.BeamNAvailable = false;
            m.XfrmNAvailable = false;
            m.EnsembleDataAvailable = false;
            m.AncillaryAvailable = false;
            m.BottomTrackAvailable = false;
            m.NmeaAvailable = false;
            m.EngProfileDataAvailable = false;
            m.EngBottomTrackDataAvailable = false;
            m.SystemSetupDataAvailable = false;
            m.RTonWPAvailable = false;
            m.GageAvailable = false;

            textBoxExtractProfileEnsembleNumber.Text = "";

            for (i = 0; i < MaxArray; i++)
            {
                m.Type[i] = ByteArrayToInt(packet);
                m.Bins[i] = ByteArrayToInt(packet);
                m.Beams[i] = ByteArrayToInt(packet);
                m.Imag[i] = ByteArrayToInt(packet);
                m.NameLen[i] = ByteArrayToInt(packet);
                m.Name[i] = ByteArrayToString(packet, 8);

                ArrayCount = m.Bins[i] * m.Beams[i];
                /*
                if (ArrayCount > 200 * 4)
                {
                    //ArrayCount = 0;
                    break;
                }*/

                switch (m.Type[i])
                {
                    default:
                        break;
                    case 50:
                        break;
                    case 0:
                        ArrayCount *= 8;
                        break;
                    case 10:
                    case 20:
                        ArrayCount *= 4;
                        break;
                    case 30:
                    case 40:
                        ArrayCount *= 2;
                        break;
                }

                SizeCount = PacketPointer;

                if (VelocityID.Equals(m.Name[i], StringComparison.Ordinal))
                {
                    

                    m.VelocityAvailable = true;
                    switch(m.Beams[i])
                    {
                        case 1:
                            for (int beam = 0; beam < m.Beams[i]; beam++)
                            {
                                for (int bin = 0; bin < m.Bins[i]; bin++)
                                {
                                    m.Velocity[beam, bin] = ByteArrayToFloat(packet);
                                    m.Instrument[beam, bin] = m.Velocity[beam, bin];
                                    m.Earth[beam, bin] = m.Velocity[beam, bin];
                                }
                            }
                        
                            break;
                        case 2:
                            for (int beam = 0; beam < m.Beams[i]; beam++)
                            {
                                for (int bin = 0; bin < m.Bins[i]; bin++)
                                {
                                    m.Velocity[beam, bin] = ByteArrayToFloat(packet);
                                    //m.Instrument[beam, bin] = m.Velocity[beam, bin];
                                    m.Earth[beam, bin] = m.Velocity[beam, bin];
                                }
                            }

                            break;
                        default:
                            for (int beam = 0; beam < m.Beams[i]; beam++)
                            {
                                for (int bin = 0; bin < m.Bins[i]; bin++)
                                {
                                    m.Velocity[beam, bin] = ByteArrayToFloat(packet);
                                }
                            }
                            break;
                    }
                }
                else
                {
                    if (InstrumentID.Equals(m.Name[i], StringComparison.Ordinal))
                    {
                        m.InstrumentAvailable = true;

                        for (int beam = 0; beam < m.Beams[i]; beam++)
                        {
                            for (int bin = 0; bin < m.Bins[i]; bin++)
                            {
                                m.Instrument[beam, bin] = ByteArrayToFloat(packet);
                            }
                        }
                    }
                    else
                    {
                        if (EarthID.Equals(m.Name[i], StringComparison.Ordinal))
                        {
                            m.EarthAvailable = true;
                            for (int beam = 0; beam < m.Beams[i]; beam++)
                            {
                                for (int bin = 0; bin < m.Bins[i]; bin++)
                                {
                                    m.Earth[beam, bin] = ByteArrayToFloat(packet);
                                }
                            }
                        }
                        else
                        {
                            if (AmplitudeID.Equals(m.Name[i], StringComparison.Ordinal))
                            {
                                m.AmplitudeAvailable = true;
                                for (int beam = 0; beam < m.Beams[i]; beam++)
                                {
                                    for (int bin = 0; bin < m.Bins[i]; bin++)
                                    {
                                        m.Amplitude[beam, bin] = ByteArrayToFloat(packet);
                                    }
                                }
                            }
                            else
                            {
                                if (CorrelationID.Equals(m.Name[i], StringComparison.Ordinal))
                                {
                                    m.CorrelationAvailable = true;
                                    for (int beam = 0; beam < m.Beams[i]; beam++)
                                    {
                                        for (int bin = 0; bin < m.Bins[i]; bin++)
                                        {
                                            m.Correlation[beam, bin] = ByteArrayToFloat(packet);
                                        }
                                    }
                                }
                                else
                                {
                                    if (BeamNID.Equals(m.Name[i], StringComparison.Ordinal))
                                    {
                                        m.BeamNAvailable = true;
                                        for (int beam = 0; beam < m.Beams[i]; beam++)
                                        {
                                            for (int bin = 0; bin < m.Bins[i]; bin++)
                                            {
                                                m.BeamN[beam, bin] = ByteArrayToInt(packet);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (XfrmNID.Equals(m.Name[i], StringComparison.Ordinal))
                                        {
                                            m.XfrmNAvailable = true;
                                            for (int beam = 0; beam < m.Beams[i]; beam++)
                                            {
                                                for (int bin = 0; bin < m.Bins[i]; bin++)
                                                {
                                                    m.XfrmN[beam, bin] = ByteArrayToInt(packet);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (EnsembleDataID.Equals(m.Name[i], StringComparison.Ordinal))
                                            {
                                                m.EnsembleDataAvailable = true;
                                                m.E_EnsembleNumber = ByteArrayToInt(packet);

                                                textBoxExtractProfileEnsembleNumber.Text = m.E_EnsembleNumber.ToString();

                                                m.E_Cells = ByteArrayToInt(packet);
                                                if (m.E_Cells > cbins)
                                                    m.E_Cells = cbins;
                                                m.E_Beams = ByteArrayToInt(packet);
                                                if (m.E_Beams > cbeams)
                                                    m.E_Beams = cbeams;
                                                m.E_PingsInEnsemble = ByteArrayToInt(packet);
                                                m.E_PingCount = ByteArrayToInt(packet);
                                                m.E_Status = ByteArrayToInt(packet);
                                                m.E_Year = ByteArrayToInt(packet);
                                                m.E_Month = ByteArrayToInt(packet);
                                                m.E_Day = ByteArrayToInt(packet);
                                                m.E_Hour = ByteArrayToInt(packet);
                                                m.E_Minute = ByteArrayToInt(packet);
                                                m.E_Second = ByteArrayToInt(packet);
                                                m.E_Hsec = ByteArrayToInt(packet);
                                                for (n = 0; n < 32; n++)
                                                {
                                                    if (packet[PacketPointer] > 31 && packet[PacketPointer] < 127)
                                                        m.E_SN_Buffer[n] = packet[PacketPointer];
                                                    else
                                                        m.E_SN_Buffer[n] = 63;

                                                    PacketPointer++;
                                                }
                                                m.E_SN_Buffer[n] = 0;
                                                for (n = 0; n < 4; n++)
                                                {
                                                    m.E_FW_Vers[n] = packet[PacketPointer];
                                                    PacketPointer++;
                                                }
                                                m.E_CurrentSystem = (ByteArrayToInt(packet));

                                                long cs = m.E_CurrentSystem >> 24;
                                                if (cs > csubs - 1)
                                                    cs = csubs - 1;
                                                subbeams[cs] = (int)m.E_Beams;

                                                m.E_Status2 = ByteArrayToInt(packet);
                                                m.E_BurstIndex = ByteArrayToInt(packet);
                                                

                                                //ProfileAmpScaledB = true;
                                                //if (m.E_SN_Buffer[0] == '0' && m.E_SN_Buffer[1] == '0')
                                                //{
                                                //    if (m.E_FW_Vers[0] < 37 && m.E_FW_Vers[1] == 0)
                                                //        ProfileAmpScaledB = false;
                                                //}
                                            }
                                            else
                                            {
                                                if (AncillaryID.Equals(m.Name[i], StringComparison.Ordinal))
                                                {
                                                    m.AncillaryAvailable = true;
                                                    m.A_FirstCellDepth = ByteArrayToFloat(packet);
                                                    m.A_CellSize = ByteArrayToFloat(packet);
                                                    m.A_FirstPingSeconds = ByteArrayToFloat(packet);
                                                    m.A_LastPingSeconds = ByteArrayToFloat(packet);

                                                    m.A_Heading = ByteArrayToFloat(packet);
                                                    m.A_Pitch = ByteArrayToFloat(packet);
                                                    m.A_Roll = ByteArrayToFloat(packet);
                                                    m.A_WaterTemperature = ByteArrayToFloat(packet);
                                                    m.A_BoardTemperature = ByteArrayToFloat(packet);
                                                    m.A_Salinity = ByteArrayToFloat(packet);
                                                    m.A_Pressure = ByteArrayToFloat(packet);
                                                    m.A_Depth = ByteArrayToFloat(packet);
                                                    m.A_SpeedOfSound = ByteArrayToFloat(packet);
                                                    m.A_Mx = ByteArrayToFloat(packet);
                                                    m.A_My = ByteArrayToFloat(packet);
                                                    m.A_Mz = ByteArrayToFloat(packet);
                                                    m.A_Gp = ByteArrayToFloat(packet);
                                                    m.A_Gr = ByteArrayToFloat(packet);
                                                    m.A_Gz = ByteArrayToFloat(packet);
                                                    m.A_HS1Temperature = ByteArrayToFloat(packet);
                                                    m.A_HS2Temperature = ByteArrayToFloat(packet);
                                                    m.A_RCV1Temperature = ByteArrayToFloat(packet);
                                                    m.A_RCV2Temperature = ByteArrayToFloat(packet);
                                                    m.A_VINF = ByteArrayToFloat(packet);
                                                    m.A_VG = ByteArrayToFloat(packet);
                                                    m.A_VT = ByteArrayToFloat(packet);
                                                    m.A_VTL = ByteArrayToFloat(packet);
                                                    m.A_D3V3 = ByteArrayToFloat(packet);
                                                    m.A_SPARE = ByteArrayToFloat(packet);
                                                }
                                                else//m.GageAvailable
                                                {
                                                    if (GageID.Equals(m.Name[i], StringComparison.Ordinal))
                                                    {
                                                        m.GageAvailable = true;
                                                        m.RGH_Status = ByteArrayToFloat(packet);//1
                                                        m.RGH_AvgRange = ByteArrayToFloat(packet);//2
                                                        m.RGH_sd = ByteArrayToFloat(packet);//3
                                                        m.RGH_AvgSN = ByteArrayToFloat(packet);//4
                                                        m.RGH_n = ByteArrayToFloat(packet);//5
                                                        m.RGH_Salinity = ByteArrayToFloat(packet);//6
                                                        m.RGH_Pressure = ByteArrayToFloat(packet);//7
                                                        m.RGH_Depth = ByteArrayToFloat(packet);//8
                                                        m.RGH_WaterTemperature = ByteArrayToFloat(packet);//9
                                                        m.RGH_BackPlaneTemperature = ByteArrayToFloat(packet);//10
                                                        m.RGH_SOS = ByteArrayToFloat(packet);//11
                                                        m.RGH_Heading = ByteArrayToFloat(packet);//12
                                                        m.RGH_Pitch = ByteArrayToFloat(packet);//13
                                                        m.RGH_Roll = ByteArrayToFloat(packet);//14
                                                        m.RGH_AvgS = ByteArrayToFloat(packet);//15
                                                        m.RGH_AvgN1 = ByteArrayToFloat(packet);//16
                                                        m.RGH_AvgN2 = ByteArrayToFloat(packet);//17
                                                        m.RGH_GainFrac = ByteArrayToFloat(packet);//18

                                                        m.RGH_Pings = ByteArrayToFloat(packet);//19
                                                        m.RGH_SNthreshold = ByteArrayToFloat(packet);//20
                                                        m.RGH_GainThres = ByteArrayToFloat(packet);//21
                                                        m.RGH_StatThres = ByteArrayToFloat(packet);//22
                                                        m.RGH_XmtCycles = ByteArrayToFloat(packet);//23
                                                        m.RGH_DepthOffset = ByteArrayToFloat(packet);//24
                                                    }
                                                else
                                                {
                                                    if (BottomTrackID.Equals(m.Name[i], StringComparison.Ordinal))
                                                    {
                                                        m.BottomTrackAvailable = true;
                                                        m.B_FirstPingSeconds = ByteArrayToFloat(packet);
                                                        m.B_LastPingSeconds = ByteArrayToFloat(packet);

                                                        m.B_Heading = ByteArrayToFloat(packet);
                                                        m.B_Pitch = ByteArrayToFloat(packet);
                                                        m.B_Roll = ByteArrayToFloat(packet);
                                                        m.B_WaterTemperature = ByteArrayToFloat(packet);
                                                        m.B_BoardTemperature = ByteArrayToFloat(packet);
                                                        m.B_Salinity = ByteArrayToFloat(packet);
                                                        m.B_Pressure = ByteArrayToFloat(packet);
                                                        m.B_Depth = ByteArrayToFloat(packet);
                                                        m.B_SpeedOfSound = ByteArrayToFloat(packet);

                                                        m.B_Status = ByteArrayToFloat(packet);
                                                        m.B_Beams = ByteArrayToFloat(packet);
                                                        if (m.B_Beams > 4)
                                                            m.B_Beams = 4;
                                                        m.B_PingCount = ByteArrayToFloat(packet);

                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Range[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_SNR[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Amplitude[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Correlation[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Velocity[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_BeamN[beam] = ByteArrayToFloat(packet);

                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Instrument[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_XfrmN[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_Earth[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                        {
                                                            m.B_EarthN[beam] = ByteArrayToFloat(packet);
                                                            if((m.B_Earth[beam] > 20) || (m.B_Earth[beam] < -20))
                                                            {
                                                                if (m.B_Earth[beam] < 88)
                                                                {
                                                                    m.B_Earth[beam] = 89;
                                                                    m.B_EarthN[beam] = 0;
                                                                }
                                                            }
                                                            if (float.IsNaN(m.B_Earth[beam]) || float.IsInfinity(m.B_Earth[beam]))
                                                            {
                                                                m.B_Earth[beam] = 90;
                                                                m.B_EarthN[beam] = 0;
                                                            }   
                                                        }
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_SNRs[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_AmpS[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_VelS[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_NoiseS[beam] = ByteArrayToFloat(packet);
                                                        for (int beam = 0; beam < m.B_Beams; beam++)
                                                            m.B_CorS[beam] = ByteArrayToFloat(packet);

                                                            m.B_HS1Temperature = ByteArrayToFloat(packet);
                                                            m.B_HS2Temperature = ByteArrayToFloat(packet);
                                                            m.B_RCV1Temperature = ByteArrayToFloat(packet);
                                                            m.B_RCV2Temperature = ByteArrayToFloat(packet);
                                                            m.B_VINF = ByteArrayToFloat(packet);
                                                            m.B_VG = ByteArrayToFloat(packet);
                                                            m.B_VT = ByteArrayToFloat(packet);
                                                            m.B_VTL = ByteArrayToFloat(packet);
                                                            m.B_D3V3 = ByteArrayToFloat(packet);
                                                            m.B_SPARE = ByteArrayToFloat(packet);

                                                            m.B_SounderRange = ByteArrayToFloat(packet);
                                                            m.B_SounderSNR = ByteArrayToFloat(packet);
                                                            m.B_SounderAmp = ByteArrayToFloat(packet);

                                                        }
                                                    else
                                                    {
                                                        if (NMEAID.Equals(m.Name[i], StringComparison.Ordinal))
                                                        {
                                                            m.NmeaAvailable = true;
                                                            int ii = 0;
                                                            while (packet[PacketPointer] != 0)
                                                            {
                                                                m.NMEA_Buffer[ii++] = packet[PacketPointer++];
                                                                if (ii >= 8192)
                                                                    break;
                                                            }
                                                            m.NMEA_Bytes = ii;
                                                            for (int end = ii; end < 8192; end++)
                                                            {
                                                                m.NMEA_Buffer[end] = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (EngProfileDataID.Equals(m.Name[i], StringComparison.Ordinal))
                                                            {
                                                                m.EngProfileDataAvailable = true;
                                                                for (int beam = 0; beam < 4; beam++)
                                                                    m.Eng_ProfVel[beam] = ByteArrayToFloat(packet);
                                                                for (int beam = 0; beam < 4; beam++)
                                                                    m.Eng_ProfCor[beam] = ByteArrayToFloat(packet);
                                                                for (int beam = 0; beam < 4; beam++)
                                                                    m.Eng_ProfAmp[beam] = ByteArrayToFloat(packet);

                                                                m.Eng_SamplesPerSecond = ByteArrayToFloat(packet);
                                                                m.Eng_SystemFrequency = ByteArrayToFloat(packet);
                                                                m.Eng_LagSamples = ByteArrayToFloat(packet);
                                                                m.Eng_CPCE = ByteArrayToFloat(packet);
                                                                m.Eng_NCE = ByteArrayToFloat(packet);
                                                                m.Eng_RepeatN = ByteArrayToFloat(packet);

                                                                m.Eng_sGap = ByteArrayToFloat(packet);
                                                                m.Eng_sNCE = ByteArrayToFloat(packet);
                                                                m.Eng_sRepeatN = ByteArrayToFloat(packet);
                                                                m.Eng_sLagSamples = ByteArrayToFloat(packet);
                                                                m.Eng_RcvGain = ByteArrayToFloat(packet);
                                                                if (m.Eng_sLagSamples == 0)
                                                                    m.Eng_sLagSamples = m.Eng_sNCE;
                                                                m.Eng_sCPCE = m.Eng_CPCE;
                                                            }
                                                            else
                                                            {
                                                                if (EngBottomTrackDataID.Equals(m.Name[i], StringComparison.Ordinal))
                                                                {
                                                                    m.EngBottomTrackDataAvailable = true;
                                                                    m.EngBT_SamplesPerSecond = ByteArrayToFloat(packet);
                                                                    m.EngBT_SystemFrequency = ByteArrayToFloat(packet);

                                                                    m.EngBT_CPCE = ByteArrayToFloat(packet);
                                                                    m.EngBT_NCE = ByteArrayToFloat(packet);
                                                                    m.EngBT_RepeatN = ByteArrayToFloat(packet);
                                                                    m.EngBT_Spare = ByteArrayToFloat(packet);

                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_SLag[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_SNoise[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_SSNR[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_SCor[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_SAmp[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_SVel[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_SHz[beam] = ByteArrayToFloat(packet);

                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M1Lag[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M1Noise[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M1SNR[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M1Cor[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M1Amp[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M1Vel[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M1Hz[beam] = ByteArrayToFloat(packet);

                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M2Lag[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M2Noise[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M2SNR[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M2Cor[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M2Amp[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M2Vel[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M2Hz[beam] = ByteArrayToFloat(packet);

                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_LLLag[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_LLNoise[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_LLSNR[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_LLCor[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_LLAmp[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_LLVel[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_LLHz[beam] = ByteArrayToFloat(packet);

                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.EngBT_LagUsed[beam] = ByteArrayToFloat(packet);

                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_SLSampleDepth[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M1SampleDepth[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_M2SampleDepth[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_LLSampleDepth[beam] = ByteArrayToFloat(packet);

                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_AmbHz[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_AmbVel[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_AmbAmp[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_AmbCor[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_AmbSNR[beam] = ByteArrayToFloat(packet);
                                                                    for (int beam = 0; beam < 4; beam++)
                                                                        m.Eng_AmbLagSamp[beam] = ByteArrayToFloat(packet);
                                                                }
                                                                else
                                                                {
                                                                    if (SystemSetupDataID.Equals(m.Name[i], StringComparison.Ordinal))
                                                                    {
                                                                        m.SystemSetupDataAvailable = true;
                                                                        m.SystemSetup_BTSamplesPerSecond = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_BTSystemFreqHz = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_BTCPCE = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_BTNCE = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_BTRepeatN = ByteArrayToFloat(packet);

                                                                        m.SystemSetup_WPSamplesPerSecond = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPSystemFreqHz = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPCPCE = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPNCE = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPRepeatN = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPLagSamples = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_InputVoltage = ByteArrayToFloat(packet);//12
                                                                        m.SystemSetup_TransmitVoltage = ByteArrayToFloat(packet);//13

                                                                        m.SystemSetup_BTBB = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_BTLL = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_BTNB = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_BTMUX = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPBB = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPLL = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPXMTBW = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPRCVBW = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_TransmitVoltageMinus = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPMUX = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_BTscale = ByteArrayToFloat(packet);
                                                                        m.SystemSetup_WPscale = ByteArrayToFloat(packet);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (RTonWPDataID.Equals(m.Name[i], StringComparison.Ordinal))
                                                                        {
                                                                            m.RTonWPAvailable = true;
                                                                            m.RTonWP_Beams = ByteArrayToFloat(packet);
                                                                            if (m.RTonWP_Beams > 4)
                                                                                m.RTonWP_Beams = 4;
                                                                            for (int beam = 0; beam < m.RTonWP_Beams; beam++)
                                                                                m.RTonWP_SNR[beam] = ByteArrayToFloat(packet);
                                                                            for (int beam = 0; beam < m.RTonWP_Beams; beam++)
                                                                                m.RTonWP_Range[beam] = ByteArrayToFloat(packet);
                                                                            for (int beam = 0; beam < m.RTonWP_Beams; beam++)
                                                                                m.RTonWP_Pings[beam] = ByteArrayToFloat(packet);
                                                                            for (int beam = 0; beam < m.RTonWP_Beams; beam++)
                                                                                m.RTonWP_Amp[beam] = ByteArrayToFloat(packet);
                                                                            for (int beam = 0; beam < m.RTonWP_Beams; beam++)
                                                                                 m.RTonWP_Cor[beam] = ByteArrayToFloat(packet);
                                                                            for (int beam = 0; beam < m.RTonWP_Beams; beam++)
                                                                                m.RTonWP_Vel[beam] = ByteArrayToFloat(packet);
                                                                            for (int beam = 0; beam < m.RTonWP_Beams; beam++)
                                                                                m.RTonWP_Ins[beam] = ByteArrayToFloat(packet);
                                                                            for (int beam = 0; beam < m.RTonWP_Beams; beam++)
                                                                                m.RTonWP_Earth[beam] = ByteArrayToFloat(packet);
                                                                                
                                                                            }
                                                                            else 
                                                                            {
                                                                                if (RiverBTID.Equals(m.Name[i], StringComparison.Ordinal))
                                                                                {
                                                                                    RiverBT.Available = true;
                                                                                    RiverBT.Subs = ByteArrayToFloat(packet);
                                                                                    for(int sb=0;sb< RiverBT.Subs;sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.PingCount[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Status[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Beams[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.NCE[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.RepeatN[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.CPCE[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.BB[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.LL[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.BTbeamMux[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.NB[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.PingSeconds[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Heading[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Pitch[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Roll[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.WaterTemperature[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.BackPlaneTemperature[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Salinity[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Pressure[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Depth[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.SpeedOfSound[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Mx[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.My[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Mz[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Gp[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Gr[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.Gz[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.SamplesPerSecond[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        if (sb < BTMAXSUBS)
                                                                                            RiverBT.SystemFreqHz[sb] = ByteArrayToFloat(packet);
                                                                                        else
                                                                                            PacketPointer += 4;
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.Range[sb,bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.SNR[sb, bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.Amplitude[sb,bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.NoiseAmpBackPorch[sb,bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.NoiseAmpFrontPorch[sb,bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.Correlation[sb, bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.Velocity[sb, bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.BeamN[sb, bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    /*
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.Instrument[sb, bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.XfrmN[sb, bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.Earth[sb, bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    for (int sb = 0; sb < RiverBT.Subs; sb++)
                                                                                    {
                                                                                        for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                                                                                        {
                                                                                            if (sb < BTMAXSUBS && bm < BTMAXBEAMS)
                                                                                                RiverBT.EarthN[sb, bm] = ByteArrayToFloat(packet);
                                                                                            else
                                                                                                PacketPointer += 4;
                                                                                        }
                                                                                    }
                                                                                    */
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (RiverTimeStampID.Equals(m.Name[i], StringComparison.Ordinal))
                                                                                    {
                                                                                        RiverTS.Available = true;
                                                                                        RiverTS.TimeStampGGA = ByteArrayToFloat(packet);
                                                                                        RiverTS.TimeStampVTG = ByteArrayToFloat(packet);
                                                                                        RiverTS.TimeStampHDT = ByteArrayToFloat(packet);
                                                                                        RiverTS.TimeStampDBT = ByteArrayToFloat(packet);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (RiverNMEAID.Equals(m.Name[i], StringComparison.Ordinal))
                                                                                        {
                                                                                            RiverNMEA.Available = true;
                                                                                            int bytes = m.Bins[i];//m.Beams[i]
                                                                                            byte ii;
                                                                                            byte b;

                                                                                            ii = 0;
                                                                                            if (Math.Abs(RiverTS.TimeStampGGA) != 1000)
                                                                                            {
                                                                                                for (n = 0; n < bytes; n++)
                                                                                                {
                                                                                                    b = packet[PacketPointer++];
                                                                                                    if (b != 0)
                                                                                                    {
                                                                                                        RiverNMEA.GGA[ii++] = b;
                                                                                                        if (ii >= 200 || b == 10)
                                                                                                            break;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        break;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            RiverNMEA.GGAbytes = ii;

                                                                                            ii = 0;
                                                                                            if (Math.Abs(RiverTS.TimeStampVTG) != 1000)
                                                                                            {
                                                                                                for (n = 0; n < bytes; n++)
                                                                                                {
                                                                                                    b = packet[PacketPointer++];
                                                                                                    if (b != 0)
                                                                                                    {
                                                                                                        RiverNMEA.VTG[ii++] = b;
                                                                                                        if (ii >= 100 || b == 10)
                                                                                                            break;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        break;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            RiverNMEA.VTGbytes = ii;

                                                                                            ii = 0;
                                                                                            if (Math.Abs(RiverTS.TimeStampHDT) != 1000)
                                                                                            {
                                                                                                for (n = 0; n < bytes; n++)
                                                                                                {
                                                                                                    b = packet[PacketPointer++];
                                                                                                    if (b != 0)
                                                                                                    {
                                                                                                        RiverNMEA.HDT[ii++] = b;
                                                                                                        if (ii >= 100 || b == 10)
                                                                                                            break;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        break;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            RiverNMEA.HDTbytes = ii;

                                                                                            ii = 0;
                                                                                            if (Math.Abs(RiverTS.TimeStampDBT) != 1000)
                                                                                            {
                                                                                                for (n = 0; n < bytes; n++)
                                                                                                {
                                                                                                    b = packet[PacketPointer++];
                                                                                                    if (b != 0)
                                                                                                    {
                                                                                                        RiverNMEA.DBT[ii++] = b;
                                                                                                        if (ii >= 100 || b == 10)
                                                                                                            break;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        break;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            RiverNMEA.DBTbytes = ii;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (RiverBThump.Equals(m.Name[i], StringComparison.Ordinal))
                                                                                            {
                                                                                                RiverBTH.Available = true;
                                                                                                RiverBTH.Subs = ByteArrayToFloat(packet);
                                                                                                int numsub;
                                                                                                int numbeam;
                                                                                                int numbin;

                                                                                                for (int sb = 0; sb < (int)RiverBTH.Subs; sb++)
                                                                                                {
                                                                                                    numsub = sb;
                                                                                                    if (numsub >= BTMAXSUBS)
                                                                                                        numsub = BTMAXSUBS - 1;

                                                                                                    RiverBTH.Beams[numsub] = ByteArrayToFloat(packet);
                                                                                                    RiverBTH.Bins[numsub] = ByteArrayToFloat(packet);
                                                                                                    RiverBTH.BinSize[numsub] = ByteArrayToFloat(packet);
                                                                                                    for (int beam = 0; beam < (int)RiverBTH.Beams[numsub]; beam++)
                                                                                                    {
                                                                                                        numbeam = beam;
                                                                                                        if (numbeam >= BTHMAXBEAMS)
                                                                                                            numbeam = BTHMAXBEAMS - 1;
                                                                                                        for (int bin = 0; bin < (int)RiverBTH.Bins[numsub]; bin++)
                                                                                                        {
                                                                                                            numbin = bin;
                                                                                                            if (numbin >= BTHMAXBINS)
                                                                                                                numbin = BTHMAXBINS - 1;

                                                                                                            RiverBTH.Amp[numsub, numbeam, numbin] = ByteArrayToFloat(packet);
                                                                                                        }
                                                                                                    }
                                                                                                    for (int beam = 0; beam < (int)RiverBTH.Beams[numsub]; beam++)
                                                                                                    {
                                                                                                        numbeam = beam;
                                                                                                        if (numbeam >= BTHMAXBEAMS)
                                                                                                            numbeam = BTHMAXBEAMS - 1;
                                                                                                        for (int bin = 0; bin < (int)RiverBTH.Bins[numsub]; bin++)
                                                                                                        {
                                                                                                            numbin = bin;
                                                                                                            if (numbin >= BTHMAXBINS)
                                                                                                                numbin = BTHMAXBINS - 1;

                                                                                                            RiverBTH.Cor[numsub, numbeam, numbin] = ByteArrayToFloat(packet);
                                                                                                        }
                                                                                                    }
                                                                                                    for (int beam = 0; beam < (int)RiverBTH.Beams[numsub]; beam++)
                                                                                                    {
                                                                                                        numbeam = beam;
                                                                                                        if (numbeam >= BTHMAXBEAMS)
                                                                                                            numbeam = BTHMAXBEAMS - 1;
                                                                                                        for (int bin = 0; bin < (int)RiverBTH.Bins[numsub]; bin++)
                                                                                                        {
                                                                                                            numbin = bin;
                                                                                                            if (numbin >= BTHMAXBINS)
                                                                                                                numbin = BTHMAXBINS - 1;

                                                                                                            RiverBTH.Vel[numsub, numbeam, numbin] = ByteArrayToFloat(packet);
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (RiverStationID.Equals(m.Name[i], StringComparison.Ordinal))
                                                                                                {
                                                                                                    RiverTran.StationAvailable = true;
                                                                                                    
                                                                                                    int ii = 0;
                                                                                                    while (packet[PacketPointer] != 0)
                                                                                                    {
                                                                                                        RiverTran.StationName[ii++] = packet[PacketPointer++];
                                                                                                        if (ii > 199)
                                                                                                            break;
                                                                                                    }
                                                                                                    RiverTran.LenName = ii;
                                                                                                    PacketPointer++;
                                                                                                    ii = 0;
                                                                                                    while (packet[PacketPointer] != 0)
                                                                                                    {
                                                                                                        RiverTran.StationNumber[ii++] = packet[PacketPointer++];
                                                                                                        if (ii > 199)
                                                                                                            break;
                                                                                                    }
                                                                                                    RiverTran.LenNumber = ii;
                                                                                                    PacketPointer++;

                                                                                                    //RiverBT.Subs = ByteArrayToFloat(packet);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if (RiverTransectID.Equals(m.Name[i], StringComparison.Ordinal))
                                                                                                    {
                                                                                                        RiverTran.Available = true;
                                                                                                        RiverTran.TransectState = ByteArrayToFloat(packet);
                                                                                                        RiverTran.TransectNumber = ByteArrayToFloat(packet);
                                                                                                        RiverTran.TransectStatus = ByteArrayToFloat(packet);
                                                                                                        RiverTran.BottomStatus = ByteArrayToFloat(packet);
                                                                                                        RiverTran.ProfileStatus = ByteArrayToFloat(packet);
                                                                                                        RiverTran.MovingEnsembles = ByteArrayToFloat(packet);
                                                                                                        RiverTran.MovingBTEnsembles = ByteArrayToFloat(packet);
                                                                                                        RiverTran.MovingWPEnsembles = ByteArrayToFloat(packet);
                                                                                                        RiverTran.CurrentEdge = ByteArrayToFloat(packet);
                                                                                                        
                                                                                                        RiverTran.EdgeType[0] = ByteArrayToFloat(packet);
                                                                                                        RiverTran.EdgeDistance[0] = ByteArrayToFloat(packet);
                                                                                                        RiverTran.EdgeEnsembles[0] = ByteArrayToFloat(packet);
                                                                                                        RiverTran.EdgeStatus[0] = ByteArrayToFloat(packet);
                                                                                                        
                                                                                                        RiverTran.EdgeType[1] = ByteArrayToFloat(packet);
                                                                                                        RiverTran.EdgeDistance[1] = ByteArrayToFloat(packet);
                                                                                                        RiverTran.EdgeEnsembles[1] = ByteArrayToFloat(packet);
                                                                                                        RiverTran.EdgeStatus[1] = ByteArrayToFloat(packet);
                                                                                                    }
                                                                                                }

                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //correct for changes in array length
                SizeCount = PacketPointer - SizeCount;

                if (SizeCount != ArrayCount)
                {
                    PacketPointer += (ArrayCount - SizeCount);
                    if (PacketPointer < 0)
                        PacketPointer = 2 * PacketSize;
                }

                if (PacketPointer + 4 >= PacketSize)
                    break;//no more data
            }
            //nArray = i;
            if (m.E_Cells < 1)
                m.E_Cells = 0;

            BottomStats(m);
        }
        
        void FlushText()
        {
            int i = 0;
            while (DataBuff[DataBuffReadIndex] < 128 && DataBuffReadIndex != DataBuffWriteIndex)
            {
                bBuff[i++] = DataBuff[DataBuffReadIndex];
                DataBuffReadIndex++;
                if (DataBuffReadIndex > MaxDataBuff)
                    DataBuffReadIndex = 0;
            }
            if (i > 0)
            {
                bBuff[i] = 0;
                string message = System.Text.ASCIIEncoding.ASCII.GetString(bBuff);
                WriteMessageTxtSerial(message, false);
            }
        }

        const int MagN = 180;

        public int NextVal = 0;
        public double[] BXdata = new double[2000];
        public double[] BYdata = new double[2000];
        public double[] BZdata = new double[2000];
        public double[] WXdata = new double[2000];
        public double[] WYdata = new double[2000];
        public double[] WZdata = new double[2000];
        public double[] BTDdata = new double[2000];
        public double[] BTTdata = new double[2000];
        public double[] WTDdata = new double[2000];
        public double[] WTTdata = new double[2000];

        public double[] BXdataMT = new double[2000];
        public double[] BYdataMT = new double[2000];
        public double[] BZdataMT = new double[2000];
        public double[] WXdataMT = new double[2000];
        public double[] WYdataMT = new double[2000];
        public double[] WZdataMT = new double[2000];
        public double[] BTDdataMT = new double[2000];
        public double[] BTTdataMT = new double[2000];
        public double[] WTDdataMT = new double[2000];
        public double[] WTTdataMT = new double[2000];

        //NMEA-----------------------------------------------------------------
        private int NmeaDecodeState = 0;
        private const int GotNMEAheader = 1;
        private const int GotNMEAstar   = 2;
        //private const int GotNMEAcsum   = 3;
        private const int GotColon      =  4;

        //private const int GPHDT = 2;
        //private const int GPVTG = 3;
        //private const int HEROT = 4;
        //private const int GPGSV = 5;
        //private const int GPZDA = 6;
        //private const int GPGGA = 7;

        //private const int PRTI01 = 01;
        //private const int PRTI02 = 02;
        //private const int PRTI11 = 11;
        //private const int PRTI12 = 12;
        //private const int PRTI21 = 21;

        //private const int DVLNAV = 30;
        
        bool [] FirstGGA = new bool[csubs];//true;
        bool FirstRTI01 = true;
        bool Navigate = false;
        const int NMEAmaxBytes = 300;
        private byte[] NMEAstr = new byte[NMEAmaxBytes];
        private int NMEAbyteCount = 0;
        private byte[] PD13buff = new byte[10000];

        public struct NavStruct
        {
            public double SOG;
            public double COG;
            public double CMG;
            public double DMG;
            public double TotalDist;
        }
        public struct GPGGAstruct
        {
            //$GPGGA,hhmmss.ss,llll.ll,a,yyyyy.yy,a,x,xx,x.x,x.x,M,x.x,M,x.x,xxxx*hh

            //GGA  = Global Positioning System Fix Data

            //1    = UTC of Position
            //2    = Latitude
            //3    = N or S
            //4    = Longitude
            //5    = E or W
            //6    = GPS quality indicator (0=invalid; 1=GPS fix; 2=Diff. GPS fix)
            //7    = Number of satellites in use [not those in view]
            //8    = Horizontal dilution of position
            //9    = Antenna altitude above/below mean sea level (geoid)
            //10   = Meters  (Antenna height unit)
            //11   = Geoidal separation (Diff. between WGS-84 earth ellipsoid and
            //       mean sea level.  -=geoid is below WGS-84 ellipsoid)
            //12   = Meters  (Units of geoidal separation)
            //13   = Age in seconds since last update from diff. reference station
            //14   = Diff. reference station ID#
            //15   = Checksum
            public double utc;
            public double lat;
            public double lon;
            public double latrad;
            public double lonrad;
            public int quality;
            public int SatsInUse;
            public double Hdilution;
            public double AntAlt;
            public char AltUnits;
            public double GeoidalSeparation;
            public char GeoUnits;
            public double DiffRefAge;
            public long RefStaID;
            public long SampleNumber;
        }
        public struct GPHDTstruct
        {
            //$--HDT,x.x,T
            //x.x = Heading, degrees True 

            public double Heading;
            public char Reference;
            public long SampleNumber;
        }
        public struct GPVTGstruct
        {
            //$--VTG,x.x,T,x.x,M,x.x,N,x.x,K
            //x.x,T = Track, degrees True 
            //x.x,M = Track, degrees Magnetic 
            //x.x,N = Speed, knots 
            //x.x,K = Speed, Km/hr 
            public double TrackTrue;
            public char TrackTrueIndicator;
            public double TrackMagnetic;
            public char TrackMagneticIndicator;
            public double SpeedKnots;
            public char SpeedKnotsIndicator;
            public double SpeedKPH;
            public char SpeedKPHIndicator;
            public long SampleNumber;
        }

        public struct RTI01struct
        {
            //sssssshh = Start time of this sample in hundreds of seconds since power up or user reset.  
            //nnnnnn = Sample number 
            // TTTT = Temperature in hundreds of degrees Celsius.
            // XXXX = Bottom track X velocity component mm/s.
            // YYYY = Bottom track Y velocity component mm/s.
            // ZZZZ = Bottom track Z velocity component mm/s.
            // DDDD = Depth below transducer in mm. 
            //xxxx =   Water mass X velocity component mm/s. 
            //yyyy = Water mass Y velocity component mm/s.
            //zzzz = Water mass Z velocity component mm/s.
            //dddd = Depth of water mass measurement in mm.
            //ABCD = Built in test and status bits in hexadecimal (0000 = OK).
            public bool BtOK;
            public bool WtOK;

            public long SampleNumber;
            public double TimeSeconds;
            public long EnsembleNumber;
            public double Temperature;

            public double BottomVelocityX;
            public double BottomVelocityY;
            public double BottomVelocityZ;
            public double BottomDepth;

            public double WaterVelocityX;
            public double WaterVelocityY;
            public double WaterVelocityZ;
            public double WaterDepth;

            public string Status;
        }
        public struct RTI10struct
        {
            //sssssshh = Start time of this sample in hundreds of seconds since power up or user reset.  
            //nnnnnn = Sample number 
            // TTTT = Temperature in hundreds of degrees Celsius.
            // XXXX = Bottom track X velocity component mm/s.
            // YYYY = Bottom track Y velocity component mm/s.
            // ZZZZ = Bottom track Z velocity component mm/s.
            // DDDD = Depth below transducer in mm. 
            //xxxx =   Water mass X velocity component mm/s. 
            //yyyy = Water mass Y velocity component mm/s.
            //zzzz = Water mass Z velocity component mm/s.
            //dddd = Depth of water mass measurement in mm.
            //ABCD = Built in test and status bits in hexadecimal (0000 = OK).
            public bool BtOK;

            public long SampleNumber;
            public double TimeSeconds;
            public long PingNumber;
            public double Temperature;

            public double BottomVelocityX;
            public double BottomVelocityY;
            public double BottomVelocityZ;

            public double BottomDepth;

            public long BeamSet;
            public string Status;
        }
        public struct RTI20struct
        {
            public bool WtOK;

            public long SampleNumber;
            public double TimeSeconds;
            public long PingNumber;
            public double Temperature;

            public double WaterVelocityX;
            public double WaterVelocityY;
            public double WaterVelocityZ;

            public double WaterDepth;

            public long BeamSet;
            public string Status;
        }

        public struct RTIACCstruct
        {
            public long N;
            public double Time;

            public double BX;
            public double BY;
            public double BZ;
            public double BE;
            public double BN;

            public double BCMG;
            public double BDMG;

            public double WX;
            public double WY;
            public double WZ;
            public double WE;
            public double WN;

            public double WCMG;
            public double WDMG;
        }

        public GPVTGstruct[] VTG = new GPVTGstruct[csubs];
        public GPHDTstruct[] HDT = new GPHDTstruct[csubs];
        public NavStruct[] GGANAV = new NavStruct[csubs];
        public GPGGAstruct[] GGA = new GPGGAstruct[csubs];
        public GPGGAstruct[] GGAFirst = new GPGGAstruct[csubs];
        public GPGGAstruct[] GGALast = new GPGGAstruct[csubs];

        public RTI01struct RTI01 = new RTI01struct();
        public RTI01struct RTI01Last = new RTI01struct();

        //public RTI10struct RTI10 = new RTI10struct();
        //public RTI10struct RTI10Last = new RTI10struct();
        //public RTI20struct RTI20 = new RTI20struct();
        //public RTI20struct RTI20Last = new RTI20struct();

        public RTIACCstruct RTIACC = new RTIACCstruct();
        public NavStruct RTIBNAV = new NavStruct();
        public NavStruct RTIWNAV = new NavStruct();

        public GPVTGstruct VTGMT = new GPVTGstruct();
        public GPHDTstruct HDTMT = new GPHDTstruct();
        public NavStruct GGANAVMT = new NavStruct();
        public GPGGAstruct GGAMT = new GPGGAstruct();
        public GPGGAstruct GGAFirstMT = new GPGGAstruct();
        public GPGGAstruct GGALastMT = new GPGGAstruct();
        public RTI01struct RTI01MT = new RTI01struct();
        public RTI01struct RTI01LastMT = new RTI01struct();
        public RTIACCstruct RTIACCMT = new RTIACCstruct();
        public NavStruct RTIBNAVMT = new NavStruct();
        public NavStruct RTIWNAVMT = new NavStruct();
        public bool DecodeGPGGA(string str, int cs)
        {
            double d;
            char c;
            int i;
            long l;

            string s = str;

            s = s.Replace("*", ",\0");
            string[] words = s.Split(',');


            if (words[0] == "$GPGGA" && words.Length > 14)
            {

                GGA[cs].SampleNumber++;
                //utc
                bool canConvert = double.TryParse(words[1], out d);
                if (canConvert == true)
                {
                    GGA[cs].utc = d;
                }
                //lat
                canConvert = double.TryParse(words[2], out d);
                if (canConvert == true)
                {
                    canConvert = char.TryParse(words[3], out c);
                    if (canConvert == true)
                    {
                        GGA[cs].lat = (int)(d / 100);
                        d -= 100 * GGA[cs].lat;
                        GGA[cs].lat += d / 60;
                        if (c == 'S')
                            GGA[cs].lat = -GGA[cs].lat;
                        GGA[cs].latrad = GGA[cs].lat / 180.0 * Math.PI;
                    }
                }
                //lon
                canConvert = double.TryParse(words[4], out d);
                if (canConvert == true)
                {
                    canConvert = char.TryParse(words[5], out c);
                    if (canConvert == true)
                    {
                        GGA[cs].lon = (int)(d / 100);
                        d -= 100 * GGA[cs].lon;
                        GGA[cs].lon += d / 60;
                        if (c == 'W')
                            GGA[cs].lon = -GGA[cs].lon;
                        GGA[cs].lonrad = GGA[cs].lon / 180.0 * Math.PI;
                    }
                }
                //qua
                canConvert = int.TryParse(words[6], out i);
                if (canConvert == true)
                {
                    GGA[cs].quality = i;
                }
                //sats
                canConvert = int.TryParse(words[7], out i);
                if (canConvert == true)
                {
                    GGA[cs].SatsInUse = i;
                }
                //hdil
                canConvert = double.TryParse(words[8], out d);
                if (canConvert == true)
                {
                    GGA[cs].Hdilution = d;
                }
                //antAlt
                canConvert = double.TryParse(words[9], out d);
                if (canConvert == true)
                {
                    GGA[cs].AntAlt = d;

                    canConvert = char.TryParse(words[10], out c);
                    if (canConvert == true)
                    {
                        GGA[cs].AltUnits = c;
                    }
                }
                //GeoidalSeparation
                canConvert = double.TryParse(words[11], out d);
                if (canConvert == true)
                {
                    GGA[cs].GeoidalSeparation = d;

                    canConvert = char.TryParse(words[12], out c);
                    if (canConvert == true)
                    {
                        GGA[cs].GeoUnits = c;
                    }
                }
                //DiffRefAge
                canConvert = double.TryParse(words[13], out d);
                if (canConvert == true)
                {
                    GGA[cs].DiffRefAge = d;
                }
                //RefStaID
                canConvert = long.TryParse(words[14], out l);
                if (canConvert == true)
                {
                    GGA[cs].RefStaID = l;
                }
                return true;
            }
            return false;
        }
        bool VTGSpeedKPHFresh = false;
        float VTGspeed = 0;
        public bool DecodeGPVTG(string str, int cs)
        {
            //$--VTG,x.x,T,x.x,M,x.x,N,x.x,K
            //x.x,T = Track, degrees True 
            //x.x,M = Track, degrees Magnetic 
            //x.x,N = Speed, knots 
            //x.x,K = Speed, Km/hr 
            
            //NMEA-0183 message: VTG
            //Track made good and speed over ground
            //An example of the VTG message string is: $GPVTG,,T,,M,0.00,N,0.00,K*4E
            //VTG message fields
            //Field Meaning
            //    0 Message ID $GPVTG 
            //    1 Track made good (degrees true) 
            //    2 T: track made good is relative to true north 
            //    3 Track made good (degrees magnetic) 
            //    4 M: track made good is relative to magnetic north 
            //    5 Speed, in knots 
            //    6 N: speed is measured in knots 
            //    7 Speed over ground in kilometers/hour (kph) 
            //    8 K: speed over ground is measured in kph 
            //    9 The checksum data, always begins with * 
            
            double d;
            char c;

            string s = str;

            s = s.Replace("*", ",\0");
            string[] words = s.Split(',');

            if (words[0] == "$GPVTG" && words.Length > 8)
            {
                VTG[cs].SampleNumber++;

                bool canConvert = double.TryParse(words[1], out d);
                if (canConvert == true)
                {
                    VTG[cs].TrackTrue = d;
                }
                canConvert = char.TryParse(words[2], out c);
                if (canConvert == true)
                {
                    VTG[cs].TrackTrueIndicator = c;
                }

                canConvert = double.TryParse(words[3], out d);
                if (canConvert == true)
                {
                    VTG[cs].TrackMagnetic = d;
                }
                canConvert = char.TryParse(words[4], out c);
                if (canConvert == true)
                {
                    VTG[cs].TrackMagneticIndicator = c;
                }

                canConvert = double.TryParse(words[5], out d);
                if (canConvert == true)
                {
                    VTG[cs].SpeedKnots = d;
                }
                canConvert = char.TryParse(words[6], out c);
                if (canConvert == true)
                {
                    VTG[cs].SpeedKnotsIndicator = c;
                }

                canConvert = double.TryParse(words[7], out d);
                if (canConvert == true)
                {
                    VTGSpeedKPHFresh = true;
                    VTG[cs].SpeedKPH = d;
                }
                canConvert = char.TryParse(words[8], out c);
                if (canConvert == true)
                {
                    VTG[cs].SpeedKPHIndicator = c;
                }


                return true;
            }
            return false;
        }
        public bool DecodeGPHDT(string str,int cs)
        {
            double d;
            char c;

            string s = str;



            s = s.Replace("*", ",\0");
            string[] words = s.Split(',');

            if (words[0] == "$GPHDT" || words[0] == "$HEHDT" && words.Length > 2)
            {
                HDT[cs].SampleNumber++;
                bool canConvert = double.TryParse(words[1], out d);
                if (canConvert == true)
                {
                    HDT[cs].Heading = d;
                }
                canConvert = char.TryParse(words[2], out c);
                if (canConvert == true)
                {
                    HDT[cs].Reference = c;
                }
                return true;
            }
            return false;
        }
        /*public bool DecodeRTI10(string str)
        {  
            double d;
            long l;
            string s = str;

            s = s.Replace("*", ",\0");

            string[] words = s.Split(',');

            if (words[0] == "$PRTI10" && words.Length > 9)
            {   
                RTI10.SampleNumber++;
                bool canConvert = double.TryParse(words[1], out d);
                if (canConvert == true)
                {
                    RTI10.TimeSeconds = d / 100;
                }
                canConvert = long.TryParse(words[2], out l);
                if (canConvert == true)
                {
                    RTI10.PingNumber = l;
                }
                canConvert = double.TryParse(words[3], out d);
                if (canConvert == true)
                {
                    RTI10.Temperature = d / 100;
                }
            
                canConvert = double.TryParse(words[4], out d);
                if (canConvert == true)
                {
                    RTI10.BottomVelocityX = d / 1000;
                }
                canConvert = double.TryParse(words[5], out d);
                if (canConvert == true)
                {
                    RTI10.BottomVelocityY = d / 1000;
                }
                canConvert = double.TryParse(words[6], out d);
                if (canConvert == true)
                {
                    RTI10.BottomVelocityZ = d / 1000;
                }            
                canConvert = double.TryParse(words[7], out d);
                if (canConvert == true)
                {
                    RTI10.BottomDepth = d / 1000;
                }
                canConvert = long.TryParse(words[8], out l);
                if (canConvert == true)
                {
                    RTI10.BeamSet = l;
                }
            
                RTI10.Status = words[9];
            
                return true;
            }
            return false;
        }*/
        public bool DecodeRTI01(string str)
        {
            double d;
            long l;
            string s = str;

            s = s.Replace("*", ",\0");
            //$PRTI01,92650,1072,2680,-99999,-99999,-99999,0,-1313,-1184,-649,12000,0008*2B

            string[] words = s.Split(',');

            if (words[0] == "$PRTI01" && words.Length > 13)
            {
                RTI01.SampleNumber++;
                bool canConvert = double.TryParse(words[1], out d);
                if (canConvert == true)
                {
                    RTI01.TimeSeconds = d / 100;
                }
                canConvert = long.TryParse(words[2], out l);
                if (canConvert == true)
                {
                    RTI01.EnsembleNumber = l;
                }
                canConvert = double.TryParse(words[3], out d);
                if (canConvert == true)
                {
                    RTI01.Temperature = d / 100;
                }

                canConvert = double.TryParse(words[4], out d);
                if (canConvert == true)
                {
                    RTI01.BottomVelocityX = d / 1000;
                }
                canConvert = double.TryParse(words[5], out d);
                if (canConvert == true)
                {
                    RTI01.BottomVelocityY = d / 1000;
                }
                canConvert = double.TryParse(words[6], out d);
                if (canConvert == true)
                {
                    RTI01.BottomVelocityZ = d / 1000;
                }
                canConvert = double.TryParse(words[7], out d);
                if (canConvert == true)
                {
                    RTI01.BottomDepth = d / 1000;
                }
                canConvert = double.TryParse(words[8], out d);
                if (canConvert == true)
                {
                    RTI01.WaterVelocityX = d / 1000;
                }
                canConvert = double.TryParse(words[9], out d);
                if (canConvert == true)
                {
                    RTI01.WaterVelocityY = d / 1000;
                }
                canConvert = double.TryParse(words[10], out d);
                if (canConvert == true)
                {
                    RTI01.WaterVelocityZ = d / 1000;
                }
                canConvert = double.TryParse(words[11], out d);
                if (canConvert == true)
                {
                    RTI01.WaterDepth = d / 1000;
                }

                RTI01.Status = words[12];

                return true;
            }
            return false;
        }

        
        public bool DecodeDVLNAV(string str)
        {
            //long ens;
            long l;
            double d;
            string s = str;

            s = s.Replace("*", ",\0");
            //$DVLNAV,<sample#>,<fix type>,<fix quality>,<Vx>,<Vy>,<Vz>,<Xdist>,<Ydist>,<Zdist>,<RNGBM1>,<RNGBM2>,<RNGBM3>,<RNGBM4>*cc
            string[] words = s.Split(',');

            
            long typ;
            long gd;
            double[] V = new double[3];
            double[] D = new double[3];
            double[] R = new double[4];

            if (words[0] == "$DVLNAV" && words.Length > 13)
            {
                int i = 1;
                //bool canConvert = long.TryParse(words[i], out l);
                //if (canConvert == true)
                //{
                //    ens = 1;
                //}
                //else
                //{
                //    ens = -1;
                //}
                i++;
                bool canConvert = long.TryParse(words[i], out l);
                if (canConvert == true)
                {
                    typ = l;
                }
                else
                    typ = -1;
                i++;
                canConvert = long.TryParse(words[i], out l);
                if (canConvert == true)
                {
                    gd = l;
                    if (typ == 0 && gd < 8)
                        textBoxDecoded.Text += str + "\r\n";
                }
                else
                    gd = 0;
                i++;
                for (int j = 0; j < 3; j++)
                {
                    canConvert = double.TryParse(words[i], out d);
                    if (canConvert == true)
                    {
                        V[j] = d;
                    }
                    else
                        V[j] = 88;
                    i++;
                }
                for (int j = 0; j < 3; j++)
                {
                    canConvert = double.TryParse(words[i], out d);
                    if (canConvert == true)
                    {
                        D[j] = d;
                    }
                    else
                        D[j] = 0;
                    i++;
                }
                for (int j = 0; j < 4; j++)
                {
                    canConvert = double.TryParse(words[i], out d);
                    if (canConvert == true)
                    {
                        R[j] = d;
                    }
                    else
                        R[j] = 88;
                    i++;
                }
                str += "\r\n";
                if (typ == 0 && gd < 8)
                    textBoxDecoded.Text += str;

                string DirName = "c:\\RoweTechRiverTools_DVLNAV";
                string FilName = "UK.txt";
                if (typ == 0)
                {                    
                    FilName = "BT.txt";
                }
                if (typ == 1)
                {                    
                    FilName = "WT.txt";
                }

                s += "\r\n";
                SaveTextFile(DirName, FilName, s, true, true);

                return true;
            }
            return false;
        }
        private void WriteMessageTxtDecodeNMEA(string message)
        {
            //string s = textBoxCapturedNMEA.Text + message + Environment.NewLine;
            string s = message + Environment.NewLine;
            textBoxCapturedNMEA.Text += s;

            //this.textBoxCapturedNMEA.SelectionStart = textBoxCapturedNMEA.Text.Length;
            //this.textBoxCapturedNMEA.ScrollToCaret();
        }
        public bool NMEAisValid(string sentence)
        {
            // Returns True if a sentence's checksum matches the 
            // calculated checksum
            // Compare the characters after the asterisk to the calculation

            string sub = "  ";
            string act = " ";

            try
            {
                sub = sentence.Substring(sentence.IndexOf("*") + 1, 2);                
                act = CalculateNMEAChecksum(sentence);
            }
            catch { }

            if (sub == act)
                return true;
            else
                return false;
        }
        public void ShowDecoded(int cs)
        {
            string s = "";

            s += "HDTSam = " + HDT[cs].SampleNumber.ToString();
            s += ", hdg = " + HDT[cs].Heading.ToString("000.00");

            s += "\r\nGGASam = " + GGA[cs].SampleNumber.ToString();
            s += ", UTC = " + GGA[cs].utc.ToString("0.00");
            s += " lat = " + GGA[cs].lat.ToString("000.000000");
            //s += ", " + GGA[cs].latrad.ToString("000.000000");
            s += " lon = " + GGA[cs].lon.ToString("000.000000");
            //s += ", " + GGA[cs].lonrad.ToString("000.000000");
            s += "\r\nqua = " + GGA[cs].quality.ToString();
            s += ",sat = " + GGA[cs].SatsInUse.ToString();
            s += ",hdi = " + GGA[cs].Hdilution.ToString();
            s += ",alt = " + GGA[cs].AntAlt.ToString();
            s += GGA[cs].AltUnits;
            s += ",geo = " + GGA[cs].GeoidalSeparation.ToString();
            s += GGA[cs].GeoUnits;
            s += ",age = " + GGA[cs].DiffRefAge.ToString();
            s += ",rid = " + GGA[cs].RefStaID.ToString();

            s += "\r\nVTGSam = " + VTG[cs].SampleNumber.ToString();
            s += " tra = " + VTG[cs].TrackTrue.ToString("000.00");
            s += ",spd = " + (VTG[cs].SpeedKPH * 1000 / 3600).ToString("00.00") + " m/s";
            s += "," + VTG[cs].SpeedKPH.ToString("00.00") + " kph";
            s += "," + VTG[cs].SpeedKnots.ToString("00.00") + " knots";
            s += "," + (VTG[cs].SpeedKPH * 0.62137119224).ToString("00.00") + " mph";

            s += "\r\nRTISam = " + RTI01.SampleNumber.ToString();
            s += " time = " + RTI01.TimeSeconds.ToString("0.00");
            s += " ping = " + RTI01.EnsembleNumber.ToString();
            s += " temp = " + RTI01.Temperature.ToString("0.00");
            s += " Stat = " + RTI01.Status;
            s += "\r\n  BtVX = " + RTI01.BottomVelocityX.ToString("0.000");
            s += ", Y = " + RTI01.BottomVelocityY.ToString("0.000");
            s += ", Z = " + RTI01.BottomVelocityZ.ToString("0.000");
            s += ", D = " + RTI01.BottomDepth.ToString("0.000");
            s += "\r\n  WtVX = " + RTI01.WaterVelocityX.ToString("0.000");
            s += ", Y = " + RTI01.WaterVelocityY.ToString("0.000");
            s += ", Z = " + RTI01.WaterVelocityZ.ToString("0.000");
            s += ", D = " + RTI01.WaterDepth.ToString("0.000");            

            textBoxDecoded.Text = s;
        }

        public void ClearStructures()
        {   
            for (int i = 0; i < csubs; i++)
            {
                VTG[i] = VTGMT;
                HDT[i] = HDTMT;
                GGANAV[i] = GGANAVMT;
                GGAFirst[i] = GGAFirstMT;
                GGALast[i] = GGALastMT;
                GGA[i] = GGAMT;
                FreshGGA[i] = false;
            }            
            
            RTI01 = RTI01MT;
            RTI01Last = RTI01LastMT;

            RTIACC = RTIACCMT;
            RTIBNAV = RTIBNAVMT;
            RTIWNAV = RTIWNAVMT;

            BXdata = BXdataMT;
            BYdata = BYdataMT;
            BZdata = BZdataMT;
            WXdata = WXdataMT;
            WYdata = WYdataMT;
            WZdata = WZdataMT;

            BTDdata = BTDdataMT;
            BTTdata = BTTdataMT;

            WTDdata = WTDdataMT;
            WTTdata = WTTdataMT;

            NextVal = 0;
        }
        public string CalculateNMEAChecksum(string sentence)
        {
            // Loop through all chars to get a checksum
            int Checksum = 0;
            //bool firstcs = true;
            foreach (char Character in sentence)
            {
                if (Character == '$')
                {
                    // Ignore the dollar sign
                }
                else if (Character == '*')
                {
                    // Stop processing before the asterisk
                    break;
                }
                else
                {
                    // Is this the first value for the checksum?
                    /*
                    if (firstcs)
                    {
                        // Yes. Set the checksum to the value
                        Checksum = Convert.ToByte(Character);
                        firstcs = false;
                    }
                    else*/
                    {
                        // No. XOR the checksum with this character's value
                        //Checksum = (Checksum ^ Convert.ToByte(Character));
                        Checksum ^= Convert.ToByte(Character);
                    }
                }
            }
            // Return the checksum formatted as a two-character hexadecimal
            return Checksum.ToString("X2");
        }
        bool[] NewGGA = new bool[csubs];
        bool[] FreshGGA = new bool[csubs];
        bool[] FreshBT = new bool[csubs];
        bool[] bFreshBT = new bool[csubs];
        bool[] FirstAllNav = new bool[csubs];
        void doGGANav(int cs)
        {
            if (FreshGGA[cs])
            {
                if (FirstGGA[cs])
                {
                    GGAFirst[cs] = GGA[cs];
                    GGALast[cs] = GGA[cs];
                }
                
                CalculateGGANavigation(cs);
                
                ShowDecoded(cs);
                ShowNavigation(cs);

                if (FirstGGA[cs])
                {
                    GGAFirst[cs] = GGA[cs];
                    FirstGGA[cs] = false;
                }
                NewGGA[cs] = true;
                GGALast[cs] = GGA[cs];
                FreshGGA[cs] = false;
            }
            else
                NewGGA[cs] = false;
        }

        int PD13index = 0;
        private void DecodeNmea(int cs, byte[] DataBuff, int DataBuffWriteIndex, bool RecordOn, bool PD13 )
        {
            int stay = 1;
            int i;
            while (stay == 1)
            {
                int ByteCount = DataBuffWriteIndex - NmeaBuffReadIndex;
                if (ByteCount < 0)
                    ByteCount += MaxDataBuff;

                /*
                if (!checkBoxNMEA_ASCII_Input.Checked)
                {
                    if (ByteCount > 1000)
                    {
                        NmeaBuffReadIndex += 900;
                        if (NmeaBuffReadIndex >= MaxDataBuff)
                            NmeaBuffReadIndex = NmeaBuffReadIndex - MaxDataBuff;

                        ByteCount = DataBuffWriteIndex - NmeaBuffReadIndex;
                        if (ByteCount < 0)
                            ByteCount += MaxDataBuff;
                    }
                }
                */

                if (ByteCount <= 0)
                    break;

                switch (NmeaDecodeState)
                {
                    case GotColon:
                        int j1, k1;
                        j1 = PD13index;
                        for (i = 0; i < ByteCount; i++)
                        {
                            PD13buff[PD13index] = DataBuff[NmeaBuffReadIndex];
                            
                            if (PD13buff[PD13index] == 13)//got a possible string
                            {
                                k1 = PD13index;
                                if (((k1 - j1 > 32) && (k1 - j1 < 35)) || ((k1 - j1 > 56) && (k1 - j1 < 59)))//WI or WD or BI or BD
                                {
                                    string PD13str = (new ASCIIEncoding()).GetString(PD13buff, j1, k1 - j1);
                                    
                                    PD13str += "\r\n";

                                    if (PD13str.Substring(0, 3) == ":WI")
                                        textBoxCapturedNMEA.Text = "";
                                    //if (PD13str.Substring(0, 3) == ":BI")
                                    //    textBoxCapturedNMEA.Text = "";

                                    textBoxCapturedNMEA.Text += PD13str;
                                    
                                    if (RecordOn)
                                    {
                                        string DirName = "c:\\RoweTechRiverTools_PD13";
                                        string FilName = "";
                                        if (PD13str.IndexOf(":WI") >= 0)
                                        {
                                            FilName = "WI.txt";
                                        }
                                        else
                                        {
                                            if (PD13str.IndexOf(":WD") >= 0)
                                            {
                                                FilName = "WD.txt";
                                            }
                                            else
                                            {
                                                if (PD13str.IndexOf(":BI") >= 0)
                                                {
                                                    FilName = "BI.txt";
                                                }
                                                else
                                                {
                                                    if (PD13str.IndexOf(":BD") >= 0)
                                                    {
                                                        FilName = "BD.txt";
                                                    }
                                                }
                                            }
                                        }
                                        if (FilName.Length > 0)
                                        {                                            
                                            SaveTextFile(DirName, FilName, PD13str, true, true);
                                            
                                        }
                                    }                             
                                }
                                NmeaDecodeState = 0;
                                break;
                            }
                            NmeaBuffReadIndex++;
                            if (NmeaBuffReadIndex > MaxDataBuff)
                                NmeaBuffReadIndex = 0;// NmeaBuffReadIndex - MaxDataBuff;
                            
                            PD13index++;
                        }
                        if (i < ByteCount)
                            stay = 1;
                        else
                        {
                            stay = 0;//to exit
                        }
                        break;
                    default:
                        NMEAbyteCount = 0;
                       
                        if (DataBuff[NmeaBuffReadIndex] == 58)// :
                        {
                            NmeaDecodeState = GotColon;
                            PD13index = 0;
                        }
                        else
                        if (!PD13 && DataBuff[NmeaBuffReadIndex] == 36)// $
                        {
                            if (ByteCount > 7)
                            {// binary data
                                int TempReadIndex = NmeaBuffReadIndex;
                                byte[] DBuff = new byte[7];
                                for (i = 0; i < 7; i++)
                                {
                                    DBuff[i] = DataBuff[TempReadIndex];
                                    TempReadIndex++;
                                    if (TempReadIndex > MaxDataBuff)
                                        TempReadIndex = 0;
                                }
                                // NMEA string
                                if ((DBuff[0] == '$') &&
                                    (DBuff[1] == 'G') &&
                                    (DBuff[2] == 'P') &&
                                    (DBuff[3] == 'G') &&
                                    (DBuff[4] == 'G') &&
                                    (DBuff[5] == 'A'))
                                {
                                    //NMEAstrType = GPGGA;
                                    NMEAbyteCount = 6;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else
                                    if ((DBuff[0] == '$') &&
                                        (DBuff[1] == 'P') &&
                                        (DBuff[2] == 'R') &&
                                        (DBuff[3] == 'T') &&
                                        (DBuff[4] == 'I') &&
                                        (DBuff[5] == '0') &&
                                        (DBuff[6] == '1'))
                                {
                                    //NMEAstrType = PRTI01;
                                    NMEAbyteCount = 7;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else
                                        if ((DBuff[0] == '$') &&
                                            (DBuff[1] == 'P') &&
                                            (DBuff[2] == 'R') &&
                                            (DBuff[3] == 'T') &&
                                            (DBuff[4] == 'I') &&
                                            (DBuff[5] == '0') &&
                                            (DBuff[6] == '2'))
                                {
                                    //NMEAstrType = PRTI02;
                                    NMEAbyteCount = 7;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else
                                            if ((DBuff[0] == '$') &&
                                                (DBuff[1] == 'P') &&
                                                (DBuff[2] == 'R') &&
                                                (DBuff[3] == 'T') &&
                                                (DBuff[4] == 'I') &&
                                                (DBuff[5] == '1') &&
                                                (DBuff[6] == '1'))
                                {
                                    //NMEAstrType = PRTI11;
                                    NMEAbyteCount = 7;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else
                                                if ((DBuff[0] == '$') &&
                                                    (DBuff[1] == 'P') &&
                                                    (DBuff[2] == 'R') &&
                                                    (DBuff[3] == 'T') &&
                                                    (DBuff[4] == 'I') &&
                                                    (DBuff[5] == '1') &&
                                                    (DBuff[6] == '2'))
                                {
                                    //NMEAstrType = PRTI12;
                                    NMEAbyteCount = 7;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else
                                                    if ((DBuff[0] == '$') &&
                                                        (DBuff[1] == 'P') &&
                                                        (DBuff[2] == 'R') &&
                                                        (DBuff[3] == 'T') &&
                                                        (DBuff[4] == 'I') &&
                                                        (DBuff[5] == '2') &&
                                                        (DBuff[6] == '1'))
                                {
                                    //NMEAstrType = PRTI21;
                                    NMEAbyteCount = 7;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else
                                                        if ((DBuff[0] == '$') &&
                                                            (DBuff[3] == 'H') &&
                                                            (DBuff[4] == 'D') &&
                                                            (DBuff[5] == 'T'))
                                {
                                    //NMEAstrType = GPHDT;
                                    NMEAbyteCount = 6;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else
                                                            if ((DBuff[0] == '$') &&
                                                                (DBuff[1] == 'G') &&
                                                                (DBuff[2] == 'P') &&
                                                                (DBuff[3] == 'V') &&
                                                                (DBuff[4] == 'T') &&
                                                                (DBuff[5] == 'G'))
                                {
                                    //NMEAstrType = GPVTG;
                                    NMEAbyteCount = 6;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else
                                                                if ((DBuff[0] == '$') &&
                                                                    (DBuff[1] == 'H') &&
                                                                    (DBuff[2] == 'E') &&
                                                                    (DBuff[3] == 'R') &&
                                                                    (DBuff[4] == 'O') &&
                                                                    (DBuff[5] == 'T'))
                                {
                                    //NMEAstrType = HEROT;
                                    NMEAbyteCount = 6;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else
                                                                    if ((DBuff[0] == '$') &&
                                                                        (DBuff[1] == 'G') &&
                                                                        (DBuff[2] == 'P') &&
                                                                        (DBuff[3] == 'G') &&
                                                                        (DBuff[4] == 'S') &&
                                                                        (DBuff[5] == 'V'))
                                {
                                    //NMEAstrType = GPGSV;
                                    NMEAbyteCount = 6;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else
                                                                        if ((DBuff[0] == '$') &&
                                                                            (DBuff[1] == 'G') &&
                                                                            (DBuff[2] == 'P') &&
                                                                            (DBuff[3] == 'Z') &&
                                                                            (DBuff[4] == 'D') &&
                                                                            (DBuff[5] == 'A'))
                                {
                                    //NMEAstrType = GPZDA;
                                    NMEAbyteCount = 6;
                                    NmeaDecodeState = GotNMEAheader;
                                }//$DVLNAV,<sample#>,<fix type>,<fix quality>,<Vx>,<Vy>,<Vz>,<Xdist>,<Ydist>,<RNGBM1>,<RNGBM2>,<RNGBM3>,<RNGBM4>*cc
                                else
                                                                        if ((DBuff[0] == '$') &&
                                                                            (DBuff[1] == 'D') &&
                                                                            (DBuff[2] == 'V') &&
                                                                            (DBuff[3] == 'L') &&
                                                                            (DBuff[4] == 'N') &&
                                                                            (DBuff[5] == 'A') &&
                                                                            (DBuff[6] == 'V'))
                                {
                                    //NMEAstrType = DVLNAV;
                                    NMEAbyteCount = 7;
                                    NmeaDecodeState = GotNMEAheader;
                                }
                                else//point to next byte in the buffer
                                {
                                    //NMEAstrType = 0;
                                    NMEAbyteCount = 1;
                                }
                                for (i = 0; i < NMEAbyteCount; i++)
                                    NMEAstr[i] = DBuff[i];

                                NmeaBuffReadIndex += NMEAbyteCount;
                                if (NmeaBuffReadIndex > MaxDataBuff)
                                    NmeaBuffReadIndex = (NmeaBuffReadIndex - MaxDataBuff - 1);
                            }
                            else
                                stay = 0;
                        }
                        else
                        {
                            stay = 1;
                            NmeaBuffReadIndex++;
                            if (NmeaBuffReadIndex > MaxDataBuff)
                                NmeaBuffReadIndex = 0;// NmeaBuffReadIndex - MaxDataBuff;
                        }
                        break;
                    case GotNMEAheader:
                        for (i = 0; i < ByteCount; i++)
                        {
                            NMEAstr[NMEAbyteCount] = DataBuff[NmeaBuffReadIndex];
                            NmeaBuffReadIndex++;
                            if (NmeaBuffReadIndex > MaxDataBuff)
                                NmeaBuffReadIndex = 0;

                            NMEAbyteCount++;
                            if (NMEAbyteCount > NMEAmaxBytes - 1)
                            {
                                textBoxCapturedNMEA.Text += "\r\nNMEA string length exceeded\r\n";
                                NmeaDecodeState = 0;
                                stay = 0;
                                break;
                            }
                            else
                                if (NMEAstr[NMEAbyteCount - 1] == '*')
                                {
                                    NmeaDecodeState = GotNMEAstar;
                                    break;
                                }
                        }
                        break;
                    case GotNMEAstar:
                        if (ByteCount > 1)
                        {//read in the csum bytes
                            NMEAstr[NMEAbyteCount] = DataBuff[NmeaBuffReadIndex];
                            NmeaBuffReadIndex++;
                            if (NmeaBuffReadIndex > MaxDataBuff)
                                NmeaBuffReadIndex = 0;

                            NMEAbyteCount++;

                            NMEAstr[NMEAbyteCount] = DataBuff[NmeaBuffReadIndex];

                            NmeaBuffReadIndex++;
                            if (NmeaBuffReadIndex > MaxDataBuff)
                                NmeaBuffReadIndex = 0;

                            NMEAbyteCount++;
                            if (NMEAbyteCount > NMEAmaxBytes - 1)
                                NMEAbyteCount = NMEAmaxBytes - 1;

                            NMEAstr[NMEAbyteCount] = 0;
                            string converted = (new ASCIIEncoding()).GetString(NMEAstr, 0, NMEAbyteCount);
                            
                            if (!NMEAisValid(converted))
                            {
                                textBoxCapturedNMEA.Text += converted + "\r\nNMEA checksum failed\r\n";
                            }
                            else
                            {
                                WriteMessageTxtDecodeNMEA(converted);
                                if (Navigate)
                                {
                                    if (converted.IndexOf("GGA") > 0)
                                    {
                                        if (DecodeGPGGA(converted, cs))
                                        {
                                            FreshGGA[cs] = true;
                                            if (checkBoxNMEA_ASCII_Input.Checked)
                                                stay = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (converted.IndexOf("HDT") > 0)//NMEAstrType == GPHDT)
                                        {
                                            if (DecodeGPHDT(converted, cs))
                                            {
                                                ShowDecoded(cs);
                                            }
                                        }
                                        else
                                        {
                                            if (converted.IndexOf("VTG") > 0)//NMEAstrType == GPVTG)
                                            {
                                                if (DecodeGPVTG(converted, cs))
                                                {
                                                    //VTGstr = converted;
                                                    ShowDecoded(cs);
                                                }
                                            }
                                            else
                                            {
                                                if (converted.IndexOf("DVLNAV") > 0)//NMEAstrType == DVLNAV)
                                                {
                                                    DecodeDVLNAV(converted);
                                                }
                                                else
                                                {
                                                    if (converted.IndexOf("PRTI01") > 0)//NMEAstrType == PRTI01)
                                                    {
                                                        if (DecodeRTI01(converted))
                                                        {
                                                            if (FirstRTI01)
                                                            {
                                                                RTI01Last = RTI01;
                                                                FirstRTI01 = false;
                                                            }
                                                            CalculateRTI01Navigation(cs);
                                                            ShowDecoded(cs);
                                                            ShowNavigation(cs);

                                                            BXdata[NextVal] = RTI01.BottomVelocityX;
                                                            BYdata[NextVal] = RTI01.BottomVelocityY;
                                                            BZdata[NextVal] = RTI01.BottomVelocityZ;

                                                            WXdata[NextVal] = RTI01.WaterVelocityX;
                                                            WYdata[NextVal] = RTI01.WaterVelocityY;
                                                            WZdata[NextVal] = RTI01.WaterVelocityZ;

                                                            BTDdata[NextVal] = RTI01.BottomDepth;
                                                            BTTdata[NextVal] = RTI01.Temperature;

                                                            NextVal++;
                                                            if (NextVal >= MagN)
                                                                NextVal = 0;

                                                            if (RTI01.BtOK)
                                                                RTI01Last = RTI01;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            NmeaDecodeState = 0;
                        }
                        else
                            stay = 0;

                        break;
                }
            }
        }
        bool DoNMEA = false;
        long VTGlastens = 0;

        private double GetEnsembleNmeaVTG(ArrayClass m)
        {
            if (m.NmeaAvailable && m.NMEA_Bytes > 0)
            {
                int ByteCount = m.NMEA_Bytes;
                if (ByteCount > 8192)
                    ByteCount = 8192;

                byte[] Buff = new byte[1000];
                int j = 0;
                for (int i = 0; i < ByteCount; i++)
                {
                    if (m.NMEA_Buffer[i] == 36)//$ starts the string
                        j = 0;
                    Buff[j] = m.NMEA_Buffer[i];
                    if (Buff[j] == 42)// * indicates checksum
                    {
                        //move the 2 byte checksum
                        i++;
                        Buff[j + 1] = m.NMEA_Buffer[i];
                        i++;
                        Buff[j + 2] = m.NMEA_Buffer[i];

                        string NMEAstr = (new ASCIIEncoding()).GetString(Buff, 0, j + 3);
                        if (NMEAisValid(NMEAstr))
                        {
                            //WriteMessageTxtDecodeNMEA(NMEAstr);
                            
                            if (NMEAstr.IndexOf("VTG") > 0)
                            {
                                if (DecodeGPVTG(NMEAstr, 0))
                                {
                                    //VTGstr = NMEAstr; VTGstr = NMEAstr;
                                }
                            }
                        }
                        j = 0;//start next message
                    }
                    else
                    {
                        j++;
                        if (j > 1000)
                            j = 0;
                    }
                }            
            }
            return (0.51444444444 * VTG[0].SpeedKnots);
        }

        //string VTGstr = "Hi";
        private void DecodeEnsembleNmea(ArrayClass m,int cs)
        {   
            if (m.NmeaAvailable && m.NMEA_Bytes > 0)
            {   
                int ByteCount = m.NMEA_Bytes;
                if (ByteCount > 8192)
                    ByteCount = 8192;
                
                byte[] Buff = new byte[ByteCount];

                //find PINGBT
                var pattern = new byte[] { (byte)'P', (byte)'I', (byte)'N', (byte)'G', (byte)'B', (byte)'T' };
                int maxFirstCharSlot = ByteCount - pattern.Length + 1;
                
                bool done = false;
                int ii;
                for (ii = 0; ii < maxFirstCharSlot; ii++)
                {
                    if (m.NMEA_Buffer[ii] != pattern[0])
                        continue;
                    // found a match on first byte, now try to match rest of the pattern
                    for (int k = pattern.Length - 1; k >= 1; k--)
                    {
                        if (m.NMEA_Buffer[ii + k] != pattern[k]) 
                            break;
                        if (k == 1)
                        {
                            done = true;
                            break;
                        }
                    }
                    if (done)
                        break;
                }

                int j = 0;
                if (!done)
                    ii = 0;
                for (int i = ii; i < ByteCount; i++)
                {
                    if (m.NMEA_Buffer[i] == 36)//$ starts the string
                        j = 0;
                    Buff[j] = m.NMEA_Buffer[i];
                    if (Buff[j] == 42 && i < ByteCount - 1)
                    {
                        //move the 2 byte checksum
                        i++;
                        Buff[j + 1] = m.NMEA_Buffer[i];
                        i++;
                        Buff[j + 2] = m.NMEA_Buffer[i];
                        
                        string NMEAstr = (new ASCIIEncoding()).GetString(Buff,0,j+3);
                        if (NMEAisValid(NMEAstr))
                        {
                            WriteMessageTxtDecodeNMEA(NMEAstr);
                            if (Navigate)
                            {
                                if (NMEAstr.IndexOf("GGA") > 0)
                                {
                                    if (DecodeGPGGA(NMEAstr, cs))
                                    {
                                        FreshGGA[cs] = true;
                                    }
                                }
                                else
                                {
                                    if (NMEAstr.IndexOf("HDT") > 0)
                                    {
                                        if (DecodeGPHDT(NMEAstr, cs))
                                        {
                                            ShowDecoded(cs);
                                        }
                                    }
                                    else
                                    {
                                        if (NMEAstr.IndexOf("VTG") > 0)
                                        {
                                            if (DecodeGPVTG(NMEAstr, cs))
                                            {
                                                //VTGstr = NMEAstr;

                                                ShowDecoded(cs);
                                                if(DoNMEA)
                                                {   
                                                    if (VTGlastens != Arr.E_EnsembleNumber)
                                                    {
                                                        string DN = "c:\\RoweTechRiverTools_VTG";
                                                        string FN = "VTG.txt";
                                                        string ss = (VTG[cs].SpeedKPH).ToString() + "\r\n";

                                                        VTGlastens = Arr.E_EnsembleNumber;
                                                        ss = VTGlastens.ToString() + "," + ss;
                                                        SaveTextFile(DN, FN, ss, true, true);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        j = 0;//start next message
                    }
                    else
                    {
                        j++;
                        if (j > 1000)
                            j = 0;
                    }
                }
            }
        }
        bool NMEAEnableDecode = true;        
        /*
        private void WriteMessageTxtNmea(string message, bool linefeed)
        {
            txtSerial.Text += message;

            if (linefeed)
                txtSerial.Text += Environment.NewLine;

            if (txtSerial.Text.Length > 16000)
                txtSerial.Text = txtSerial.Text.Remove(0, txtSerial.Text.Length - 6000);

            txtSerial.SelectionStart = txtSerial.Text.Length;
            txtSerial.ScrollToCaret();

        }
        */
        //Navigate-------------------------------------------------------------
        public void ClearNavigation()
        {
            Navigate = true;
            
            FirstRTI01 = true;
            this.textBoxCapturedNMEA.Clear();
            //this.textBoxSerialRTI.Clear();
            //this.textBoxSerialNMEA.Clear();
            
            RTI01.SampleNumber = 0;

            RTIACC.N = 0;
            RTIACC.Time = 0;

            RTIACC.BX = 0;
            RTIACC.BY = 0;
            RTIACC.BZ = 0;
            RTIACC.WX = 0;
            RTIACC.WY = 0;
            RTIACC.WZ = 0;

            RTIACC.BE = 0;
            RTIACC.BN = 0;

            RTIACC.WE = 0;
            RTIACC.WN = 0;

            RTIBNAV.CMG = 0;
            RTIBNAV.DMG = 0;
            RTIBNAV.SOG = 0;
            RTIBNAV.COG = 0;

            RTIWNAV.CMG = 0;
            RTIWNAV.DMG = 0;
            RTIWNAV.SOG = 0;
            RTIWNAV.COG = 0;
            for (int i = 0; i < csubs; i++)
            {
                GGA[i].SampleNumber = 0;
                VTG[i].SampleNumber = 0;
                HDT[i].SampleNumber = 0;
                GGANAV[i].CMG = 0;
                GGANAV[i].DMG = 0;
                GGANAV[i].SOG = 0;
                GGANAV[i].COG = 0;
                FirstGGA[i] = true;
            }
        }
        public void ShowNavigation(int cs)
        {
            string s = "";
            s += "TIME = " + RTIACC.Time.ToString("0.00");
            s += ", N = " + RTIACC.N.ToString();
            s += "\r\n BX = " + RTIACC.BX.ToString("0000.000");
            s += ", BY = " + RTIACC.BY.ToString("0000.000");
            s += ", BZ = " + RTIACC.BZ.ToString("0000.000");
            s += "\r\n WX = " + RTIACC.WX.ToString("0000.000");
            s += ", WY = " + RTIACC.WY.ToString("0000.000");
            s += ", WZ = " + RTIACC.WZ.ToString("0000.000");


            s += "\r\n\r\nNAVSOG" + "[" + cs.ToString() + "]" + "=" + GGANAV[cs].SOG.ToString("00.00") + " m/s";
            s += ", " + (GGANAV[cs].SOG * 3600 / 1000).ToString("00.00") + " kph";
            s += ", " + (GGANAV[cs].SOG * 1.9438444925).ToString("00.00") + " knots";
            s += ", " + (GGANAV[cs].SOG * 2.2369362921).ToString("00.00") + " mph";

            s += "\r\n  BSOG" + "[" + cs.ToString() + "]" + "=" + RTIBNAV.SOG.ToString("00.00") + " m/s";
            s += "\r\n  WSOG" + "[" + cs.ToString() + "]" + "=" + RTIWNAV.SOG.ToString("00.00") + " m/s";

            s += "\r\n\r\nNAVCOG" + "[" + cs.ToString() + "]" + "=" + GGANAV[cs].COG.ToString("000.00") + " deg";
            s += "\r\n  BCOG" + "[" + cs.ToString() + "]" + "=" + RTIBNAV.COG.ToString("000.00") + " deg";
            s += "\r\n  WCOG" + "[" + cs.ToString() + "]" + "=" + RTIWNAV.COG.ToString("000.00") + " deg";


            s += "\r\n\r\nNAVDMG" + "[" + cs.ToString() + "]" + "=" + GGANAV[cs].DMG.ToString("0000.00") + " m";
            s += ", NAVCMG" + "[" + cs.ToString() + "]" + "=" + GGANAV[cs].CMG.ToString("000.00") + " deg";
            //s += ", NAVDis = " + GGANAV.TotalDist.ToString("000.00") + " m";
            s += "\r\n  BDMG" + "[" + cs.ToString() + "]" + "=" + RTIBNAV.DMG.ToString("0000.00") + " m";
            s += ",   BCMG" + "[" + cs.ToString() + "]" + "=" + RTIBNAV.CMG.ToString("000.00") + " deg";
            s += "\r\n  WDMG" + "[" + cs.ToString() + "]" + "=" + RTIWNAV.DMG.ToString("0000.00") + " m";
            s += ",   WCMG" + "[" + cs.ToString() + "]" + "=" + RTIWNAV.CMG.ToString("000.00") + " deg";

            textBoxNavigation.Text = s;
        }

        double[,] BTdisBm = new double[4,csubs];
        double[] BTLastTimeBm = new double[csubs];
        double[,] BTlastBm = new double[4,csubs];
        bool[] BTnavFirstBm = new bool[csubs];

        double[] BTdisE = new double[csubs];
        double[] BTdisN = new double[csubs];
        double[] BTdisU = new double[csubs];
        double[] BTdisMag = new double[csubs];
        double[] BTdisDir = new double[csubs];
        double[] BTmag = new double[csubs];
        double[] BTdir = new double[csubs];        

        double[] BTdisX = new double[csubs];
        double[] BTdisY = new double[csubs];
        double[] BTdisZ = new double[csubs];
        double[] BTdisMagI = new double[csubs];
        double[] BTdisDirI = new double[csubs];
        double[] BTmagI = new double[csubs];
        double[] BTdirI = new double[csubs];
        double[] BTlastX = new double[csubs];
        double[] BTlastY = new double[csubs];
        double[] BTlastZ = new double[csubs];

        double[] bBTdisX = new double[csubs];
        double[] bBTdisY = new double[csubs];
        double[] bBTdisZ = new double[csubs];
        double[] bBTdisMagI = new double[csubs];
        double[] bBTdisDirI = new double[csubs];
        double[] bBTmagI = new double[csubs];
        double[] bBTdirI = new double[csubs];
        double[] bBTlastX = new double[csubs];
        double[] bBTlastY = new double[csubs];
        double[] bBTlastZ = new double[csubs];
        double[] bBTlastE = new double[csubs];
        double[] bBTlastN = new double[csubs];
        double[] bBTlastU = new double[csubs];

        double[] bBTdisE = new double[csubs];
        double[] bBTdisN = new double[csubs];
        double[] bBTdisU = new double[csubs];
        double[] bBTdisMag = new double[csubs];
        double[] bBTdisDir = new double[csubs];
        double[] bBTmag = new double[csubs];
        double[] bBTdir = new double[csubs];

        double[] BTlastE = new double[csubs];
        double[] BTlastN = new double[csubs];
        double[] BTlastU = new double[csubs];

        double[] BTLastTime = new double[csubs];
        double[] bBTLastTime = new double[csubs];

        double[] WPdisE = new double[csubs];
        double[] WPdisN = new double[csubs];
        double[] WPdisU = new double[csubs];
        double[] WPdisMag = new double[csubs];
        double[] WPdisDir = new double[csubs];
        double[] WPmag = new double[csubs];
        double[] WPdir = new double[csubs];
        double[] WPlastE = new double[csubs];
        double[] WPlastN = new double[csubs];
        double[] WPlastU = new double[csubs];
        double[] WPdisX = new double[csubs];
        double[] WPdisY = new double[csubs];
        double[] WPdisZ = new double[csubs];
        double[] WPdisMagI = new double[csubs];
        double[] WPdisDirI = new double[csubs];
        double[] WPmagI = new double[csubs];
        double[] WPdirI = new double[csubs];
        double[] WPlastX = new double[csubs];
        double[] WPlastY = new double[csubs];
        double[] WPlastZ = new double[csubs];
        double[] WPLastTime = new double[csubs];

        bool[] BTnavFirst = new bool[csubs];
        bool[] bBTnavFirst = new bool[csubs];
        int[] BTnavGood = new int[csubs];
        int[] BTnavN = new int[csubs];
        bool[] WPnavFirst = new bool [csubs];
        int[] WPnavGood = new int[csubs];
        int[] WPnavN = new int[csubs];

        int BTBeamErrorGood = 0;
        int[] BTBeamGood = new int[csubs];
        int BTBeamN = 0;
        double BTBeamErrorSum = 0;
        double BTBeamErrorSumSqr = 0;

        private void buttonBTnavBinPlus_Click(object sender, EventArgs e)
        {
            int WPNAVBIN = Convert.ToInt32(textBoxBTNavBin.Text);
            WPNAVBIN++;
            if (WPNAVBIN > 199)
                WPNAVBIN = 199;
            textBoxBTNavBin.Text = WPNAVBIN.ToString();
            BTnavClr();
        }

        private void buttonBTnavBinMinus_Click(object sender, EventArgs e)
        {
            int WPNAVBIN = Convert.ToInt32(textBoxBTNavBin.Text);
            WPNAVBIN--;
            if (WPNAVBIN < 0)
                WPNAVBIN = 0;
            textBoxBTNavBin.Text = WPNAVBIN.ToString();
            BTnavClr();
        }

        void WaterProfileEnsembleNavigate(ArrayClass m, int cs)
        {
            //if (!FirstGGA[cs])
            {
                double dT;

                int WPNAVBIN = Convert.ToInt32(textBoxBTNavBin.Text);

                if (WPNAVBIN < 0)
                    WPNAVBIN = 0;
                if (WPNAVBIN > m.Bins[0] - 1)
                    WPNAVBIN = m.Bins[0] - 1;

                /* 
                int gd = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (m.Correlation[i, WPNAVBIN] > 0.8 && m.Correlation[i, WPNAVBIN] < 0.95)
                        gd++;
                }
                */

                if (WPnavFirst[cs])
                {
                    WPnavGood[cs] = 0;
                    WPnavN[cs] = 0;

                    WPdisE[cs] = 0.0;
                    WPdisN[cs] = 0.0;
                    WPdisU[cs] = 0.0;
                    WPdir[cs] = 0.0;
                    WPdisMag[cs] = 0.0;
                    WPdisDir[cs] = 0.0;
                    WPmag[cs] = 0.0;

                    WPdisX[cs] = 0.0;
                    WPdisY[cs] = 0.0;
                    WPdisZ[cs] = 0.0;
                    WPdirI[cs] = 0.0;
                    WPdisMagI[cs] = 0.0;
                    WPdisDirI[cs] = 0.0;
                    WPmag[cs] = 0.0;
                    //if (m.EarN[0, WPNAVBIN] >= 1)
                    if (m.XfrmN[0, WPNAVBIN] >= 1)
                    {
                        WPLastTime[cs] = m.A_FirstPingSeconds;
                        WPlastE[cs] = m.Earth[0, WPNAVBIN];
                        WPlastN[cs] = m.Earth[1, WPNAVBIN];
                        WPlastU[cs] = m.Earth[2, WPNAVBIN];

                        WPlastX[cs] = m.Instrument[0, WPNAVBIN];
                        WPlastY[cs] = m.Instrument[1, WPNAVBIN];
                        WPlastZ[cs] = m.Instrument[2, WPNAVBIN];

                        WPnavFirst[cs] = false;
                    }
                }
                else
                {   
                     if (m.EarthAvailable)
                            WPnavN[cs]++;

                    if (m.XfrmN[0, WPNAVBIN] >= 1 && m.XfrmN[1, WPNAVBIN] >= 1 && m.XfrmN[2, WPNAVBIN] >= 1)
                    {
                        WPnavGood[cs]++;

                        dT = m.A_FirstPingSeconds - WPLastTime[cs];

                        WPdisE[cs] += 0.5 * dT * (m.Earth[0, WPNAVBIN] + WPlastE[cs]);
                        WPdisN[cs] += 0.5 * dT * (m.Earth[1, WPNAVBIN] + WPlastN[cs]);
                        WPdisU[cs] += 0.5 * dT * (m.Earth[2, WPNAVBIN] + WPlastU[cs]);

                        double scale = Convert.ToDouble(textBoxBTNavBinScale.Text);

                        WPdisMag[cs] = scale * Math.Sqrt(WPdisE[cs] * WPdisE[cs] + WPdisN[cs] * WPdisN[cs] + UseZ * WPdisU[cs] * WPdisU[cs]);
                        WPdisDir[cs] = Math.Atan2(WPdisE[cs], WPdisN[cs]) * 180.0 / Math.PI;
                        if (WPdisDir[cs] < 0.0)
                            WPdisDir[cs] = 360.0 + WPdisDir[cs];
                        WPlastE[cs] = m.Earth[0, WPNAVBIN];
                        WPlastN[cs] = m.Earth[1, WPNAVBIN];
                        WPlastU[cs] = m.Earth[2, WPNAVBIN];
                        WPLastTime[cs] = m.A_FirstPingSeconds;

                        WPmag[cs] = Math.Sqrt(WPlastE[cs] * WPlastE[cs] + WPlastN[cs] * WPlastN[cs] + UseZ * WPlastU[cs] * WPlastU[cs]);
                        WPdir[cs] = Math.Atan2(WPlastE[cs], WPlastN[cs]) * 180.0 / Math.PI;
                        if (WPdir[cs] < 0.0)
                            WPdir[cs] = 360.0 + WPdir[cs];

                        WPdisX[cs] += 0.5 * dT * (m.Instrument[0, WPNAVBIN] + WPlastX[cs]);
                        WPdisY[cs] += 0.5 * dT * (m.Instrument[1, WPNAVBIN] + WPlastY[cs]);
                        WPdisZ[cs] += 0.5 * dT * (m.Instrument[2, WPNAVBIN] + WPlastZ[cs]);

                        WPdisMagI[cs] = scale * Math.Sqrt(WPdisX[cs] * WPdisX[cs] + WPdisY[cs] * WPdisY[cs] + UseZ * WPdisZ[cs] * WPdisZ[cs]);
                        WPdisDirI[cs] = Math.Atan2(WPdisX[cs], WPdisY[cs]) * 180.0 / Math.PI;
                        if (WPdisDirI[cs] < 0.0)
                            WPdisDirI[cs] = 360.0 + WPdisDirI[cs];
                        WPlastX[cs] = m.Instrument[0, WPNAVBIN];
                        WPlastY[cs] = m.Instrument[1, WPNAVBIN];
                        WPlastZ[cs] = m.Instrument[2, WPNAVBIN];

                        WPmagI[cs] = Math.Sqrt(WPlastX[cs] * WPlastX[cs] + WPlastY[cs] * WPlastY[cs] + UseZ * WPlastZ[cs] * WPlastZ[cs]);
                        WPdirI[cs] = Math.Atan2(WPlastX[cs], WPlastY[cs]) * 180.0 / Math.PI;
                        if (WPdirI[cs] < 0.0)
                            WPdirI[cs] = 360.0 + WPdirI[cs];
                    }

                }
            }
        }
        void BottomTrackEnsembleNavigate(ArrayClass m, int cs,bool showanyways)
        {
            //if (NewGGA[cs])
            {
                double dT;
                /*
                //screen the bottom data            
                if (m.B_EarthN[0] >= 1)
                {
                    if(Math.Abs(m.B_Instrument[2]) > 0.3)
                        m.B_EarthN[0] = 0;
                    if (Math.Abs(m.B_Instrument[3]) > 0.3)
                        m.B_EarthN[0] = 0;

                    if (m.B_EarthN[3] == 0)
                        m.B_EarthN[0] = 0;
                }
                */
                /*
                if (m.B_Correlation[0] < 0.95 || 
                    m.B_Correlation[1] < 0.95 || 
                    m.B_Correlation[2] < 0.95 || 
                    m.B_Correlation[3] < 0.95)
                {
                    m.B_EarthN[0] = 0;
                }
                */

                if (BTnavFirst[cs])
                {
                    BTnavGood[cs] = 0;
                    BTnavN[cs] = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        BTdisBm[i, cs] = 0;
                    }
                    BTdisE[cs] = 0.0;
                    BTdisN[cs] = 0.0;
                    BTdisU[cs] = 0.0;
                    BTdir[cs] = 0.0;
                    BTdisMag[cs] = 0.0;
                    BTdisDir[cs] = 0.0;
                    BTmag[cs] = 0.0;

                    BTdisX[cs] = 0.0;
                    BTdisY[cs] = 0.0;
                    BTdisZ[cs] = 0.0;
                    BTdirI[cs] = 0.0;
                    BTdisMagI[cs] = 0.0;
                    BTdisDirI[cs] = 0.0;
                    BTmagI[cs] = 0.0;
                    FreshBT[cs] = false;

                    if (m.B_EarthN[0] >= 1 && m.BottomTrackAvailable)
                    {
                        BTLastTime[cs] = m.B_FirstPingSeconds;
                        BTlastE[cs] = m.B_Earth[0];
                        BTlastN[cs] = m.B_Earth[1];
                        BTlastU[cs] = m.B_Earth[2];

                        BTlastX[cs] = m.B_Instrument[0];
                        BTlastY[cs] = m.B_Instrument[1];
                        BTlastZ[cs] = m.B_Instrument[2];

                        BTnavFirst[cs] = false;
                        FreshBT[cs] = true;
                    }
                    //FirstGGA[cs] = true;
                }
                else
                {
                    if(m.BottomTrackAvailable)
                        BTnavN[cs]++;
                    if (m.B_EarthN[0] >= 1 && m.BottomTrackAvailable && (!FirstGGA[cs] || showanyways))
                    {
                        if (m.B_EarthN[0] >= 1 && m.BottomTrackAvailable)
                        {
                            BTnavGood[cs]++;
                        }

                        FreshBT[cs] = true;
                        dT = m.B_FirstPingSeconds - BTLastTime[cs];

                        BTdisE[cs] += 0.5 * dT * (m.B_Earth[0] + BTlastE[cs]);
                        BTdisN[cs] += 0.5 * dT * (m.B_Earth[1] + BTlastN[cs]);
                        BTdisU[cs] += 0.5 * dT * (m.B_Earth[2] + BTlastU[cs]);

                        BTdisMag[cs] = Math.Sqrt(BTdisE[cs] * BTdisE[cs] + BTdisN[cs] * BTdisN[cs] + UseZ * BTdisU[cs] * BTdisU[cs]);
                        BTdisDir[cs] = Math.Atan2(BTdisE[cs], BTdisN[cs]) * 180.0 / Math.PI;
                        if (BTdisDir[cs] < 0.0)
                            BTdisDir[cs] = 360.0 + BTdisDir[cs];
                        BTlastE[cs] = m.B_Earth[0];
                        BTlastN[cs] = m.B_Earth[1];
                        BTlastU[cs] = m.B_Earth[2];
                        BTLastTime[cs] = m.B_FirstPingSeconds;

                        BTmag[cs] = Math.Sqrt(BTlastE[cs] * BTlastE[cs] + BTlastN[cs] * BTlastN[cs] + UseZ * BTlastU[cs] * BTlastU[cs]);
                        BTdir[cs] = Math.Atan2(BTlastE[cs], BTlastN[cs]) * 180.0 / Math.PI;
                        if (BTdir[cs] < 0.0)
                            BTdir[cs] = 360.0 + BTdir[cs];
                        //
                        BTdisX[cs] += 0.5 * dT * (m.B_Instrument[0] + BTlastX[cs]);
                        BTdisY[cs] += 0.5 * dT * (m.B_Instrument[1] + BTlastY[cs]);
                        BTdisZ[cs] += 0.5 * dT * (m.B_Instrument[2] + BTlastZ[cs]);

                        BTdisMagI[cs] = Math.Sqrt(BTdisX[cs] * BTdisX[cs] + BTdisY[cs] * BTdisY[cs] + UseZ * BTdisZ[cs] * BTdisZ[cs]);
                        BTdisDirI[cs] = Math.Atan2(BTdisX[cs], BTdisY[cs]) * 180.0 / Math.PI;
                        if (BTdisDirI[cs] < 0.0)
                            BTdisDirI[cs] = 360.0 + BTdisDirI[cs];
                        BTlastX[cs] = m.B_Instrument[0];
                        BTlastY[cs] = m.B_Instrument[1];
                        BTlastZ[cs] = m.B_Instrument[2];

                        BTmagI[cs] = Math.Sqrt(BTlastX[cs] * BTlastX[cs] + BTlastY[cs] * BTlastY[cs] + UseZ * BTlastZ[cs] * BTlastZ[cs]);
                        BTdirI[cs] = Math.Atan2(BTlastX[cs], BTlastY[cs]) * 180.0 / Math.PI;
                        if (BTdirI[cs] < 0.0)
                            BTdirI[cs] = 360.0 + BTdirI[cs];
                    }
                }
                if (m.BottomTrackAvailable)// && ProfileAmpScaledB)// && RecalcBT)
                {
                    //calc for reality
                    rtixfrm_BeamToEarthVelocityBT(m);
                    rtibeam_nav(m);
                }
            }
        }
        void rtibeam_nav(ArrayClass m)
        {
            int cs = (int)(m.E_CurrentSystem >> 24);
            if (cs < 0)
                cs = 0;
            if (cs > csubs - 1)
                cs = csubs - 1;

            double dT;

            int goodbm = 0;
            for (int i = 0; i < 4; i++)
            {
                if (m.B_Velocity[i] < 80)
                    goodbm++;
            }

            if (BTnavFirstBm[cs] && m.BottomTrackAvailable)
            {
                for (int i = 0; i < 4; i++)
                {
                    BTdisBm[i, cs] = 0;
                }

                if (goodbm > 3)
                {
                    BTLastTimeBm[cs] = m.B_FirstPingSeconds;

                    BTLastTimeBm[cs] = m.B_FirstPingSeconds;
                    for (int i = 0; i < 4; i++)
                        BTlastBm[i,cs] = m.B_Velocity[i];

                    BTnavFirstBm[cs] = false;
                }
            }
            else
            {
                if (goodbm > 3 && m.BottomTrackAvailable)
                {
                    dT = m.B_FirstPingSeconds - BTLastTimeBm[cs];
                    for (int i = 0; i < 4; i++)
                    {
                        BTdisBm[i, cs] += 0.5 * dT * (m.B_Velocity[i] + BTlastBm[i, cs]);
                        BTlastBm[i,cs] = m.B_Velocity[i];
                        BTLastTimeBm[cs] = m.B_FirstPingSeconds;
                    }
                }
            }
        }

        double[,] M = new double[4, 4];

        void rtixfrm_BeamToEarthVelocityBT(ArrayClass m)
        {
            int cs = (int)(m.E_CurrentSystem >> 24);
            if (cs < 0)
                cs = 0;
            if (cs > csubs - 1)
                cs = csubs - 1;

            int goodBm;
            int badBm = 4;
            
            float [] V = new float[4];
           
            int i;
            
            float P,R,H1;
            float Pitch   = m.B_Pitch;
            float Roll    = m.B_Roll;
            
            float Heading = m.B_Heading;

            //Heading = (float)HDT[cs].Heading;

            char st = '0';
            if (m.E_FW_Vers[3] > 31 && m.E_FW_Vers[3] < 127)
                st = Convert.ToChar(m.E_FW_Vers[3]);

            float Bm0SinHeading = 0;
            float Bm0CosHeading = 1;

            double ang = 20;
            switch (st)
            {
                case 'j'://   2.4 MHz 4 beam 30 degree piston
                case 'k'://   1.2 MHz 4 beam 20 degree piston
                case 'l'://   0.6 MHz 4 beam 20 degree piston
                case 'm'://   0.3 MHz 4 beam 20 degree piston
                case 'n'://   0.15 MHz 4 beam 20 degree piston
                case 'o'://   0.075 MHz 4 beam 20 degree piston
                case 'p'://   0.038 MHz 4 beam 20 degree piston
                case 'q'://   0.019 MHz 4 beam 20 degree piston
                    ang = 30;
                    break;
            }

                    double s = Math.Sin(ang / 180.0 * Math.PI);
            double c = Math.Cos(ang / 180.0 * Math.PI);

            int[] EarthN = new int[4];
            double E, N, U, Q;
            double X, Y, Z;

            if (m.E_SN_Buffer[0] == '0' && m.E_SN_Buffer[1] == '4')
            {
                //subsystem selection
                switch (st)
                {
                    case 'c'://   1.2 MHz 4 beam 20 degree piston opposite facing
                    case 'd'://   0.6 MHz 4 beam 20 degree piston opposite facing
                    case 'e'://   0.3 MHz 4 beam 20 degree piston opposite facing
                        Bm0SinHeading = (float)0.0;
                        Bm0CosHeading = (float)1.0;
                        //Pitch = Pitch;
                        if (Roll < 0)
                            Roll = (float)180.0 + Roll;
                        else
                            Roll -= (float)180.0;
                        break;
                    case '0':
                    case '1'://   2 MHz 4 beam 20 degree piston
                    case '2'://   1.2 MHz 4 beam 20 degree piston
                    default:
                    case '3'://   600 kHz 4 beam 20 degree piston
                    case '4'://   300 kHz 4 beam 20 degree piston                
                        Bm0SinHeading = (float)0.0;
                        Bm0CosHeading = (float)1.0;
                        break;
                    case '5'://   2400 kHz 4 beam 20 degree piston
                    case '6'://   1200 kHz 4 beam 20 degree piston
                    case '7'://   600 kHz 4 beam 20 degree piston
                    case '8'://   300 kHz 4 beam 20 degree piston
                        Bm0SinHeading = (float)0.866025404;
                        Bm0CosHeading = (float)0.5;

                        //Bm0CosHeading = (float)0.866025404;
                        //Bm0SinHeading = (float)0.5;

                        break;
                    case 'A'://   1.2 MHz Vertical beam piston
                    case 'B'://   600 kHz Vertical beam piston
                    case 'C'://   300 kHz Vertical beam piston
                        Bm0SinHeading = 0;
                        Bm0CosHeading = 0;
                        break;
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':

                        break;
                }

                // 1.949203 -0.000000  0.354726
                //-0.974602 -1.688059  0.354726
                //-0.974602  1.688059  0.354726

                //X
                M[0, 0] = -1.949203;
                M[0, 1] = 0.974601;
                M[0, 2] = 0.974601;
                M[0, 3] = 0;
                //Y
                M[1, 0] = 0;
                M[1, 1] = -1.688059;
                M[1, 2] = 1.688059;
                M[1, 3] = 0;
                //Z
                M[2, 0] = -0.354726;
                M[2, 1] = -0.354726;
                M[2, 2] = -0.354726;
                M[2, 3] = 0;
                //Q
                M[3, 0] = 0;
                M[3, 1] = 0;
                M[3, 2] = 0;
                M[3, 3] = 0;

                // calculate new Pitch and Roll for beam0 offset from compass bin mapping
                //
                // A = new angle +45 degrees for second piston system
                // P = Pitch 
                // R = Roll
                //
                // P' = PcosA - RsinA
                // R' = PsinA + RcosA

                if (Roll >= 90.0 || Roll <= -90.0)//down facing case
                {
                    float R1;
                    float B1;
                    if (Roll > 90.0)
                        B1 = (float)-180.0;
                    else
                        B1 = (float)+180.0;

                    //R1 = Roll + B1;
                    //P = Pitch * Bm0CosHeading - R1 * Bm0SinHeading;
                    //R = Pitch * Bm0SinHeading + R1 * Bm0CosHeading - B1;

                    R1 = Roll + B1;
                    P = Pitch * Bm0CosHeading - R1 * Bm0SinHeading;
                    R = Pitch * Bm0SinHeading + R1 * Bm0CosHeading;
                    R -= B1;


                    //P = 0;
                    //R = 0;
                }
                else//up facing
                {
                    P = Pitch * Bm0CosHeading + Roll * Bm0SinHeading;
                    R = -Pitch * Bm0SinHeading + Roll * Bm0CosHeading;
                }

                double SP = Math.Sin(Math.PI * P / 180.0);
                double CP = Math.Cos(Math.PI * P / 180.0);
                double SR = Math.Sin(Math.PI * R / 180.0);
                double CR = Math.Cos(Math.PI * R / 180.0);

                double H = Math.Asin(Bm0SinHeading);
                H *= 180;
                H /= Math.PI;
                H += Heading;
                H += 360;

                //double SH = Math.Sin(Math.PI * Heading / 180.0);
                //double CH = Math.Cos(Math.PI * Heading / 180.0);
                double SH = Math.Sin(Math.PI * H / 180.0);
                double CH = Math.Cos(Math.PI * H / 180.0);

                //rotate to ENU
                //check to see if a three beam solution is needed
                goodBm = 0;


                for (i = 0; i < 3; i++)
                {
                    EarthN[i] = 0;
                    //if((m.B_SNR[i] > 30) && (m.B_Correlation[i] > 0.98))
                    if ((m.B_SNR[i] > 15) && (m.B_Correlation[i] > 0.8))
                    {
                        goodBm++;
                        V[i] = m.B_Velocity[i];
                    }
                }
                if (goodBm >= 3)
                {
                    /*
                    V[0] *= (float)(1.015);
                    V[1] *= (float)(1.015);
                    V[2] *= (float)(1.015);
                    V[3] *= (float)(1.015);
                    */
                    /*
                    V[0] = (float)0.5;
                    V[1] = (float)-1;
                    V[2] = (float)0.5;                    
                    */
                    X = -(V[0] * M[0, 0]
                              + V[1] * M[0, 1]
                              + V[2] * M[0, 2]);

                    Y = -(V[0] * M[1, 0]
                              + V[1] * M[1, 1]
                              + V[2] * M[1, 2]);

                    //rotate axis to align beam 0 with compass

                    double X1 = X;
                    double Y1 = Y;

                    X = X1 * Bm0CosHeading - Y1 * Bm0SinHeading;
                    Y = X1 * Bm0SinHeading + Y1 * Bm0CosHeading;


                    Z = -(V[0] * M[2, 0]
                        + V[1] * M[2, 1]
                        + V[2] * M[2, 2]);

                    E = X1 * (SH * CP)
                       - Y1 * (CH * CR + SH * SR * SP)
                       + Z * (CH * SR - SH * CR * SP);//east

                    N = X1 * (CH * CP)
                       + Y1 * (SH * CR - CH * SR * SP)
                       - Z * (SH * SR + CH * SP * CR);//north

                    U = X1 * (SP)
                       + Y1 * (SR * CP)
                       + Z * (CP * CR);//up

                    EarthN[0] = 1;
                    EarthN[1] = 1;
                    EarthN[2] = 1;
                    EarthN[3] = 0;
                    //Q = 0.0;
                }
                else//not enough beams for xfrm
                {
                    X = 0;
                    Y = 0;
                    Z = 0;
                    E = 0.0;
                    N = 0.0;
                    U = 0.0;
                    //Q = 0.0;

                    EarthN[0] = 0;
                    EarthN[1] = 0;
                    EarthN[2] = 0;
                    EarthN[3] = 0;
                }
            }
            else
            {
                if (m.E_SN_Buffer[0] == '0' && m.E_SN_Buffer[1] == '0')//ADCP 0 array
                {
                    goodBm = 0;
                    badBm = 4;

                    P = Pitch;
                    R = Roll;
                    H1 = Heading;

                    //P = 0;
                    //R = 0;
                    //H1 = 0;

                    //BTIC
                    //if using PNI with project 1 BTIC
                    //double off = -15.805;// M->Bm0Heading + RP->PNItiltHeadingOffset;
                    /*
                    double off = 0;
                    off *= Math.PI;
                    off /= 180;
                    float Bm0SinHeadingA = (float)Math.Sin(off);
                    float Bm0CosHeadingA = (float)Math.Cos(off);

                    if (Roll >= 90.0 || Roll <= -90.0)//down facing case
                    {
                        float R1;
                        float B1;
                        if (Roll > 90.0)
                            B1 = -180;
                        else
                            B1 = +180;

                        R1 = Roll + B1;

                        P = Pitch * Bm0CosHeadingA
                            - R1 * Bm0SinHeadingA;

                        R = Pitch * Bm0SinHeadingA
                            + R1 * Bm0CosHeadingA
                            - B1;
                    }
                    else//up facing
                    {
                        P = Pitch * Bm0CosHeadingA
                            + Roll * Bm0SinHeadingA;

                        R = -Pitch * Bm0SinHeadingA
                            + Roll * Bm0CosHeadingA;
                    }
                    */
                    double SP = Math.Sin(Math.PI * P / 180);
                    double CP = Math.Cos(Math.PI * P / 180);

                    double SR = Math.Sin(Math.PI * R / 180);
                    double CR = Math.Cos(Math.PI * R / 180);

                    /*
                     * double H = Math.Asin(Bm0SinHeadingA);
                    H *= 180;
                    H /= Math.PI;
                    H += Heading;
                    H += 360;
                    */
                    double SH = Math.Sin(Math.PI * H1 / 180);
                    double CH = Math.Cos(Math.PI * H1 / 180);

                    //rotate to XYZ and ENU
                    //check to see if a three beam solution is needed
                    //goodBm = 0;

                    //m.B_Correlation[3] = 0;

                    if (!FindBTinProfile)
                    {
                        for (i = 0; i < 4; i++)
                        {
                            EarthN[i] = 0;
                            
                            if (m.B_Velocity[i] < 88)//if ((m.B_SNR[i] > 15) && (m.B_Correlation[i] >= 0.97))
                            {
                                goodBm++;
                                V[i] = m.B_Velocity[i];
                            }
                            else
                            {
                                badBm = i;
                            }
                        }
                    }
                    else
                    {
                        for (i = 0; i < 4; i++)
                        {
                            EarthN[i] = 0;
                            
                            if (BPR_beamVel[i] < 88)//if ((m.B_SNR[i] > 15) && (m.B_Correlation[i] >= 0.97))
                            {
                                goodBm++;
                                V[i] = BPR_beamVel[i]; 
                            }
                            else
                            {
                                badBm = i;
                            }
                        }
                    }

                    if (goodBm >= 3)
                    {
                        if (goodBm == 3)//we need a 3 beam solution
                        {
                            // Q = (V[0] + V[1]  - V[2] - V[3];
                            // set Q = 0 then solve for missing beam
                            switch (badBm)
                            {
                                case 0:
                                    V[badBm] = -V[1] + V[2] + V[3];
                                    break;
                                case 1:
                                    V[badBm] = -V[0] + V[2] + V[3];
                                    break;
                                case 2:
                                    V[badBm] = V[0] + V[1] - V[3];
                                    break;
                                case 3:
                                    V[badBm] = V[0] + V[1] - V[2];
                                    break;
                            }
                        }
                        
                        //#define DA78_ELEMENT_SPACING 0.00889//m  = 0.350"
                        //RBT->BeamAngleRadians = asin((RBT->SosMperSec / (4 * RBT->SystemFreqHz * RBT->ElementSpacing)));
                        //RBT->BeamAngleSine    = sin(RBT->BeamAngleRadians);
                        
                        double ElementSpacing = 0.00889;//72 kHz m  = 0.350"
                        double BeamAngleRadians = Math.Asin((m.B_SpeedOfSound / (4 * m.SystemSetup_BTSystemFreqHz * ElementSpacing)));
                        double BeamAngleSine = Math.Sin(BeamAngleRadians);
                        double BeamAngleCosine = Math.Cos(BeamAngleRadians);

                        X = (V[0] - V[1]) / (2.0 * BeamAngleSine);
                        Y = (V[2] - V[3]) / (2.0 * BeamAngleSine);
                        Z = (V[0] + V[1] + V[2] + V[3]) / (4.0 * BeamAngleCosine);
                        
                        E = +X * (SH * CP)
                                        - Y * (CH * CR + SH * SR * SP)
                                        + Z * (CH * SR - SH * CR * SP);//east

                        N = +X * (CH * CP)
                                        + Y * (SH * CR - CH * SR * SP)
                                        - Z * (SH * SR + CH * SP * CR);//north

                        U = +X * (SP)
                                        + Y * (SR * CP)
                                        + Z * (CP * CR);//up

                        EarthN[0] = 1;
                        EarthN[1] = 1;
                        EarthN[2] = 1;
                        
                        if (goodBm == 3)
                        {
                            Q = 0.0;
                            EarthN[3] = 0;
                        }
                        else
                        {
                            Q = (V[0] + V[1] - V[2] - V[3]) / 4.0;
                            EarthN[3] = 1;
                        }

                        BPR_InstVel[0] = (float)X;
                        BPR_InstVel[1] = (float)Y;
                        BPR_InstVel[2] = (float)Z;
                        BPR_InstVel[3] = (float)Q;

                       

                        BPR_EarthVel[0] = (float)E;
                        BPR_EarthVel[1] = (float)N;
                        BPR_EarthVel[2] = (float)U;
                        BPR_EarthVel[3] = (float)Q;
                       
                    }
                    else//not enough beams for xfrm
                    {
                        X = 0;
                        Y = 0;
                        Z = 0;
                        E = 0.0;
                        N = 0.0;
                        U = 0.0;
                        //Q = 0.0;

                        EarthN[0] = 0;
                        EarthN[1] = 0;
                        EarthN[2] = 0;
                        EarthN[3] = 0;
                        
                        BPR_InstVel[0] = (float)88.88;
                        BPR_InstVel[1] = (float)88.88;
                        BPR_InstVel[2] = (float)88.88;
                        BPR_InstVel[3] = (float)88.88;

                        BPR_EarthVel[0] = (float)88.88;
                        BPR_EarthVel[1] = (float)88.88;
                        BPR_EarthVel[2] = (float)88.88;
                        BPR_EarthVel[3] = (float)88.88;
                    }
                }
                else//assume ADCP01
                {
                    //subsystem selection
                    switch (st)
                    {
                        case 'c'://   1.2 MHz 4 beam 20 degree piston opposite facing
                        case 'd'://   0.6 MHz 4 beam 20 degree piston opposite facing
                        case 'e'://   0.3 MHz 4 beam 20 degree piston opposite facing
                            Bm0SinHeading = (float)0.0;
                            Bm0CosHeading = (float)1.0;
                            //Pitch = Pitch;
                            if (Roll < 0)
                                Roll = (float)180.0 + Roll;
                            else
                                Roll -= (float)180.0;
                            break;
                        case '0':
                        case '1'://   2 MHz 4 beam 20 degree piston
                        case '2'://   1.2 MHz 4 beam 20 degree piston
                        default:
                        case '3'://   600 kHz 4 beam 20 degree piston
                        case '4'://   300 kHz 4 beam 20 degree piston                
                            Bm0SinHeading = (float)0.0;
                            Bm0CosHeading = (float)1.0;
                            break;
                        case '5'://   2400 kHz 4 beam 20 degree piston
                        case '6'://   1200 kHz 4 beam 20 degree piston
                        case '7'://   600 kHz 4 beam 20 degree piston
                        case '8'://   300 kHz 4 beam 20 degree piston
                            Bm0SinHeading = (float)0.707106781;
                            Bm0CosHeading = (float)0.707106781;
                            break;
                        case 'A'://   1.2 MHz Vertical beam piston
                        case 'B'://   600 kHz Vertical beam piston
                        case 'C'://   300 kHz Vertical beam piston
                            Bm0SinHeading = 0;
                            Bm0CosHeading = 0;
                            break;
                        case 'I':
                        case 'J':
                        case 'K':
                        case 'L':
                        case 'M':

                            break;
                    }


                    //X
                    M[0, 0] = -1 / (2 * s);//sn 534 * 1.015697546;//sn508 * 0.985901311;//SN498 * 0.98735;//SN500 * 0.98464;
                    M[0, 1] = 1 / (2 * s);//sn 534  * 1.015697546;//sn508 * 0.985901311;//SN498 * 0.98735;//SN500 * 0.98464;
                    M[0, 2] = 0;
                    M[0, 3] = 0;
                    //Y
                    M[1, 0] = 0;
                    M[1, 1] = 0;
                    M[1, 2] = -1 / (2 * s);//sn 534 * 1.022572485;//sn508 * 0.998050797;//SN498 * 0.98986;//SN500 * 0.99111;// * 0.98794;// * 1.0185;
                    M[1, 3] = 1 / (2 * s);//sn 534 * 1.022572485;//sn508 * 0.998050797;//SN498 * 0.98986;//SN500 * 0.99111;// * 0.98794;// * 1.0185;
                    //Z
                    M[2, 0] = -1 / (4 * c);
                    M[2, 1] = -1 / (4 * c);
                    M[2, 2] = -1 / (4 * c);
                    M[2, 3] = -1 / (4 * c);
                    //Q
                    M[3, 0] = 0.25;
                    M[3, 1] = 0.25;
                    M[3, 2] = -0.25;
                    M[3, 3] = -0.25;

                    // calculate new Pitch and Roll for beam0 offset from compass bin mapping
                    //
                    // A = new angle +45 degrees for second piston system
                    // P = Pitch 
                    // R = Roll
                    //
                    // P' = PcosA - RsinA
                    // R' = PsinA + RcosA

                    if (Roll >= 90.0 || Roll <= -90.0)//down facing case
                    {
                        float R1;
                        float B1;
                        if (Roll > 90.0)
                            B1 = (float)-180.0;
                        else
                            B1 = (float)+180.0;

                        //R1 = Roll + B1;
                        //P = Pitch * Bm0CosHeading - R1 * Bm0SinHeading;
                        //R = Pitch * Bm0SinHeading + R1 * Bm0CosHeading - B1;

                        R1 = Roll + B1;
                        P = Pitch * Bm0CosHeading - R1 * Bm0SinHeading;
                        R = Pitch * Bm0SinHeading + R1 * Bm0CosHeading;
                        R -= B1;


                        //P = 0;
                        //R = 0;
                    }
                    else//up facing
                    {
                        P = Pitch * Bm0CosHeading + Roll * Bm0SinHeading;
                        R = -Pitch * Bm0SinHeading + Roll * Bm0CosHeading;
                    }

                    double SP = Math.Sin(Math.PI * P / 180.0);
                    double CP = Math.Cos(Math.PI * P / 180.0);
                    double SR = Math.Sin(Math.PI * R / 180.0);
                    double CR = Math.Cos(Math.PI * R / 180.0);

                    double H = Math.Asin(Bm0SinHeading);
                    H *= 180;
                    H /= Math.PI;
                    H += Heading;
                    H += 360;

                    //double SH = Math.Sin(Math.PI * Heading / 180.0);
                    //double CH = Math.Cos(Math.PI * Heading / 180.0);
                    double SH = Math.Sin(Math.PI * H / 180.0);
                    double CH = Math.Cos(Math.PI * H / 180.0);

                    //rotate to ENU
                    //check to see if a three beam solution is needed
                    goodBm = 0;


                    //m.B_Correlation[3] = 0;//force a three beam solution
                    for (i = 0; i < 4; i++)
                    {
                        EarthN[i] = 0;
                        //if((m.B_SNR[i] > 30) && (m.B_Correlation[i] > 0.98))
                        if ((m.B_SNR[i] > 15) && (m.B_Correlation[i] > 0.8))
                        {
                            goodBm++;
                            V[i] = m.B_Velocity[i];
                        }
                        else
                        {
                            badBm = i;
                        }
                    }
                    if (goodBm >= 3)
                    {
                        /*
                        V[0] *= (float)(1.015);
                        V[1] *= (float)(1.015);
                        V[2] *= (float)(1.015);
                        V[3] *= (float)(1.015);

                        V[0] *= (float)(1.0 / 1.005);
                        V[1] *= (float)(1.0 / 1.005);
                        V[2] *= (float)(1.0 / 1.005);
                        V[3] *= (float)(1.0 / 1.005);
                        */

                        //goodBm = 3;
                        //badBm = 2;

                        if (goodBm == 3)//we need a 3 beam solution
                        {
                            // Q = (V[0] + V[1]  - V[2] - V[3];
                            // set Q = 0 then solve for missing beam            
                            switch (badBm)
                            {
                                case 0:
                                    V[badBm] = -V[1] + V[2] + V[3];
                                    break;
                                case 1:
                                    V[badBm] = -V[0] + V[2] + V[3];
                                    break;
                                case 2:
                                    V[badBm] = V[0] + V[1] - V[3];
                                    break;
                                case 3:
                                    V[badBm] = V[0] + V[1] - V[2];
                                    break;
                            }
                        }

                        X = -(V[0] * M[0, 0]
                                  + V[1] * M[0, 1]
                                  + V[2] * M[0, 2]
                                  + V[3] * M[0, 3]);

                        Y = -(V[0] * M[1, 0]
                                  + V[1] * M[1, 1]
                                  + V[2] * M[1, 2]
                                  + V[3] * M[1, 3]);

                        //rotate axis to align beam 0 with compass

                        double X1 = X;
                        double Y1 = Y;

                        X = X1 * Bm0CosHeading - Y1 * Bm0SinHeading;
                        Y = X1 * Bm0SinHeading + Y1 * Bm0CosHeading;


                        Z = -(V[0] * M[2, 0]
                            + V[1] * M[2, 1]
                            + V[2] * M[2, 2]
                            + V[3] * M[2, 3]);

                        E = X1 * (SH * CP)
                           - Y1 * (CH * CR + SH * SR * SP)
                           + Z * (CH * SR - SH * CR * SP);//east

                        N = X1 * (CH * CP)
                           + Y1 * (SH * CR - CH * SR * SP)
                           - Z * (SH * SR + CH * SP * CR);//north

                        U = X1 * (SP)
                           + Y1 * (SR * CP)
                           + Z * (CP * CR);//up

                        EarthN[0] = 1;
                        EarthN[1] = 1;
                        EarthN[2] = 1;

                        if (goodBm == 3)
                        {
                            //Q = 0.0;
                            EarthN[3] = 0;
                        }
                        else
                        {
                            //Q = V[0] * M[3, 0] + V[1] * M[3, 1] + V[2] * M[3, 2] + V[3] * M[3, 3];
                            EarthN[3] = 1;
                        }
                    }
                    else//not enough beams for xfrm
                    {
                        X = 0;
                        Y = 0;
                        Z = 0;
                        E = 0.0;
                        N = 0.0;
                        U = 0.0;
                        //Q = 0.0;
                        EarthN[0] = 0;
                        EarthN[1] = 0;
                        EarthN[2] = 0;
                        EarthN[3] = 0;
                    }
                }
            }

            //screen the bottom data            
            /*
            if (EarthN[0] >= 1)
            {
                if (Math.Abs(Z) > 0.3)
                    EarthN[0] = 0;
                if (Math.Abs(Q) > 0.3)
                    EarthN[0] = 0;

                if (EarthN[3] == 0)
                    EarthN[0] = 0;
            }
            */
            if (bBTnavFirst[cs])
            {
                bBTdisE[cs] = 0.0;
                bBTdisN[cs] = 0.0;
                bBTdisU[cs] = 0.0;
                bBTdir[cs] = 0.0;
                bBTdisMag[cs] = 0.0;
                bBTdisDir[cs] = 0.0;
                bBTmag[cs] = 0.0;


                bBTdisX[cs] = 0.0;
                bBTdisY[cs] = 0.0;
                bBTdisZ[cs] = 0.0;
                bBTdirI[cs] = 0.0;
                bBTdisMagI[cs] = 0.0;
                bBTdisDirI[cs] = 0.0;
                bBTmagI[cs] = 0.0;

                bFreshBT[cs] = false;

                if (EarthN[0] >= 1)
                {
                    bBTLastTime[cs] = m.B_FirstPingSeconds;
                    bBTlastE[cs] = E;
                    bBTlastN[cs] = N;
                    bBTlastU[cs] = U;

                    bBTlastX[cs] = X;
                    bBTlastY[cs] = Y;
                    bBTlastZ[cs] = Z;

                    bBTnavFirst[cs] = false;
                }
            }
            else
            {
                if (EarthN[0] >= 1 && m.BottomTrackAvailable)
                {
                    bFreshBT[cs] = true;
                    double dT = m.B_FirstPingSeconds - bBTLastTime[cs];

                    bBTdisE[cs] += 0.5 * dT * (E + bBTlastE[cs]);
                    bBTdisN[cs] += 0.5 * dT * (N + bBTlastN[cs]);
                    bBTdisU[cs] += 0.5 * dT * (U + bBTlastU[cs]);

                    bBTdisMag[cs] = Math.Sqrt(bBTdisE[cs] * bBTdisE[cs] + bBTdisN[cs] * bBTdisN[cs] + bBTdisU[cs] * bBTdisU[cs]);
                    bBTdisDir[cs] = Math.Atan2(bBTdisE[cs], bBTdisN[cs]) * 180.0 / Math.PI;
                    if (bBTdisDir[cs] < 0.0)
                        bBTdisDir[cs] = 360.0 + bBTdisDir[cs];
                    bBTlastE[cs] = E;
                    bBTlastN[cs] = N;
                    bBTlastU[cs] = U;
                    bBTLastTime[cs] = m.B_FirstPingSeconds;

                    bBTmag[cs] = Math.Sqrt(bBTlastE[cs] * bBTlastE[cs] + bBTlastN[cs] * bBTlastN[cs] + bBTlastU[cs] * bBTlastU[cs]);
                    bBTdir[cs] = Math.Atan2(bBTlastE[cs], bBTlastN[cs]) * 180.0 / Math.PI;
                    if (bBTdir[cs] < 0.0)
                        bBTdir[cs] = 360.0 + bBTdir[cs];
                    //
                    bBTdisX[cs] += 0.5 * dT * (X + bBTlastX[cs]);
                    bBTdisY[cs] += 0.5 * dT * (Y + bBTlastY[cs]);
                    bBTdisZ[cs] += 0.5 * dT * (Z + bBTlastZ[cs]);

                    bBTdisMagI[cs] = Math.Sqrt(bBTdisX[cs] * bBTdisX[cs] + bBTdisY[cs] * bBTdisY[cs] + bBTdisZ[cs] * bBTdisZ[cs]);
                    bBTdisDirI[cs] = Math.Atan2(bBTdisX[cs], bBTdisY[cs]) * 180.0 / Math.PI;
                    if (bBTdisDirI[cs] < 0.0)
                        bBTdisDirI[cs] = 360.0 + bBTdisDirI[cs];
                    bBTlastX[cs] = X;
                    bBTlastY[cs] = Y;
                    bBTlastZ[cs] = Z;

                    bBTmagI[cs] = Math.Sqrt(bBTlastX[cs] * bBTlastX[cs] + bBTlastY[cs] * bBTlastY[cs] + bBTlastZ[cs] * bBTlastZ[cs]);
                    bBTdirI[cs] = Math.Atan2(bBTlastX[cs], bBTlastY[cs]) * 180.0 / Math.PI;
                    if (bBTdirI[cs] < 0.0)
                        bBTdirI[cs] = 360.0 + bBTdirI[cs];
                }
            }
        }
        public void CalculateRTI01Navigation(int cs)
        {
            if (RTI01.BottomVelocityX == -99.999 || RTI01.BottomVelocityX == 88.888)
                RTI01.BtOK = false;
            else
                RTI01.BtOK = true;            

            if (RTI01.BtOK && RTI01Last.BtOK)
            {
                double theta = HDT[cs].Heading / 180.0 * Math.PI;
                double dT = RTI01.TimeSeconds - RTI01Last.TimeSeconds;

                double dBX = dT * (RTI01.BottomVelocityX + RTI01Last.BottomVelocityX) / 2;
                double dBY = dT * (RTI01.BottomVelocityY + RTI01Last.BottomVelocityY) / 2;
                double dBZ = dT * (RTI01.BottomVelocityZ + RTI01Last.BottomVelocityZ) / 2;

                double CosT = Math.Cos(theta);
                double SinT = Math.Sin(theta);

                RTIACC.Time += dT;
                RTIACC.N++;

                RTIACC.BX += dBX;
                RTIACC.BY += dBY;
                RTIACC.BZ += dBZ;

                RTIACC.BN += dBX * CosT - dBY * SinT;
                RTIACC.BE += dBX * SinT + dBY * CosT;

                double E, N;

                N = RTI01.BottomVelocityX * CosT - RTI01.BottomVelocityY * SinT;
                E = RTI01.BottomVelocityX * SinT + RTI01.BottomVelocityY * CosT;

                RTIBNAV.CMG = Math.Atan2(RTIACC.BE, RTIACC.BN);
                RTIBNAV.CMG = 180.0 * RTIBNAV.CMG / Math.PI;
                RTIBNAV.CMG = (RTIBNAV.CMG + 360.0) % 360; //use mod to turn -90 = 270
                RTIBNAV.COG = Math.Atan2(E, N);
                //RTIBNAV.COG = Math.Atan2(RTI10.BottomVelocityY, RTI10.BottomVelocityX);
                RTIBNAV.COG = 180.0 * RTIBNAV.COG / Math.PI;
                RTIBNAV.COG = (RTIBNAV.COG + 360.0) % 360; //use mod to turn -90 = 270
                RTIBNAV.DMG = Math.Sqrt(RTIACC.BN * RTIACC.BN + RTIACC.BE * RTIACC.BE);
                RTIBNAV.SOG = Math.Sqrt(RTI01.BottomVelocityX * RTI01.BottomVelocityX + RTI01.BottomVelocityY * RTI01.BottomVelocityY);
            }

            if (RTI01.WaterVelocityX == -99.999 || RTI01.WaterVelocityX == 88.888)
                RTI01.WtOK = false;
            else
                RTI01.WtOK = true;

            if (RTI01.WtOK && RTI01Last.WtOK)// && RTI01.BtOK)
            {
                double theta = HDT[cs].Heading / 180.0 * Math.PI;
                double dT = RTI01.TimeSeconds - RTI01Last.TimeSeconds;


                double dWX = dT * (RTI01.WaterVelocityX + RTI01Last.WaterVelocityX) / 2;
                double dWY = dT * (RTI01.WaterVelocityY + RTI01Last.WaterVelocityY) / 2;
                double dWZ = dT * (RTI01.WaterVelocityZ + RTI01Last.WaterVelocityZ) / 2;

                double CosT = Math.Cos(theta);
                double SinT = Math.Sin(theta);

                //RTIACC.Time += dT;
                //RTIACC.N++;

                RTIACC.WX += dWX;
                RTIACC.WY += dWY;
                RTIACC.WZ += dWZ;

                RTIACC.WN += dWX * CosT - dWY * SinT;
                RTIACC.WE += dWX * SinT + dWY * CosT;

                RTIWNAV.CMG = Math.Atan2(RTIACC.WE, RTIACC.WN);
                RTIWNAV.CMG = 180.0 * RTIWNAV.CMG / Math.PI;
                RTIWNAV.CMG = (RTIWNAV.CMG + 360.0) % 360; //use mod to turn -90 = 270
                RTIWNAV.COG = Math.Atan2(RTI01.WaterVelocityY, RTI01.WaterVelocityX);
                RTIWNAV.COG = 180.0 * RTIWNAV.COG / Math.PI;
                RTIWNAV.COG = (RTIWNAV.COG + 360.0) % 360; //use mod to turn -90 = 270
                RTIWNAV.DMG = Math.Sqrt(RTIACC.WN * RTIACC.WN + RTIACC.WE * RTIACC.WE);
                RTIWNAV.SOG = Math.Sqrt(RTI01.WaterVelocityX * RTI01.WaterVelocityX + RTI01.WaterVelocityY * RTI01.WaterVelocityY);
            }
        }
        public void CalculateGGANavigation(int cs)
        {
            double dd, dt;
            dd = calcDistance(GGALast[cs].latrad, GGALast[cs].lonrad, GGA[cs].latrad, GGA[cs].lonrad);

            double now;
            double last;
            int hh;
            int mm;
            int ss;
            hh = (int)(GGA[cs].utc / 10000);
            mm = (int)((GGA[cs].utc - 10000 * hh) / 100);
            ss = (int)((GGA[cs].utc - 10000 * hh - 100 * mm));

            now = 3600 * hh + 60 * mm + ss;

            hh = (int)(GGALast[cs].utc / 10000);
            mm = (int)((GGALast[cs].utc - 10000 * hh) / 100);
            ss = (int)((GGALast[cs].utc - 10000 * hh - 100 * mm));

            last = 3600 * hh + 60 * mm + ss;
            
            dt = now - last;

            if (dt < 0)
            {
                dt += 86400;
            }
            GGANAV[cs].TotalDist += dd * dt;
            GGANAV[cs].SOG = 0;
            if (dt != 0)
                GGANAV[cs].SOG = dd / dt;
            GGANAV[cs].COG = calcBearing(GGALast[cs].latrad, GGALast[cs].lonrad, GGA[cs].latrad, GGA[cs].lonrad);
            GGANAV[cs].CMG = calcBearing(GGAFirst[cs].latrad, GGAFirst[cs].lonrad, GGA[cs].latrad, GGA[cs].lonrad);
            GGANAV[cs].DMG = calcDistance(GGAFirst[cs].latrad, GGAFirst[cs].lonrad, GGA[cs].latrad, GGA[cs].lonrad);
        }
        public double calcDistance(double lat1, double lon1, double lat2, double lon2)
        {
            //Calculate distance form lat1/lon1 to lat2/lon2 using haversine formula
            //Note lat1/lon1/lat2/lon2 must be in radians
            //Returns float distance in feet
            //haversin(?) = [sin(?/2)]^2
            //haversine(d/R) = haversine(lat2-lat1)+cos(lat1)*cos(lat2)*haversine(lon2-lon1)

            //in matlab
            //latrad1 = lat1 * pi / 180;
            //lonrad1 = lon1 * pi / 180;
            //latrad2 = lat2 * pi / 180;
            //lonrad2 = lon2 * pi / 180;
            //londif = abs(lonrad2 - lonrad1);
            //raddis = acos(sin(latrad2) * sin(latrad1) + cos(latrad2) * cos(latrad1) * cos(londif));
            //nautdis = raddis * 3437.74677;
            //statdis = nautdis * 1.1507794;
            //stdiskm = nautdis * 1.852;

            double dlon, dlat, a, c;
            double dist;
            lon1 *= -1.0; //make west = positive
            lon2 *= -1.0;
            dlon = lon2 - lon1;
            dlat = lat2 - lat1;
            a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);
            c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            //6,371.009 
            dist = 6378140 * c;  //radius of the earth (6378140 meters) in feet 20925656.2
            return (dist);
        }
        public double calcBearing(double lat1, double lon1, double lat2, double lon2)
        {
            //Calculate bearing from lat1/lon1 to lat2/lon2
            //Note lat1/lon1/lat2/lon2 must be in radians
            //Returns float bearing in degrees

            double bearing;
            //determine angle
            bearing = Math.Atan2(Math.Sin(lon2 - lon1) * Math.Cos(lat2), (Math.Cos(lat1) * Math.Sin(lat2)) - (Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1)));
            //convert to degrees
            bearing = 180.0 * bearing / Math.PI;
            //use mod to turn -90 = 270
            bearing = (bearing + 360.0) % 360;
            return bearing;
        }
        void BTnavClr()
        {
            for (int i = 0; i < csubs; i++)
            {
                BTnavFirstBm[i] = true;
                bBTnavFirst[i] = true;
                BTnavFirst[i] = true;
                BTnavGood[i] = 0;
                BTnavN[i] = 0;
                WPnavFirst[i] = true;
                WPnavGood[i] = 0;
                WPnavN[i] = 0;
                BTNavstr[i] = "";
                FirstAllNav[i] = true;
                BTBeamGood[i] = 0;
            }
            BTBeamErrorGood = 0;
            BTBeamN = 0;
            BTBeamErrorSum = 0.0;
            BTBeamErrorSumSqr = 0.0;

            ClearNavigation();
            ClearStructures();
        }

        //Bottom Track---------------------------------------------------------
        int UseZ = 0;
        bool RecalcBT = false;
        bool FindBTinProfile = false;

        string[] BTNavstr = new string[csubs];
        string BTPRstr;
        string FindBTinProfileF()
        {
            string s = "";

            if (Arr.AmplitudeAvailable && Arr.BottomTrackAvailable)//FindBTinProfile
            {   
                float XmtSize = Arr.A_FirstCellDepth;
                float BinSize = Arr.A_CellSize;
                if (BinSize <= 0)
                    BinSize = XmtSize;
                int re = (int)Arr.B_Status & 16;

                if (re == 16)
                {
                    if (Arr.E_FW_Vers[1] == 4)
                        XmtSize -= 200;
                    //else
                    //    if (Arr.E_FW_Vers[1] == 2)
                    //        XmtSize -= 5;
                    else
                        XmtSize -= 0;
                }

                if (XmtSize < 0)
                    XmtSize = 0;
                int XmtBins = 0;
                if(BinSize > 0)
                    XmtBins = (int)(XmtSize / BinSize);
                int HalfXmtBins = XmtBins / 2;
                XmtBins = 2*HalfXmtBins;
                int csE = (int)(Arr.E_CurrentSystem >> 24);
                float[] INT = new float[Arr.Bins[csE]];
                
                for (int beam=0;beam<Arr.Beams[csE];beam++)
                {
                    BPR_Range[beam] = 0;
                    BPR_SNR[beam] = 0;
                    BPR_beamVel[beam] = (float)88.88;
                    INT[0] = Arr.Amplitude[beam,1];//skip bin 0

                    for (int bin = 1; bin < Arr.Bins[csE]; bin++)
                    {
                        INT[bin] = INT[bin-1] + Arr.Amplitude[beam, bin];
                    }
                    float Max = -10000;
                    int MaxBin = 0;

                    //int A = 0;
                    //int B = XmtBins;
                    //int C = B + HalfXmtBins;
                    //int D = C + XmtBins;

                    int A = 1;
                    int B = 2 * HalfXmtBins + 1;
                    int C = 3 * HalfXmtBins + 1;
                    int D = 5 * HalfXmtBins + 1;
                    
                    for (int bin = 1; bin < Arr.Bins[csE] - (2*XmtBins + HalfXmtBins); bin++)
                    {
                        float Eval = (INT[B] - INT[A]) - (INT[D] - INT[C]);
                        
                        if (Max < Eval)
                        {
                            Max = Eval;
                            MaxBin = bin;
                        }
                        A++;
                        B++;
                        C++;
                        D++;
                    }

                    float MinA = 1000;

                    for (int bin = 1; bin < MaxBin; bin++)
                    {
                        if (MinA > Arr.Amplitude[beam, bin])
                            MinA = Arr.Amplitude[beam, bin];
                    }

                    int binA = MaxBin + HalfXmtBins / 2 + 1;
                    int binB = binA + HalfXmtBins;

                    //float XBins = (XmtSize / BinSize);

                    double BTVelocity = 0;
                    double BTAmplitude = 0;
                    //int MB = MaxBin - HalfXmtBins - 1;
                    //if (MB < 1)
                    //   MB = 1;
                    //double LeadEdgeAmp = Arr.Amplitude[beam, MB];

                    double [] BTtop = new double[HalfXmtBins];

                    string mess = "FindBT " + beam.ToString() + "," + MaxBin.ToString() + "," + HalfXmtBins.ToString() + "," + binA.ToString() + "," + binB.ToString() + "\r\n";
                    WriteMessageTxtSerial(mess, false);

                    int n = 0;
                    for (int bin = binA; bin < binB; bin++)
                    {
                        if (Arr.Velocity[beam, bin] < 88)
                        {
                            BTVelocity += Arr.Velocity[beam, bin];
                            BTAmplitude += Arr.Amplitude[beam, bin];
                            BTtop[n] = Arr.Amplitude[beam, bin];
                            n++;
                        }
                    }
                    if (n > 0)
                    {
                        BTVelocity /= (double)n;
                        BTAmplitude /= (double)n;
                        BPR_beamAmp[beam] = (float)BTAmplitude;
                    }
                    int offset = 0;
                    BPR_Range[beam] = Arr.A_FirstCellDepth + BinSize * (MaxBin - offset);
                    BPR_SNR[beam] = Max / ((float)2 * HalfXmtBins);
                    if (BPR_SNR[beam] > 15 && (BTAmplitude - MinA) > 5)
                    {
                        if (n > 0)
                            BPR_beamVel[beam] = (float)BTVelocity;
                        else
                            BPR_beamVel[beam] = Arr.Velocity[beam, MaxBin + HalfXmtBins];
                    }
                    else
                        BPR_Range[beam] = 1500 + 100*beam;

                }
                s += "\r\nrange      ";
                for (int beam = 0; beam < Arr.Beams[csE]; beam++)
                {
                    s += AddSpaces(BPR_Range[beam].ToString("0.000"), 11);
                }
                s += "\r\nsnr        ";
                for (int beam = 0; beam < Arr.Beams[csE]; beam++)
                {
                    s += AddSpaces(BPR_SNR[beam].ToString("0"), 11);
                }
                s += "\r\n";
            }
            return s;
        }

        void BottomStats(ArrayClass m)
        {
            if (!FormLoading && m.BottomTrackAvailable)
            {   
                int good = 0;
                for (int i = 0; i < m.B_Beams; i++)
                {
                    if(m.B_Velocity[i] < 80.0)
                    {
                        BTBeamGood[i] ++;
                        good++;
                    }
                }
                BTBeamN++;
                if (m.B_Beams == 4)
                {   
                    if (good == 4)
                    {
                        BTBeamErrorGood++;
                        double error = m.B_Velocity[0] + m.B_Velocity[1] - m.B_Velocity[2] - m.B_Velocity[3];
                        BTBeamErrorSum += error;
                        BTBeamErrorSumSqr += (error * error);
                    }
                }
            }
        }

        //float BTperr = 0;
        void ShowBottomTrackEnsemble(ArrayClass m, bool shownavanyway)
        {
            int i;
            
            textBoxEnsembleSub.Text = ProfileSubSystem.ToString();
            textBoxSeriesSub.Text = ProfileSubSystem.ToString();
            //int csE = (int)(m.E_CurrentSystem >> 24);
            int cs = ProfileSubSystem;

            string s = AncillaryString(m);
            
            if (m.SystemSetupDataAvailable)
            {
                s += "CPE   ";
                s += AddSpaces(((int)m.SystemSetup_BTCPCE).ToString(), 8);
                s += AddSpaces((m.SystemSetup_BTSystemFreqHz).ToString("0.00") + " Hz", 16);

                s += "\r\nNCE   ";
                s += AddSpaces(((int)m.SystemSetup_BTNCE).ToString(), 8);
                s += AddSpaces((m.SystemSetup_BTSamplesPerSecond).ToString("0.00") + " Hz", 16);

                s += "\r\nRPT   ";
                s += AddSpaces(((int)m.SystemSetup_BTRepeatN).ToString(), 8);
            }
            
            s += "\r\n";
            if (m.BottomTrackAvailable)
            {
                s += "Heading" + AddSpaces(m.B_Heading.ToString("0.00"), 10);
                s += "     Wtemp    " + AddSpaces(m.B_WaterTemperature.ToString("0.00"), 10);
                s += ", HS1" + AddSpaces(m.B_HS1Temperature.ToString("0.00"), 7);
                s += "     Pings " + AddSpaces(m.B_PingCount.ToString("0"), 4);
                s += "\r\n";

                s += "Pitch  " + AddSpaces(m.B_Pitch.ToString("0.00"), 10);
                s += "     Btemp    " + AddSpaces(m.B_BoardTemperature.ToString("0.00"), 10);
                s += ", HS2" + AddSpaces(m.B_HS2Temperature.ToString("0.00"), 7);
                s += "     Beams " + AddSpaces(m.B_Beams.ToString("0"), 4);
                s += "\r\n";
                s += "Roll   " + AddSpaces(m.B_Roll.ToString("0.00"), 10);
                s += "     Salinity " + AddSpaces(m.B_Salinity.ToString("0.00"), 10);
                s += "\r\n";
                s += "       " + "          ";
                s += "     Pressure " + AddSpaces(m.B_Pressure.ToString("0.000000"), 14);
                s += " 1st Ping  (s)" + AddSpaces(m.B_FirstPingSeconds.ToString("0.000"), 11);
                s += "\r\n";

                long st = (long)m.B_Status;
                s += "Status " + AddSpaces(st.ToString("X04"), 10);
                s += "     SOS      " + AddSpaces(m.B_SpeedOfSound.ToString("0.00"), 10);
                s += "     Last Ping (s)" + AddSpaces(m.B_LastPingSeconds.ToString("0.000"), 11);
                s += "\r\n";
                switch ((int)m.B_Beams)
                {
                    case 4:
                        s += "               Bm0/X/E    Bm1/Y/N    Bm2/Z/U    Bm3/Q/Q      N   good  percent     mean     s.d.";
                        break;
                    case 3:
                        s += "               Bm0/X/E    Bm1/Y/N    Bm2/Z/U";
                        break;
                }
                s += "\r\n";
                s += "Beam  Vel  ";
                for (i = 0; i < m.B_Beams; i++)
                    s += AddSpaces(m.B_Velocity[i].ToString("0.000"), 11);

                s += ", " + AddSpaces(BTBeamN.ToString(), 5);
                s += ", " + AddSpaces(BTBeamErrorGood.ToString(), 5);
                
                double percent = 0;
                if (m.B_Beams == 4)
                {
                    double sd = 0;
                    double mean = 0;

                    if (BTBeamErrorGood > 1)
                    {
                        percent = 100.0 * ((double)BTBeamErrorGood / (double)BTBeamN);
                        mean = BTBeamErrorSum / BTBeamErrorGood;
                        sd = Math.Sqrt((BTBeamErrorSumSqr - ((BTBeamErrorSum * BTBeamErrorSum) / BTBeamErrorGood)) / (BTBeamErrorGood - 1));
                    }
                    s += ", " + AddSpaces(percent.ToString("0.0"), 7);
                    s += ", " + AddSpaces(mean.ToString("0.000"), 7);
                    s += ", " + AddSpaces(sd.ToString("0.000"), 7);
                }
                
                //s += "\r\n";
                //s += "Beam  Vels ";
                //for (i = 0; i < m.B_Beams; i++)
                //    s += AddSpaces(m.B_VelS[i].ToString("0.000"), 11);
                
                s += "\r\nBm Good    ";
                for (i = 0; i < m.B_Beams; i++)
                    s += AddSpaces(m.B_BeamN[i].ToString("0"), 11);

                for (i = 0; i < m.B_Beams; i++)
                {
                    s += ", " + AddSpaces(BTBeamGood[i].ToString("0"), 5);
                    percent = 100.0 * ((double)BTBeamGood[i] / (double)BTBeamN);
                    s += ", " + AddSpaces(percent.ToString("0.0"), 4) + "%";
                }
                s += "\r\nInstr Vel  ";
                for (i = 0; i < m.B_Beams; i++)
                    s += AddSpaces(m.B_Instrument[i].ToString("0.000"), 11);
                s += "\r\nIn Good    ";
                for (i = 0; i < m.B_Beams; i++)
                    s += AddSpaces(m.B_XfrmN[i].ToString("0"), 11);

                //s += "\r\n";
                s += "      mag,       dir";
                s += "\r\nEarth Vel  ";
                for (i = 0; i < m.B_Beams; i++)
                    s += AddSpaces(m.B_Earth[i].ToString("0.000"), 11);

                s += AddSpaces(BTmag[cs].ToString("0.000"), 11);
                s += AddSpaces(BTdir[cs].ToString("0.000"), 11);
                s += "\r\nEa Good    ";
                for (i = 0; i < m.B_Beams; i++)
                    s += AddSpaces(m.B_EarthN[i].ToString("0"), 11);
                s += AddSpaces(bBTmag[cs].ToString("0.000"), 11);
                s += AddSpaces(bBTdir[cs].ToString("0.000"), 11);

                s += "\r\nSNR        ";
                for (i = 0; i < m.B_Beams; i++)
                    s += AddSpaces(m.B_SNR[i].ToString("0"), 11);
                s += AddSpaces(bBTmagI[cs].ToString("0.000"), 11);
                s += AddSpaces(bBTdirI[cs].ToString("0.000"), 11);
                s += "\r\nAmplitude  ";
                for (i = 0; i < m.B_Beams; i++)
                    s += AddSpaces(m.B_Amplitude[i].ToString("0"), 11);
                s += "\r\nCorrelation";
                for (i = 0; i < m.B_Beams; i++)
                    s += AddSpaces(m.B_Correlation[i].ToString("0.00"), 11);

                s += "\r\nRange      ";
                for (i = 0; i < m.B_Beams; i++)
                    s += AddSpaces(m.B_Range[i].ToString("0.000"), 11);

                

                if (FindBTinProfile)
                    s += BTPRstr;

                if ((NewGGA[cs] && FreshBT[cs]) || shownavanyway)
                {
                    BTNavstr[cs] = "\r\n";
                    //BTNavstr[cs] += "\r\n";
                    //BTdisMag[cs].ToString("0.000")
                    //GGANAV[cs].DMG.ToString("0.000")
                    BTNavstr[cs] += "SS = " + AddSpaces(cs.ToString(), 2) + "         DisE/X     DisN/Y     DisU/Z     DisMag     DisDir\r\n";
                    BTNavstr[cs] += "GPS                                         ";
                    BTNavstr[cs] += AddSpaces(GGANAV[cs].DMG.ToString("0.000"), 11); // + " m";
                    BTNavstr[cs] += AddSpaces(GGANAV[cs].CMG.ToString("0.000"), 11);// +" deg";
                    BTNavstr[cs] += "\r\n";
                    //Amplitude  "; 
                    BTNavstr[cs] += "BT ENU     ";
                    BTNavstr[cs] += AddSpaces(BTdisE[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(BTdisN[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(BTdisU[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(BTdisMag[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(BTdisDir[cs].ToString("0.000"), 11);

                    BTNavstr[cs] += ", " +BTnavGood[cs].ToString("");
                    BTNavstr[cs] += ", " +BTnavN[cs].ToString("");
                    double Pgood = 0;
                    if (BTnavN[cs] > 0)
                        Pgood = 100.0*((double)BTnavGood[cs] / (double)BTnavN[cs]);
                    BTNavstr[cs] += ", " + Pgood.ToString("0.00") + "% good";

                    float BTperr = (float)(100.0 * (BTdisMag[cs] / GGANAV[cs].DMG - 1));
                    BTNavstr[cs] += ", " + BTperr.ToString("0.00") + "% err";

                    BTNavstr[cs] += "\r\n";
                    if (RecalcBT)
                    {
                        BTNavstr[cs] += "bT ENU     ";
                        BTNavstr[cs] += AddSpaces(bBTdisE[cs].ToString("0.000"), 11);
                        BTNavstr[cs] += AddSpaces(bBTdisN[cs].ToString("0.000"), 11);
                        BTNavstr[cs] += AddSpaces(bBTdisU[cs].ToString("0.000"), 11);
                        BTNavstr[cs] += AddSpaces(bBTdisMag[cs].ToString("0.000"), 11);
                        BTNavstr[cs] += AddSpaces(bBTdisDir[cs].ToString("0.000"), 11);
                        BTNavstr[cs] += "\r\n";
                    }
                    BTNavstr[cs] += "BT XYZ     ";
                    BTNavstr[cs] += AddSpaces(BTdisX[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(BTdisY[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(BTdisZ[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(BTdisMagI[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(BTdisDirI[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += "\r\n";
                    if (RecalcBT)
                    {
                        BTNavstr[cs] += "bT XYZ     ";
                        BTNavstr[cs] += AddSpaces(bBTdisX[cs].ToString("0.000"), 11);
                        BTNavstr[cs] += AddSpaces(bBTdisY[cs].ToString("0.000"), 11);
                        BTNavstr[cs] += AddSpaces(bBTdisZ[cs].ToString("0.000"), 11);
                        BTNavstr[cs] += AddSpaces(bBTdisMagI[cs].ToString("0.000"), 11);
                        BTNavstr[cs] += AddSpaces(bBTdisDirI[cs].ToString("0.000"), 11);
                        BTNavstr[cs] += "\r\n";
                        //BTNavstr[cs] += "bT Bm      ";
                        //BTNavstr[cs] += AddSpaces(BTdisBm[0, cs].ToString("0.000"), 11);
                        //BTNavstr[cs] += AddSpaces(BTdisBm[1, cs].ToString("0.000"), 11);
                        //BTNavstr[cs] += AddSpaces(BTdisBm[2, cs].ToString("0.000"), 11);
                        //BTNavstr[cs] += AddSpaces(BTdisBm[3, cs].ToString("0.000"), 11);
                        //BTNavstr[cs] += "\r\n";
                    }
                    BTNavstr[cs] += "WP ENU     ";
                    BTNavstr[cs] += AddSpaces(WPdisE[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(WPdisN[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(WPdisU[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(WPdisMag[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(WPdisDir[cs].ToString("0.000"), 11);

                    BTNavstr[cs] += ", " + WPnavGood[cs].ToString("");
                    BTNavstr[cs] += ", " + WPnavN[cs].ToString("");
                    Pgood = 0;
                    if (WPnavN[cs] > 0)
                        Pgood = 100.0 * ((double)WPnavGood[cs] / (double)WPnavN[cs]);
                    BTNavstr[cs] += ", " + Pgood.ToString("0.00") + "% good";

                    

                    float WPperr = (float)(100.0*(WPdisMag[cs] / GGANAV[cs].DMG - 1));

                    BTNavstr[cs] += ", " + WPperr.ToString("0.00") + "% err";

                    BTNavstr[cs] += "\r\n";
                    BTNavstr[cs] += "WP XYZ     ";
                    BTNavstr[cs] += AddSpaces(WPdisX[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(WPdisY[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(WPdisZ[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(WPdisMagI[cs].ToString("0.000"), 11);
                    BTNavstr[cs] += AddSpaces(WPdisDirI[cs].ToString("0.000"), 11);

                    FreshBT[cs] = false;
                    NewGGA[cs] = false;
                }
                else
                    BTNavstr[cs] = "";


            }
            if (m.RTonWPAvailable)
            {
                s += "\r\nWPRT Range ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Range[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT SNR   ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_SNR[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Pings ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Pings[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Amp   ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Amp[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Cor   ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Cor[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Vel   ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Vel[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Ins   ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Ins[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Earth ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Earth[i].ToString("F3"), 11);
                }
                
            }

            s += BTNavstr[cs];
            if (ShowBottomTrack && GotData)
                textBoxProfile.Text = s;
        }

        private void checkBoxBTNAVRecalc_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBTNAVRecalc.Checked)
                RecalcBT = true;
            else
                RecalcBT = false;
        }

        //Gage Height --------------------------------------------------------
        void ShowGageEnsemble(ArrayClass m)
        {
            int i;
            
            //textBoxEnsembleSub.Text = ProfileSubSystem.ToString();
            //textBoxSeriesSub.Text = ProfileSubSystem.ToString();
            //int csE = (int)(m.E_CurrentSystem >> 24);

            string s = "";// AncillaryString(m);

            if (m.GageAvailable)
                s += "Gage Data\r\n";
            s += "\r\n";
            if (m.GageAvailable)
            {
                double un = m.RGH_sd;
                if (m.RGH_n > 0)
                    un /= Math.Sqrt(m.RGH_n);
                s += "V Range  " + AddSpaces(m.RGH_AvgRange.ToString("0.0000"), 11);
                s += " +-" + un.ToString("0.0000") + " m" + ", Status " + Convert.ToInt32(m.RGH_Status).ToString("X04");
                s += "\r\n";
                s += "P Depth  " + AddSpaces((m.RGH_Depth + m.RGH_DepthOffset).ToString("0.0000"), 11) + " m";
                s += "\r\n";
                s += "\r\n";
                
                s += "Amplitude Data:\r\n";
                s += "SNR      " + AddSpaces(m.RGH_AvgSN.ToString("0.00"), 9) + " dB";
                s += "\r\n";
                s += "S        " + AddSpaces(m.RGH_AvgS.ToString("0.00"), 9) + " dB";
                s += "\r\n";
                s += "N1       " + AddSpaces(m.RGH_AvgN1.ToString("0.00"), 9) + " dB";
                s += "\r\n";
                s += "N2       " + AddSpaces(m.RGH_AvgN2.ToString("0.00"), 9) + " dB";
                s += "\r\n";
                s += "GainFrac " + AddSpaces(m.RGH_GainFrac.ToString("0.000"), 10);
                s += "\r\n";
                s += "Good     " + AddSpaces(m.RGH_n.ToString(), 6);
                s += "\r\n";
                s += "\r\n";

                s += "User Parameters:\r\n";
                s += "Pings    " + AddSpaces(((int)Math.Round(m.RGH_Pings)).ToString(), 6);
                s += "\r\n";
                s += "SN Thres " + AddSpaces(m.RGH_SNthreshold.ToString("0.00"), 9) + " dB";
                s += "\r\n";
                s += "G Thres  " + AddSpaces(m.RGH_GainThres.ToString("0.00"), 9) + " dB";
                s += "\r\n";
                s += "sd Thres " + AddSpaces(m.RGH_StatThres.ToString("0.00"), 9) + " sigma";
                s += "\r\n";
                s += "Xmt cc   " + AddSpaces(((int)Math.Round(m.RGH_XmtCycles)).ToString(), 6);
                s += "\r\n";
                s += "\r\n";

                s += "Ancillary:\r\n";
                s += "Salinity " + AddSpaces(m.RGH_Salinity.ToString("0.00"), 9) + " ppt";
                s += "\r\n";
                s += "Pressure " + AddSpaces(m.RGH_Pressure.ToString("0.00000000"), 15) + " P";
                s += "\r\n";
                s += "Wtemp    " + AddSpaces(m.RGH_WaterTemperature.ToString("0.00"), 9) + " deg";
                s += "\r\n";
                s += "Btemp    " + AddSpaces(m.RGH_BackPlaneTemperature.ToString("0.00"), 9) + " deg";
                s += "\r\n";
                s += "SOS      " + AddSpaces(m.RGH_SOS.ToString("0.00"), 9) + " m/s";
                s += "\r\n";
                s += "Heading  " + AddSpaces(m.RGH_Heading.ToString("0.00"), 9) + " deg";
                s += "\r\n";
                s += "Pitch    " + AddSpaces(m.RGH_Pitch.ToString("0.00"), 9) + " deg";
                s += "\r\n";
                s += "Roll     " + AddSpaces(m.RGH_Roll.ToString("0.00"), 9) + " deg";
                s += "\r\n";
            }
            if (m.SystemSetupDataAvailable)
            {   
                s += "\r\n";
                s += "System:\r\n";
                s += "CPE   ";
                s += AddSpaces(((int)m.SystemSetup_BTCPCE).ToString(), 8);
                s += AddSpaces((m.SystemSetup_BTSystemFreqHz).ToString("0.00") + " Hz", 16);
                s += "\r\n";
                s += "NCE   ";
                s += AddSpaces(((int)m.SystemSetup_BTNCE).ToString(), 8);
                s += AddSpaces((m.SystemSetup_BTSamplesPerSecond).ToString("0.00") + " Hz", 16);
                s += "\r\n";
                s += "RPT   ";
                s += AddSpaces(((int)m.SystemSetup_BTRepeatN).ToString(), 8);
            }
            if (m.RTonWPAvailable)
            {
                s += "\r\n";
                s += "WPRT Range ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Range[i].ToString("F3"), 11);
                }
                s += "\r\n";
                s += "WPRT SNR   ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_SNR[i].ToString("F3"), 11);
                }
                s += "\r\n";
                s += "WPRT Pings ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Pings[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Amp   ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Amp[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Cor   ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Cor[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Vel   ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Vel[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Ins   ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Ins[i].ToString("F3"), 11);
                }
                s += "\r\nWPRT Earth ";
                for (i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += AddSpaces(m.RTonWP_Earth[i].ToString("F3"), 11);
                }
            }
            
            if (ShowBottomTrack && GotData)
                textBoxProfile.Text = s;
        }
        
        private void PNIwrite(byte[] packet,int offset, int count)
        {
            if (UsingSerial && _serialPort.IsOpen)
                _serialPort.Write(packet, offset, count);
            
        }

        Font drawFont = new Font("Courier New", (float)10, FontStyle.Regular, GraphicsUnit.Point);
        SolidBrush drawBrush = new SolidBrush(Color.Black);

        StringFormat drawFormat = new StringFormat();
        void Graphics_PlotFloatMatrix(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                                     float[,] Matrix, int rows, int col, float Yscale, float Yoffset, Pen FPen, string LegendStr, int LegendN, bool Left)
        {
            SolidBrush Brush = new SolidBrush(FPen.Color);
            float X, Y;

            float dXstep = ((X2 - X1) / rows);
            float dX = X1;
            float LastdX = dX;

            float LastY = Yoffset + (Yscale * (float)Matrix[col, 0]);
            if (LastY > Y2)
                LastY = Y2;
            if (LastY < Y1)
                LastY = Y1;

            for (int i = 0; i < rows; i++)
            {
                Y = Yoffset + (Yscale * (float)Matrix[col, i]);
                if (double.IsNaN(Y))
                    Y = 0;
                if (Y > Y2)
                    Y = Y2;
                if (Y < Y1)
                    Y = Y1;
                graphics.DrawLine(FPen, LastdX, LastY, dX, Y);
                LastY = Y;

                LastdX = dX;
                dX += dXstep;
            }
            if (LegendStr != "")
            {
                if (Left)
                    X = 0;
                else
                    X = X2;
                Y = Y1 + 15 * LegendN;
                graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
            }
        }
        


        void Graphics_InitPlot(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2, string TitleStr)
        {
            float X;
            float Y;
            //string drawString;

            graphics.FillRectangle(Brushes.White, X1, Y1, X2 - X1, Y2 - Y1);
            drawFormat.FormatFlags = StringFormatFlags.NoWrap;
            //drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            Y = Y1;
            //X = X1;

            //drawString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            //graphics.DrawString(drawString, drawFont, drawBrush, X, Y, drawFormat);

            //if (TitleStr)
            {

                X = X1 + (X2 - X1) / 2 - drawFont.SizeInPoints * TitleStr.Length / 2;

                graphics.DrawString(TitleStr, drawFont, drawBrush, X, Y, drawFormat);
            }
        }
        void Graphics_PlotDoubleMatrix(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                                     double[,] Matrix, int rows, int col, float Yscale, float Yoffset, Pen FPen, string LegendStr, int LegendN, bool Left)
        {
            SolidBrush Brush = new SolidBrush(FPen.Color);
            float X, Y;

            float dXstep = ((X2 - X1) / rows);
            float dX = X1;
            float LastdX = dX;

            float LastY = Yoffset + (Yscale * (float)Matrix[col, 0]);
            if (LastY > Y2)
                LastY = Y2;
            if (LastY < Y1)
                LastY = Y1;

            for (int i = 0; i < rows; i++)
            {
                Y = Yoffset + (Yscale * (float)Matrix[col, i]);
                if (double.IsNaN(Y))
                    Y = 0;
                if (Y > Y2)
                    Y = Y2;
                if (Y < Y1)
                    Y = Y1;
                graphics.DrawLine(FPen, LastdX, LastY, dX, Y);
                LastY = Y;

                LastdX = dX;
                dX += dXstep;
            }
            if (LegendStr != "")
            {
                if (Left)
                    X = 0;
                else
                    X = X2;
                Y = Y1 + 15 * LegendN;
                graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
            }
        }
        void Graphics_PlotDouble(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                                     double[] Matrix, int rows, float Yscale, float Yoffset, Pen FPen, string LegendStr, int LegendN, bool Left, bool Wrap)
        {
            SolidBrush Brush = new SolidBrush(FPen.Color);
            float X, Y;

            float dXstep = ((X2 - X1) / rows);
            float dX = X1;
            float LastdX = dX;

            float LastY = 0;
            int n;
            for (n = 0; n < rows; n++)
            {
                LastY = Yoffset + (Yscale * (float)Matrix[n]);
                if (double.IsNaN(LastY))
                {
                    LastY = Y1;
                    LastdX = dX;
                    dX += dXstep;
                }
                else
                    break;
            }
            if (Wrap)
            {
                for (int i = n; i < rows; i++)
                {
                    Y = Yoffset + (Yscale * (float)Matrix[i]);
                    if (!double.IsNaN(Y))
                    {
                        if (Math.Abs(LastY - Y) > (Y2 - Y1) / 2)//need to wrap
                        {
                            float x1, x2, y1, y2, m;
                            if ((Y2 - Y) > (Y - Y1))//closer to Y1
                            {
                                y1 = LastY - (Y2 - Y1);
                                y2 = Y;
                                x1 = LastdX;
                                x2 = dX;
                                m = (y2 - y1) / (x2 - x1);
                                x1 += (Y1 - y1) / m;
                                y1 = Y1;
                                graphics.DrawLine(FPen, x1, y1, x2, y2);
                                //
                                y1 = LastY;
                                x1 = LastdX;
                                x2 = x1 + (Y2 - y1) / m;
                                y2 = Y2;
                                graphics.DrawLine(FPen, x1, y1, x2, y2);
                            }
                            else//closer to Y2
                            {
                                y1 = LastY + (Y2 - Y1);
                                y2 = Y;
                                x1 = LastdX;
                                x2 = dX;
                                m = (y2 - y1) / (x2 - x1);
                                x1 += (Y2 - y1) / m;
                                y1 = Y2;
                                graphics.DrawLine(FPen, x1, y1, x2, y2);
                                //
                                y1 = LastY;
                                y2 = Y1;
                                x1 = LastdX;
                                x2 = x1 + (Y1 - y1) / m;
                                graphics.DrawLine(FPen, x1, y1, x2, y2);
                            }
                        }
                        else
                        {
                            graphics.DrawLine(FPen, LastdX, LastY, dX, Y);
                        }
                        LastY = Y;
                    }
                    LastdX = dX;
                    dX += dXstep;
                }
            }
            else
            {
                if (LastY > Y2)
                    LastY = Y2;
                if (LastY < Y1)
                    LastY = Y1;
                for (int i = n; i < rows; i++)
                {
                    Y = Yoffset + (Yscale * (float)Matrix[i]);
                    if (!double.IsNaN(Y))
                    {
                        if (Y > Y2)
                            Y = Y2;
                        if (Y < Y1)
                            Y = Y1;
                        graphics.DrawLine(FPen, LastdX, LastY, dX, Y);
                        LastY = Y;
                    }
                    LastdX = dX;
                    dX += dXstep;
                }
            }
            if (LegendStr != "")
            {
                if (Left)
                    X = 0;
                else
                    X = X2;
                Y = Y1 + 15 * LegendN;
                graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
            }
        }
        void Graphics_PlotDoubleXY(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                                     double[] Xmatrix, double[] Ymatrix, int rows, float Xscale, float Xoffset, float Yscale, float Yoffset, Pen FPen, string LegendStr, int LegendN, bool Left)
        {
            SolidBrush Brush = new SolidBrush(FPen.Color);
            float X, Y;

            float LastX = 0;
            float LastY = 0;
            int n;
            for (n = 0; n < rows; n++)
            {
                LastX = Xoffset + (Xscale * (float)Xmatrix[n]);
                LastY = Yoffset + (Yscale * (float)Ymatrix[n]);

                if (double.IsNaN(LastY) || double.IsNaN(LastX))
                {
                    LastX = X1;
                    LastY = Y1;
                }
                else
                    break;
            }

            for (int i = n; i < rows; i++)
            {
                X = Xoffset + (Xscale * (float)Xmatrix[i]);
                Y = Yoffset + (Yscale * (float)Ymatrix[i]);
                if (double.IsNaN(Y) && double.IsNaN(Y))
                {
                    //X = 0;
                    //Y = 0;
                }
                else
                {
                    if (X > X2)
                        X = X2;
                    if (X < X1)
                        X = X1;
                    if (Y > Y2)
                        Y = Y2;
                    if (Y < Y1)
                        Y = Y1;
                    graphics.DrawLine(FPen, LastX, LastY, X, Y);
                    LastX = X;
                    LastY = Y;
                }
            }
            if (LegendStr != "")
            {
                if (Left)
                    X = 0;
                else
                    X = X2;
                Y = Y1 + 15 * LegendN;
                graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
            }
        }
        void Graphics_PlotDoubleData(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                                     double[,] Data, int MagN, int beam, int d, float Yscale, float Yoffset,
                                     Pen FPen, string LegendStr, int LegendN, bool Left)
        {
            SolidBrush Brush = new SolidBrush(FPen.Color);
            float X, Y;

            float dXstep = ((X2 - X1) / MagN);
            float dX = X1;
            float LastdX = dX;

            float LastY = Yoffset + (Yscale * (float)Data[d, beam]);
            if (LastY > Y2)
                LastY = Y2;
            if (LastY < Y1)
                LastY = Y1;

            for (int i = d; i < MagN + d; i++)
            {
                Y = Yoffset + (Yscale * (float)Data[i, beam]);
                if (double.IsNaN(Y))
                    Y = 0;
                if (Y > Y2)
                    Y = Y2;
                if (Y < Y1)
                    Y = Y1;
                graphics.DrawLine(FPen, LastdX, LastY, dX, Y);
                LastY = Y;

                LastdX = dX;
                dX += dXstep;
            }
            /*
            if (LegendStr != "")
            {
                X = X2 + 60;
                Y = Y1 + 15 * LegendN;
                graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
            }
            */
            if (LegendStr != "")
            {
                if (Left)
                    X = 0;
                else
                    X = X2;
                Y = Y1 + 15 * LegendN;
                graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
            }
        }

        void Graphics_PlotTripleDoubleData(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                                     double[, ,] Data, int a, int b, int c, float Yscale, float Yoffset, Pen FPen, string LegendStr, int LegendN)
        {
            SolidBrush Brush = new SolidBrush(FPen.Color);
            float X, Y;

            float dXstep = ((X2 - X1) / c);
            float dX = X1;
            float LastdX = dX;

            float LastY = Yoffset + (Yscale * (float)Data[a, b, 0]);
            if (LastY > Y2)
                LastY = Y2;
            if (LastY < Y1)
                LastY = Y1;

            for (int i = 0; i < c; i++)
            {
                Y = Yoffset + (Yscale * (float)Data[a, b, i]);
                if (Y > Y2)
                    Y = Y2;
                if (Y < Y1)
                    Y = Y1;
                graphics.DrawLine(FPen, LastdX, LastY, dX, Y);
                LastY = Y;

                LastdX = dX;
                dX += dXstep;
            }
            if (LegendStr != "")
            {
                X = X2 + 60;
                Y = Y1 + 15 * LegendN;
                graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
            }
        }

        void Graphics_PlotTripleIntegerData(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                                     int[, ,] Data, int a, int b, int c, int d, float Yscale, float Yoffset, Pen FPen, string LegendStr, int LegendN)
        {
            SolidBrush Brush = new SolidBrush(FPen.Color);
            float X, Y;

            float dXstep = ((X2 - X1) / c);
            float dX = X1;
            float LastdX = dX;

            float LastY = Yoffset + (Yscale * (float)Data[a, b, 0]);
            if (LastY > Y2)
                LastY = Y2;
            if (LastY < Y1)
                LastY = Y1;

            for (int i = 0; i < c; i++)
            {
                Y = Yoffset + (Yscale * (float)Data[a, b, d]);
                if (Y > Y2)
                    Y = Y2;
                if (Y < Y1)
                    Y = Y1;
                graphics.DrawLine(FPen, LastdX, LastY, dX, Y);
                LastY = Y;

                LastdX = dX;
                dX += dXstep;
                d++;
            }
            if (LegendStr != "")
            {
                X = X2 + 60;
                Y = Y1 + 15 * LegendN;
                graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
            }
        }

        void Graphics_PlotGrid_Profile(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                           float Vsteps, float VvalL, float VvalStepL, string LeftScaleStr, float VvalR, float VvalStepR, string RightScaleStr,
                           float Hsteps, float Hval, float HvalStep, float Hrange, string HscaleStr, bool Xcentered)
        {
            if ((Vsteps > 0) && (Y2 > Y1))
            {
                int i;
                string s;
                float step = ((Y2 - Y1) / Vsteps);
                float X, Y;
                float VvalTL = VvalL;
                float VvalTR = VvalR;

                if (double.IsNaN(VvalTL))
                    VvalTL = 0;
                if (double.IsNaN(VvalTR))
                    VvalTR = 0;

                //Left vertical Y axis
                X = X1 - LeftScaleStr.Length * drawFont.SizeInPoints;
                Y = Y1 - 2 * drawFont.SizeInPoints;
                graphics.DrawString(LeftScaleStr, drawFont, drawBrush, X, Y, drawFormat);

                //Right vertical Y axis
                X = X2;
                graphics.DrawString(RightScaleStr, drawFont, drawBrush, X, Y, drawFormat);

                //draw vertical grid
                graphics.DrawLine(System.Drawing.Pens.Gray, X1, Y1, X1, Y2);
                graphics.DrawLine(System.Drawing.Pens.Gray, X2, Y1, X2, Y2);
                bool Doit = true;
                int LD = 0;

                if (VvalTL < (float)0.2)
                    LD = 3;
                else
                    if (VvalTL < 1)
                    LD = 2;
                else
                        if (VvalTL < 10)
                    LD = 1;
                else
                            if (VvalTL < 100)
                    LD = 0;

                for (Y = Y1; (int)Y <= (int)Y2; Y += step)
                {
                    if ((int)Y <= (int)Y2 && Doit)
                    {
                        Doit = false;
                        if (LeftScaleStr != "")
                        {
                            switch (LD)
                            {
                                case 0:
                                    s = VvalTL.ToString("0");
                                    break;
                                case 1:
                                    s = VvalTL.ToString("0.0");
                                    break;
                                case 2:
                                default:
                                    s = VvalTL.ToString("0.00");
                                    break;
                                case 3:
                                    s = VvalTL.ToString("0.000");
                                    break;
                            }

                            while (s.Length < 7)
                                s = " " + s;

                            graphics.DrawString(s, drawFont, drawBrush, X1 - (s.Length) * drawFont.SizeInPoints, Y - 8, drawFormat);
                        }
                        if (RightScaleStr != "")
                        {
                            s = VvalTR.ToString("");
                            while (s.Length < 3)
                                s = " " + s;
                            graphics.DrawString(s, drawFont, drawBrush, X2, Y - 8, drawFormat);
                        }
                    }
                    else
                        Doit = true;
                    graphics.DrawLine(System.Drawing.Pens.Gray, X1, Y, X2, Y);
                    VvalTL += VvalStepL;
                    VvalTR += VvalStepR;
                }

                //draw and label horizontal grid
                float Hscale = (X2 - X1) / Hrange;
                float Hoffset = X1;
                float HvalT = Hval;

                if (Xcentered)
                    Hoffset += (X2 - X1) / 2;
                else
                    if (HvalT < 0)
                    Hoffset = X2;

                for (i = 0; i < Hsteps; i++)
                {
                    X = Hoffset + HvalT * Hscale;

                    graphics.DrawLine(System.Drawing.Pens.Gray, X, Y1, X, Y2);

                    HvalT += HvalStep;
                }

                bool On = true;
                int HD = 0;
                if (HvalT <= 0.1)
                    HD = 3;
                else
                    if (HvalT < 1)
                    HD = 2;
                else
                        if (HvalT < 2)
                    HD = 1;
                else
                            if (HvalT < 4)
                    HD = 0;
                if (HscaleStr != "")
                {
                    HvalT = Hval;

                    for (i = 0; i < Hsteps + 1; i++)
                    {
                        if (On)
                        {
                            X = Hoffset + HvalT * Hscale;
                            switch (HD)
                            {
                                case 0:
                                    s = HvalT.ToString("0");
                                    break;
                                case 1:
                                    s = HvalT.ToString("0.0");
                                    break;
                                case 2:
                                default:
                                    s = HvalT.ToString("0.00");
                                    break;
                                case 3:
                                    s = HvalT.ToString("0.000");
                                    break;
                            }

                            graphics.DrawString(s, drawFont, drawBrush, X - drawFont.SizeInPoints * (s.Length) / 2, Y2 + 10, drawFormat);
                            On = false;
                        }
                        else
                            On = true;
                        HvalT += HvalStep;
                    }

                    X = X1 + (X2 - X1) / 2 - drawFont.SizeInPoints * (HscaleStr.Length - 1) / 2;
                    Y = Y2 + 30;
                    graphics.DrawString(HscaleStr, drawFont, drawBrush, X, Y, drawFormat);
                }
            }
        }

        void Graphics_PlotGrid(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                          float Vsteps, float VvalL, float VvalStepL, string LeftScaleStr, float VvalR, float VvalStepR, string RightScaleStr,
                          float Hsteps, float Hval, float HvalStep, float Hrange, string HscaleStr, bool Xcentered)
        {
            int i;
            string s;
            float step = ((Y2 - Y1) / Vsteps);
            float X, Y;
            float VvalTL = VvalL;
            float VvalTR = VvalR;

            //draw vertical grid
            float y2 = Y2;
            if (HscaleStr == "")
                y2 -= step;

            for (Y = Y1; Y <= Y2; Y += step)
            {
                graphics.DrawLine(System.Drawing.Pens.LightGray, X1, Y, X2, Y);

                if (Y <= y2)
                {
                    if (LeftScaleStr != "")
                    {
                        s = VvalTL.ToString();
                        while (s.Length < 3)
                            s = " " + s;

                        s += LeftScaleStr;

                        graphics.DrawString(s, drawFont, drawBrush, X1, Y - drawFont.SizeInPoints + 1, drawFormat);
                    }
                    if (RightScaleStr != "")
                    {   
                        s = VvalTR.ToString() + RightScaleStr;
                        graphics.DrawString(s, drawFont, drawBrush, X2 - drawFont.SizeInPoints * s.Length, Y - drawFont.SizeInPoints + 1, drawFormat);
                    }
                }
                VvalTL += VvalStepL;
                VvalTR += VvalStepR;
            }

            //draw and label horizontal grid
            float Hscale = (X2 - X1) / Hrange;
            float Hoffset = X1;
            float HvalT = 0;// Hval;

            if (Xcentered)
            {
                Hoffset += (X2 - X1) / 2;
                HvalT = Hval;
            }

            for (i = 0; i < Hsteps - 1; i++)
            {
                X = Hoffset + HvalT * Hscale;

                graphics.DrawLine(System.Drawing.Pens.LightGray, X, Y1, X, Y2);

                HvalT += HvalStep;
            }
            graphics.DrawLine(System.Drawing.Pens.LightGray, X1, Y1, X1, Y2);
            graphics.DrawLine(System.Drawing.Pens.LightGray, X2, Y1, X2, Y2);

            if (HscaleStr != "")
            {
                if (Xcentered)
                {
                    HvalT = Hval;
                }
                else
                {
                    HvalT = 0;
                }

                for (i = 0; i < Hsteps - 1; i++)
                {
                    X = Hoffset + HvalT * Hscale;

                    s = Hval.ToString();

                    //graphics.DrawString(s, drawFont, drawBrush, X - drawFont.SizeInPoints * s.Length / 2, Y2 - 3 * drawFont.SizeInPoints - 1, drawFormat);
                    graphics.DrawString(s, drawFont, drawBrush, X - drawFont.SizeInPoints * s.Length / 2, Y2 + (float)1 * drawFont.SizeInPoints, drawFormat);

                    HvalT += HvalStep;
                    Hval += HvalStep;
                }

                X = X1 + (X2 - X1) / 2 - drawFont.SizeInPoints * HscaleStr.Length / 2;
                //Y = Y2 + (float)3 * drawFont.SizeInPoints;// -(float)1.5 * drawFont.SizeInPoints - 1;
                Y = Y2;// + (float)1 * drawFont.SizeInPoints;// -(float)1.5 * drawFont.SizeInPoints - 1;
                graphics.DrawString(HscaleStr, drawFont, drawBrush, X, Y, drawFormat);
            }
        }
        void Graphics_PlotWavesGrid(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                         float Vsteps, float VvalLa, float VvalStepLa, string LeftScaleStra,
                                       float VvalLb, float VvalStepLb, string LeftScaleStrb,
                                       float VvalRa, float VvalStepRa, string RightScaleStra,
                                       float VvalRb, float VvalStepRb, string RightScaleStrb,
                                       float VvalRc, float VvalStepRc, string RightScaleStrc,
                         float Hsteps, float Hval, float HvalStep, float Hrange, string HscaleStr, bool Xcentered)
        {
            int i;
            string s, sb;
            float step = ((Y2 - Y1) / Vsteps);
            float X, Y;
            float VvalTLa = VvalLa;
            float VvalTLb = VvalLb;
            float VvalTRa = VvalRa;
            float VvalTRb = VvalRb;
            float VvalTRc = VvalRc;

            //draw vertical grid
            float y2 = Y2;
            if (HscaleStr == "")
                y2 -= step;

            for (Y = Y1; Y <= Y2; Y += step)
            {
                graphics.DrawLine(System.Drawing.Pens.LightGray, X1, Y, X2, Y);

                if (Y <= y2)
                {
                    if (LeftScaleStra != "")
                    {
                        s = VvalTLa.ToString();
                        s += LeftScaleStra;
                        if (LeftScaleStrb != "")
                        {
                            sb = VvalTLb.ToString();
                            sb += LeftScaleStrb;
                            s += "," + sb;
                        }

                        graphics.DrawString(s, drawFont, drawBrush, X1 - drawFont.SizeInPoints * s.Length, Y - drawFont.SizeInPoints, drawFormat);
                    }
                    if (RightScaleStra != "")
                    {
                        s = VvalTRa.ToString() + RightScaleStra;
                        if (RightScaleStrb != "")
                        {
                            sb = VvalTRb.ToString();
                            sb += RightScaleStrb;
                            s += "," + sb;
                            if (RightScaleStrc != "")
                            {
                                sb = VvalTRc.ToString();
                                sb += RightScaleStrc;
                                s += "," + sb;
                            }
                        }
                        graphics.DrawString(s, drawFont, drawBrush, X2, Y - drawFont.SizeInPoints, drawFormat);
                    }
                }
                VvalTLa += VvalStepLa;
                VvalTLb += VvalStepLb;
                VvalTRa += VvalStepRa;
                VvalTRb += VvalStepRb;
                VvalTRc += VvalStepRc;
            }

            //draw and label horizontal grid
            float Hscale = (X2 - X1) / Hrange;
            float Hoffset = X1;
            float HvalT = 0;// Hval;

            if (Xcentered)
            {
                Hoffset += (X2 - X1) / 2;
                HvalT = Hval;
            }

            for (i = 0; i < Hsteps - 1; i++)
            {
                X = Hoffset + HvalT * Hscale;

                graphics.DrawLine(System.Drawing.Pens.LightGray, X, Y1, X, Y2);

                HvalT += HvalStep;
            }

            if (HscaleStr != "")
            {
                if (Xcentered)
                {
                    HvalT = Hval;
                }
                else
                {
                    HvalT = 0;
                }

                for (i = 0; i < Hsteps - 1; i++)
                {
                    X = Hoffset + HvalT * Hscale;

                    s = Hval.ToString("0.00");

                    graphics.DrawString(s, drawFont, drawBrush, X - drawFont.SizeInPoints * s.Length / 2, Y2 + (float)1 * drawFont.SizeInPoints, drawFormat);

                    HvalT += HvalStep;
                    Hval += HvalStep;
                }

                X = X1 + (X2 - X1) / 2 - drawFont.SizeInPoints * HscaleStr.Length / 2;
                Y = Y2 + (float)3 * drawFont.SizeInPoints;
                graphics.DrawString(HscaleStr, drawFont, drawBrush, X, Y, drawFormat);
            }
        }

        void Graphics_PlotGridSeries(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                         float Vsteps, float VvalL, float VvalStepL, string LeftScaleStr, float VvalR, float VvalStepR, string RightScaleStr, int RightScaleLen,
                         float Hsteps, float Hval, float HvalStep, float Hrange, string HscaleStr, bool Xcentered)
        {
            if ((Y2 > Y1) && (Vsteps > 0))
            {
                int i;
                string s;
                float step = ((Y2 - Y1) / Vsteps);
                float X, Y;
                float VvalTL = VvalL;
                float VvalTR = VvalR;
                int NDL;
                int NDR;

                float val0 = Math.Abs(VvalL);
                float val1 = Math.Abs(VvalL + Vsteps * VvalStepL);

                if (val0 < val1)
                    val0 = val1;

                //NDL = 2;
                if (val0 >= 10)
                    NDL = 1;
                else if (val0 > 2)
                    NDL = 1;
                else if (val0 >= 1)
                    NDL = 2;
                else if (val0 >= 0.1)
                    NDL = 3;
                else //if (val0 > 0.5)
                    NDL = 4;

                val0 = Math.Abs(VvalR);
                val1 = Math.Abs(VvalR + Vsteps * VvalStepR);

                if (val0 < val1)
                    val0 = val1;
                if (val0 > 10)
                    NDR = 0;
                else
                    if (val0 > 2)
                    NDR = 1;
                else
                    NDR = 2;

                //draw vertical grid            

                for (Y = Y1; Y <= Y2; Y += step)
                {
                    graphics.DrawLine(System.Drawing.Pens.LightGray, X1 - 5, Y, X2 + 5, Y);

                    if (Y <= Y2)
                    {
                        if (LeftScaleStr != "")
                        {
                            switch (NDL)
                            {
                                default:
                                case 0:
                                    s = VvalTL.ToString();
                                    break;
                                case 1:
                                    s = VvalTL.ToString("0.0");
                                    break;
                                case 2:
                                    s = VvalTL.ToString("0.00");
                                    break;
                                case 3:
                                    s = VvalTL.ToString("0.000");
                                    break;
                                case 4:
                                    s = VvalTL.ToString("0.0000");
                                    break;
                            }
                            while (s.Length < 4)
                                s = " " + s;

                            s += LeftScaleStr;

                            graphics.DrawString(s, drawFont, drawBrush, X1 - drawFont.SizeInPoints * s.Length, Y - drawFont.SizeInPoints, drawFormat);
                        }
                        if (RightScaleStr != "")
                        {
                            switch (NDR)
                            {
                                default:
                                case 0:
                                    s = VvalTR.ToString();
                                    break;
                                case 1:
                                    s = VvalTR.ToString("0.0");
                                    break;
                                case 2:
                                    s = VvalTR.ToString("0.00");
                                    break;
                            }
                            while (s.Length < RightScaleLen)
                                s = " " + s;

                            s += RightScaleStr;

                            //s = VvalTR.ToString() + RightScaleStr;
                            graphics.DrawString(s, drawFont, drawBrush, X2, Y - drawFont.SizeInPoints, drawFormat);
                        }
                    }
                    VvalTL += VvalStepL;
                    VvalTR += VvalStepR;
                }

                //draw and label horizontal grid
                float Hscale = (X2 - X1) / Hrange;
                float Hoffset = X1;
                float HvalT = Hval;

                if (Xcentered)
                    Hoffset += (X2 - X1) / 2;

                for (i = 0; i < Hsteps; i++)
                {
                    X = Hoffset + HvalT * Hscale;

                    graphics.DrawLine(System.Drawing.Pens.LightGray, X, Y1, X, Y2);

                    HvalT += HvalStep;
                }

                if (HscaleStr != "")
                {
                    HvalT = Hval;

                    for (i = 0; i < Hsteps; i++)
                    {
                        X = Hoffset + HvalT * Hscale;

                        s = HvalT.ToString();

                        graphics.DrawString(s, drawFont, drawBrush, X - drawFont.SizeInPoints * s.Length / 2, Y2, drawFormat);

                        HvalT += HvalStep;
                    }

                    X = X1 + (X2 - X1) / 2 - drawFont.SizeInPoints * HscaleStr.Length / 2;
                    Y = Y2 - (float)1.5 * drawFont.SizeInPoints - 1;
                    graphics.DrawString(HscaleStr, drawFont, drawBrush, X, Y, drawFormat);
                }
            }
        }

        //Series--------------------------------------------------------------
        private void radioButtonSeriesProfileVel_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesProfileVel.Checked)
            {
                SeriesDataType = TypeVel;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesProfileAmp_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesProfileAmp.Checked)
            {
                SeriesDataType = TypeAmp;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesProfileCor_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesProfileCor.Checked)
            {
                SeriesDataType = TypeCor;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesCoordBeam_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesCoordBeam.Checked)
            {
                SeriesCoordinateState = SeriesBeam;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesCoordXYZ_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesCoordXYZ.Checked)
            {
                SeriesCoordinateState = SeriesXYZ;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesCoordENU_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesCoordENU.Checked)
            {
                SeriesCoordinateState = SeriesENU;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesProfile_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesProfile.Checked)
            {
                SeriesDataSource = SourceProfile;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesBT_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesBT.Checked)
            {
                SeriesDataSource = SourceBT;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesWT_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesWT.Checked)
            {
                SeriesDataSource = SourceWT;
                ShowSeries(Arr, Arr2);
            }
        }
        private void textBoxSeriesBin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SeriesBin = Convert.ToInt32(textBoxSeriesBin.Text);
                ShowSeries(Arr, Arr2);
            }
            catch { }
        }        
        private void buttonSeriesBinPlus_Click(object sender, EventArgs e)
        {
            SeriesBin++;
            if (SeriesBin > Arr.E_Cells - 1)
                SeriesBin = (int)Arr.E_Cells - 1;
            if (SeriesBin < 0)
                SeriesBin = 0;
            textBoxSeriesBin.Text = SeriesBin.ToString();// +"\r";
            ShowSeries(Arr, Arr2);
        }
        private void buttonSeriesBinMinus_Click(object sender, EventArgs e)
        {
            SeriesBin--;
            if (SeriesBin < 0)
                SeriesBin = 0;
            textBoxSeriesBin.Text = SeriesBin.ToString();// +"\r";
            ShowSeries(Arr, Arr2);
        }
        private void radioButtonSeriesBTvel_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesBTvel.Checked)
            {
                SeriesBTDataType = TypeBTvel;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesBTamp_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesBTamp.Checked)
            {
                SeriesBTDataType = TypeBTamp;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesBTcor_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesBTcor.Checked)
            {
                SeriesBTDataType = TypeBTcor;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesBTsnr_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesBTsnr.Checked)
            {
                SeriesBTDataType = TypeBTsnr;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesBTrange_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesBTrange.Checked)
            {
                SeriesBTDataType = TypeBTrange;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesAncillaryBT_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesAncillaryBT.Checked)
            {
                SeriesDataSource = SourceAncillaryBT;
                ShowSeries(Arr, Arr2);
            }
        }
        private void radioButtonSeriesWPBT_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesWPRT.Checked)
            {
                SeriesDataSource = SourceWPBT;
                ShowSeries(Arr, Arr2);
            }
        }

        private void radioButtonSeriesAncillaryProfile_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesAncillaryProfile.Checked)
            {
                SeriesDataSource = SourceAncillaryProfile;
                ShowSeries(Arr, Arr2);
            }
        }
        private void buttonSeriesPlus_Click(object sender, EventArgs e)
        {
            
            switch (SeriesDataSource)
            {
                case SourceWPBT:
                    RSpanBTIndex++;
                    if (RSpanBTIndex >= RSpanBTIndexCount)
                        RSpanBTIndex = RSpanBTIndexCount - 1;
                    RSpanBT = (float)RSpanBTSetting[RSpanBTIndex];
                    break;
                case SourceAncillaryProfile:
                    PSpanSeriesIndex++;
                    if (PSpanSeriesIndex >= PSpanSeriesIndexCount)
                        PSpanSeriesIndex = PSpanSeriesIndexCount - 1;
                    PSpanSeries = (float)PSpanSeriesSetting[PSpanSeriesIndex];
                    break;
                case SourceProfile:
                    switch (SeriesDataType)
                    {
                        case TypeVel:
                            VSpanSeriesIndex++;
                            if (VSpanSeriesIndex >= VSpanSeriesIndexCount)
                                VSpanSeriesIndex = VSpanSeriesIndexCount - 1;
                            VSpanSeries = (float)VSpanSeriesSetting[VSpanSeriesIndex];
                            break;

                    }
                    break;
                case SourceBT:
                    switch (SeriesBTDataType)
                    {
                        case TypeBTmag:
                        case TypeBTvel:
                            VSpanBTIndex++;
                            if (VSpanBTIndex >= VSpanBTIndexCount)
                                VSpanBTIndex = VSpanBTIndexCount - 1;
                            VSpanBT = (float)VSpanBTSetting[VSpanBTIndex];
                            break;
                        case TypeBTrange:
                            RSpanBTIndex++;
                            if (RSpanBTIndex >= RSpanBTIndexCount)
                                RSpanBTIndex = RSpanBTIndexCount - 1;
                            RSpanBT = (float)RSpanBTSetting[RSpanBTIndex];
                            break;
                    }
                    break;
            }

            ShowSeries(Arr, Arr2);
        }
        private void buttonSeriesMinus_Click(object sender, EventArgs e)
        {
            switch (SeriesDataSource)
            {
                case SourceWPBT:
                    RSpanBTIndex--;
                    if (RSpanBTIndex < 0)
                        RSpanBTIndex = 0;
                    RSpanBT = (float)RSpanBTSetting[RSpanBTIndex];
                    break;
                case SourceAncillaryProfile:
                    PSpanSeriesIndex--;
                    if (PSpanSeriesIndex < 0)
                        PSpanSeriesIndex = 0;
                    PSpanSeries = (float)PSpanSeriesSetting[PSpanSeriesIndex];
                    break;
                case SourceProfile:
                    switch (SeriesDataType)
                    {
                        case TypeVel:
                            VSpanSeriesIndex--;
                            if (VSpanSeriesIndex < 0)
                                VSpanSeriesIndex = 0;
                            VSpanSeries = (float)VSpanSeriesSetting[VSpanSeriesIndex];
                            break;
                    }
                    break;
                case SourceBT:
                    switch (SeriesBTDataType)
                    {
                        case TypeBTmag:
                        case TypeBTvel:
                            VSpanBTIndex--;
                            if (VSpanBTIndex < 0)
                                VSpanBTIndex = 0;
                            VSpanBT = (float)VSpanBTSetting[VSpanBTIndex];
                            break;
                        case TypeBTrange:
                            RSpanBTIndex--;
                            if (RSpanBTIndex < 0)
                                RSpanBTIndex = 0;
                            RSpanBT = (float)RSpanBTSetting[RSpanBTIndex];
                            break;
                    }
                    break;
            }

            ShowSeries(Arr, Arr2);
        }
        private void buttonClearSeries_Click(object sender, EventArgs e)
        {
            ClearSeries();
            ShowSeries(Arr, Arr2);
        }

        int SeriesBin = 0;
        const int MaxSeries = 200;
        int[] SeriesIndex = new int[csubs];

        float[, , ,] BeamSeries = new float[csubs, 4, cbins, MaxSeries];
        float[, , ,] InstrumentSeries = new float[csubs, 4, cbins, MaxSeries];
        float[, , ,] EarthSeries = new float[csubs, 4, cbins, MaxSeries];
        float[, , ,] AmpSeries = new float[csubs, 4, cbins, MaxSeries];
        float[, , ,] CorSeries = new float[csubs, 4, cbins, MaxSeries];
        
        int[] WPBTSeriesBeams = new int[csubs];
        float[, ,] WPBTSeries = new float[csubs, 8, MaxSeries];

        float[,,] GHdepthSeries = new float[1,8,MaxSeries];

        int[] BTSeriesBeams = new int[csubs];
        int[] WPSeriesBeams = new int[csubs];

        float[, ,] HPRTPSeries = new float[csubs, 9, MaxSeries];
        float[, ,] BeamSeriesBT = new float[csubs, 4, MaxSeries];
        float[, ,] BeamSeriesBTAmb = new float[csubs, 4, MaxSeries];
        float[, ,] BeamSeriesBTAmbCor = new float[csubs, 4, MaxSeries];
        float[, ,] BeamSeriesBTSL = new float[csubs, 4, MaxSeries];
        float[, ,] BeamSeriesBTM1 = new float[csubs, 4, MaxSeries];
        float[, ,] BeamSeriesBTM2 = new float[csubs, 4, MaxSeries];
        float[, ,] InstrumentSeriesBT = new float[csubs, 4, MaxSeries];
        float[, ,] MagSeriesBT = new float[csubs, 6, MaxSeries];
        double[] SumMagBT = new double[csubs];
        double[] SumVTG = new double[csubs];
        double[] SumN = new double[csubs];
        float[, ,] EarthSeriesBT = new float[csubs, 4, MaxSeries];
        float[, ,] AmpSeriesBT = new float[csubs, 4, MaxSeries];
        float[,,] NoiseAmpSeriesRiverBT = new float[csubs, 10, MaxSeries];
        float[, ,] CorSeriesBT = new float[csubs, 4, MaxSeries];
        float[, ,] SNRSeriesBT = new float[csubs, 4, MaxSeries];
        float[, ,] RangeSeriesBT = new float[csubs, 4, MaxSeries];
        float[,,] RangeSeriesBTPR = new float[csubs, 4, MaxSeries];
        float[,,] SNRSeriesBTPR = new float[csubs, 4, MaxSeries];
        float[] BPR_Range = new float[4];
        float[] BPR_SNR = new float[4];
        float[] BPR_beamAmp = new float[4];
        float[] BPR_beamVel = new float[4];
        float[] BPR_InstVel = new float[4];
        float[] BPR_EarthVel = new float[4];


        //RangeSeriesBTPR[cs, i, SeriesIndex[cs]] = PR_Range[i];
        float[, ,] HPRTSeriesBT = new float[csubs, 9, MaxSeries];
        float SX1 = 80;
        float SX2 = 700;
        float SY1 = 20;
        float SY2 = 380;

        int SeriesDataType = 0;
        const int TypeVel = 0;
        const int TypeAmp = 1;
        const int TypeCor = 2;

        int SeriesDataSource = 2;
        const int SourceBT = 0;
        const int SourceWT = 1;
        const int SourceProfile = 2;
        const int SourceAncillaryProfile = 3;
        const int SourceAncillaryBT = 4;
        const int SourceWPBT = 5;

        int SeriesBTDataType = 0;
        const int TypeBTvel = 0;
        const int TypeBTamp = 1;
        const int TypeBTcor = 2;
        const int TypeBTsnr = 3;
        const int TypeBTrange = 4;
        const int TypeBTmag = 5;

        void ClearSeries()
        {   
            for (int k = 0; k < csubs; k++)
            {
                SumMagBT[k] = 0;
                SumVTG[k] = 0;
                SumN[k] = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < MaxSeries; j++)
                    {
                        for (int n = 0; n < cbins; n++)
                        {
                            BeamSeries[k, i, n, j] = 0;
                            InstrumentSeries[k, i, n, j] = 0;
                            EarthSeries[k, i, n, j] = 0;
                            AmpSeries[k, i, n, j] = 0;
                            CorSeries[k, i, n, j] = 0;
                        }
                        BeamSeriesBT[k, i, j] = 0;
                        BeamSeriesBTAmb[k, i, j] = 0;
                        BeamSeriesBTAmbCor[k, i, j] = 0;
                        BeamSeriesBTSL[k, i, j] = 0;
                        BeamSeriesBTM1[k, i, j] = 0;
                        BeamSeriesBTM2[k, i, j] = 0;

                        
                        InstrumentSeriesBT[k, i, j] = 0;
                        EarthSeriesBT[k, i, j] = 0;
                        AmpSeriesBT[k, i, j] = 0;
                        CorSeriesBT[k, i, j] = 0;
                        SNRSeriesBT[k, i, j] = 0;
                        RangeSeriesBT[k, i, j] = 0;

                        NoiseAmpSeriesRiverBT[k, i, j] = 0;

                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < MaxSeries; j++)
                    {
                        MagSeriesBT[k, i, j] = 100;
                    }
                }

                        for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < MaxSeries; j++)
                    {
                        WPBTSeries[k, i, j] = 0;
                        HPRTPSeries[k,i, j] = 0;
                        HPRTSeriesBT[k,i, j] = 0;
                    }
                }

                SeriesIndex[k] = 0;
            }
            for (int j = 0; j < MaxSeries; j++)
            {
                GHdepthSeries[0,0,j] = 0;
                GHdepthSeries[0, 1, j] = 0;
            }
        }
        int movecount = 0;
        void MoveSeriesData(ArrayClass m)
        {
            if (SeriesBin < 0)
                SeriesBin = 0;            

            int cs = (int)(m.E_CurrentSystem >> 24);
            if (cs > csubs - 1)
                cs = csubs - 1;

            if (RiverBT.Available)
            {
                for (int sb = 0; sb < RiverBT.Subs; sb++)
                {
                    for (int bm = 0; bm < RiverBT.Beams[sb]; bm++)
                    {
                        NoiseAmpSeriesRiverBT[sb,bm, SeriesIndex[0]] = RiverBT.NoiseAmpBackPorch[sb, bm];
                    }
                }
                SeriesIndex[1] = SeriesIndex[0] + 1;
            }
            for (int i = 0; i < m.E_Beams; i++)
            {
                for (int n = 0; n < m.E_Cells; n++)
                {
                    BeamSeries[cs, i,n, SeriesIndex[cs]] = m.Velocity[i, n];
                    InstrumentSeries[cs, i,n, SeriesIndex[cs]] = m.Instrument[i, n];
                    EarthSeries[cs, i,n, SeriesIndex[cs]] = m.Earth[i, n];
                    AmpSeries[cs, i,n, SeriesIndex[cs]] = m.Amplitude[i, n];
                    CorSeries[cs, i,n, SeriesIndex[cs]] = m.Correlation[i, n];
                }
            }
            for (int i = 0; i < m.B_Beams; i++)
            {
                BeamSeriesBT[cs, i, SeriesIndex[cs]] = m.B_Velocity[i];
                BeamSeriesBTAmb[cs, i, SeriesIndex[cs]] = m.Eng_AmbVel[i];
                BeamSeriesBTAmbCor[cs, i, SeriesIndex[cs]] = m.Eng_AmbCor[i];
                BeamSeriesBTSL[cs, i, SeriesIndex[cs]] = m.Eng_SVel[i];
                BeamSeriesBTM1[cs, i, SeriesIndex[cs]] = m.Eng_M1Vel[i];
                BeamSeriesBTM2[cs, i, SeriesIndex[cs]] = m.Eng_M2Vel[i];

                InstrumentSeriesBT[cs, i, SeriesIndex[cs]] = m.B_Instrument[i];
                EarthSeriesBT[cs, i, SeriesIndex[cs]] = m.B_Earth[i];
                AmpSeriesBT[cs, i, SeriesIndex[cs]] = m.B_Amplitude[i];
                CorSeriesBT[cs, i, SeriesIndex[cs]] = m.B_Correlation[i];
                SNRSeriesBT[cs, i, SeriesIndex[cs]] = m.B_SNR[i];
                RangeSeriesBT[cs, i, SeriesIndex[cs]] = m.B_Range[i];
                RangeSeriesBTPR[cs, i, SeriesIndex[cs]] = BPR_Range[i];
                SNRSeriesBTPR[cs, i, SeriesIndex[cs]] = BPR_SNR[i];
            }
            if (VTGSpeedKPHFresh)
            {
                MagSeriesBT[cs, 1, SeriesIndex[cs]] = (float)(VTG[cs].SpeedKPH * 1000 / 3600);
                VTGSpeedKPHFresh = false;
                VTGspeed = MagSeriesBT[cs, 1, SeriesIndex[cs]];
                if (Application.OpenForms["View"] != null)
                {
                    float speedlimit = 0;
                    try
                    {
                        speedlimit = Convert.ToSingle(textBoxVTGspeedLimit.Text);
                    }
                    catch { }

                    if (VTGspeed > speedlimit)
                        RoweTech.View.IniVar.BadFlag = true;
                    else
                        RoweTech.View.IniVar.BadFlag = false;

                    //RoweTech.View.IniVar.StrToTheTop = "Top";
                    RoweTech.View.IniVar.UpdateStr = VTGspeed.ToString("0.00") + " m/s\r\n";
                }
                movecount = 0;
            }
            else
            {
                MagSeriesBT[cs, 1, SeriesIndex[cs]] = (float)0;
                movecount++;
                if(movecount > 10)
                {
                    if (Application.OpenForms["View"] != null)
                    {
                        RoweTech.View.IniVar.UpdateStr = "GPS VTG NA\r\n";
                    }
                }
            }
            

            if (m.BottomTrackAvailable)
            {
                if (Math.Abs(m.B_Instrument[0]) < 88 && Math.Abs(m.B_Instrument[1]) < 88 && Math.Abs(m.B_Instrument[2]) < 88)
                {
                    MagSeriesBT[cs, 0, SeriesIndex[cs]] = (float)Math.Sqrt((double)(m.B_Instrument[0] * (double)m.B_Instrument[0]) + (double)m.B_Instrument[1] * (double)m.B_Instrument[1] + UseZ * (double)m.B_Instrument[2] * (double)m.B_Instrument[2]);
                    if (MagSeriesBT[cs, 1, SeriesIndex[cs]] > 0)
                    {
                        SumN[cs] += 1.0;
                        SumMagBT[cs] += MagSeriesBT[cs, 0, SeriesIndex[cs]];
                        SumVTG[cs] += MagSeriesBT[cs, 1, SeriesIndex[cs]];
                        float temp = (float)(100.0 * (SumMagBT[cs] / SumVTG[cs] - 1.0));
                        MagSeriesBT[cs, 4, SeriesIndex[cs]] = temp;
                    }
                    else
                        MagSeriesBT[cs, 4, SeriesIndex[cs]] = (float)88.88;
                }
                else
                {
                    MagSeriesBT[cs, 0, SeriesIndex[cs]] = (float)88.88;
                    MagSeriesBT[cs, 4, SeriesIndex[cs]] = (float)88.88;
                }
            }
            if (FindBTinProfile && Arr.AmplitudeAvailable && Arr.BottomTrackAvailable && Math.Abs(BPR_InstVel[0]) < 88 && Math.Abs(BPR_InstVel[1]) < 88 && Math.Abs(BPR_InstVel[2]) < 88)
            {
                MagSeriesBT[cs, 2, SeriesIndex[cs]] = (float)Math.Sqrt((double)(BPR_InstVel[0] * (double)BPR_InstVel[0]) + (double)BPR_InstVel[1] * (double)BPR_InstVel[1] + UseZ * (double)BPR_InstVel[2] * (double)BPR_InstVel[2]);
            }
            else
                MagSeriesBT[cs, 2, SeriesIndex[cs]] = (float)88.88;

            if (m.EnsembleDataAvailable)
            {
                if (Math.Abs(m.Instrument[0, SeriesBin]) < 88 && Math.Abs(m.Instrument[1, SeriesBin]) < 88 && Math.Abs(m.Instrument[2, SeriesBin]) < 88)
                {
                    MagSeriesBT[cs, 3, SeriesIndex[cs]] = (float)Math.Sqrt((double)(m.Instrument[0, SeriesBin] * (double)m.Instrument[0, SeriesBin]) + (double)m.Instrument[1, SeriesBin] * (double)m.Instrument[1, SeriesBin] + UseZ * (double)m.Instrument[2, SeriesBin] * (double)m.Instrument[2, SeriesBin]);
                }
                else
                    MagSeriesBT[cs, 3, SeriesIndex[cs]] = (float)88.88;
            }
            if (m.AncillaryAvailable)
            {
                if (m.A_Heading > 179.9999)
                    HPRTPSeries[cs, 0, SeriesIndex[cs]] = m.A_Heading - 360;
                else
                    HPRTPSeries[cs, 0, SeriesIndex[cs]] = m.A_Heading;

                if (HDT[cs].Heading > 179.9999)
                    HPRTPSeries[cs, 8, SeriesIndex[cs]] = (float)HDT[cs].Heading - 360;
                else
                    HPRTPSeries[cs, 8, SeriesIndex[cs]] = (float)HDT[cs].Heading;

                HPRTPSeries[cs, 1, SeriesIndex[cs]] = 10 * m.A_Pitch;

                if (m.A_Roll > 90)
                    HPRTPSeries[cs, 2, SeriesIndex[cs]] = m.A_Roll - 180;
                else
                    if (m.A_Roll < -90)
                    HPRTPSeries[cs, 2, SeriesIndex[cs]] = m.A_Roll + 180;
                else
                    HPRTPSeries[cs, 2, SeriesIndex[cs]] = m.A_Roll;

                HPRTPSeries[cs, 2, SeriesIndex[cs]] *= 10;

                HPRTPSeries[cs, 3, SeriesIndex[cs]] = m.A_WaterTemperature;
                HPRTPSeries[cs, 4, SeriesIndex[cs]] = m.A_BoardTemperature;
                HPRTPSeries[cs, 5, SeriesIndex[cs]] = m.A_Pressure;
                HPRTPSeries[cs, 6, SeriesIndex[cs]] = m.A_Depth;
                HPRTPSeries[cs, 7, SeriesIndex[cs]] = 0;
            }
            if (m.RTonWPAvailable)
            {
                for (int i = 0; i < m.E_Beams; i++)
                    WPBTSeries[cs, i, SeriesIndex[cs]] = m.RTonWP_Range[i];
            }
            //-----------
            if (m.BottomTrackAvailable)
            {
                if (m.B_Heading > 179.9999)
                    HPRTSeriesBT[cs, 0, SeriesIndex[cs]] = m.B_Heading - 360;
                else
                    HPRTSeriesBT[cs, 0, SeriesIndex[cs]] = m.B_Heading;

                HPRTSeriesBT[cs, 1, SeriesIndex[cs]] = 10 * m.B_Pitch;

                if (m.B_Roll > 90)
                    HPRTSeriesBT[cs, 2, SeriesIndex[cs]] = m.B_Roll - 180;
                else
                    if (m.B_Roll < -90)
                    HPRTSeriesBT[cs, 2, SeriesIndex[cs]] = m.B_Roll + 180;
                else
                    HPRTSeriesBT[cs, 2, SeriesIndex[cs]] = m.B_Roll;

                HPRTSeriesBT[cs, 2, SeriesIndex[cs]] *= 10;

                HPRTSeriesBT[cs, 3, SeriesIndex[cs]] = m.B_WaterTemperature;
                HPRTSeriesBT[cs, 4, SeriesIndex[cs]] = m.B_BoardTemperature;
                HPRTSeriesBT[cs, 5, SeriesIndex[cs]] = m.B_Pressure;
                HPRTSeriesBT[cs, 6, SeriesIndex[cs]] = m.B_Depth;
                HPRTSeriesBT[cs, 7, SeriesIndex[cs]] = 0;
            }
            if (m.GageAvailable)
            {
                GHdepthSeries[0, 0, SeriesIndex[cs]] = m.RGH_AvgRange;
                GHdepthSeries[0, 1, SeriesIndex[cs]] = m.RGH_Depth;
            }
            

            SeriesIndex[cs]++;
            if (SeriesIndex[cs] >= MaxSeries)
                SeriesIndex[cs] = 0;

        }
        void Graphics_PlotSeriesFloatData(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                             float[,,] Data, int M,int N, float Vscale, float Voffset, Pen FPen, string LegendStr, int LegendN, bool LegendLeft, float Max, float Min)
        {
            if ((X2 > X1) && (MaxSeries > 0))
            {
                SolidBrush Brush = new SolidBrush(FPen.Color);
                float X, Y;
                int j = SeriesIndex[N];

                float dXstep = ((X2 - X1) / MaxSeries);
                float dX = X1;
                float LastdX = dX;

                bool gotfirstsample = false;
                float LastY = 0;

                for (int i = 0; i < MaxSeries; i++)
                {
                    if (Data[N, M, j] < Max && Data[N, M, j] > Min)
                    {
                        Y = Voffset + (Vscale * (float)Data[N, M, j]);
                        if (Y > Y2)
                            Y = Y2;
                        if (Y < Y1)
                            Y = Y1;
                        if (gotfirstsample == false)
                        {
                            //LastY = Y;
                            gotfirstsample = true;
                        }
                        else
                        {
                            graphics.DrawLine(FPen, LastdX, LastY, dX, Y);
                        }
                        LastY = Y;

                        LastdX = dX;
                    }
                    dX += dXstep;
                    j++;
                    if (j >= MaxSeries)
                        j = 0;
                }
                if (LegendStr != "")
                {
                    if (LegendLeft)
                        X = 0;
                    else
                        X = X2 + 50;// 70;
                    Y = Y1 + 15 * LegendN;
                    graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
                }
            }
        }

        void Graphics_PlotSeriesFloatBinData(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                             float[,,,] Data, int M, int N, int P, float Vscale, float Voffset, Pen FPen, string LegendStr, int LegendN, bool LegendLeft, float Max, float Min)
        {
            if ((X2 > X1) && (MaxSeries > 0))
            {
                SolidBrush Brush = new SolidBrush(FPen.Color);
                float X, Y;
                int j = SeriesIndex[N];

                float dXstep = ((X2 - X1) / MaxSeries);
                float dX = X1;
                float LastdX = dX;

                bool gotfirstsample = false;
                float LastY = 0;

                for (int i = 0; i < MaxSeries; i++)
                {
                    if (Data[N, M, P, j] < Max && Data[N, M, P, j] > Min)
                    {
                        Y = Voffset + (Vscale * (float)Data[N, M, P, j]);
                        if (Y > Y2)
                            Y = Y2;
                        if (Y < Y1)
                            Y = Y1;
                        if (gotfirstsample == false)
                        {
                            //LastY = Y;
                            gotfirstsample = true;
                        }
                        else
                        {
                            graphics.DrawLine(FPen, LastdX, LastY, dX, Y);
                        }
                        LastY = Y;

                        LastdX = dX;
                    }
                    dX += dXstep;
                    j++;
                    if (j >= MaxSeries)
                        j = 0;
                }
                if (LegendStr != "")
                {
                    if (LegendLeft)
                        X = 0;
                    else
                        X = X2 + 70;
                    Y = Y1 + 15 * LegendN;
                    graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
                }
            }
        }

        //void PlotSeries(Graphics graphics, ArrayClass m, ArrayClass2 m2)
        void PlotSeries(Graphics graphics, ArrayClass m)
        {
            Pen FPen = new Pen(Color.Blue, 1);

            string s;// = "";
            int i;

            float vval, vvalstep;

            float X1 = 0;// pictureBoxSeries.Left;// 0;
            float X2 = pictureBoxSeries.Width;// - pictureBoxSeries.Left;
            float Y1 = 0;// pictureBoxSeries.Top;// 0;
            float Y2 = pictureBoxSeries.Height;// - pictureBoxSeries.Top;
            Graphics_InitPlot(graphics, X1, X2, Y1, Y2, "Series Graph");

            SX1 = X1 + 100;
            SX2 = X2 - 100;
            SY1 = Y1 + 20;
            SY2 = Y2 - 0;

            //int cs = (int)(m.E_CurrentSystem >> 24);
            int cs = ProfileSubSystem;
            if (cs < 0)
                cs = 0;
            if (cs > csubs - 1)
                cs = csubs - 1;

            float sy1;
            float sy2;

            if (SeriesDataSource == SourceProfile && m.VelocityAvailable)
            {
                for (i = 0; i < WPSeriesBeams[cs]; i++)
                {
                    switch (i)
                    {
                        case 0:
                            FPen.Color = Color.Black;
                            //FPen.Color = Color.FromArgb(50, 0, 0, 0);
                            break;
                        case 1:
                            FPen.Color = Color.Brown;
                            //FPen.Color = Color.FromArgb(50, 165, 42, 42);
                            break;
                        case 2:
                            FPen.Color = Color.Red;
                            //FPen.Color = Color.FromArgb(50, 255, 0, 0);
                            break;
                        case 3:
                            FPen.Color = Color.Orange;
                            //FPen.Color = Color.FromArgb(50, 255, 165, 0);
                            break;
                    }
                    switch (SeriesDataType)
                    {
                        case TypeVel:
                            s = " ";
                            sy1 = SY1;
                            sy2 = SY2 - 20;
                            Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                          8, VSpanSeries / 2, (float)-VSpanSeries / 8, " ", VSpanSeries / 2, (float)-VSpanSeries / 8, " ", 5,
                                          9, 0, 50 / 2, MaxSeries, s, false);
                            switch (SeriesCoordinateState)
                            {
                                case SeriesBeam:
                                    s = "B" + i.ToString();
                                    Graphics_PlotSeriesFloatBinData(graphics, SX1, SX2, sy1, sy2, BeamSeries, i, cs,SeriesBin, -(float)(sy2 - sy1) / VSpanSeries, sy2 - (sy2 - sy1) / 2, FPen, s, i, true, 80, -80);
                                    break;
                                case SeriesXYZ:
                                    switch (i)
                                    {
                                        case 0:
                                            s = "X";
                                            break;
                                        case 1:
                                            s = "Y";
                                            break;
                                        case 2:
                                            s = "Z";
                                            break;
                                        case 3:
                                            s = "Q";
                                            break;
                                    }
                                    Graphics_PlotSeriesFloatBinData(graphics, SX1, SX2, sy1, sy2, InstrumentSeries, i, cs, SeriesBin, (float)(sy2 - sy1) / VSpanSeries, sy2 - (sy2 - sy1) / 2, FPen, s, i, true, 80, -80);
                                    break;
                                case SeriesENU:
                                    switch (i)
                                    {
                                        case 0:
                                            s = "E";
                                            break;
                                        case 1:
                                            s = "N";
                                            break;
                                        case 2:
                                            s = "U";
                                            break;
                                        case 3:
                                            s = "Q";
                                            break;
                                    }
                                    Graphics_PlotSeriesFloatBinData(graphics, SX1, SX2, sy1, sy2, EarthSeries, i, cs, SeriesBin, (float)(sy2 - sy1) / VSpanSeries, sy2 - (sy2 - sy1) / 2, FPen, s, i, true, 80, -80);
                                    break;
                            }
                            break;
                        case TypeAmp:
                            s = " ";
                            //if (ProfileAmpScaledB)
                            {
                                vval = 200;
                                vvalstep = -50;
                            }
                            //else
                            //{
                            //    vval = 2000;
                            //    vvalstep = -500;
                            //}
                            sy1 = SY1;
                            sy2 = SY2 - 20;
                            Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                          4, vval, vvalstep, " ", vval, vvalstep, " ", 5,
                                          9, 0, 50 / 2, MaxSeries, s, false);

                            s = "A" + i.ToString();
                            Graphics_PlotSeriesFloatBinData(graphics, SX1, SX2, sy1, sy2, AmpSeries, i, cs, SeriesBin, (float)-(sy2 - sy1) / vval, sy2, FPen, s, i, true, vval, 0);
                            break;
                        case TypeCor:
                            s = " ";
                            
                            sy1 = SY1;
                            sy2 = SY2 - 20;
                            Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                            4, 1, (float)-0.25, " ", 1, (float)-0.25, " ", 5,
                                            9, 0, 50 / 2, MaxSeries, s, false);
                            s = "C" + i.ToString();
                            Graphics_PlotSeriesFloatBinData(graphics, SX1, SX2, sy1, sy2, CorSeries, i, cs, SeriesBin, (float)-(sy2 - sy1), sy2, FPen, s, i, true, 80, -80);
                            

                            break;
                    }
                }
            }
            if (SeriesDataSource == SourceWPBT)
            {
                sy1 = SY1;
                sy2 = SY2 - 20;
                s = " ";
                
                Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                              10, 0, (float)RSpanBT / 10, " ", 0, (float)RSpanBT / 10, " ", 5,
                              9, 0, 50 / 2, MaxSeries, s, false);
                
                FPen.Width = 1;

                //for (i = 0; i < (int)m.RTonWP_Beams; i++)
                for (i = 0; i < WPBTSeriesBeams[cs]; i++)
                {
                    switch (i)
                    {
                        case 0:
                            FPen.Color = Color.Black;
                            break;
                        case 1:
                            FPen.Color = Color.Brown;
                            break;
                        case 2:
                            FPen.Color = Color.Red;
                            break;
                        case 3:
                            FPen.Color = Color.Orange;
                            break;
                    }                    
                    s = "WPBTD" + i.ToString();
                    Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, WPBTSeries, i, cs, (float)(sy2 - sy1) / RSpanBT, sy1, FPen, s, i, true, 4096, (float)0.01);
                }
                FPen.Color = Color.Magenta;
                s = "PresD";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTPSeries, 6, cs, (float)(sy2 - sy1) / RSpanBT, sy1, FPen, s, 5, true, 4096, (float)0.01);
            }
            if (SeriesDataSource == SourceBT)
            {
                /*
                m.GageAvailable = true;
                m.RGH_AvgRange = ByteArrayToFloat(packet);//1
                m.RGH_sd = ByteArrayToFloat(packet);//2
                m.RGH_AvgSN = ByteArrayToFloat(packet);//3
                m.RGH_n = ByteArrayToFloat(packet);//4
                m.RGH_Salinity = ByteArrayToFloat(packet);//5
                m.RGH_Pressure = ByteArrayToFloat(packet);//6
                m.RGH_Depth = ByteArrayToFloat(packet);//7
                m.RGH_WaterTemperature = ByteArrayToFloat(packet);//8
                m.RGH_BackPlaneTemperature = ByteArrayToFloat(packet);//9
                m.RGH_SOS = ByteArrayToFloat(packet);//10
                m.RGH_Heading = ByteArrayToFloat(packet);//11
                m.RGH_Pitch = ByteArrayToFloat(packet);//12
                m.RGH_Roll = ByteArrayToFloat(packet);//13
                 */
                if (m.GageAvailable)
                {
                    sy1 = SY1;
                    sy2 = SY2 - 20;
                    s = " ";

                    Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                  10, 0, (float)RSpanBT / 10, " ", 0, (float)RSpanBT / 10, " ", 5,
                                  9, 0, 50 / 2, MaxSeries, s, false);

                    FPen.Width = 1;
                    FPen.Color = Color.Black;
                    
                    s = "GageD";
                    Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, GHdepthSeries, 0, cs, (float)(sy2 - sy1) / RSpanBT, sy1, FPen, s, 0, true, 4096, (float)0.01);
                    
                    FPen.Color = Color.Magenta;
                    s = "PresD";
                    Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, GHdepthSeries, 1, cs, (float)(sy2 - sy1) / RSpanBT, sy1, FPen, s, 1, true, 4096, (float)0.01);
                }
                else
                {
                // Bottom Track
                    switch (SeriesBTDataType)
                    {
                        case TypeBTmag:
                            s = " ";
                            sy1 = SY1;
                            sy2 = SY2 - 20;
                            /*Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                        4, 180, (float)-90, " ", 180, -90, " ", 5,
                                        9, 0, 50 / 2, MaxSeries, s, false);*/

                            Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                                                      8, VSpanBT, (float)-VSpanBT / 8, " ", 8, (float)-2, " ", 5,
                                                                      9, 0, 50 / 2, MaxSeries, s, false);
                            FPen.Width = 1;
                            FPen.Color = Color.Blue;
                            s = "BTM";
                            Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, MagSeriesBT, 0, cs, (float)-(sy2 - sy1) / VSpanBT, sy2, FPen, s, 0, true, 80, 0);
                            FPen.Color = Color.Red;
                            s = "VTG";
                            Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, MagSeriesBT, 1, cs, (float)-(sy2 - sy1) / VSpanBT, sy2, FPen, s, 1, true, 80, 0);
                            FPen.Color = Color.LightBlue;
                            s = "mag";
                            Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, MagSeriesBT, 2, cs, (float)-(sy2 - sy1) / VSpanBT, sy2, FPen, s, 2, true, 80, 0);
                            FPen.Color = Color.Green;
                            s = "WTM";
                            Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, MagSeriesBT, 3, cs, (float)-(sy2 - sy1) / VSpanBT, sy2, FPen, s, 3, true, 80, 0);

                            FPen.Color = Color.Black;
                            s = "%erVTG";
                            Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, MagSeriesBT, 4, cs, (float)-(sy2 - sy1) / 16, sy1 + (sy2 - sy1) / 2, FPen, s, 0, false, 8, -8);

                            FPen.Color = Color.Brown;
                            s = "%erGGA";
                            Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, MagSeriesBT, 5, cs, (float)-(sy2 - sy1) / 16, sy1 + (sy2 - sy1) / 2, FPen, s, 1, false, 8, -8);

                            break;
                        case TypeBTvel:
                            s = " ";
                            sy1 = SY1;
                            sy2 = SY2 - 20;
                            Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                          8, VSpanBT / 2, (float)-VSpanBT / 8, " ", 40, -10, "", 5,
                                          9, 0, 50 / 2, MaxSeries, s, false);

                            FPen.Width = 1;

                            for (i = 0; i < BTSeriesBeams[cs]; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        FPen.Color = Color.Black;
                                        break;
                                    case 1:
                                        FPen.Color = Color.Brown;
                                        break;
                                    case 2:
                                        FPen.Color = Color.Red;
                                        break;
                                    case 3:
                                        FPen.Color = Color.Orange;
                                        break;
                                }

                                switch (SeriesCoordinateState)
                                {
                                    case SeriesBeam:
                                        //if (i == 1)
                                        {
                                            s = "B" + i.ToString();

                                            if (m.EngBottomTrackDataAvailable)
                                            {
                                                FPen.Width = 1;
                                                //FPen.Color = Color.Blue;
                                                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, BeamSeriesBTAmb, i, cs, (float)-(sy2 - sy1) / VSpanBT, sy2 - (sy2 - sy1) / 2, FPen, s, i, true, 80, -80);
                                                /*
                                                FPen.Width = 2;
                                                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, SY1, SY2, BeamSeriesBTM1, i, (float)-(SY2 - SY1) / VSpanBT, SY2 - (SY2 - SY1) / 2, FPen, s, i, true, 80, -80);
                                                */
                                                FPen.Width = 1;
                                                //FPen.Color = Color.Green;
                                                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, BeamSeriesBTSL, i, cs, (float)-(sy2 - sy1) / VSpanBT, sy2 - (sy2 - sy1) / 2, FPen, s, i, true, 80, -800);

                                            }
                                            FPen.Width = 1;
                                            //FPen.Color = Color.Black;
                                            Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, BeamSeriesBT, i, cs, (float)-(sy2 - sy1) / VSpanBT, sy2 - (sy2 - sy1) / 2, FPen, s, i, true, 80, -80);
                                        }
                                        break;
                                    case SeriesXYZ:
                                        switch (i)
                                        {
                                            case 0:
                                                s = "X";
                                                break;
                                            case 1:
                                                s = "Y";
                                                break;
                                            case 2:
                                                s = "Z";
                                                break;
                                            case 3:
                                                s = "Q";
                                                break;
                                        }
                                        Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, InstrumentSeriesBT, i, cs, (float)-(sy2 - sy1) / VSpanBT, sy2 - (sy2 - sy1) / 2, FPen, s, i, true, 80, -80);
                                        break;
                                    case SeriesENU:
                                        switch (i)
                                        {
                                            case 0:
                                                s = "E";
                                                break;
                                            case 1:
                                                s = "N";
                                                break;
                                            case 2:
                                                s = "U";
                                                break;
                                            case 3:
                                                s = "Q";
                                                break;
                                        }
                                        Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, EarthSeriesBT, i, cs, (float)-(sy2 - sy1) / VSpanBT, sy2 - (sy2 - sy1) / 2, FPen, s, i, true, 80, -80);
                                        break;
                                }
                            }
                            break;
                        case TypeBTamp:
                            s = " ";

                            //if (ProfileAmpScaledB)
                            {
                                vval = 200;
                                vvalstep = -50;
                            }
                            //else
                            //{
                            //    vval = 2000;
                            //    vvalstep = -500;
                            //}

                            

                            sy1 = SY1;
                            sy2 = SY2 - 20;
                            Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                          4, vval, (float)vvalstep, " ", vval, vvalstep, " ", 5,
                                          9, 0, 50 / 2, MaxSeries, s, false);

                            FPen.Width = 1;

                            
                            for (i = 0; i < BTSeriesBeams[cs]; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        FPen.Color = Color.Black;
                                        break;
                                    case 1:
                                        FPen.Color = Color.Brown;
                                        break;
                                    case 2:
                                        FPen.Color = Color.Red;
                                        break;
                                    case 3:
                                        FPen.Color = Color.Orange;
                                        break;
                                }

                                //s = "A" + i.ToString();
                                s = "A" + cs.ToString() + "," + i.ToString();
                                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, AmpSeriesBT, i, cs, -(float)(sy2 - sy1) / vval, sy2, FPen, s, i, true, vval, 0);

                            }
                            if (RiverBT.Available)
                            {
                                int sbs = (int)RiverBT.Subs;
                                if (sbs > BTMAXSUBS)
                                    sbs = BTMAXSUBS;
                                //int sub = 1;
                                for (int sb = 0; sb < sbs; sb++)
                                {
                                    if (RiverBT.PingCount[sb] > 0)
                                    {
                                        //NoiseAmpSeriesRiverBT[sb, bm, SeriesIndex[0]]
                                        int bms = (int)RiverBT.Beams[sb];
                                        if (bms > BTMAXBEAMS)
                                            bms = BTMAXBEAMS;

                                        for (int bm = 0; bm < bms; bm++)
                                        {
                                            if (sb == 0)
                                            {
                                                switch (bm)
                                                {
                                                    case 0:
                                                        FPen.Color = Color.Gray;
                                                        break;
                                                    case 1:
                                                        FPen.Color = Color.SandyBrown;
                                                        break;
                                                    case 2:
                                                        FPen.Color = Color.Pink;
                                                        break;
                                                    case 3:
                                                        FPen.Color = Color.OrangeRed;
                                                        break;
                                                }

                                                
                                            }
                                            else
                                            {
                                                switch (bm)
                                                {
                                                    case 0:
                                                        FPen.Color = Color.Green;
                                                        break;
                                                    case 1:
                                                        FPen.Color = Color.SandyBrown;
                                                        break;
                                                    case 2:
                                                        FPen.Color = Color.Pink;
                                                        break;
                                                    case 3:
                                                        FPen.Color = Color.OrangeRed;
                                                        break;
                                                }
                                            }
                                            s = "N" + sb.ToString() + "," + bm.ToString();
                                            Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, NoiseAmpSeriesRiverBT
                                                                         , bm, sb, -(float)(sy2 - sy1) / vval, sy2, FPen
                                                                         , s, 4*sb+bm+4, true, vval, 0);
                                        }
                                        
                                    }
                                }
                            }
                            break;
                        case TypeBTcor:
                            s = " ";
                            sy1 = SY1;
                            sy2 = SY2 - 20;
                            Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                          4, 1, (float)-0.25, " ", 1, (float)-0.25, " ", 5,
                                          9, 0, 50 / 2, MaxSeries, s, false);



                            for (i = 0; i < BTSeriesBeams[cs]; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        FPen.Color = Color.Black;
                                        break;
                                    case 1:
                                        FPen.Color = Color.Brown;
                                        break;
                                    case 2:
                                        FPen.Color = Color.Red;
                                        break;
                                    case 3:
                                        FPen.Color = Color.Orange;
                                        break;
                                }
                                FPen.Width = 1;
                                s = "C" + i.ToString();
                                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, CorSeriesBT, i, cs, -(float)(sy2 - sy1), sy2, FPen, s, i, true, (float)1.0001, (float)0.01);

                                if (m.EngBottomTrackDataAvailable)
                                {
                                    FPen.Width = 1;
                                    Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, BeamSeriesBTAmbCor, i, cs, -(float)(sy2 - sy1), sy2, FPen, s, i, true, (float)1.0001, (float)0.01);
                                }
                            }
                            break;
                        case TypeBTsnr:
                            s = " ";

                            //if (ProfileAmpScaledB)
                            {
                                vval = 200;
                                vvalstep = -50;
                            }
                            //else
                            //{
                            //    vval = 2000;
                            //    vvalstep = -500;
                            //}
                            sy1 = SY1;
                            sy2 = SY2 - 20;
                            Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                          4, vval, (float)vvalstep, " ", vval, vvalstep, " ", 5,
                                          9, 0, 50 / 2, MaxSeries, s, false);

                            

                            

                            for (i = 0; i < BTSeriesBeams[cs]; i++)
                            {

                                FPen.Width = 1;
                                switch (i)
                                {
                                    case 0:
                                        FPen.Color = Color.Gray;
                                        break;
                                    case 1:
                                        FPen.Color = Color.LightGreen;
                                        break;
                                    case 2:
                                        FPen.Color = Color.LightPink;
                                        break;
                                    case 3:
                                        FPen.Color = Color.Aqua;
                                        break;
                                }

                                s = "s" + i.ToString();
                                if (FindBTinProfile)
                                    Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, SNRSeriesBTPR, i, cs, -(float)(sy2 - sy1) / vval, sy2, FPen, s, i+4, true, vval, 0);
                                

                                switch (i)
                                {
                                    case 0:
                                        FPen.Color = Color.Black;
                                        break;
                                    case 1:
                                        FPen.Color = Color.Brown;
                                        break;
                                    case 2:
                                        FPen.Color = Color.Red;
                                        break;
                                    case 3:
                                        FPen.Color = Color.Orange;
                                        break;
                                }

                                s = "S" + i.ToString();
                                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, SNRSeriesBT, i, cs, -(float)(sy2 - sy1) / vval, sy2, FPen, s, i, true, vval, 0);
                            }
                            break;
                        case TypeBTrange:
                            s = " ";
                            sy1 = SY1;
                            sy2 = SY2 - 20;
                            Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                          10, 0, (float)RSpanBT / 10, " ", 10, (float)-2, " ", 5,
                                          9, 0, 50 / 2, MaxSeries, s, false);

                            FPen.Width = 1;

                            for (i = 0; i < BTSeriesBeams[cs]; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        FPen.Color = Color.Gray;
                                        break;
                                    case 1:
                                        FPen.Color = Color.LightGreen;
                                        break;
                                    case 2:
                                        FPen.Color = Color.LightPink;
                                        break;
                                    case 3:
                                        FPen.Color = Color.Aqua;
                                        break;
                                }

                                s = "r" + i.ToString();
                                FPen.Width = 1;
                                if (FindBTinProfile)
                                    Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, RangeSeriesBTPR, i, cs, (float)(sy2 - sy1) / RSpanBT, sy1, FPen, s, i+4, true, 4096, (float)0.01);

                                switch (i)
                                {
                                    case 0:
                                        FPen.Color = Color.Black;
                                        break;
                                    case 1:
                                        FPen.Color = Color.Brown;
                                        break;
                                    case 2:
                                        FPen.Color = Color.Red;
                                        break;
                                    case 3:
                                        FPen.Color = Color.Orange;
                                        break;
                                }
                                s = "R" + i.ToString();
                                FPen.Width = 1;
                                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, RangeSeriesBT, i, cs, (float)(sy2 - sy1) / RSpanBT, sy1, FPen, s, i, true, 4096, (float)0.01);                                
                            }
                            FPen.Color = Color.Blue;
                            s = "%er";
                            Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, MagSeriesBT, 4, cs, (float)-(sy2 - sy1) / 20, sy1 + (sy2 - sy1) / 2, FPen, s, 0, false, 8, -8);
                            break;
                    }
                }
            }
            if (SeriesDataSource == SourceAncillaryBT)
            {
                sy1 = SY1;
                sy2 = SY1 + (SY2 - SY1) / 2 - 20;

                s = "";

                Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                      4, 180, (float)-90, " ", 180, -90, " ", 5,
                                      9, 0, 50 / 2, MaxSeries, s, false);

                FPen.Width = 1;

                FPen.Color = Color.Black;
                s = "H";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTSeriesBT, 0, cs, (float)-(sy2 - sy1) / 360, sy1 + (sy2 - sy1) / 2, FPen, s, 0, true, 360, -360);

                FPen.Color = Color.Brown;
                s = "Px10";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTSeriesBT, 1, cs, (float)-(sy2 - sy1) / 360, sy1 + (sy2 - sy1) / 2, FPen, s, 1, true, 360, -360);

                FPen.Color = Color.Red;
                s = "Rx10";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTSeriesBT, 2, cs, (float)-(sy2 - sy1) / 360, sy1 + (sy2 - sy1) / 2, FPen, s, 2, true, 360, -360);
                FPen.Color = Color.Orange;

                FPen.Color = Color.Blue;
                s = "°HDT";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTPSeries, 8, cs, (float)-(sy2 - sy1) / 360, sy1 + (sy2 - sy1) / 2, FPen, s, 8, true, 360, -360);


                //HDT[cs].Heading.ToString("000.00");

                //-----------------
                sy1 = sy2 + 20;
                sy2 = sy1 + (SY2 - SY1) / 2 - 18;
                s = " ";

                Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                      8, 70, (float)-10, " ", 70, -10, " ", 5,
                                      9, 0, 50 / 2, MaxSeries, s, false);

                FPen.Width = 1;

                FPen.Color = Color.Black;
                s = "°T w";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTSeriesBT, 3, cs, (float)-(sy2 - sy1) / 80, sy2 - 10 * (sy2 - sy1) / 80, FPen, s, 0, true, 100, -100);
                FPen.Color = Color.Brown;
                s = "°T b";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTSeriesBT, 4, cs, (float)-(sy2 - sy1) / 80, sy2 - 10 * (sy2 - sy1) / 80, FPen, s, 1, true, 100, -100);

            }
            if (SeriesDataSource == SourceAncillaryProfile)
            {
                sy1 = SY1;
                sy2 = SY1 + (SY2 - SY1) / 2 - 20;
                s = "";

                Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                      4, 180, (float)-90, " ", 180, -90, " ", 5,
                                      9, 0, 50 / 2, MaxSeries, s, false);

                FPen.Width = 1;

                FPen.Color = Color.Black;
                s = "°H";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTPSeries, 0, cs, (float)-(sy2 - sy1) / 360, sy1 + (sy2 - sy1) / 2, FPen, s, 0, true, 360, -360);

                FPen.Color = Color.Brown;
                s = "°Px10";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTPSeries, 1, cs, (float)-(sy2 - sy1) / 360, sy1 + (sy2 - sy1) / 2, FPen, s, 1, true, 360, -360);

                FPen.Color = Color.Red;
                s = "°Rx10";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTPSeries, 2, cs, (float)-(sy2 - sy1) / 360, sy1 + (sy2 - sy1) / 2, FPen, s, 2, true, 360, -360);
                FPen.Color = Color.Orange;

                FPen.Color = Color.Blue;
                s = "°HDT";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTPSeries, 8, cs, (float)-(sy2 - sy1) / 360, sy1 + (sy2 - sy1) / 2, FPen, s, 8, true, 360, -360);

                //-----------------
                sy1 = sy2 + 20;
                sy2 = sy1 + (SY2 - SY1) / 2 - 18;
                s = " ";

                /*Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                      6, 50, (float)-10, " ", 50, -10, " ",
                                      9, 0, 50 / 2, MaxSeries, s, false);*/
                Graphics_PlotGridSeries(graphics, SX1, SX2, sy1, sy2,
                                      8, 70, (float)-10, " ", PSpanSeries, (float)-PSpanSeries / 8, " ", 8,
                                      9, 0, 50 / 2, MaxSeries, s, false);
                FPen.Width = 1;

                FPen.Color = Color.Black;
                s = "°C w";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTPSeries, 3, cs, (float)-(sy2 - sy1) / 80, sy2 - 10 * (sy2 - sy1) / 80, FPen, s, 0, true, 100, -100);
                FPen.Color = Color.Brown;
                s = "°C b";//char degree = '°'
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTPSeries, 4, cs, (float)-(sy2 - sy1) / 80, sy2 - 10 * (sy2 - sy1) / 80, FPen, s, 1, true, 100, -100);
                FPen.Color = Color.Red;
                s = "Pa";
                Graphics_PlotSeriesFloatData(graphics, SX1, SX2, sy1, sy2, HPRTPSeries, 5, cs, (float)-(sy2 - sy1) / PSpanSeries, sy2, FPen, s, 0, false, 100001, -1);
                //1 Pascal = 0.00001 bar
                //100000 Pa = 1 bar
            }
        }
        void ShowSeries(ArrayClass m, ArrayClass2 m2)
        {
            Graphics g = Graphics.FromImage(pictureBoxSeries.Image);
            if (GotData)
            {
                //int cs = ProfileSubSystem;
                textBoxSeriesSub.Text = ProfileSubSystem.ToString();

                int acs = (int)(m.E_CurrentSystem >> 24);
                if (acs > csubs - 1)
                    acs = csubs - 1;
                if (m.EnsembleDataAvailable)
                {
                    WPBTSeriesBeams[acs] = (int)m.E_Beams;
                    WPSeriesBeams[acs] = (int)m.E_Beams;
                }
                else
                {
                    WPBTSeriesBeams[acs] = 0;
                    WPSeriesBeams[acs] = 0;
                }
                //helpme

                if (m.BottomTrackAvailable)
                    BTSeriesBeams[acs] = (int)m.B_Beams;
                else
                    BTSeriesBeams[acs] = 0;

                if (SeriesBin > m2.bins[acs] - 1)
                    SeriesBin = (int)m2.bins[acs] - 1;

                if (SeriesBin < 0)
                    SeriesBin = 0;

                //PlotSeries(g, m,m2);
                PlotSeries(g, m);
                pictureBoxSeries.Refresh();
            }
        }

        //Profile TAB----------------------------------------------------------
        private void buttonCoordinate_Click(object sender, EventArgs e)
        {
            ProfileCoordinateState++;
            if (ProfileCoordinateState > ProfBeam)
                ProfileCoordinateState = ProfENU;
            ShowEnsemble(Arr, Arr2);
        }

        /*private void textBoxProfileSubStats_TextChanged(object sender, EventArgs e)
        {
            buttonClearAverage_Click(sender,e);
        }*/
        private void buttonClearAverage_Click(object sender, EventArgs e)
        {
            BTnavClr();

            LastEnsemble = 0;

            Array.Clear(SumMagBT, 0, SumMagBT.Length);
            Array.Clear(SumVTG, 0, SumVTG.Length);
            Array.Clear(SumN, 0, SumN.Length);

            for (int k = 0; k < csubs; k++)
            {
                SumMagBT[k] = 0;
                SumVTG[k] = 0;
                SumN[k] = 0;

                subbeams[k] = 0;
                Arr2.bins[k] = 0;
                Arr2.firstbin[k] = 0;
                Arr2.binsize[k] = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 200; j++)
                    {
                        Arr2.VelSum[k, i, j] = 0;
                        Arr2.InsSum[k, i, j] = 0;
                        Arr2.EarSum[k, i, j] = 0;
                        Arr2.AmpSum[k, i, j] = 0;
                        Arr2.CorSum[k, i, j] = 0;

                        Arr2.AmpMax[k, i, j] = -10000;
                        Arr2.CorMax[k, i, j] = -10000;
                        Arr2.AmpMin[k, i, j] = 10000;
                        Arr2.CorMin[k, i, j] = 10000;

                        Arr2.VelN[k, i, j] = 0;
                        Arr2.InsN[k, i, j] = 0;
                        Arr2.EarN[k, i, j] = 0;
                        Arr2.AmpN[k, i, j] = 0;
                        Arr2.CorN[k, i, j] = 0;

                        Arr2.VelDiffSum[k, i, j] = 0;
                        Arr2.VelDiffSumSqr[k, i, j] = 0;
                        Arr2.VelDiffN[k, i, j] = 0;

                        Arr2.VelSumI[k, i, j] = 0;
                        Arr2.VelSumSqrI[k, i, j] = 0;
                        Arr2.VelNI[k, i, j] = 0;

                        Arr2.VelSumE[k, i, j] = 0;
                        Arr2.VelSumSqrE[k, i, j] = 0;
                        Arr2.VelNE[k, i, j] = 0;
                    }
                    Arr2.PPVelSum[k, i] = 0;
                    Arr2.PPVelSumSqr[k, i] = 0;
                    Arr2.PPVelN[k, i] = 0;
                    Arr2.PPVelSD[k, i] = 0;
                }
            }
            ShowEnsemble(Arr, Arr2);
        }
        
        private void radioButtonStatisticsNone_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStatisticsNone.Checked)
            {
                ProfileView = ProfileViewCur;
                ShowEnsemble(Arr, Arr2);
            }
        }

        private void radioButtonStatisticsSD_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStatisticsSD.Checked)
            {
                ProfileView = ProfileViewSD;
                ShowEnsemble(Arr, Arr2);
            }
        }

        private void radioButtonStatisticsAVG_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStatisticsAVG.Checked)
            {
                ProfileView = ProfileViewAvg;
                ShowEnsemble(Arr, Arr2);
            }
        }

        private void checkBoxProfStatPeaks_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxProfStatPeaks.Checked)
                ShowEnsemble(Arr, Arr2);
        }
        bool ShowBottomTrack = false;
        private void radioButtonProfileDisplayGraph_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonProfileDisplayGraph.Checked)
            {
                groupBox10.Enabled = true;
                groupBox23.Enabled = true;
                ShowBottomTrack = false;
                ProfileGraph = true;
                ShowEnsemble(Arr, Arr2);
            }
        }

        private void radioButtonProfileDisplayText_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonProfileDisplayText.Checked)
            {
                groupBox10.Enabled = true;
                groupBox23.Enabled = true;
                ShowBottomTrack = false;
                ProfileGraph = false;
                ShowEnsemble(Arr, Arr2);
            }
        }

        private void radioButtonProfileDisplayBottomTrack_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonProfileDisplayBottomTrack.Checked)
            {
                groupBox23.Enabled = false;
                groupBox10.Enabled = false;
                ShowBottomTrack = true;
                ShowEnsemble(Arr, Arr2);

            }
        }

        int ProfileSubSystem = 0;
        private void buttonSeriesSubPlus_Click(object sender, EventArgs e)
        {
            ProfileSubSystem++;
            if (ProfileSubSystem > csubs - 1)
                ProfileSubSystem = csubs - 1;
            textBoxEnsembleSub.Text = ProfileSubSystem.ToString();
            textBoxSeriesSub.Text = ProfileSubSystem.ToString();
        }

        private void textBoxSeriesSub_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProfileSubSystem = Convert.ToInt32(textBoxEnsembleSub.Text);
                if (ProfileSubSystem < 0)
                    ProfileSubSystem = 0;
                if (ProfileSubSystem > csubs - 1)
                    ProfileSubSystem = csubs - 1;
                ShowSeries(Arr,Arr2);
                ShowEnsemble(Arr, Arr2);
            }
            catch { }
        }

        private void buttonSeriesSubMinus_Click(object sender, EventArgs e)
        {
            ProfileSubSystem--;
            if (ProfileSubSystem < 0)
                ProfileSubSystem = 0;
            textBoxEnsembleSub.Text = ProfileSubSystem.ToString();
            textBoxSeriesSub.Text = ProfileSubSystem.ToString();
        }

        private void buttonEnsembleSubMinus_Click(object sender, EventArgs e)
        {
            ProfileSubSystem--;
            if (ProfileSubSystem < 0)
                ProfileSubSystem = 0;
            textBoxEnsembleSub.Text = ProfileSubSystem.ToString();
            textBoxSeriesSub.Text = ProfileSubSystem.ToString();
        }
        private void buttonEnsembleSubPlus_Click(object sender, EventArgs e)
        {
            ProfileSubSystem++;
            if (ProfileSubSystem > csubs - 1)
                ProfileSubSystem = csubs - 1;
            textBoxEnsembleSub.Text = ProfileSubSystem.ToString();
            textBoxSeriesSub.Text = ProfileSubSystem.ToString();
        }
        private void textBoxEnsembleSub_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProfileSubSystem = Convert.ToInt32(textBoxEnsembleSub.Text);
                if (ProfileSubSystem < 0)
                    ProfileSubSystem = 0;
                if (ProfileSubSystem > csubs - 1)
                    ProfileSubSystem = csubs - 1;
                textBoxSeriesSub.Text = ProfileSubSystem.ToString();

                ShowEnsemble(Arr, Arr2);
            }
            catch { }

        }
        private void buttonPlusProfileScale_Click(object sender, EventArgs e)
        {
            VSpanIndex++;
            if (VSpanIndex >= VSpanIndexCount)
                VSpanIndex = VSpanIndexCount - 1;
            VSpan = (float)VSpanSetting[VSpanIndex];

            ShowEnsemble(Arr, Arr2);
        }
        private void buttonMinusProfileScale_Click(object sender, EventArgs e)
        {
            VSpanIndex--;
            if (VSpanIndex < 0)
                VSpanIndex = 0;
            VSpan = (float)VSpanSetting[VSpanIndex];

            ShowEnsemble(Arr, Arr2);
        }
        
        float VX1 = 80;
        float VX2 = 310;
        float VY1 = 60;//30;
        float VY2 = 360;

        float AX1 = 360;
        float AX2 = 530;
        float AY1 = 60;
        float AY2 = 360;

        float CX1 = 580;
        float CX2 = 749;
        float CY1 = 60;
        float CY2 = 360;

        float VSpan = 8;
        float VSpanSeries = 8;
        float VSpanBT = 8;
        float RSpanBT = 1000;

        

        int VSpanIndex = 1;
        const int VSpanIndexCount = 9;
        private double[] VSpanSetting = { 20, 8, 4, 2, 1, 0.4, 0.2, 0.1, 0.04 };
        int VSpanSeriesIndex = 1;
        const int VSpanSeriesIndexCount = 9;
        private double[] VSpanSeriesSetting = { 20, 8, 4, 2, 1, 0.4, 0.2, 0.1, 0.04 };

        float PSpanSeries = 100000;
        int PSpanSeriesIndex = 4;
        const int PSpanSeriesIndexCount = 11;
        private double[] PSpanSeriesSetting = { 1000000000, 100000000, 10000000, 1000000, 100000, 10000, 1000, 100, 10, 1, 0.1 };

        int VSpanBTIndex = 1;
        const int VSpanBTIndexCount = 9;
        private double[] VSpanBTSetting = { 20, 8, 4, 2, 1, 0.4, 0.2, 0.1, 0.04 };
        int RSpanBTIndex = 1;
        const int RSpanBTIndexCount = 11;
        private double[] RSpanBTSetting = { 2000, 1000, 500, 200, 100, 50, 20, 10, 5, 2, 1 };

        bool ProfileGraph = true;
        int ProfileView = 0;
        const int ProfileViewCur = 0;
        const int ProfileViewAvg = 1;
        const int ProfileViewSD = 2;

        void Graphics_PlotProfileFloatData(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                  float[,] Data, int Beam, int Bins, float Xscale, float Xoffset, Pen FPen, string LegendStr, int LegendN,
                  float Max, float Min, bool usedots)
        {
            SolidBrush Brush = new SolidBrush(FPen.Color);
            float X;

            float dYstep = ((Y2 - Y1) / Bins);
            float Y = Y1;
            float LastY = Y;

            bool GotFirstBin = false;
            float LastX = 0;
            /* 
            float LastX = Xoffset + (Xscale * (float)Data[Beam, 0]);
            if (LastX > X2)
                LastX = X2;
            if (LastX < X1)
                LastX = X1;
            */
            for (int i = 0; i < Bins; i++)
            {
                if (Data[Beam, i] < Max && Data[Beam, i] > Min)
                {
                    X = Xoffset + (Xscale * (float)Data[Beam, i]);
                    if (X > X2)
                        X = X2;
                    if (X < X1)
                        X = X1;

                    if (GotFirstBin == false)
                    {
                        LastX = X;
                        GotFirstBin = true;
                    }
                    if (usedots)
                        graphics.DrawRectangle(FPen, X, Y, 1, 1);
                    else
                        graphics.DrawLine(FPen, LastX, LastY, X, Y);
                    LastX = X;

                    LastY = Y;
                }

                Y += dYstep;
            }
            if (LegendStr != "")
            {
                X = X1 + (LegendN) * (X2 - X1) / 4;
                Y = Y2 + 50;
                graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
            }
        }
        void Graphics_PlotProfileFloatData3(System.Drawing.Graphics graphics, float X1, float X2, float Y1, float Y2,
                         float[,,] Data, int cs, int Beam, int Bins, float Xscale, float Xoffset, Pen FPen, string LegendStr, int LegendN,
                         float Max, float Min, bool usedots)
        {
            SolidBrush Brush = new SolidBrush(FPen.Color);
            float X;

            float dYstep = ((Y2 - Y1) / Bins);
            float Y = Y1;
            float LastY = Y;

            bool GotFirstBin = false;
            float LastX = 0;
            /* 
            float LastX = Xoffset + (Xscale * (float)Data[Beam, 0]);
            if (LastX > X2)
                LastX = X2;
            if (LastX < X1)
                LastX = X1;
            */
            for (int i = 0; i < Bins; i++)
            {
                if (Data[cs,Beam, i] < Max && Data[cs,Beam, i] > Min)
                {
                    X = Xoffset + (Xscale * (float)Data[cs,Beam, i]);
                    if (X > X2)
                        X = X2;
                    if (X < X1)
                        X = X1;

                    if (GotFirstBin == false)
                    {
                        LastX = X;
                        GotFirstBin = true;
                    }
                    if (usedots)
                        graphics.DrawRectangle(FPen, X, Y, 1, 1);
                    else
                        graphics.DrawLine(FPen, LastX, LastY, X, Y);
                    LastX = X;

                    LastY = Y;
                }

                Y += dYstep;
            }
            if (LegendStr != "")
            {
                X = X1 + (LegendN) * (X2 - X1) / 4;
                Y = Y2 + 50;
                graphics.DrawString(LegendStr, drawFont, Brush, X, Y, drawFormat);
            }
        }
        
        //bool ProfileAmpScaledB = true;

        void PlotProfile(Graphics graphics, ArrayClass m, ArrayClass2 m2)
        {
            Pen FPen = new Pen(Color.Blue, 2);
            string s = "";
            int i;

            float X1 = 0;
            float X2 = pictureBoxProfile.Width;
            float Y1 = 0;
            float Y2 = pictureBoxProfile.Height;
            Graphics_InitPlot(graphics, X1, X2, Y1, Y2, "");

            int cs = ProfileSubSystem;            
            int csE = (int)(m.E_CurrentSystem >> 24);

            if (cs < 0)
                cs = 0;
            if (cs > csubs - 1)
                cs = csubs - 1;

            if (csE < 0)
                csE = 0;
            if (csE > csubs - 1)
                csE = csubs - 1;

            VX1 = 80;
            VX2 = 310;
            AX1 = 360;
            AX2 = 530;
            CX1 = 580;
            CX2 = 749;

            float wX = (pictureBoxProfile.Width - VX1 - 150) / 3;

            VX2 = VX1 + wX;
            AX1 = VX2 + 50;
            AX2 = AX1 + wX;
            CX1 = AX2 + 50;
            CX2 = CX1 + wX;

            VY2 = Y2 - 82;
            AY2 = Y2 - 82;
            CY2 = Y2 - 82;

            //if (m.VelocityAvailable)
            {
                if (ProfileView > ProfileViewCur)
                {
                    textBoxEnsembleSub.Text = cs.ToString();
                    textBoxSeriesSub.Text = cs.ToString();
                }
                string st;
                float X0 = X1 + 70;
                float X = X0;
                float Y = Y1;
                if (m.EngProfileDataAvailable)
                {
                    graphics.DrawString("V", drawFont, drawBrush, 0, Y, drawFormat);
                    for (i = 0; i < 4; i++)
                    {
                        st = AddSpaces(m.Eng_ProfVel[i].ToString("0.000"), 10);
                        graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);
                        X += 60;
                    }
                    X = X0;
                    Y += 10;
                    graphics.DrawString("sd", drawFont, drawBrush, 0, Y, drawFormat);
                    for (i = 0; i < 4; i++)
                    {
                        st = AddSpaces(Arr2.PPVelSD[cs, i].ToString("0.000"), 10);
                        graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);
                        X += 60;
                    }
                    X = X0;
                    Y += 10;
                    graphics.DrawString("C", drawFont, drawBrush, 0, Y, drawFormat);
                    for (i = 0; i < 4; i++)
                    {
                        st = AddSpaces(m.Eng_ProfCor[i].ToString("0.000"), 10);
                        graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);
                        X += 60;
                    }
                    X = X0;
                    Y += 10;
                    graphics.DrawString("A", drawFont, drawBrush, 0, Y, drawFormat);
                    for (i = 0; i < 4; i++)
                    {
                        st = AddSpaces(m.Eng_ProfAmp[i].ToString("0.000"), 10);
                        graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);
                        X += 60;
                    }
                }
                //lag data
                X0 = X1 + 100;// + (X2 - X1) / 2;
                X = X0;
                Y = Y1;
                if (m.EngProfileDataAvailable)
                {
                    st = "      Lag CPCE NCE RepeatN     Gap Gain";
                    graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);

                    //short lag
                    X = X0;
                    Y += 10;
                    st = "Short " + AddSpaces(m.Eng_sLagSamples.ToString("0"), 3)
                                  + AddSpaces(m.Eng_sCPCE.ToString("0"), 5)
                                  + AddSpaces(m.Eng_sNCE.ToString("0"), 4)
                                  + AddSpaces(m.Eng_sRepeatN.ToString("0"), 8)
                                  + AddSpaces(m.Eng_sGap.ToString("0"), 8)
                                  + AddSpaces(m.Eng_RcvGain.ToString("0"), 5);

                    graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);

                    //long lag
                    X = X0;
                    Y += 10;
                    st = "Long  " + AddSpaces(m.Eng_LagSamples.ToString("0"), 3)
                                  + AddSpaces(m.Eng_CPCE.ToString("0"), 5)
                                  + AddSpaces(m.Eng_NCE.ToString("0"), 4)
                                  + AddSpaces(m.Eng_RepeatN.ToString("0"), 8);

                    graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);

                    //sample rate
                    X = X0;
                    Y += 10;
                    st = "Sample Rate " + AddSpaces(m.Eng_SamplesPerSecond.ToString("0.0") + " Hz", 12);
                    graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);
                    X = X0;
                    Y += 10;
                    st = "System Freq " + AddSpaces(m.Eng_SystemFrequency.ToString("0.0") + " Hz", 12);
                    graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);
                }
                else
                {
                    if (m.SystemSetupDataAvailable)
                    {
                        //st = "  Lag CPCE NCE RepeatN Volts";
                        if (m.E_FW_Vers[1] == 2)//ADCP01
                        {
                            st = "  Lag CPCE NCE RepeatN   InV   XmtV";
                        }
                        else///ADCP03
                        {
                            st = "  Lag CPCE NCE RepeatN   VINF     VT    VTL     VG   D3V3";
                        }
                        /* 
                        A_VINF;
                        A_VG;
                        A_VT;
                        A_VTL;
                        A_D3V3;
                        A_SPARE;
                        */
                        
                        graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);
                        X = X0;
                        Y += 10;

                        st = AddSpaces(m.SystemSetup_WPLagSamples.ToString("0"), 5)
                           + AddSpaces(m.SystemSetup_WPCPCE.ToString("0"), 5)
                           + AddSpaces(m.SystemSetup_WPNCE.ToString("0"), 4)
                           + AddSpaces(m.SystemSetup_WPRepeatN.ToString("0"), 8);
                        if (m.E_FW_Vers[1] == 2)//ADCP01
                        {   
                            st += AddSpaces(m.SystemSetup_InputVoltage.ToString("0.0"), 7);
                            st += AddSpaces(m.SystemSetup_TransmitVoltage.ToString("0.0"), 7);
                        }
                        else//ADCP03
                        {
                            st += (AddSpaces(m.A_VINF.ToString("0.0"), 7)
                                + AddSpaces(m.A_VT.ToString("0.0"), 7)
                                + AddSpaces(m.A_VTL.ToString("0.0"), 7)
                                + AddSpaces(m.A_VG.ToString("0.0"), 7)
                                + AddSpaces(m.A_D3V3.ToString("0.0"), 7));
                        }

                        graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);

                        //sample rate
                        X = X0;
                        Y += 10;
                        st = "Sample Rate " + AddSpaces(m.SystemSetup_WPSamplesPerSecond.ToString("0.0") + " Hz", 12);
                        graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);
                        X = X0;
                        Y += 10;
                        st = "System Freq " + AddSpaces(m.SystemSetup_WPSystemFreqHz.ToString("0.0") + " Hz", 12);
                        graphics.DrawString(st, drawFont, drawBrush, X, Y, drawFormat);
                    }
                }
                float vSteps;
                float vFirst;
                float vStepSize;

                if (ProfileView == ProfileViewCur)
                {
                    vSteps = 10;
                    vFirst = m2.firstbin[csE];
                    vStepSize = m2.binsize[csE] * m2.bins[csE] / 10;

                    if (m2.bins[csE] < 10)
                    {
                        vSteps = m2.bins[csE];
                        vFirst = m2.firstbin[csE];
                        vStepSize = m2.binsize[csE];
                    }
                }
                else
                {
                    vSteps = 10;
                    vFirst = m2.firstbin[cs];
                    vStepSize = m2.binsize[cs] * m2.bins[cs] / 10;

                    if (m2.bins[cs] < 10)
                    {
                        vSteps = m2.bins[cs];
                        vFirst = m2.firstbin[cs];
                        vStepSize = m2.binsize[cs];
                    }
                }

                //---------
                switch (ProfileView)
                {
                    case ProfileViewCur:
                        s = "Vel (m/s)";                        
                        Graphics_PlotGrid_Profile(graphics, VX1, VX2, VY1, VY2,
                              vSteps, vFirst, vStepSize, "Depth(m)", m.A_FirstCellDepth, m.A_CellSize, "",
                              9, -VSpan / 2, VSpan / 8, VSpan, s, true);
                        break;
                    case ProfileViewAvg:
                        s = "Vel Average(m/s)";                        
                        Graphics_PlotGrid_Profile(graphics, VX1, VX2, VY1, VY2,
                              vSteps, vFirst, vStepSize, "Depth(m)", m2.firstbin[cs], m2.binsize[cs], "",
                              9, -VSpan / 2, VSpan / 8, VSpan, s, true);
                        break;
                    case ProfileViewSD:
                        s = "Vel s.d.(m/s)";
                        Graphics_PlotGrid_Profile(graphics, VX1, VX2, VY1, VY2,
                              vSteps, vFirst, vStepSize, "Depth(m)", m2.firstbin[cs], m2.binsize[cs], "",
                              5, 0, VSpan / 4, VSpan, s, false);
                        break;
                }

                long beams = m.E_Beams;

                if (ProfileView > ProfileViewCur)
                {
                    beams = subbeams[cs];// m.E_Beams;
                }
                if (beams > 4)
                    beams = 4;

                for (i = 0; i < beams; i++)
                {
                    switch (i)
                    {
                        case 0:
                            FPen.Color = Color.Black;
                            break;
                        case 1:
                            FPen.Color = Color.Brown;
                            break;
                        case 2:
                            FPen.Color = Color.Red;
                            break;
                        case 3:
                            FPen.Color = Color.Orange;
                            break;
                    }
                    switch (ProfileCoordinateState)
                    {
                        case ProfBeam:
                            s = "B" + i.ToString();
                            switch (ProfileView)
                            {
                                case ProfileViewCur:
                                    if (m.VelocityAvailable)
                                        Graphics_PlotProfileFloatData(graphics, VX1, VX2, VY1, VY2, m.Velocity, i, (int)m.E_Cells, (float)(VX2 - VX1) / VSpan, VX2 - (VX2 - VX1) / 2, FPen, s, i, 80, -80, false);
                                    break;
                                case ProfileViewAvg:
                                    float[,] VelAvg = new float[4, 200];
                                    for (int bin = 0; bin < m2.bins[cs]; bin++)
                                    {
                                        if (m2.VelN[cs, i, bin] > 0)
                                            VelAvg[i, bin] = m2.VelSum[cs, i, bin] / m2.VelN[cs, i, bin];
                                        else
                                            VelAvg[i, bin] = 81;
                                    }
                                    Graphics_PlotProfileFloatData(graphics, VX1, VX2, VY1, VY2, VelAvg, i, (int)m2.bins[cs], (float)(VX2 - VX1) / VSpan, VX2 - (VX2 - VX1) / 2, FPen, s, i, 80, -80, false);
                                    break;
                                case ProfileViewSD:
                                    float[,] VelSD = new float[4, 200];
                                    for (int bin = 0; bin < m2.bins[cs]; bin++)
                                    {
                                        if (m2.VelDiffN[cs, i, bin] > 1)
                                        {
                                            VelSD[i, bin] = (float)(0.707106781 * Math.Sqrt((m2.VelDiffSumSqr[cs, i, bin] - ((m2.VelDiffSum[cs, i, bin] * m2.VelDiffSum[cs, i, bin]) / m2.VelN[cs, i, bin])) / (m2.VelN[cs, i, bin] - 1)));
                                        }
                                        else
                                            VelSD[i, bin] = 81;
                                    }
                                    Graphics_PlotProfileFloatData(graphics, VX1, VX2, VY1, VY2, VelSD, i, (int)m2.bins[cs], (float)(VX2 - VX1) / VSpan, VX1, FPen, s, i, 80, -80, false);

                                    break;
                            }
                            
                            break;
                        case ProfXYZ:

                            if (subbeams[cs] == 1)
                            {
                                s = "V";
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 0:
                                        s = "X";
                                        break;
                                    case 1:
                                        s = "Y";
                                        break;
                                    case 2:
                                        s = "Z";
                                        break;
                                    case 3:
                                        s = "Q";
                                        break;
                                }
                            }
                            switch (ProfileView)
                            {
                                case ProfileViewCur:
                                    if (m.InstrumentAvailable)
                                        Graphics_PlotProfileFloatData(graphics, VX1, VX2, VY1, VY2, m.Instrument, i, (int)m.E_Cells, (float)(VX2 - VX1) / VSpan, VX2 - (VX2 - VX1) / 2, FPen, s, i, 80, -80, false);
                                    break;
                                case ProfileViewAvg:
                                    float[,] InsAvg = new float[4, 200];
                                    for (int bin = 0; bin < m2.bins[cs]; bin++)
                                    {
                                        if (m2.InsN[cs, i, bin] > 0)
                                            InsAvg[i, bin] = m2.InsSum[cs, i, bin] / m2.InsN[cs, i, bin];
                                        else
                                            InsAvg[i, bin] = 81;
                                    }
                                    Graphics_PlotProfileFloatData(graphics, VX1, VX2, VY1, VY2, InsAvg, i, (int)m2.bins[cs], (float)(VX2 - VX1) / VSpan, VX2 - (VX2 - VX1) / 2, FPen, s, i, 80, -80, false);
                                    break;
                                case ProfileViewSD:
                                    float scale = 1;
                                    if (i == 3)
                                        scale = (float)(2.0 / (Math.Sqrt(2.0) * Math.Sin(20.0 / 180.0 * Math.PI)));
                                    
                                    float[,] VelSD = new float[4, 200];
                                    for (int bin = 0; bin < m2.bins[cs]; bin++)
                                    {
                                        if (m2.VelNI[cs, i, bin] > 1)
                                        {
                                            VelSD[i, bin] = scale*(float)(Math.Sqrt((m2.VelSumSqrI[cs, i, bin] - ((m2.VelSumI[cs, i, bin] * m2.VelSumI[cs, i, bin]) / m2.VelNI[cs, i, bin])) / (m2.VelNI[cs, i, bin] - 1)));
                                        }
                                        else
                                            VelSD[i, bin] = 81;
                                    }
                                    Graphics_PlotProfileFloatData(graphics, VX1, VX2, VY1, VY2, VelSD, i, (int)m2.bins[cs], (float)(VX2 - VX1) / VSpan, VX1, FPen, s, i, 80, -80, false);

                                    break;
                            }
                            break;
                        case ProfENU:
                            if (subbeams[cs] == 1)
                            {
                                s = "V";
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 0:
                                        s = "E";
                                        break;
                                    case 1:
                                        s = "N";
                                        break;
                                    case 2:
                                        s = "U";
                                        break;
                                    case 3:
                                        s = "Q";
                                        break;
                                }
                            }
                            switch (ProfileView)
                            {
                                case ProfileViewCur:
                                    if (m.EarthAvailable)
                                        Graphics_PlotProfileFloatData(graphics, VX1, VX2, VY1, VY2, m.Earth, i, (int)m.E_Cells, (float)(VX2 - VX1) / VSpan, VX2 - (VX2 - VX1) / 2, FPen, s, i, 80, -80, false);
                                    break;
                                case ProfileViewAvg:
                                    float[,] EarAvg = new float[4, 200];
                                    for (int bin = 0; bin < m2.bins[cs]; bin++)
                                    {
                                        if (m2.EarN[cs, i, bin] > 0)
                                            EarAvg[i, bin] = m2.EarSum[cs, i, bin] / m2.EarN[cs, i, bin];
                                        else
                                            EarAvg[i, bin] = 81;
                                    }
                                    Graphics_PlotProfileFloatData(graphics, VX1, VX2, VY1, VY2, EarAvg, i, (int)m2.bins[cs], (float)(VX2 - VX1) / VSpan, VX2 - (VX2 - VX1) / 2, FPen, s, i, 80, -80, false);
                                    break;
                                case ProfileViewSD:
                                    float scale = 1;
                                    if (i == 3)
                                        scale = (float)(2.0 / (Math.Sqrt(2.0) * Math.Sin(20.0 / 180.0 * Math.PI)));
                                    float[,] VelSD = new float[4, 200];
                                    for (int bin = 0; bin < m2.bins[cs]; bin++)
                                    {
                                        if (m2.VelNE[cs, i, bin] > 1)
                                        {
                                            VelSD[i, bin] = scale * (float)(Math.Sqrt((m2.VelSumSqrE[cs, i, bin] - ((m2.VelSumE[cs, i, bin] * m2.VelSumE[cs, i, bin]) / m2.VelNE[cs, i, bin])) / (m2.VelNE[cs, i, bin] - 1)));
                                        }
                                        else
                                            VelSD[i, bin] = 81;
                                    }
                                    Graphics_PlotProfileFloatData(graphics, VX1, VX2, VY1, VY2, VelSD, i, (int)m2.bins[cs], (float)(VX2 - VX1) / VSpan, VX1, FPen, s, i, 80, -80, false);

                                    break;
                            }
                            break;
                    }
                }
                //---------                
                
                if (ProfileView > ProfileViewCur)
                {
                    s = "Amp Average";
                }
                else
                {
                    s = "Amp";
                }
                //if (ProfileAmpScaledB)
                {
                    if (ProfileView > ProfileViewCur)
                    {
                        Graphics_PlotGrid_Profile(graphics, AX1, AX2, AY1, AY2,
                                      vSteps, vFirst, vStepSize, "", m2.firstbin[cs], m2.binsize[cs], "",
                                      6, 0, 25, 150, s, false);
                    }
                    else
                    {
                        Graphics_PlotGrid_Profile(graphics, AX1, AX2, AY1, AY2,
                                      vSteps, vFirst, vStepSize, "", m.A_FirstCellDepth, m.A_CellSize, "",
                                      6, 0, 25, 150, s, false);
                    }
                    //Graphics_PlotGrid_Profile(graphics, VX1, VX2, VY1, VY2,
                    //          vSteps, vFirst, vStepSize, "Depth(m)", m.A_FirstCellDepth, m.A_CellSize, "",
                    //          9, -VSpan / 2, VSpan / 8, VSpan, s, true);
                }
                //else
                //{
                 //   if (ProfileView > ProfileViewCur)
                 //   {
                 //       Graphics_PlotGrid_Profile(graphics, AX1, AX2, AY1, AY2,
                 //                     vSteps, vFirst, vStepSize, "", m2.firstbin[cs], m2.binsize[cs], "",
                 //                     5, 0, 1024, 4096, s, false);
                 //   }
                 //   else
                 //   {
                 //       Graphics_PlotGrid_Profile(graphics, AX1, AX2, AY1, AY2,
                 //                     vSteps, vFirst, vStepSize, "", m.A_FirstCellDepth, m.A_CellSize, "",
//                                      5, 0, 1024, 4096, s, false);
                   // }
                //}

                //int bm = (int)m.E_Beams;
                //if (ProfileView > ProfileViewCur)
                //    bm = (int)beams;

                for (i = 0; i < beams; i++)
                {
                    switch (i)
                    {
                        case 0:
                            FPen.Color = Color.Black;
                            break;
                        case 1:
                            FPen.Color = Color.Brown;
                            break;
                        case 2:
                            FPen.Color = Color.Red;
                            break;
                        case 3:
                            FPen.Color = Color.Orange;
                            break;
                    }
                    s = "A" + i.ToString();
                    if (ProfileView > ProfileViewCur)
                    {
                        float[,] AmpAvg = new float[4, 200];
                        for (int bin = 0; bin < m2.bins[cs]; bin++)
                        {
                            if (m2.AmpN[cs, i, bin] > 0)
                                AmpAvg[i, bin] = m2.AmpSum[cs, i, bin] / m2.AmpN[cs, i, bin];
                            else
                                AmpAvg[i, bin] = 0;
                        }
                        //if (ProfileAmpScaledB)
                        {
                            if (checkBoxProfStatPeaks.Checked)
                            {
                                Graphics_PlotProfileFloatData3(graphics, AX1, AX2, AY1, AY2, m2.AmpMax, cs, i, (int)m2.bins[cs], (float)(AX2 - AX1) / 150, AX1, FPen, s, i, 8888, -1, true);
                                Graphics_PlotProfileFloatData3(graphics, AX1, AX2, AY1, AY2, m2.AmpMin, cs, i, (int)m2.bins[cs], (float)(AX2 - AX1) / 150, AX1, FPen, s, i, 8888, -1, true);                                    
                            }
                            Graphics_PlotProfileFloatData(graphics, AX1, AX2, AY1, AY2, AmpAvg, i, (int)m2.bins[cs], (float)(AX2 - AX1) / 150, AX1, FPen, s, i, 8888, -1, false);
                        }
                        //else
                        //{
                        //    Graphics_PlotProfileFloatData(graphics, AX1, AX2, AY1, AY2, AmpAvg, i, (int)m2.bins[cs], (float)(AX2 - AX1) / 4096, AX1, FPen, s, i, 8888, -1, false);
                        //}
                    }
                    else
                    {
                        //if (ProfileAmpScaledB)
                        {
                            Graphics_PlotProfileFloatData(graphics, AX1, AX2, AY1, AY2, m.Amplitude, i, (int)m.E_Cells, (float)(AX2 - AX1) / 150, AX1, FPen, s, i, 8888, -1, false);
                        }
                        //else
                       // {
                       //     Graphics_PlotProfileFloatData(graphics, AX1, AX2, AY1, AY2, m.Amplitude, i, (int)m.E_Cells, (float)(AX2 - AX1) / 4096, AX1, FPen, s, i, 8888, -1, false);
                       // }
                    }
                }
                //---------
                float CorDenom = 1;
                float CorMin = -1;
                float CorX = CX1;
                //if ((m.E_Status & 0x10) == 0x10 && !ProfileAmpScaledB)
                //{
                //    CorDenom = 50;
                 //   CorMin = -200;
                //    CorX = CX2;
                //    if (ProfileView > ProfileViewCur)
                //        s = "Pow Average";
                //    else
                //        s = "Pow";
                //    Graphics_PlotGrid_Profile(graphics, CX1, CX2, CY1, CY2,
                //              vSteps, vFirst, vStepSize, "", 1, 1, "",
                //              5, -40, (float)10, 40, s, false);
               // }
                //else
                {
                    if (ProfileView > ProfileViewCur)
                        s = "Cor Average";
                    else
                        s = "Cor";
                    Graphics_PlotGrid_Profile(graphics, CX1, CX2, CY1, CY2,
                              vSteps, vFirst, vStepSize, "", 1, 1, "",
                              5, 0, (float)0.25, 1, s, false);
                }

                for (i = 0; i < beams; i++)
                {
                    switch (i)
                    {
                        case 0:
                            FPen.Color = Color.Black;
                            break;
                        case 1:
                            FPen.Color = Color.Brown;
                            break;
                        case 2:
                            FPen.Color = Color.Red;
                            break;
                        case 3:
                            FPen.Color = Color.Orange;
                            break;
                    }
                    //if ((m.E_Status & 0x10) == 0x10 && !ProfileAmpScaledB)
                    //    s = "P" + i.ToString();
                    //else
                        s = "C" + i.ToString();

                    if (ProfileView > ProfileViewCur)
                    {
                        float[,] CorAvg = new float[4, 200];
                        for (int bin = 0; bin < m2.bins[cs]; bin++)
                        {
                            if (m2.CorN[cs,i, bin] > 0)
                            {
                                CorAvg[i, bin] = m2.CorSum[cs,i, bin] / m2.CorN[cs,i, bin];
                            }
                            else
                                CorAvg[i, bin] = 0;
                        }
                        if (checkBoxProfStatPeaks.Checked)
                        {
                            Graphics_PlotProfileFloatData3(graphics, CX1, CX2, CY1, CY2, m2.CorMax, cs, i, (int)m2.bins[cs], (float)(CX2 - CX1) / CorDenom, CorX, FPen, s, i, 8, CorMin, true);
                            Graphics_PlotProfileFloatData3(graphics, CX1, CX2, CY1, CY2, m2.CorMin, cs, i, (int)m2.bins[cs], (float)(CX2 - CX1) / CorDenom, CorX, FPen, s, i, 8, CorMin, true);
                        }
                        Graphics_PlotProfileFloatData(graphics, CX1, CX2, CY1, CY2, CorAvg, i, (int)m2.bins[cs], (float)(CX2 - CX1) / CorDenom, CorX, FPen, s, i, 8, CorMin, false);
                    }
                    else
                        Graphics_PlotProfileFloatData(graphics, CX1, CX2, CY1, CY2, m.Correlation, i, (int)m.E_Cells, (float)(CX2 - CX1) / CorDenom, CorX, FPen, s, i, 8, CorMin, false);
                }
            }
        }

        string AncillaryString(ArrayClass m)
        {
            string s = "";

            s += "Ensemble" + AddSpaces(m.E_EnsembleNumber.ToString("0"), 8);
            s += "     Heading" + AddSpaces(m.A_Heading.ToString("0.00"), 10);
            s += "     First Bin (m)" + AddSpaces(m.A_FirstCellDepth.ToString("0.000"), 11);
            s += "     Wtemp    " + AddSpaces(m.A_WaterTemperature.ToString("0.00"), 10);
            if (m.E_FW_Vers[1] == 4)//ADCP03
            {
                s += ", HS1" + AddSpaces(m.A_HS1Temperature.ToString("0.00"), 7);
                s += ", RC1" + AddSpaces(m.A_RCV1Temperature.ToString("0.00"), 7);
            }
            //s += "  FW ";
            //s += m.E_FW_Vers[2].ToString() + ".";
            //s += m.E_FW_Vers[1].ToString() + ".";
            //s += m.E_FW_Vers[0].ToString();
            s += "\r\n";
            s += "Bins    " + AddSpaces(m.E_Cells.ToString("0"), 8);
            s += "     Pitch  " + AddSpaces(m.A_Pitch.ToString("0.00"), 10);
            s += "     Bin Size  (m)" + AddSpaces(m.A_CellSize.ToString("0.000"), 11);
            s += "     Btemp    " + AddSpaces(m.A_BoardTemperature.ToString("0.00"), 10);
            if (m.E_FW_Vers[1] == 4)//ADCP03
            {
                s += ", HS2" + AddSpaces(m.A_HS2Temperature.ToString("0.00"), 7);
                s += ", RC2" + AddSpaces(m.A_RCV2Temperature.ToString("0.00"), 7);
            }
            s += "\r\n";
            s += "Beams   " + AddSpaces(m.E_Beams.ToString("0"), 8);
            s += "     Roll   " + AddSpaces(m.A_Roll.ToString("0.00"), 10);
            s += "     1st Ping  (s)" + AddSpaces(m.A_FirstPingSeconds.ToString("0.000"), 11);
            s += "     Salinity " + AddSpaces(m.A_Salinity.ToString("0.00"), 10);
            s += "\r\n";
            s += "D Pings " + AddSpaces(m.E_PingsInEnsemble.ToString("0"), 8);
            s += "            " + "          ";
            s += "     Last Ping (s)" + AddSpaces(m.A_LastPingSeconds.ToString("0.000"), 11);
            s += "     Pressure " + AddSpaces(m.A_Pressure.ToString("0.000000"), 14);
            s += "\r\n";
            
            string st = "A Pings " + AddSpaces(m.E_PingCount.ToString("0"), 8);
            s += st;
            if (st.Length < 72)
                s += AddSpaces("", 72 - st.Length);

            s += "Depth" + AddSpaces(m.A_Depth.ToString("0.0000"), 16);
            s += "\r\n";
            s += "Status" + AddSpaces(m.E_Status.ToString("X04"), 5);
            s += "" + AddSpaces(m.E_Status2.ToString("X04"), 5);
            s += " Ensemble Time ";            

            st = m.E_Year.ToString("D4") + "/" + m.E_Month.ToString("D2") + "/" + m.E_Day.ToString("D2")
                + " " + m.E_Hour.ToString("D2") + ":" + m.E_Minute.ToString("D2") + ":" + m.E_Second.ToString("D2") + "." + m.E_Hsec.ToString("D2");

            s += st;
            if(st.Length < 41)
                s += AddSpaces("", 41 - st.Length);

            s += "SOS      " + AddSpaces(m.A_SpeedOfSound.ToString("0.00"), 10);
            s += "\r\n";
            st = "Mx " + AddSpaces(m.A_Mx.ToString("0.000"), 8) + " My " + AddSpaces(m.A_My.ToString("0.000"), 8) + " Mz " + AddSpaces(m.A_Mz.ToString("0.000"), 8);

            s += st;
            if (st.Length < 72)
                s += AddSpaces("", 72 - st.Length);


            s += "SN ";
            string st3;
            st3 = System.Text.ASCIIEncoding.ASCII.GetString(m.E_SN_Buffer, 0, 32);
            s += st3;            
            s += "\r\n";
            st = "Gp " + AddSpaces(m.A_Gp.ToString("0.000"), 8) + " Gr " + AddSpaces(m.A_Gr.ToString("0.000"), 8) + " Gz " + AddSpaces(m.A_Gz.ToString("0.000"), 8);

            s += st;
            if (st.Length < 72)
                s += AddSpaces("", 72 - st.Length);

            int cs = (int)(m.E_CurrentSystem >> 24);
            if (cs > csubs - 1)
                cs = csubs - 1;

            st = "SS " + cs.ToString();
            st += ",SStype ";
            if (m.E_FW_Vers[3] > 31 && m.E_FW_Vers[3] < 127)
                st += Convert.ToChar(m.E_FW_Vers[3]);
            s += st;
            s += ",FW ";
            s += m.E_FW_Vers[2].ToString() + ".";
            s += m.E_FW_Vers[1].ToString() + ".";
            s += m.E_FW_Vers[0].ToString();

            cs = (int)(0xFF & (m.E_CurrentSystem >> 16));
            st = ",ID " + cs.ToString();
            s += st;

            st = ",Index " + m.E_BurstIndex.ToString();
            s += st;

            s += "\r\n";
            s += "\r\n";

            return s;
        }

        void ProfileHideGraph(bool HideAll)
        {
            pictureBoxProfile.Hide();
            buttonPlusProfileScale.Hide();
            buttonMinusProfileScale.Hide();
            if (ShowBottomTrack && !HideAll)
            {
                buttonBTnavBinPlus.Show();
                checkBoxBTNAVshowalways.Show();
                checkBoxBTNAVRecalc.Show();
                checkBoxBTNAVuseZ.Show();
                buttonBTnavBinMinus.Show();
                textBoxBTNavBin.Show();
                textBoxBTNavBinScale.Show();
                textBoxBTNavBinScale.Show();
                checkBoxBTNAVshowalways.BringToFront();
                checkBoxBTNAVRecalc.BringToFront();
                checkBoxBTNAVuseZ.BringToFront();
                buttonBTnavBinMinus.BringToFront();
                textBoxBTNavBin.BringToFront();
                textBoxBTNavBinScale.BringToFront();
            }
            else
            {
                buttonBTnavBinPlus.Hide();
                checkBoxBTNAVshowalways.Hide();
                checkBoxBTNAVRecalc.Hide();
                checkBoxBTNAVuseZ.Hide();
                buttonBTnavBinMinus.Hide();
                textBoxBTNavBin.Hide();
                textBoxBTNavBinScale.Hide();
            }
        }
        void ProfileShowGraph()
        {
            pictureBoxProfile.Show();

            Application.DoEvents();
            buttonPlusProfileScale.Show();
            buttonPlusProfileScale.BringToFront();
            buttonMinusProfileScale.Show();
            buttonMinusProfileScale.BringToFront();
            Application.DoEvents();
            buttonBTnavBinPlus.Hide();
            checkBoxBTNAVshowalways.Hide();
            checkBoxBTNAVRecalc.Hide();
            checkBoxBTNAVuseZ.Hide();
            buttonBTnavBinMinus.Hide();
            textBoxBTNavBin.Hide();
            textBoxBTNavBinScale.Hide();
        }

        
        void ShowEnsemble(ArrayClass m, ArrayClass2 m2)
        {
            if (RiverTS.Available)
            {
                string STR;
                string Rstr = "Time(s), NMEA string\r\n";
                if (RiverTS.TimeStampGGA != 1000)
                {
                    Rstr += AddSpaces(RiverTS.TimeStampGGA.ToString("0.00"), 7) + ", ";
                    STR = System.Text.ASCIIEncoding.ASCII.GetString(RiverNMEA.GGA, 0, RiverNMEA.GGAbytes);
                    Rstr += STR;
                }

                if (RiverTS.TimeStampVTG != 1000)
                {
                    Rstr += AddSpaces(RiverTS.TimeStampVTG.ToString("0.00"), 7) + ", ";
                    STR = System.Text.ASCIIEncoding.ASCII.GetString(RiverNMEA.VTG, 0, RiverNMEA.VTGbytes);
                    Rstr += STR;
                }

                if (RiverTS.TimeStampHDT != 1000)
                {
                    Rstr += AddSpaces(RiverTS.TimeStampHDT.ToString("0.00"), 7) + ", ";
                    STR = System.Text.ASCIIEncoding.ASCII.GetString(RiverNMEA.HDT, 0, RiverNMEA.HDTbytes);
                    Rstr += STR;
                }

                if (RiverTS.TimeStampDBT != 1000)
                {
                    Rstr += AddSpaces(RiverTS.TimeStampDBT.ToString("0.00"), 7) + ", ";
                    STR = System.Text.ASCIIEncoding.ASCII.GetString(RiverNMEA.DBT, 0, RiverNMEA.DBTbytes);
                    Rstr += STR;
                }
                textBoxRiverNMEA.Text = Rstr;

            }
            else
            {
                textBoxRiverNMEA.Text = "";
            }
            if (RiverBT.Available)
            {
                string BTstr = "";// "Bottom Track:\r\n";
                int sbs = (int)RiverBT.Subs;
                if (sbs > BTMAXSUBS)
                    sbs = BTMAXSUBS;
                int sub = 1;
                for (int sb = 0; sb < sbs; sb++)
                {
                    if (RiverBT.PingCount[sb] > 0)
                    {
                        BTstr += "Bottom Track sub" + AddSpaces(sub.ToString(), 3) + ":";
                        BTstr += " Frequency(hz), " + AddSpaces(RiverBT.SystemFreqHz[sb].ToString(), 8) + "\r\n";
                        int bms = (int)RiverBT.Beams[sb];
                        if (bms > BTMAXBEAMS)
                            bms = BTMAXBEAMS;

                        BTstr += "      Range(m)";
                        for (int bm = 0; bm < bms; bm++)
                        {
                            BTstr += ", " + AddSpaces(RiverBT.Range[sb, bm].ToString("0.000"), 9);
                        }
                        BTstr += "\r\n";
                        BTstr += "       SNR(dB)";
                        for (int bm = 0; bm < bms; bm++)
                        {
                            BTstr += ", " + AddSpaces(RiverBT.SNR[sb, bm].ToString("0.000"), 9);
                        }
                        BTstr += "\r\n";
                        BTstr += "       AMP(dB)";
                        for (int bm = 0; bm < bms; bm++)
                        {
                            BTstr += ", " + AddSpaces(RiverBT.Amplitude[sb, bm].ToString("0.000"), 9);
                        }
                        BTstr += "\r\n";
                        BTstr += " NoiseAMP1(dB)";
                        for (int bm = 0; bm < bms; bm++)
                        {
                            BTstr += ", " + AddSpaces(RiverBT.NoiseAmpFrontPorch[sb, bm].ToString("0.000"), 9);
                        }
                        BTstr += "\r\n";
                        BTstr += " NoiseAMP2(dB)";
                        for (int bm = 0; bm < bms; bm++)
                        {
                            BTstr += ", " + AddSpaces(RiverBT.NoiseAmpBackPorch[sb, bm].ToString("0.000"), 9);
                        }
                        BTstr += "\r\n";

                        BTstr += "   Correlation";
                        for (int bm = 0; bm < bms; bm++)
                        {
                            BTstr += ", " + AddSpaces(RiverBT.Correlation[sb, bm].ToString("0.000"), 9);
                        }
                        BTstr += "\r\n";

                        BTstr += " Beam Vel(m/s)";
                        for (int bm = 0; bm < bms; bm++)
                        {
                            BTstr += ", " + AddSpaces(RiverBT.Velocity[sb, bm].ToString("0.000"), 9);
                        }
                        BTstr += "\r\n";
                        BTstr += "     Beam Good";
                        for (int bm = 0; bm < bms; bm++)
                        {
                            BTstr += ", " + AddSpaces(RiverBT.BeamN[sb, bm].ToString("0.000"), 9);
                        }
                        BTstr += "\r\n\r\n";
                        
                        sub++;
                    }
                    

                }
                int beam = 0;
                bool badbeam = false;
                try
                {
                    //BTstr += "\r\n";
                    if (textBoxRiverBeam.Text != "")
                    {
                        beam = Convert.ToInt32(textBoxRiverBeam.Text);
                    }
                }
                catch
                {
                    badbeam = true;// BTstr += "Invalid beam number\r\n";
                }
                if (beam >= BTHMAXBEAMS)
                    beam = BTHMAXBEAMS - 1;

                BTstr += "Beam " + beam.ToString() + " BT Echo:";
                if(badbeam)
                    BTstr += "Invalid beam number";
                BTstr += "\r\n       A       C       V       A       C       V\r\n";
                for (int bin = 0; bin < RiverBTH.Bins[0]; bin++)
                {
                    BTstr += AddSpaces(RiverBTH.Amp[0, beam, bin].ToString("0.000"), 8);
                    BTstr += AddSpaces(RiverBTH.Cor[0, beam, bin].ToString("0.000"), 8);
                    BTstr += AddSpaces(RiverBTH.Vel[0, beam, bin].ToString("0.000"), 8);
                    BTstr += AddSpaces(RiverBTH.Amp[1, beam, bin].ToString("0.000"), 8);
                    BTstr += AddSpaces(RiverBTH.Cor[1, beam, bin].ToString("0.000"), 8);
                    BTstr += AddSpaces(RiverBTH.Vel[1, beam, bin].ToString("0.000"), 8);
                    BTstr += "\r\n";
                }
                textBoxRiverBT.Text = BTstr;

                BTstr = "Bottom Track Ancillary:\r\n";
                BTstr += "Sub,   Time(s), Status, H(deg), P(deg), R(deg),  WT(C)\r\n";
                sub = 1;
                for (int sb = 0; sb < sbs; sb++)
                {
                    if (RiverBT.PingCount[sb] > 0)
                    {
                        BTstr += AddSpaces(sub.ToString(),3) +",";
                        BTstr += AddSpaces(RiverBT.PingSeconds[sb].ToString("0.00"), 10) + ",";
                        BTstr += AddSpaces("0x" + ((int)RiverBT.Status[sb]).ToString("X"), 7) + ",";
                        BTstr += AddSpaces(RiverBT.Heading[sb].ToString("0.00"), 7) + ",";
                        BTstr += AddSpaces(RiverBT.Pitch[sb].ToString("0.00"), 7) + ",";
                        BTstr += AddSpaces(RiverBT.Roll[sb].ToString("0.00"), 7) + ",";
                        BTstr += AddSpaces(RiverBT.WaterTemperature[sb].ToString("0.00"), 7);
                        
                        

                        /*
                        public float[] NCE = new float[BTMAXSUBS];
                        public float[] RepeatN = new float[BTMAXSUBS];
                        public float[] CPCE = new float[BTMAXSUBS];
                        public float[] BB = new float[BTMAXSUBS];
                        public float[] LL = new float[BTMAXSUBS];
                        public float[] BTbeamMux = new float[BTMAXSUBS];
                        public float[] NB = new float[BTMAXSUBS];
                        
                        public float[] BackPlaneTemperature = new float[BTMAXSUBS];
                        public float[] Salinity = new float[BTMAXSUBS];
                        public float[] Pressure = new float[BTMAXSUBS];
                        public float[] Depth = new float[BTMAXSUBS];
                        public float[] SpeedOfSound = new float[BTMAXSUBS];
                        public float[] Mx = new float[BTMAXSUBS];
                        public float[] My = new float[BTMAXSUBS];
                        public float[] Mz = new float[BTMAXSUBS];
                        public float[] Gp = new float[BTMAXSUBS];
                        public float[] Gr = new float[BTMAXSUBS];
                        public float[] Gz = new float[BTMAXSUBS];
                        public float[] SamplesPerSecond = new float[BTMAXSUBS];
                        */
                        
                        BTstr += "\r\n";
                        sub++;
                    }
                }
                BTstr += "\r\nDepth Sounder:\r\n";
                BTstr += "Range(m)," + AddSpaces(m.B_SounderRange.ToString("0.000"), 8);
                BTstr += "\r\n";
                BTstr += " SNR(dB)," + AddSpaces(m.B_SounderSNR.ToString("0.000"), 8);
                BTstr += "\r\n";
                BTstr += " Amp(dB)," + AddSpaces(m.B_SounderAmp.ToString("0.000"), 8);
                BTstr += "\r\n\r\n";

                //BTstr += "Transect:\r\n";
                BTstr += "Station Name: " + System.Text.ASCIIEncoding.ASCII.GetString(RiverTran.StationName, 0, RiverTran.LenName);
                BTstr += "\r\n";
                BTstr += "Station Number: " + System.Text.ASCIIEncoding.ASCII.GetString(RiverTran.StationNumber, 0, RiverTran.LenNumber);
                BTstr += "\r\n";

                BTstr += "Transect Number: " + RiverTran.TransectNumber.ToString() + "\r\n";
                

                
                BTstr += "State: ";// + RiverTran.TransectState.ToString() +"\r\n";

                switch(RiverTran.TransectState)
                {
                    case 0:
                        BTstr += "Stopped";
                        break;
                    case 1:
                        BTstr += "Compass Calibration";
                        break;
                    case 2:
                        BTstr += "Stationary Bed Test";
                        break;
                    case 3:
                        BTstr += "Loop Bed Test";
                        break;
                    case 4:
                        BTstr += "System Test";
                        break;
                    case 5:
                        BTstr += "Pinging";
                        break;
                    case 6:
                        if (RiverTran.CurrentEdge < 1)
                            BTstr += "Left Edge Collection";
                        else
                            BTstr += "Right Edge Collection";

                        break;
                    case 7:
                        BTstr += "Underway";
                        break;

                }
                BTstr += "\r\n";
                BTstr += "Status: " + RiverTran.TransectStatus.ToString() + "\r\n";
                BTstr += " BT: " + RiverTran.BottomStatus.ToString() + "\r\n";
                BTstr += " WP: " + RiverTran.ProfileStatus.ToString() + "\r\n";

                BTstr += "Ensembles: " + RiverTran.MovingEnsembles.ToString() + "\r\n";
                BTstr += " Good BT:  " + RiverTran.MovingBTEnsembles.ToString() + "\r\n";
                BTstr += " Good WP:  " + RiverTran.MovingWPEnsembles.ToString() + "\r\n";
                /*
                #define TRANSECT_STATE_STOP 0
                #define TRANSECT_STATE_COMPASSCAL 1
                #define TRANSECT_STATE_BEDSTATIONARY 2
                #define TRANSECT_STATE_BEDLOOP 3
                #define TRANSECT_STATE_SYSTEMTEST 4
                #define TRANSECT_STATE_ACQUIRING_BT 5
                #define TRANSECT_STATE_EDGE 6
                #define TRANSECT_STATE_MOVING 7
                */

                BTstr += "Left Edge:\r\n";
                BTstr += " Type: " + RiverTran.EdgeType[0].ToString() + "\r\n";
                BTstr += " Distance: " + RiverTran.EdgeDistance[0].ToString("0.000") + " meters\r\n";
                BTstr += " Ensembles: " + RiverTran.EdgeEnsembles[0].ToString() + "\r\n";
                BTstr += " Status: " + RiverTran.EdgeStatus[0].ToString() + "\r\n";

                BTstr += "Right Edge:\r\n";
                BTstr += " Type: " + RiverTran.EdgeType[1].ToString() + "\r\n";
                BTstr += " Distance: " + RiverTran.EdgeDistance[1].ToString("0.000") + " meters\r\n";
                BTstr += " Ensembles: " + RiverTran.EdgeEnsembles[1].ToString() + "\r\n";
                BTstr += " Status: " + RiverTran.EdgeStatus[1].ToString() + "\r\n";



                textBoxRiverBT2.Text = BTstr;
            }
            else
            {
                textBoxRiverBT.Text = "";
                textBoxRiverBT2.Text = "";
            }

            int cs = ProfileSubSystem;
            textBoxEnsembleSub.Text = ProfileSubSystem.ToString();
            int csE = (int)(m.E_CurrentSystem >> 24);
            if(csE > 11)
            {
                csE = 0;
            }
            {
                if (ShowBottomTrack)
                {
                    if (m.GageAvailable)
                    {
                        ProfileHideGraph(true);
                        SeriesBTDataType = TypeBTrange;
                        ShowGageEnsemble(Arr);
                    }
                    else
                    {   
                        ProfileHideGraph(false);
                        ShowBottomTrackEnsemble(Arr, checkBoxBTNAVshowalways.Checked);
                    }
                }
                else
                {
                    string s = AncillaryString(m);

                    if (ProfileGraph)
                    {
                        ProfileShowGraph();

                        Graphics g = Graphics.FromImage(pictureBoxProfile.Image);
                        if (GotData)
                        {
                            PlotProfile(g, m, m2);
                            pictureBoxProfile.Refresh();
                        }
                    }
                    else
                    {
                        int ss = cs;
                        if (ProfileView == ProfileViewCur)
                            ss = csE;
                       
                        if (FirstBin > m2.bins[ss] - 1)
                            FirstBin = m2.bins[ss] - 1;
                        if (FirstBin < 0)
                            FirstBin = 0;

                        int LastBin = m2.bins[ss];// FirstBin + 30;
                       // if (LastBin > m2.bins[ss])
                         //   LastBin = m2.bins[ss];

                        textBoxFirstBin.Text = (FirstBin).ToString();

                        int FirstBin2 = FirstBin;
                        if (FirstBin2 > m2.bins[ss] - 1)
                            FirstBin2 = m2.bins[ss] - 1;
                        if (FirstBin2 < 0)
                            FirstBin2 = 0;

                        int LastBin2 = FirstBin2 + 30;
                        if (LastBin2 > m2.bins[ss])
                            LastBin2 = m2.bins[ss];

                        switch (ProfileView)
                        {
                            case ProfileViewCur:

                                s += "Bin  Depth";

                                switch (subbeams[csE])
                                {
                                    case 1:
                                        if (m.VelocityAvailable)// && ProfileCoordinateState == ProfBeam)
                                            s += "      V0";
                                        if (m.AmplitudeAvailable)
                                            s += "    A0";
                                        if (m.CorrelationAvailable)
                                            //if ((m.E_Status & 0x10) == 0x10 && !ProfileAmpScaledB)
                                            //    s += "     P0";
                                            //else
                                                s += "     C0";
                                        if (m.BeamNAvailable || m.XfrmNAvailable)
                                            s += " NG0";
                                        break;
                                    default:
                                        if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                            s += "       X       Y       Z       Q";
                                        if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                            s += "       E       N       U       Q";
                                        if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                            s += "      V0      V1      V2      V3";

                                        if (m.AmplitudeAvailable)
                                            s += "    A0    A1    A2    A3";
                                        if (m.CorrelationAvailable)
                                            //if ((m.E_Status & 0x10) == 0x10 && !ProfileAmpScaledB)
                                            //    s += "     P0     P1     P2     P3";
                                            //else
                                                s += "     C0     C1     C2     C3";
                                        if (m.BeamNAvailable || m.XfrmNAvailable)
                                            s += " NG0 NG1 NG2 NG3";
                                        break;
                                    case 3:
                                        if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                            s += "       X       Y       Z";
                                        if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                            s += "       E       N       U";
                                        if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                            s += "      V0      V1      V2";

                                        if (m.AmplitudeAvailable)
                                            s += "    A0    A1    A2";
                                        if (m.CorrelationAvailable)
                                            //if ((m.E_Status & 0x10) == 0x10 && !ProfileAmpScaledB)
                                            //    s += "     P0     P1     P2";
                                            //else
                                                s += "     C0     C1     C2";
                                        if (m.BeamNAvailable || m.XfrmNAvailable)
                                            s += " NG0 NG1 NG2";
                                        break;
                                    case 2:
                                        if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                            s += "       X       Y";
                                        if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                            s += "       E       N";
                                        if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                            s += "      V0      V1";

                                        if (m.AmplitudeAvailable)
                                            s += "    A0    A1";
                                        if (m.CorrelationAvailable)
                                            //if ((m.E_Status & 0x10) == 0x10 && !ProfileAmpScaledB)
                                            //    s += "     P0     P1";
                                            //else
                                                s += "     C0     C1";
                                        if (m.BeamNAvailable || m.XfrmNAvailable)
                                            s += " NG0 NG1";
                                        break;
                                }
                                s += "\r\n";


                                if (subbeams[csE] == 1)
                                {
                                    for (int bin = FirstBin; bin < LastBin; bin++)
                                    {
                                        s += AddSpaces(bin.ToString("0"), 3);

                                        s += AddSpaces((m.A_FirstCellDepth + bin * m.A_CellSize).ToString("0.00"), 7);

                                        if (m.VelocityAvailable)
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.Velocity[beam, bin].ToString("0.000"), 8);
                                        if (m.AmplitudeAvailable)
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.Amplitude[beam, bin].ToString("0.0"), 6);
                                        if (m.CorrelationAvailable)
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.Correlation[beam, bin].ToString("0.00"), 7);
                                        if (m.BeamNAvailable)
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.BeamN[beam, bin].ToString("0"), 4);

                                        s += "\r\n";
                                    }
                                }
                                else
                                {
                                    for (int bin = FirstBin; bin < LastBin; bin++)
                                    {
                                        s += AddSpaces(bin.ToString("0"), 3);

                                        s += AddSpaces((m.A_FirstCellDepth + bin * m.A_CellSize).ToString("0.00"), 7);

                                        if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.Instrument[beam, bin].ToString("0.000"), 8);
                                        if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.Earth[beam, bin].ToString("0.000"), 8);
                                        if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.Velocity[beam, bin].ToString("0.000"), 8);

                                        if (m.AmplitudeAvailable)
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.Amplitude[beam, bin].ToString("0.0"), 6);
                                        if (m.CorrelationAvailable)
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.Correlation[beam, bin].ToString("0.00"), 7);

                                        if (m.XfrmNAvailable && (ProfileCoordinateState == ProfXYZ || ProfileCoordinateState == ProfENU))
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.XfrmN[beam, bin].ToString("0"), 4);
                                        if (m.BeamNAvailable && ProfileCoordinateState == ProfBeam)
                                            for (int beam = 0; beam < subbeams[csE]; beam++)
                                                s += AddSpaces(m.BeamN[beam, bin].ToString("0"), 4);

                                        s += "\r\n";
                                    }
                                }
                                break;
                            case ProfileViewAvg:
                                s += "Bin  Depth";

                                switch (m2.beams[ss])
                                {
                                    default:
                                        if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                            s += "      aX      aY      aZ      aQ";
                                        if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                            s += "      aE      aN      aU      aQ";
                                        if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                            s += "     aV0     aV1     aV2     aV3";

                                        if (m.AmplitudeAvailable)
                                            s += "   aA0   aA1   aA2   aA3";
                                        if (m.CorrelationAvailable)
                                            //if ((m.E_Status & 0x10) == 0x10 && !ProfileAmpScaledB)
                                            //    s += "    aP0    aP1    aP2    aP3";
                                            //else
                                                s += "    aC0    aC1    aC2    aC3";
                                        //if (m.BeamNAvailable || m.XfrmNAvailable)
                                            s += " NG0 NG1 NG2 NG3";
                                        break;
                                    case 3:
                                        if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                            s += "      aX      aY      aZ";
                                        if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                            s += "      aE      aN      aU";
                                        if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                            s += "     aV0     aV1     aV2";

                                        if (m.AmplitudeAvailable)
                                            s += "   aA0   aA1   aA2";
                                        if (m.CorrelationAvailable)
                                            //if ((m.E_Status & 0x10) == 0x10 && !ProfileAmpScaledB)
                                            //    s += "    aP0    aP1    aP2    aP3";
                                            //else
                                            s += "    aC0    aC1    aC2";
                                        //if (m.BeamNAvailable || m.XfrmNAvailable)
                                        s += " NG0 NG1 NG2";
                                        break;
                                    case 1:
                                        if (m.VelocityAvailable)// && ProfileCoordinateState == ProfBeam)
                                            s += "     aV0";

                                        if (m.AmplitudeAvailable)
                                            s += "   aA0";
                                        if (m.CorrelationAvailable)
                                                s += "    aC0";
                                        //if (m.BeamNAvailable || m.XfrmNAvailable)
                                            s += " NG0";
                                        break;
                                    case 2:
                                        if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                            s += "      aX      aY";
                                        if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                            s += "      aE      aN";
                                        if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                            s += "     aV0     aV1";

                                        if (m.AmplitudeAvailable)
                                            s += "   aA0   aA1";
                                        if (m.CorrelationAvailable)
                                                s += "    aC0    aC1";
                                        //if (m.BeamNAvailable || m.XfrmNAvailable)
                                            s += " NG0 NG1";
                                        break;
                                }
                                s += "\r\n";
                                //for (int bin = 0; bin < m2.bins[cs]; bin++)
                                for (int bin = FirstBin2; bin < LastBin2; bin++)
                                {
                                    s += AddSpaces((bin + 1).ToString("0"), 3);

                                    s += AddSpaces((m2.firstbin[ss] + bin * m2.binsize[ss]).ToString("0.00"), 7);
                                    if (ProfileCoordinateState == ProfXYZ)
                                    {
                                        if (m.InstrumentAvailable)
                                        {
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                            {
                                                if (m2.InsN[ss, beam, bin] > 0)
                                                    s += AddSpaces((m2.InsSum[ss, beam, bin] / m2.InsN[ss, beam, bin]).ToString("0.000"), 8);
                                                else
                                                    s += "  ------";
                                            }
                                        }
                                        else
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                                s += "  ------";
                                    }

                                    if (ProfileCoordinateState == ProfENU)
                                    {
                                        if (m.EarthAvailable)
                                        {
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                            {
                                                if (m2.EarN[ss, beam, bin] > 0)
                                                    s += AddSpaces((m2.EarSum[ss, beam, bin] / m2.EarN[ss, beam, bin]).ToString("0.000"), 8);
                                                else
                                                    s += "  ------";
                                            }
                                        }
                                        else
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                                s += "  ------";
                                    }
                                    
                                    if (ProfileCoordinateState == ProfBeam)
                                    {
                                        if (m.VelocityAvailable)
                                        {
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                            {
                                                if (m2.VelN[ss, beam, bin] > 0)
                                                    s += AddSpaces((m2.VelSum[ss, beam, bin] / m2.VelN[ss, beam, bin]).ToString("0.000"), 8);
                                                else
                                                    s += "  ------";
                                            }
                                        }
                                        else
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                                s += "  ------";
                                    }
                                    
                                    if (m.AmplitudeAvailable)
                                    {
                                        for (int beam = 0; beam < m2.beams[ss]; beam++)
                                        {
                                            if (m2.AmpN[ss, beam, bin] > 0)
                                                s += AddSpaces((m2.AmpSum[ss, beam, bin] / m2.AmpN[ss, beam, bin]).ToString("0.0"), 6);
                                            else
                                                s += "  ---";
                                        }
                                    }
                                    else
                                        for (int beam = 0; beam < m2.beams[ss]; beam++)
                                            s += "  ---";
                                    if (m.CorrelationAvailable)
                                    {
                                        for (int beam = 0; beam < m2.beams[ss]; beam++)
                                        {
                                            if (m2.CorN[ss, beam, bin] > 0)
                                                s += AddSpaces((m2.CorSum[ss, beam, bin] / m2.CorN[ss, beam, bin]).ToString("0.00"), 7);
                                            else
                                                s += "  ---";
                                        }
                                    }
                                    else
                                        for (int beam = 0; beam < m2.beams[ss]; beam++)
                                            s += "  ---";
                                    if (ProfileCoordinateState == ProfXYZ)
                                    {
                                        if (m.InstrumentAvailable)
                                        {
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                                s += AddSpaces(m2.InsN[ss, beam, bin].ToString("0"), 4);
                                        }
                                        else
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                                s += " ---";
                                    }

                                    if (ProfileCoordinateState == ProfENU)
                                    {
                                        if (m.EarthAvailable)
                                        {
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                                s += AddSpaces(m2.EarN[ss, beam, bin].ToString("0"), 4);
                                        }
                                        else
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                                s += " ---";
                                    }
                                    
                                    if (ProfileCoordinateState == ProfBeam)
                                    {
                                        if (m.VelocityAvailable)
                                        {
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                                s += AddSpaces(m2.VelN[ss, beam, bin].ToString("0"), 4);
                                        }
                                        else
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                                s += " ---";
                                    }
                                    

                                    s += "\r\n";
                                }
                                break;
                            case ProfileViewSD:
                                s += "Bin  Depth";

                                switch (subbeams[ss])
                                {
                                    default:
                                        //if (m.VelocityAvailable)
                                        {
                                            if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                                s += "      sX      sY      sZ      sQ";
                                            if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                                s += "      sE      sN      sU      sQ";
                                            if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                                s += "     sV0     sV1     sV2     sV3";
                                        }

                                        if (m.AmplitudeAvailable)
                                            s += "   aA0   aA1   aA2   aA3";
                                        if (m.CorrelationAvailable)
                                                s += "    aC0    aC1    aC2    aC3";
                                        //if (m.BeamNAvailable || m.XfrmNAvailable)
                                            s += " NG0 NG1 NG2 NG3";
                                        break;
                                    case 3:
                                        //if (m.VelocityAvailable)
                                        {
                                            if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                                s += "      sX      sY      sZ";
                                            if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                                s += "      sE      sN      sU";
                                            if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                                s += "     sV0     sV1     sV2";
                                        }

                                        if (m.AmplitudeAvailable)
                                            s += "   aA0   aA1   aA2";
                                        if (m.CorrelationAvailable)
                                            s += "    aC0    aC1    aC2";
                                        //if (m.BeamNAvailable || m.XfrmNAvailable)
                                        s += " NG0 NG1 NG2";
                                        break;
                                    case 1:
                                        if (m.VelocityAvailable)
                                            s += "     sV0";

                                        if (m.AmplitudeAvailable)
                                            s += "   aA0";
                                        if (m.CorrelationAvailable)
                                            //if ((m.E_Status & 0x10) == 0x10 && !ProfileAmpScaledB)
                                            //    s += "    aP0";
                                            //else
                                                s += "    aC0";
                                        //if (m.BeamNAvailable || m.XfrmNAvailable)
                                            s += " NG0";
                                        break;
                                    case 2:
                                        if (m.VelocityAvailable)
                                        {
                                            if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                                s += "      sX      sY";
                                            if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                                s += "      sE      sN";
                                            if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                                s += "     sV0     sV1";
                                        }

                                        if (m.AmplitudeAvailable)
                                            s += "   aA0   aA1";
                                        if (m.CorrelationAvailable)
                                            //if ((m.E_Status & 0x10) == 0x10 && !ProfileAmpScaledB)
                                            //    s += "    aP0    aP1";
                                            //else
                                                s += "    aC0    aC1";
                                        //if (m.BeamNAvailable || m.XfrmNAvailable)
                                            s += " NG0 NG1";
                                        break;
                                }
                                s += "\r\n";
                                //for (int bin = 0; bin < m2.bins[cs]; bin++)
                                for (int bin = FirstBin2; bin < LastBin2; bin++)
                                {
                                    s += AddSpaces((bin + 1).ToString("0"), 3);

                                    s += AddSpaces((m2.firstbin[ss] + bin * m2.binsize[ss]).ToString("0.00"), 7);

                                    //if (m.VelocityAvailable)
                                    {
                                        if (m.VelocityAvailable && ProfileCoordinateState == ProfBeam)
                                        {
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                            {
                                                if (m2.VelDiffN[ss, beam, bin] > 1)
                                                    s += AddSpaces((0.707106781 * Math.Sqrt((m2.VelDiffSumSqr[ss, beam, bin] - ((m2.VelDiffSum[ss, beam, bin] * m2.VelDiffSum[ss, beam, bin]) / m2.VelN[ss, beam, bin])) / (m2.VelN[ss, beam, bin] - 1))).ToString("0.000"), 8);
                                                else
                                                    s += "  ------";
                                            }
                                        }
                                        if (m.InstrumentAvailable && ProfileCoordinateState == ProfXYZ)
                                        {
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                            {
                                                float scale = 1;
                                                if (beam == 3)
                                                    scale = (float)(2.0 / (Math.Sqrt(2.0) * Math.Sin(20.0 / 180.0 * Math.PI)));
                                                if (m2.VelNI[ss, beam, bin] > 1)
                                                    s += AddSpaces((scale * Math.Sqrt((m2.VelSumSqrI[ss, beam, bin] - ((m2.VelSumI[ss, beam, bin] * m2.VelSumI[ss, beam, bin]) / m2.VelNI[ss, beam, bin])) / (m2.VelNI[ss, beam, bin] - 1))).ToString("0.000"), 8);
                                                else
                                                    s += "  ------";
                                            }
                                        }
                                        if (m.EarthAvailable && ProfileCoordinateState == ProfENU)
                                        {
                                            for (int beam = 0; beam < m2.beams[ss]; beam++)
                                            {
                                                double scale = 1;
                                                if (beam == 3)
                                                    scale = (float)(2.0 / (Math.Sqrt(2.0) * Math.Sin(20.0 / 180.0 * Math.PI)));
                                                
                                                if (m2.VelNE[ss, beam, bin] > 1)
                                                    s += AddSpaces((scale*Math.Sqrt((m2.VelSumSqrE[ss, beam, bin] - ((m2.VelSumE[ss, beam, bin] * m2.VelSumE[ss, beam, bin]) / m2.VelNE[ss, beam, bin])) / (m2.VelNE[ss, beam, bin] - 1))).ToString("0.000"), 8);
                                                else
                                                    s += "  ------";
                                            }
                                        }
                                    }
                                    if (m.AmplitudeAvailable)
                                    {
                                        for (int beam = 0; beam < m2.beams[ss]; beam++)
                                        {
                                            if (m2.AmpN[ss, beam, bin] > 0)
                                                s += AddSpaces((m2.AmpSum[ss, beam, bin] / m2.AmpN[ss, beam, bin]).ToString("0.0"), 6);
                                            else
                                                s += "  ---";
                                        }
                                    }
                                    if (m.CorrelationAvailable)
                                    {
                                        for (int beam = 0; beam < m2.beams[ss]; beam++)
                                        {
                                            if (m2.CorN[ss, beam, bin] > 0)
                                                s += AddSpaces((m2.CorSum[ss, beam, bin] / m2.CorN[ss, beam, bin]).ToString("0.00"), 7);
                                            else
                                                s += "  ---";
                                        }
                                    }

                                    //if (m.VelocityAvailable)
                                        for (int beam = 0; beam < m2.beams[ss]; beam++)
                                            s += AddSpaces(m2.VelN[ss, beam, bin].ToString("0"), 4);

                                    s += "\r\n";
                                }
                                break;
                        }
                        ProfileHideGraph(false);

                    }
                    if (GotData)
                        textBoxProfile.Text = s;
                }
            }
            //Application.DoEvents();
        }
        int FirstBin = 0;
        private void buttonBinPlus_Click(object sender, EventArgs e)
        {
            FirstBin++;
            textBoxFirstBin.Text = (FirstBin).ToString();
        }
        private void buttonBinMinus_Click(object sender, EventArgs e)
        {
            FirstBin--;
            if (FirstBin < 0)
                FirstBin = 0;
            textBoxFirstBin.Text = (FirstBin).ToString();
        }
        //Communications-------------------------------------------------------
        const int DefaultMaxTxtLen = 16000;

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int count;
            try
            {
                if (_serialPort.IsOpen)
                {
                    int bytes = _serialPort.BytesToRead;
                    byte[] buffer = new byte[bytes];
                    try
                    {
                        count = _serialPort.Read(buffer, 0, bytes);
                    }
                    catch
                    {
                        count = 0;
                    }
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)// move data into the internal circular buffer;
                        {
                            DataBuff[DataBuffWriteIndex] = buffer[i];
                            DataBuffWriteIndex++;
                            if (DataBuffWriteIndex >= MaxDataBuff)
                                DataBuffWriteIndex = 0;
                        }
                        BackScatter.DataBuffWriteIndex = DataBuffWriteIndex;
                    }
                }
            }
            catch //(TimeoutException ex) 
            {
                //Console.WriteLine(ex);
                //return;
            }
        }


        string GetTimeStr()
        {
            DateTime currentDate = DateTime.Now;

            string s = currentDate.Year.ToString("D4");
            s += "/" + currentDate.Month.ToString("D2");
            s += "/" + currentDate.Day.ToString("D2");
            s += " " + currentDate.Hour.ToString("D2");
            s += ":" + currentDate.Minute.ToString("D2");
            s += ":" + currentDate.Second.ToString("D2");

            return s;
        }

        DateTime lastDate = DateTime.Now;
        private void tmrTime_Tick(object sender, System.EventArgs e)
        {
            DateTime currentDate = DateTime.Now;

            if (currentDate.Second != lastDate.Second)
            {
                string s = currentDate.Year.ToString("D4");
                s += "/" + currentDate.Month.ToString("D2");
                s += "/" + currentDate.Day.ToString("D2");
                s += " " + currentDate.Hour.ToString("D2");
                s += ":" + currentDate.Minute.ToString("D2");
                s += ":" + currentDate.Second.ToString("D2");

                labelTime.Text = s;
                lastDate = currentDate;
            }
            if (CaptureData)
            {
                try
                {
                    int count = DataBuffWriteIndex - DataBuffReadIndexC;
                    if (count < 0)
                        count += MaxDataBuff;

                    if (count > 0)
                    {
                        byte[] buff = new byte[count];

                        for (int i = 0; i < count; i++)
                        {
                            buff[i] = DataBuff[DataBuffReadIndexC];
                            DataBuffReadIndexC++;
                            if (DataBuffReadIndexC > MaxDataBuff)
                                DataBuffReadIndexC = 0;
                        }
                        CaptureAppendPacket(buff, 0, count, @"c:\RoweTechRiverTools_Capture", CaptureFileName, false);
                        CapturedBytes += count;
                        FileBytes += count;
                        if (FileBytes > MaxFileBytes)
                        {
                            CaptureFileName = IncFileSequenceNumber(CaptureFileName);
                            FileBytes = 0;
                        }

                        textBoxCaptureStatus.Text = CapturedBytes.ToString();
                    }
                }
                catch { }
            }
            else
            {
                textBoxCaptureStatus.Text = "Capture OFF";
                DataBuffReadIndexC = DataBuffWriteIndex;
            }
        }
        //ticker
        bool tickerbusy = false;
        private void tmrTerminal_Tick(object sender, System.EventArgs e)
        {
            if (!tickerbusy)
            {
                tickerbusy = true;
                if (ClearTextScreen)
                {
                    txtSerialStr = "";
                    ClearTextScreen = false;
                    txtSerialNewData = true;
                }
                
                try
                {
                    int DBWI = DataBuffWriteIndex;
                    int count = DBWI - DataBuffReadIndexA;
                    if (count < 0)
                        count += MaxDataBuff;

                    if (count > 0)
                    {
                        int cc = 0;
                        bool clearit = false;
                        for (int i = 0; i < count; i++)
                        {
                            aBuff[cc] = DataBuff[DataBuffReadIndexA++];

                            if (DataBuffReadIndexA > MaxDataBuff)
                                DataBuffReadIndexA = 0;

                            switch (aBuff[cc])
                            {
                                case 0:
                                    aBuff[cc] = 58;// : instead of null for text display
                                    break;
                                case 0xc://form feed
                                    clearit = true;
                                    cc = -1;
                                    break;
                                case 0x06://ack
                                    aBuff[cc] = 43;// +
                                    break;
                                case 0x15://nack
                                    aBuff[cc] = 45;// -
                                    break;
                            }
                            cc++;
                        }

                        DataBuffReadIndexA = DBWI;

                        //display text data
                        aBuff[cc] = 0;//just in case it is a partial string
                        string message = System.Text.ASCIIEncoding.ASCII.GetString(aBuff, 0, cc);

                        if (clearit)
                        {
                            txtSerialStr = message;//txtSerial.Clear();                                
                        }
                        else
                            WriteMessageTxtSerial(message, false);

                        txtSerialNewData = true;
                    }
                }
                catch //(System.Exception ex)
                {
                    //ExSTR = ex.ToString();
                }
                
                int DataSize = DataBuffWriteIndex - DataBuffReadIndex;
                if (DataSize < 0)
                    DataSize += MaxDataBuff;

                if (DataBuffWriteIndex != DataBuffReadIndex)
                {
                    textBoxDataSize.Text = (DataSize).ToString();

                    if (radioButtonASCII.Checked) //checkBoxNMEA_ASCII_Input.Checked)
                    {
                        textBoxCapturedNMEA.Clear();
                        if (DataBuffWriteIndex != NmeaBuffReadIndex && NMEAEnableDecode)
                        {
                            int cs = 0;
                            DecodeNmea(cs, DataBuff, DataBuffWriteIndex, false, false);
                            doGGANav(cs);

                            DataBuffReadIndex = NmeaBuffReadIndex;
                        }
                    }
                    //if (radioButtonBinary.Checked)
                    {
                        if (DecodeData(false, false, false))
                        {
                            textBoxCapturedNMEA.Clear();
                            if (Arr.EnsembleDataAvailable)
                            {
                                int cs = (int)(Arr.E_CurrentSystem >> 24);
                                if (cs < 0)
                                    cs = 0;
                                if (cs > csubs - 1)
                                    cs = csubs - 1;

                                DecodeEnsembleNmea(Arr, cs);
                                
                                if (Arr.NmeaAvailable)
                                {
                                    NmeaBuffReadIndex = 0;
                                    DecodeNmea(cs, Arr.NMEA_Buffer, Arr.NMEA_Bytes, false, true);
                                }

                                AccumulateEnsemble(Arr, Arr2);
                                ProfileStats(Arr, false);

                                MoveSeriesData(Arr);

                                DoAllNav(Arr, cs);

                                float BTperr = 100;
                                if(GGANAV[cs].DMG > 0)
                                    BTperr = (float)(100.0 * (BTdisMag[cs] / GGANAV[cs].DMG - 1));

                                int index = SeriesIndex[cs] - 1;
                                if (index < 0)
                                    index = MaxSeries - 1;

                                if (NewGGA[cs] && FreshBT[cs])
                                {
                                    MagSeriesBT[cs, 5, index] = BTperr;
                                }
                                else
                                {
                                    MagSeriesBT[cs, 5, index] = 100;
                                }
                                
                                ShowEnsemble(Arr, Arr2);
                                ShowSeries(Arr, Arr2);

                            }
                            //are we getting to far behind?
                            if (DataSize > 8 * EnsembleSize)
                            {
                                //DataBuffReadIndex = DataBuffWriteIndex;//clear the buffer

                                DataBuffReadIndex += DataSize / 2;//reduce the buffer
                                if (DataBuffReadIndex > MaxDataBuff)
                                    DataBuffReadIndex = (DataBuffReadIndex - MaxDataBuff - 1);
                            }
                        }
                    }
                }
                tickerbusy = false;
            }
        }

        bool TextTickerBusy = false;
        private void tmrText_Tick(object sender, System.EventArgs e)
        {
            

            if (tabControl1.SelectedTab == tabPageTerminal && !TextTickerBusy)
            {
                if (txtSerialNewData && !tickerbusy)
                {
                    try
                    {
                        string it = "";
                        if (txtSerialStr.Length > 0)
                        {
                            it = txtSerialStr.Substring(txtSerialStr.Length - 1, 1);
                            if (it == "\r")
                                txtSerialStr += "\n";
                        }

                        //Application.DoEvents();
                        txtSerial.Text = txtSerialStr;

                        txtSerial.SelectionStart = txtSerial.Text.Length;
                        txtSerial.ScrollToCaret();

                        txtSerialNewData = false;
                    }                    
                    catch { }
                    TextTickerBusy = false;
                }
                //if (!tickerbusy)
                //    Application.DoEvents();
            }
        }

        void DoAllNav(ArrayClass m,int cs)
        {
            bool ok = true;
            if (FirstAllNav[cs])
            {
                if (FreshGGA[cs])
                {
                    if (m.XfrmNAvailable)
                    {
                        //int WPNAVBIN = Convert.ToInt32(textBoxBTNavBin.Text);

                        //if (WPNAVBIN < 0)
                        //    WPNAVBIN = 0;
                        //if (WPNAVBIN > m.Bins[0] - 1)
                        //    WPNAVBIN = m.Bins[0] - 1;

                        //if (m.XfrmN[0, WPNAVBIN] == 0)
                        //    ok = false;
                    }
                    if (m.BottomTrackAvailable)
                    {
                        //if (m.B_EarthN[3] == 0)// 4 beam solutions only
                        if (m.B_EarthN[0] == 0)//allow 3 beam solutions
                            ok = false;
                    }
                }
                else
                    ok = false;

                if (ok)
                    FirstAllNav[cs] = false;

            }
            if (ok || checkBoxBTNAVshowalways.Checked)
            {
                WaterProfileEnsembleNavigate(m, cs);
                BottomTrackEnsembleNavigate(m, cs, checkBoxBTNAVshowalways.Checked);
                doGGANav(cs);
            }
        }

        private bool IsPortAvailable(string ComPort, SerialPort Port, bool LeaveOpen)
        {
            bool Available;// = false;
            try
            {
                if (Port.PortName != ComPort)
                {
                    if (Port.IsOpen)
                    {
                        Port.Close();
                    }

                    Port.PortName = ComPort;
                }
                if (!Port.IsOpen)
                {
                    Port.Open();
                }
                if(!LeaveOpen)
                    Port.Close();
                Available = true;

                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show (String.Format("caughtB: {0}", ex.GetType().ToString()));
                Available =  false;
            }
            return Available;
        }
        private void OpenPort(string port, SerialPort Port)
        {
            Port.PortName = port;

            try
            {
                Port.Open();
                Port.DiscardInBuffer();
                
            }
            catch { }
        }

        //Comms ---------------------------------------------------------------------        
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }

            string curItem;
            if (listBoxAvailableMainPorts.SelectedItem != null)
            {
                try
                {
                    curItem = listBoxAvailableMainPorts.SelectedItem.ToString();

                    string portnum, comport;
                    portnum = curItem.Substring(3);

                    for (int i = 0; i < portnum.Length; i++)
                        if (portnum[i] < 48 || portnum[i] > 57)
                        {
                            portnum = curItem.Substring(3, i);
                            break;
                        }

                    int ThePort = Convert.ToInt32(portnum);

                    comport = "COM" + portnum;

                    if (IsPortAvailable(comport, _serialPort, true))
                    {
                        //System.Threading.Thread.Sleep(2000);

                        //OpenPort(comport, _serialPort);
                        txtMainPort.Text = curItem;
                        txtUserCommand.Enabled = true;
                        btnSendCom.Enabled = true;
                        if (_serialPort.IsOpen)
                        {
                            tmrTerminal.Enabled = true;
                            tmrTerminal.Start();
                        }
                    }
                }
                catch { }
            }
        }
    

        private void buttonCommsDiconnect_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
            //if (radioButtonEthernet.Checked)
            //    radioButtonSerial.Checked = true;
            txtMainPort.Text = "Closed";
        }
   
        private void btnCheckForPorts_Click(object sender, System.EventArgs e)
        {
            // Check for Availability of each of the Comm Ports, and
            //   place a check in the list box items that have openable ports.

            //tmrTerminal.Enabled = false;

            ListLoading = true;

            btnSendCom.Enabled = false;
            txtUserCommand.Enabled = false;

            listBoxAvailableMainPorts.Items.Clear();

            string[] ports = SerialPort.GetPortNames();
            int n = 0;
            
            foreach (string port in ports)
            {
                //txtSerial.Text += ports[n] + "\r\n";
                try
                {
                    string theport = ports[n];

                    char last = theport[theport.Length - 1];

                    while ((last < 0x30 || last > 0x39) && theport.Length > 3)
                    {
                        theport = ports[n].Substring(0, ports[n].Length - 1);
                        last = theport[theport.Length - 1];
                    }
                    listBoxAvailableMainPorts.Items.Add(theport);// ports[n]);
                    listBoxAvailableMainPorts.SelectedIndex = n;
                }
                catch 
                {
                    //n = n;
                }
                n++;
            }
            listBoxAvailableMainPorts.Items.Add("UDP");
            ListLoading = false;

            /*
            int portcount = 0;
            for (int i = 1; i < 333; i++)
            {
                if (IsPortAvailable("COM" + i.ToString(), _serialPort))
                {
                    listBoxAvailableMainPorts.Items.Add("COM" + i.ToString());
                    listBoxAvailableMainPorts.SelectedIndex = portcount;
                    portcount++;
                }
            }
             */
            if (!FormLoading)
            {
                //if (radioButtonSerial.Checked)
                    btnConnect_Click(sender, e);
            }
        }

        //-----------------------------------------------------------------
        /*
        public string GetPortInformation()
        {
            ManagementClass processClass = new ManagementClass("Win32_PnPEntity");
            ManagementObjectCollection Ports = processClass.GetInstances();
            foreach (ManagementObject property in Ports)
            {
                var name = property.GetPropertyValue("Name");
                if (name != null && name.ToString().Contains("USB") && name.ToString().Contains("COM"))
                {
                    var portInfo = new SerialPortInfo(property);
                    //Thats all information i got from port.
                    //Do whatever you want with this information
                }
            }
            return string.Empty;
        }
        */
        /*
        public string GetInfo()
        {
            string str = "";
            
            System.Management.ManagementObjectSearcher Searcher = new System.Management.ManagementObjectSearcher("Select * from WIN32_SerialPort");
            foreach (System.Management.ManagementObject Port in Searcher.Get())
            {
                foreach (System.Management.PropertyData Property in Port.Properties)
                {
                    //Console.WriteLine(Property.Name + " " + (Property.Value == null ? null : Property.Value.ToString()));
                    str += Property.Name + " " + (Property.Value == null ? null : Property.Value.ToString());
                }
            }            
            return str;
        }
        */
        //--------------------------------------------------------------
        bool FormLoading = true;
        bool ListLoading = true;
        private void listBoxAvailablePorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!FormLoading && !ListLoading)
            {
                string curItem = listBoxAvailableMainPorts.SelectedItem.ToString();

                // Find the string in ListBox.
                int index = listBoxAvailableMainPorts.FindString(curItem);
                // If the item was not found in ListBox display a message box, otherwise select it in ListBox.
                if (index == -1)
                    MessageBox.Show("Item is not available in ListBox2");
                else
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                    }
                    UsingSerial = true;
                        
                    string portnum, comport;

                    portnum = curItem.Substring(3);

                    for (int i = 0; i < portnum.Length; i++)
                    {
                        if (portnum[i] < 48 || portnum[i] > 57)
                        {
                            portnum = curItem.Substring(3, i);
                            break;
                        }
                    }
                    comport = "COM" + portnum;

                    if (IsPortAvailable(comport, _serialPort, true))
                    {
                        //OpenPort(curItem, _serialPort);
                        if(!(_serialPort.IsOpen))
                            OpenPort(comport, _serialPort);

                        txtMainPort.Text = curItem;
                        txtUserCommand.Enabled = true;

                        btnSendCom.Enabled = true;

                        //GetPortInformation();
                        //GetInfo();

                        
                    }
                    else
                        btnCheckForPorts_Click(sender, e);
                }
            }
        }
        private void listBoxBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listBoxMainPortBaud.SelectedItem.ToString();
            //int index = listBoxMainPortBaud.FindString(curItem);

            int Baud = Convert.ToInt32(curItem);

            _serialPort.BaudRate = Baud;

            textBoxCommsMainPortManBaud.Text = Baud.ToString();
        }
        private void buttonCommsSetBaud_Click(object sender, EventArgs e)
        {
            try
            {
                int Baud = Convert.ToInt32(textBoxCommsMainPortManBaud.Text);
                _serialPort.BaudRate = Baud;
                textBoxCommsMainPortManBaud.Text = Baud.ToString();
            }
            catch { }
        }
        private void buttonCommsSetMainPortParity_Click(object sender, EventArgs e)
        {
            try
            {   
                switch (textBoxCommsMainPortManParity.Text)
                {
                    case "None":
                        _serialPort.Parity = Parity.None;
                        break;
                    case "Even":
                        _serialPort.Parity = Parity.Even;
                        break;
                    case "Odd":
                        _serialPort.Parity = Parity.Odd;
                        break;
                    case "Mark":
                        _serialPort.Parity = Parity.Mark;
                        break;
                    case "Space":
                        _serialPort.Parity = Parity.Space;
                        break;
                }
                textBoxCommsMainPortManParity.Text = _serialPort.Parity.ToString();
            }
            catch { }
        }
        private void listBoxMainPortParity_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string curItem = listBoxMainPortParity.SelectedItem.ToString();
            //int index = listBoxMainPortParity.FindString(curItem);
            switch (curItem)
            {
                case "None":
                    _serialPort.Parity = Parity.None;
                    break;
                case "Even":
                    _serialPort.Parity = Parity.Even;
                    break;
                case "Odd":
                    _serialPort.Parity = Parity.Odd;
                    break;
                case "Mark":
                    _serialPort.Parity = Parity.Mark;
                    break;
                case "Space":
                    _serialPort.Parity = Parity.Space;
                    break;
            } 
            textBoxCommsMainPortManParity.Text = curItem;
        }
        private void listBoxMainPortBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listBoxMainPortBits.SelectedItem.ToString();
            //int index = listBoxMainPortBits.FindString(curItem);
            switch (curItem)
            {
                case "7":
                    _serialPort.DataBits = 7;
                    break;
                case "8":
                    _serialPort.DataBits = 8;
                    break;
            }
            textBoxCommsMainPortManParity.Text = curItem;
        }

        private void buttonCommsSetMainPortBits_Click(object sender, EventArgs e)
        {
            try
            {
                switch (textBoxCommsMainPortManBits.Text)
                {
                    case "7":
                        _serialPort.DataBits = 7;
                        break;
                    case "8":
                        _serialPort.DataBits = 8;
                        break;
                }
                textBoxCommsMainPortManBits.Text = _serialPort.DataBits.ToString();
            }
            catch { }
        }

        /*
        None,
        //
        // Summary:
        //     One stop bit is used.
        One,
        //
        // Summary:
        //     Two stop bits are used.
        Two,
        //
        // Summary:
        //     1.5 stop bits are used.
        OnePointFive
        */

        private void listBoxMainPortStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listBoxMainPortStopBits.SelectedItem.ToString();
            //int index = listBoxMainPortStopBits.FindString(curItem);
            switch (curItem)
            {
                case "1":
                    _serialPort.StopBits = StopBits.One;
                    break;
                case "2":
                    _serialPort.StopBits = StopBits.Two;
                    break;
            }
            textBoxCommsMainPortManParity.Text = curItem;
        }

        private void buttonCommsSetMainPortStopbits_Click(object sender, EventArgs e)
        {
            try
            {
                switch (textBoxCommsMainPortManStopBits.Text)
                {
                    case "1":
                        _serialPort.StopBits = StopBits.One;
                        break;
                    case "2":
                        _serialPort.StopBits = StopBits.Two;
                        break;
                }
                textBoxCommsMainPortManStopBits.Text = _serialPort.DataBits.ToString();
            }
            catch { }
        }
        //XMODEM---------------------------------------------------------------
        private bool XmodemCancelled = false;
        private bool XMODEM = false;
        private bool GotEOT = false;

        // xmodem control characters
        private const byte SOH = 0x01;
        private const byte STX = 0x02;
        private const byte EOT = 0x04;
        private const byte ACK = 0x06;
        private const byte DC1 = 0x11;
        private const byte NAK = 0x15;
        private const byte CAN = 0x18;
        private const byte CTRLZ = 0x1A;
        private const byte CR = 0x0D;

        private const int XMODEM_TIMEOUT_DELAY = 1; //second
        private const int XMODEM_RETRY_LIMIT = 5;//16;

        private int XmodemState = 0;

        private const int XmodemInit = 0;
        private const int XmodemXsent = 1;
        private const int XmodemXrcvd = 2;
        private const int XmodemEOTsent = 3;

        private bool XmodemGotC = false;
        private bool XmodemCancel = false;
        private bool XmodemFirstPacketSent = false;

        const int XmodemPackageSize = 1024;
        byte[] XmodemBuff = new byte[2000];
        byte[] XmodemData = new byte[2000];
        byte XmodemPacketNum = 0;
        private void StopXmodem()
        {
            txtSerialStr = "";
            btnSendCom.Enabled = true;
            txtUserCommand.Enabled = true;
            buttonFileUpload.Enabled = true;

            XMODEM = false;
            tmrTerminal.Interval = DefaultReadInterval;
            tmrTerminal.Enabled = true;
        }
        private void SendXmodemPacket(byte[] Data)
        {
            int i, j, k;
            int sum;

            int bytes;

            if (XmodemGotC)
            {
                long crc;
                long D;
                
                XmodemBuff[0] = STX;
                XmodemBuff[1] = XmodemPacketNum;
                XmodemBuff[2] = (byte)(255 - (int)XmodemPacketNum);

                crc = 0;
                j = 3;
                int packetSize;
                if (UsingSerial)
                {
                    packetSize = 1024;
                }
                else
                {
                    packetSize = 1000;
                }
                bytes = packetSize + 5;
                for (i = 0; i < packetSize; i++)
                {
                    XmodemBuff[j] = Data[i];
                    crc ^= (Data[i] << 8);
                    for (k = 0; k < 8; ++k)
                    {
                        long it = crc & 0x8000;
                        if (it == 0x8000)
                            crc = crc << 1 ^ 0x1021;
                        else
                            crc = (crc << 1);
                    }
                    j++;
                }
                crc = (crc & 0xFFFF);
                D = 0xFF & (crc >> 8);
                XmodemBuff[j] = (byte)D;
                D = 0xFF & crc;
                XmodemBuff[j + 1] = (byte)D;
            }
            else
            {
                bytes = 128 + 4;
                XmodemBuff[0] = SOH;
                XmodemBuff[1] = XmodemPacketNum;
                XmodemBuff[2] = (byte)(255 - (int)XmodemPacketNum);
                sum = 0;
                j = 3;
                for (i = 0; i < 128; i++)
                {
                    XmodemBuff[j] = Data[i];
                    sum += XmodemBuff[j];
                    j++;
                }
                XmodemBuff[j] = (byte)sum;
            }

            try
            {   
                 PNIwrite(XmodemBuff, 0, bytes);
            }
            catch { }
        }

        void XModemUpload(Stream stream, string FileName, bool FirmwareUpdate)
        {
            string exceptionmessage;
            int i;
            int count;
            int nBytesRead;

            
            for (i = 0; i < XmodemPackageSize; i++)
                XmodemData[i] = 0xFF;

            nBytesRead = stream.Read(XmodemData, 0, XmodemPackageSize);

            if (nBytesRead > 0)
            {
                tmrTerminal.Enabled = false;
                btnSendCom.Enabled = false;
                txtUserCommand.Enabled = false;
                buttonFileUpload.Enabled = false;

                XmodemState = 0;
                XmodemGotC = false;
                XmodemCancel = false;
                XmodemFirstPacketSent = false;
                XmodemPacketNum = 1;
                XMODEM = true;

                string message2 = FileName;
                string message1;
                if (textBoxFileSDcard.Text == "0")
                    message1 = "DSXR" + Path.GetFileName(message2) + "\r";
                else
                    message1 = "DSXR" + Path.GetFileName(message2) + "," + textBoxFileSDcard.Text + "\r";
                
                try
                {
                    if (UsingSerial)
                        _serialPort.Write(message1);
                    
                    XmodemState = XmodemXsent;

                    while (XMODEM)
                    {
                        if (XmodemCancel)
                        {
                            XMODEM = false;
                            CancelXmodem(true);
                        }
                        else
                        {
                            try
                            {
                                count = 0;
                                
                                int ByteCount = DataBuffWriteIndex - DataBuffReadIndex;
                                if (ByteCount < 0)
                                    ByteCount += MaxDataBuff;

                                if (ByteCount > 0)
                                {
                                    count = 1;
                                    bBuff[0] = DataBuff[DataBuffReadIndex++];
                                    if (DataBuffReadIndex >= MaxDataBuff)
                                        DataBuffReadIndex = 0;
                                }
                                
                                if (count > 0)
                                {
                                    bBuff[1] = 0;

                                    //string message = System.Text.ASCIIEncoding.ASCII.GetString(bBuff);
                                    string message = "";
                                    switch (bBuff[0])
                                    {
                                        default:
                                            message = System.Text.ASCIIEncoding.ASCII.GetString(bBuff, 0, 1);
                                            break;
                                        case 0x06:
                                            message = "+";
                                            break;
                                        case 0x15:
                                            message = "-";
                                            break;
                                        case 0x02:
                                            message = "/";
                                            break;
                                        case 0x04:
                                            message = "\\";
                                            break;
                                    }

                                    if (XmodemState == XmodemXsent)
                                    {
                                        switch (bBuff[0])
                                        {
                                            default:
                                                break;
                                            case ACK:
                                                XmodemState = XmodemXrcvd;
                                                break;
                                            case NAK:
                                                XMODEM = false;
                                                CancelXmodem(true);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        switch (bBuff[0])
                                        {
                                            default:
                                                //if (bBuff[0] > 126)
                                                //    message = bBuff[0].ToString("X2");
                                                break;
                                            case (byte)'C':
                                                XmodemGotC = true;
                                                SendXmodemPacket(XmodemData);
                                                XmodemFirstPacketSent = true;

                                                break;
                                            case ACK:
                                                if (XmodemState == XmodemEOTsent)
                                                {
                                                    StopXmodem();
                                                }
                                                else
                                                {
                                                    if (XmodemFirstPacketSent)
                                                    {
                                                        try
                                                        {
                                                            nBytesRead = stream.Read(XmodemData, 0, XmodemPackageSize);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            nBytesRead = 0;
                                                            exceptionmessage = String.Format("caughtH: {0}", ex.GetType().ToString());
                                                            if (FirmwareUpdate)
                                                                textBoxFirmware.Text += exceptionmessage;
                                                            WriteMessageTxtSerial(exceptionmessage, true);
                                                        }

                                                        if (nBytesRead > 0)
                                                        {
                                                            if (nBytesRead < XmodemPackageSize)
                                                            {
                                                                for (i = nBytesRead; i < XmodemPackageSize; i++)
                                                                    XmodemData[i] = 0xFF;
                                                            }
                                                            XmodemPacketNum++;
                                                            SendXmodemPacket(XmodemData);
                                                        }
                                                        else
                                                        {
                                                            XmodemState = XmodemEOTsent;
                                                            try
                                                            {
                                                                byte[] buff = new byte[2];
                                                                buff[0] = EOT;
                                                                PNIwrite(buff, 0, 1);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                nBytesRead = 0;
                                                                exceptionmessage = String.Format("caughtI: {0}", ex.GetType().ToString());
                                                                if (FirmwareUpdate)
                                                                    textBoxFirmware.Text += exceptionmessage;
                                                                WriteMessageTxtSerial(exceptionmessage, true);
                                                            }
                                                            XMODEM = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        SendXmodemPacket(XmodemData);
                                                        XmodemFirstPacketSent = true;
                                                    }
                                                }
                                                break;
                                            case NAK:
                                                SendXmodemPacket(XmodemData);
                                                XmodemFirstPacketSent = true;
                                                break;
                                            case CAN:
                                                message += "\r\nClient requests CANCEL\r\n";
                                                //WriteMessageTxtSerial(message, false);
                                                StopXmodem();
                                                break;
                                        }
                                    }
                                    if (FirmwareUpdate)
                                        textBoxFirmware.Text += message;
                                    WriteMessageTxtSerial(message, false);
                                }
                                //System.Threading.Thread.Sleep(10);
                                //Application.DoEvents();
                            }
                            catch { }
                            Application.DoEvents();
                        }
                    }
                }

                catch (Exception ex)
                {
                    XmodemState = XmodemInit;
                    exceptionmessage = String.Format("caughtJ: {0}", ex.GetType().ToString());
                    WriteMessageTxtSerial(exceptionmessage, true);
                }

                stream.Close();
            }
            StopXmodem();
        }

        private void buttonFileUpload_Click(object sender, EventArgs e)
        {
            
            Stream stream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            DisableButtons();

            txtSerialStr = "";// ClearTextScreen = true;//txtSerial.Clear();

            openFileDialog1.InitialDirectory = "";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog1.OpenFile()) != null)
                    {   
                        XModemUpload(stream, openFileDialog1.FileName,false);
                    }
                }
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("caughtK: {0}", ex.GetType().ToString());
                    WriteMessageTxtSerial(exceptionmessage, true);
                }
                stream.Close();
            }
            EnableButtons();
        }
        private void CancelXmodem(bool EnButtons)
        {
            XmodemCancelled = true;

            int sl;
            if (UsingSerial)
                sl = 1000;
            else
                sl = 100;
            
            try
            {
                byte[] buff = new byte[4];
                buff[0] = CAN;
                buff[1] = CAN;
                buff[2] = CAN;
                PNIwrite(buff, 0, 3);

                System.Threading.Thread.Sleep(sl);
                Application.DoEvents();

                buff[0] = Convert.ToByte('D');
                buff[1] = CR;
                PNIwrite(buff, 0, 2);

                System.Threading.Thread.Sleep(sl);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                WriteMessageTxtSerial(String.Format("caughtL: {0}", ex.GetType().ToString()), true);
            }
            
            if(EnButtons)
                EnableButtons();
        }
        
        private void buttonXmodemCancel_Click(object sender, EventArgs e)
        {
            
            /*
            RoweTech.MessageBox form = new RoweTech.MessageBox("Save Processed Data File?", "", "YES", "NO");
            if (form.ShowDialog() == DialogResult.OK)
            {
             
            }
            */
            XmodemCancel = true;
            WriteMessageTxtSerial("\r\nCanceling Transfer\r\n", true);
            try
            {
                int sl = 1000;
                if (UsingSerial)
                    sl = 1000;
                else
                    sl = 100;

                byte[] buff = new byte[4];
                buff[0] = 0x18;
                buff[1] = 0x18;
                buff[2] = 0x18;
                PNIwrite(buff, 0, 3);

                System.Threading.Thread.Sleep(sl);
                Application.DoEvents();

                buff[0] = Convert.ToByte('D');
                buff[1] = CR;
                PNIwrite(buff, 0, 2);

                System.Threading.Thread.Sleep(sl);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                WriteMessageTxtSerial(String.Format("Cancel: {0}", ex.GetType().ToString()), true);
            }
            EnableButtons();
        }
        private void StartXmodemDownload()
        {
            tmrTerminal.Enabled = false;
            btnSendCom.Enabled = false;
            txtUserCommand.Enabled = false;
            buttonFileUpload.Enabled = false;
            buttonXModemDownload.Enabled = false;
            //buttonXmodemCancel.Enabled = true;
        }
        private void StopXmodemDownload()
        {
            btnSendCom.Enabled = true;
            txtUserCommand.Enabled = true;
            buttonFileUpload.Enabled = true;
            //buttonXmodemCancel.Enabled = false;
            this.buttonXModemDownload.Enabled = true;
            Application.DoEvents();
            tmrTerminal.Enabled = true;
        }
        private void buttonXModemDownload_Click(object sender, EventArgs e)
        {
            DisableButtons();

            txtSerialStr = "";// ClearTextScreen = true;//txtSerial.Clear();

            string DirectoryName = @"c:\RoweTechRiverTools_Download_Data";

            int bytes = 1024;

            if (UsingSerial)
            {
                //if (_serialPort.BaudRate == 921600)
                //    bytes = 16384;
            }
            else
                bytes = 1000;

            
            XModemDownload(DirectoryName, txtUserCommand.Text, bytes, DOWNLOAD_SERIAL);

            EnableButtons();
        }

        string Header;
        private void XModemDownload(string DirectoryName, string FileName, int buffsize,int window)
        {
            int Bytes = 0;

            bool OK = true;
            bool C_ACK = false;
            bool done = false;
            int i, j, k, count;
            byte[] buff = new byte[4];
            byte[] Dbuff = new byte[buffsize + 5];
            byte[] Fbuff = new byte[buffsize];
            long elapsedSec;
            DateTime currentDate = DateTime.Now;
            long lastSec;// = currentDate.Second;
            int retry = XMODEM_RETRY_LIMIT;
            //string exceptionmessage;
            int seqnum = 1;

            int slowdown = 0;

            string message1;

            GotEOT = false;
            XmodemCancelled = false;
            /*
            try
            {   
                buff[0] = 0x18;//CAN
                buff[1] = 0x18;//CAN
                buff[2] = 0x18;//CAN
                PNIwrite(buff, 0, 3);

                System.Threading.Thread.Sleep(100);
                Application.DoEvents();

                buff[0] = Convert.ToByte('D');
                buff[1] = CR;
                PNIwrite(buff, 0, 2);
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
            catch
            {
            }
            try
            {
                buff[0] = 0xD;
                PNIwrite(buff,0,1);
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
            catch
            {
            }
            */
            
           
            DirectoryInfo di = new DirectoryInfo(DirectoryName);
            try
            {
                // Determine whether the directory exists.
                if (di.Exists)
                {
                }
                else
                {
                    // Try to create the directory.
                    di.Create();
                }
                // Delete the directory.
                //di.Delete();
            }
            catch
            {
                OK = false;

                DownLoadLogUpdate(DOWNLOAD_NODIR, 0, 0, 0, window, FileName,"");
                
            }
            string Path;// = "";

            int packets = 0;

            if (OK)
            {
                DownLoadTicks = currentDate.Ticks;

                textBoxDownloadRetries.Text = "";
                DownloadRetries = 0;
                textBoxDownloadTries.Text = "";
                DownloadTries = 0;

                DownLoadLogUpdate(DOWNLOAD_FILENAME, 0, Bytes, 0, window, FileName,Header);

                Path = DirectoryName + "\\" + FileName;

                FileStream fs;

                if (OK)
                {
                    try
                    {
                        if (File.Exists(Path))
                        {
                            fs = new FileStream(Path, FileMode.Create);
                        }
                        else
                            fs = new FileStream(Path, FileMode.CreateNew);

                        // Create the writer for data.
                        BinaryWriter w = new BinaryWriter(fs);

                        //if (_serialPort.IsOpen)
                        {
                            tmrTerminal.Interval = DefaultReadInterval;// 200;
                            txtSerialStr = "";
                            try
                            {
                                if (buffsize == 16384)
                                {
                                    if(textBoxFileSDcard.Text == "0")
                                        message1 = "DSXD" + FileName + '\r';
                                    else
                                        message1 = "DSXD" + FileName + "," + textBoxFileSDcard.Text + '\r';
                                }
                                else
                                {
                                    if (textBoxFileSDcard.Text == "0")
                                        message1 = "DSXT" + FileName + '\r';
                                    else
                                        message1 = "DSXT" + FileName + "," + textBoxFileSDcard.Text + '\r';
                                }
                                if (UsingSerial)
                                {
                                    if (_serialPort.IsOpen)
                                        _serialPort.Write(message1);
                                }

                                elapsedSec = 0;
                                currentDate = DateTime.Now;
                                lastSec = currentDate.Second;
                                XmodemCancel = false;
                                string tempstr = txtSerialStr;//.Text;
                                string tstr = "";
                                if (tempstr.Length >= 7)
                                {
                                    tstr = tempstr.Substring(tempstr.Length - 7, 5);
                                }
                                while (!XmodemCancel && tstr != "READY" && elapsedSec < 20)
                                {
                                    tempstr = txtSerialStr;
                                    if (tempstr.Length >= 7)
                                    {
                                        tstr = tempstr.Substring(tempstr.Length - 7, 5);
                                    }
                                    
                                    currentDate = DateTime.Now;
                                    elapsedSec = currentDate.Second - lastSec;
                                    if (elapsedSec < 0)
                                        elapsedSec += 60;
                                    Application.DoEvents();
                                }
                                
                                string s = "File Size =";
                                int ii = tempstr.IndexOf(s);
                                int jj = tempstr.IndexOf('\r',ii+s.Length);
                                string FS = tempstr.Substring(ii + s.Length, jj - ii - s.Length);
                                int FileSize = Convert.ToInt32(FS);

                                if (tstr == "READY")
                                {
                                    string filsizstr = tempstr.Substring(tempstr.Length - 19, 10);
                                    //long filesize = 0;
                                    long byteswritten = 0;
                                    bool canConvert = long.TryParse(filsizstr, out long filesize);

                                    StartXmodemDownload();
                                    XmodemCancel = false;

                                    long TO = 2;

                                    if (!UsingSerial)
                                    {
                                        TO = 2;
                                    }
                                    else
                                    {
                                        if (buffsize == 16384)
                                        {
                                            switch (_serialPort.BaudRate)
                                            {
                                                default:
                                                    TO = 10;
                                                    break;
                                                case 921600:
                                                    TO = 2;
                                                    break;
                                                case 115200:
                                                    TO = 4;
                                                    break;
                                            }
                                        }
                                    }

                                    Header = txtSerialStr;
                                    DownLoadLogUpdate(DOWNLOAD_C, 0, Bytes, 0, window, FileName,Header);
                                    
                                    buff[0] = (byte)'C';
                                    PNIwrite(buff, 0, 1);
                                    while (!done)
                                    {
                                        try
                                        {
                                            currentDate = DateTime.Now;
                                            elapsedSec = currentDate.Second - lastSec;
                                            if (elapsedSec < 0)
                                                elapsedSec += 60;

                                            if (elapsedSec > TO)
                                            {
                                                lastSec = currentDate.Second;
                                                if (!C_ACK)
                                                {
                                                    DownLoadLogUpdate(DOWNLOAD_C, 0, Bytes, 0, window, FileName, Header);
                                                    
                                                    buff[0] = (byte)'C';
                                                    PNIwrite(buff, 0, 1);
                                                }
                                                else
                                                {
                                                    DownLoadLogUpdate(DOWNLOAD_N, 0, Bytes, 0, window, FileName, Header);

                                                    //empty out the buffer
                                                    System.Threading.Thread.Sleep(1000);
                                                    DataBuffReadIndex = DataBuffWriteIndex;
                                                    DataBuffReadIndexA = DataBuffReadIndex;

                                                    //attempt a resend
                                                    buff[0] = NAK;
                                                    PNIwrite(buff, 0, 1);
                                                }
                                                DownloadRetries++;
                                                textBoxDownloadRetries.Text = DownloadRetries.ToString();
                                                retry--;
                                                if (retry < 1)
                                                    XmodemCancel = true;
                                            }
                                            else
                                            {
                                                count = 0;
                                                Application.DoEvents();
                                                
                                                int ByteCount = DataBuffWriteIndex - DataBuffReadIndex;
                                                if (ByteCount < 0)
                                                    ByteCount += MaxDataBuff;

                                                if (ByteCount >= buffsize + 5)
                                                {
                                                    count = ByteCount;
                                                    bBuff[0] = DataBuff[DataBuffReadIndex++];
                                                    if (DataBuffReadIndex > MaxDataBuff)
                                                        DataBuffReadIndex = 0;
                                                    DataBuffReadIndexA = DataBuffReadIndex;
                                                }

                                                if (count >= buffsize + 5)
                                                {
                                                    bBuff[1] = 0;
                                                    switch (bBuff[0])
                                                    {
                                                        default:
                                                            DownLoadLogUpdate(DOWNLOAD_I, 0, Bytes, 0, window, FileName, Header);
                                                            System.Threading.Thread.Sleep(1000);
                                                            DataBuffReadIndex = DataBuffWriteIndex;
                                                            DataBuffReadIndexA = DataBuffReadIndex;
                                                            break;
                                                        case CAN:
                                                            break;
                                                        case SOH:
                                                            C_ACK = true;
                                                            DownLoadLogUpdate(DOWNLOAD_SOH, 0, Bytes, 0, window, FileName, Header);
                                                            //pktsize = 128;
                                                            break;
                                                        case STX:
                                                            C_ACK = true;
                                                            //DownLoadLogUpdate(DOWNLOAD_STX, 0, Bytes, 0, window, FileName, Header);
                                                            Dbuff[0] = bBuff[0];
                                                            currentDate = DateTime.Now;
                                                            lastSec = currentDate.Second;

                                                            for (i = 1; i < buffsize + 5; i++)
                                                            {
                                                                currentDate = DateTime.Now;
                                                                elapsedSec = currentDate.Second - lastSec;
                                                                if (elapsedSec < 0)
                                                                    elapsedSec += 60;

                                                                if (elapsedSec < TO)
                                                                {
                                                                    bBuff[0] = DataBuff[DataBuffReadIndex++];

                                                                    if (DataBuffReadIndex > MaxDataBuff)
                                                                        DataBuffReadIndex = 0;
                                                                    DataBuffReadIndexA = DataBuffReadIndex;

                                                                    Dbuff[i] = bBuff[0];
                                                                    currentDate = DateTime.Now;
                                                                    lastSec = currentDate.Second;
                                                                }
                                                                else//timeout
                                                                    break;
                                                            }

                                                            if (i == buffsize + 5)//got one!
                                                            {
                                                                DownloadTries++;
                                                                textBoxDownloadTries.Text = DownloadTries.ToString();
                                                                packets++;
                                                                long crc;
                                                                long pktcrc = 0;
                                                                crc = 0;
                                                                j = 3;

                                                                for (i = 0; i < buffsize; i++)
                                                                {
                                                                    Fbuff[i] = Dbuff[j];
                                                                    //crc = crc ^ (Fbuff[i] << 8);
                                                                    crc ^= (Fbuff[i] << 8);
                                                                    for (k = 0; k < 8; ++k)
                                                                    {
                                                                        long it = crc & 0x8000;
                                                                        if (it == 0x8000)
                                                                            crc = (crc << 1) ^ 0x1021;
                                                                        else
                                                                            crc = (crc << 1);
                                                                    }
                                                                    crc &= 0xFFFF;
                                                                    j++;
                                                                }
                                                                pktcrc = (0xFF & Dbuff[j]) << 8;
                                                                pktcrc += 0xFF & Dbuff[j + 1];

                                                                if (pktcrc == crc && seqnum == Dbuff[1] && seqnum == (0xFF & (255 - Dbuff[2])))
                                                                {
                                                                    buff[0] = ACK;
                                                                    PNIwrite(buff, 0, 1);

                                                                    Bytes += buffsize;

                                                                    textBoxDownloadBytes.Text = Bytes.ToString();
                                                                    currentDate = DateTime.Now;
                                                                    double Aseconds = (double)(currentDate.Ticks - DownLoadTicks) / 10000000.0;
                                                                    textBoxDownloadSeconds.Text = Aseconds.ToString("0.0");
                                                                    float pc = 100 * (float)Bytes / (float)filesize;
                                                                    if (pc > 0)
                                                                        textBoxDownloadPercent.Text = pc.ToString("0.0");
                                                                    else
                                                                        textBoxDownloadPercent.Text = "NA";

                                                                    double bps = 10.0 * (double)Bytes / Aseconds;
                                                                    textBoxDownloadBPS.Text = bps.ToString("0.0");

                                                                    DownLoadLogUpdate(DOWNLOAD_COLON, 0, Bytes, 0, window, FileName, Header);

                                                                    byteswritten += buffsize;

                                                                    if (byteswritten <= filesize)
                                                                    {
                                                                        w.Write(Fbuff);
                                                                    }
                                                                    else
                                                                    {
                                                                        int bytes = buffsize - (int)(byteswritten - filesize);
                                                                        w.Write(Fbuff, 0, bytes);
                                                                    }

                                                                    if (slowdown > 0 && UsingSerial)
                                                                        System.Threading.Thread.Sleep(slowdown);

                                                                    if (retry == XMODEM_RETRY_LIMIT)
                                                                    {
                                                                        //slowdown -= 10;
                                                                        if (slowdown < 0)
                                                                            slowdown = 0;
                                                                    }
                                                                    retry = XMODEM_RETRY_LIMIT;
                                                                    seqnum++;
                                                                    if (seqnum > 255)
                                                                        seqnum = 0;
                                                                }
                                                                else//bad checksum or wrong seqnum
                                                                {
                                                                    DownLoadLogUpdate(DOWNLOAD_M, 0, Bytes, 0, window, FileName, Header);

                                                                    if (pktcrc != crc)
                                                                    {
                                                                        DataBuffReadIndex = DataBuffWriteIndex;
                                                                        DataBuffReadIndexA = DataBuffReadIndex;

                                                                        buff[0] = NAK;
                                                                        PNIwrite(buff, 0, 1);
                                                                        if (UsingSerial)
                                                                            System.Threading.Thread.Sleep(50);
                                                                        Application.DoEvents();

                                                                        slowdown += 10;
                                                                        if (slowdown > 150)
                                                                            slowdown = 150;
                                                                        DownloadRetries++;
                                                                        textBoxDownloadRetries.Text = DownloadRetries.ToString();
                                                                        retry--;
                                                                        if (retry < 1)
                                                                            XmodemCancel = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        buff[0] = ACK;
                                                                        PNIwrite(buff, 0, 1);
                                                                        if (UsingSerial)
                                                                            System.Threading.Thread.Sleep(50);
                                                                        Application.DoEvents();
                                                                    }
                                                                }
                                                            }//incorrect buffsize 
                                                            else
                                                            {
                                                                DataBuffReadIndex = DataBuffWriteIndex;
                                                                DataBuffReadIndexA = DataBuffReadIndex;

                                                                DownLoadLogUpdate(DOWNLOAD_n, 0, Bytes, 0, window, FileName, Header);

                                                                buff[0] = NAK;
                                                                PNIwrite(buff, 0, 1);
                                                                DownloadRetries++;
                                                                textBoxDownloadRetries.Text = DownloadRetries.ToString();
                                                                retry--;
                                                                if (retry < 1)
                                                                    XmodemCancel = true;
                                                            }
                                                            break;
                                                        case EOT:
                                                            GotEOT = true;
                                                            C_ACK = true;
                                                            done = true;
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (ByteCount > 0)
                                                    {
                                                        if (DataBuff[DataBuffReadIndex] == EOT)
                                                        {
                                                            GotEOT = true;
                                                            C_ACK = true;
                                                            done = true;
                                                            DataBuffReadIndex++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch //(System.Exception ex)
                                        {
                                            //exceptionmessage = String.Format("XMODEMA: {0}", ex.GetType().ToString());
                                            //if (Waves)
                                            //    textBoxWavesRecoverDownload.Text = textBoxWavesRecoverDownload.Text + exceptionmessage + "\r\n";
                                            //else
                                            //    WriteMessageTxtSerial(exceptionmessage, true);
                                        }
                                        if (XmodemCancel)
                                        {
                                            CancelXmodem(true);
                                            done = true;
                                        }
                                        Application.DoEvents();
                                    }//done
                                    if(GotEOT)
                                    {
                                        DownLoadLogUpdate(DOWNLOAD_EOT, packets, Bytes, 0, window, FileName, Header);                                        
                                        DataBuffReadIndexA = DataBuffReadIndex;
                                    }
                                }
                                else
                                {
                                    CancelXmodem(true);
                                }
                            }
                            catch// (System.Exception ex)
                            {
                                //exceptionmessage = String.Format("XMODEMB: {0}", ex.GetType().ToString());                                
                                //textBoxWavesRecoverDownload.Text = textBoxWavesRecoverDownload.Text + exceptionmessage + "\r\n";
                                //WriteMessageTxtSerial(exceptionmessage, true);
                            }
                            StopXmodemDownload();
                            tmrTerminal.Interval = DefaultReadInterval;
                        }
                        w.Close();
                        fs.Close();
                    }
                    catch //(System.Exception ex)
                    {
                        //exceptionmessage = String.Format("XMODEMC: {0}", ex.GetType().ToString());
                        //if (Waves)
                        //    textBoxWavesRecoverDownload.Text = textBoxWavesRecoverDownload.Text + exceptionmessage + "\r\n";
                        //else
                        //    WriteMessageTxtSerial(exceptionmessage, true);
                        //OK = false;
                        CancelXmodem(true);
                    }
                    //CancelXmodem(false);
                }
                DownLoadLogUpdate(DOWNLOAD_DONE, packets, Bytes, 0, window, FileName, Header);
                
                currentDate = DateTime.Now;
                double seconds = (double)(currentDate.Ticks - DownLoadTicks) / 10000000.0;//185,850,630 
                DownLoadLogUpdate(DOWNLOAD_STATS, 0, Bytes, seconds, window, FileName, Header);

                if (DirDir != null && DirDir.Length > 0)
                {
                    int fn = DirDir.IndexOf(FileName);
                    if (fn >= 0)
                    {
                        int CR;
                        CR = DirDir.IndexOf("\r", fn);
                        string s1 = DirDir.Substring(0, CR);

                        //string s1 = DirDir.Substring(0, fn + FileName.Length);
                        s1 += " " + DirDirStats;
                        s1 += DirDir.Substring(CR);
                        DirDir = s1;

                        textBoxWavesRecover.Text = FileLocation + DirDir;

                        textBoxWavesRecover.SelectionStart = FileLocation.Length + CR;// textBoxDownloadLog.Text.Length;// fn;// FileLocation.Length;
                        textBoxWavesRecover.ScrollToCaret();
                        Application.DoEvents();
                    }
                    
                }
            }
        }
     
        private void buttonEnsembleDownload_Click(object sender, EventArgs e)
        {
            DisableButtons();
            ClearTextScreen = true;//txtSerial.Clear();

            int FileNum = 0;
            int bytes = 1000;

            if (UsingSerial)
            {   
                //if (_serialPort.BaudRate == 921600)
                //    bytes = 16384;
                //else
                    bytes = 1024;
            }
            string DirectoryName = @"c:\RoweTechRiverTools_Download_Data";
            for (int i = 0; i < 10000; i++)
            {
                FileNum++;
                string Tstr = "A" + FileNum.ToString("0000000") + ".ens";
                //string Tstr = "B" + FileNum.ToString("0000000") + ".ens";

                txtUserCommand.Text = Tstr;
                
                XModemDownload(DirectoryName, Tstr, bytes, DOWNLOAD_SERIAL);
                

                WriteMessageTxtSerial(Tstr + " ", false);

                if (!GotEOT || XmodemCancelled)
                    break;
            }
            EnableButtons(); 
        }

        //Extract----------------------------------------------------------
        bool[] FirstSeries = new bool[csubs];
        string[] dtstrSeries = new string[csubs];
        string[] snSeries = new string[csubs];
        string[] DirNameSeries = new string[csubs];
        string[] FilNameSeries = new string[csubs];
        void ExtractSeries(ArrayClass m, bool BTonly,bool freshGGA, bool gotPTIC)
        {
            string s = "";
            int cs = (int)(m.E_CurrentSystem >> 24);
            bool gotone = false;

            if (!BTonly)
            { 
                if (m.VelocityAvailable)
                    gotone = true;
                if (m.InstrumentAvailable)
                    gotone = true;
                if (m.EarthAvailable)
                    gotone = true;
                if (m.AmplitudeAvailable)
                    gotone = true;
                if (m.CorrelationAvailable)
                    gotone = true;
                if (m.BeamNAvailable)
                    gotone = true;
                if (m.XfrmNAvailable)
                    gotone = true;
            }


            if (FirstSeries[cs])
            {
                dtstrSeries[cs] = m.E_Year.ToString("D04") + m.E_Month.ToString("D02") + m.E_Day.ToString("D02")
                            + m.E_Hour.ToString("D02") + m.E_Minute.ToString("D02") + m.E_Second.ToString("D02")
                            + m.E_Hsec.ToString("D02");

                snSeries[cs] = System.Text.ASCIIEncoding.ASCII.GetString(m.E_SN_Buffer, 0, 32);

                DirNameSeries[cs] = "c:" + "\\RoweTechRiverTools_Extract" + "\\" + snSeries[cs];

                FilNameSeries[cs] = dtstrSeries[cs] + "_Series" + cs.ToString();
                
                s += "Ensemble Number,VTG(m/s),GGADMG(m),DMGBT(m),Bins,Beams,Pings, Pings Averaged,Status,Year,Month,Day,Hour,Minute,Second,Hsec,SN,Firmware Vers,Sub System,";

                if (m.AncillaryAvailable)
                {
                    s += "WP Bin 1 Range (m),WP Bin Size (m),WP First Ping (s),WP Last Ping (s),WP Heading (deg),WP Pitch (deg),WP Roll (deg),";
                    s += "WP Water Temperature (deg c),WP Internal Temperature (deg C),";
                    s += "WP Salinity (ppt),WP Pressure (P),WP Transducer Depth (m),WP Speed of Sound (m/s),";
                }

                if (m.RTonWPAvailable)
                {
                    switch ((int)m.RTonWP_Beams)
                    {
                        case 1:
                            s += "WPRT Range (m),WPRT SNR (dB),WPRT Pings,";
                            break;
                        default:
                        case 4:
                            s += "WPRT Range bm0 (m),WPRT Range bm1 (m),WPRT Range bm2 (m),WPRT Range bm3 (m),";
                            s += "WPRT SNR bm0 (db),WPRT SNR bm1 (db),WPRT SNR bm2 (db),WPRT SNR bm3 (db),";
                            s += "WPRT Pings bm0,WPRT Pings bm1,WPRT Pings bm2,WPRT Pings bm3,";
                            s += "WPRT Amp bm0,WPRT Amp bm1,WPRT Amp bm2,WPRT Amp bm3,";
                            s += "WPRT Cor bm0,WPRT Cor bm1,WPRT Cor bm2,WPRT Cor bm3,";
                            s += "WPRT Vel bm0,WPRT Vel bm1,WPRT Vel bm2,WPRT Vel bm3,";
                            s += "WPRT Amp Ins,WPRT Ins bm1,WPRT Ins bm2,WPRT Ins bm3,";
                            s += "WPRT Amp Earth,WPRT Earth bm1,WPRT Earth bm2,WPRT Earth bm3,";
                            break;
                    }
                }
                if (m.BottomTrackAvailable)
                {
                    s += "BT First Ping (s),BT Last Ping (s),BT Heading (deg), BT Pitch (deg),BT Roll (deg), BT Water Temperature (deg C), BT Internal Temperature (deg C),";
                    s += "BT Salinity (ppt), BT Pressure (P),BT Transducer Depth (m), BT Speed of Sound (m/s), BT Status (hex),";
                    s += "BT Beams, BT Pings,";
                    if (m.B_Beams == 1)
                    {
                        s += "BT Range bm0 (m),";
                        s += "BT SNR bm0 (dB),";
                        s += "BT Amplitude bm0 (dB),";
                        s += "BT Correlation bm0,";
                        s += "BT Velocity bm0 (m/s),";
                        s += "BT Pings Good bm0,";
                        s += "BT Velocity Z (m/s),";
                        s += "BT Pings Good Z,";
                        s += "BT Velocity Up (m/s),";
                        s += "BT Pings Good Up,";
                    }
                    else
                    {
                        s += "BT Range bm0 (m),BT Range bm1 (m),BT Range bm2 (m),BT Range bm3 (m),";
                        s += "BT SNR bm0 (dB),BT SNR bm1 (dB),BT SNR bm2 (dB),BT SNR bm3 (dB),";
                        s += "BT Amplitude bm0 (dB),BT Amplitude bm1 (dB),BT Amplitude bm2 (dB),BT Amplitude bm3 (dB),";
                        s += "BT Correlation bm0,BT Correlation bm1,BT Correlation bm2,BT Correlation bm3,";
                        s += "BT Velocity bm0 (m/s),BT Velocity bm1 (m/s),BT Velocity bm2 (m/s),BT Velocity bm3 (m/s),";
                        s += "BT Pings Good bm0,BT Pings Good bm1,BT Pings Good bm2,BT Pings Good bm3,";
                        s += "BT Velocity X (m/s),BT Velocity Y (m/s),BT Velocity Z (m/s),BT Velocity Q (m/s),";
                        s += "BT Pings Good X,BT Pings Good Y,BT Pings Good Z,BT Pings Good Q,";
                        s += "BT Velocity East (m/s),BT Velocity North (m/s),BT Velocity Up (m/s),BT Velocity Q (m/s),";
                        s += "BT Pings Good East,BT Pings Good North,BT Pings Good Up,BT Pings Good Q,";

                        s += "BTs SNR bm0 (dB),BTs SNR bm1 (dB),BTs SNR bm2 (dB),BTs SNR bm3 (dB),";
                        s += "BTs Amplitude bm0 (dB),BTs Amplitude bm1 (dB),BTs Amplitude bm2 (dB),BTs Amplitude bm3 (dB),";
                        s += "BTs Noise bm0 (dB),BTs Noise bm1 (dB),BTs Noise bm2 (dB),BTs Noise bm3 (dB),";
                        s += "BTs Correlation bm0,BTs Correlation bm1,BTs Correlation bm2,BTs Correlation bm3,";
                        s += "BTs Velocity bm0 (m/s),BTs Velocity bm1 (m/s),BTs Velocity bm2 (m/s),BTs Velocity bm3 (m/s),";
                    }
                }
                if (gotone)
                {
                    for (int n = 0; n < m.E_Cells; n++)
                    {
                        if (m.E_Beams == 1)
                        {
                            s += "Bin,Depth,";
                            if (m.VelocityAvailable)
                                s += "V0,";
                            if (m.InstrumentAvailable)
                                s += "Z,";
                            if (m.EarthAvailable)
                                s += "Up,,";
                            if (m.AmplitudeAvailable)
                                s += "A0,";
                            if (m.CorrelationAvailable)
                                s += "C0,";
                            if (m.BeamNAvailable)
                                s += "NG0,";
                            if (m.XfrmNAvailable)
                                s += "XG0,";
                        }
                        else
                        {
                            s += "Bin,Depth,";
                            if (m.VelocityAvailable)
                                s += "V0,V1,V2,V3,";
                            if (m.InstrumentAvailable)
                                s += "X,Y,Z,Q,";
                            if (m.EarthAvailable)
                                s += "East,North,Up,Q,";
                            if (m.AmplitudeAvailable)
                                s += "A0,A1,A2,A3,";
                            if (m.CorrelationAvailable)
                                s += "C0,C1,C2,C3,";
                            if (m.BeamNAvailable)
                                s += "NG0,NG1,NG2,NG3,";
                            if (m.XfrmNAvailable)
                                s += "XG0,XG1,XG2,XG3,";
                        }
                    }
                }
            }
            s += "\r\n";
            //Ensemble data
            s += m.E_EnsembleNumber.ToString();

            s += "," + (VTG[cs].SpeedKPH * 1000 / 3600).ToString("00.000");
            if (freshGGA)
                s += "," + GGANAV[cs].DMG.ToString("0.000");
            else
                s += ",";
            s += "," + BTdisMag[cs].ToString("0.000");
            
            s += "," + m.E_Cells.ToString();
            s += "," + m.E_Beams.ToString();
            s += "," + m.E_PingsInEnsemble.ToString();
            s += "," + m.E_PingCount.ToString();
            s += "," + m.E_Status.ToString();
            s += "," + m.E_Year.ToString();
            s += "," + m.E_Month.ToString();
            s += "," + m.E_Day.ToString();
            s += "," + m.E_Hour.ToString();
            s += "," + m.E_Minute.ToString();
            s += "," + m.E_Second.ToString();
            s += "," + m.E_Hsec.ToString();
            s += "," + System.Text.ASCIIEncoding.ASCII.GetString(m.E_SN_Buffer, 0, 32);
            s += "," + m.E_FW_Vers[2].ToString() + "." + m.E_FW_Vers[1].ToString() + "." + m.E_FW_Vers[0].ToString();            
            s += "," + cs.ToString();
            if (m.AncillaryAvailable)
            {
                s += "," + m.A_FirstCellDepth.ToString();
                s += "," + m.A_CellSize.ToString();
                s += "," + m.A_FirstPingSeconds.ToString();
                s += "," + m.A_LastPingSeconds.ToString();
                s += "," + m.A_Heading.ToString();
                s += "," + m.A_Pitch.ToString();
                s += "," + m.A_Roll.ToString();
                s += "," + m.A_WaterTemperature.ToString();
                s += "," + m.A_BoardTemperature.ToString();
                s += "," + m.A_Salinity.ToString();
                s += "," + m.A_Pressure.ToString();
                s += "," + m.A_Depth.ToString();
                s += "," + m.A_SpeedOfSound.ToString();
            }
            
            if (m.RTonWPAvailable)
            {
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Range[i].ToString("");
                }
                
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_SNR[i].ToString("");
                }
               
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Pings[i].ToString("");
                }
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Amp[i].ToString("");
                }
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Cor[i].ToString("");
                }
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Vel[i].ToString("");
                }
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Ins[i].ToString("");
                }
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Earth[i].ToString("");
                }
            }
            
            if (m.BottomTrackAvailable)
            {
                s += "," + m.B_FirstPingSeconds.ToString();
                s += "," + m.B_LastPingSeconds.ToString();
                s += "," + m.B_Heading.ToString();
                s += "," + m.B_Pitch.ToString();
                s += "," + m.B_Roll.ToString();
                s += "," + m.B_WaterTemperature.ToString();
                s += "," + m.B_BoardTemperature.ToString();
                s += "," + m.B_Salinity.ToString();
                s += "," + m.B_Pressure.ToString();
                s += "," + m.B_Depth.ToString();
                s += "," + m.B_SpeedOfSound.ToString();
                s += "," + m.B_Status.ToString();
                s += "," + m.B_Beams.ToString();
                s += "," + m.B_PingCount.ToString();
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Range[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_SNR[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Amplitude[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Correlation[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Velocity[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_BeamN[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Instrument[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_XfrmN[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Earth[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_EarthN[n].ToString();
                }

                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_SNRs[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_AmpS[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_NoiseS[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_CorS[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_VelS[n].ToString();
                }
            }
            if (gotone)
            {
                float Depth;
                float FirstBin = m.A_FirstCellDepth;
                float BinSize = m.A_CellSize;
                long Bins = m.E_Cells;
                if (Bins > 300)
                    Bins = 300;

                for (long Bin = 0; Bin < Bins; Bin++)
                {
                    s += "," + Bin.ToString();
                    Depth = FirstBin + Bin * BinSize;
                    s += "," + Depth.ToString();

                    /*
                     
                                s += "V0,V1,V2,V3,";
                            
                                s += "X,Y,Z,Q,";
                            
                                s += "East,North,Up,Q,";
                            if (m.AmplitudeAvailable)
                                s += "A0,A1,A2,A3,";
                            
                                s += "C0,C1,C2,C3,";
                            
                                s += "NG0,NG1,NG2,NG3,";
                            
                                s += "XG0,XG1,XG2,XG3,";
                     */
                    if (m.VelocityAvailable)
                    {
                        for (int n = 0; n < m.E_Beams; n++)
                        {
                            s += "," + m.Velocity[n, Bin].ToString();
                        }
                    }
                    if (m.InstrumentAvailable)
                    {
                        for (int n = 0; n < m.E_Beams; n++)
                        {
                            s += "," + m.Instrument[n, Bin].ToString();
                        }
                    }
                    if (m.EarthAvailable)
                    {
                        for (int n = 0; n < m.E_Beams; n++)
                        {
                            s += "," + m.Earth[n, Bin].ToString();
                        }
                    }
                    if (m.AmplitudeAvailable)
                    {
                        for (int n = 0; n < m.E_Beams; n++)
                        {
                            s += "," + m.Amplitude[n, Bin].ToString();
                        }
                    }
                    if (m.CorrelationAvailable)
                    {
                        for (int n = 0; n < m.E_Beams; n++)
                        {
                            s += "," + m.Correlation[n, Bin].ToString();
                        }
                    }
                    if (m.BeamNAvailable)
                    {
                        for (int n = 0; n < m.E_Beams; n++)
                        {
                            s += "," + m.BeamN[n, Bin].ToString();
                        }
                    }
                    if (m.XfrmNAvailable)
                    {
                        for (int n = 0; n < m.E_Beams; n++)
                        {
                            s += "," + m.XfrmN[n, Bin].ToString();
                        }
                    }
                }
            }
            
            if(gotPTIC)
            {
                if (textBoxCapturedNMEA.Text != "")
                {
                    string str1 = textBoxCapturedNMEA.Text;
                    s += ",";
                    int j1 = str1.IndexOf(":WI");
                    int k1 = str1.IndexOf("\r");
                    if(k1-j1 == 33)
                        s += str1.Substring(j1,k1-j1);
                    s += ",";
                    j1 = str1.IndexOf(":WD");
                    k1 = str1.IndexOf("\r",j1);
                    if (k1 - j1 == 57)
                        s += str1.Substring(j1, k1 - j1);
                    s += ",";
                    j1 = str1.IndexOf(":BI");
                    k1 = str1.IndexOf("\r", j1);
                    if (k1 - j1 == 33)
                        s += str1.Substring(j1, k1 - j1);
                    s += ",";
                    j1 = str1.IndexOf(":BD");
                    k1 = str1.IndexOf("\r", j1);
                    if (k1 - j1 == 57)
                        s += str1.Substring(j1, k1 - j1);
                }
            }
            /*
            if (m.NmeaAvailable)
            {
                s += ",";
                int ne = 199;
                for (int i = 0; i < 8192; i++)
                {
                    ne++;
                    char c = (char)m.NMEA_Buffer[i];
                    if (c == (char)0)
                        break;
                    if (c == '*')
                    {
                        s += ",";
                        ne = 0;
                    }
                    s += c;

                    if (ne == 2)//checksum has been read in
                    {
                        s += ",";
                        ne = 199;
                    }
                }
            }
            */
            //write the file
            DirectoryInfo di = new DirectoryInfo(DirNameSeries[cs]);
            try
            {
                if (di.Exists)// Determine whether the directory exists.
                {
                    //di.Delete();// Delete the directory.
                }
                else// Try to create the directory.
                {
                    di.Create();
                }
            }
            catch (System.Exception ex)
            {
                textBoxExtract.Text += "Can't save txt! " + ex.ToString() + "\r\n";
            }
            finally
            {
                string str = DirNameSeries[cs] + "\\" + FilNameSeries[cs] + ".csv";
                if (FirstSeries[cs])
                    textBoxExtract.Text += "Destination File: \r\n" + str + "\r\n";
                try
                {
                    if (FirstSeries[cs])
                    {
                        System.IO.File.WriteAllText(str, s);
                        FirstSeries[cs] = false;
                    }
                    else
                        System.IO.File.AppendAllText(str, s);
                }
                catch (System.Exception ex)
                {
                    textBoxExtract.Text += "Can't save text! " + ex.ToString() + "\r\n";
                }
            }
        }
        void ExtractNavSeries(ArrayClass m)
        {
            string s = "";
            int cs = (int)(m.E_CurrentSystem >> 24);
            
            if (FirstSeries[cs])
            {
                dtstrSeries[cs] = m.E_Year.ToString("D04") + m.E_Month.ToString("D02") + m.E_Day.ToString("D02")
                            + m.E_Hour.ToString("D02") + m.E_Minute.ToString("D02") + m.E_Second.ToString("D02")
                            + m.E_Hsec.ToString("D02");

                snSeries[cs] = System.Text.ASCIIEncoding.ASCII.GetString(m.E_SN_Buffer, 0, 32);

                DirNameSeries[cs] = "c:" + "\\RoweTechRiverTools_NavExtract" + "\\" + snSeries[cs];

                FilNameSeries[cs] = dtstrSeries[cs] + "_Series" + cs.ToString();

                s += "Ensemble Number,Status,Year,Month,Day,Hour,Minute,Second,Hsec,SN,Firmware Vers,Sub System,";
                
                if (m.BottomTrackAvailable)
                {
                    s += "BT First Ping (s),BT Last Ping (s),BT Heading (deg), BT Pitch (deg),BT Roll (deg), BT Water Temperature (deg C), BT Internal Temperature (deg C),";
                    s += "BT Salinity (ppt), BT Pressure (P),BT Transducer Depth (m), BT Speed of Sound (m/s), BT Status (hex),";
                    s += "BT Beams, BT Pings,";
                    if (m.B_Beams == 1)
                    {
                        s += "BT Range bm0 (m),";
                        s += "BT SNR bm0 (dB),";
                        s += "BT Amplitude bm0 (dB),";
                        s += "BT Correlation bm0,";
                        s += "BT Velocity bm0 (m/s),";
                        s += "BT Pings Good bm0,";
                        s += "BT Velocity Z (m/s),";
                        s += "BT Pings Good Z,";
                        s += "BT Velocity Up (m/s),";
                        s += "BT Pings Good Up,";
                    }
                    else
                    {
                        s += "BT Range bm0 (m),BT Range bm1 (m),BT Range bm2 (m),BT Range bm3 (m),";
                        s += "BT SNR bm0 (dB),BT SNR bm1 (dB),BT SNR bm2 (dB),BT SNR bm3 (dB),";
                        s += "BT Amplitude bm0 (dB),BT Amplitude bm1 (dB),BT Amplitude bm2 (dB),BT Amplitude bm3 (dB),";
                        s += "BT Correlation bm0,BT Correlation bm1,BT Correlation bm2,BT Correlation bm3,";
                        s += "BT Velocity bm0 (m/s),BT Velocity bm1 (m/s),BT Velocity bm2 (m/s),BT Velocity bm3 (m/s),";
                        s += "BT Pings Good bm0,BT Pings Good bm1,BT Pings Good bm2,BT Pings Good bm3,";
                        s += "BT Velocity X (m/s),BT Velocity Y (m/s),BT Velocity Z (m/s),BT Velocity Q (m/s),";
                        s += "BT Pings Good X,BT Pings Good Y,BT Pings Good Z,BT Pings Good Q,";
                        s += "BT Velocity East (m/s),BT Velocity North (m/s),BT Velocity Up (m/s),BT Velocity Q (m/s),";
                        s += "BT Pings Good East,BT Pings Good North,BT Pings Good Up,BT Pings Good Q,";
                        
                    }
                }
                if (m.NmeaAvailable)
                    s += "VTG,";
            }
            s += "\r\n";
            //Ensemble data
            s += m.E_EnsembleNumber.ToString();
            s += "," + m.E_Status.ToString();
            s += "," + m.E_Year.ToString();
            s += "," + m.E_Month.ToString();
            s += "," + m.E_Day.ToString();
            s += "," + m.E_Hour.ToString();
            s += "," + m.E_Minute.ToString();
            s += "," + m.E_Second.ToString();
            s += "," + m.E_Hsec.ToString();
            s += "," + System.Text.ASCIIEncoding.ASCII.GetString(m.E_SN_Buffer, 0, 32);
            s += "," + m.E_FW_Vers[2].ToString() + "." + m.E_FW_Vers[1].ToString() + "." + m.E_FW_Vers[0].ToString();
            s += "," + cs.ToString();
            if (m.BottomTrackAvailable)
            {
                s += "," + m.B_FirstPingSeconds.ToString();
                s += "," + m.B_LastPingSeconds.ToString();
                s += "," + m.B_Heading.ToString();
                s += "," + m.B_Pitch.ToString();
                s += "," + m.B_Roll.ToString();
                s += "," + m.B_WaterTemperature.ToString();
                s += "," + m.B_BoardTemperature.ToString();
                s += "," + m.B_Salinity.ToString();
                s += "," + m.B_Pressure.ToString();
                s += "," + m.B_Depth.ToString();
                s += "," + m.B_SpeedOfSound.ToString();
                s += "," + m.B_Status.ToString();
                s += "," + m.B_Beams.ToString();
                s += "," + m.B_PingCount.ToString();
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Range[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_SNR[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Amplitude[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Correlation[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Velocity[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_BeamN[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Instrument[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_XfrmN[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_Earth[n].ToString();
                }
                for (int n = 0; n < m.B_Beams; n++)
                {
                    s += "," + m.B_EarthN[n].ToString();
                }
            }
            
            if (m.NmeaAvailable)
            {
                double Speed = GetEnsembleNmeaVTG(Arr);

                s += "," + Speed.ToString();// +"\r\n";
                //VTG[0].SpeedKnots


                /*s += ",";
                int ne = 199;
                for (int i = 0; i < 8192; i++)
                {
                    ne++;
                    char c = (char)m.NMEA_Buffer[i];
                    if (c == (char)0)
                        break;
                    if (c == '*')
                    {
                        s += ",";
                        ne = 0;
                    }
                    s += c;

                    if (ne == 2)//checksum has been read in
                    {
                        s += ",";
                        ne = 199;
                    }
                }*/
            }
            //else
            //    s += "\r\n";
            
            //write the file
            DirectoryInfo di = new DirectoryInfo(DirNameSeries[cs]);
            try
            {
                if (di.Exists)// Determine whether the directory exists.
                {
                    //di.Delete();// Delete the directory.
                }
                else// Try to create the directory.
                {
                    di.Create();
                }
            }
            catch (System.Exception ex)
            {
                textBoxExtract.Text += "Can't save txt! " + ex.ToString() + "\r\n";
            }
            finally
            {
                string str = DirNameSeries[cs] + "\\" + FilNameSeries[cs] + ".csv";
                if (FirstSeries[cs])
                    textBoxExtract.Text += "Destination File: \r\n" + str + "\r\n";
                try
                {
                    if (FirstSeries[cs])
                    {
                        System.IO.File.WriteAllText(str, s);
                        FirstSeries[cs] = false;
                    }
                    else
                        System.IO.File.AppendAllText(str, s);
                }
                catch (System.Exception ex)
                {
                    textBoxExtract.Text += "Can't save text! " + ex.ToString() + "\r\n";
                }
            }
        }
        private void buttonExtractSeries_Click(object sender, EventArgs e)
        {
            PlaybackPaused = false;

            int i;
            for (i = 0; i < csubs; i++)
                FirstSeries[i] = true;

           

            long nBytesRead;
            long nBytes;
            Stream stream;// = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            PlaybackDisableButtons();

            tmrTerminal.Enabled = false;

            DecodeState = 0;
            DataBuffReadIndex = 0;
            DataBuffWriteIndex = 0;
            NmeaBuffReadIndex = 0;

            BackScatter.DataBuffWriteIndex = 0;
            BackScatter.DataBuffReadIndex = 0;

            openFileDialog1.InitialDirectory = "";
            //openFileDialog1.Filter = "rti files (*.rti)|*.rti|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.Filter = "ens files (*.ens)|*.ens|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog1.OpenFile()) != null)
                    {
                        try
                        {
                            nBytes = stream.Length;
                            textBoxExtract.Text = "Source File: " + nBytes.ToString() + " bytes\r\n" + openFileDialog1.FileName + "\r\n";

                            nBytesRead = stream.Read(bBuff, 0, 10000);
                            while (nBytesRead > 0 && PlaybackEnabled)
                            {
                                for (i = 0; i < nBytesRead; i++)
                                {
                                    DataBuff[DataBuffWriteIndex] = bBuff[i];
                                    DataBuffWriteIndex++;
                                    if (DataBuffWriteIndex > MaxDataBuff)
                                        DataBuffWriteIndex = 0;
                                    BackScatter.DataBuffWriteIndex = DataBuffWriteIndex;
                                }

                                int DBRI = DataBuffWriteIndex;

                                while (DBRI != DataBuffReadIndex)
                                {
                                    DBRI = DataBuffReadIndex;
                                    DecodeData(false, false, true);
                                    
                                    //System.Threading.Thread.Sleep(100);
                                    //Application.DoEvents();
                                }
                                textBoxExtract.Text += ".";
                                nBytesRead = stream.Read(bBuff, 0, 10000);
                                Application.DoEvents();
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("caughtF2: {0}", ex.GetType().ToString());
                            MessageBox.Show(exceptionmessage);
                        }
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("caughtG: {0}", ex.GetType().ToString());
                    MessageBox.Show(exceptionmessage);
                }
            }
            PlaybackEnableButtons();
        }
        void ExtractProfile(ArrayClass m, bool ShowName)
        {
            string s = "";

            //profile data
            bool gotone = false;

            if (m.VelocityAvailable)
                gotone = true;
            if (m.InstrumentAvailable)
                gotone = true;
            if (m.EarthAvailable)
                gotone = true;
            if (m.AmplitudeAvailable)
                gotone = true;
            if (m.CorrelationAvailable)
                gotone = true;
            if (m.BeamNAvailable)
                gotone = true;
            if (m.XfrmNAvailable)
                gotone = true;

            if (gotone)
            {
                s += "Profile Data:";

                s += ",,";
                if (m.VelocityAvailable)
                {
                    s += "E000001 Beam Velocity (m/s)";
                    for (int beam = 0; beam < m.E_Beams; beam++)
                        s += ",";
                }
                if (m.InstrumentAvailable)
                {
                    s += "E000002 Instrument Velocity (m/s)";
                    for (int beam = 0; beam < m.E_Beams; beam++)
                        s += ",";
                }
                if (m.EarthAvailable)
                {
                    s += "E000003 Earth Velocity (m/s)";
                    for (int beam = 0; beam < m.E_Beams; beam++)
                        s += ",";
                }
                if (m.AmplitudeAvailable)
                {
                    s += "E000004 Beam Amplitude (dB)";
                    for (int beam = 0; beam < m.E_Beams; beam++)
                        s += ",";
                }
                if (m.CorrelationAvailable)
                {
                    s += "E000005 Beam Correlation";
                    for (int beam = 0; beam < m.E_Beams; beam++)
                        s += ",";
                }
                if (m.BeamNAvailable)
                {
                    s += "E000006 Beam Pings Good";
                    for (int beam = 0; beam < m.E_Beams; beam++)
                        s += ",";
                }
                if (m.XfrmNAvailable)
                {
                    s += "E000007 Earth Pings Good";
                    for (int beam = 0; beam < m.E_Beams; beam++)
                        s += ",";
                }

                s += "\r\n";

                s += "Bin,Depth,";
                if (m.VelocityAvailable)
                {
                    for (int beam = 0; beam < m.E_Beams; beam++)
                    {
                        switch (beam)
                        {
                            default:
                            case 0:
                                s += "V0,";
                                break;
                            case 1:
                                s += "V1,";
                                break;
                            case 2:
                                s += "V2,";
                                break;
                            case 3:
                                s += "V3,";
                                break;
                        }
                    }
                }
                if (m.InstrumentAvailable)
                {
                    for (int beam = 0; beam < m.E_Beams; beam++)
                    {
                        switch (beam)
                        {
                            default:
                            case 0:
                                s += "X,";
                                break;
                            case 1:
                                s += "Y,";
                                break;
                            case 2:
                                s += "Z,";
                                break;
                            case 3:
                                s += "Q,";
                                break;
                        }
                    }
                }
                if (m.EarthAvailable)
                {
                    for (int beam = 0; beam < m.E_Beams; beam++)
                    {
                        switch (beam)
                        {
                            default:
                            case 0:
                                s += "East,";
                                break;
                            case 1:
                                s += "North,";
                                break;
                            case 2:
                                s += "Up,";
                                break;
                            case 3:
                                s += "Q,";
                                break;
                        }
                    }
                }
                if (m.AmplitudeAvailable)
                {
                    for (int beam = 0; beam < m.E_Beams; beam++)
                    {
                        switch (beam)
                        {
                            default:
                            case 0:
                                s += "A0,";
                                break;
                            case 1:
                                s += "A1,";
                                break;
                            case 2:
                                s += "A2,";
                                break;
                            case 3:
                                s += "A3,";
                                break;
                        }
                    }
                }
                if (m.CorrelationAvailable)
                {
                    for (int beam = 0; beam < m.E_Beams; beam++)
                    {
                        switch (beam)
                        {
                            default:
                            case 0:
                                s += "C0,";
                                break;
                            case 1:
                                s += "C1,";
                                break;
                            case 2:
                                s += "C2,";
                                break;
                            case 3:
                                s += "C3,";
                                break;
                        }
                    }
                }
                if (m.BeamNAvailable)
                {
                    for (int beam = 0; beam < m.E_Beams; beam++)
                    {
                        switch (beam)
                        {
                            default:
                            case 0:
                                s += "NG0,";
                                break;
                            case 1:
                                s += "NG1,";
                                break;
                            case 2:
                                s += "NG2,";
                                break;
                            case 3:
                                s += "NG3,";
                                break;
                        }
                    }
                }
                if (m.XfrmNAvailable)
                {
                    for (int beam = 0; beam < m.E_Beams; beam++)
                    {
                        switch (beam)
                        {
                            default:
                            case 0:
                                s += "XG0,";
                                break;
                            case 1:
                                s += "XG1,";
                                break;
                            case 2:
                                s += "XG2,";
                                break;
                            case 3:
                                s += "XG3,";
                                break;
                        }
                    }
                }

                s += "\r\n";

                float Depth;
                float FirstBin = m.A_FirstCellDepth;
                float BinSize = m.A_CellSize;
                long Bins = m.E_Cells;
                if (Bins > 200)
                    Bins = 200;
                for (long Bin = 0; Bin < Bins; Bin++)
                {
                    s += Bin.ToString();
                    Depth = FirstBin + Bin * BinSize;
                    s += "," + Depth.ToString();
                    if (m.VelocityAvailable)
                    {
                        for(int beam=0;beam<m.E_Beams;beam++)
                            s += "," + m.Velocity[beam, Bin].ToString();
                    }
                    if (m.InstrumentAvailable)
                    {
                        for (int beam = 0; beam < m.E_Beams; beam++)
                            s += "," + m.Instrument[beam, Bin].ToString();
                    }
                    if (m.EarthAvailable)
                    {
                        for (int beam = 0; beam < m.E_Beams; beam++)
                            s += "," + m.Earth[beam, Bin].ToString();
                    }
                    if (m.AmplitudeAvailable)
                    {
                        for (int beam = 0; beam < m.E_Beams; beam++)
                            s += "," + m.Amplitude[beam, Bin].ToString();
                    }
                    if (m.CorrelationAvailable)
                    {
                        for (int beam = 0; beam < m.E_Beams; beam++)
                            s += "," + m.Correlation[beam, Bin].ToString();
                    }
                    if (m.BeamNAvailable)
                    {
                        for (int beam = 0; beam < m.E_Beams; beam++)
                            s += "," + m.BeamN[beam, Bin].ToString();
                    }
                    if (m.XfrmNAvailable)
                    {
                        for (int beam = 0; beam < m.E_Beams; beam++)
                            s += "," + m.XfrmN[beam, Bin].ToString();
                    }
                    s += "\r\n";
                }
            }
            s += "Ensemble Data: E000008\r\n";
            s += "Ensemble Number," + m.E_EnsembleNumber.ToString() + "\r\n";
            s += "Bins," + m.E_Cells.ToString() + "\r\n";
            s += "Beams," + m.E_Beams.ToString() + "\r\n";
            s += "Pings," + m.E_PingsInEnsemble.ToString() + "\r\n";
            s += "Ping Averaged," + m.E_PingCount.ToString() + "\r\n";
            s += "Status (hex)," + ((int)(m.E_Status)).ToString("X04") + "\r\n";
            s += "Year,Month,Day,Hour,Minute,Second,Hsec\r\n";
            s += m.E_Year.ToString() + ",";
            s += m.E_Month.ToString() + ",";
            s += m.E_Day.ToString() + ",";
            s += m.E_Hour.ToString() + ",";
            s += m.E_Minute.ToString() + ",";
            s += m.E_Second.ToString() + ",";
            s += m.E_Hsec.ToString() + "\r\n";

            s += "SN," + System.Text.ASCIIEncoding.ASCII.GetString(m.E_SN_Buffer, 0, 32);
            s += "Firmware," + m.E_FW_Vers[2].ToString() + ",";
            s += m.E_FW_Vers[1].ToString() + ",";
            s += m.E_FW_Vers[0].ToString() + "\r\n";
            int cs = (int)(m.E_CurrentSystem >> 24);
            s += "Sub System," + cs.ToString();
            s += "\r\n";

            if (m.AncillaryAvailable)
            {
                s += "Ancillary Data: E000009\r\n";
                s += "First Bin (m)," + m.A_FirstCellDepth.ToString() + "\r\n";
                s += "Bin Size (m)," + m.A_CellSize.ToString() + "\r\n";
                s += "First Ping (s)," + m.A_FirstPingSeconds.ToString() + "\r\n";
                s += "Last Ping (s)," + m.A_LastPingSeconds.ToString() + "\r\n";
                s += "Heading (deg)," + m.A_Heading.ToString() + "\r\n";
                s += "Pitch (deg)," + m.A_Pitch.ToString() + "\r\n";
                s += "Roll (deg)," + m.A_Roll.ToString() + "\r\n";
                s += "Water Temperature(deg C)," + m.A_WaterTemperature.ToString() + "\r\n";
                s += "Internal Temperature (deg C)," + m.A_BoardTemperature.ToString() + "\r\n";
                s += "Salinity (ppt)," + m.A_Salinity.ToString() + "\r\n";
                s += "Pressure (P)," + m.A_Pressure.ToString() + "\r\n";
                s += "Transducer Depth (m)," + m.A_Depth.ToString() + "\r\n";
                s += "Speed of Sound (m/s)," + m.A_SpeedOfSound.ToString() + "\r\n";
            }

            if (m.RTonWPAvailable)
            {
                s += "WPRT Range";
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Range[i].ToString("");
                }
                s += "\r\nWPRT SNR";
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_SNR[i].ToString("");
                }
                s += "\r\nWPRT Pings ";
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Pings[i].ToString("");
                }
                s += "\r\nWPRT Amp   ";
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Amp[i].ToString("");
                }
                s += "\r\nWPRT Cor   ";
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Cor[i].ToString("");
                }
                s += "\r\nWPRT Vel   ";
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Vel[i].ToString("");
                }
                s += "\r\nWPRT Ins   ";
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Ins[i].ToString("");
                }
                s += "\r\nWPRT Earth ";
                for (int i = 0; i < m.RTonWP_Beams; i++)
                {
                    s += "," + m.RTonWP_Earth[i].ToString("");
                }
                s += "\r\n";
            }

            if (m.BottomTrackAvailable)
            {
                s += "Bottom Track Data: E000010\r\n";
                s += "First Ping (s)," + m.B_FirstPingSeconds.ToString() + "\r\n";
                s += "Last Ping (s)," + m.B_LastPingSeconds.ToString() + "\r\n";
                s += "Heading (deg)," + m.B_Heading.ToString() + "\r\n";
                s += "Pitch (deg)," + m.B_Pitch.ToString() + "\r\n";
                s += "Roll (deg)," + m.B_Roll.ToString() + "\r\n";
                s += "Water Temperature (deg C)," + m.B_WaterTemperature.ToString() + "\r\n";
                s += "Internal Temperature (deg C)," + m.B_BoardTemperature.ToString() + "\r\n";
                s += "Salinity (ppt)," + m.B_Salinity.ToString() + "\r\n";
                s += "Pressure (P)," + m.B_Pressure.ToString() + "\r\n";
                s += "Transducer Depth (m)," + m.B_Depth.ToString() + "\r\n";
                s += "Speed of Sound (m/s)," + m.B_SpeedOfSound.ToString() + "\r\n";
                s += "Status (hex)," + ((int)m.B_Status).ToString("X04") + "\r\n";
                s += "Beams," + m.B_Beams.ToString() + "\r\n";
                s += "Pings," + m.B_PingCount.ToString() + "\r\n";
                
                for (int beam = 0; beam < m.E_Beams; beam++)
                {
                    switch (beam)
                    {
                        default:
                        case 0:
                            s += ",bm0";
                            break;
                        case 1:
                            s += ",bm1";
                            break;
                        case 2:
                            s += ",bm2";
                            break;
                        case 3:
                            s += ",bm3";
                            break;
                    }
                }
                s += "\r\n";
                s += "Range to Bottom (m)";
                for (int beam = 0; beam < m.E_Beams; beam++)                
                    s += "," + m.B_Range[beam].ToString();
                s += "\r\n";
                s += "SNR (db)";
                for (int beam = 0; beam < m.E_Beams; beam++)
                    s += "," + m.B_SNR[beam].ToString();
                s += "\r\n";
                s += "Amplitude (db)";
                for (int beam = 0; beam < m.E_Beams; beam++)
                    s += "," + m.B_Amplitude[beam].ToString();
                s += "\r\n";
                s += "Correlation";
                for (int beam = 0; beam < m.E_Beams; beam++)
                    s += "," + m.B_Correlation[beam].ToString();
                s += "\r\n";                
                s += "Velocity (m/s)";
                for (int beam = 0; beam < m.E_Beams; beam++)
                    s += "," + m.B_Velocity[beam].ToString();
                s += "\r\n";
                s += "Pings Good";
                for (int beam = 0; beam < m.E_Beams; beam++)
                    s += "," + m.B_BeamN[beam].ToString();
                s += "\r\n";
                s += "Instrument Velocity (m/s)";
                for (int beam = 0; beam < m.E_Beams; beam++)
                    s += "," + m.B_Instrument[beam].ToString();
                s += "\r\n";
                s += "Instrument Pings Good";
                for (int beam = 0; beam < m.E_Beams; beam++)
                    s += "," + m.B_XfrmN[beam].ToString();
                s += "\r\n";                
                s += "Earth Velocity (m/s)";
                for (int beam = 0; beam < m.E_Beams; beam++)
                    s += "," + m.B_Earth[beam].ToString();
                s += "\r\n";
                s += "Earth Pings Good";
                for (int beam = 0; beam < m.E_Beams; beam++)
                    s += "," + m.B_EarthN[beam].ToString();
                s += "\r\n";
            }
            if (m.NmeaAvailable)
            {
                s += "NMEA Data: E000011\r\n";
                int ne = 99;
                for (int i = 0; i < 8192; i++)
                {
                    ne++;
                    char c = (char)m.NMEA_Buffer[i];
                    if (c == (char)0)
                        break;
                    if (c == '*')
                    {
                        ne = 0;
                        s += ",";
                    }
                    s += c;

                    if (ne == 2)//checksum has been read in
                    {
                        s += "\r\n";
                        ne = 99;
                    }
                }
            }
            if (m.EngProfileDataAvailable)
            {
                s += "Eng Profile Data: E000012\r\n";
            }
            if (m.EngBottomTrackDataAvailable)
            {
                s += "Eng Bottom Track Data: E000013\r\n";
            }
            if (m.SystemSetupDataAvailable)
            {
                s += "System Setup: E000014\r\n";
                s += "Bottom Track:\r\n";
                s += "Samples per Second," + m.SystemSetup_BTSamplesPerSecond.ToString() + "\r\n";
                s += "System Frequency (Hz)," + m.SystemSetup_BTSystemFreqHz.ToString() + "\r\n";
                s += "Cycles per Element," + m.SystemSetup_BTCPCE.ToString() + "\r\n";
                s += "Code Elements," + m.SystemSetup_BTNCE.ToString() + "\r\n";
                s += "Code Repeats," + m.SystemSetup_BTRepeatN.ToString() + "\r\n";
                s += "Profile:\r\n";
                s += "Samples per Second," + m.SystemSetup_WPSamplesPerSecond.ToString() + "\r\n";
                s += "System Frequency (Hz)," + m.SystemSetup_WPSystemFreqHz.ToString() + "\r\n";
                s += "Cycles per Element," + m.SystemSetup_WPCPCE.ToString() + "\r\n";
                s += "Code Elements," + m.SystemSetup_WPNCE.ToString() + "\r\n";
                s += "Code Repeats," + m.SystemSetup_WPRepeatN.ToString() + "\r\n";
                s += "Lag Samples," + m.SystemSetup_WPLagSamples.ToString() + "\r\n";
                s += "Input Voltage (V)," + m.SystemSetup_InputVoltage.ToString() + "\r\n";
                
            }
            //write the file
            string dtstr = m.E_Year.ToString("D04") + m.E_Month.ToString("D02") + m.E_Day.ToString("D02")
                         + m.E_Hour.ToString("D02") + m.E_Minute.ToString("D02") + m.E_Second.ToString("D02")
                         + m.E_Hsec.ToString("D02");

            string sn = System.Text.ASCIIEncoding.ASCII.GetString(m.E_SN_Buffer, 0, 32);

            string DirName = "c:" + "\\RoweTechRiverTools_Extract" + "\\" + sn;
            string FilName = dtstr + "_Profile" + m.E_EnsembleNumber.ToString();

            DirectoryInfo di = new DirectoryInfo(DirName);
            try
            {
                if (di.Exists)// Determine whether the directory exists.
                {
                    //di.Delete();// Delete the directory.
                }
                else// Try to create the directory.
                {
                    di.Create();
                }
            }
            catch (System.Exception ex)
            {
                textBoxExtract.Text += "Can't save txt! " + ex.ToString() + "\r\n";
            }
            finally
            {
                string str = DirName + "\\" + FilName + ".csv";
                if (ShowName)
                    textBoxExtract.Text += "Destination File: \r\n" + str + "\r\n";
                try
                {
                    System.IO.File.WriteAllText(str, s);
                }
                catch (System.Exception ex)
                {
                    textBoxExtract.Text += "Can't save text! " + ex.ToString() + "\r\n";
                }
            }
        }
        private void buttonExtractProfile_Click(object sender, EventArgs e)
        {
            //string dtstr = DateTime.Now.ToString("yyyyMMddHHmmss");
            bool ShowName = true;

            PlaybackDisableButtons();
            tmrTerminal.Enabled = false;

            if (Arr.EnsembleDataAvailable)
                ExtractProfile(Arr, ShowName);
            
            PlaybackEnableButtons();
            tmrTerminal.Enabled = true;
        }
        private void buttonExtractADCP1raw_Click(object sender, EventArgs e)
        {
            PlaybackPaused = false;
            uint val;
            double vald;
            long RnBytes;
            long TnBytes;
            Stream Rstream;// = null;
            Stream Tstream = null;
            OpenFileDialog RopenFileDialog1 = new OpenFileDialog();
            OpenFileDialog TopenFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            PlaybackDisableButtons();
            tmrTerminal.Enabled = false;

            RopenFileDialog1.InitialDirectory = "";
            //openFileDialog1.Filter = "rti files (*.rti)|*.rti|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            RopenFileDialog1.Filter = "ens files (*.ens)|*.ens|txt files (*.txt)|*.txt|All files (*.*)|*.*|raw files (*.raw)||bin files (*.bin)|*.bin";
            RopenFileDialog1.FilterIndex = 3;
            RopenFileDialog1.RestoreDirectory = true;

            if (RopenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((Rstream = RopenFileDialog1.OpenFile()) != null)
                    {
                        try
                        {
                            string namestr = RopenFileDialog1.FileName;
                            string fname = namestr.Substring(namestr.Length - 12, 8);
                            if (fname[0] == 'R')//Transmit buffer
                            {
                                TopenFileDialog1.FileName = namestr.Substring(0, namestr.Length - 12);
                                TopenFileDialog1.FileName += "T" + namestr.Substring(namestr.Length - 11, 11);
                                if ((Tstream = TopenFileDialog1.OpenFile()) != null)
                                {

                                    fname = "T" + fname;
                                    RnBytes = Rstream.Length;

                                    textBoxExtract.Text = "Source File: " + RnBytes.ToString() + " bytes\r\n" + RopenFileDialog1.FileName + "\r\n";

                                    TnBytes = Tstream.Length;
                                    if (TnBytes > MaxDataBuff)
                                        TnBytes = MaxDataBuff;

                                    string DirName = "c:" + "\\RoweTechRiverTools_RawExtract" + "\\";
                                    string FilName = fname;

                                    DirectoryInfo di = new DirectoryInfo(DirName);
                                    try
                                    {
                                        if (di.Exists)// Determine whether the directory exists.
                                        {
                                            //di.Delete();// Delete the directory.
                                        }
                                        else// Try to create the directory.
                                        {
                                            di.Create();
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        textBoxExtract.Text += "Can't save csv! " + ex.ToString() + "\r\n";
                                    }
                                    finally
                                    {
                                        int TnBytesRead = Tstream.Read(aBuff, 0, (int)TnBytes);

                                        string str = DirName + "\\" + FilName + ".csv";
                                        textBoxExtract.Text += "Destination File: \r\n" + str + "\r\n";

                                        try
                                        {
                                            string s = "";
                                            int nBytesRead = Rstream.Read(bBuff, 0, 400);
                                            int i, j, k;
                                            j = 0;
                                            for (i = 0; i < 100; i++)
                                            {
                                                val = (uint)aBuff[j] + 256 * (uint)aBuff[j + 1] + 256 * 256 * (uint)aBuff[j + 2] + 256 * 256 * 256 * (uint)aBuff[j + 3];
                                                s += val.ToString() + ",";
                                                val = (uint)bBuff[j] + 256 * (uint)bBuff[j + 1] + 256 * 256 * (uint)bBuff[j + 2] + 256 * 256 * 256 * (uint)bBuff[j + 3];
                                                s += val.ToString() + "\r\n";
                                                j += 4;
                                            }
                                            System.IO.File.WriteAllText(str, s);

                                            k = 0;
                                            val = (uint)aBuff[j] + 256 * (uint)aBuff[j + 1] + 256 * 256 * (uint)aBuff[j + 2] + 256 * 256 * 256 * (uint)aBuff[j + 3];
                                            s = val.ToString() + ",";

                                            bool TwoBeam = false;
                                            int ByteDiv = 32;
                                            if (aBuff[199] == 2)
                                            {
                                                TwoBeam = true;
                                                ByteDiv = 16;
                                            }

                                            //Rstream.Read(bBuff, 0, 4);//skip one value to line up the data

                                            /*
                                             Raw Data Output Format 
                                                Bit 31=msb=first bit transmitted.

                                                First 32 bit word:
                                                bit 	31	= msb of real part of sample data (even channels)
	                                                bits	30-8	= next 23 bits of real sample data (up to 24 bits per sample)
	                                                bit   	7    	= spare bit set to 0  
	                                                bits   	6-5    	= current gain (11=0dB gain (i.e. minimum gain), 10=18dB, 01=36dB, 00=54dB)
	                                                bits	4-2	= channel number (0 to 7)					
	                                                bit   	1     	= sync bit 
			                                                (sample gate; (trigenable || input trigger || internal trigger) && data ath reset)
	                                                bit	0	= 1=real bit for rawdata
                                                						
                                                Second 32 bit word:
	                                                bit 	31	= msb of imaginary part of sample data (odd channels)
	                                                bits	30-8	= next 23 bits of imaginary sample data (up to 24 bits per sample)
	                                                bits	7-3	= BrdID (lower 5 bits of I2C device address)
	                                                bits	2	= spare bit set to 0
	                                                bit	1	= sampled trigger input (rx_trig) 
	                                                bits	0	= 0=imag bit for rawdata

                                             */
                                            for (i = 0; i < (RnBytes - 400) / ByteDiv; i++)
                                            {
                                                for (int m = 0; m < 4; m++)
                                                {
                                                    Rstream.Read(bBuff, 0, 4);
                                                    vald = (double)((uint)bBuff[2] + 256 * (uint)bBuff[1] + 65536 * (uint)bBuff[0]);
                                                    if (vald > Math.Pow( 2, 23))
                                                        vald -= Math.Pow(2,24);

                                                    s += vald.ToString();
                                                    s += ",";
                                                }
                                                if (!TwoBeam)
                                                {
                                                    for (int m = 0; m < 4; m++)
                                                    {   
                                                        Rstream.Read(bBuff, 0, 4);
                                                        vald = (double)((uint)bBuff[2] + 256 * (uint)bBuff[1] + 65536 * (uint)bBuff[0]);
                                                        if (vald > Math.Pow(2, 23))
                                                            vald = vald - Math.Pow(2, 24);
                                                        s += vald.ToString();
                                                        s += ",";
                                                    }
                                                }
                                                s += "\r\n";
                                                
                                                System.IO.File.AppendAllText(str, s);
                                                k++;
                                                if (k > 99)
                                                {
                                                    k = 0;
                                                    textBoxExtract.Text += ".";
                                                }
                                                Application.DoEvents();
                                                j += 4;
                                                if (j < TnBytesRead)
                                                {
                                                    val = (uint)aBuff[j] + 256 * (uint)aBuff[j + 1] + 256 * 256 * (uint)aBuff[j + 2] + 256 * 256 * 256 * (uint)aBuff[j + 3];
                                                    s = val.ToString() + ",";
                                                }
                                                else
                                                {
                                                    j = TnBytesRead;
                                                    s = ",";
                                                }
                                            }
                                            if (j < TnBytesRead)
                                            {
                                                while (j < TnBytesRead)
                                                {
                                                    Application.DoEvents();
                                                    j += 4;
                                                    if (j < TnBytesRead)
                                                    {
                                                        val = (uint)aBuff[j] + 256 * (uint)aBuff[j + 1] + 256 * 256 * (uint)aBuff[j + 2] + 256 * 256 * 256 * (uint)aBuff[j + 3];
                                                        s = val.ToString() + "\r\n";
                                                        System.IO.File.AppendAllText(str, s);
                                                    }
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            textBoxExtract.Text += "Can't save csv! " + ex.ToString() + "\r\n";
                                        }
                                        textBoxExtract.Text += "\r\ndone";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("caughtAA: {0}", ex.GetType().ToString());
                            MessageBox.Show(exceptionmessage);
                        }
                        Rstream.Close();
                        Tstream.Close();
                    }

                }
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("caughtBB: {0}", ex.GetType().ToString());
                    MessageBox.Show(exceptionmessage);
                }
            }
            PlaybackEnableButtons();
        }
        
        private void buttonExtractADCP0raw_Click(object sender, EventArgs e)
        {
            PlaybackPaused = false;
            uint val;
            long RnBytes;
            long TnBytes;
            Stream Rstream;// = null;
            Stream Tstream = null;
            OpenFileDialog RopenFileDialog1 = new OpenFileDialog();
            OpenFileDialog TopenFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            PlaybackDisableButtons();
            tmrTerminal.Enabled = false;            

            RopenFileDialog1.InitialDirectory = "";
            //openFileDialog1.Filter = "rti files (*.rti)|*.rti|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            RopenFileDialog1.Filter = "ens files (*.ens)|*.ens|txt files (*.txt)|*.txt|All files (*.*)|*.*|raw files (*.raw)||bin files (*.bin)|*.bin";
            RopenFileDialog1.FilterIndex = 3;
            RopenFileDialog1.RestoreDirectory = true;

            if (RopenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((Rstream = RopenFileDialog1.OpenFile()) != null)
                    {
                        try
                        {   
                            string namestr = RopenFileDialog1.FileName;
                            string fname = namestr.Substring(namestr.Length - 12, 8);
                            if (fname[0] == 'R')//Transmit buffer
                            {
                                TopenFileDialog1.FileName = namestr.Substring(0, namestr.Length - 12);
                                TopenFileDialog1.FileName += "T" + namestr.Substring(namestr.Length - 11, 11);
                                if ((Tstream = TopenFileDialog1.OpenFile()) != null)
                                {
                                    
                                    fname = "T" + fname;
                                    RnBytes = Rstream.Length;

                                    textBoxExtract.Text = "Source File: " + RnBytes.ToString() + " bytes\r\n" + RopenFileDialog1.FileName + "\r\n";
                                    
                                    TnBytes = Tstream.Length;
                                    if (TnBytes > MaxDataBuff)
                                        TnBytes = MaxDataBuff;

                                    string DirName = "c:" + "\\RoweTechRiverTools_RawExtract" + "\\";
                                    string FilName = fname;

                                    DirectoryInfo di = new DirectoryInfo(DirName);
                                    try
                                    {
                                        if (di.Exists)// Determine whether the directory exists.
                                        {
                                            //di.Delete();// Delete the directory.
                                        }
                                        else// Try to create the directory.
                                        {
                                            di.Create();
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        textBoxExtract.Text += "Can't save csv! " + ex.ToString() + "\r\n";
                                    }
                                    finally
                                    {
                                        int TnBytesRead = Tstream.Read(aBuff, 0, (int)TnBytes);

                                        string str = DirName + "\\" + FilName + ".csv";
                                        textBoxExtract.Text += "Destination File: \r\n" + str + "\r\n";

                                        try
                                        {
                                            string s = "";
                                            int nBytesRead = Rstream.Read(bBuff, 0, 400);
                                            int i, j, k;
                                            j = 0;
                                            for (i = 0; i < 100; i++)
                                            {
                                                val = (uint)aBuff[j] + 256 * (uint)aBuff[j + 1] + 256 * 256 * (uint)aBuff[j + 2] + 256 * 256 * 256 * (uint)aBuff[j + 3];
                                                s += val.ToString() + ",";
                                                val = (uint)bBuff[j] + 256 * (uint)bBuff[j + 1] + 256 * 256 * (uint)bBuff[j + 2] + 256 * 256 * 256 * (uint)bBuff[j + 3];
                                                s += val.ToString() + "\r\n";
                                                j += 4;
                                            }
                                            System.IO.File.WriteAllText(str, s);

                                            int m = 0;
                                            k = 0;
                                            val = (uint)aBuff[j] + 256 * (uint)aBuff[j + 1] + 256 * 256 * (uint)aBuff[j + 2] + 256 * 256 * 256 * (uint)aBuff[j + 3];
                                            s = val.ToString() + ",";
                                            

                                            Rstream.Read(bBuff, 0, 4);//skip one value to line up the data
                                            for (i = 0; i < (RnBytes - 400) / 4; i++)
                                            {
                                                Rstream.Read(bBuff, 0, 4);
                                                val = (uint)bBuff[0] + 256 * (uint)bBuff[1] + 256 * 256 * (uint)bBuff[2] + 256 * 256 * 256 * (uint)bBuff[3];

                                                //12 bit data comes in shifted over 2 bits
                                                uint sample = (uint)(0x0FFF & (val >> 2));
                                                // correct for 2's compliment data on the negative half
                                                if (sample >= 2048)
                                                    sample -= 2048;
                                                else
                                                    sample += 2048;

                                                s += sample.ToString();

                                                m++;

                                                if (m > 15)
                                                {
                                                    s += "\r\n";
                                                    m = 0;
                                                    System.IO.File.AppendAllText(str, s);
                                                    k++;
                                                    if (k > 99)
                                                    {
                                                        k = 0;
                                                        textBoxExtract.Text += ".";
                                                    }
                                                    Application.DoEvents();
                                                    j += 4;
                                                    if (j < TnBytesRead)
                                                    {
                                                        val = (uint)aBuff[j] + 256 * (uint)aBuff[j + 1] + 256 * 256 * (uint)aBuff[j + 2] + 256 * 256 * 256 * (uint)aBuff[j + 3];
                                                        s = val.ToString() + ",";
                                                    }
                                                    else
                                                    {
                                                        j = TnBytesRead;
                                                        s = " ,";
                                                    }
                                                }
                                                else
                                                    s += ",";
                                            }
                                            if (j < TnBytesRead)
                                            {
                                                while (j < TnBytesRead)
                                                {   
                                                    Application.DoEvents();
                                                    j += 4;
                                                    if (j < TnBytesRead)
                                                    {
                                                        val = (uint)aBuff[j] + 256 * (uint)aBuff[j + 1] + 256 * 256 * (uint)aBuff[j + 2] + 256 * 256 * 256 * (uint)aBuff[j + 3];
                                                        s = val.ToString() + "\r\n";
                                                        System.IO.File.AppendAllText(str, s);
                                                    }
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            textBoxExtract.Text += "Can't save csv! " + ex.ToString() + "\r\n";
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("caughtAAA: {0}", ex.GetType().ToString());
                            MessageBox.Show(exceptionmessage);
                        }
                        //if (Rstream != null)
                            Rstream?.Close();
                        //if (Tstream != null) 
                            Tstream?.Close();
                    }
                
                }
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("caughtBBB: {0}", ex.GetType().ToString());
                    MessageBox.Show(exceptionmessage);
                }
            }
            PlaybackEnableButtons();
        }
        
        void ExtractPD3Series(int cs, PD3ArrayClass PD)
        {   
            string s = "";

            if (FirstSeries[cs])
            {  
                dtstrSeries[cs] = ((int)(PD.Hour)).ToString("D04") + ((int)(PD.Minute)).ToString("D02")+ ((int)(Math.Round(PD.Second))).ToString("D02");

                DirNameSeries[cs] = "c:" + "\\RoweTechRiverTools_PD3Extract";

                FilNameSeries[cs] = dtstrSeries[cs];

                s += "PD3ID,";
                s += "Data,";
                s += "BT velX(m/s),BT velY(m/s),BT velZ(m/s),";
                s += "WT velX(m/s),WT velY(m/s),WT velZ(m/s),";
                s += "BT range1(m),BT range2(m),BT range3(m),BT range4(m),";
                s += "BT avg range(m),";
                for (int i = 0; i < 16; i++)
                    s += "Spare" + i.ToString("D02") + ",";
                s += "Sensor,";
                s += "Seconds,";
                s += "Heading(deg),";
                s += "Pitch(deg),";
                s += "Roll(deg),";
                s += "Temperature(deg),";
                s += "BIT,";
                s += "Checksum";
            }
            s += "\r\n";

            s += PD.ID.ToString("X02") + ",";
            s += PD.Data.ToString("X02") + ",";
            s += PD.BTvel[0].ToString() + ",";
            s += PD.BTvel[1].ToString() + ",";
            s += PD.BTvel[2].ToString() + ",";
            s += PD.WTvel[0].ToString() + ",";
            s += PD.WTvel[1].ToString() + ",";
            s += PD.WTvel[2].ToString() + ",";
            s += PD.BTrange[0].ToString() + ",";
            s += PD.BTrange[1].ToString() + ",";
            s += PD.BTrange[2].ToString() + ",";
            s += PD.BTrange[3].ToString() + ",";
            s += PD.BTavgRange.ToString() + ",";
            for (int i = 0; i < 16; i++)
                s+=PD.Spare[i].ToString() + ",";
            s += PD.Sensor.ToString("X04") + ",";
            s += (3600 * PD.Hour + 60 * PD.Minute + PD.Second).ToString() + ",";
            s += PD.Heading.ToString() + ",";
            //string ss = PD.Pitch.ToString() + ",";
            s += PD.Pitch.ToString() + ",";
            s += PD.Roll.ToString() + ",";
            s += PD.Temperature.ToString() + ",";
            s += PD.BIT.ToString("X04") + ",";
            s += PD.Checksum.ToString();

            //write the file
            DirectoryInfo di = new DirectoryInfo(DirNameSeries[cs]);
            try
            {
                if (di.Exists)// Determine whether the directory exists.
                {
                    //di.Delete();// Delete the directory.
                }
                else// Try to create the directory.
                {
                    di.Create();
                }
            }
            catch (System.Exception ex)
            {
                textBoxExtract.Text += "Can't save txt! " + ex.ToString() + "\r\n";
            }
            finally
            {
                string str = DirNameSeries[cs] + "\\" + FilNameSeries[cs] + ".csv";
                if (FirstSeries[cs])
                    textBoxExtract.Text += "Destination File: \r\n" + str + "\r\n";
                try
                {
                    if (FirstSeries[cs])
                    {
                        System.IO.File.WriteAllText(str, s);
                        FirstSeries[cs] = false;
                    }
                    else
                        System.IO.File.AppendAllText(str, s);
                }
                catch (System.Exception ex)
                {
                    textBoxExtract.Text += "Can't save text! " + ex.ToString() + "\r\n";
                }
            }
             
        }
        
        private void buttonExtractVTGbottomnav_Click(object sender, EventArgs e)
        {
            PlaybackPaused = false;

            int i;
            for (i = 0; i < csubs; i++)
                FirstSeries[i] = true;

            long nBytesRead;
            long nBytes;
            Stream stream;// = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            PlaybackDisableButtons();

            tmrTerminal.Enabled = false;

            DecodeState = 0;
            DataBuffReadIndex = 0;
            DataBuffWriteIndex = 0;
            NmeaBuffReadIndex = 0;
            BackScatter.DataBuffWriteIndex = 0;
            BackScatter.DataBuffReadIndex = 0;

            openFileDialog1.InitialDirectory = "";
            //openFileDialog1.Filter = "rti files (*.rti)|*.rti|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.Filter = "ens files (*.ens)|*.ens|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog1.OpenFile()) != null)
                    {
                        try
                        {
                            nBytes = stream.Length;
                            textBoxExtract.Text = "Source File: " + nBytes.ToString() + " bytes\r\n" + openFileDialog1.FileName + "\r\n";

                            nBytesRead = stream.Read(bBuff, 0, 10000);
                            while (nBytesRead > 0 && PlaybackEnabled)
                            {
                                for (i = 0; i < nBytesRead; i++)
                                {
                                    DataBuff[DataBuffWriteIndex] = bBuff[i];
                                    DataBuffWriteIndex++;
                                    if (DataBuffWriteIndex > MaxDataBuff)
                                        DataBuffWriteIndex = 0;
                                    BackScatter.DataBuffWriteIndex = DataBuffWriteIndex;
                                }

                                int DBRI = DataBuffWriteIndex;

                                while (DBRI != DataBuffReadIndex)
                                {
                                    DBRI = DataBuffReadIndex;
                                    DecodeNavData();

                                    //System.Threading.Thread.Sleep(100);
                                    //Application.DoEvents();
                                }
                                textBoxExtract.Text += ".";
                                nBytesRead = stream.Read(bBuff, 0, 10000);
                                Application.DoEvents();
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("caughtF3: {0}", ex.GetType().ToString());
                            MessageBox.Show(exceptionmessage);
                        }
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("caughtG: {0}", ex.GetType().ToString());
                    MessageBox.Show(exceptionmessage);
                }
            }
            PlaybackEnableButtons();
        }
        private void buttonPD3toCSV_Click(object sender, EventArgs e)
        {
            PlaybackPaused = false;

            int i;
            for (i = 0; i < csubs; i++)
                FirstSeries[i] = true;

            long nBytesRead;
            long nBytes;
            Stream stream;// = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            PlaybackDisableButtons();

            tmrTerminal.Enabled = false;

            DecodeState = 0;
            DataBuffReadIndex = 0;
            DataBuffWriteIndex = 0;
            NmeaBuffReadIndex = 0;

            BackScatter.DataBuffWriteIndex = 0;
            BackScatter.DataBuffReadIndex = 0;

            openFileDialog1.InitialDirectory = "";
            openFileDialog1.Filter = "ens files (*.ens)|*.ens|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog1.OpenFile()) != null)
                    {
                        try
                        {
                            nBytes = stream.Length;
                            textBoxExtract.Text = "Source File: " + nBytes.ToString() + " bytes\r\n" + openFileDialog1.FileName + "\r\n";

                            nBytesRead = stream.Read(bBuff, 0, 10000);
                            while (nBytesRead > 0 && PlaybackEnabled)
                            {
                                for (i = 0; i < nBytesRead; i++)
                                {
                                    DataBuff[DataBuffWriteIndex] = bBuff[i];
                                    DataBuffWriteIndex++;
                                    if (DataBuffWriteIndex > MaxDataBuff)
                                        DataBuffWriteIndex = 0;
                                    BackScatter.DataBuffWriteIndex = DataBuffWriteIndex;
                                }

                                int DBRI = DataBuffWriteIndex;

                                while (DBRI != DataBuffReadIndex)
                                {
                                    DBRI = DataBuffReadIndex;
                                    if (DecodePD3Data(PD3))
                                        ExtractPD3Series(0,PD3);

                                    //System.Threading.Thread.Sleep(100);
                                    //Application.DoEvents();
                                }
                                textBoxExtract.Text += ".";
                                nBytesRead = stream.Read(bBuff, 0, 10000);
                                Application.DoEvents();
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("caughtF4: {0}", ex.GetType().ToString());
                            MessageBox.Show(exceptionmessage);
                        }
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("caughtG: {0}", ex.GetType().ToString());
                    MessageBox.Show(exceptionmessage);
                }
            }
            PlaybackEnableButtons();
        }
        //Terminal---------------------------------------------------------
        private void SendCom()
        {
            if (UsingSerial)
            {
                if (_serialPort.IsOpen)
                {
                    string message1 = txtUserCommand.Text + '\r';
                    //string message3;

                    try
                    {
                        _serialPort.Write(message1);
                    }
                    catch //(System.Exception ex)
                    {
                        //message3 = String.Format("caughtC: {0}", ex.GetType().ToString());
                    }
                    tmrTerminal.Enabled = true;
                    tmrTerminal.Start();
                }
            }
        }

        string txtSerialStr = "";
        bool txtSerialNewData = true;
        bool DontClip = false;
        private void WriteMessageTxtSerial(string message, bool linefeed)
        {
            string str = txtSerialStr + message;
            if (linefeed)
                str += Environment.NewLine;
            if (!DontClip)
            {
                if (str.Length >= txtSerial.MaxLength)
                {
                    int si = str.Length - txtSerial.MaxLength;
                    str = str.Substring(si, str.Length - si);
                }
            }
            txtSerialStr = str;

            txtSerialNewData = true;

        }
        
        private void txtUserCommand_TextChanged(object sender, EventArgs e)
        {
            int i;
            bool gotone = false;
            string message1 = txtUserCommand.Text;
            string message2 = "";

            for (i = 0; i < message1.Length; i++)
            {
                if (message1[i] == '\n' || message1[i] == '\r')
                {
                    gotone = true;
                }
                else
                    message2 += message1[i];
            }
            if (gotone)
            {
                txtUserCommand.Text = message2;
                //txtUserCommand.Select(txtUserCommand.Text.Length, 1);
                SendCom(txtUserCommand.Text);
                txtUserCommand.Text = "";
            }
        }

        private string IncFileSequenceNumber(string FN)
        {
            string FNout;// ="";            
            string sname = FN.Substring(0, FN.Length - 11);
            string sn = FN.Substring(FN.Length - 11, 7);
            int n = Convert.ToInt32(sn);

            int last = FN.LastIndexOf(".");

            string ext = FN.Substring(last, FN.Length - last);

            n ++;
            sn = n.ToString();
            sn = AddZeros(sn, 7);

            FNout = sname + sn + ext;

            return FNout;
        }
        private void btnCapture_Click(object sender, EventArgs e)
        {   
            if (CaptureData)
            {
                CaptureData = false;
                textBoxCaptureStatus.Text = "Capture OFF";
            }
            else
            {
                textBoxDataSize.Text = "";
                FileBytes = 0;
                textBoxCaptureStatus.Text = "Capture ON";

                //DataBuffReadIndexA = DataBuffWriteIndex;
                DataBuffReadIndexC = DataBuffWriteIndex;
                DataBuffReadIndex = DataBuffWriteIndex;

                BTnavClr();
                CapturedBytes = 0;

                CaptureData = true;
                

                string dtstr = DateTime.Now.ToString("yyyyMMddHHmmss");
                CaptureFileName = dtstr + "_rti0000000.bin";
                CaptureFileName = IncFileSequenceNumber(CaptureFileName);
            }
        }
        private void btnClear_Click(object sender, System.EventArgs e)
        {
            //EMACpings = 0;
            ClearTextScreen = true;//txtSerial.Clear();
            txtSerialStr = "";
            //ClearTextScreen = false;
            txtSerialNewData = true;

        }
        
        private void buttonTerminalSTART_Click(object sender, EventArgs e)
        {
            buttonSTART_Click(sender,e);
        }
        private void buttonTerminalSTOP_Click(object sender, EventArgs e)
        {
            buttonSTOP_Click(sender, e);
        }
        private void buttonTerminalBREAK_Click(object sender, EventArgs e)
        {
            buttonBreak_Click(sender, e);
        }
        private void buttonTerminalSLEEP_Click(object sender, EventArgs e)
        {
            buttonSleep_Click(sender, e);
        }
        private void buttonTerminalSetTime_Click(object sender, EventArgs e)
        {
            buttonSetTime_Click(sender, e);
        }
        private void buttonSetTime_Click(object sender, EventArgs e)
        {
            PlaybackEnabled = false;
            tmrTerminal.Enabled = true;
            DateTime currentDate = DateTime.Now;
            string s = currentDate.Year.ToString("D4");
            s += "/" + currentDate.Month.ToString("D2");
            s += "/" + currentDate.Day.ToString("D2");
            s += " " + currentDate.Hour.ToString("D2");
            s += ":" + currentDate.Minute.ToString("D2");
            s += ":" + currentDate.Second.ToString("D2");

            txtUserCommand.Text = "STIME " + s;
            SendCom(txtUserCommand.Text);
        }
        private void buttonSTART_Click(object sender, EventArgs e)
        {
            BTnavClr();
            tmrTerminal.Enabled = true;
            txtUserCommand.Text = "";//clear the input buffer
            SendCom(txtUserCommand.Text);
            //delay for rs485 response
            System.Threading.Thread.Sleep(100);
            Application.DoEvents();

            DecodeState = 0;
            DataBuffReadIndex = DataBuffWriteIndex;

            NmeaDecodeState = 0;
            NmeaBuffReadIndex = DataBuffWriteIndex;

            txtUserCommand.Text = "Start";
            SendCom(txtUserCommand.Text);
        }
        private void buttonSTOP_Click(object sender, EventArgs e)
        {   
            txtUserCommand.Text = "STOP";
            SendCom(txtUserCommand.Text);
            //delay for rs485 response
            System.Threading.Thread.Sleep(100);
            Application.DoEvents();

            DecodeState = 0;
            DataBuffReadIndex = DataBuffWriteIndex;

            //NMEAEnableDecode = false;
            NmeaDecodeState = 0;

            NmeaBuffReadIndex = DataBuffWriteIndex;
            //CompassBuffReadIndex = DataBuffWriteIndex;

            PlaybackEnabled = false;
        }
        private void buttonBreak_Click(object sender, EventArgs e)
        {
            BreakPort(1000);
            //BreakPort(300);
        }
        private void buttonSleep_Click(object sender, EventArgs e)
        {
            PlaybackEnabled = false;
            tmrTerminal.Enabled = true;
            //if (radioButtonEthernet.Checked)
            //    txtUserCommand.Text = "SLEEPA";
            //else
                txtUserCommand.Text = "SLEEP";

            SendCom(txtUserCommand.Text);
        }
        private void buttonTerminalDeploy_Click(object sender, EventArgs e)
        {
            DeployUploadCommands();
        }


        void BreakPort(int msec)
        {
            PlaybackEnabled = false;
            tmrTerminal.Enabled = true;
            if (UsingSerial)
            {
                try
                {
                    _serialPort.BreakState = true;
                    System.Threading.Thread.Sleep(msec);//1000 = 1 second
                    Application.DoEvents();
                    _serialPort.BreakState = false;
                }
                catch
                { }
            }
            else
            {
                SendCom("BREAK");
            }
        }
        private void buttonForceBaud_Click(object sender, EventArgs e)
        {
            DisableButtons();

            ClearTextScreen = true;//txtSerial.Clear();


            if (UsingSerial)
            {
                try
                {
                    int n = listBoxMainPortBaud.FindStringExact("115200");
                    if (n != ListBox.NoMatches)
                        listBoxMainPortBaud.SelectedIndex = n;

                    n = listBoxMainPortBits.FindStringExact("8");
                    if (n != ListBox.NoMatches)
                        listBoxMainPortBits.SelectedIndex = n;

                    n = listBoxMainPortParity.FindStringExact("None");
                    if (n != ListBox.NoMatches)
                        listBoxMainPortParity.SelectedIndex = n;

                    n = listBoxMainPortStopBits.FindStringExact("1");
                    if (n != ListBox.NoMatches)
                        listBoxMainPortStopBits.SelectedIndex = n;

                    int Baud = 115200;

                    _serialPort.BaudRate = Baud;
                    textBoxCommsMainPortManBaud.Text = Baud.ToString();
                    
                    _serialPort.DataBits = 8;
                    textBoxCommsMainPortManBits.Text = "8";
                    
                    _serialPort.Parity = Parity.None;
                    textBoxCommsMainPortManBits.Text = "None";

                    PlaybackEnabled = false;
                    tmrTerminal.Enabled = true;

                    _serialPort.BreakState = true;

                    for (int ttt = 0; ttt < 20; ttt++)//20 seconds
                    {
                        textBoxForceBaudTime.Text = (19-ttt).ToString();
                        System.Threading.Thread.Sleep(1000);//1000 = 1 second
                        Application.DoEvents();
                    }
                    
                    _serialPort.BreakState = false;

                    System.Threading.Thread.Sleep(1000);//1000 = 1 second
                    Application.DoEvents();
                    _serialPort.Write("STOP\r");
                    System.Threading.Thread.Sleep(1000);//1000 = 1 second
                    Application.DoEvents();
                    _serialPort.Write("STOP\r");
                    System.Threading.Thread.Sleep(1000);//1000 = 1 second
                    Application.DoEvents();
                    _serialPort.Write("STOP\r");
                    System.Threading.Thread.Sleep(1000);//1000 = 1 second
                    Application.DoEvents();
                    _serialPort.Write("ENGPORT\r");

                    
                }
                catch
                { }
            }
            EnableButtons();
        }
        private void buttonFileErase_Click(object sender, EventArgs e)
        {
            //if (_serialPort.IsOpen)
            {
                string FileName = txtUserCommand.Text;
                string message1;
                try
                {
                    if (textBoxFileSDcard.Text == "0")
                    {
                        message1 = "DSERASE" + FileName;
                    }
                    else
                    {
                        message1 = "DSERASE" + FileName + "," + textBoxFileSDcard.Text;
                    }
                    //_serialPort.Write(message1);
                    SendCom(message1);
                }
                catch { }
            }
        }
        
        private void DisableButtons()
        {
            
            groupBoxADCPControl2.Enabled = false;
            txtUserCommand.Enabled = false;
            buttonSendCom1.Enabled = false;
            comboBox1.Enabled = false;
            groupBoxADCPControl.Enabled = false;            
            groupBoxFile.Enabled = false;
            
            groupBoxPlayback.Enabled = false;
            groupBoxPlaybackSeries.Enabled = false;
            groupBoxPlaybackNMEA.Enabled = false;
            btnSendCom.Enabled = false;            
            btnCapture.Enabled = false;            
        }
        private void EnableButtons()
        {
            
            groupBoxADCPControl2.Enabled = true;
            txtUserCommand.Enabled = true;
            buttonSendCom1.Enabled = true;
            comboBox1.Enabled = true;
            groupBoxADCPControl.Enabled = true;
            groupBoxFile.Enabled = true;
            
            groupBoxPlayback.Enabled = true;
            groupBoxPlaybackSeries.Enabled = true;
            groupBoxPlaybackNMEA.Enabled = true;
            btnSendCom.Enabled = true;            
            btnCapture.Enabled = true;            
        }

        private void btnSendCom_Click(object sender, System.EventArgs e)
        {
            SendCom(comboBox1.Text);
        }
        private void buttonSendCom1_Click(object sender, EventArgs e)
        {
            SendCom(txtUserCommand.Text);
        }
        private void SendCom(string txt)
        {
            if (UsingSerial)
            {
                if (_serialPort.IsOpen)
                {
                    tmrTerminal.Enabled = true;
                    tmrTerminal.Start();

                    string message1 = txt + '\r';
                    //string message3;

                    try
                    {
                        _serialPort.Write(message1);
                    }
                    catch// (System.Exception ex)
                    {
                        //message3 = String.Format("caughtC: {0}", ex.GetType().ToString());
                    }
                }
            }
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (comboBox1.Text != "")
            {
                SendComboCom(comboBox1.Text);
                comboBox1.Text = "";
            }*/
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((this.ActiveControl == comboBox1) && (keyData == Keys.Return))
            {
                if (comboBox1.Text != "")
                {
                    bool needtoadd = true;
                    for (int n = 0; n < comboBox1.Items.Count; n++)
                    {
                        if (comboBox1.FindStringExact(comboBox1.Text) != -1)
                        {
                            needtoadd = false;
                            break;
                        }
                    }
                    if (needtoadd)
                        comboBox1.Items.Add(comboBox1.Text);

                    SendCom(comboBox1.Text);
                    comboBox1.Text = "";
                }
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        string AddZeros(string str, int n)
        {
            string s = str;

            for (int i = 0; i < n - str.Length; i++)
            {
                s = "0" + s;
            }

            return s;
        }

        //MERGE ------------------------------------------------------------------------------
        private Boolean FirstMerge = true;
        private string MergeFileName;
        private string MergeFileNameAndDirectory;

        private void MergeAppendPacket(byte[] buf, int offset, int bytes)
        {
            Boolean OK = true;

            string DirectoryName = @"c:\RoweTechRiverTools_Merge";
            DirectoryInfo di = new DirectoryInfo(DirectoryName);

            try
            {
                if (di.Exists)
                {
                }
                else
                {
                    di.Create();
                }
            }
            catch
            {
                OK = false;
                WriteMessageTxtSerial("Can't create directory", true);
            }

            string Path;

            if (OK)
            {
                if (FirstMerge)
                {
                    string dtstr = DateTime.Now.ToString("yyyyMMddHHmmss");
                    MergeFileName = dtstr + "_rti.bin";
                    FirstMerge = false;
                    MergeFileNameAndDirectory = DirectoryName + "\\" + MergeFileName;
                }

                Path = DirectoryName + "\\" + MergeFileName;

                FileStream fs;
                // Create the new, empty data file.
                if (File.Exists(Path))
                {
                    //if (MessageBox.Show("Ok to overwrite " + Path + "?", "Erase File", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    //    OK = false;
                }

                if (OK)
                {
                    try
                    {
                        if (File.Exists(Path))
                        {
                            fs = new FileStream(Path, FileMode.Append);
                        }
                        else
                            fs = new FileStream(Path, FileMode.CreateNew);

                        BinaryWriter w = new BinaryWriter(fs);

                        w.Write(buf, offset, bytes);
                        w.Close();
                        fs.Close();
                    }
                    catch (System.Exception ex)
                    {
                        string exceptionmessage = String.Format("caughtX: {0}", ex.GetType().ToString());
                        WriteMessageTxtSerial(exceptionmessage, true);
                    }
                }
            }
        }

        private void buttonMergeFiles_Click(object sender, EventArgs e)
        {

            //rawData0000001.bin

            string ss;
            long nBytes;
            long nBytesRead;
            long TotalBytes = 0;
            Stream stream;// = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string exceptionmessage;
            int FileInc = 1;
            try
            {
                FileInc = Convert.ToInt32(textBoxMergeFilesInc.Text);
                if (FileInc < 1)
                {
                    FileInc = 1;
                    textBoxMergeFilesInc.Text = FileInc.ToString();
                }
            }
            catch
            {
                textBoxMergeFilesInc.Text = FileInc.ToString();
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ss = openFileDialog1.FileName;
                string sname = ss.Substring(0, ss.Length - 11);
                string sn = ss.Substring(ss.Length - 11, 7);
                int n = Convert.ToInt32(sn);

                int FilesMerged = 0;

                int last = ss.LastIndexOf(".");

                string ext = ss.Substring(last, ss.Length - last);

                FirstMerge = true;
                bool done = false;

                textBoxExtract.Text = "";

                while (!done)
                {
                    try
                    {
                        if ((stream = openFileDialog1.OpenFile()) != null)
                        {
                            try
                            {
                                FilesMerged++;
                                nBytes = stream.Length;
                                textBoxExtract.Text = openFileDialog1.FileName + " " + nBytes.ToString() + "\r\n" + textBoxExtract.Text;

                                //textBoxMergeTotalBytes.Text = TotalBytes.ToString();

                                Application.DoEvents();

                                while (nBytes > 0)
                                {
                                    nBytesRead = stream.Read(bBuff, 0, 1000000);
                                    TotalBytes += nBytesRead;
                                    textBoxMergeTotalBytes.Text = TotalBytes.ToString();
                                    Application.DoEvents();

                                    MergeAppendPacket(bBuff, 0, (int)nBytesRead);
                                    nBytes -= nBytesRead;
                                }
                                stream.Close();
                            }
                            catch (Exception ex)
                            {
                                exceptionmessage = String.Format("caughtZ: {0}", ex.GetType().ToString());
                                MessageBox.Show(exceptionmessage);
                            }
                            stream.Close();
                        }
                        else
                            done = true;
                    }
                    catch//(Exception ex)
                    {
                        done = true;
                        //exceptionmessage = String.Format("caughtY: {0}", ex.GetType().ToString());
                        //MessageBox.Show(exceptionmessage);
                    }

                    n += FileInc;
                    sn = n.ToString();
                    sn = AddZeros(sn, 7);
                    openFileDialog1.FileName = sname + sn + ext;// ".bin";
                }

                string str = textBoxExtract.Text;
                textBoxExtract.Text = "Merge Complete:\r\n";
                textBoxExtract.Text += "   Files: " + FilesMerged.ToString() + "\r\n";
                textBoxExtract.Text += "   Bytes: " + textBoxMergeTotalBytes.Text + "\r\n\r\n";
                textBoxExtract.Text += "Merge File Location:\r\n";
                textBoxExtract.Text += MergeFileNameAndDirectory + "\r\n\r\n";
                textBoxExtract.Text += "Original File Location:\r\n";
                textBoxExtract.Text += str;
            }
        }

        //Waves Recover ----------------------------------------------------------------------
        int rtitime_JulianDayNumber(int year, int month, int day)
        {
            int a = (14 - month) / 12;
            int y = year + 4800 - a;
            int m = month + 12 * a - 3;

            int JDN = day + (153 * m + 2) / 5 + (365 * y) + y / 4 - y / 100 + y / 400 - 32045;

            return JDN;
        }
        long gregorian_calendar_to_jd(int y, int m, int d)
        {
            y += 8000;
            if (m < 3)
            {
                y--;
                m += 12;
            }
            return (y * 365) + (y / 4) - (y / 100) + (y / 400) - 1200820 + (m * 153 + 3) / 5 - 92 + d - 1;
        }

        long ymd_to_jdnl(int y, int m, int d, int julian)
        {
            long jdn;
            if (y < 0) // adjust BC year
                y++;
            if (julian > 0)
                jdn = 367L * y - 7 * (y + 5001L + (m - 9) / 7) / 4 + 275 * m / 9 + d + 1729777L;
            else
                jdn = (long)(d - 32076) + 1461L * (y + 4800L + (m - 14) / 12) / 4 + 367 * (m - 2 - (m - 14) / 12 * 12) / 12
                            - 3 * ((y + 4900L + (m - 14) / 12) / 100) / 4 + 1;/* correction by rdg */
            return jdn;
        }
        void jdnl_to_ymd(long jdn, out int yy, out int mm, out int dd, int julian)
        {
            long x, z, m, d, y;
            long daysPer400Years = 146097;
            long fudgedDaysPer4000Years = 1460970 + 31;

            x = jdn + 68569;
            if (julian > 0)
            {
                x += 38;
                daysPer400Years = 146100;
                fudgedDaysPer4000Years = 1461000 + 1;
            }
            z = 4 * x / daysPer400Years;
            x = x - (daysPer400Years * z + 3) / 4;
            y = 4000 * (x + 1) / fudgedDaysPer4000Years;
            x = x - 1461 * y / 4 + 31;
            m = 80 * x / 2447;
            d = x - 2447 * m / 80;
            x = m / 11;
            m = m + 2 - 12 * x;
            y = 100 * (z - 49) + y + x;
            yy = (int)y;
            mm = (int)m;
            dd = (int)d;
            if (yy <= 0)// adjust BC years
                (yy)--;
        }


        string DirDir, DirDirStats;
        
        

        bool HighSpeedPortAvailable()
        {   
            if (SendADCPCommand("ENGPORT"))
            {
                if(txtSerial.Text.IndexOf("RS422") > 0)
                    return true;
                if (txtSerial.Text.IndexOf("RS485") > 0)
                    return true;
                if (txtSerial.Text.IndexOf("EMAC") > 0)
                    return true;
            }
            return false;
        }

        string strMatlabFile = "";
        int MatlabWavesN = 0;
        int MatlabEnsembleN = 0;
        string ExtractMatlab(byte[] packet, ArrayClass m)
        {
            int PayloadPointer = HDRLEN;
            int PayloadSize = PacketSize - HDRLEN - 4;
            string str;
            if (m.EnsembleDataAvailable)
            {
                MatlabWavesN++;
                str = CaptureAppendPacket(packet, PayloadPointer, PayloadSize, @"c:\RoweTechRiverTools_Matlab", "a" + MatlabWavesN.ToString("D07") + ".mat", true);
            }
            else
            {
                MatlabEnsembleN++;
                str = CaptureAppendPacket(packet, PayloadPointer, PayloadSize, @"c:\RoweTechRiverTools_Matlab", "w" + MatlabEnsembleN.ToString("D07") + ".mat", true);
            }
            return str;
        }
        private void buttonExtractMatlab_Click(object sender, EventArgs e)
        {
            //string dtstr = DateTime.Now.ToString("yyyyMMddHHmmss");

            PlaybackPaused = false;

            MatlabWavesN = 0;
            MatlabEnsembleN = 0;

            int i;
            
            long nBytesRead;
            long nBytes;
            Stream stream;// = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            PlaybackDisableButtons();

            tmrTerminal.Enabled = false;
            DecodeState = 0;
            DataBuffReadIndex = 0;
            DataBuffWriteIndex = 0;
            NmeaBuffReadIndex = 0;

            BackScatter.DataBuffWriteIndex = 0;
            BackScatter.DataBuffReadIndex = 0;

            openFileDialog1.InitialDirectory = "";
            //openFileDialog1.Filter = "rti files (*.rti)|*.rti|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.Filter = "ens files (*.ens)|*.ens|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog1.OpenFile()) != null)
                    {
                        try
                        {
                            nBytes = stream.Length;
                            textBoxWavesRecover.Text = "Source File: " + nBytes.ToString() + " bytes\r\n" + openFileDialog1.FileName + "\r\n";
                            string st = "";
                            nBytesRead = stream.Read(bBuff, 0, 10000);
                            while (nBytesRead > 0 && PlaybackEnabled)
                            {
                                for (i = 0; i < nBytesRead; i++)
                                {
                                    DataBuff[DataBuffWriteIndex] = bBuff[i];
                                    DataBuffWriteIndex++;
                                    if (DataBuffWriteIndex > MaxDataBuff)
                                        DataBuffWriteIndex = 0;
                                    BackScatter.DataBuffWriteIndex = DataBuffWriteIndex;
                                }

                                int DBRI = DataBuffWriteIndex;

                                while (DBRI != DataBuffReadIndex)
                                {
                                    DBRI = DataBuffReadIndex;
                                    DecodeData(true, false, false);
                                }
                                st += ".";
                                textBoxExtract.Text = strMatlabFile + "\r\n" + st;
                                nBytesRead = stream.Read(bBuff, 0, 10000);
                                Application.DoEvents();
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("caughtJ: {0}", ex.GetType().ToString());
                            MessageBox.Show(exceptionmessage);
                        }
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("caughtK: {0}", ex.GetType().ToString());
                    MessageBox.Show(exceptionmessage);
                }
            }
            PlaybackEnableButtons();
        }

        string StopADCPandGetSN(bool MustStop)
        {
            if (UsingSerial)
            {
                string message1 = "\r";// "\rD\r";
                _serialPort.Write(message1);
                System.Threading.Thread.Sleep(100);

                message1 = "D\r";
                _serialPort.Write(message1);
                System.Threading.Thread.Sleep(1000);
                Application.DoEvents();
            }   

            string sn = "";

            bool ADCPOK = true;
            if (MustStop)
            {
                if (!SendADCPCommand("STOP"))
                {
                    txtSerial.Text = "sending BREAK \r\n";
                    Application.DoEvents();
                    //send break then stop
                    BreakPort(1000);

                    System.Threading.Thread.Sleep(2000);
                    Application.DoEvents();

                    if (!SendADCPCommand("STOP"))
                    {
                        ADCPOK = false;
                        MessageBox.Show("ADCP/DVL not responding A");
                    }
                }
            }
            if (ADCPOK)
            {
                System.Threading.Thread.Sleep(1000);
                Application.DoEvents();

                txtSerialStr = "";

                if (SendADCPCommand("SN"))
                {
                    System.Threading.Thread.Sleep(1000);
                    Application.DoEvents();
                    sn = txtSerialStr;
                    int j = sn.IndexOf("SN+\r\n");
                    if (j >= 0 )//&& sn.Length == 37)
                    {
                        sn = sn.Substring(j+5, 32);
                    }
                    else
                    {
                        sn = "";
                        txtSerialStr = "";
                        if (SendADCPCommand("SN"))
                        {
                            sn = txtSerialStr;
                            j = sn.IndexOf("SN+\r\n");
                            if (j >= 0)
                            {
                                sn = sn.Substring(j+5, 32);
                            }
                        }
                    }
                }
                if(sn.Length < 32)
                    MessageBox.Show("ADCP/DVL not responding C");
            }
            return sn;
        }


        private void buttonDeployFormatSD_Click(object sender, EventArgs e)
        {
            //labelWavesFormatOK.Text = " ";
            //labelWavesFormatOK.BackColor = System.Drawing.Color.Yellow;

            if (MessageBox.Show("Ok to erase all data files?", "Format ADCP SD", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (SendADCPCommand("DSFORMAT"))
                {
                    System.Threading.Thread.Sleep(100);

                    DateTime currentDate = DateTime.Now;
                    int lastSec = currentDate.Second;

                    int ElapsedSec = 0;
                    //textBoxDeployADCPTimeOut.Text = "0";
                    while (txtSerial.Text.IndexOf("SD card Format Finished") < 0 )
                    {
                        Application.DoEvents();

                        currentDate = DateTime.Now;
                        ElapsedSec = currentDate.Second - lastSec;
                        if (ElapsedSec < 0)
                            ElapsedSec += 60;
                        //textBoxDeployADCPTimeOut.Text = ElapsedSec.ToString();
                        if (ElapsedSec > 15)
                            break;
                    }
                    Application.DoEvents();
                    //textBoxDeploy.Text = txtSerial.Text + textBoxDeploy.Text;

                    if (ElapsedSec > 15)
                    {
                        //labelWavesFormatOK.Text = " ";
                        //labelWavesFormatOK.BackColor = System.Drawing.Color.Red;
                        //textBoxDeploy.Text = "Timeout\r\n" + textBoxDeploy.Text;
                    }
                    else
                    {
                        /*
                        SD card Format Started
                        7580.000 MB, Allocation Size 32768 bytes
                        SD card Format Finished
                        */

                        //labelWavesFormatOK.Text = " ";
                        //labelWavesFormatOK.BackColor = System.Drawing.Color.LightGreen;
                    }
                }
                else
                {
                    //labelWavesFormatOK.Text = " ";
                    //labelWavesFormatOK.BackColor = System.Drawing.Color.Red;
                }
            }
        }
        /*private void buttonDeployCheckTime_Click(object sender, EventArgs e)
        {
            labelWavesReadTimeOK.Text = " ";
            labelWavesReadTimeOK.BackColor = System.Drawing.Color.Yellow;
            if (SendADCPCommand("STIME"))
            {
                textBoxDeployCheckTime.Text = txtSerial.Text.Substring(8);
                labelWavesReadTimeOK.Text = " ";
                labelWavesReadTimeOK.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                textBoxDeployCheckTime.Text = "";
                labelWavesReadTimeOK.Text = " ";
                labelWavesReadTimeOK.BackColor = System.Drawing.Color.Red;
            }
        }*/
        /*bool WavesTimeSet(bool UTC)
        {
            DateTime currentDate;
            if (UTC)
                currentDate = DateTime.UtcNow;
            else
                currentDate = DateTime.Now;
            string s = currentDate.Year.ToString("D4");
            s += "/" + currentDate.Month.ToString("D2");
            s += "/" + currentDate.Day.ToString("D2");
            s += " " + currentDate.Hour.ToString("D2");
            s += ":" + currentDate.Minute.ToString("D2");
            s += ":" + currentDate.Second.ToString("D2");

            string str = "STIME " + s;
            if (SendADCPCommand(str))
            {
                textBoxDeployCheckTime.Text = s;
                return true;
            }
            else
            {
                textBoxDeployCheckTime.Text = "Not Set";
                return false;
            }
        }

        private void buttonDeploySetTime_Click(object sender, EventArgs e)
        {
            labelWavesSetTimeOK.Text = " ";
            labelWavesSetTimeOK.BackColor = System.Drawing.Color.Yellow;
            bool ok = false;

            if(radioButtonDeployADCPGMT.Checked)
                ok = WavesTimeSet(true);
            else
                ok = WavesTimeSet(false);

            if (ok)
            {
                labelWavesSetTimeOK.Text = " ";
                labelWavesSetTimeOK.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                labelWavesSetTimeOK.Text = "X";
                labelWavesSetTimeOK.BackColor = System.Drawing.Color.Red;
            }
        }

        private void buttonDeployZeroPressure_Click(object sender, EventArgs e)
        {
            labelWavesZeroPsensOK.Text = " ";
            labelWavesZeroPsensOK.BackColor = System.Drawing.Color.Yellow;

            if (SendADCPCommand("CPZ"))
            {
                labelWavesZeroPsensOK.Text = " ";
                labelWavesZeroPsensOK.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                labelWavesZeroPsensOK.Text = "X";
                labelWavesZeroPsensOK.BackColor = System.Drawing.Color.Red;
            }
        }

        
        private void buttonDeployPrediction_Click(object sender, EventArgs e)
        {
            //DeployPrediction();            
        }
        void GetSS(out char c1, out char c2)
        {
            string s = comboBoxDeploySubSystemType.Text;            
            while (s.Substring(0,1) == " ")
            {
                s = s.Substring(1,s.Length - 1);
            }
            string sa = "0";
            string sb = "0";
            if (s.Length > 0)
            {
                sa = s.Substring(0, 1);
                if (s.Length > 1)
                {
                    sb = s.Substring(1, 1);
                }
            }
            c1 = Convert.ToChar(sa);
            c2 = Convert.ToChar(sb);
        }
        private void buttonDeploySaveConfigurationFile_Click(object sender, EventArgs e)
        {
            
        }

        void Deploy_SetConfiguration(string s)
        {
            
        }

        private void buttonDeployOpenConfigurationFile_Click(object sender, EventArgs e)
        {
            long nBytes = 0;
            Stream stream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            openFileDialog1.InitialDirectory = "c:\\RTI_Configuration_Files";
            openFileDialog1.Filter = "ens files (*.ens)|*.ens|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog1.OpenFile()) != null)
                    {
                        try
                        {
                            nBytes = stream.Length;
                            if (nBytes < 4096)
                            {
                                string s = File.ReadAllText(openFileDialog1.FileName);
                                textBoxDeploy.Text = openFileDialog1.FileName + "\r\n" + s;
                                Deploy_SetConfiguration(s);
                                buttonDeployPrediction_Click(sender, e);
                            }
                            else
                                MessageBox.Show("File Not Valid");
                        }

                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("Command File Error A: {0}", ex.GetType().ToString());
                            MessageBox.Show(exceptionmessage);
                        }
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("Command File Error B: {0}", ex.GetType().ToString());
                    MessageBox.Show(exceptionmessage);
                }
            }
        }

        private void labelDeployBinSize_Click(object sender, EventArgs e)
        {
            
        }

        private void labelDeployBlank_Click(object sender, EventArgs e)
        {
            
        }

        private void labelDeployLag_Click(object sender, EventArgs e)
        {
            
        }

        private void labelDeployBins_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonWaveDeployStartTime_Click(object sender, EventArgs e)
        {
            labelWavesStartTimeOK.Text = " ";
            labelWavesStartTimeOK.BackColor = System.Drawing.Color.Yellow;

            if (SendADCPCommand("CETFP " + textBoxDeployTFP.Text + ".00"))
            {
                if (SendADCPCommand("CSAVE"))
                {
                    labelWavesStartTimeOK.Text = " ";
                    labelWavesStartTimeOK.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    MessageBox.Show("Can't Save CETFP");
                    labelWavesStartTimeOK.Text = "X";
                    labelWavesStartTimeOK.BackColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                MessageBox.Show("Invalid Start Time");
                labelWavesStartTimeOK.Text = "X";
                labelWavesStartTimeOK.BackColor = System.Drawing.Color.Red;
            }
        }

        private void buttonDeployStartADCP_Click(object sender, EventArgs e)
        {
            labelWavesStartADCPok.Text = " ";
            labelWavesStartADCPok.BackColor = System.Drawing.Color.Yellow;

            if (SendADCPCommand("START"))
            {
                labelWavesStartADCPok.Text = " ";
                labelWavesStartADCPok.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                labelWavesStartADCPok.Text = "X";
                labelWavesStartADCPok.BackColor = System.Drawing.Color.Red;
                MessageBox.Show("Can't START ADCP");
            }
        }

        private void buttonDeployClearText_Click(object sender, EventArgs e)
        {
            textBoxDeploy.Text = "";
        }
        private void buttonDeploySaveText_Click(object sender, EventArgs e)
        {
            string DirName = "c:\\RTI_Waves_Configuration_Files";
            string FilName = "RTI_WavesLog.txt";
            //SaveTextFile(DirName, FilName, textBoxDeploy.Text, true);
            textBoxDeploy.Text = SaveTextFile(DirName, FilName, textBoxDeploy.Text, true, false) + "\r\n" + textBoxDeploy.Text;
        }
        */
        string ResponseString;

        bool SendCommandFile(string s)
        {
            bool stat = false;
            string line, cmd;
            try
            {
                using (StringReader reader = new StringReader(s))
                {
                    
                    ResponseString = "";
                    txtSerialStr = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            cmd = GetADCPCommand(line);
                        
                            if ((cmd[0] >= 'A' && cmd[0] <= 'Z') || (cmd[0] >= 'a' && cmd[0] <= 'z'))
                            {
                                switch (cmd)
                                {
                                    case "Days":
                                        break;
                                    case "SN":
                                        break;
                                    case "CEPO":
                                        SendADCPCommandNoVerify(line, 1000);
                                        ResponseString += txtSerialStr;
                                        textBoxCurrentCommand.Text = txtSerialStr;
                                        Application.DoEvents();
                                        break;
                                    default:
                                        stat = SendADCPCommand(line);
                                        ResponseString += txtSerialStr;
                                        textBoxCurrentCommand.Text = txtSerialStr;
                                        Application.DoEvents();
                                        if (!stat)
                                        {
                                            //return false;
                                            MessageBox.Show("Invalid Command " + txtSerialStr);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    
                    if (stat)
                    {
                        SendADCPCommand("CSAVE");
                        ResponseString += txtSerialStr;
                        textBoxCurrentCommand.Text = txtSerialStr;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                string exceptionmessage = String.Format("Configuration Upload Error: {0}", ex.GetType().ToString());
                MessageBox.Show(exceptionmessage);
            }
            return stat;
        }
        bool DeployUploadCommands()
        {
            bool ok = false;
            long nBytes;
            Stream stream;// = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            openFileDialog1.InitialDirectory = "c:\\RoweTechRiverTools_Configuration_Files";
            openFileDialog1.Filter = "ens files (*.ens)|*.ens|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog1.OpenFile()) != null)
                    {
                        txtUserCommand.Text = openFileDialog1.FileName;
                        try
                        {
                            nBytes = stream.Length;
                            if (nBytes < 4096)
                            {
                                string s = File.ReadAllText(openFileDialog1.FileName);
                                
                                //for (int n = 0; n < 3; n++)
                                {
                                    ok = SendCommandFile(s);
                                    
                                    txtSerialStr = ResponseString;
                                    txtSerialNewData = true;
                                    
                                }
                            }
                            else
                                MessageBox.Show("File Not Valid");
                        }

                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("Command File Error A: {0}", ex.GetType().ToString());
                            MessageBox.Show(exceptionmessage);
                        }
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("Command File Error B: {0}", ex.GetType().ToString());
                    MessageBox.Show(exceptionmessage);
                }
            }
            return ok;
        }

        /*private void buttonDeployUploadCommands_Click(object sender, EventArgs e)
        {
            labelWavesConfigOK.Text = " ";
            labelWavesConfigOK.BackColor = System.Drawing.Color.Yellow;

            if (DeployUploadCommands())
            {
                labelWavesConfigOK.Text = " ";
                labelWavesConfigOK.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                labelWavesConfigOK.Text = "X";
                labelWavesConfigOK.BackColor = System.Drawing.Color.Red;
            }
        }*/
        
        //EMAC------------------------------------------

        //

        


        //
        /*byte[] IPbuffer = new byte[1472];
        int EMACpings = 0;
        private int IP_Message(bool dummy, bool SendCommand, bool ShowText, string CMD, int timeout, byte[] buff, int offset)
        {
            EMACpings ++;
            textBoxEMACpings.Text = EMACpings.ToString();
            int i,j;
            byte[] IPa = new byte[4];

            string[] args = { "82.84.73.80", "123" };

            args[0] = textBoxEMACA.Text + "." +
                      textBoxEMACB.Text + "." +
                      textBoxEMACC.Text + "." +
                      textBoxEMACD.Text;

            IPa[0] = Convert.ToByte(textBoxEMACA.Text);
            IPa[1] = Convert.ToByte(textBoxEMACB.Text);
            IPa[2] = Convert.ToByte(textBoxEMACC.Text);
            IPa[3] = Convert.ToByte(textBoxEMACD.Text);

            int IPbytes = 0;

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of bytes to be transmitted.

            byte[] buffer = new byte[1472 - 448];

            string data;
            if (dummy)
                data = "ABCD";
            else
                data = "RTIy";

            if (SendCommand)
            {
                data += CMD;
            }

            byte[] tbuff = Encoding.ASCII.GetBytes(data);

            for (i = 0; i < tbuff.Length; i++)
            {
                buffer[i] = tbuff[i];
            }

            if (CMD == "x")
            {
                buffer[i++] = buff[0];
                buffer[i++] = buff[1];
                buffer[i++] = buff[2];
                buffer[i++] = buff[3];
                buffer[i++] = buff[4];
                buffer[i++] = buff[5];
                buffer[i++] = buff[6];
                buffer[i++] = buff[7];
            }
            else
            {
                if (CMD == "u")
                {
                    int bytes = buff[0] << 8;
                    bytes += buff[1];
                    for (j = 0; j < bytes + 6 + 2; j++)
                    {
                        buffer[i++] = buff[j];
                    }
                }
                else
                {
                    if (CMD == "s")
                    {
                        int bytes = buff[0];
                        for (j = 0; j < bytes + 4 + 2 + 1; j++)
                        {
                            buffer[i++] = buff[j];
                        }
                    }
                }
            }

            try
            {
                PingReply reply = pingSender.Send(args[0], timeout, buffer, options);
                
                if (reply.Status == IPStatus.Success)
                {
                    if (ShowText)
                    {
                        WriteMessageTxtSerial("\r\nStatus:         " + reply.Status.ToString(),true);
                        WriteMessageTxtSerial("Address:        " + reply.Address.ToString(), true);
                        WriteMessageTxtSerial("RoundTrip time: " + reply.RoundtripTime.ToString(), true);
                        WriteMessageTxtSerial("Time to live:   " + reply.Options.Ttl.ToString(), true);
                        WriteMessageTxtSerial("Don't fragment: " + reply.Options.DontFragment.ToString(), true);
                        WriteMessageTxtSerial("Buffer size:    " + reply.Buffer.Length.ToString() + "\r\n", true);
                    }
                    if (reply.Buffer[0] == IPa[0] &&
                        reply.Buffer[1] == IPa[1] &&
                        reply.Buffer[2] == IPa[2] &&
                        reply.Buffer[3] == IPa[3])
                    {
                        int bytes;
                        bytes = reply.Buffer[4];
                        bytes += reply.Buffer[5] << 8;                        

                        int mbytes = reply.Buffer.Length - 6;

                        if (bytes > mbytes)
                            bytes = mbytes;

                        IPbytes = bytes;

                        j = 0;
                        for (i = 6; i < bytes + 6; i++, j++)
                        {   
                            buffer[j] = reply.Buffer[i];                            
                            IPbuffer[j] = reply.Buffer[i];
                        }
                        if (ShowText)
                        {
                            buffer[j] = 0;
                            data = Encoding.ASCII.GetString(buffer,0,j);
                            WriteMessageTxtSerial(data, true);
                        }
                        else
                        {
                            for (i = 0; i < IPbytes; i++)
                            {
                                buff[offset] = buffer[i];
                                offset++;
                            }
                        }
                    }
                    else
                        WriteMessageTxtSerial("Payload: " + reply.Buffer[0].ToString("X02") + reply.Buffer[1].ToString("X02") + reply.Buffer[2].ToString("X02") + reply.Buffer[3].ToString("X02") + "\r\n", false);
                }
                else
                {
                    if (ShowText)
                    {
                        WriteMessageTxtSerial("\r\n* Timeout:",true);
                    }
                    IPbytes = -1;
                }
            }
            catch
            {
                WriteMessageTxtSerial("...EMAC error...", false);
            }            
            return IPbytes;
        }
        
        byte[] dummybuffer = new byte[4096];
        */
        /*
        void disablegroups()
        {
            groupBoxADCPControl.Enabled = false;
            
            groupBoxADCPControl2.Enabled = false;
            groupBoxFile.Enabled = false;
            Application.DoEvents();
            //groupsdisabled = true;
        }
        void enablegroups()
        {
            groupBoxADCPControl.Enabled = true;
            
            groupBoxADCPControl2.Enabled = true;
            groupBoxFile.Enabled = true;
            Application.DoEvents();
            //groupsdisabled = false;
        }
        */
  
        //DateTime currentDate = DateTime.Now;
        long DownLoadTicks;
        const int DOWNLOAD_DOT = 1;
        const int DOWNLOAD_COLON = 2;
        const int DOWNLOAD_x = 3;
        const int DOWNLOAD_NODIR = 4;
        const int DOWNLOAD_DONE = 5;
        const int DOWNLOAD_C = 6;
        const int DOWNLOAD_N = 7;
        const int DOWNLOAD_I = 8;
        const int DOWNLOAD_M = 9;
        const int DOWNLOAD_n = 10;
        const int DOWNLOAD_STATS = 11;
        const int DOWNLOAD_FILENAME = 12;
        const int DOWNLOAD_CHECKSUMFAIL = 13;
        const int DOWNLOAD_EOT = 14;
        const int DOWNLOAD_STX = 15;
        const int DOWNLOAD_SOH = 16;

        const int DOWNLOAD_WAVES = 1;
        const int DOWNLOAD_SERIAL = 2;

        string DownLoadLog;
        string DownLoadLogNow;

        void DownLoadLogUpdate(int type, int packets, int bytes, double seconds, int whichone, string FileName, string Header)
        {
            switch (type)
            {
                default:
                    break;
                case DOWNLOAD_CHECKSUMFAIL:
                    DownLoadLog += "\r\nChecksum Failed\r\n";
                    DownLoadLogNow += "\r\nChecksum Failed\r\n";
                    break;
                case DOWNLOAD_FILENAME:
                    DownLoadLog += FileName + " ";
                    DownLoadLogNow = FileName + " ";
                    break;
                case DOWNLOAD_DOT:
                    DownLoadLog += ".";
                    DownLoadLogNow += ".";
                    break;
                case DOWNLOAD_x://might want to comment this out
                    //DownLoadLog += "x";
                    //DownLoadLogNow += "x";
                    break;
                case DOWNLOAD_COLON:
                    DownLoadLog += ":";
                    DownLoadLogNow += ":";
                    break;
                case DOWNLOAD_NODIR:
                    DownLoadLog += "Can't create directory\r\n";
                    DownLoadLogNow += "Can't create directory\r\n";
                    break;                
                case DOWNLOAD_DONE:                    
                    string ms = packets.ToString();
                    DownLoadLog += "\r\n" + ms + " packets";
                    DownLoadLogNow += "\r\n" + ms + " packets";
                    break;
                case DOWNLOAD_C:
                    DownLoadLog += "C";
                    DownLoadLogNow += "C";
                    break;
                case DOWNLOAD_EOT:
                    DownLoadLog += "EOT";
                    DownLoadLogNow += "EOT";
                    break;
                case DOWNLOAD_STX:
                    DownLoadLog += "STX";
                    DownLoadLogNow += "STX";
                    break;
                case DOWNLOAD_SOH:
                    DownLoadLog += "SOH";
                    DownLoadLogNow += "SOH";
                    break;
                case DOWNLOAD_I://unknown responce
                    DownLoadLog += "I";
                    DownLoadLogNow += "I";
                    break;
                case DOWNLOAD_M://had the correct number of bytes for the packet but failed checksum, flushed buffer
                    DownLoadLog += "M";
                    DownLoadLogNow += "M";
                    break;
                case DOWNLOAD_N://Timeout, flushed the buffer
                    DownLoadLog += "N";
                    DownLoadLogNow += "N";
                    break;
                case DOWNLOAD_n://incorrect number of bytes received, flushed buffer
                    DownLoadLog += "n";
                    DownLoadLogNow += "n";
                    break;
                case DOWNLOAD_STATS:
                    //DirDirStats = bytes.ToString() + " bytes, " + seconds.ToString("0.00") + " seconds, ";
                    //DirDirStats += (8 * bytes / seconds).ToString("0") + " bps";

                    DirDirStats = AddSpaces(bytes.ToString(),15) + AddSpaces(seconds.ToString("0.00"),8);
                    DirDirStats += AddSpaces((8 * bytes / seconds).ToString("0"),10);

                    DownLoadLog += ", " + bytes.ToString() + " bytes, " + seconds.ToString("0.00") + " seconds, ";
                    DownLoadLog += (8 * bytes / seconds).ToString("0") + " bps\r\n";
                    DownLoadLogNow += ", " + bytes.ToString() + " bytes, " + seconds.ToString("0.00") + " seconds, ";
                    DownLoadLogNow += (8 * bytes / seconds).ToString("0") + " bps\r\n";
                    break;
            }
            switch(whichone)
            {
                default:
                    //txtSerial.Text = DownLoadLogNow;
                    txtSerialStr = Header + DownLoadLogNow;
                    txtSerialNewData = true;
                    break;
                case DOWNLOAD_WAVES:
                    textBoxWavesRecoverDownload.Text = Header + DownLoadLogNow;
                    break;
            }
        }
        int DownloadRetries = 0;
        int DownloadTries = 0;
        
        bool UsingSerial = true;
 
        private void radioButtonSerial_CheckedChanged(object sender, EventArgs e)
        {
            /*if (radioButtonSerial.Checked)
            {
                UsingSerial = true;
                listBoxAvailablePorts_SelectedIndexChanged(sender, e);
            }*/
        }

        
        

        //Playback-------------------------------------------------------------
        private void buttonPlaybackPause_Click(object sender, EventArgs e)
        {
            PlaybackPaused = true;
        }
        private void buttonPlaybackStop_Click(object sender, EventArgs e)
        {
            PlaybackEnableButtons();
            ShowBottomTrackEnsemble(Arr, true);
        }
        private void buttonPlaybackStep_Click(object sender, EventArgs e)
        {
            PlaybackStep = true;
        }
        private void buttonPlaybackStepBack_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
                DataBuffReadIndex -= (LastEnsembleSize[i]+8);
            //DataBuffReadIndex -= (3 * (EnsembleSize + 8));
            if (DataBuffReadIndex < 0)
                DataBuffReadIndex += MaxDataBuff;

            PlaybackStepBack = true;
            PlaybackPaused = true;
        }
        private void buttonPlayBackGo_Click(object sender, EventArgs e)
        {
            PlaybackStep = true;
            PlaybackStepBack = false;
            PlaybackPaused = false;
        }
        private void PlaybackDisableButtons()
        {
            PlaybackEnabled = true;
            buttonPLAYBACK.Enabled = false;
            buttonSTART.Enabled = false;
            btnCapture.Enabled = false;

            
        }
        private void PlaybackEnableButtons()
        {
            PlaybackEnabled = false;
            buttonPLAYBACK.Enabled = true;
            buttonSTART.Enabled = true;
            btnCapture.Enabled = true;

        }
        private void buttonPLAYBACK_Click(object sender, EventArgs e)
        {
            int i;
            
            VTGlastens = 0;
            
            for (i = 0; i < 3; i++)
                LastEnsembleSize[i] = 0;

            BTnavClr();
            PlaybackPaused = true;

            long nBytesRead;
            long nBytes;
            Stream stream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            PlaybackDisableButtons();

            tmrTerminal.Enabled = false;

            buttonClearAverage_Click(sender, e);
            
            NmeaDecodeState = 0;
            for (i = 0; i < csubs;i++ )
                FirstGGA[i] = true;
            
            Navigate = true;
            FirstRTI01 = true;

            DecodeState = 0;
            DataBuffReadIndex = 0;
            DataBuffWriteIndex = 0;
            NmeaBuffReadIndex = 0;

            BackScatter.DataBuffWriteIndex = 0;
            BackScatter.DataBuffReadIndex = 0;

            openFileDialog1.InitialDirectory = "";
            openFileDialog1.Filter = "ens files (*.ens)|*.ens|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;

            bool FirstEnsemble = true;
            long LastEnsemble = 0;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog1.OpenFile()) != null)
                    {
                        txtUserCommand.Text = openFileDialog1.FileName;
                        try
                        {
                            nBytes = stream.Length;

                            nBytesRead = stream.Read(bBuff, 0, 1000000);
                            while (nBytesRead > 0 && PlaybackEnabled)
                            {
                                for (i = 0; i < nBytesRead; i++)
                                {
                                    DataBuff[DataBuffWriteIndex] = bBuff[i];
                                    DataBuffWriteIndex++;
                                    if (DataBuffWriteIndex > MaxDataBuff)
                                        DataBuffWriteIndex = 0;
                                    BackScatter.DataBuffWriteIndex = DataBuffWriteIndex;
                                }

                                int DBRI = DataBuffWriteIndex;
                                
                                while (DBRI != DataBuffReadIndex)
                                {
                                    DBRI = DataBuffReadIndex;

                                    if (radioButtonASCII.Checked)
                                    {
                                        textBoxCapturedNMEA.Clear();
                                        if (DataBuffWriteIndex != NmeaBuffReadIndex && NMEAEnableDecode)
                                        {
                                            int cs = 0;
                                            int DBRJ = DataBuffWriteIndex;
                                            while (DBRJ != NmeaBuffReadIndex && NMEAEnableDecode)
                                            {
                                                DBRJ = NmeaBuffReadIndex;
                                                DecodeNmea(cs, DataBuff, DataBuffWriteIndex,true,false);
                                                Application.DoEvents();
                                                if(FreshGGA[cs])
                                                    doGGANav(cs);
                                            }
                                            
                                        }
                                        if (PlaybackEnabled)
                                        {
                                            while (PlaybackPaused && !PlaybackStep && !PlaybackStepBack && PlaybackEnabled)
                                            {
                                                System.Threading.Thread.Sleep(100);
                                                Application.DoEvents();
                                            }
                                            PlaybackStep = false;
                                            PlaybackStepBack = false;
                                        }
                                    }
                                    else
                                    {
                                        if (DecodeData(false, false, false))
                                        {   
                                            int cs = (int)(Arr.E_CurrentSystem >> 24);
                                            if (cs < 0)
                                                cs = 0;
                                            if (cs > csubs - 1)
                                                cs = csubs - 1;

                                            if (Arr.EnsembleDataAvailable)
                                            {   
                                                if (FirstEnsemble)
                                                {
                                                    FirstEnsemble = false;
                                                    WriteMessageTxtSerial("\r\n" + Arr.E_EnsembleNumber.ToString() + ",First Ensemble", true);
                                                }

                                                textBoxCapturedNMEA.Clear();
                                                DecodeEnsembleNmea(Arr, cs);

                                                //show NMEA or WI/WD BI/BD
                                                //byte v = Arr.E_FW_Vers[1];
                                                if (Arr.NmeaAvailable)
                                                {
                                                    NmeaBuffReadIndex = 0;
                                                    DecodeNmea(cs, Arr.NMEA_Buffer, Arr.NMEA_Bytes, false,true);
                                                    //Application.DoEvents();
                                                }

                                                LastEnsemble = Arr.E_EnsembleNumber;
                                                AccumulateEnsemble(Arr, Arr2);
                                                ProfileStats(Arr,true);

                                                MoveSeriesData(Arr);
                                                DoAllNav(Arr,cs);

                                                float BTperr = 100;
                                                if (GGANAV[cs].DMG > 0)
                                                    BTperr = (float)(100.0 * (BTdisMag[cs] / GGANAV[cs].DMG - 1));

                                                int index = SeriesIndex[cs] - 1;
                                                if (index < 0)
                                                    index = MaxSeries - 1;

                                                if (NewGGA[cs] && FreshBT[cs])
                                                {
                                                    MagSeriesBT[cs, 5, index] = BTperr;
                                                }
                                                else
                                                {
                                                    MagSeriesBT[cs, 5, index] = 100;
                                                }

                                                ShowEnsemble(Arr, Arr2);
                                                ShowSeries(Arr, Arr2);
                                            }

                                            if (PlaybackEnabled)
                                            {
                                                while (PlaybackPaused && !PlaybackStep && !PlaybackStepBack && PlaybackEnabled)
                                                {
                                                    System.Threading.Thread.Sleep(100);
                                                    Application.DoEvents();
                                                }
                                                PlaybackStep = false;
                                                PlaybackStepBack = false;
                                            }
                                        }
                                    }
                                    //System.Threading.Thread.Sleep(100);
                                    Application.DoEvents();
                                }
                                nBytesRead = stream.Read(bBuff, 0, 10000);
                            }
                            WriteMessageTxtSerial(LastEnsemble.ToString() + ",Last Ensemble", true);
                        }
                        
                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("caughtD1: {0}", ex.GetType().ToString());
                            MessageBox.Show(exceptionmessage);
                        }
                        stream.Close();
                    }
                }
                
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("caughtE: {0}", ex.GetType().ToString());
                    MessageBox.Show(exceptionmessage);
                }
                
            }
            PlaybackEnableButtons();
        }
        
        private void buttonPlaybackSeries_Click(object sender, EventArgs e)
        {   
            buttonPLAYBACK_Click(sender, e);
        }
        private void buttonPlaybackGoSeries_Click(object sender, EventArgs e)
        {
            buttonPlayBackGo_Click(sender, e);
        }
        private void buttonPlaybackStepSeries_Click(object sender, EventArgs e)
        {
            buttonPlaybackStep_Click(sender,e);
        }
        private void buttonPlaybackStepBackSeries_Click(object sender, EventArgs e)
        {
            buttonPlaybackStepBack_Click(sender, e);
        }
        private void buttonPlaybackPauseSeries_Click(object sender, EventArgs e)
        {
            buttonPlaybackPause_Click(sender, e);
        }
        private void buttonPlaybackStopSeries_Click(object sender, EventArgs e)
        {
            buttonPlaybackStop_Click(sender, e);
        }

        private void buttonPlaybackNMEA_Click(object sender, EventArgs e)
        {
            buttonPLAYBACK_Click(sender, e);
        }
        private void buttonPlaybackStepBackNMEA_Click(object sender, EventArgs e)
        {
            buttonPlaybackStepBack_Click(sender, e);
        }
        private void buttonPlaybackStepNMEA_Click(object sender, EventArgs e)
        {
            buttonPlaybackStep_Click(sender, e);
        }
        private void buttonPlaybackGoNMEA_Click(object sender, EventArgs e)
        {
            buttonPlayBackGo_Click(sender, e);
        }
        private void buttonPlaybackPauseNMEA_Click(object sender, EventArgs e)
        {
            buttonPlaybackPause_Click(sender, e);
        }
        private void buttonPlaybackStopNMEA_Click(object sender, EventArgs e)
        {
            buttonPlaybackStop_Click(sender, e);
        }
        /*
        //DVL TAB ------------------------------------------------------------
        //int[] DVLbottomEnabled = new int[16];
        //int[] DVLwaterEnabled = new int[16];

        
        string logstring(string InStr, string OutStr)
        {
            return ("-->\r\n" + OutStr + "<--\r\n" + InStr);
        }
        string DVLbtbb(string instr1, string instr2, string instr3)
        {
            string outstr = "";
            try//CBTBB[0] 1, 0.000, 50.00, 4
            {
                if (instr2 == "OFF")
                    outstr = "1,0,0";//broadband
                else
                    outstr = "7,0," + instr2;//auto switch BB/NB

                if (instr3 == "OFF")
                    outstr += ",4";//4 beam xmt
                else
                    outstr += ",1";//1 beam xmt singaround
            }
            catch { }
            return outstr;
        }
        string DVLoutputType(string instr)
        {
            string outstr = "";
            switch (instr)
            {
                default:
                case "OFF":
                    outstr = "0";
                    break;
                case "RTI 1":
                    outstr = "1";
                    break;
                case "RTI 2":
                    outstr = "2";
                    break;
                case "PD 0 beam":
                    outstr = "100,0";
                    break;
                case "PD 0 xyz":
                    outstr = "100,1";
                    break;
                case "PD 0 ship":
                    outstr = "100,3";
                    break;
                case "PD 0 Earth":
                    outstr = "100,2";
                    break;
                case "PD 4":
                    outstr = "104";
                    break;
                case "PD 5":
                    outstr = "105";
                    break;
                case "PD 6":
                    outstr = "106";
                    break;
                case "PD 13":
                    outstr = "113";
                    break;
            }
            return outstr;
        }
        string GetDVLConfiguration()
        {
            string message1 = "CSHOW\r";

            ClearTextScreen = true; //txtSerial.Text = "";

            if (UsingSerial)
            {
                tmrTerminal.Enabled = true;
                tmrTerminal.Start();
                if (_serialPort.IsOpen)
                {
                    try
                    {

                        _serialPort.Write(message1);
                    }
                    catch { }
                }
            }
            
            System.Threading.Thread.Sleep(1000);
            Application.DoEvents();

            return message1 + "\n" + txtSerial.Text;
        }
        string GetDVLoutputType(string line, int i)
        {
            int j = line.IndexOf(",");
            int PD0xfrm = 0;
            if (j > 0)
            {
                PD0xfrm = Convert.ToInt32(line.Substring(j + 1));
                j = Convert.ToInt32(line.Substring(i, j - i));
            }
            else
                j = Convert.ToInt32(line.Substring(i));

            string st = "";
            switch (j)
            {
                default:
                    st = j.ToString();
                    break;
                case 0:
                    st = "OFF";
                    break;
                case 1:
                    st = "RTI 1";
                    break;
                case 2:
                    st = "RTI 2";
                    break;
                case 100:
                    st = "PD 0";
                    switch (PD0xfrm)
                    {
                        default:
                            st += PD0xfrm.ToString();
                            break;
                        case 0:
                            st += " beam";
                            break;
                        case 1:
                            st += " xyz";
                            break;
                        case 2:
                            st += " Earth";
                            break;
                        case 3:
                            st += " ship";
                            break;
                    }
                    break;
                case 104:
                    st = "PD 4";
                    break;
                case 105:
                    st = "PD 5";
                    break;
                case 106:
                    st = "PD 6";
                    break;
                case 113:
                    st = "PD 13";
                    break;
                case 213:
                    st = "PTIC";
                    break;

                
            }
            return st;
        }
        */
        private void radioButtonSeriesBTmag_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSeriesBTmag.Checked)
            {
                SeriesBTDataType = TypeBTmag;
                ShowSeries(Arr, Arr2);
            }
        }


        private void groupBox45_Enter(object sender, EventArgs e)
        {

        }

        private void button1PTICPD13toCSV_Click(object sender, EventArgs e)
        {
            PlaybackPaused = false;

            //int i;
            //for (i = 0; i < csubs; i++)
            //    FirstSeries[i] = true;

            //long nBytesRead = 0;
            long nBytes;
            Stream stream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string exceptionmessage;

            PlaybackDisableButtons();

            tmrTerminal.Enabled = false;

            string line;// = "";

            openFileDialog1.InitialDirectory = "";
            openFileDialog1.Filter = "ens files (*.ens)|*.ens|txt files (*.txt)|*.txt|All files (*.*)|*.*|bin files (*.bin)|*.bin";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog1.OpenFile()) != null)
                    {
                        try
                        {
                            nBytes = stream.Length;
                            textBoxExtract.Text = "Source File: " + nBytes.ToString() + " bytes\r\n" + openFileDialog1.FileName + "\r\n";
                            //"c:" + "\\RTI_Extract" + "\\" + snSeries[cs];
                            string DirName = "c:" + "\\RoweTechRiverTools_Extract" + "\\" + "WIWDBIBD";
                            //string str = DirNameSeries[cs] + "\\" + FilNameSeries[cs] + ".csv";
                            System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog1.FileName);
                            DirectoryInfo di = new DirectoryInfo(DirName);
                            try
                            {
                                if (di.Exists)// Determine whether the directory exists.
                                {
                                    //di.Delete();// Delete the directory.
                                }
                                else// Try to create the directory.
                                {
                                    di.Create();
                                }
                            }
                            catch (System.Exception ex)
                            {
                                textBoxExtract.Text += "Can't save txt! " + ex.ToString() + "\r\n";
                            }
                            finally
                            {   
                                int j1,j2,j3,j4;
                                int linecount = 0;
                                string str = DirName + "\\PD13.csv";

                                textBoxExtract.Text += "Destination File: \r\n" + str + "\r\n";
                                string s = "";
                                System.IO.File.WriteAllText(str, s);

                                while ((line = file.ReadLine()) != null)
                                {
                                    j1 = line.IndexOf(":WI");
                                    j2 = line.IndexOf(":WD");
                                    j3 = line.IndexOf(":BI");
                                    j4 = line.IndexOf(":BD");
                                    s = "";
                                    if (j1 >= 0 && (line.Length - j1) == 33)
                                    {
                                        s += line.Substring(j1) + ",";
                                    }
                                    if (j2 >= 0 && (line.Length - j2) == 57)
                                    {
                                        s += line.Substring(j2) + ",";
                                    }
                                    if (j3 >= 0 && (line.Length - j3) == 33)
                                    {
                                        s += line.Substring(j3) + ",";
                                    }
                                    if (j4 >= 0 && (line.Length - j4) == 57)
                                    {
                                        s += line.Substring(j4) + "\r";
                                    }
                                    System.IO.File.AppendAllText(str, s);
                                    linecount++;
                                    if (linecount > 99)
                                    {
                                        linecount = 0;
                                        textBoxExtract.Text += ".";
                                        Application.DoEvents();
                                    }
                                }
                                textBoxExtract.Text += "\r\nFinished";
                            }
                            file.Close();
                        }
                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("caughtF1: {0}", ex.GetType().ToString());
                            MessageBox.Show(exceptionmessage);
                        }
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    exceptionmessage = String.Format("caughtG1: {0}", ex.GetType().ToString());
                    MessageBox.Show(exceptionmessage);
                }
            }
            PlaybackEnableButtons();
        }
        private void checkBoxNMEA_ASCII_Input_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNMEA_ASCII_Input.Checked)
                NmeaBuffReadIndex = DataBuffWriteIndex;
        }

      
        //Download
        string RecoverDir(string FirstLetter, string ext)
        {
            string dirstr;// = "";
            string s = "";
            txtSerialStr = ""; //ClearTextScreen = true;//txtSerial.Clear();

            if (SendADCPCommand("DSDIR"))
            {
                textBoxWavesRecoverDownload.Clear();
                //wait for quiet
                int BS = 0;
                DontClip = true;
                while (txtSerialStr.Length > BS)
                {
                    BS = txtSerialStr.Length;

                    if(UsingSerial)
                        System.Threading.Thread.Sleep(2000);
                    else
                        System.Threading.Thread.Sleep(200);

                    Application.DoEvents();
                }

                dirstr = txtSerialStr;

                for (int n = 0; n < FirstLetter.Length; n++)
                {
                    char FirstChar = FirstLetter[n];
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    int i = dirstr.LastIndexOf("Used Space:");//Used Space:                           5.458 MB
                    if (i > 0 && dirstr.LastIndexOf("MB", i) > 0)
                    {
                        using (StringReader reader = new StringReader(dirstr))
                        {
                            string line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.ToUpper().IndexOf(ext) > 0 && line[0] == FirstChar)
                                {
                                    s += line + "\r\n";
                                }
                            }
                        }
                    }
                }
                DontClip = false;
            }
            
            return s;
        }
        private void buttonRecoverDir_Click(object sender, EventArgs e)
        {
            string FirstChar = "";
            string ext = ".ENS";

            if (radioButtonBurstDownLoad.Checked)
            {
                FirstChar = "B";
                ext = ".ENS";
            }
            
            if (radioButtonProfileDownLoad.Checked)
            {
                FirstChar = "A";
                ext = ".ENS";
            }
            if (radioButtonAllDownLoad.Checked)
            {
                FirstChar = "BWAE";
                ext = ".ENS";
            }
            if (radioButtonTxtDownload.Checked)
            {
                FirstChar = "TH";
                ext = ".TXT";
            }
            if (radioButtonRawDownload.Checked)
            {
                FirstChar = "RT";
                ext = ".RAW";
            }

            DirDir = RecoverDir(FirstChar, ext);
            if (DirDir != null && DirDir == "")
                DirDir = "No Files";
            textBoxWavesRecoverDownload.Text = DirDir;
        }
        int CurrentBaud = 0;

        string FileLocation = "";
        void RecoverDownload(string FirstLetter, string ext)
        {
            buttonRecoverDownload.Enabled = false;
            buttonRecoverDir.Enabled = false;
            //buttonWavesExtractAll.Enabled = false;

            XmodemCancelled = false;

            textBoxWavesRecoverDownload.Clear();
            textBoxWavesRecover.Clear();

            string dir = RecoverDir(FirstLetter, ext);

            DirDir = dir;

            int packetbytes = 1000;
            CurrentBaud = 0;

            string sn = StopADCPandGetSN(true);

            if (UsingSerial)
            {
                if (_serialPort.BaudRate != 921600 && HighSpeedPortAvailable())
                {
                    if (SendADCPCommand("CBAUD921600"))
                    {
                        CurrentBaud = _serialPort.BaudRate;
                        _serialPort.BaudRate = 921600;
                        SendADCPCommand("");
                        System.Threading.Thread.Sleep(100);
                        Application.DoEvents();
                    }
                }
                //if (_serialPort.BaudRate == 921600)
                //    packetbytes = 16384;
                //else
                    packetbytes = 1024;
            }

            DownLoadLog = "";

            string DirectoryName = @"c:\RoweTechRiverTools_Download_Data\" + sn;

            //textBoxWavesRecover.Text = "Download File Location:\r\n" + DirectoryName + "\r\n";
            //textBoxWavesRecover.Text += "   File Name                   Mbytes\r\n" + dir;

            FileLocation = "Download File Destination:\r\n" + DirectoryName + "\r\n";
            FileLocation += "   File Name       Date     Time     Mbytes BytesTransfered Seconds       BPS\r\n";

            textBoxWavesRecover.Text = FileLocation + dir;
            //textBoxDownloadLog.Text = FileLocation + dir;

            string FirstFile = textBoxFirstFile.Text;
            bool GotFirst = false;
            if (FirstFile == "")
                GotFirst = true;
            //EmacAbort = false;
            using (StringReader reader = new StringReader(dir))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (GotFirst)
                    {
                        System.Threading.Thread.Sleep(100);
                        Application.DoEvents();
                    }
                    string FileName = line.Substring(0, line.ToUpper().IndexOf(ext) + ext.Length);

                    if (!GotFirst && FirstFile == FileName)
                        GotFirst = true;

                    if (GotFirst)
                    {
                        txtSerialStr = "";
                        XModemDownload(DirectoryName, FileName, packetbytes, DOWNLOAD_WAVES);
                    }
                    if (XmodemCancelled)// || EmacAbort)
                        break;

                    CancelXmodem(false);
                }
                if (dir == "")
                    textBoxWavesRecoverDownload.Text = "No Files";
                else
                    textBoxWavesRecoverDownload.Text = DownLoadLog;
            }
            if (CurrentBaud != 0)
            {
                _serialPort.BaudRate = CurrentBaud;
                BreakPort(1000);
            }
            buttonRecoverDownload.Enabled = true;
            buttonRecoverDir.Enabled = true;
            //buttonWavesExtractAll.Enabled = true;
        }

        private void buttonRecoverDownload_Click(object sender, EventArgs e)
        {
            string FirstChar = "";
            string ext = ".ENS";

            if (radioButtonBurstDownLoad.Checked)
            {
                FirstChar = "B";
                ext = ".ENS";
            }
            
            if (radioButtonProfileDownLoad.Checked)
            {
                FirstChar = "A";
                ext = ".ENS";
            }
            if (radioButtonAllDownLoad.Checked)
            {
                FirstChar = "BWAE";
                ext = ".ENS";
            }
            if (radioButtonRawDownload.Checked)
            {
                FirstChar = "TR";
                ext = ".RAW";
            }
            if (radioButtonTxtDownload.Checked)
            {
                FirstChar = "TH";
                ext = ".TXT";
            }

            RecoverDownload(FirstChar, ext);
        }

        private void buttonRecoverDownloadCancel_Click(object sender, EventArgs e)
        {
            //EmacAbort = true;
            XmodemCancel = true;
            //if (UsingSerial)
            {
                try
                {
                    byte[] buff = new byte[3];
                    buff[0] = Convert.ToByte('D');
                    buff[1] = CR;
                    PNIwrite(buff, 0, 2);// _serialPort.Write(buff, 0, 2);
                    textBoxWavesRecoverDownload.Text += "\r\nTransfer Canceled\r\n";
                }
                catch (Exception ex)
                {
                    textBoxWavesRecoverDownload.Text += String.Format("Cancel: {0}", ex.GetType().ToString()) + "\r\n";
                }
            }
        }

        private void buttonENGSAMP_Click(object sender, EventArgs e)
        {

        }

        private void buttonTime_Click(object sender, EventArgs e)
        {

        }

        private void buttonDirectory_Click(object sender, EventArgs e)
        {

        }

        private void buttonBurstDownload_Click(object sender, EventArgs e)
        {

        }

        private void buttonRTrawDownload_Click(object sender, EventArgs e)
        {

        }

        private void buttonXModemUpload_Click(object sender, EventArgs e)
        {

        }

        private void buttonRawDownLoad_Click(object sender, EventArgs e)
        {

        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {

        }

        private void buttonGetConf_Click(object sender, EventArgs e)
        {

        }

        private void buttonExtractText_Click(object sender, EventArgs e)
        {

        }

        private void buttonBTnavClr_Click(object sender, EventArgs e)
        {

        }

        private void textBoxProfileSubStats_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonProfileText_Click(object sender, EventArgs e)
        {

        }

        private void buttonProfileGraph_Click(object sender, EventArgs e)
        {

        }

        private void buttonStatistics_Click(object sender, EventArgs e)
        {

        }

        private void buttonNMEADisableExport_Click(object sender, EventArgs e)
        {

        }

        private void buttonNMEAEnableExport_Click(object sender, EventArgs e)
        {

        }

        private void buttonNMEADisableDecode_Click(object sender, EventArgs e)
        {

        }

        private void buttonNMEAEnableDecode_Click(object sender, EventArgs e)
        {

        }
      
        private void buttonEngPingPlayback_Click(object sender, EventArgs e)
        {

        }

        private void buttonEngPingGraph_Click(object sender, EventArgs e)
        {

        }

        private void buttonEngPingText_Click(object sender, EventArgs e)
        {

        }

        private void buttonEngPingStop_Click(object sender, EventArgs e)
        {

        }

        private void buttonEngPingStart_Click(object sender, EventArgs e)
        {

        }

        private void buttonPNIstandard270_Click(object sender, EventArgs e)
        {

        }

        private void buttonPNIStandard0_Click(object sender, EventArgs e)
        {

        }

        private void buttonPNIStandard90_Click(object sender, EventArgs e)
        {

        }

        private void buttonStandard180_Click(object sender, EventArgs e)
        {

        }

    
        

        //BTIC Project 2 -------------------------------------------------------------------------
        
        private void textBoxFirstBin_TextChanged(object sender, EventArgs e)
        {
            if (!FormLoading)
            {
                try
                {
                    FirstBin = Convert.ToInt16(textBoxFirstBin.Text);
                    if (FirstBin < 0)
                        FirstBin = 0;
                    if (FirstBin >= Arr.E_Cells)
                        FirstBin = (int)Arr.E_Cells - 1;
                    ShowEnsemble(Arr, Arr2);
                }
                catch { }
            }
        }

        

        private void checkBoxBTNAVuseZ_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBTNAVuseZ.Checked)
                UseZ = 1;
            else
                UseZ = 0;

            ShowEnsemble(Arr, Arr2);
        }

        private void checkBoxBTNAVshowalways_CheckedChanged(object sender, EventArgs e)
        {
            ShowEnsemble(Arr, Arr2);
        }

        /*private void GroupBox28_Enter(object sender, EventArgs e)
        {

        }
        */
        private void RadioButtonBIT0_CheckedChanged(object sender, EventArgs e)
        {

        }

        //River-----------------------------------------------------------------------------
        private void buttonRiverPlayback_Click(object sender, EventArgs e)
        {
            buttonPLAYBACK_Click(sender, e);
        }

        private void buttonRiverMinus_Click(object sender, EventArgs e)
        {
            buttonPlaybackStepBack_Click(sender, e);
        }

        private void buttonRiverGo_Click(object sender, EventArgs e)
        {
            buttonPlayBackGo_Click(sender, e);
        }

        private void buttonRiverStop_Click(object sender, EventArgs e)
        {
            buttonPlaybackStop_Click(sender, e);
        }

        private void buttonRiverPause_Click(object sender, EventArgs e)
        {
            buttonPlaybackPause_Click(sender, e);
        }

        private void buttonRiverPlus_Click(object sender, EventArgs e)
        {
            buttonPlaybackStep_Click(sender, e);
        }

        //Firmware-----------------------------------------------------------------
        private void SendCom(SerialPort Port, string txt)
        {


            if (UsingSerial)
            {
                if (Port.IsOpen)
                {
                    tmrTerminal.Enabled = true;
                    tmrTerminal.Start();

                    string message1 = txt + '\r';
                    //string message3;

                    try
                    {
                        Port.Write(message1);
                    }
                    catch { }// (System.Exception ex)
                    //{
                    //    message3 = String.Format("caughtC: {0}", ex.GetType().ToString());
                    //}
                }
            }
        }

        void BreakPort(SerialPort Port, int msec)
        {
            tmrTerminal.Enabled = true;
            if (UsingSerial)
            {
                try
                {
                    Port.BreakState = true;
                    System.Threading.Thread.Sleep(msec);//1000 = 1 second
                    Application.DoEvents();
                    Port.BreakState = false;
                }
                catch
                { }
            }
            else
            {
                SendCom(Port, "BREAK");
            }
        }

        private void WriteMessageFirmware(string message, bool linefeed)
        {
            string str = message;
            if (linefeed)
                str += Environment.NewLine;

            textBoxFirmware.Text += str;

            textBoxFirmware.SelectionStart = textBoxFirmware.Text.Length;
            textBoxFirmware.ScrollToCaret();
        }
        private void buttonFirmwareCurrentVersion_Click(object sender, EventArgs e)
        {
            txtSerialStr = "";
            SendADCPCommand("FMSHOW");
            Application.DoEvents();
            System.Threading.Thread.Sleep(1000);//1000 = 1 second
            Application.DoEvents();
            WriteMessageFirmware(txtSerialStr, true);
        }

        private void buttonFirmwareUpdate_Click(object sender, EventArgs e)
        {
            DisableButtons();

            labelFirmwareUpdate.Text = "Checking ADCP Attached";
            Application.DoEvents();
            txtSerialStr = "";
            BreakPort(_serialPort, 1000);

            int TimeCount = 10;
            while (txtSerialStr.IndexOf("Copyright") < 1 && TimeCount >= 0)
            {
                System.Threading.Thread.Sleep(100);//1000 = 1 second
                Application.DoEvents();
                TimeCount--;
            }
            //System.Threading.Thread.Sleep(1000);//1000 = 1 second
            //Application.DoEvents();

            WriteMessageFirmware(txtSerialStr, true);

            int co = txtSerialStr.IndexOf("Copyright");

            if (co > 0)
            {
                int r;// = txtSerialStr.IndexOf("RIVER");
                r = 1;
                if (r > 0)
                {
                    string FileName = "";
                    bool ok = true;
                    textBoxFirmware.Text = "";
                    txtSerialStr = "";
                    Application.DoEvents();

                    Stream stream = null;
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    string exceptionmessage;

                    openFileDialog1.InitialDirectory = "";
                    openFileDialog1.Filter = "bl files (*.bl)|*.bl|All files (*.*)|*.*";
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.RestoreDirectory = true;

                    labelFirmwareUpdate.Text = "Select Upload File";
                    Application.DoEvents();

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        FileName = openFileDialog1.SafeFileName;//.FileName;
                        try
                        {
                            if ((stream = openFileDialog1.OpenFile()) != null)
                            {
                                labelFirmwareUpdate.Text = "Uploading File";
                                Application.DoEvents();
                                if (UsingSerial)
                                    XModemUpload(stream, openFileDialog1.FileName, true);
                                //else
                                //    EMACupload(stream, openFileDialog1.FileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionmessage = String.Format("Firmware Update Exception: {0}", ex.GetType().ToString());
                            WriteMessageFirmware(exceptionmessage, true);
                            ok = false;
                        }
                        //if (stream != null)
                            stream?.Close();
                    }
                    else
                    {
                        ok = false;
                    }

                    int SD = 0;
                    if (ok)
                    {
                        labelFirmwareUpdate.Text = "Writing File To SD";
                        Application.DoEvents();

                        TimeCount = 100;
                        while (txtSerialStr.IndexOf("File successfully written to SD") < 1 && TimeCount >= 0)
                        {
                            System.Threading.Thread.Sleep(100);//1000 = 1 second
                            Application.DoEvents();
                            TimeCount--;
                        }

                        //System.Threading.Thread.Sleep(10000);//1000 = 1 second
                        //Application.DoEvents();

                        textBoxFirmware.Text = txtSerialStr;// txtSerial.Text;
                        Application.DoEvents();

                        SD = textBoxFirmware.Text.IndexOf("File successfully written to SD");
                    }

                    if ((SD > 0) && ok)//upload succeeded
                    {
                        labelFirmwareUpdate.Text = "Checking OS State";
                        Application.DoEvents();
                        txtSerialStr = "";

                        BreakPort(_serialPort, 1000);

                        TimeCount = 10;
                        while (txtSerialStr.IndexOf("Copyright") < 1 && TimeCount >= 0)
                        {
                            System.Threading.Thread.Sleep(100);//1000 = 1 second
                            Application.DoEvents();
                            TimeCount--;
                        }
                        //System.Threading.Thread.Sleep(1000);//1000 = 1 second
                        //Application.DoEvents();

                        WriteMessageFirmware(txtSerialStr, true);

                        int c = txtSerialStr.IndexOf("Copyright");

                        if (c > 0)//ADCP is attached
                        {

                            labelFirmwareUpdate.Text = "Enabling BootLoader";
                            Application.DoEvents();

                            int b = txtSerialStr.IndexOf("Bootloader");
                            if (b <= 0)//Bootloader is not running
                            {
                                txtSerialStr = "";
                                SendADCPCommand("BLJMPBL");
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(200);//1000 = 1 second
                                Application.DoEvents();

                                BreakPort(_serialPort, 1000);
                                Application.DoEvents();
                                TimeCount = 100;
                                while (txtSerialStr.IndexOf("Bootloader") < 1 && TimeCount >= 0)
                                {
                                    System.Threading.Thread.Sleep(100);//1000 = 1 second
                                    Application.DoEvents();
                                    TimeCount--;
                                }
                                //System.Threading.Thread.Sleep(1000);//1000 = 1 second
                                //Application.DoEvents();
                                WriteMessageFirmware(txtSerialStr, true);
                                b = txtSerialStr.IndexOf("Bootloader");
                            }
                            if (b > 0)//bootloader is running
                            {
                                labelFirmwareUpdate.Text = "Erasing Flash";
                                Application.DoEvents();

                                txtSerialStr = "";
                                SendADCPCommand("BLERASEFL 53A0E2D9");
                                Application.DoEvents();

                                TimeCount = 50;
                                while (txtSerialStr.IndexOf("Successfully erased Flash") < 1 && TimeCount >= 0)
                                {
                                    System.Threading.Thread.Sleep(100);//1000 = 1 second
                                    Application.DoEvents();
                                    TimeCount--;
                                }
                                //System.Threading.Thread.Sleep(5000);//1000 = 1 second
                                //Application.DoEvents();
                                WriteMessageFirmware(txtSerialStr, true);
                                b = txtSerialStr.IndexOf("Successfully erased Flash");

                                if (b > 0)//flash is erased
                                {
                                    labelFirmwareUpdate.Text = "Programming Flash";
                                    Application.DoEvents();
                                    txtSerialStr = "";
                                    SendADCPCommand("BLWRFL " + FileName + " 430000 0");
                                    Application.DoEvents();

                                    TimeCount = 50;
                                    while (txtSerialStr.IndexOf("Successfully programmed the Flash") < 1 && TimeCount >= 0)
                                    {
                                        System.Threading.Thread.Sleep(100);//1000 = 1 second
                                        Application.DoEvents();
                                        TimeCount--;
                                    }
                                    //System.Threading.Thread.Sleep(5000);//1000 = 1 second
                                    //Application.DoEvents();

                                    WriteMessageFirmware(txtSerialStr, true);
                                    b = txtSerialStr.IndexOf("Successfully programmed the Flash");
                                    if (b > 0)
                                    {
                                        labelFirmwareUpdate.Text = "Flash Programmed";
                                        //WriteMessageFirmware("\r\nPlease Cycle ADCP Power", true);
                                        //WriteMessageFirmware("\r\nVerify Proper Wakeup Message on Terminal\r\n", true);
                                        Application.DoEvents();

                                        labelFirmwareUpdate.Text = "Resetting System";
                                        Application.DoEvents();
                                        txtSerialStr = "";
                                        //SendADCPCommand("BLRST");
                                        SendADCPCommand("BLJMPAPP");
                                        Application.DoEvents();

                                        TimeCount = 50;
                                        while (txtSerialStr.IndexOf("Copyright") < 1 && TimeCount >= 0)
                                        {
                                            System.Threading.Thread.Sleep(100);//1000 = 1 second
                                            Application.DoEvents();
                                            TimeCount--;
                                        }
                                        System.Threading.Thread.Sleep(500);//1000 = 1 second
                                        Application.DoEvents();
                                        WriteMessageFirmware(txtSerialStr, true);
                                        c = txtSerialStr.IndexOf("Copyright (c)");
                                        b = txtSerialStr.IndexOf("Bootloader");
                                        if (b > 0)
                                        {
                                            labelFirmwareUpdate.Text = "Update Failed Bootloader Detected";
                                            Application.DoEvents();
                                        }
                                        else
                                        {
                                            if (c > 0)
                                            {
                                                labelFirmwareUpdate.Text = "Update Complete";
                                                txtSerialStr = "";
                                                SendADCPCommand("CDEFAULT");
                                                System.Threading.Thread.Sleep(500);
                                                Application.DoEvents();
                                                WriteMessageFirmware(txtSerialStr, true);
                                                SendADCPCommand("CSAVE");
                                                System.Threading.Thread.Sleep(500);
                                                Application.DoEvents();
                                                WriteMessageFirmware(txtSerialStr, true);
                                            }
                                            else
                                            {
                                                labelFirmwareUpdate.Text = "Update Failed";
                                            }
                                        }
                                        Application.DoEvents();
                                    }
                                    else
                                    {
                                        labelFirmwareUpdate.Text = "Program Flash Failed";
                                        Application.DoEvents();
                                    }
                                }
                                else
                                {
                                    labelFirmwareUpdate.Text = "Erase Flash Failed";
                                    Application.DoEvents();
                                }
                            }
                            else
                            {
                                labelFirmwareUpdate.Text = "Bootloader Not Running";
                                Application.DoEvents();
                            }

                        }
                        else
                        {
                            labelFirmwareUpdate.Text = "File Upload Failed";
                            Application.DoEvents();
                        }
                    }
                }
                else
                {
                    labelFirmwareUpdate.Text = "RQ3 not detected";
                    Application.DoEvents();
                }
            }
            else
            {
                labelFirmwareUpdate.Text = "ADCP Not Attached";
                Application.DoEvents();
            }
            txtSerialStr = "";
            txtSerialNewData = true;


            EnableButtons();

        }

        /*private void buttonFirmwareUpdate_Click(object sender, EventArgs e)
        {
            DisableButtons();

            labelFirmwareUpdate.Text = "Checking ADCP Attached";
            Application.DoEvents();
            txtSerialStr = "";
            BreakPort(_serialPort, 1000);
            System.Threading.Thread.Sleep(1000);//1000 = 1 second
            Application.DoEvents();
            WriteMessageFirmware(txtSerialStr, true);

            int co = txtSerialStr.IndexOf("Copyright");

            if (co > 0)
            {
                string FileName = "";
                bool ok = true;
                textBoxFirmware.Text = "";
                txtSerialStr = "";
                Application.DoEvents();

                Stream stream = null;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                string exceptionmessage;

                openFileDialog1.InitialDirectory = "";
                openFileDialog1.Filter = "bl files (*.bl)|*.bl|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                labelFirmwareUpdate.Text = "Select Upload File";
                Application.DoEvents();

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileName = openFileDialog1.SafeFileName;//.FileName;
                    try
                    {
                        if ((stream = openFileDialog1.OpenFile()) != null)
                        {
                            labelFirmwareUpdate.Text = "Uploading File";
                            Application.DoEvents();
                            if (UsingSerial)
                                XModemUpload(stream, openFileDialog1.FileName, true);
                            //else
                            //    EMACupload(stream, openFileDialog1.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionmessage = String.Format("Firmware Update Exception: {0}", ex.GetType().ToString());
                        WriteMessageFirmware(exceptionmessage, true);
                        ok = false;
                    }
                    if (stream != null)
                        stream.Close();
                }
                else
                {
                    ok = false;
                }

                int SD = 0;
                if (ok)
                {
                    labelFirmwareUpdate.Text = "Writing File To SD";
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(10000);//1000 = 1 second
                    Application.DoEvents();

                    textBoxFirmware.Text = txtSerialStr;// txtSerial.Text;
                    Application.DoEvents();

                    SD = textBoxFirmware.Text.IndexOf("File successfully written to SD");
                }

                if ((SD > 0) && ok)//upload succeeded
                {
                    labelFirmwareUpdate.Text = "Checking OS State";
                    Application.DoEvents();
                    txtSerialStr = "";

                    BreakPort(_serialPort, 1000);

                    System.Threading.Thread.Sleep(1000);//1000 = 1 second
                    Application.DoEvents();

                    WriteMessageFirmware(txtSerialStr, true);

                    int c = txtSerialStr.IndexOf("Copyright");

                    if (c > 0)//ADCP is attached
                    {
                        labelFirmwareUpdate.Text = "Enabling BootLoader";
                        Application.DoEvents();

                        int b = txtSerialStr.IndexOf("Bootloader");
                        if (b <= 0)//Bootloader is not running
                        {
                            txtSerialStr = "";
                            SendADCPCommand("BLJMPBL");
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(1000);//1000 = 1 second
                            Application.DoEvents();
                            WriteMessageFirmware(txtSerialStr, true);
                            b = txtSerialStr.IndexOf("Bootloader");
                        }
                        if (b > 0)//bootloader is running
                        {
                            labelFirmwareUpdate.Text = "Erasing Flash";
                            Application.DoEvents();

                            txtSerialStr = "";
                            SendADCPCommand("BLERASEFL 53A0E2D9");
                            System.Threading.Thread.Sleep(5000);//1000 = 1 second
                            Application.DoEvents();
                            WriteMessageFirmware(txtSerialStr, true);
                            b = txtSerialStr.IndexOf("Successfully erased Flash");

                            if (b > 0)//flash is erased
                            {
                                labelFirmwareUpdate.Text = "Programming Flash";
                                Application.DoEvents();
                                txtSerialStr = "";
                                SendADCPCommand("BLWRFL " + FileName + " 430000 0");
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(5000);//1000 = 1 second
                                Application.DoEvents();
                                WriteMessageFirmware(txtSerialStr, true);
                                b = txtSerialStr.IndexOf("Successfully programmed the Flash");
                                if (b > 0)
                                {
                                    labelFirmwareUpdate.Text = "Flash Programmed";
                                    //WriteMessageFirmware("\r\nPlease Cycle ADCP Power", true);
                                    //WriteMessageFirmware("\r\nVerify Proper Wakeup Message on Terminal\r\n", true);
                                    Application.DoEvents();

                                    labelFirmwareUpdate.Text = "Resetting System";
                                    Application.DoEvents();
                                    txtSerialStr = "";
                                    SendADCPCommand("BLRST");
                                    Application.DoEvents();
                                    System.Threading.Thread.Sleep(5000);//1000 = 1 second
                                    Application.DoEvents();
                                    WriteMessageFirmware(txtSerialStr, true);
                                    c = txtSerialStr.IndexOf("Copyright (c)");
                                    b = txtSerialStr.IndexOf("Bootloader");
                                    if (b > 0)
                                    {
                                        labelFirmwareUpdate.Text = "Update Failed Bootloader Detected";
                                        Application.DoEvents();
                                    }
                                    else
                                    {
                                        if (c > 0)
                                        {
                                            labelFirmwareUpdate.Text = "Update Complete";
                                        }
                                        else
                                        {
                                            labelFirmwareUpdate.Text = "Update Failed";
                                        }
                                    }
                                    Application.DoEvents();
                                }
                                else
                                {
                                    labelFirmwareUpdate.Text = "Program Flash Failed";
                                    Application.DoEvents();
                                }
                            }
                            else
                            {
                                labelFirmwareUpdate.Text = "Erase Flash Failed";
                                Application.DoEvents();
                            }
                        }
                        else
                        {
                            labelFirmwareUpdate.Text = "Bootloader Not Running";
                            Application.DoEvents();
                        }
                    }
                }
                else
                {
                    labelFirmwareUpdate.Text = "File Upload Failed";
                    Application.DoEvents();
                }
            }
            else
            {
                labelFirmwareUpdate.Text = "ADCP Not Attached";
                Application.DoEvents();
            }
            txtSerialStr = "";
            txtSerialNewData = true;


            EnableButtons();
        }
      */  //BackScatter----------------------------------------------------------------------
        private void radioButtonBSsystem_CheckedChanged(object sender, EventArgs e)
        {
            textBoxBSsystem.BringToFront(); 
        }

        private void radioButtonBSdata_CheckedChanged(object sender, EventArgs e)
        {
            textBoxBSdata.BringToFront();
        }

        private void radioButtonBSleaders_CheckedChanged(object sender, EventArgs e)
        {
            textBoxBSleaders.BringToFront();
        }
        int TheBSbeam = 0;
        private void textBoxBSbeam_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TheBSbeam = Convert.ToInt32(textBoxBSbeam.Text);
                textBoxBSprofile.Text = BackScatter.GetBsProfileString(TheBSbeam, BackScatter.Ensemble);
            }
            catch { }
        }

        private void checkBoxShowVTGspeed_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxShowVTGspeed.Checked)
            {
                if (Application.OpenForms["View"] != null)
                {
                    //form already open
                }
                else//form isn't open
                {
                    string str = "VTG Speed";
                    RoweTech.View dummy = new RoweTech.View(str);//, x, y);
                    RoweTech.View.IniVar.StrToTheTop = "Top";
                    RoweTech.View.IniVar.BadFlag = true;

                    dummy.Show();

                    //dummy.Location = new Point(x, y);
                }
            }
            else
            {
                Application.OpenForms["View"]?.Close();
            }
        }

        private void radioButtonBSprofile_CheckedChanged(object sender, EventArgs e)
        {
            textBoxBSprofile.BringToFront();
        }
        //--------------------------------------------------------------------------------
      
            
    }
}
