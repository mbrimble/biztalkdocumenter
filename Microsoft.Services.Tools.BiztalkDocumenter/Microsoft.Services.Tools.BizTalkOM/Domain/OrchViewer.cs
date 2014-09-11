
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Configuration;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Printing;
    using System.Windows.Forms;
    using Microsoft.VisualStudio.EFT;
    using Microsoft.VisualStudio.Forms.Internal;
    using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

    public enum ShapeType : int
    {
        Unknown,
        SendShape,
        MessageAssignment,
        VariableAssignment,
        ReceiveShape,
        TransformShape,
    }

    public class OrchMetrics
    {
        public int numStarted;
        public int numCompleted;
        public int numTerminated;
        public int minDurationMillis;
        public int maxDurationMillis;
        public int avgDurationMillis;
    }

    public class OrchShape
    {
        public string Text;
        public string Id;
        public SelectionArea SelectionArea;
        public string PortName;
        public string OperationName;
        public string MessageName;
        public ShapeType ShapeType;
        public int entryCount;
        public int exitCount;
        public int minDurationMillis;
        public int maxDurationMillis;
        public int avgDurationMillis;
        public string TransformName;
        public string InputMessage;
        public string OutputMessage;


        public OrchShape()
        {
            this.SelectionArea = new SelectionArea(0, 0, 0, 0);
        }

        public OrchShape(string text, string id, SelectionArea selectionArea)
        {
            this.Id = id;
            this.Text = text;
            this.SelectionArea = selectionArea;
        }

        public double GetSuccessRate()
        {
            double percentage = 0;
            int inCount = this.entryCount;
            int outCount = this.exitCount > this.entryCount ? this.entryCount : this.exitCount;

            if (inCount > 0)
            {
                percentage = (((double)outCount) / ((double)inCount)) * 100;
            }

            return percentage;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class OrchestrationCoverageInfo
    {
        public int totalCoverageCompletePercentage;
        public int totalCoverageFailedPercentage;
        public int successRatePercentage;
        public int failureRatePercentage;

        public OrchestrationCoverageInfo()
        {
        }
    }

    public class SelectionArea
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public SelectionArea()
        {
        }

        public SelectionArea(int x, int y, int w, int h)
        {
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }

        public string Coordinates
        {
            get
            {
                return string.Format("{0},{1},{2},{3}", this.X, this.Y, this.X + this.Width, this.Y + this.Height);
            }
            set { }
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(this.X, this.Y, this.Width, this.Height);
        }
    }

    /// <summary>
    /// Summary description for OdViewWithSave.
    /// </summary>
    public class OrchViewer : ProcessView
    {
        public static Bitmap GetOrchestationImage(Orchestration orchestration)
        {
            return GetOrchestationImage(orchestration, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orchestration"></param>
        /// <returns></returns>
        public static Rectangle GetOrchImageSize(Orchestration orchestration)
        {
            OrchViewer ov = new OrchViewer();
            string text1 = string.Empty;
            Rectangle maxRect = new Rectangle(0, 0, 0, 0);

            if (orchestration.ViewData != string.Empty)
            {
                int width = 1;
                int height = 1;

                Bitmap bmp = new Bitmap(width, height);
                Graphics memGraphic = Graphics.FromImage(bmp);

                XLANGView.XsymFile.ReadXsymFromString(ov.Root, orchestration.ViewData, false, ref text1);

                PageSettings ps = new PageSettings();
                Rectangle marginBounds = new Rectangle(0, 0, width, height);

                PrintPageEventArgs args = new PrintPageEventArgs(
                    memGraphic,
                    marginBounds,
                    marginBounds,
                    ps);

                maxRect = ov.DoPrintWithSize(args);

                ps = null;
                args = null;
                memGraphic.Dispose();
                bmp.Dispose();
            }

            ov.Dispose();
            return maxRect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orchestration"></param>
        /// <returns></returns>
        public static Bitmap GetOrchestationImage(Orchestration orchestration, bool drawWithCoverage)
        {
            OrchViewer ov = new OrchViewer();
            string text1 = string.Empty;

            if (orchestration.ViewData != string.Empty)
            {
                XLANGView.XsymFile.ReadXsymFromString(ov.Root, orchestration.ViewData, false, ref text1);

                PageSettings ps = new PageSettings();

                Rectangle maxRect = GetOrchImageSize(orchestration);

                Bitmap realBmp = new Bitmap(maxRect.Width, maxRect.Height);
                Graphics realGraphic = Graphics.FromImage(realBmp);

                PrintPageEventArgs realArgs = new PrintPageEventArgs(
                    realGraphic,
                    maxRect,
                    maxRect,
                    ps);

                // Set background colour
                realGraphic.FillRectangle(new SolidBrush(Color.White), maxRect);

                // Draw the orch image
                ov.DoPrintWithSize(realArgs);

                bool drawHotspots = false;

                try
                {
                    // Decide whether we need hotspots or not
                    object drawHotspotsObj = new AppSettingsReader().GetValue("ShowHotSpots", typeof(int));

                    if (drawHotspotsObj != null)
                    {
                        int tmp = Convert.ToInt32(drawHotspotsObj);
                        if (tmp == 1) drawHotspots = true;
                    }
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }

                GetSelectionAreas(realGraphic, ov.Root, orchestration, drawHotspots);

                if (drawWithCoverage)
                {
                    DrawCoverageAreas(realGraphic, ov.Root, orchestration, null);
                }

                ps = null;
                realArgs = null;
                ov.Dispose();
                realGraphic.Dispose();
                return realBmp;
            }
            else
            {
                string errorText = "Unable to load image for orchestration";
                Font f = new Font("Arial", 9, FontStyle.Bold);
                int fontHeight = (int)f.GetHeight() + 5;
                int errorWidth = MeasureStringWidth(errorText, f);
                int nameWidth = MeasureStringWidth(orchestration.Name, f);
                int w = Math.Max(errorWidth, nameWidth) + 50;
                int h = 100;

                SolidBrush titleBrush = new SolidBrush(Color.Black);
                Bitmap bmp = new Bitmap(w, h);
                Graphics g = Graphics.FromImage(bmp);

                g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);
                g.DrawString(errorText, f, titleBrush, (bmp.Width / 2) - (errorWidth / 2), 10);
                g.DrawString(orchestration.Name, f, titleBrush, (bmp.Width / 2) - (nameWidth / 2), 10 + fontHeight);
                return bmp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="shape"></param>
        /// <param name="orchestration"></param>
        private static void DrawCoverageAreas(Graphics g, BaseShape shape, Orchestration orchestration, BaseShape parent)
        {
            foreach (BaseShape bc in shape.Shapes)
            {
                try
                {
                    if (bc is CallShape ||
                        bc is CompensateShape ||
                        bc is ConstructShape ||
                        bc is DelayShape ||
                        //bc is ListenShape ||
                        //bc is MessageAssignmentShape ||
                        bc is ReceiveShape ||
                        bc is RulesShape ||
                        bc is SuspendShape ||
                        bc is TerminateShape ||
                        bc is ThrowShape ||
                        bc is TransformShape ||
                        bc is VariableAssignmentShape ||
                        bc is SendShape)
                    {
                        OrchShape os = CreateOrchShape(bc);

                        if (os != null)
                        {
                            if (orchestration.CoverageShapes.ContainsKey(os.Id))
                            {
                                OrchShape osTmp = orchestration.CoverageShapes[os.Id] as OrchShape;
                                os.exitCount = osTmp.exitCount;
                                os.entryCount = osTmp.entryCount;
                            }

                            orchestration.ShapeMap.Add(os);

                            DrawCoverageRect(g, os);
                        }
                    }

                    DrawCoverageAreas(g, bc, orchestration, shape);
                }
                catch (Exception ex)
                {
                    TraceManager.SmartTrace.TraceError(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="shape"></param>
private static void GetSelectionAreas(Graphics g, BaseShape shape, Orchestration orchestration, bool drawHotspots)
        {
            try
            {
                foreach (BaseShape bc in shape.Shapes)
                {
                    //if (bc is ReceiveShape ||
                    //    bc is SendShape ||
                    //    bc is MessageAssignmentShape ||
                    //    bc is VariableAssignmentShape ||
                    //    bc is TransformShape)//NJB adding transform
                    if (!bc.HasChildShapes) // Only shapes with no children should be clickable. - CD 20140408
                    {
                        OrchShape os = CreateOrchShape(bc);

                        if (os != null)
                        {
                            orchestration.ShapeMap.Add(os);

                            if (drawHotspots)
                            {
                                DrawDebugRect(g, os.SelectionArea.GetRectangle());
                            }
                        }
                    }
                    // Exit gracefully rather than throwing an exception - CD 20140408
                    else
                    {
                        GetSelectionAreas(g, bc, orchestration, drawHotspots);
                    }
                }
            }
            catch (Exception ex)
            {
                 string message = ex.Message;
                TraceManager.SmartTrace.TraceError(ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bc"></param>
        /// <returns></returns>
        private static OrchShape CreateOrchShape(BaseShape bc)
        {
            OrchShape os = null;

            XLANGView.TrkMetadata data = bc.Relationship as XLANGView.TrkMetadata;

            if (data != null)
            {
                os = new OrchShape();
                os.Text = bc.Text;
                os.Id = data.ShapeID;

                //				Rectangle selRect = new Rectangle(
                //					bc.DesignSurfaceClientLocation.X-5,
                //					bc.DesignSurfaceClientLocation.Y-7,
                //					bc.NativeSize.Width,
                //                    bc.NativeSize.Height);

                Rectangle selRect = new Rectangle(
                    bc.DesignSurfaceClientLocation.X - 5,
                    bc.DesignSurfaceClientLocation.Y - 7,
                    bc.Width,
                    bc.Height);

                os.SelectionArea = new SelectionArea(
                    selRect.X,
                    selRect.Y,
                    selRect.Width,
                    selRect.Height);

                switch (bc.GetType().ToString())
                {
                    case "Microsoft.VisualStudio.EFT.SendShape": os.ShapeType = ShapeType.SendShape; break;
                    case "Microsoft.VisualStudio.EFT.ReceiveShape": os.ShapeType = ShapeType.ReceiveShape; break;
                    case "Microsoft.VisualStudio.EFT.VariableAssignmentShape": os.ShapeType = ShapeType.VariableAssignment; break;
                    case "Microsoft.VisualStudio.EFT.MessageAssignmentShape": os.ShapeType = ShapeType.MessageAssignment; break;
                }
            }

            return os;
        }

        private static int MeasureStringWidth(string text, System.Drawing.Font font)
        {
            Bitmap bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            int i = (int)g.MeasureString(text, font).Width;
            g.Dispose();
            bmp.Dispose();
            return i;
        }

        public static bool SaveOrchestrationToJpg(Orchestration orchestration, string fileName)
        {
            return SaveOrchestrationToJpg(orchestration, fileName, false);
        }

        public static bool SaveOrchestrationToJpg(Orchestration orchestration, string fileName, bool includeCoverage)
        {
            bool success = true;
            Bitmap bmp = null;

            bmp = GetOrchestationImage(orchestration, includeCoverage);

            if (bmp != null)
            {
                bmp.Save(fileName);
                bmp.Dispose();
            }
            return success;
        }


        private Rectangle DoPrintWithSize(PrintPageEventArgs e)
        {
            int maxX = 0;
            int maxY = 0;

            Rectangle rectangle1;
            GraphicsContainer container1;
            GraphicsContainer container2;
            Point point1;
            rectangle1 = new Rectangle(new Point(0, 0), base.Size);
            PaintEventArgs args1 = new PaintEventArgs(e.Graphics, rectangle1);
            this.BackColor = Color.White;

            try
            {
                base.PrintingInProgress = true;

                container1 = e.Graphics.BeginContainer();
                point1 = base.AutoScrollPosition;
                point1 = base.AutoScrollPosition;
                e.Graphics.TranslateTransform(((float)-point1.X), ((float)-point1.Y));
                this.OnPaint(args1);

                maxX = point1.X;
                maxY = point1.Y;

                foreach (Control control1 in base.Controls)
                {
                    if (!(control1 is BaseControl))
                    {
                        continue;
                    }
                    container2 = e.Graphics.BeginContainer();
                    e.Graphics.TranslateTransform(((float)control1.Left) / 2, ((float)control1.Top) / 2);

                    maxX = Math.Max(control1.Left + control1.Width, maxX);
                    maxY = Math.Max(control1.Top + control1.Height, maxY);

                    ((BaseControl)control1).DoPrint(e);
                    e.Graphics.EndContainer(container2);
                }

                e.Graphics.EndContainer(container1);
            }
            finally
            {
                base.PrintingInProgress = false;
            }

            return new Rectangle(0, 0, maxX, maxY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private static void DrawDebugRect(Graphics g, Rectangle r)
        {
            Pen p = new Pen(Color.Red);
            g.DrawRectangle(p, r);
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private static void DrawCoverageRect(Graphics g, OrchShape os)
        {
            if (os != null && os.SelectionArea != null)
            {
                SelectionArea sa = os.SelectionArea;

                Font f = new Font("Arial", 8, FontStyle.Regular);
                int fontHeight = (int)f.GetHeight();

                double shapeSuccessRate = os.GetSuccessRate();

                string toolTipText = "Success Rate: " + (shapeSuccessRate == 0 ? "0" : shapeSuccessRate.ToString("###")) + "%";

                int toolTipWidth = MeasureStringWidth(toolTipText, f);
                int toolTipHeight = f.Height + 6;

                int w = sa.Width + 6;
                int h = toolTipHeight;
                int x = sa.X - 2;
                int y = sa.Y - 2 - (h - ((h / 3)));

                float fttt = (float)toolTipWidth;
                float fw = (float)w;
                float fh = (float)h;
                float fx = (float)x;
                float fy = (float)y;

                Rectangle toolTipRect = new Rectangle(x, y, w, h);

                GraphicsPath gp = GetGpForShape(os);

                if (os.entryCount > 0)
                {
                    if (shapeSuccessRate != 100)
                    {
                        // Blue
                        g.FillRegion(new SolidBrush(Color.FromArgb(20, 0, 0, 255)), new Region(gp));
                    }
                    else
                    {
                        // Green
                        g.FillRegion(new SolidBrush(Color.FromArgb(20, 0, 255, 0)), new Region(gp));
                    }
                }
                else
                {
                    // Red
                    g.FillRegion(new SolidBrush(Color.FromArgb(20, 255, 0, 0)), new Region(gp));
                }

                gp.Dispose();

                g.FillRegion(new SolidBrush(Color.LemonChiffon), new Region(toolTipRect));
                g.DrawRectangle(new Pen(Color.Black, 0.5f), toolTipRect);
                g.DrawString(toolTipText, f, new SolidBrush(Color.Black), ((sa.X + (sa.Width / 2)) - (fttt / 2)) + 3, y + 3);
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        /// <returns></returns>
        private static GraphicsPath GetGpForShape(OrchShape os)
        {
            float radius = 3.2f;

            int x = os.SelectionArea.X;
            int y = os.SelectionArea.Y;
            int width = os.SelectionArea.Width;
            int height = os.SelectionArea.Height;

            GraphicsPath gp1 = new GraphicsPath();
            gp1.AddLine(x + radius, y, x + width - (radius * 2), y);
            gp1.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90);
            gp1.AddLine(x + width, y + radius, x + width, y + height - (radius * 2));
            gp1.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
            gp1.AddLine(x + width - (radius * 2), y + height, x + radius, y + height);
            gp1.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
            gp1.AddLine(x, y + height - (radius * 2), x, y + radius);
            gp1.AddArc(x, y, radius * 2, radius * 2, 180, 90);
            gp1.CloseFigure();

            return gp1;
        }
    }
}
