using System;
using System.Collections.Generic;

namespace RedGTR_VLBL
{
    public class Measurements
    {
        #region Properties

        double refLat = double.NaN;
        public double RefLat
        {
            get { return refLat; }
        }

        double refLon = double.NaN;
        public double RefLon
        {
            get { return refLon; }
        }

        public bool IsRefPointSet
        {
            get
            {
                return !double.IsNaN(refLat) && !double.IsNaN(refLon);
            }
        }

        public bool IsBaseExists
        {
            get
            {
                return byDistance.Count >= BaseSize + 1;
            }
        }       

        public double DistanceRange
        {
            get
            {
                if (byDistance.Count > 0)
                {
                    return Math.Abs(byDistance[byDistance.Count - 1].Distance_m - byDistance[0].Distance_m);
                }
                else
                    return double.NaN;
            }
        }

        public double TimeRange
        {
            get
            {
                if (byTimeStamp.Count > 0)
                {
                    return Math.Abs(byTimeStamp[byTimeStamp.Count - 1].TimeStamp.Subtract(byTimeStamp[0].TimeStamp).TotalSeconds);
                }
                else
                    return double.NaN;
            }
        }

        double angleRange = 0.0;
        public double AngleRange
        {
            get
            {
                return angleRange;
            }
        }


        List<Measurement> byAzimuth;
        List<Measurement> byTimeStamp;
        List<Measurement> byDistance;

        AzimuthComparer aComparer = new AzimuthComparer();
        DistanceComparer dComparer = new DistanceComparer();
        TimeStampComperer tComparer = new TimeStampComperer();

        public Measurement[] ToArray()
        {
            return byTimeStamp.ToArray();
        }

        public int MaxSize { get; private set; }

        public int BaseSize { get; private set; }

        public int Count
        {
            get
            {
                return byAzimuth.Count;
            }
        }

        public Measurement Nearest
        {
            get
            {
                if (byDistance.Count > 0)
                    return byDistance[0];
                else
                    return null;
            }
        }

        #endregion

        #region Constructor

        public Measurements(int maxSize, int baseSize)
        {
            if (maxSize < 3) throw new ArgumentOutOfRangeException("Maximal size of measurements list cannot be less than 3");
            MaxSize = maxSize;

            if (baseSize < 3) throw new ArgumentOutOfRangeException("Base size cannot be less than 3");
            BaseSize = baseSize;

            byAzimuth = new List<Measurement>();
            byDistance = new List<Measurement>();
            byTimeStamp = new List<Measurement>();                       
        }

        #endregion

        #region Methods

        private void Resort()
        {
            byTimeStamp.Sort(tComparer);
            byDistance.Sort(dComparer);
            byAzimuth.Sort(aComparer);     
        }

        public void Clear()
        {
            byDistance.Clear();
            byAzimuth.Clear();
            byTimeStamp.Clear();

            refLon = double.NaN;
            refLat = double.NaN;
        }        

        public void UpdateReferencePoint(double lat, double lon)
        {
            if (double.IsNaN(lat) || (double.IsNaN(lon)))
                throw new ArgumentOutOfRangeException();

            refLat = lat;
            refLon = lon;

            foreach (var item in byAzimuth)
            {
                item.Alpha = Navigation.GetInitialBearingBy2Points(refLat, refLon, item.Latitude, item.Longitude);
            }

            Resort();
        }

        public void CenterOfMass(out double oLat, out double oLon)
        {
            if (byDistance.Count > 0)
            {

                oLat = 0.0;
                oLon = 0.0;

                foreach (var item in byDistance)
                {
                    oLat += item.Latitude;
                    oLon += item.Longitude;
                }

                oLat /= byDistance.Count;
                oLon /= byDistance.Count;
            }
            else
                throw new InvalidOperationException("Unable to calculate center of mass of an empy set of points");
            
        }

