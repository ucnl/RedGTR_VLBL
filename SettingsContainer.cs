using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UCNLDrivers;

namespace RedGTR_VLBL
{
    [Serializable]
    public class SettingsContainer
    {
        #region Properties

        public string GTRPortName;

        public bool IsGNSSEmulator;
        public string GNSSEmulatorPortName;
                
        public int MaxDistance;

        public double Salinity;

        public int MeasurementsFIFOSize;

        public int BaseSize;

        public int TargetAddr;

        public double RadialErrorThreshold;

        #endregion

        #region Constructor

        public SettingsContainer()
        {
            SetDefaults();
        }

        #endregion

        #region Methods

        public void SetDefaults()
        {
            GTRPortName = "COM1";
            IsGNSSEmulator = false;
            GNSSEmulatorPortName = "COM1";
            MaxDistance = 1000;
            Salinity = 0.0;           
            MeasurementsFIFOSize = 100;
            BaseSize = 5;
            TargetAddr = 0;
            RadialErrorThreshold = 25;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("\r\nSettings:\r\n");
            sb.AppendFormat(CultureInfo.InvariantCulture, "GTRPortName = {0}\r\n", GTRPortName);
            sb.AppendFormat(CultureInfo.InvariantCulture, "IsGNSSEmulator = {0}, GNSSEmulatorPortName = {1}\r\n", IsGNSSEmulator, GNSSEmulatorPortName);              
            
            sb.AppendFormat(CultureInfo.InvariantCulture, "MaxDistance = {0} m\r\n", MaxDistance);
            sb.AppendFormat(CultureInfo.InvariantCulture, "Salinity = {0:F01} PSU\r\n", Salinity);
            sb.AppendFormat(CultureInfo.InvariantCulture, "MeasurementsFIFOSize = {0}\r\n", MeasurementsFIFOSize);
            sb.AppendFormat(CultureInfo.InvariantCulture, "BaseSize = {0}\r\n", BaseSize);
            sb.AppendFormat(CultureInfo.InvariantCulture, "TargetAddr = {0}\r\n", TargetAddr);
            sb.AppendFormat(CultureInfo.InvariantCulture, "RadialErrorThreshold = {0:F03} m\r\n", RadialErrorThreshold);

            return sb.ToString();
        }

        #endregion
    }
}
