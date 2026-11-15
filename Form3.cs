using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IDMA
{
    public partial class Form3 : Form
    {
        Color cardColor = Color.FromArgb(26, 10, 48);
        Color textColor = Color.FromArgb(205, 195, 235);
        string dataFile;
        string heading ;
        string body;

        string chartTitle;
        Chart chart;
        TextBox txtInfo;
        string infoText;
        Panel header;

        Font headingFont = new Font("Segoe UI", 18f, FontStyle.Bold);

        public Form3() : this("data.txt", "Global surface air temperature anomaly", "")
        {
        }

        public Form3(string dataFile, string chartTitle) : this(dataFile, chartTitle, "")
        {
        }

        public Form3(string dataFile, string chartTitle, string infoText)
        {
            this.dataFile = dataFile;
            this.chartTitle = chartTitle;
            this.infoText = infoText;

            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Text = chartTitle;
            Size = new Size(1250, 600);
            MinimumSize = new Size(800, 420);
            StartPosition = FormStartPosition.CenterParent;
            Padding = new Padding(14);
            BackColor = Color.FromArgb(23, 8, 37);
            DoubleBuffered = true;
            ResizeRedraw = true;

            SplitText();
            BuildLayout();
            BuildChart();
        }

        void SplitText()
        {
            string text = (infoText ?? "").Replace("\r\n", "\n");
            int pos = text.IndexOf("\n\n", StringComparison.Ordinal);

            if (pos > 0 && text.IndexOf('\n') == pos)
            {
                heading = text.Substring(0, pos);
                body = text.Substring(pos + 2);
            }
            else
            {
                body = text;
            }

            body = body.Replace("\n", "\r\n");
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (ClientSize.Width == 0 || ClientSize.Height == 0) return;

            LinearGradientBrush back = new LinearGradientBrush(ClientRectangle,
                Color.FromArgb(46, 18, 84), Color.FromArgb(14, 5, 26), LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(back, ClientRectangle);
            back.Dispose();
        }

        void BuildLayout()
        {
            Panel rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Width = 290;
            rightPanel.Padding = new Padding(16, 0, 16, 16);
            rightPanel.BackColor = cardColor;

            header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = 90;
            header.BackColor = cardColor;
            header.Visible = heading.Length > 0;
            header.Paint += Header_Paint;
            header.Resize += Header_Resize;

            txtInfo = new TextBox();
            txtInfo.Dock = DockStyle.Fill;
            txtInfo.Multiline = true;
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.BorderStyle = BorderStyle.None;
            txtInfo.TabStop = false;
            txtInfo.Font = new Font("Segoe UI", 11f);
            txtInfo.BackColor = cardColor;
            txtInfo.ForeColor = textColor;
            txtInfo.Text = body;

            rightPanel.Controls.Add(txtInfo);
            rightPanel.Controls.Add(header);
            txtInfo.BringToFront();

            chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = cardColor;

            Controls.Add(rightPanel);
            Controls.Add(chart);
            chart.BringToFront();
        }

        private void Header_Resize(object sender, EventArgs e)
        {
            header.Invalidate();
        }

        private void Header_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 8, header.Width, header.Height - 24);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            GraphicsPath path = new GraphicsPath();
            float size = g.DpiY * headingFont.SizeInPoints / 72f;
            path.AddString(heading, headingFont.FontFamily, (int)headingFont.Style, size, rect, format);

            Pen pen = new Pen(Color.FromArgb(220, 0, 0, 0), 4f);
            pen.LineJoin = LineJoin.Round;
            g.DrawPath(pen, path);

            SolidBrush brush = new SolidBrush(Color.White);
            g.FillPath(brush, path);

            int center = header.Width / 2;
            int y = header.Height - 10;
            Pen linePen = new Pen(Color.FromArgb(90, 255, 255, 255), 1f);
            g.DrawLine(linePen, center - 110, y, center + 110, y);

            linePen.Dispose();
            brush.Dispose();
            pen.Dispose();
            path.Dispose();
            format.Dispose();
        }

        void BuildChart()
        {
            string path = Path.Combine(Application.StartupPath, dataFile);

            if (!File.Exists(path))
            {
                MessageBox.Show("File not found:\n" + path, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Color gridColor = Color.FromArgb(50, 200, 190, 235);
            Font axisFont = new Font("Segoe UI", 9f);
            Font axisTitleFont = new Font("Segoe UI", 10f, FontStyle.Bold);

            ChartArea area = new ChartArea("main");
            area.BackColor = cardColor;

            area.AxisX.Title = "Year";
            area.AxisY.Title = "Anomaly, K";
            area.AxisX.TitleFont = axisTitleFont;
            area.AxisY.TitleFont = axisTitleFont;
            area.AxisX.TitleForeColor = textColor;
            area.AxisY.TitleForeColor = textColor;

            area.AxisX.LabelStyle.Format = "yyyy";
            area.AxisX.LabelStyle.Font = axisFont;
            area.AxisY.LabelStyle.Font = axisFont;
            area.AxisX.LabelStyle.ForeColor = textColor;
            area.AxisY.LabelStyle.ForeColor = textColor;

            area.AxisX.IntervalType = DateTimeIntervalType.Years;
            area.AxisX.Interval = 4;
            area.AxisX.IsMarksNextToAxis = false;

            area.AxisX.LineColor = textColor;
            area.AxisY.LineColor = textColor;
            area.AxisX.MajorTickMark.LineColor = textColor;
            area.AxisY.MajorTickMark.LineColor = textColor;
            area.AxisX.MajorGrid.LineColor = gridColor;
            area.AxisY.MajorGrid.LineColor = gridColor;

            area.CursorX.IsUserSelectionEnabled = true;
            area.CursorX.SelectionColor = Color.FromArgb(90, 255, 215, 0);
            area.AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas.Add(area);

            Title title = chart.Titles.Add(chartTitle);
            title.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            title.ForeColor = Color.White;

            Series series = new Series("Anomaly");
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.DateTime;
            series.BorderWidth = 2;
            series.Color = Color.Gold;
            chart.Series.Add(series);

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length < 2) continue;

                DateTime date;
                double value;

                if (!DateTime.TryParse(parts[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) continue;
                if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out value)) continue;

                series.Points.AddXY(date, value);
            }
        }
    }
}