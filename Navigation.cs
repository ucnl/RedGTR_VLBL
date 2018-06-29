using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedGTR_VLBL
{
    public struct GeoPoint
    {
        public double Latitude;
        public double Longitude;
    }

    public struct GeoPoint3D
    {
        public double Latitude;
        public double Longitude;
        public double Depth;
    }

    public struct GeoPoint3DWE
    {
        public double Latitude;
        public double Longitude;
        public double Depth;
        public double RadialError;
    }

    public class Navigation
    {
        #region Properties

        public const double NAV_STANDARD_EARTH_RADIUS = 6366707.1;
        public const double NAV_STANDARD_ONE_DEGREE_CURVATURE_LENGTH = 111120.0;
        public const double NAV_STANDARD_MILE = 1852.0;

        public const double WGS84_MAJOR_SEMIAXIS = 6378137.0;
        public const double WGS84_INVERSE_FLATTERING = 298.257223563;
        public const double WGS84_FLATTERING = 1.0 / WGS84_INVERSE_FLATTERING;
        public const double WGS84_MINOR_SEMIAXIS = WGS84_MAJOR_SEMIAXIS * (1.0 - WGS84_FLATTERING);
        public const double WGS84_ECCINTRICITY_SQUARED = (1.0 - WGS84_MINOR_SEMIAXIS * WGS84_MINOR_SEMIAXIS / WGS84_MAJOR_SEMIAXIS / WGS84_MAJOR_SEMIAXIS);

        public const double PI_DBY_180 = Math.PI / 180.0;
        public const double PI_MBY_180 = Math.PI * 180.0;

        public const double NAV_NLM_A = 1.0;
        public const double NAV_NLM_B = 0.5;
        public const double NAV_NLM_G = 2.0;
        public const int NAV_NLM_MIT = 100;
        public const double NAV_NLM_EPSILON = 0.0001;        

        #endregion

        #region Constructor


        #endregion

        #region Methods

        #region Private


        #endregion

        #region Public

        public static double DegSum360(double a1, double a2)
        {
            double result = a1 + a2;

            while (result > 360.0)
                result -= 360.0;

            return result;
        }

        public static double GetDistance2DM(double sLat, double sLon, double eLat, double eLon)
        {
            double dLat = (eLat - sLat) * PI_DBY_180;
            double dLon = (eLon - sLon) * PI_DBY_180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                      Math.Cos(sLat * PI_DBY_180) * Math.Cos(eLat * PI_DBY_180) *
                      Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return NAV_STANDARD_EARTH_RADIUS * c;
        }


        // Converts degrees to meters depending on current geolocation
        public static void GetDistance2DDeg(double sLat, double sLon, double eLat, double eLon, out double dLat, out double dLon)
        {
            double rLat = (sLat + eLat) / 2.0 * PI_DBY_180;

            double meterPerDegLat = 111132.92 - 559.82 * Math.Cos(2.0 * rLat) + 1.175 * Math.Cos(4.0 * rLat);
            double meterPerDegLon = 111412.84 * Math.Cos(rLat) - 93.5 * Math.Cos(3.0 * rLat);

            dLon = (sLon - eLon) * meterPerDegLon;
            dLat = (sLat - eLat) * meterPerDegLat;
        }   

        // Converts meters to degrees depending on current geolocation
        public static void Meters2Deg(double lon1, double lat1, double dLonM, double dLatM, out double nLon, out double nLat)
        {
            double rLat = lat1 * PI_DBY_180;

            double meterPerDegLat = 111132.92 - 559.82 * Math.Cos(2.0 * rLat) + 1.175 * Math.Cos(4.0 * rLat);
            double meterPerDegLon = 111412.84 * Math.Cos(rLat) - 93.5 * Math.Cos(3.0 * rLat);

            nLon = lon1 - dLonM / meterPerDegLon;
            nLat = lat1 - dLatM / meterPerDegLat;
        }

        public static double GetSlantRangeProjection(double sRange, double dpt1, double dpt2)
        {
            return Math.Sqrt(sRange * sRange - ((dpt1 - dpt2) * (dpt1 - dpt2)));
        }

        /// <summary>
        /// Calculates initial bearing to end point
        /// </summary>
        /// <param name="sLat">Start point latitude, degrees</param>
        /// <param name="sLon">Start point longitude, degrees</param>
        /// <param name="eLat">End point latitude, degrees</param>
        /// <param name="eLon">End point longitude, degrees</param>
        public static double GetInitialBearingBy2Points(double sLat, double sLon, double eLat, double eLon)
        {
            //var y = Math.sin(λ2 - λ1) * Math.cos(φ2);
            //var x = Math.cos(φ1) * Math.sin(φ2) -
            //        Math.sin(φ1) * Math.cos(φ2) * Math.cos(λ2 - λ1);
            //var brng = Math.atan2(y, x).toDegrees();

            double lt1 = sLat * Math.PI / 180.0;
            double ln1 = sLon * Math.PI / 180.0;
            double lt2 = eLat * Math.PI / 180.0;
            double ln2 = eLon * Math.PI / 180.0;

            double y = Math.Sin(ln2 - ln1) * Math.Cos(lt2);
            double x = Math.Cos(lt1) * Math.Sin(lt2) - Math.Sin(lt1) * Math.Cos(lt2) * Math.Cos(ln2 - ln1);

            return (360.0 + Math.Atan2(y, x) * 180.0 / Math.PI) % 360.0;
        }

        /// <summary>
        /// Calculates final bearing to end point
        /// </summary>
        /// <param name="sLat">Start point latitude, degrees</param>
        /// <param name="sLon">Start point longitude, degrees</param>
        /// <param name="eLat">End point latitude, degrees</param>
        /// <param name="eLon">End point longitude, degrees</param>
        public static double GetFinalBearingBy2Points(double sLat, double sLon, double eLat, double eLon)
        {
            return (GetInitialBearingBy2Points(eLat, eLon, sLat, sLon) + 180.0) % 360.0;
        }

        /// <summary>
        /// Calculates geographic location of an end point by the start point, distance and bearing
        /// </summary>
        /// <param name="sLat">Start point latitude, degrees</param>
        /// <param name="sLon">Start point longitude, degrees</param>
        /// <param name="distance_m">Distance, meters</param>
        /// <param name="bearing_deg">Initial bearing, degrees</param>
        /// <param name="eLat">End point latitude, degrees</param>
        /// <param name="eLon">End point longitude, degrees</param>
        public static void GetPointByDDB(double sLat, double sLon, double distance_m, double bearing_deg, out double eLat, out double eLon)
        {
            double lt1 = sLat * Math.PI / 180.0;
            double ln1 = sLon * Math.PI / 180.0;
            double teta = bearing_deg * Math.PI / 180.0;
            double delta = distance_m / NAV_STANDARD_EARTH_RADIUS;

            double lt2 = Math.Asin(Math.Sin(lt1) * Math.Cos(delta) + Math.Cos(lt1) * Math.Sin(delta) * Math.Cos(teta));
            double ln2 = ln1 + Math.Atan2(Math.Sin(teta) * Math.Sin(delta) * Math.Cos(lt1), Math.Cos(delta) - Math.Sin(lt1) * Math.Sin(lt2));

            eLat = lt2 * 180.0 / Math.PI;
            eLon = (540.0 + ln2 * 180.0 / Math.PI) % 360.0 - 180.0;
        }


        private static double EpsTOA(double x, double y, double z, double[] bx, double[] by, double[] bz, double[] bd)
        {
            double err = 0;
            double bv;

            for (int i = 0; i < bx.Length; i++)
            {
                bv = Math.Sqrt((x - bx[i]) * (x - bx[i]) + (y - by[i]) * (y - by[i]) + (z - bz[i]) * (z - bz[i])) - bd[i];                
                err += bv * bv;
            }

            return err;
        }


        public static double LocateLBL_1D(double z, double[] bx, double[] by, double[] bz, double[] bd, double aStart, double aEnd, double aStep)
        {
            double alpha = aStart;
            double alphaBest = aStart;
            double eps;
            double minEps = double.MaxValue;

            double x, y;          

            while (alpha < aEnd)
            {
                x = bx[0] + bd[0] * Math.Cos(alpha * PI_DBY_180);
                y = by[0] + bd[0] * Math.Sin(alpha * PI_DBY_180);

                eps = EpsTOA(x, y, z, bx, by, bz, bd);

                if (eps < minEps)
                {
                    minEps = eps;
                    alphaBest = alpha;                   
                }

                alpha += aStep;
            }

            return alphaBest;            
        }

        public static GeoPoint3DWE LocateLBL_NLM(List<Measurement> bases, GeoPoint3DWE previousLocation, double rErrThreshold, out double stStageRErr, out int nlm_itCount)
        {
            // bases sorted from nearest to farest

            #region variables

            double[] bx = new double[bases.Count]; // base x-coordinate
            double[] by = new double[bases.Count]; // base y-coordinate
            double[] bz = new double[bases.Count]; // base z-coordinate
            double[] bd = new double[bases.Count]; // distance to a base            

            double meanLat = 0;
            double meanLon = 0;

            double z = previousLocation.Depth;

            double radialError = double.NaN;

            double simplexSize = 25;

            #endregion

            #region bases mid-point

            foreach (var item in bases)
            {
                meanLat += item.Latitude;
                meanLon += item.Longitude;
            }

            meanLat /= bases.Count;
            meanLon /= bases.Count;

            #endregion

            #region translation of coordinates from degrees to meters

            for (int i = 0; i < bases.Count; i++)
            {
                Navigation.GetDistance2DDeg(meanLat, meanLon, bases[i].Latitude, bases[i].Longitude, out by[i], out bx[i]);
                bd[i] = bases[i].Distance_m;
                bz[i] = bases[i].OwnDepth;
            }            

            #endregion

            double xBest = double.NaN;
            double yBest = double.NaN;
            stStageRErr = double.NaN;

            if ((double.IsNaN(previousLocation.Latitude)) ||
                (double.IsNaN(previousLocation.Longitude)) ||
                (previousLocation.RadialError > rErrThreshold))
            {
                #region 1st stage 1D optimization

                double alpha = LocateLBL_1D(z, bx, by, bz, bd, 0, 360, 10);
                alpha = LocateLBL_1D(z, bx, by, bz, bd, alpha - 10, alpha + 10, 1);

                xBest = bx[0] + bd[0] * Math.Cos(alpha * PI_DBY_180);
                yBest = by[0] + bd[0] * Math.Sin(alpha * PI_DBY_180);

                stStageRErr = Math.Sqrt(EpsTOA(xBest, yBest, z, bx, by, bz, bd));
                simplexSize = stStageRErr;

                #endregion
            }
            else
            {
                Navigation.GetDistance2DDeg(meanLat, meanLon, previousLocation.Latitude, previousLocation.Longitude, out yBest, out xBest);
            }

            #region Nelder-Mead 2D optimization

            bool isFinished = false;
	        int itCnt = 0;
	        double tmp, tmp1;
	        double[] xix = new double[3];
	        double[] xiy = new double[3];
            double[] fxi = new double[3];
	        double fl, fg, fh, fr, fe, fs;
	        double xcx, xcy, xrx, xry, xex, xey, xsx, xsy;

            xix[0] = xBest;
            xiy[0] = yBest;
            xix[1] = xix[0] + simplexSize; //
            xiy[1] = xiy[0] + simplexSize; //
            xix[2] = xix[0] - simplexSize; //
            xiy[2] = xiy[0] + simplexSize; //
            
            while ((!isFinished) && (itCnt < NAV_NLM_MIT))
            {
                fxi[0] = EpsTOA(xix[0], xiy[0], z, bx, by, bz, bd);
                fxi[1] = EpsTOA(xix[1], xiy[1], z, bx, by, bz, bd);
                fxi[2] = EpsTOA(xix[2], xiy[2], z, bx, by, bz, bd);
                
                if (fxi[0] > fxi[1])
                {
                    tmp = fxi[0]; fxi[0] = fxi[1]; fxi[1] = tmp;
                    tmp = xix[0]; xix[0] = xix[1]; xix[1] = tmp;
                    tmp = xiy[0]; xiy[0] = xiy[1]; xiy[1] = tmp;
                }
                
                if (fxi[0] > fxi[2])
                {
                    tmp = fxi[0]; fxi[0] = fxi[2]; fxi[2] = tmp;
                    tmp = xix[0]; xix[0] = xix[2]; xix[2] = tmp;
                    tmp = xiy[0]; xiy[0] = xiy[2]; xiy[2] = tmp;
                }
                
                if (fxi[1] > fxi[2])
                {
                    tmp = fxi[1]; fxi[1] = fxi[2]; fxi[2] = tmp;
		            tmp = xix[1]; xix[1] = xix[2]; xix[2] = tmp;
		            tmp = xiy[1]; xiy[1] = xiy[2]; xiy[2] = tmp;
		        }
                
                fl = fxi[0];		
                fg = fxi[1];
                fh = fxi[2];
                
                xcx = (xix[0] + xix[1]) / 2.0f;
                xcy = (xiy[0] + xiy[1]) / 2.0f;
                
                xrx = (1.0f + NAV_NLM_A) * xcx - NAV_NLM_A * xix[2];
                xry = (1.0f + NAV_NLM_A) * xcy - NAV_NLM_A * xiy[2];

                fr = EpsTOA(xrx, xry, z, bx, by, bz, bd);
                
                if (fr < fl)
                {
                    xex = (1.0f - NAV_NLM_G) * xcx + NAV_NLM_G * xrx;
                    xey = (1.0f - NAV_NLM_G) * xcy + NAV_NLM_G * xry;

                    fe = EpsTOA(xex, xey, z, bx, by, bz, bd);
                    
                    if (fe < fr)
                    {
                        xix[2] = xex;
                        xiy[2] = xey;
                    }
                    else
                    {
				        xix[2] = xrx;
				        xiy[2] = xry;
			        }
		        }
		        else
		        {
			        if ((fr > fl) && (fr < fg))
			        {
				        xix[2] = xrx;
				        xiy[2] = xry;
			        }
			        else
			        {
				        if ((fr > fg) && (fr < fh))
				        {
					        tmp = xix[2]; xix[2] = xrx; xrx = tmp;
					        tmp = xiy[2]; xiy[2] = xry; xry = tmp;
					        tmp = fxi[2]; fxi[2] = fr;  fr = tmp;
				        }
				        else
				        {
					        if (fh < fr)
					        {
						        //
					        }
				        }

				        xsx = NAV_NLM_B * xix[2] + (1.0f - NAV_NLM_B) * xcx;
				        xsy = NAV_NLM_B * xiy[2] + (1.0f - NAV_NLM_B) * xcy;
                        fs = EpsTOA(xsx, xsy, z, bx, by, bz, bd);

				        if (fs < fh)
				        {
					        xix[2] = xsx;
					        xiy[2] = xsy;
				        }
				        else
				        {
					        xix[1] = (xix[1] - xix[0]) / 2.0f;
					        xiy[1] = (xiy[1] - xiy[0]) / 2.0f;
					        xix[2] = (xix[2] - xix[0]) / 2.0f;
					        xiy[2] = (xiy[2] - xiy[0]) / 2.0f;
				        }
			        }
		        }                

		        tmp = (fxi[0] + fxi[1] + fxi[2]) / 3.0f;
		        tmp1 = ((fxi[0] - tmp) * (fxi[0] - tmp) +
			            (fxi[1] - tmp) * (fxi[1] - tmp) +
			            (fxi[2] - tmp) * (fxi[2] - tmp)) / 3.0f;

		        isFinished = (Math.Sqrt(tmp1) <= NAV_NLM_EPSILON);
		        itCnt++;
	        }

	        xBest = xix[0];
	        yBest = xiy[0];


            radialError = Math.Sqrt(EpsTOA(xix[0], xiy[0], z, bx, by, bz, bd));
            nlm_itCount = itCnt;
            
            #endregion

            #region convert coordinates to degrees
            
            double targetLongitude = double.NaN;
            double targetLatitude = double.NaN;

            Meters2Deg(meanLon, meanLat, yBest, xBest, out targetLongitude, out targetLatitude);

            #endregion

            GeoPoint3DWE result = new GeoPoint3DWE();

            result.Depth = previousLocation.Depth;
            result.Latitude = targetLatitude;
            result.Longitude = targetLongitude;
            result.RadialError = radialError;

            return result;
        }


        #endregion

        #endregion
    }

}
