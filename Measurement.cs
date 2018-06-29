using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RedGTR_VLBL
{
    public class AzimuthComparer : IComparer<Measurement>
    {
        public int Compare(Measurement first, Measurement second)
        {
            return first.Alpha.CompareTo(second.Alpha);
        }
    }

    public class DistanceComparer : IComparer<Measurement>
    {
        public int Compare(Measurement first, Measurement second)
        {
            return first.Distance_m.CompareTo(second.Distance_m);
        }
    }

    public class TimeStampComperer : IComparer<Measurement>
    {
        public int Compare(Measurement first, Measurement second)
        {
            return first.TimeStamp.CompareTo(second.TimeStamp);
        }
    }

    public abstract class AgingValue
    {
        public bool IsInitialized { get; protected set; }

        public DateTime TimeStamp { get; protected set; }

        public int ObsoleteIntervalSec { get; set; }

        public void ForceUpdate()
        {
            TimeStamp = DateTime.Now;
        }

        public int MinSecToShowAge { get; set; }

        public TimeSpan Age
        {
            get
            {
                return DateTime.Now.Subtract(TimeStamp);
            }
        }

        public bool IsObsolete
        {
            get
            {
                return Age.TotalSeconds > ObsoleteIntervalSec;
            }
        }

        public abstract string SpecificToString();

        public override string ToString()
        {
            if (!IsInitialized)
            {
                return "- - -";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(SpecificToString());

                TimeSpan age = Age;

                if (Age.TotalSeconds > MinSecToShowAge)
                {
                    if (age.TotalSeconds > ObsoleteIntervalSec)
                        sb.Append(" (OBS)");
                    else
                        sb.AppendFormat(" ({0:00}:{1:00})", age.Minutes, age.Seconds);
                }

                return sb.ToString();
            }
        }

        public AgingValue()
        {
            IsInitialized = false;
            MinSecToShowAge = 5;
            ObsoleteIntervalSec = 600;
        }
    }

    public class AgingDouble : AgingValue
    {
        double val;
        public double Value
        {
            get
            {
                return val;
            }
            set
            {
                IsInitialized = true;
                val = value;
                TimeStamp = DateTime.Now;
            }
        }

        public string Units { get; set; }
        public string FormatString { get; set; }

        public override string SpecificToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:" + FormatString + "}{1}", Value, Units);
        }

        public AgingDouble(double value, string fString, string units)
        {
            Value = value;
            FormatString = fString;
            Units = units;
        }

        public AgingDouble(string fString, string units)
        {
            FormatString = fString;
            Units = units;
        }

        public AgingDouble()
        {
            FormatString = "F06";
            Units = string.Empty;
        }
    }

    public class AgingBool : AgingValue
    {
        bool val;
        public bool Value
        {
            get
            {
                return val;
            }
            set
            {
                IsInitialized = true;
                val = value;
                TimeStamp = DateTime.Now;
            }
        }

        public string FormatString { get; set; }

        public override string SpecificToString()
        {
            return FormatString;
        }

        public AgingBool(string fString)
        {
            FormatString = fString;
        }
    }

    public class AgingString : AgingValue
    {
        string val;
        public string Value
        {
            get
            {
                return val;
            }
            set
            {
                IsInitialized = true;
                val = value;
                TimeStamp = DateTime.Now;
            }
        }

        public override string SpecificToString()
        {
            return val;
        }

        public AgingString()
        {
            //
        }

        public AgingString(string value)
        {
            Value = value;
        }
    }

    public class Measurement
    {
        #region Properties

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public double Distance_m { get; private set; }
        public double SNR_dB { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public double OwnDepth { get; private set; }        

        public double Alpha { get; set;  }

        #endregion

        #region Constructor

        public Measurement(double lat, double lon, double dist_m, double snr_db, double ownDpt)
            : this(lat, lon, dist_m, snr_db, ownDpt, DateTime.Now)
        {
        }

        public Measurement(double lat, double lon, double dist_m, double snr_db, double ownDpt, DateTime tStamp)
        {
            if (double.IsNaN(lat) || double.IsNaN(lon) ||
                double.IsNaN(dist_m) || double.IsNaN(snr_db))
                throw new ArgumentOutOfRangeException();

            Latitude = lat;
            Longitude = lon;
            Distance_m = dist_m;
            SNR_dB = snr_db;
            OwnDepth = ownDpt;
            TimeStamp = tStamp;
        }

        #endregion        
    }
}