        public void Add(Measurement item)
        {
            if (!IsRefPointSet)
            {
                if (byDistance.Count == 0)
                {
                    refLat = item.Latitude;
                    refLon = item.Longitude;
                }
                else
                {
                    CenterOfMass(out refLat, out refLon);
                }
            }

            item.Alpha = Navigation.GetInitialBearingBy2Points(refLat, refLon, item.Latitude, item.Longitude);

            byTimeStamp.Add(item);
            byDistance.Add(item);
            byAzimuth.Add(item);

            if (byTimeStamp.Count > MaxSize)
            {
                var oldest = byTimeStamp[byTimeStamp.Count - 1];
                byTimeStamp.Remove(oldest);
                byDistance.Remove(oldest);
                byAzimuth.Remove(oldest);

            }

            Resort();

            double rs = 0;
            double re = 0;
            double r = 0;
            double maxGap = MeasureAngularGap(out rs, out re, out r);
            angleRange = 360 - maxGap;
        }
                  
        private double Variance(double[] items)
        {
            double mean = 0.0;
            double variance = 0.0;

            for (int i = 0; i < items.Length; i++)
            {
                mean += items[i];
            }

            mean /= items.Length;

            
            for (int i = 0; i < items.Length; i++)
            {
                variance += (items[i] - mean) * (items[i] - mean);
            }

            variance /= items.Length;

            return Math.Sqrt(variance);
        }

        private double MeasureAngularGap(out double rangeStart, out double rangeEnd, out double range)
        {
            int maxGapStartIdx = 0;
            int maxGapEndIdx = 1;
            double maxGap = byAzimuth[maxGapEndIdx].Alpha - byAzimuth[maxGapStartIdx].Alpha;
            double gap;

            int idx = 2;
            int lIdx, rIdx;
            while (idx <= byAzimuth.Count)
            {
                lIdx = idx - 1;
                rIdx = idx;

                if (rIdx >= byAzimuth.Count)
                    rIdx = rIdx % byAzimuth.Count;

                gap = byAzimuth[rIdx].Alpha - byAzimuth[lIdx].Alpha;

                if (gap < 0)
                    gap = gap + 360;

                if (gap > maxGap)
                {
                    maxGap = gap;
                    maxGapStartIdx = lIdx - 1;
                    maxGapEndIdx = rIdx;
                }

                idx = idx + 1;
            }

            rangeStart = byAzimuth[maxGapEndIdx].Alpha;
            rangeEnd = byAzimuth[maxGapStartIdx].Alpha;
            range = rangeEnd - rangeStart;

            if (range < 0)
                range = range + 360;    

            return maxGap;
        }
       
        public List<Measurement> GetBase()
        {
            if (!IsBaseExists)
                throw new InvalidOperationException("Base does not exists");

            List<Measurement> result = new List<Measurement>();
            
            #region find max gap

            double aRangeStart = 0;
            double aRangeEnd = 0;
            double aRange = 0;
            double maxGap = MeasureAngularGap(out aRangeStart, out aRangeEnd, out aRange);
            angleRange = 360 - maxGap;

            #endregion

            #region arrange desired angles
            
            double delta = aRange / (BaseSize - 1);

            if (maxGap < delta)
            {
                double deltaDec = (delta - maxGap) * (BaseSize - 1) / (BaseSize - 2); 
                delta = delta - deltaDec / BaseSize;
            }
            
            double[] desiredAngles = new double[BaseSize];

            for (int i = 0; i < BaseSize; i++)
            {
                desiredAngles[i] = aRangeStart + i * delta;
                while (desiredAngles[i] > 360)
                    desiredAngles[i] -= 360;
            }

            #endregion

            #region select nearest to desired angles

            double minDiff = double.MaxValue;
            int minDiffIdx = 0;
            double diff;
            for (int i = 0; i < BaseSize; i++)
            {
                minDiff = double.MaxValue;
                minDiffIdx = 0;

                for (int j = 0; j < byAzimuth.Count; j++)
                {
                    if (!result.Contains(byAzimuth[j]))
                    {
                        diff = desiredAngles[i] - byAzimuth[j].Alpha;
                        if (diff < 0)
                            diff += 360;

                        if (diff < minDiff)
                        {
                            minDiff = diff;
                            minDiffIdx = j;
                        }
                    }
                }

                result.Add(byAzimuth[minDiffIdx]);
            }
            
            #endregion
            
            if (!result.Contains(byDistance[0]))
                result.Add(byDistance[0]);

            result.Sort(dComparer);

            return result;
        }

        #endregion
    }
}
