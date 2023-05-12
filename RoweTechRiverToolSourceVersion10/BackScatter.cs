using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RoweTechBackScatter
{
    class BackScatter
    {
        public const int SYSTEM_CHECKSUM = 0;
        public const int SYSTEM_LEADER = 1;

        //public const int VERTICAL_STAGE = 2;
        //public const int HORIZONTAL_JANUS_LEADER = 3;
        //public const int HORIZONTAL_JANUS_GOOD = 4;
        //public const int HORIZONTAL_JANUS_BEAM = 5;
        //public const int HORIZONTAL_JANUS_INSTRUMENT = 6;
        //public const int HORIZONTAL_JANUS_AMPLITUDE = 7;
        //public const int HORIZONTAL_JANUS_CORRELATION = 8;

        public const int BACKSCATTER_LEADER_00 = 9;
        public const int BACKSCATTER_PROFILE_00 = 10;
        public const int BACKSCATTER_LEADER_01 = 11;
        public const int BACKSCATTER_PROFILE_01 = 12;
        public const int BACKSCATTER_LEADER_02 = 13;
        public const int BACKSCATTER_PROFILE_02 = 14;
        public const int BACKSCATTER_LEADER_03 = 15;
        public const int BACKSCATTER_PROFILE_03 = 16;
        public const int BACKSCATTER_LEADER_04 = 17;
        public const int BACKSCATTER_PROFILE_04 = 18;

        public const int BACKSCATTER_LEADER_05 = 19;
        public const int BACKSCATTER_PROFILE_05 = 20;
        public const int BACKSCATTER_LEADER_06 = 21;
        public const int BACKSCATTER_PROFILE_06 = 22;
        public const int BACKSCATTER_LEADER_07 = 23;
        public const int BACKSCATTER_PROFILE_07 = 24;
        public const int BACKSCATTER_LEADER_08 = 25;
        public const int BACKSCATTER_PROFILE_08 = 26;
        public const int BACKSCATTER_LEADER_09 = 27;
        public const int BACKSCATTER_PROFILE_09 = 28;

        public const int AMPLITUDE_PROFILE_00 = 29;
        public const int AMPLITUDE_PROFILE_01 = 30;
        public const int AMPLITUDE_PROFILE_02 = 31;
        public const int AMPLITUDE_PROFILE_03 = 32;
        public const int AMPLITUDE_PROFILE_04 = 33;
        public const int AMPLITUDE_PROFILE_05 = 34;
        public const int AMPLITUDE_PROFILE_06 = 35;
        public const int AMPLITUDE_PROFILE_07 = 36;
        public const int AMPLITUDE_PROFILE_08 = 37;
        public const int AMPLITUDE_PROFILE_09 = 38;

        public static string[] DataTypeName = {
                "System Checksum"
                ,"System Leader"
                ,"Stage"
                ,"Janus Leader"
                ,"Janus Beam Good"
                ,"Janus Beam Velocity"
                ,"Janus XY Velocity"
                ,"Janus Beam Amplitude"
                ,"Janus Beam Correlation"
                ,"Backscatter Leader 0"
                ,"Backscatter 0"
                ,"Backscatter Leader 1"
                ,"Backscatter 1"
                ,"Backscatter Leader 2"
                ,"Backscatter 2"
                ,"Backscatter Leader 3"
                ,"Backscatter 3"
                ,"Backscatter Leader 4"
                ,"Backscatter 4"
                ,"Backscatter Leader 5"
                ,"Backscatter 5"
                ,"Backscatter Leader 6"
                ,"Backscatter 6"
                ,"Backscatter Leader 7"
                ,"Backscatter 7"
                ,"Backscatter Leader 8"
                ,"Backscatter 8"
                ,"Backscatter Leader 9"
                ,"Backscatter 9"
                ,"Amplitude 0"
                ,"Amplitude 1"
                ,"Amplitude 2"
                ,"Amplitude 3"
                ,"Amplitude 4"
                ,"Amplitude 5"
                ,"Amplitude 6"
                ,"Amplitude 7"
                ,"Amplitude 8"
                ,"Amplitude 9"
        };
        public const int MaxDataTypes = 42;
        public static bool[] DataTypeAvailable = new bool[MaxDataTypes];

        public const int MaxBeams = 1;
        public const int MaxBins = 300;

        public static int DataBuffReadIndex = 0;
        public static int DataBuffWriteIndex = 0;

        public const int MaxBuff = 1400000;
        public static int MaxDataIndex = MaxBuff - 1;

        public static uint PacketPointer = 0;

        public const int MaxBSbeams = 10;
        public class EnsembleClass
        {
            public int MostBins;

            //Header Data
            public ulong Header_Type;
            public ushort Header_PayloadSize;

            //Leader Data
            public ushort System_ID;
            public ushort System_Bytes;
            public uint System_EnsembleNumber;
            public byte[] System_SN = new byte[32];

            public ushort System_FW_MAJOR;
            public ushort System_FW_MINOR;
            public ushort System_FW_REVISION;

            public ushort System_Year;
            public byte System_Month;
            public byte System_Day;
            public byte System_Hour;
            public byte System_Minute;
            public byte System_Second;
            public byte System_Hsec;
            public double System_Latitude;
            public double System_Longitude;
            public float System_DeployDepth;
            public float System_DeployHeight;
            public ushort System_RightBank;
            public float System_Heading;
            public float System_Pitch;
            public float System_Roll;
            public float System_Salinity;
            public float System_Temperature;
            public float System_BackPlaneTemperature;
	        public float System_HeatSink1Temperature;
            public float System_Pressure;
            public float System_SpeedOfSound;
            public ushort System_Status;
            public ushort System_Status2;

            

            //Backscatter Leader Data
            public ushort[] BS_ID = new ushort[MaxBSbeams];
            public ushort[] BS_Bytes = new ushort[MaxBSbeams];

            public float[] BS_Frequency = new float[MaxBSbeams];

            public float[] BS_Diameter = new float[MaxBSbeams];
            public float[] BS_BeamAngle = new float[MaxBSbeams];

            public float[] BS_Rcvr1Temperature = new float[MaxBSbeams];
            public float[] BS_Rcvr2Temperature = new float[MaxBSbeams];
            public float[] BS_TransmitVolts = new float[MaxBSbeams];

            public float[] BS_Gain = new float[MaxBSbeams];
            public float[] BS_TransmitBandwidth = new float[MaxBSbeams];
            public float[] BS_ReceiveBandwidth = new float[MaxBSbeams];

            public float[] BS_SampleFrequency = new float[MaxBSbeams];

            public ushort[] BS_LagSamples = new ushort[MaxBSbeams];
            public ushort[] BS_CyclePerElement = new ushort[MaxBSbeams];
            public ushort[] BS_NumberOfElements = new ushort[MaxBSbeams];
            public ushort[] BS_NumberOfRepeats = new ushort[MaxBSbeams];

            public ushort[] BS_Pings = new ushort[MaxBSbeams];
            public ushort[] BS_Beams = new ushort[MaxBSbeams];
            public ushort[] BS_Bins = new ushort[MaxBSbeams];
            public float[] BS_FirstBin = new float[MaxBSbeams];
            public float[] BS_BinSize = new float[MaxBSbeams];

            public float[] BS_VolBegin = new float [MaxBSbeams];
            public float[] BS_VolEnd = new float[MaxBSbeams];
            public float[] BS_VolAmp = new float[MaxBSbeams];
            public float[] BS_WPVOLthreshold = new float[MaxBSbeams];
            public ushort[] BS_Voln = new ushort[MaxBSbeams];
            public float[] BS_SL = new float[MaxBSbeams];
            public float[] BS_RS = new float[MaxBSbeams];

            public float[] BS_Direction = new float[MaxBSbeams];
            public float[] BS_NoiseFactor = new float[MaxBSbeams];
            public float[] BS_Watts = new float[MaxBSbeams];
            public float[] BS_Efficiency = new float[MaxBSbeams];
            public float[] BS_NoiseBandwidth = new float[MaxBSbeams];
            public float[] BS_XmtLength = new float[MaxBSbeams];
            public float[] BS_Blank = new float[MaxBSbeams];

            

            //Backscatter_0 Amplitude Data
            public ushort[] BS_Amplitude_ID = new ushort[MaxBSbeams];
            public ushort[] BS_Amplitude_Bytes = new ushort[MaxBSbeams];
            public float[,] BS_Amplitude = new float[MaxBSbeams, MaxBins];
            public float[] BS_NoiseAmplitude = new float[MaxBSbeams];

            public ushort[] BS_BackScatter_ID = new ushort[MaxBSbeams];
            public ushort[] BS_BackScatter_Bytes = new ushort[MaxBSbeams];
            public float[,] BS_BackScatter = new float[MaxBSbeams, MaxBins];

            public bool SystemAvailable;
        }

        public static EnsembleClass Ensemble = new EnsembleClass();

        [StructLayout(LayoutKind.Explicit)]

        public struct TestUnion
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
            public short Short;
            [FieldOffset(0)]
            public float Float;
            [FieldOffset(0)]
            public int Int;
            [FieldOffset(0)]
            public long Long;
            [FieldOffset(0)]
            public double Double;
        }

        public static TestUnion ByteArrayToNumber;

        public static int ByteArrayToInt(byte[] packet)
        {
            ByteArrayToNumber.A = packet[PacketPointer++];
            ByteArrayToNumber.B = packet[PacketPointer++];
            ByteArrayToNumber.C = packet[PacketPointer++];
            ByteArrayToNumber.D = packet[PacketPointer++];

            return ByteArrayToNumber.Int;
        }
        public static short ByteArrayToShort(byte[] packet)
        {
            ByteArrayToNumber.A = packet[PacketPointer++];
            ByteArrayToNumber.B = packet[PacketPointer++];

            return ByteArrayToNumber.Short;
        }
        public static long ByteArrayToLong(byte[] packet)
        {
            ByteArrayToNumber.A = packet[PacketPointer++];
            ByteArrayToNumber.B = packet[PacketPointer++];
            ByteArrayToNumber.C = packet[PacketPointer++];
            ByteArrayToNumber.D = packet[PacketPointer++];
            ByteArrayToNumber.E = packet[PacketPointer++];
            ByteArrayToNumber.F = packet[PacketPointer++];
            ByteArrayToNumber.G = packet[PacketPointer++];
            ByteArrayToNumber.H = packet[PacketPointer++];

            return ByteArrayToNumber.Long;
        }
        public static string ByteArrayToString(byte[] packet, int len)
        {
            string s = "";
            int i;
            for (i = 0; i < len; i++)
            {
                s += (char)packet[PacketPointer++];
            }
            return s;
        }
        public static float ByteArrayToFloat(byte[] packet)
        {
            ByteArrayToNumber.A = packet[PacketPointer++];
            ByteArrayToNumber.B = packet[PacketPointer++];
            ByteArrayToNumber.C = packet[PacketPointer++];
            ByteArrayToNumber.D = packet[PacketPointer++];

            return ByteArrayToNumber.Float;
        }
        public static double ByteArrayToDouble(byte[] packet)
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

        public static uint ExtractLeader(byte[] packet, EnsembleClass Ensemble, int bm)
        {
            Ensemble.BS_Bytes[bm] = (ushort)ByteArrayToShort(packet);
            uint NextID = PacketPointer + Ensemble.BS_Bytes[bm];

            Ensemble.BS_Frequency[bm] = ByteArrayToFloat(packet);
            Ensemble.BS_Diameter[bm] = (float)ByteArrayToShort(packet) / 1000;
            Ensemble.BS_BeamAngle[bm] = (float)ByteArrayToShort(packet) / 1000;

            Ensemble.BS_Rcvr1Temperature[bm] = (float)ByteArrayToShort(packet) / 100;
            Ensemble.BS_Rcvr2Temperature[bm] = (float)ByteArrayToShort(packet) / 100;
            Ensemble.BS_TransmitVolts[bm] = (float)ByteArrayToShort(packet) / 100;

            Ensemble.BS_Gain[bm] = (float)ByteArrayToShort(packet) / 1000;
            Ensemble.BS_TransmitBandwidth[bm] = (float)ByteArrayToShort(packet) / 65535;
            Ensemble.BS_ReceiveBandwidth[bm] = (float)ByteArrayToShort(packet) / 65535;

            Ensemble.BS_SampleFrequency[bm] = ByteArrayToFloat(packet);

            Ensemble.BS_LagSamples[bm] = (ushort)ByteArrayToShort(packet);
            Ensemble.BS_CyclePerElement[bm] = (ushort)ByteArrayToShort(packet);
            Ensemble.BS_NumberOfElements[bm] = (ushort)ByteArrayToShort(packet);
            Ensemble.BS_NumberOfRepeats[bm] = (ushort)ByteArrayToShort(packet);

            Ensemble.BS_Pings[bm] = (ushort)ByteArrayToShort(packet);
            Ensemble.BS_Beams[bm] = (ushort)ByteArrayToShort(packet);
            if (Ensemble.BS_Beams[bm] > MaxBeams)
                Ensemble.BS_Beams[bm] = MaxBeams;
            Ensemble.BS_Bins[bm] = (ushort)ByteArrayToShort(packet);
            if (Ensemble.BS_Bins[bm] > MaxBins)
                Ensemble.BS_Bins[bm] = MaxBins;
            if (Ensemble.MostBins < Ensemble.BS_Bins[bm])
                Ensemble.MostBins = Ensemble.BS_Bins[bm];
            Ensemble.BS_FirstBin[bm] = (float)ByteArrayToShort(packet) / 1000;
            Ensemble.BS_BinSize[bm] = (float)ByteArrayToShort(packet) / 1000;

            Ensemble.BS_VolBegin[bm] = (float)ByteArrayToShort(packet) / 1000;
            Ensemble.BS_VolEnd[bm] = (float)ByteArrayToShort(packet) / 1000;
            Ensemble.BS_VolAmp[bm] = (float)ByteArrayToShort(packet) / 100;
            Ensemble.BS_WPVOLthreshold[bm] = (float)ByteArrayToShort(packet) / 100;
            Ensemble.BS_Voln[bm] = (ushort)ByteArrayToShort(packet);
            Ensemble.BS_SL[bm] = (float)ByteArrayToShort(packet) / 100;
            Ensemble.BS_RS[bm] = (float)ByteArrayToShort(packet) / 100;

            Ensemble.BS_Direction[bm] = (float)ByteArrayToShort(packet) / 100;
            Ensemble.BS_NoiseFactor[bm] = (float)ByteArrayToShort(packet) / 1000;
            Ensemble.BS_Watts[bm] = (float)ByteArrayToShort(packet) / 1000;
            Ensemble.BS_Efficiency[bm] = (float)ByteArrayToShort(packet) / 1000;
            Ensemble.BS_NoiseBandwidth[bm] = (float)ByteArrayToShort(packet) / 1000;
            Ensemble.BS_XmtLength[bm] = (float)ByteArrayToShort(packet) / 1000;
            Ensemble.BS_Blank[bm] = (float)ByteArrayToShort(packet) / 1000;

            return NextID;
        }
        public static uint ExtractAmpProfile(byte[] packet, EnsembleClass Ensemble, int bm)
        {
            Ensemble.BS_Amplitude_Bytes[bm] = (ushort)ByteArrayToShort(packet);
            uint NextID = PacketPointer + Ensemble.BS_Amplitude_Bytes[bm];
            for (int bin = 0; bin < Ensemble.BS_Bins[bm]; bin++)
            {
                Ensemble.BS_Amplitude[bm, bin] = (float)ByteArrayToShort(packet) / 256;
            }
            return NextID;
        }
        public static uint ExtractBackScattterProfile(byte[] packet, EnsembleClass Ensemble, int bm)
        {
            Ensemble.BS_BackScatter_Bytes[bm] = (ushort)ByteArrayToShort(packet);
            uint NextID = PacketPointer + Ensemble.BS_BackScatter_Bytes[bm];
            for (int bin = 0; bin < Ensemble.BS_Bins[bm]; bin++)
            {
                Ensemble.BS_BackScatter[bm, bin] = (float)ByteArrayToShort(packet) / 256;
            }
            Ensemble.BS_NoiseAmplitude[bm] = (float)ByteArrayToShort(packet) / 256;
            return NextID;
        }

        public static void DecodeEnsemble(byte[] packet, EnsembleClass Ensemble)
        {
            int i, bm;

            for(i=0;i< MaxDataTypes; i++)
                DataTypeAvailable[i] = false;

            //init just in case the ID are out of order
            for (i = 0; i < MaxBSbeams; i++)
            {
                Ensemble.BS_Beams[i] = 0;// MaxBeams;
                Ensemble.BS_Bins[i] = 0;// MaxBins;
            }

            Ensemble.SystemAvailable = false;
            Ensemble.MostBins = 0;

            PacketPointer = 0;//point to first byte of the data buffer

            //get Header
            Ensemble.Header_Type = (ulong)ByteArrayToLong(packet);
            Ensemble.Header_PayloadSize = (ushort)ByteArrayToShort(packet);
            PacketPointer += 2;

            //decode the linked list
            uint PayloadStart = PacketPointer;
            uint NextID = PacketPointer;
            bool done = false;
            ushort TempBytes;
            while (!done)
            {
                PacketPointer = NextID;
                ushort ID = (ushort)ByteArrayToShort(packet);

                if(ID < MaxDataTypes)
                    DataTypeAvailable[ID] = true;

                switch (ID)
                {
                    case SYSTEM_CHECKSUM:
                        done = true;
                        break;
                    default://unknown ID
                        TempBytes = (ushort)ByteArrayToShort(packet);
                        NextID = PacketPointer + TempBytes;
                        break;
                    case SYSTEM_LEADER:
                        Ensemble.SystemAvailable = true;

                        Ensemble.System_ID = ID;
                        Ensemble.System_Bytes = (ushort)ByteArrayToShort(packet);
                        NextID = PacketPointer + Ensemble.System_Bytes;

                        Ensemble.System_EnsembleNumber = (uint)ByteArrayToInt(packet);

                        for (i = 0; i < 32; i++)
                        {
                            Ensemble.System_SN[i] = (byte)packet[PacketPointer++];
                        }

                        Ensemble.System_FW_MAJOR = (ushort)ByteArrayToShort(packet);
                        Ensemble.System_FW_MINOR = (ushort)ByteArrayToShort(packet);
                        Ensemble.System_FW_REVISION = (ushort)ByteArrayToShort(packet);

                        Ensemble.System_Year = (ushort)ByteArrayToShort(packet);
                        Ensemble.System_Month = (byte)packet[PacketPointer++];
                        Ensemble.System_Day = (byte)packet[PacketPointer++];
                        Ensemble.System_Hour = (byte)packet[PacketPointer++];
                        Ensemble.System_Minute = (byte)packet[PacketPointer++];
                        Ensemble.System_Second = (byte)packet[PacketPointer++];
                        Ensemble.System_Hsec = (byte)packet[PacketPointer++];

                        Ensemble.System_Latitude = ByteArrayToDouble(packet);
                        Ensemble.System_Longitude = ByteArrayToDouble(packet);
                        Ensemble.System_DeployDepth = ByteArrayToFloat(packet);
                        Ensemble.System_DeployHeight = ByteArrayToFloat(packet);

                        Ensemble.System_RightBank = (ushort)ByteArrayToShort(packet);

                        Ensemble.System_Heading = (float)((ushort)ByteArrayToShort(packet)) / 100;
                        Ensemble.System_Pitch = (float)ByteArrayToShort(packet) / 100;
                        Ensemble.System_Roll = (float)ByteArrayToShort(packet) / 100;
                        Ensemble.System_Salinity = (float)ByteArrayToShort(packet) / 100;
                        Ensemble.System_Temperature = (float)ByteArrayToShort(packet) / 100;
                        Ensemble.System_BackPlaneTemperature = (float)ByteArrayToShort(packet) / 100;
                        Ensemble.System_HeatSink1Temperature = (float)ByteArrayToShort(packet) / 100;
                        Ensemble.System_Pressure = ByteArrayToFloat(packet);
                        Ensemble.System_SpeedOfSound = ByteArrayToFloat(packet);
                        Ensemble.System_Status = (ushort)ByteArrayToShort(packet);
                        Ensemble.System_Status2 = (ushort)ByteArrayToShort(packet);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_LEADER_00:
                        bm = 0;
                        Ensemble.BS_ID[bm] = ID;
                        NextID = ExtractLeader(packet,Ensemble,bm);
                        PacketPointer = NextID;
                        break;
                    case AMPLITUDE_PROFILE_00:
                        bm = 0;
                        Ensemble.BS_Amplitude_ID[bm] = ID;
                        NextID = ExtractAmpProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_LEADER_01:
                        bm = 1;
                        Ensemble.BS_ID[bm] = ID;
                        NextID = ExtractLeader(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case AMPLITUDE_PROFILE_01:
                        bm = 1;
                        Ensemble.BS_Amplitude_ID[bm] = ID;
                        NextID = ExtractAmpProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_LEADER_02:
                        bm = 2;
                        Ensemble.BS_ID[bm] = ID;
                        NextID = ExtractLeader(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case AMPLITUDE_PROFILE_02:
                        bm = 2;
                        NextID = ExtractAmpProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_LEADER_03:
                        bm = 3;
                        Ensemble.BS_ID[bm] = ID;
                        NextID = ExtractLeader(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case AMPLITUDE_PROFILE_03:
                        bm = 3;
                        NextID = ExtractAmpProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_LEADER_04:
                        bm = 4;
                        Ensemble.BS_ID[bm] = ID;
                        NextID = ExtractLeader(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case AMPLITUDE_PROFILE_04:
                        bm = 4;
                        NextID = ExtractAmpProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_LEADER_05:
                        bm = 5;
                        Ensemble.BS_ID[bm] = ID;
                        NextID = ExtractLeader(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case AMPLITUDE_PROFILE_05:
                        bm = 5;
                        NextID = ExtractAmpProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_LEADER_06:
                        bm = 6;
                        Ensemble.BS_ID[bm] = ID;
                        NextID = ExtractLeader(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case AMPLITUDE_PROFILE_06:
                        bm = 6;
                        NextID = ExtractAmpProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_LEADER_07:
                        bm = 7;
                        Ensemble.BS_ID[bm] = ID;
                        NextID = ExtractLeader(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case AMPLITUDE_PROFILE_07:
                        bm = 7;
                        NextID = ExtractAmpProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_LEADER_08:
                        bm = 8;
                        Ensemble.BS_ID[bm] = ID;
                        NextID = ExtractLeader(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case AMPLITUDE_PROFILE_08:
                        bm = 8;
                        NextID = ExtractAmpProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_LEADER_09:
                        bm = 9;
                        Ensemble.BS_ID[bm] = ID;
                        NextID = ExtractLeader(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case AMPLITUDE_PROFILE_09:
                        bm = 9;
                        NextID = ExtractAmpProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_PROFILE_00:
                        bm = 0;
                        Ensemble.BS_BackScatter_ID[bm] = ID;
                        NextID = ExtractBackScattterProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_PROFILE_01:
                        bm = 1;
                        NextID = ExtractBackScattterProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_PROFILE_02:
                        bm = 2;
                        NextID = ExtractBackScattterProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_PROFILE_03:
                        bm = 3;
                        NextID = ExtractBackScattterProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_PROFILE_04:
                        bm = 4;
                        NextID = ExtractBackScattterProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_PROFILE_05:
                        bm = 5;
                        NextID = ExtractBackScattterProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_PROFILE_06:
                        bm = 6;
                        NextID = ExtractBackScattterProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_PROFILE_07:
                        bm = 7;
                        NextID = ExtractBackScattterProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_PROFILE_08:
                        bm = 8;
                        NextID = ExtractBackScattterProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                    case BACKSCATTER_PROFILE_09:
                        bm = 9;
                        NextID = ExtractBackScattterProfile(packet, Ensemble, bm);
                        PacketPointer = NextID;
                        break;
                }
                if (NextID > PayloadStart + Ensemble.Header_PayloadSize)
                    done = true;
                else
                     PacketPointer = NextID;
            }
        }

        public static string GetHeaderString(BackScatter.EnsembleClass m)
        {
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                s += (char)((0x0ff & (m.Header_Type >> 8 * i)));
            }
            s += "\r\n";
            //AddSpaces(m.System_Heading.ToString("F2"), 8);
            s += "Wrapper:" + AddSpaces("14", 8) + " bytes\r\n";
            s += "Payload:" + AddSpaces(m.Header_PayloadSize.ToString(),8) + " bytes\r\n";
            s += "Total  :" + AddSpaces((m.Header_PayloadSize+14).ToString(), 8) + " bytes";
            s += "\r\n";

            return s;
        }
        static string AddSpaces(string str, int n)
        {
            string s = str;

            for (int i = 0; i < n - str.Length; i++)
            {
                s = " " + s;
            }

            return s;
        }
        public static string GetSystemStringA(BackScatter.EnsembleClass m)
        {
            string s = "";

            if (m.SystemAvailable)
            {
                /*
                 //Leader Data
                    public ushort System_ID;
                    public ushort System_Bytes;
                    public uint System_EnsembleNumber;
                    public byte[] System_SN = new byte[32];

                    public ushort System_FW_MAJOR;
                    public ushort System_FW_MINOR;
                    public ushort System_FW_REVISION;

                    public ushort System_Year;
                    public byte System_Month;
                    public byte System_Day;
                    public byte System_Hour;
                    public byte System_Minute;
                    public byte System_Second;
                    public byte System_Hsec;

                    public double System_Latitude;
                    public double System_Longitude;
                    public float System_DeployDepth;
                    public float System_DeployHeight;
                    public ushort System_RightBank;
                    public float System_Heading;
                    public float System_Pitch;
                    public float System_Roll;

                    public float System_Salinity;
                    public float System_Temperature;
                    public float System_BackPlaneTemperature;
	                public float System_HeatSink1Temperature;
                    public float System_Pressure;
                    public float System_SpeedOfSound;
                    
                public ushort System_Status;
                    public ushort System_Status2;
                    */

                //s += m.System_ID.ToString("0");
                //s += "\r\n";

                //s += m.System_Bytes.ToString("0");
                //s += "\r\n";

                s += "Ens " + m.System_EnsembleNumber.ToString("0");
                s += "\r\n";

                s += "SN:";
                for (int i = 0; i < 32; i++)
                {
                    s += (char)m.System_SN[i];
                }
                s += "\r\n";
                s += "Firmware:";
                s += m.System_FW_MAJOR.ToString("D2") + ".";
                s += m.System_FW_MINOR.ToString("D2") + ".";
                s += m.System_FW_REVISION.ToString("D2");
                s += "\r\n";


                /*
                public double System_Latitude;
                public double System_Longitude;
                public float System_DeployDepth;
                public float System_DeployHeight;
                public ushort System_RightBank;*/

        s += m.System_Year.ToString("D4") + "/" + m.System_Month.ToString("D2") + "/" + m.System_Day.ToString("D2");
                s += ",";
                s += m.System_Hour.ToString("D2") + ":" + m.System_Minute.ToString("D2") + ":" + m.System_Second.ToString("D2") + "." + m.System_Hsec.ToString("D2");
                s += "\r\n";

                s += "Heading  (deg)" + AddSpaces(m.System_Heading.ToString("F2"), 8);
                s += "\r\n";
                s += "Pitch    (deg)" + AddSpaces(m.System_Pitch.ToString("F2"), 8);
                s += "\r\n";
                s += "Roll     (deg)" + AddSpaces(m.System_Roll.ToString("F2"), 8);
                s += "\r\n";

            }
            return s;
        }
        public static string GetSystemStringB(BackScatter.EnsembleClass m)
        {
            string s = "";

            if (m.SystemAvailable)
            {
                /*
                 //Leader Data
                    public ushort System_ID;
                    public ushort System_Bytes;
                    public uint System_EnsembleNumber;
                    public byte[] System_SN = new byte[32];

                    public ushort System_FW_MAJOR;
                    public ushort System_FW_MINOR;
                    public ushort System_FW_REVISION;

                    public ushort System_Year;
                    public byte System_Month;
                    public byte System_Day;
                    public byte System_Hour;
                    public byte System_Minute;
                    public byte System_Second;
                    public byte System_Hsec;

                    public double System_Latitude;
                    public double System_Longitude;
                    public float System_DeployDepth;
                    public float System_DeployHeight;
                    public ushort System_RightBank;
                    public float System_Heading;
                    public float System_Pitch;
                    public float System_Roll;

                    public float System_Salinity;
                    public float System_Temperature;
                    public float System_BackPlaneTemperature;
	                public float System_HeatSink1Temperature;
                    public float System_Pressure;
                    public float System_SpeedOfSound;
                    
                public ushort System_Status;
                    public ushort System_Status2;
                    */

                //s += m.System_ID.ToString("0");
                //s += "\r\n";

                //s += m.System_Bytes.ToString("0");
                //s += "\r\n";

                

                s += "Salinity (ppt)";
                s += AddSpaces(m.System_Salinity.ToString("F2"), 8);
                s += "\r\n";
                s += "Water    (C)  ";
                s += AddSpaces(m.System_Temperature.ToString("F2"), 8);
                s += "\r\n";
                s += "Internal (C)  ";
                s += AddSpaces(m.System_BackPlaneTemperature.ToString("F2"), 8);
                s += "\r\n";
                s += "Heatsink (C)  ";
                s += AddSpaces(m.System_HeatSink1Temperature.ToString("F2"), 8);
                s += "\r\n";
                s += "Pressure (Bar)";
                s += AddSpaces(m.System_Pressure.ToString("F7"), 13);
                s += "\r\n";
                s += "SofS     (m/s)";
                s += AddSpaces(m.System_SpeedOfSound.ToString("F2"), 8);
                s += "\r\n";
                s += "Status 0x";
                s += m.System_Status.ToString("X04");
                s += ",0x";
                s += m.System_Status2.ToString("X04");
                s += "\r\n";
            }
            return s;
        }
        public static string GetSystemString(BackScatter.EnsembleClass m)
        {
            string s = "";

            if (m.SystemAvailable)
            {
                s += "SYSTEM\r\n";

                s += "Ens " + m.System_EnsembleNumber.ToString("0");
                s += "\r\n";
                s += "\r\n";

                s += "SN:";
                for (int i = 0; i < 32; i++)
                {
                    s += (char)m.System_SN[i];
                }
                s += "\r\n";
                s += "\r\n";
                s += "Firmware:";
                s += m.System_FW_MAJOR.ToString("D2") + ".";
                s += m.System_FW_MINOR.ToString("D2") + ".";
                s += m.System_FW_REVISION.ToString("D2");
                s += "\r\n";
                s += "\r\n";

                s += m.System_Year.ToString("D4") + "/" + m.System_Month.ToString("D2") + "/" + m.System_Day.ToString("D2");
                s += ",";
                s += m.System_Hour.ToString("D2") + ":" + m.System_Minute.ToString("D2") + ":" + m.System_Second.ToString("D2") + "." + m.System_Hsec.ToString("D2");
                s += "\r\n";
                s += "\r\n";

                s += "Latitude(deg) " + AddSpaces(m.System_Latitude.ToString("F7"), 13);
                s += "\r\n";
                s += "Longitude(deg)" + AddSpaces(m.System_Longitude.ToString("F7"), 13);
                s += "\r\n";
                s += "\r\n";
                s += "Deploy Depth(m) " + AddSpaces(m.System_DeployDepth.ToString("F3"), 4);
                s += "\r\n";
                //s += "Deploy Height(m)" + AddSpaces(m.System_DeployHeight.ToString("F3"), 4);
                //s += "\r\n";
                //s += "\r\n";
                //s += "Right Bank" + AddSpaces(m.System_RightBank.ToString(), 2);
                //s += "\r\n";
                s += "\r\n";
                s += "Heading(deg)" + AddSpaces(m.System_Heading.ToString("F2"), 7);
                s += "\r\n";
                s += "Pitch(deg)  " + AddSpaces(m.System_Pitch.ToString("F2"), 7);
                s += "\r\n";
                s += "Roll(deg)   " + AddSpaces(m.System_Roll.ToString("F2"), 7);
                s += "\r\n";
                s += "\r\n";
                s += "Salinity(ppt)   " + AddSpaces(m.System_Salinity.ToString("F2"), 8);
                s += "\r\n";
                s += "Water Temp(C)   " + AddSpaces(m.System_Temperature.ToString("F2"), 8);
                s += "\r\n";
                s += "Internal Temp(C)";
                s += AddSpaces(m.System_BackPlaneTemperature.ToString("F2"), 8);
                s += "\r\n";
                s += "Heatsink Temp(C)" + AddSpaces(m.System_HeatSink1Temperature.ToString("F2"), 8);
                s += "\r\n";
                s += "Pressure(Bar)   " + AddSpaces(m.System_Pressure.ToString("F7"), 13);
                s += "\r\n";
                s += "\r\n";
                s += "Speed of Sound(m/s)" + AddSpaces(m.System_SpeedOfSound.ToString("F2"), 8);
                s += "\r\n";
                s += "\r\n";
                s += "Status 0x" + m.System_Status.ToString("X04") + ",0x" + m.System_Status2.ToString("X04");
                s += "\r\n";
            }
            return s;
        }
        public static string GetVolumeAmplitudeString(BackScatter.EnsembleClass m)
        {
            string s;
            /*
            public float BS0_Diameter;
            public float BS0_BeamAngle;

            public float BS0_Rcvr1Temperature;
            public float BS0_Rcvr2Temperature;

            public float BS0_Gain;
            public float BS0_TransmitBandwidth;
            public float BS0_ReceiveBandwidth;

            public float BS0_SampleFrequency;
            */

            double XmtLen;
            double Xmtmsec;

            /*
            s = "Volume Velocity\r\n";
            s += "  kHz Ang(deg)  XmtV Xmt(m) (msec) SL(dB) Pings Good Beg(m) End(m) Th(dB) S0(dB) N0(dB) SL(dB) Pings Good Beg(m) End(m) Th(dB) S1(dB) N1(dB) VX(m/s) VY(m/s)\r\n";

            if (m.JanusInstrumentVelocityAvailable)
            {
                s += AddSpaces(((int)m.Janus_Frequency / 1000).ToString(), 5);
                s += AddSpaces(((int)m.Janus_BeamAngle).ToString("F1"), 8);

                s += AddSpaces((m.Janus_TransmitVolts).ToString("F2"), 7);

                float cycles = (m.Janus_CyclePerElement * m.Janus_NumberOfElements * m.Janus_NumberOfRepeats);
                double MetersPerCarrierVertical = Math.Cos(Math.PI * m.Janus_BeamAngle/180) * (m.System_SpeedOfSound / 2.0) / m.Janus_Frequency;

                XmtLen = cycles * MetersPerCarrierVertical;
                s += AddSpaces((XmtLen).ToString("F3"), 7);
                
                Xmtmsec = 1000.0 * cycles / m.Janus_Frequency;
                s += AddSpaces((Xmtmsec).ToString("F3"), 7);

                for (i = 0; i < m.Janus_Beams; i++)
                {
                    s += AddSpaces(m.BS0_SL[i].ToString("F2"), 7);
                    s += AddSpaces(m.Janus_Pings.ToString(), 6);
                    s += AddSpaces(m.Janus_Voln[i].ToString(), 5);
                    s += AddSpaces(m.Janus_VolBegin[i].ToString("F1"), 7);
                    s += AddSpaces(m.Janus_VolEnd[i].ToString("F1"), 7);
                    s += AddSpaces(m.Janus_VolThres.ToString("F1"), 7);
                    s += AddSpaces(m.Janus_VolAmp[i].ToString("F1"), 7);
                    s += AddSpaces(m.Janus_NoiseAmplitude[i].ToString("F1"), 7);
                }
                s += AddSpaces(m.Janus_VolInst[0].ToString("F3"), 8);
                s += AddSpaces(m.Janus_VolInst[1].ToString("F3"), 8);

                s += "\r\n";
            }
            */
            s = "Volume Amplitude\r\n";
            s += "  kHz Ang(deg)  XmtV Xmt(m) (msec) SL(dB) RS(dB) Pings Good Beg(m) End(m) Th(dB) S0(dB) N0(dB)\r\n";

            for (int bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0)
                {
                    s += AddSpaces(((int)m.BS_Frequency[bm] / 1000).ToString(), 5);
                    s += AddSpaces(((int)m.BS_BeamAngle[bm]).ToString("F1"), 8);

                    s += AddSpaces((m.BS_TransmitVolts[bm]).ToString("F2"), 7);

                    float cycles = (m.BS_CyclePerElement[bm] * m.BS_NumberOfElements[bm] * m.BS_NumberOfRepeats[bm]);

                    double MetersPerCarrierVertical = Math.Cos(Math.PI * m.BS_BeamAngle[bm] / 180) * (m.System_SpeedOfSound / 2.0) / m.BS_Frequency[bm];
                    XmtLen = cycles * MetersPerCarrierVertical;
                    s += AddSpaces((XmtLen).ToString("F3"), 7);

                    Xmtmsec = 1000.0 * cycles / m.BS_Frequency[bm];
                    s += AddSpaces((Xmtmsec).ToString("F3"), 7);

                    s += AddSpaces(m.BS_SL[bm].ToString("F2"), 7);
                    s += AddSpaces(m.BS_RS[bm].ToString("F2"), 7);
                    s += AddSpaces(m.BS_Pings[bm].ToString(), 6);
                    s += AddSpaces(m.BS_Voln[bm].ToString(), 5);
                    s += AddSpaces(m.BS_VolBegin[bm].ToString("F1"), 7);
                    s += AddSpaces(m.BS_VolEnd[bm].ToString("F1"), 7);
                    s += AddSpaces(m.BS_WPVOLthreshold[bm].ToString("F1"), 7);
                    s += AddSpaces(m.BS_VolAmp[bm].ToString("F1"), 7);
                    s += AddSpaces(m.BS_NoiseAmplitude[bm].ToString("F1"), 7);
                    s += "\r\n";
                }
            }
            return s;
        }
        public static string GetBsString(BackScatter.EnsembleClass m)
        {
            string s = "";
            int bm;

            s += "AMPLITUDE             ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += "BS" + bm.ToString() + "     ";
            }
            s += "\r\n";

            s += "Frequency(Hz)    ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Frequency[bm].ToString("F0"), 8);
            }
            s += "\r\n";

            s += "Diameter(deg)    ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Diameter[bm].ToString("F3"), 8);
            }
            s += "\r\n";

            s += "Beam Angle(deg)  ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_BeamAngle[bm].ToString("F2"), 8);
            }
            s += "\r\n";

            s += "Rcvr1 Temp(deg)  ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Rcvr1Temperature[bm].ToString("F2"), 8);
            }
            s += "\r\n";

            s += "Rcvr2 Temp(deg)  ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Rcvr2Temperature[bm].ToString("F2"), 8);
            }
            s += "\r\n";

            s += "Xmt(Volts)       ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_TransmitVolts[bm].ToString("F2"), 8);
            }
            s += "\r\n";

            s += "Rcv Gain(dB)     ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Gain[bm].ToString("F2"), 8);
            }
            s += "\r\n";

            s += "Xmt BW(%)        ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces((100 * m.BS_TransmitBandwidth[bm]).ToString("F2"), 8);
            }
            s += "\r\n";

            s += "Rcv BW(%)        ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces((100 * m.BS_ReceiveBandwidth[bm]).ToString("F2"), 8);
            }
            s += "\r\n";

            s += "Sample Rate(Hz)  ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_SampleFrequency[bm].ToString("F0"), 8);
            }
            s += "\r\n";
            s += "\r\n";

            s += "Lag Samples      ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_LagSamples[bm].ToString("F0"), 8);
            }
            s += "\r\n";

            s += "Cycle Per Element";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_CyclePerElement[bm].ToString("F0"), 8);
            }
            s += "\r\n";

            s += "Elements         ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_NumberOfElements[bm].ToString("F0"), 8);
            }
            s += "\r\n";

            s += "Repeats          ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_NumberOfRepeats[bm].ToString("F0"), 8);
            }
            s += "\r\n";
            s += "\r\n";

            s += "Pings            ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Pings[bm].ToString(), 8);
            }
            s += "\r\n";

            s += "Beams            ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Beams[bm].ToString(), 8);
            }
            s += "\r\n";

            s += "Bins             ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Bins[bm].ToString(), 8);
            }
            s += "\r\n\r\n";

            s += "First Bin(m)     ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_FirstBin[bm].ToString("F3"), 8);
            }
            s += "\r\n";

            s += "Bin Size(m)      ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_BinSize[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            s += "Transmit (m)     ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_XmtLength[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            s += "Blank (m)        ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Blank[bm].ToString("F3"), 8);
            }
            s += "\r\n";

            s += "Dir(deg)         ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Direction[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            s += "Noise Factor     ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_NoiseFactor[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            s += "Noise BW (Hz)    ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_NoiseBandwidth[bm].ToString("F3"), 8);
            }
            s += "\r\n";

            s += "Source Level (dB)";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_SL[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            s += "Calibration (dB) ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_RS[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            s += "\r\n";
            s += "Volume Start (m) ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_VolBegin[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            s += "Volume End (m)   ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_VolEnd[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            s += "Volume Amp (dB)  ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_VolAmp[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            s += "Volume Thr (dB)  ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_WPVOLthreshold[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            s += "Volume Pings     ";
            for (bm = 0; bm < MaxBSbeams; bm++)
            {
                if (m.BS_Beams[bm] > 0) s += AddSpaces(m.BS_Voln[bm].ToString("F3"), 8);
            }
            s += "\r\n";
            return s;
        }
        public static string GetBsProfileString(int beam,BackScatter.EnsembleClass m)
        {
            float Frequency;
            float BinPos;
            float Noise;
            if (beam >= MaxBSbeams)
                beam = 0;
            if (beam < 0)
                beam = 0;

            Frequency = m.BS_Frequency[beam];
            Noise = m.BS_NoiseAmplitude[beam];

            string s = "Beam " + beam.ToString() + "\r\n";
            s += "Frequency " + Frequency.ToString() + " (Hertz)\r\n";
            s += "Noise Level    "+ Noise.ToString("F3")+ "(dB)\r\n";

            s += "Bin, Meters, Amp(dB),  BS(dB)\r\n";
            
            BinPos = m.BS_FirstBin[beam];
            for(int bin=0;bin<m.BS_Bins[beam]; bin++)
            {
                s += AddSpaces(bin.ToString(), 3);
                s += ",";
                s += AddSpaces(BinPos.ToString("F3"), 7);
                s += ",";
                s += AddSpaces(m.BS_Amplitude[beam,bin].ToString("F3"), 8);
                s += ",";
                s += AddSpaces(m.BS_BackScatter[beam,bin].ToString("F3"), 8);
                s += "\r\n";
                BinPos += m.BS_BinSize[beam];
            }
            s += "\r\n";
            return s;
        }


        public static string GetHeaderDataTypesString()
        {
            string s = "\r\n";
            //if (tabControl1.SelectedTab == tabPageLeaders)
            {
                //s += HeaderString + "\r\n";
                s += "Payload Data Structures:\r\n";
                for (int i = 0; i < BackScatter.MaxDataTypes; i++)
                {
                    if (BackScatter.DataTypeAvailable[i] == true)
                    {
                        s += i.ToString().PadLeft(3) + " " + BackScatter.DataTypeName[i];

                        switch (i)
                        {
                            case BackScatter.BACKSCATTER_LEADER_00:
                                s += ", " + (BackScatter.Ensemble.BS_Frequency[0] / 1000).ToString() + " kHz";
                                break;
                            case BackScatter.BACKSCATTER_LEADER_01:
                                s += ", " + (BackScatter.Ensemble.BS_Frequency[1] / 1000).ToString() + " kHz";
                                break;
                            case BackScatter.BACKSCATTER_LEADER_02:
                                s += ", " + (BackScatter.Ensemble.BS_Frequency[2] / 1000).ToString() + " kHz";
                                break;
                            case BackScatter.BACKSCATTER_LEADER_03:
                                s += ", " + (BackScatter.Ensemble.BS_Frequency[3] / 1000).ToString() + " kHz";
                                break;
                            case BackScatter.BACKSCATTER_LEADER_04:
                                s += ", " + (BackScatter.Ensemble.BS_Frequency[4] / 1000).ToString() + " kHz";
                                break;
                            case BackScatter.BACKSCATTER_LEADER_05:
                                s += ", " + (BackScatter.Ensemble.BS_Frequency[5] / 1000).ToString() + " kHz";
                                break;
                            case BackScatter.BACKSCATTER_LEADER_06:
                                s += ", " + (BackScatter.Ensemble.BS_Frequency[6] / 1000).ToString() + " kHz";
                                break;
                            case BackScatter.BACKSCATTER_LEADER_07:
                                s += ", " + (BackScatter.Ensemble.BS_Frequency[7] / 1000).ToString() + " kHz";
                                break;
                            case BackScatter.BACKSCATTER_LEADER_08:
                                s += ", " + (BackScatter.Ensemble.BS_Frequency[8] / 1000).ToString() + " kHz";
                                break;
                            case BackScatter.BACKSCATTER_LEADER_09:
                                s += ", " + (BackScatter.Ensemble.BS_Frequency[9] / 1000).ToString() + " kHz";
                                break;
                        }
                        s += "\r\n";
                    }
                }
            }
            return s;
        }

        public static int EnsembleState = 0;
        private const int GotHeader = 1;

        const int HDRLEN = 12;
        public static uint EnsembleSize = 0;
        public static uint[] LastEnsembleSize = new uint[3];
        public static long EnsembleCheckSum = 0;
        public static int TempReadIndex = 0;

        public static bool FindEnsemble(byte[] EnsBuf, byte[]  DataBuff)
        {
            int stay = 1;
            int i, j;

            bool csumOK = false;

            while (stay == 1)
            {
                int ByteCount = DataBuffWriteIndex - DataBuffReadIndex;
                if (ByteCount < 0)
                    ByteCount += MaxDataIndex;

                if (ByteCount <= 0)
                    break;

                switch (EnsembleState)
                {
                    default:
                        while (ByteCount > 0)
                        {
                            if (DataBuff[DataBuffReadIndex] == 'R')//standard binary
                                break;
                            DataBuffReadIndex++;
                            if (DataBuffReadIndex >= MaxDataIndex)
                                DataBuffReadIndex = 0;

                            ByteCount--;
                        }
                        //check for header ID
                        if (ByteCount > HDRLEN)
                        {
                            TempReadIndex = DataBuffReadIndex;
                            byte[] DBuff = new byte[HDRLEN];
                            for (i = 0; i < HDRLEN; i++)
                            {
                                DBuff[i] = DataBuff[TempReadIndex];
                                TempReadIndex++;
                                if (TempReadIndex >= MaxDataIndex)
                                    TempReadIndex = 0;
                            }
                            j = 0;
                            for (i = 0; i < 8; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        if (DBuff[i] == 'R')
                                            j++;
                                        break;
                                    case 1:
                                        if (DBuff[i] == 'T')
                                            j++;
                                        break;
                                    case 2:
                                        if (DBuff[i] == 'I')
                                            j++;
                                        break;
                                    case 3:
                                        if (DBuff[i] == 'R')
                                            j++;
                                        break;
                                    case 4:
                                        if (DBuff[i] == 'I')
                                            j++;
                                        break;
                                    case 5:
                                        if (DBuff[i] == 'V')
                                            j++;
                                        break;
                                    case 6:
                                        if (DBuff[i] == 'B')
                                            j++;
                                        break;
                                    case 7:
                                        if (DBuff[i] == 'S')
                                            j++;
                                        break;
                                }
                            }

                            ushort EnsSiz;
                            ushort NotEnsSiz;
                            i = 8;
                            if (j == 8)
                            {
                                EnsSiz = DBuff[i++];
                                EnsSiz += (ushort)(DBuff[i++] << 8);

                                NotEnsSiz = DBuff[i++];
                                NotEnsSiz += (ushort)(DBuff[i++] << 8);
                                NotEnsSiz = (ushort)~NotEnsSiz;

                                TempReadIndex = DataBuffReadIndex;
                                if (EnsSiz == NotEnsSiz)
                                {
                                    for (i = 0; i < HDRLEN; i++)
                                    {
                                        EnsBuf[i] = DBuff[i];
                                    }
                                    TempReadIndex += HDRLEN;
                                    if (TempReadIndex >= MaxDataIndex)
                                        TempReadIndex -= MaxDataIndex;

                                    EnsembleState = GotHeader;

                                    LastEnsembleSize[0] = LastEnsembleSize[1];
                                    LastEnsembleSize[1] = EnsembleSize;
                                    EnsembleSize = EnsSiz;
                                    LastEnsembleSize[2] = EnsembleSize;
                                }
                                else//point to next byte in the buffer
                                {
                                    DataBuffReadIndex++;
                                    if (DataBuffReadIndex >= MaxDataIndex)
                                        DataBuffReadIndex = 0;
                                }
                            }
                            else//point to next byte in the buffer
                            {
                                DataBuffReadIndex++;
                                if (DataBuffReadIndex >= MaxDataIndex)
                                    DataBuffReadIndex = 0;

                                stay = 1;//still have data in buffer
                            }
                        }
                        else//wait for more data
                        {
                            stay = 0;
                        }
                        break;//end check for header

                    case GotHeader:
                        if (ByteCount >= HDRLEN + EnsembleSize + 2)
                        {
                            long csum;
                            ushort crc = 0;
                            //CCITT 16 bit algorithm (X^16 + X^12 + X^5 + 1)
                            for (i = HDRLEN; i < EnsembleSize + HDRLEN; i++)
                            {
                                EnsBuf[i] = DataBuff[TempReadIndex];
                                crc = (ushort)((byte)(crc >> 8) | (crc << 8));
                                crc ^= DataBuff[TempReadIndex];
                                crc ^= (byte)((crc & 0xff) >> 4);
                                crc ^= (ushort)((crc << 8) << 4);
                                crc ^= (ushort)(((crc & 0xff) << 4) << 1);

                                TempReadIndex++;
                                if (TempReadIndex >= MaxDataIndex)
                                    TempReadIndex -= MaxDataIndex;
                            }
                            csum = crc;

                            //read in one more for the checksum
                            for (j = i; j < EnsembleSize + HDRLEN + 2; j++)
                            {
                                EnsBuf[j] = DataBuff[TempReadIndex];
                                TempReadIndex++;
                                if (TempReadIndex >= MaxDataIndex)
                                    TempReadIndex -= MaxDataIndex;
                            }
                            EnsembleCheckSum = EnsBuf[i];
                            EnsembleCheckSum += EnsBuf[i + 1] << 8;

                            if (csum == EnsembleCheckSum)
                            {
                                csumOK = true;
                                DataBuffReadIndex = TempReadIndex;
                                EnsembleState = 0;
                                stay = 0;
                            }
                            else//checksum failed
                            {
                                DataBuffReadIndex += HDRLEN;//chuck the header
                                if (DataBuffReadIndex >= MaxDataIndex)
                                    DataBuffReadIndex -= MaxDataIndex;

                                EnsembleState = 0;
                                stay = 1;
                            }
                        }
                        else
                            stay = 0;
                        break;
                }
            }
            return csumOK;
        }
    }
}
