using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace IDMA
{
    public partial class Form2 : Form
    {
        class Region
        {
            public string Text;
            public string File;
            public string Title;
            public string Info;
            public float X;
            public float Y;
            public Rectangle Rect;
        }

        List<Region> regions = new List<Region>();
        Font labelFont = new Font("Segoe UI", 13f, FontStyle.Bold);
        Region hover;

        public Form2()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Text = "Surface air temperature anomaly - choose a region";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(640, 420);
            DoubleBuffered = true;
            BackgroundImageLayout = ImageLayout.Zoom;
            BackColor = Color.FromArgb(23, 8, 37);

            AddRegion("Arctic", 0.52f, 0.04f, "arctic.txt",
                "Arctic surface air temperature anomaly",
                "Arctic\r\n\r\n" +
                "The Arctic series shows a clear, statistically significant warming, similar to or slightly stronger than the global land series, although the confidence intervals overlap so a real difference cannot be claimed. " +
                "Anomalies were mostly negative in the 1980s, close to the baseline in the 1990s, and predominantly positive from the 2000s onward, with the highest decadal averages in the most recent years. " +
                "The regression t-test, the Mann-Kendall test and Sen’s slope all reject the hypothesis of no trend, and the slope estimates from the different methods agree closely. " +
                "The result holds when annual means are used, where residual autocorrelation is negligible, and when the monthly tests are corrected for autocorrelation or replaced by a block bootstrap, which only widens the confidence intervals while keeping them entirely on the positive side. " +
                "A before-versus-after comparison of mean anomalies between the early and late parts of the record also shows a large, highly significant difference. " +
                "The month-to-month variability is the largest of the series examined so far, with swings of several kelvins in both directions, so the trend explains only a modest share of the variance and is visible mainly in the longer-term average rather than in individual months. " +
                "This is typical of high-latitude regions, where cold-season anomalies fluctuate strongly with atmospheric circulation. " +
                "The warming is also not uniform: a significant rise in the 1980s and 1990s is followed by a near-flat or slightly negative stretch in the 2000s and early 2010s, and then a renewed but not individually significant increase since about 2015, so a single straight line smooths over these phases. " +
                "As with the other series, these results are based on digitizing the image, which has a coarser vertical resolution here because of the wider axis range, so narrow spikes are slightly smoothed, and the baseline period is not stated, so only change over time can be interpreted.");


            AddRegion("Amazon river", 0.33f, 0.6f, "amazon.txt",
                "Amazon river basin surface air temperature anomaly",
                "Amazon river basin\r\n\r\n" +
                "The series shows a clear, statistically significant warming, comparable to or somewhat stronger than the global land series, although the confidence intervals overlap so the difference cannot be considered established. " +
                "Anomalies were mostly negative through the 1980s, hovered near the baseline around the turn of the century, and rose steadily thereafter, with the highest values of the whole record at the very end. " +
                "The regression t-test, the Mann-Kendall test and Sen’s slope all reject the hypothesis of no trend, and the slope estimates from the different methods are very close to one another, which indicates that the result does not depend on the method or on individual outliers. " +
                "Monthly anomalies are extremely strongly autocorrelated here, because warm and cool episodes persist for many months, so the naive monthly p-values are heavily overstated. " +
                "After correcting for this, and when annual means or a block bootstrap are used, the trend remains significant, but the confidence intervals become considerably wider, showing that the rate of warming is less precisely determined than the naive figures suggest. " +
                "The linear fit explains only a moderate share of the variance, since a large part of the signal is interannual variability. " +
                "The pronounced spikes near 1998, 2015–2016 and 2023–2024 coincide with strong El Niño years and drought episodes, and the warming proceeds in steps rather than smoothly. " +
                "The rate is lower in the early 2000s and clearly accelerates after about 2015, so a single straight line understates recent warming and overstates earlier warming. " +
                "As with the other series, these results are based on digitizing the image, so narrow spikes are slightly smoothed, which has little effect on the trend, and the baseline period is not stated, so only change over time can be interpreted.");


            AddRegion("Antarctica", 0.5f, 0.965f, "antarctica.txt",
                "Antarctica surface air temperature anomaly",
                "Antarctica\r\n\r\n" +
                "The series shows no statistically significant long-term trend. " +
                "The fitted slope is very close to zero and its sign depends on the method used: ordinary least squares gives a marginally positive value, Sen’s slope is almost flat, and the annual Sen slope is marginally negative. " +
                "The t-test on the slope and the Mann-Kendall test, applied to both monthly and annual data, all fail to reject the hypothesis of no trend, and the p-values remain far above any conventional threshold after correcting for autocorrelation. " +
                "The confidence intervals for the rate of change, from regression, the autocorrelation correction and a block bootstrap, all span zero comfortably and are about as wide on the negative side as on the positive one, so even the direction of change cannot be established. " +
                "A first-versus-last-years comparison of mean anomalies likewise shows no meaningful difference. " +
                "The linear fit explains almost none of the variance, which is dominated by large month-to-month swings typical of the Antarctic climate, with sharp negative spikes (the deepest around 2006) and positive peaks (the highest around 2023). " +
                "Sub-periods show real structure that cancels out over the full record: a slight cooling from the early 1980s into the 2000s, a long near-flat stretch through the 2000s and early 2010s, and a clear warming after about 2015, with the highest values near the end of the record. " +
                "These segments are short, however, and selecting them after viewing the data inflates their apparent significance, so the sub-period trends should be treated as descriptive rather than conclusive. " +
                "Overall, the record is best described as variable but trendless over 1982–2026, with a recent upswing that is not yet sufficient to produce a significant long-term signal. " +
                "As before, these results come from digitizing the image, so narrow spikes are slightly smoothed, which has little effect on the trend, and the baseline period is not stated, so only changes over time can be interpreted.");


            AddRegion("Global", 0.095f, 0.85f, "data.txt",
                "Global surface air temperature anomaly",
                "Global\r\n\r\n" +
                "The series shows a clear, persistent warming: anomalies in the early 1980s were mostly negative, moved into positive territory by the 2000s, and in recent years remain well above the baseline. " +
                "A linear fit gives a positive slope and a substantial cumulative increase over the record, and every test applied agrees that the trend is statistically significant. " +
                "The regression t-test, the nonparametric Mann-Kendall test, Sen’s slope (which closely matches the ordinary least squares slope, indicating that outliers do not drive the result) and the confidence intervals for the rate of change all reject the hypothesis of no trend, with the intervals lying entirely on the positive side. " +
                "Because monthly anomalies are strongly autocorrelated, naive p-values on monthly data are too optimistic, so annual means, an autocorrelation correction and a block bootstrap were also used. " +
                "The conclusion held, with only wider and more realistic intervals. " +
                "The trend is not perfectly linear, however: rapid warming early in the record is followed by a near plateau from roughly the 2000s into the early 2010s, then a renewed acceleration from the mid-2010s, with the highest values at the very end. " +
                "Month-to-month scatter is large and reflects natural variability, but the long-term rise still stands out clearly against it. " +
                "These results are based on digitizing the image rather than on the original data, so narrow spikes are slightly smoothed, which has little effect on the trend. " +
                "Since the baseline period is not stated, only the change over time can be interpreted, not the absolute level.");


            Paint += Form2_Paint;
            MouseMove += Form2_MouseMove;
            MouseLeave += Form2_MouseLeave;
            MouseClick += Form2_MouseClick;
            SizeChanged += Form2_SizeChanged;
            Shown += Form2_SizeChanged;

            SetPositions();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        void AddRegion(string text, float x, float y, string file, string title, string info)
        {
            Region r = new Region();
            r.Text = text;
            r.X = x;
            r.Y = y;
            r.File = file;
            r.Title = title;
            r.Info = info;
            regions.Add(r);
        }

        Rectangle GetMapRect()
        {
            Size img = BackgroundImage.Size;
            float scale = Math.Min((float)ClientSize.Width / img.Width, (float)ClientSize.Height / img.Height);
            int w = (int)(img.Width * scale);
            int h = (int)(img.Height * scale);
            return new Rectangle((ClientSize.Width - w) / 2, (ClientSize.Height - h) / 2, w, h);
        }

        void SetPositions()
        {
            if (BackgroundImage == null || ClientSize.Width == 0 || ClientSize.Height == 0) return;

            Rectangle map = GetMapRect();

            foreach (Region r in regions)
            {
                Size size = TextRenderer.MeasureText(r.Text, labelFont);
                int w = size.Width + 24;
                int h = size.Height + 12;

                int left = map.Left + (int)(map.Width * r.X) - w / 2;
                int top = map.Top + (int)(map.Height * r.Y) - h / 2;

                left = Math.Max(0, Math.Min(left, ClientSize.Width - w));
                top = Math.Max(0, Math.Min(top, ClientSize.Height - h));

                r.Rect = new Rectangle(left, top, w, h);
            }

            Invalidate();
        }

        Region FindRegion(Point p)
        {
            foreach (Region r in regions)
            {
                if (r.Rect.Contains(p)) return r;
            }
            return null;
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (Region r in regions)
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                GraphicsPath path = new GraphicsPath();
                float size = g.DpiY * labelFont.SizeInPoints / 72f;
                path.AddString(r.Text, labelFont.FontFamily, (int)labelFont.Style, size, r.Rect, format);

                Pen pen = new Pen(Color.FromArgb(220, 0, 0, 0), 3.5f);
                pen.LineJoin = LineJoin.Round;
                g.DrawPath(pen, path);

                Color color = Color.White;
                if (r == hover) color = Color.Gold;
                SolidBrush brush = new SolidBrush(color);
                g.FillPath(brush, path);

                pen.Dispose();
                brush.Dispose();
                path.Dispose();
                format.Dispose();
            }
        }

        private void Form2_SizeChanged(object sender, EventArgs e)
        {
            SetPositions();
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            Region r = FindRegion(e.Location);
            if (r == hover) return;

            hover = r;
            if (r != null) Cursor = Cursors.Hand;
            else Cursor = Cursors.Default;
            Invalidate();
        }

        private void Form2_MouseLeave(object sender, EventArgs e)
        {
            hover = null;
            Cursor = Cursors.Default;
            Invalidate();
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            Region r = FindRegion(e.Location);
            if (r != null) ShowChart(r.File, r.Title, r.Info);
        }

        void ShowChart(string file, string title, string info)
        {
            string path = Path.Combine(Application.StartupPath, file);
            if (!File.Exists(path))
            {
                MessageBox.Show("Data file not found:\n" + path, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Form3 frm3 = new Form3(file, title, info))
            {
                frm3.ShowDialog(this);
            }
        }
    }
}