using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RedGTR_VLBL
{
    public partial class MarinePlot : UserControl
    {
        #region Properties
      
        int trackLength = 100;
        Dictionary<string, FixedSizeLIFO<PointF>> tracks;

        Dictionary<string, Color> trackColors;
        Dictionary<string, float> trackPenSizes;
        Dictionary<string, Pen> trackPens;
        Dictionary<string, bool> latestPointMarks;

        float meanLatDeg = 0.0f;
        float meanLonDeg = 0.0f;
        float farestLatDeltaDeg = 0.0f;
        float farestLonDeltaDeg = 0.0f;


        int borderGap = 5;
        int miscFntSize = 5;


        Color miscInfoClr = Color.Black;

        Color defaultPointColor = Color.Blue;
        float defaultTrackPointSize = 4.0f;

        Pen gridPen = new Pen(Brushes.Black, 1.0f);
       
        Font miscInfoFnt;
        Brush miscInfoBrs;

        string legendLbl = string.Empty;


        string leftUpperCornerText = string.Empty;

        public string LeftUpperCornerText
        {
            get { return leftUpperCornerText; }
            set { leftUpperCornerText = value; }
        }
        string leftBottomLine = string.Empty;

        public string LeftBottomLine
        {
            get { return leftBottomLine; }
            set { leftBottomLine = value; }
        }


        #endregion

        #region Constructor

        public MarinePlot()
        {
            InitializeComponent();

            tracks = new Dictionary<string, FixedSizeLIFO<PointF>>();
            trackColors = new Dictionary<string, Color>();
            trackPenSizes = new Dictionary<string, float>();
            trackPens = new Dictionary<string, Pen>();
            latestPointMarks = new Dictionary<string, bool>();


            miscInfoBrs = new SolidBrush(miscInfoClr);
            miscInfoFnt = new System.Drawing.Font("Consolas", miscFntSize, GraphicsUnit.Millimeter);

        }

        #endregion

        #region Methods

        #region Private

        private PointF DegToMeters(double lon1, double lat1, double lon2, double lat2)
        {
            double rLat = ((lat1 + lat2) / 2.0f) * Math.PI / 180.0f;
            double meterPerDegLat = 111132.92f - 559.82f * Math.Cos(2.0f * rLat) + 1.175f * Math.Cos(4.0f * rLat);
            double meterPerDegLon = 111412.84f * Math.Cos(rLat) - 93.5f * Math.Cos(3.0f * rLat);

            PointF result = new PointF();

            result.X = (float)((lon1 - lon2) * meterPerDegLon);
            result.Y = (float)((lat1 - lat2) * meterPerDegLat);

            return result;
        }

        private float GetDistance2DM(double lon1, double lat1, double lon2, double lat2)
        {
            var result = DegToMeters(lon1, lat1, lon2, lat2);
            return (float)Math.Sqrt(result.X * result.X + result.Y * result.Y);
        }


        #endregion

        public void InitTracks(int trLength)
        {
            trackLength = trLength;
        }

        public void AddTrack(string key, Color color, float penWidth, int pointSize, int trackSize, bool isMarkLatestPoint)
        {
            if (tracks.ContainsKey(key))
                throw new ArgumentException("Track with specified key is already exists");

            tracks.Add(key, new FixedSizeLIFO<PointF>(trackSize));
            trackColors.Add(key, color);
            trackPenSizes.Add(key, pointSize);
            trackPens.Add(key, new Pen(color, penWidth));
            latestPointMarks.Add(key, isMarkLatestPoint);

            UpdateLegendLabel();
        }

        private void UpdateLegendLabel()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var trk in tracks)
                sb.AppendFormat("{0}\r\n", trk.Key);

            legendLbl = sb.ToString();
        }

        private void RefreshTrackStatistics()
        {
            double meanLat = 0.0;
            double meanLon = 0.0;
            int cnt = 0;

            foreach (var item in tracks)
            {
                var points = item.Value.ToArray();
                for (int i = 0; i < points.Length; i++)
                {
                    meanLon += points[i].X;
                    meanLat += points[i].Y;
                }

                cnt += points.Length;
            }

            meanLon /= cnt;
            meanLat /= cnt;


            /// TODO: find farest point for lat and lon
            /// 
            double farestX = 0;
            double farestY = 0;
            foreach (var item in tracks)
            {
                var points = item.Value.ToArray();
                for (int i = 0; i < points.Length; i++)
                {
                    if (Math.Abs(meanLat - points[i].Y) > farestY)
                        farestY = Math.Abs(meanLat - points[i].Y);
                    if (Math.Abs(meanLon - points[i].X) > farestX)
                        farestX = Math.Abs(meanLon - points[i].X);
                }
            }

            meanLatDeg = Convert.ToSingle(meanLat);
            meanLonDeg = Convert.ToSingle(meanLon);
            farestLatDeltaDeg = Convert.ToSingle(farestY);
            farestLonDeltaDeg = Convert.ToSingle(farestX);
        }

        public void UpdateTrack(string tKey, PointF[] pnts)
        {
            if (!tracks.ContainsKey(tKey))
                AddTrack(tKey, defaultPointColor, defaultTrackPointSize, 4, trackLength, true);

            foreach (var pnt in pnts)
                tracks[tKey].Add(pnt);

            RefreshTrackStatistics();            
        }

        public void UpdateTrack(string tKey, double lat, double lon)
        {
            if (!tracks.ContainsKey(tKey))
                AddTrack(tKey, defaultPointColor, defaultTrackPointSize, 4, trackLength, true);

            tracks[tKey].Add(new PointF(Convert.ToSingle(lat), Convert.ToSingle(lon)));
            RefreshTrackStatistics();            
        }

        public void ClearTracks()
        {
            foreach (var item in tracks)
                item.Value.Clear();
        }

        #endregion        

        #region Handlers

        private void MarinePlot_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (!e.ClipRectangle.IsEmpty)
            {                
                e.Graphics.TranslateTransform(this.Width / 2.0f, this.Height / 2.0f);


                if (tracks.Count > 0)
                {
                    float farestXDelta = 0.0f;
                    float farestYDelta = 0.0f;

                    Dictionary<string, PointF[]> deltas = new Dictionary<string, PointF[]>();

                    foreach (var track in tracks)
                    {
                        var trkPoints = track.Value.ToArray();
                        var trkDeltas = new PointF[trkPoints.Length];
                        deltas.Add(track.Key, trkDeltas);
                        for (int i = 0; i < trkPoints.Length; i++)
                        {
                            trkDeltas[i] = DegToMeters(meanLonDeg, meanLatDeg, trkPoints[i].X, trkPoints[i].Y);
                            if (Math.Abs(trkDeltas[i].X) > farestXDelta) farestXDelta = Math.Abs(trkDeltas[i].X);
                            if (Math.Abs(trkDeltas[i].Y) > farestYDelta) farestYDelta = Math.Abs(trkDeltas[i].Y);
                        }
                    }

                    float farestDistM = (float)(Math.Max(farestXDelta, farestYDelta) * 1.1);

                    if (farestDistM < float.Epsilon)
                        farestDistM = 0.000001f;

                    float xScale = this.Width / (farestDistM * 2.0f);
                    float yScale = this.Height / (farestDistM * 2.0f);
                    float scale = (float)Math.Min(xScale, yScale);


                    float fDist = Math.Abs((float)farestDistM * 0.75f);
                    float fDistScaled = (float)(fDist * scale);

                    e.Graphics.DrawLine(gridPen, -fDistScaled, -4, -fDistScaled, 4);
                    e.Graphics.DrawLine(gridPen, fDistScaled, -4, fDistScaled, 4);
                    e.Graphics.DrawLine(gridPen, -4, -fDistScaled, 4, -fDistScaled);
                    e.Graphics.DrawLine(gridPen, -4, fDistScaled, 4, fDistScaled);

                    var fDistMLbl = fDist.ToString("0.0 m");
                    var fDistMLblSize = e.Graphics.MeasureString(fDistMLbl, this.Font);

                    e.Graphics.DrawString(fDistMLbl, this.Font, Brushes.Black,
                        -fDistScaled - fDistMLblSize.Width / 2, -fDistMLblSize.Height - 2);

                    e.Graphics.DrawString(fDistMLbl, this.Font, Brushes.Black,
                        fDistScaled - fDistMLblSize.Width / 2, -fDistMLblSize.Height - 2);

                    e.Graphics.DrawString(fDistMLbl, this.Font, Brushes.Black,
                        -fDistMLblSize.Width, -fDistScaled - fDistMLblSize.Height / 2);

                    e.Graphics.DrawString(fDistMLbl, this.Font, Brushes.Black,
                        -fDistMLblSize.Width, fDistScaled - fDistMLblSize.Height / 2);


                    float minDim = this.Height;
                    if (minDim > this.Width)
                        minDim = this.Width;

                    //float left = -this.Width / 2.0f;
                    //float right = this.Width / 2.0f;
                    //float top = -this.Height / 2.0f;
                    //float bottom = this.Height / 2.0f;

                    float left = -minDim / 2.0f;
                    float right = minDim / 2.0f;
                    float top = -minDim / 2.0f;
                    float bottom = minDim / 2.0f;

                    e.Graphics.DrawLine(gridPen, left, 0.0f, right, 0.0f);
                    e.Graphics.DrawLine(gridPen, 0.0f, top, 0.0f, bottom);


                    float xToDraw;
                    float yToDraw;

                    foreach (var track in deltas)
                    {
                        float pSize = trackPenSizes[track.Key];

                        for (int i = 0; i < track.Value.Length; i++)
                        {
                            xToDraw = -track.Value[i].X * scale;
                            yToDraw = track.Value[i].Y * scale;

                            if ((i == 0) && (latestPointMarks[track.Key]))
                            {
                                e.Graphics.DrawRectangle(trackPens[track.Key],
                                                         xToDraw - pSize * 2,
                                                         yToDraw - pSize * 2,
                                                         pSize * 4,
                                                         pSize * 4);
                                
                                e.Graphics.FillRectangle(trackPens[track.Key].Brush,
                                                         xToDraw - pSize * 2,
                                                         yToDraw - pSize * 2,
                                                         pSize * 4,
                                                         pSize * 4);
                            }
                            else
                            {
                                e.Graphics.DrawRectangle(trackPens[track.Key],
                                                         xToDraw - pSize,
                                                         yToDraw - pSize,
                                                         pSize * 2,
                                                         pSize * 2);
                                
                                e.Graphics.FillRectangle(trackPens[track.Key].Brush,
                                                         xToDraw - pSize,
                                                         yToDraw - pSize,
                                                         pSize * 2,
                                                         pSize * 2);
                            }
                        }                                                
                    }
                }

                #region draw additional info

                e.Graphics.DrawString(leftUpperCornerText, miscInfoFnt, miscInfoBrs, -this.Width / 2 + borderGap, -this.Height / 2 + borderGap);

                var actionLbl = string.Format("{0}", leftBottomLine);
                var actionLblSize = e.Graphics.MeasureString(actionLbl, miscInfoFnt);
                e.Graphics.DrawString(actionLbl, miscInfoFnt, miscInfoBrs, -this.Width / 2 + borderGap, this.Height / 2 - borderGap - actionLblSize.Height);                

                #endregion

                #region draw legend
                
                var legendLblSize = e.Graphics.MeasureString(legendLbl, miscInfoFnt);
                e.Graphics.DrawString(legendLbl, miscInfoFnt, miscInfoBrs, this.Width / 2 - borderGap - legendLblSize.Width, -this.Height / 2 + borderGap);

                float delta = legendLblSize.Height / tracks.Count;

                int cnt = 0;
                foreach (var trk in tracks)
                {
                    float pSize = trackPenSizes[trk.Key];
                    float xToDraw = this.Width / 2 - borderGap - legendLblSize.Width - borderGap * 3;
                    float yToDraw = -this.Height / 2 + borderGap + delta * cnt + delta / 2;
                    cnt++;
                    
                    e.Graphics.DrawRectangle(trackPens[trk.Key],
                                                       xToDraw - pSize,
                                                       yToDraw - pSize,
                                                       pSize * 2,
                                                       pSize * 2);

                    e.Graphics.FillRectangle(trackPens[trk.Key].Brush,
                                             xToDraw - pSize,
                                             yToDraw - pSize,
                                             pSize * 2,
                                             pSize * 2);

                }
                
                #endregion
            }
        }

        private void MarinePlot_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void MarinePlot_MouseClick(object sender, MouseEventArgs e)
        {
            /// TODO:
        }

        private void MarinePlot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /// TODO:
        }

        private void MarinePlot_MouseMove(object sender, MouseEventArgs e)
        {
            /// TODO:
        }

        #endregion
    }
}
