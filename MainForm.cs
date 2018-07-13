using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UCNLDrivers;
using UCNLKML;
using UCNLNMEA;
using UCNLUI.Dialogs;

namespace RedGTR_VLBL
{
    public partial class MainForm : Form
    {
        #region Properties

        NMEASerialPort gtrPort;
        NMEASerialPort gnssEmulatorPort;
        PrecisionTimer timer;
        Measurements measurements;


        string settingsFileName;
        string logPath;
        string logFileName;
        string snapshotsPath;

        TSLogProvider logger;
        SettingsProvider<SettingsContainer> settingsProvider;

        bool isAutoquery = false;
        bool isAutosnapshot = false;
        bool isRestart = false;

        bool gtr_isWaiting = false;
        bool gtr_isWaitingRemote = false;
        int gtr_timeoutCounter = 0;
        int gtr_remoteTimeoutCounter = 0;
        int gtr_Timeout = 2;
        int gtr_RemoteTimeout_S = 10;
        string gtr_lastQuery = string.Empty;
        bool gtr_salinityUpdated = false;
        bool gtr_deviceInfoUpdated = false;

        CDS_CMD gtr_requestID = CDS_CMD.CDS_CMD_INVALID;


        EventHandler<SerialErrorReceivedEventArgs> gtrPortErrorEventHandler;
        EventHandler<NewNMEAMessageEventArgs> gtrPortNewNMEAMessageEventHandler;
        EventHandler<TextAddedEventArgs> loggerTextAddedEventHandler;
        EventHandler timerTickHandler;

        AgingDouble bLatitude;
        AgingDouble bLongitude;
        AgingDouble bTemperature;
        AgingDouble bDepth;
        AgingDouble tTemperature;
        AgingDouble tDepth;

        AgingDouble tLatitude;
        AgingDouble tLongitude;
        AgingDouble tRadialError;
        

        List<GeoPoint3DWE> tLocation;
        GeoPoint3DWE tBestLocation;
        List<GeoPoint> bLocation;        

        delegate T NullChecker<T>(object parameter);
        NullChecker<int> intNullChecker = (x => x == null ? -1 : (int)x);
        NullChecker<double> doubleNullChecker = (x => x == null ? double.NaN : (double)x);
        NullChecker<string> stringNullChecker = (x => x == null ? string.Empty : (string)x);

        Random rnd = new Random();

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            this.Text = string.Format("{0}", Application.ProductName);

            #region file names & paths

            DateTime startTime = DateTime.Now;
            settingsFileName = Path.ChangeExtension(Application.ExecutablePath, "settings");
            logPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "LOG");
            logFileName = StrUtils.GetTimeDirTreeFileName(startTime, Application.ExecutablePath, "LOG", "log", true);
            snapshotsPath = StrUtils.GetTimeDirTree(startTime, Application.ExecutablePath, "SNAPSHOTS", false);

            #endregion

            #region logger

            loggerTextAddedEventHandler = new EventHandler<TextAddedEventArgs>(logger_TextAdded);

            logger = new TSLogProvider(logFileName);
            logger.TextAddedEvent += loggerTextAddedEventHandler;
            logger.WriteStart();            

            #endregion
           
            #region settings

            logger.Write("Loading settings...");
            settingsProvider = new SettingsProviderXML<SettingsContainer>();
            settingsProvider.isSwallowExceptions = false;

            try
            {
                settingsProvider.Load(settingsFileName);
            }
            catch (Exception ex)
            {
                ProcessException(ex, true);
            }

            logger.Write(settingsProvider.Data.ToString());

            gtr_RemoteTimeout_S = Convert.ToInt32((GTR.MIN_REM_TOUT_MS / 1000) + 2 * (double)settingsProvider.Data.MaxDistance / 1500) + 1;

            #endregion

            #region NMEA

            NMEAParser.AddManufacturerToProprietarySentencesBase(ManufacturerCodes.TNT);
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "0", "x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "1", "x,x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "2", "x,x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "3", "x,x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "4", "x,x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "5", "x,x.x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "6", "x,x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "7", "x,x.x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "8", "x,x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "9","x,x.x,x.x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "A", "x,x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "B", "x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "C", "x,x.x,x.x,x.x,x.x,x.x,x.x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "D", "x,x,x.x,x.x,x.x,x.x,x.x,x.x,x.x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "E", "x,x,x");                                                                             
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "!", "c--c,x,c--c,x,x,c--c");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "O", "x.x,x.x");
            NMEAParser.AddProprietarySentenceDescription(ManufacturerCodes.TNT, "P", "x,x.x");

            /*
            #define IC_D2H_ACK              '0'        // $PTNT0,errCode
            #define IC_H2D_FLD_GET          '1'        // $PTNT1,fldID,reserved
            #define IC_H2D_FLD_SET          '2'        // $PTNT2,fldID,fldNewValue
            #define IC_D2H_FLD_VAL          '3'        // $PTNT3,fldID,fldValue
            #define IC_H2D_LOC_DATA_GET     '4'        // $PTNT4,locDataID,reserved
            #define IC_D2H_LOC_DATA_VAL     '5'        // $PTNT5,locDataID,locDataValue
            #define IC_H2D_ACT_INVOKE       '6'        // $PTNT6,actionID,reserved
            #define IC_H2D_LOC_DATA_SET     '7'        // $PTNT7,locDataID,locDataValue
            #define IC_H2D_REM_SEND         '8'        // $PTNT8,targetAddr,codeMsg
            #define IC_D2H_REM_RECEIVED     '9'        // $PTNT9,codeMsg,snrd,dpl
            #define IC_H2D_REM_PING         'A'        // $PTNTA,targetAddr,timeoutMs
            #define IC_D2H_REM_TOUT         'B'        // $PTNTB,targetAddr
            #define IC_D2H_REM_PONG         'C'        // $PTNTC,requestedAddr,snrd,dpl,pTime,[dst],[dpt],[tmp]
            #define IC_D2H_REM_PONGEX       'D'        // $PTNTD,requestedAddr,requestedCmd,receivedValue_decoded,snrd,dpl,pTime,[dst],[dpt],[tmp]
            #define IC_H2D_REM_PINGEX       'E'        // $PTNTE,targetAddr,requestedCmd,timeoutMs
            #define IC_D2H_DEV_INFO_VAL     '!'        // $PTNT!,sys_moniker,sys_ver,dev_type,core_moniker,core_ver,dev_serial_num           
            #define IC_D2H_PRETMP_VAL       'O'        // $PTNTO,pressure_mbar,temp_deg_c
            #define IC_H2D_SETVAL           'P'        // $PTNTP,valueID,value
            */

            #endregion

            #region other

            gtrPortErrorEventHandler = new EventHandler<SerialErrorReceivedEventArgs>(gtrPort_Error);
            gtrPortNewNMEAMessageEventHandler = new EventHandler<NewNMEAMessageEventArgs>(gtrPort_NewNMEAMessage);

            gtrPort = new NMEASerialPort(new SerialPortSettings(settingsProvider.Data.GTRPortName, BaudRate.baudRate9600,
                System.IO.Ports.Parity.None, DataBits.dataBits8, System.IO.Ports.StopBits.One, System.IO.Ports.Handshake.None));

            gtrPort.IsRawModeOnly = false;            

            timerTickHandler = new EventHandler(timer_Tick);

            timer = new PrecisionTimer();
            timer.Mode = Mode.Periodic;
            timer.Period = 1000;

            timer.Tick += timerTickHandler;
            timer.Start();


            if (settingsProvider.Data.IsGNSSEmulator)
            {
                gnssEmulatorPort = new NMEASerialPort(new SerialPortSettings(settingsProvider.Data.GNSSEmulatorPortName, BaudRate.baudRate9600,
                 Parity.None, DataBits.dataBits8, StopBits.One, Handshake.None));
            }

            measurements = new Measurements(settingsProvider.Data.MeasurementsFIFOSize, settingsProvider.Data.BaseSize);

            bLatitude = new AgingDouble("F06", "°");
            bLongitude = new AgingDouble("F06", "°");
            bTemperature = new AgingDouble("F01", "°C");
            bDepth = new AgingDouble("F03", " m");
            tTemperature = new AgingDouble("F01", "°C");
            tDepth = new AgingDouble("F02", " m");

            tLatitude = new AgingDouble("F06", "°");
            tLongitude = new AgingDouble("F06", "°");
            tRadialError = new AgingDouble("F03", " m");

            tLocation = new List<GeoPoint3DWE>();
            bLocation = new List<GeoPoint>();

            tBestLocation = new GeoPoint3DWE();
            tBestLocation.Latitude = double.NaN;
            tBestLocation.Longitude = double.NaN;
            tBestLocation.RadialError = double.NaN;

            marinePlot.InitTracks(settingsProvider.Data.MeasurementsFIFOSize);
            marinePlot.AddTrack("BOAT GNSS", Color.Blue, 2.0f, 2, settingsProvider.Data.MeasurementsFIFOSize, true);
            marinePlot.AddTrack("BASE", Color.Salmon, 2.0f, 8, settingsProvider.Data.BaseSize, false);            
            marinePlot.AddTrack("MEASUREMENTS", Color.Green, 2.0f, 4, settingsProvider.Data.MeasurementsFIFOSize, false);
            marinePlot.AddTrack("TARGET", Color.Black, 2.0f, 4, settingsProvider.Data.MeasurementsFIFOSize, false);
            marinePlot.AddTrack("BEST", Color.Red, 2.0f, 8, 1, false);
            
            #endregion                                    
        }

        #endregion
        
        #region Methods

        #region parsers

        private void Parse_ACK(object[] parameters)
        {
            gtr_isWaiting = false;
            //#define IC_D2H_ACK              '0'        // $PTNT0,errCode
            logger.Write(string.Format("{0} (RedGTR) >> ACK {1}", gtrPort.PortName, (LocError)Enum.ToObject(typeof(LocError), (int)parameters[0])));
        }

        private void Parse_DEVICE_INFO(object[] parameters)
        {
            gtr_isWaiting = false;
            
            try
            {
                //$PTNT!,RedGTR PR: 30 bar,256,TANTRA,514,3,18003E001351353232333438*1A 

                string sysMoniker = (string)parameters[0];
                int sysVersion = (int)parameters[1];
                string coreMoniker = (string)parameters[2];
                int coreVersion = (int)parameters[3];
                DeviceType dType = (DeviceType)Enum.ToObject(typeof(DeviceType), (int)parameters[4]);
                string serialNumber = (string)parameters[5];

                gtr_deviceInfoUpdated = true;

                logger.Write(string.Format("{0} (RedGTR) >> DEV_INFO", gtrPort.PortName));

            }
            catch (Exception ex)
            {
                ProcessException(ex, false);
            }
        }

        private void Parse_LOC_DATA_VAL(object[] parameters)
        {
            gtr_isWaiting = false;

            try
            {
                LocData dataID = (LocData)Enum.ToObject(typeof(LocData), (int)parameters[0]);
                double dataValue = (double)parameters[1];

                if (dataID == LocData.LOC_DATA_SALINITY)
                {
                    gtr_salinityUpdated = true;

                    logger.Write(string.Format("{0} (RedGTR) >> STY_UPDATED", gtrPort.PortName));
                }
                // TODO

            }
            catch (Exception ex)
            {
                ProcessException(ex, false);
            }
            //#define IC_D2H_LOC_DATA_VAL     '5'        // $PTNT5,locDataID,locDataValue
        }

        private void Parse_REM_RECEIVED(object[] parameters)
        {
            gtr_isWaitingRemote = false;
            //#define IC_D2H_REM_RECEIVED     '9'        // $PTNT9,codeMsg,snrd,dpl
        }

        private void Parse_REM_TOUT(object[] parameters)
        {
            gtr_isWaitingRemote = false;
            //#define IC_D2H_REM_TOUT         'B'        // $PTNTB,targetAddr
        }

        private void Parse_REM_PONG(object[] parameters)
        {
            gtr_isWaitingRemote = false;
            //#define IC_D2H_REM_PONG         'C'        // $PTNTC,requestedAddr,snrd,dpl,pTime,[dst],[dpt],[tmp]
        }

        private void Parse_REM_PONGEX(object[] parameters)
        {            
            gtr_isWaitingRemote = false;
            //#define IC_D2H_REM_PONGEX       'D'        // $PTNTD,requestedAddr,requestedCmd,receivedValue_decoded,snrd,dpl,pTime,[dst],[dpt],[tmp]

            try
            {
                int requestedAddr = (int)parameters[0];
                CDS_CMD cmdID = (CDS_CMD)Enum.ToObject(typeof(CDS_CMD), (int)parameters[1]);
                double value = (double)parameters[2];
                double snrd = 10 * Math.Log10(Math.Abs((double)parameters[3]));
                double dpl = (double)parameters[4];
                double pTime = (double)parameters[5];
                double dst = (double)parameters[6];
                double dpt = (double)parameters[7];
                double tmp = (double)parameters[8];

                bTemperature.Value = tmp;
                bDepth.Value = dpt;

                if (cmdID == CDS_CMD.CDS_CMD_TMP)
                    tTemperature.Value = value;
                else if (cmdID == CDS_CMD.CDS_CMD_DPT)
                    tDepth.Value = value;

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(CultureInfo.InvariantCulture, "BOAT\r\nLAT: {0}\r\nLON: {1}\r\nDPT: {2}\r\nTMP: {3}\r\nPTM: {4:F04} s\r\n",
                    bLatitude.ToString(), bLongitude.ToString(), bDepth.ToString(), bTemperature.ToString(), pTime);

                sb.AppendFormat(CultureInfo.InvariantCulture, "\r\nTARGET\r\nDST: {0:F02} m\r\nSNR: {1:F01} dB\r\nDPL: {2:F02} Hz\r\n", dst, snrd, dpl);

                if (tDepth.IsInitialized && !tDepth.IsObsolete)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "DPT: {0}\r\n", tDepth.ToString());

                if (tTemperature.IsInitialized && !tTemperature.IsObsolete)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "TMP: {0}\r\n", tTemperature.ToString());
                

                if (bLatitude.IsInitialized && !bLatitude.IsObsolete &&
                    bLongitude.IsInitialized && !bLongitude.IsObsolete)
                {
                    measurements.Add(new Measurement(bLatitude.Value, bLongitude.Value, dst, snrd, dpt));

                    InvokeUpdateTrack("MEASUREMENTS", bLatitude.Value, bLongitude.Value);

                    if (measurements.IsBaseExists && tDepth.IsInitialized && (measurements.AngleRange > 270))                        
                    {
                        GeoPoint3DWE prevLocation = new GeoPoint3DWE();
                        prevLocation.Latitude = double.NaN;
                        prevLocation.Longitude = double.NaN;
                        prevLocation.Depth = tDepth.Value;
                        prevLocation.RadialError = double.NaN;                     
                       
                        double stStageRErr = 0.0;
                        int itCnt = 0;

                        var basePoints = measurements.GetBase();
                        List<PointF> basePnts = new List<PointF>();
                        foreach (var bPoint in basePoints)
                        {
                            basePnts.Add(new PointF(Convert.ToSingle(bPoint.Latitude), Convert.ToSingle(bPoint.Longitude)));
                        }

                        InvokeUpdateTrack("BASE", basePnts.ToArray());

                        var locResult = Navigation.LocateLBL_NLM(basePoints, prevLocation, settingsProvider.Data.RadialErrorThreshold, out stStageRErr, out itCnt);
                        
                        tLatitude.Value = locResult.Latitude;
                        tLongitude.Value = locResult.Longitude;
                        tRadialError.Value = locResult.RadialError;                        
                        tLocation.Add(locResult);

                        InvokeUpdateTrack("TARGET", locResult.Latitude, locResult.Longitude);

                        if (settingsProvider.Data.IsGNSSEmulator)
                        {
                            SendEMU(locResult.Latitude, locResult.Longitude, tDepth.Value, locResult.RadialError);
                        }

                        if ((double.IsNaN(tBestLocation.Latitude) || (tBestLocation.RadialError > locResult.RadialError)))                                      
                        {
                            tBestLocation.Latitude = locResult.Latitude;
                            tBestLocation.Longitude = locResult.Longitude;
                            tBestLocation.RadialError = locResult.RadialError;
                            measurements.UpdateReferencePoint(tBestLocation.Latitude, tBestLocation.Longitude);

                            InvokeUpdateTrack("BEST", tBestLocation.Latitude, tBestLocation.Longitude);
                        }                                                                     

                        InvokeSetEnabled(mainToolStrip, tracksBtn, true);
                    }
                    else
                    {
                        double cLat = 0;
                        double cLon = 0;
                        measurements.CenterOfMass(out cLat, out cLon);
                        measurements.UpdateReferencePoint(cLat, cLon);
                    }
                }



                if (tLatitude.IsInitialized && !tLatitude.IsObsolete &&
                    tLongitude.IsInitialized && !tLongitude.IsObsolete)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "LAT: {0}\r\nLON: {1}\r\n", tLatitude.ToString(), tLongitude.ToString());
                }

                if (tRadialError.IsInitialized && !tRadialError.IsObsolete)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "RER: {0}\r\n", tRadialError.ToString());

                if (!double.IsNaN(tBestLocation.RadialError))
                    sb.AppendFormat(CultureInfo.InvariantCulture, "BRE: {0:F03}\r\n", tBestLocation.RadialError);

                InvokeSetLeftUpperCornerText(sb.ToString());
                InvokeInvalidatePlot();

                if (isAutosnapshot)
                    InvokeSaveSnapShot();

            }
            catch (Exception ex)
            {
                ProcessException(ex, false);
            }
        }

        private void Parse_PRETMP_VAL(object[] parameters)
        {
            //#define IC_D2H_PRETMP_VAL       'O'        // $PTNTO,pressure_mbar,temp_deg_c
        }

        private void Parse_RMC(object[] parameters)
        {
            DateTime tStamp = (DateTime)parameters[0];

            var latitude = doubleNullChecker(parameters[2]);
            var longitude = doubleNullChecker(parameters[4]);
            var groundSpeed = doubleNullChecker(parameters[6]);
            var courseOverGround = doubleNullChecker(parameters[7]);
            var dateTime = (DateTime)parameters[8];
            var magneticVariation = doubleNullChecker(parameters[9]);

            bool isValid = (parameters[1].ToString() != "Invalid") &&
                           (!double.IsNaN(latitude)) &&
                           (!double.IsNaN(longitude)) &&
                           (!double.IsNaN(groundSpeed)) &&
                           (!double.IsNaN(courseOverGround)) &&
                           (parameters[11].ToString() != "N");

            if (isValid)
            {
                dateTime.AddHours(tStamp.Hour);
                dateTime.AddMinutes(tStamp.Minute);
                dateTime.AddSeconds(tStamp.Second);
                dateTime.AddMilliseconds(tStamp.Millisecond);
                groundSpeed = NMEAParser.NM2Km(groundSpeed);

                if (parameters[3].ToString() == "South") latitude = -latitude;
                if (parameters[5].ToString() == "West") longitude = -longitude;


                bLatitude.Value = latitude;
                bLongitude.Value = longitude;

                GeoPoint newPoint = new GeoPoint();
                newPoint.Latitude = latitude;
                newPoint.Longitude = longitude;

                bLocation.Add(newPoint);

                InvokeUpdateTrack("BOAT GNSS", latitude, longitude);
                InvokeInvalidatePlot();

                InvokeSetEnabled(mainToolStrip, tracksBtn, true);
            }
        }

        #endregion

        #region Misc

        private void InvokeSetEnabled(ToolStrip strip, ToolStripItem item, bool enabled)
        {
            if (strip.InvokeRequired)
                strip.Invoke((MethodInvoker)delegate
                {
                    if (item.Enabled != enabled)
                        item.Enabled = enabled;
                });
            else
            {
                if (item.Enabled != enabled)
                    item.Enabled = enabled;
            }
        }

        private void InvokeInvalidatePlot()
        {
            if (marinePlot.InvokeRequired)
                marinePlot.Invoke((MethodInvoker)delegate { marinePlot.Invalidate(); });
            else
                marinePlot.Invalidate();
        }

        private void InvokeSetLeftUpperCornerText(string text)
        {
            if (marinePlot.InvokeRequired)
                marinePlot.Invoke((MethodInvoker)delegate { marinePlot.LeftUpperCornerText = text; });
            else
                marinePlot.LeftUpperCornerText = text;
        }

        private void InvokeUpdateTrack(string trackID, double lat, double lon)
        {
            if (marinePlot.InvokeRequired)
                marinePlot.Invoke((MethodInvoker)delegate { marinePlot.UpdateTrack(trackID, lat, lon); });
            else
                marinePlot.UpdateTrack(trackID, lat, lon);
        }

        private void InvokeUpdateTrack(string trackID, PointF[] pnts)
        {
            if (marinePlot.InvokeRequired)
                marinePlot.Invoke((MethodInvoker)delegate { marinePlot.UpdateTrack(trackID, pnts); });
            else
                marinePlot.UpdateTrack(trackID, pnts);
        }

        private void InvokeSaveSnapShot()
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { SaveFullSnapshot(); });
            else
                SaveFullSnapshot();
        }



        private void ProcessException(Exception ex, bool isMsgBox)
        {
            string msg = logger.Write(ex);

            if (isMsgBox)
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void OnConnectionOpened()
        {
            gtr_deviceInfoUpdated = false;
            gtr_isWaiting = false;
            gtr_isWaitingRemote = false;
            gtr_lastQuery = string.Empty;
            gtr_salinityUpdated = false;

            connectionBtn.Checked = true;
            isAutoQueryBtn.Enabled = true;
            settingsBtn.Enabled = false;
            connectionStatusLbl.Text = string.Format("CONNECTED ON {0}", gtrPort.PortName);            
        }

        private void OnConnectionClosed()
        {
            connectionBtn.Checked = false;
            settingsBtn.Enabled = true;
            isAutoquery = false;
            isAutoQueryBtn.Checked = false;
            isAutoQueryBtn.Enabled = false;
            connectionStatusLbl.Text = "DISCONNECTED";
        }

        private void SaveFullSnapshot()
        {
            Bitmap target = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(target, this.DisplayRectangle);

            try
            {
                if (!Directory.Exists(snapshotsPath))
                    Directory.CreateDirectory(snapshotsPath);

                target.Save(Path.Combine(snapshotsPath, string.Format("{0}.{1}", StrUtils.GetHMSString(), ImageFormat.Png)));
            }
            catch
            {
                //
            }
        }


        private void SendEMU(double tLat, double tLon, double tdpt, double tRErr)
        {
            // "hhmmss.ss,A=Valid|V=Invalid,llll.ll,N=North|S=South,yyyyy.yy,E=East|W=West,x.x,x.x,ddmmyy,x.x,a,a" },
            // $GPRMC,105552.000,A,4831.4568,N,04430.2342,E,0.17,180.99,230518,,,A*6F

            string latCardinal;
            if (tLat > 0) latCardinal = "North";
            else latCardinal = "South";

            string lonCardinal;
            if (tLon > 0) lonCardinal = "East";
            else lonCardinal = "West";

            StringBuilder sb = new StringBuilder();

            sb.Append(NMEAParser.BuildSentence(TalkerIdentifiers.GN, SentenceIdentifiers.RMC, new object[] 
            {
                DateTime.Now, 
                "Valid", 
                tLat, latCardinal,
                tLon, lonCardinal,
                null, // speed knots
                null, // track true
                DateTime.Now,
                null, // magnetic variation
                null, // magnetic variation direction
                "A",
            }));

            // "hhmmss.ss,llll.ll,a,yyyyy.yy,a,0=Fix not availible|1=GPS fix|2=DGPS fix,xx,x.x,x.x,M,x.x,M,x.x,xxxx" },
            sb.Append(NMEAParser.BuildSentence(TalkerIdentifiers.GN, SentenceIdentifiers.GGA, new object[]
            {
                DateTime.Now,
                tLat, latCardinal[0],
                tLon, lonCardinal[0],
                "GPS fix",
                settingsProvider.Data.BaseSize,
                tRErr,
                -tdpt,
                "M",
                null,
                "M",
                null,
                null
            }));

            try
            {
                gnssEmulatorPort.SendData(sb.ToString());
            }
            catch (Exception ex)
            {
                ProcessException(ex, false);
            }
        }

        private void SaveTracks(string fileName)
        {
            #region Save to KML

            KMLData data = new KMLData(fileName, "Generated by RedGTR_VLBL application");
                        
            var gnssKmlTrack = new List<KMLLocation>();
            foreach (var trackItem in bLocation)
                gnssKmlTrack.Add(new KMLLocation(trackItem.Longitude, trackItem.Latitude, bDepth.Value));
            data.Add(new KMLPlacemark("BOAT GNSS", "Boat GNSS track", gnssKmlTrack.ToArray()));

            var msmsKmlTrack = new List<KMLLocation>();
            var msms = measurements.ToArray();
            foreach (var trackItem in msms)
                msmsKmlTrack.Add(new KMLLocation(trackItem.Longitude, trackItem.Latitude, bDepth.Value));
            data.Add(new KMLPlacemark("MEASUREMENT POINTS", "Measurements points", msmsKmlTrack.ToArray()));

            var targetKmlTrack = new List<KMLLocation>();
            foreach (var trackItem in tLocation)
                targetKmlTrack.Add(new KMLLocation(trackItem.Longitude, trackItem.Latitude, tDepth.Value));

            data.Add(new KMLPlacemark("TARGET", "Target track", targetKmlTrack.ToArray()));

            data.Add(new KMLPlacemark("BEST", "Location with minimal radial error", true, true, new KMLLocation(tBestLocation.Longitude, tBestLocation.Latitude, tDepth.Value)));


            try
            {
                TinyKML.Write(data, fileName);                
            }
            catch (Exception ex)
            {
                ProcessException(ex, true);
            }

            #endregion
        }

        private void AnalyzeLog(string[] lines)
        {
            foreach (var line in lines)
            {
                if (line.Contains(">> $"))
                {
                    var nmsg = line.Substring(line.IndexOf(">>") + 3).Trim();
                    if (!nmsg.EndsWith("\r\n"))
                        nmsg = nmsg + "\r\n";


                    gtrPort_NewNMEAMessage(gtrPort, new NewNMEAMessageEventArgs(nmsg));

                    //Thread.Sleep(200);
                }
            }

        }

        #endregion

        private void gtr_TrySend(string msg, string queryDescription, bool isRemote)
        {
            try
            {
                gtrPort.SendData(msg);
                logger.Write(string.Format("{0} (RedGTR) << {1}", gtrPort.PortName, msg));
                gtr_lastQuery = queryDescription;
                gtr_isWaiting = true;
                gtr_timeoutCounter = 0;

                if (isRemote)
                {
                    gtr_isWaitingRemote = true;
                    gtr_remoteTimeoutCounter = 0;
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex, false);
            }            
        }

        private void gtr_OnTimeout()
        {
            gtr_isWaiting = false;
            logger.Write(string.Format("{0} (RedGTR) >> {1} timeout", gtrPort.PortName, gtr_lastQuery));
        }

        private void gtr_OnRemotTimeout()
        {
            gtr_isWaitingRemote = false;
            logger.Write(string.Format("SUB #{0} {1} timeout", settingsProvider.Data.TargetAddr, gtr_requestID));
        }

        private void gtr_QuerySalinitySet(double salinity)
        {
            var msg = NMEAParser.BuildProprietarySentence(ManufacturerCodes.TNT, "7", new object[] { (int)LocData.LOC_DATA_SALINITY, salinity });
            gtr_TrySend(msg, string.Format("Salinity set = {0:F01}", salinity), false);
        }

        private void gtr_DeviceInfoQuery()
        {
            var msg = NMEAParser.BuildProprietarySentence(ManufacturerCodes.TNT, "4", new object[] { (int)LocData.LOC_DATA_DEVICE_INFO, 0 });
            gtr_TrySend(msg, "Device info query", false);
        }

        private void gtr_QueryRemote(int targetAddr, CDS_CMD cmd, int timeoutMs)
        {
            // $PTNTE,targetAddr,requestedCmd,timeoutMs
            var msg = NMEAParser.BuildProprietarySentence(ManufacturerCodes.TNT, "E", new object[] { targetAddr, (int)cmd, timeoutMs });

            gtr_requestID = cmd;
            gtr_TrySend(msg, string.Format("RedGTR << ? SUB #{0}, CMD {1}, TMO {2} ms", targetAddr, cmd, timeoutMs), true);
        }

        #endregion

        #region Handlers

        #region UI

        #region mainToolStrip

        private void connectionBtn_Click(object sender, EventArgs e)
        {
            if (gtrPort.IsOpen)
            {
                gtrPort.PortError -= gtrPortErrorEventHandler;
                gtrPort.NewNMEAMessage -= gtrPortNewNMEAMessageEventHandler;

                try
                {
                    gtrPort.Close();
                }
                catch (Exception ex)
                {
                    ProcessException(ex, true);
                }

                if (settingsProvider.Data.IsGNSSEmulator)
                {
                    try
                    {
                        gnssEmulatorPort.Close();
                    }
                    catch (Exception ex)
                    {
                        ProcessException(ex, false);
                    }
                }

                OnConnectionClosed();                
            }
            else
            {
                try
                {
                    gtrPort.Open();
                    gtrPort.PortError += gtrPortErrorEventHandler;
                    gtrPort.NewNMEAMessage += gtrPortNewNMEAMessageEventHandler;

                    OnConnectionOpened();
                }
                catch (Exception ex)
                {
                    ProcessException(ex, true);
                    OnConnectionClosed();
                }

                if ((gtrPort.IsOpen) && (settingsProvider.Data.IsGNSSEmulator))
                {
                    try
                    {
                        gnssEmulatorPort.Open();
                    }
                    catch (Exception ex)
                    {
                        ProcessException(ex, false);
                    }
                }
            }
        }

        private void isAutoQueryBtn_Click(object sender, EventArgs e)
        {
            isAutoquery = !isAutoquery;
            isAutoQueryBtn.Checked = isAutoquery;
        }

        private void isAutosnapshotBtn_Click(object sender, EventArgs e)
        {
            isAutosnapshot = !isAutosnapshot;
            isAutosnapshotBtn.Checked = isAutosnapshot;
        }

        private void exportTracksBtn_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sDialog = new SaveFileDialog())
            {
                sDialog.Title = "Exporting tracks...";
                sDialog.Filter = "Google KML (*.kml)|*.kml";
                sDialog.FileName = string.Format("{0}.kml", StrUtils.GetHMSString());
                sDialog.DefaultExt = "kml";

                if (sDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SaveTracks(sDialog.FileName);
                }
            }
        }

        private void clearTracksBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear tracks?", "Question", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                tLocation.Clear();
                bLocation.Clear();
                marinePlot.ClearTracks();
                marinePlot.Invalidate();

                tracksBtn.Enabled = false;
            }
        }

        private void analyzeBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog oDialog = new OpenFileDialog())
            {
                oDialog.Title = "Select a log file to analyze...";
                oDialog.DefaultExt = "log";
                oDialog.Filter = "Log files (*.log)|*.log";
                oDialog.InitialDirectory = logPath;

                if (oDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    bool isLoaded = false;
                    string[] logLines = null;

                    try
                    {
                        logLines = File.ReadAllLines(oDialog.FileName);
                        isLoaded = true;
                    }
                    catch (Exception ex)
                    {
                        ProcessException(ex, true);
                    }

                    if (isLoaded)
                        AnalyzeLog(logLines);
                }
            }
        }


        private void settingsBtn_Click(object sender, EventArgs e)
        {
            bool isSaved = false;

            using (SettingsEditor sDialog = new SettingsEditor())
            {
                sDialog.Text = string.Format("{0} - [Settings]", Application.ProductName);
                sDialog.Value = settingsProvider.Data;

                if (sDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    settingsProvider.Data = sDialog.Value;

                    try
                    {
                        settingsProvider.Save(settingsFileName);
                        isSaved = true;
                    }
                    catch (Exception ex)
                    {
                        ProcessException(ex, true);
                    }
                }
            }

            if (isSaved)
            {
                if (MessageBox.Show("Restart application to apply new settings?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    isRestart = true;
                    Application.Restart();
                }
            }
        }

        private void infoBtn_Click(object sender, EventArgs e)
        {
            using (AboutBox aDialog = new AboutBox())
            {
                aDialog.ApplyAssembly(Assembly.GetExecutingAssembly());
                aDialog.Weblink = "www.unavlab.com";
                aDialog.ShowDialog();
            }
        }

        #endregion

        #region mainFrom

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            logger.TextAddedEvent -= loggerTextAddedEventHandler;
            logger.FinishLog();

            timer.Tick -= timerTickHandler;
            if (timer.IsRunning)
                timer.Stop();
            timer.Dispose();

            if (gnssEmulatorPort != null)
            {
                if (gnssEmulatorPort.IsOpen)
                    gnssEmulatorPort.Close();

                gnssEmulatorPort.Dispose();
            }

            if (gtrPort.IsOpen)
            {
                gtrPort.NewNMEAMessage -= gtrPortNewNMEAMessageEventHandler;
                gtrPort.PortError -= gtrPortErrorEventHandler;
                gtrPort.Close();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (!isRestart) && (MessageBox.Show("Close application?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes);
        }

        #endregion

        #region consoleToolStrip

        private void clearHistoryBtn_Click(object sender, EventArgs e)
        {
            historyTxb.Clear();
        }

        #endregion

        #region historyTxb

        private void historyTxb_TextChanged(object sender, EventArgs e)
        {
            historyTxb.ScrollToCaret();
        }

        #endregion

        #region plotPanel

        private void plotPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!e.ClipRectangle.IsEmpty)
            {
                #region pre-init

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                float width = this.Width;
                float height = this.Height;

                e.Graphics.TranslateTransform(width / 2.0f, height / 2.0f);                

                #endregion



            }
        }

        #endregion

        #endregion

        #region logger

        private void logger_TextAdded(object sender, TextAddedEventArgs e)
        {
            if (historyTxb.InvokeRequired)
                historyTxb.Invoke((MethodInvoker)delegate { historyTxb.AppendText(e.Text); });
            else
                historyTxb.AppendText(e.Text);
        }

        #endregion        

        #region gtrPort

        private void gtrPort_Error(object sender, SerialErrorReceivedEventArgs e)
        {
            logger.Write(string.Format("{0} (RedGTR) >> {1}", gtrPort.PortName, e.EventType.ToString()));
        }

        private void gtrPort_NewNMEAMessage(object sender, NewNMEAMessageEventArgs e)
        {
            logger.Write(string.Format("{0} (RedGTR) >> {1}", gtrPort.PortName, e.Message));            

            try
            {
                var result = NMEAParser.Parse(e.Message);

                if (result is NMEAProprietarySentence)
                {
                    NMEAProprietarySentence pResult = result as NMEAProprietarySentence;

                    if (pResult.Manufacturer == ManufacturerCodes.TNT)
                    {
                        if (pResult.SentenceIDString == "0") // ACK
                            Parse_ACK(pResult.parameters);
                        else if (pResult.SentenceIDString == "!") // Device info
                            Parse_DEVICE_INFO(pResult.parameters);
                        else if (pResult.SentenceIDString == "5")
                            Parse_LOC_DATA_VAL(pResult.parameters);
                        else if (pResult.SentenceIDString == "9")
                            Parse_REM_RECEIVED(pResult.parameters);
                        else if (pResult.SentenceIDString == "B")
                            Parse_REM_TOUT(pResult.parameters);
                        else if (pResult.SentenceIDString == "C")
                            Parse_REM_PONG(pResult.parameters);
                        else if (pResult.SentenceIDString == "D")
                            Parse_REM_PONGEX(pResult.parameters);
                        else if (pResult.SentenceIDString == "O")
                            Parse_PRETMP_VAL(pResult.parameters);                          
                    }
                    else
                    {
                        // not supported manufacturer code
                    }
                }
                else
                {
                    NMEAStandartSentence sSentence = result as NMEAStandartSentence;

                    if (sSentence.SentenceID == SentenceIdentifiers.RMC)
                        Parse_RMC(sSentence.parameters);
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex, false);
            }            
        }

        #endregion

        #region timer

        private void timer_Tick(object sender, EventArgs e)
        {
            if (gtr_isWaiting)
            {
                if (++gtr_timeoutCounter > gtr_Timeout)
                {                    
                    gtr_OnTimeout();                    
                }
            }
            else if (gtr_isWaitingRemote)
            {
                if (++gtr_remoteTimeoutCounter > gtr_RemoteTimeout_S)
                {                 
                    gtr_OnRemotTimeout();
                }
            }
            else
            {
                if (gtrPort.IsOpen)
                {
                    if (!gtr_deviceInfoUpdated)
                    {
                        // query device info
                        gtr_DeviceInfoQuery();
                    }
                    else if (!gtr_salinityUpdated)
                    {
                        // query salinity set
                        gtr_QuerySalinitySet(settingsProvider.Data.Salinity);
                    }
                    else if (isAutoquery)
                    {
                        // query remote
                        CDS_CMD cmdID = CDS_CMD.CDS_CMD_DPT;
                        if (!bTemperature.IsInitialized || bTemperature.IsObsolete)
                            cmdID = CDS_CMD.CDS_CMD_TMP;

                        gtr_QueryRemote(settingsProvider.Data.TargetAddr, cmdID, gtr_RemoteTimeout_S * 1000);
                    }
                }
            }

            /// DEBUG
            /// 
            //double ranLat = 48 + rnd.NextDouble();
            //double ranLon = 44 + rnd.NextDouble();

            //measurements.Add(new Measurement(ranLat, ranLon, rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble()));

            //double refLat = 0;
            //double refLon = 0;
            //measurements.CenterOfMass(out refLat, out refLon);

            //measurements.UpdateReferencePoint(refLat, refLon);
            //if (measurements.IsBaseExists)
            //{
            //    var bs = measurements.GetBase();

            //    List<PointF> bsp = new List<PointF>();
            //    foreach (var bsItem in bs)
            //    {
            //        bsp.Add(new PointF(Convert.ToSingle(bsItem.Latitude), Convert.ToSingle(bsItem.Longitude)));
            //    }

            //    marinePlot.Invoke((MethodInvoker)delegate
            //    {
            //        marinePlot.UpdateTrack("MEASUREMENTS", ranLat, ranLon);
            //        marinePlot.UpdateTrack("BASE", bsp.ToArray());

            //        marinePlot.Invalidate();
            //    });
            //}            
        }

        #endregion                                        
        
        #endregion                
    }
}
