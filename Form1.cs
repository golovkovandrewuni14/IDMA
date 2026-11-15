using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IDMA
{
    public partial class Form1 : Form
    {
        class MenuButton
        {
            public string Text;
            public Action Click;
            public Rectangle Rect;
        }

        List<MenuButton> buttons = new List<MenuButton>();

        Font titleFont = new Font("Segoe UI", 44f, FontStyle.Bold);
        Font subtitleFont = new Font("Segoe UI", 14f);
        Font buttonFont = new Font("Segoe UI", 16f, FontStyle.Bold);
        Font footerFont = new Font("Segoe UI", 9f);

        Rectangle titleRect;
        Rectangle subtitleRect;
        Rectangle footerRect;
        int lineY;
        int selected = -1;

        public Form1()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Text = "IDMA - Interesting Data Monitoring and Analysis";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(480, 420);
            DoubleBuffered = true;
            ResizeRedraw = true;
            KeyPreview = true;

            buttons.Add(new MenuButton { Text = "Start", Click = OpenMap });
            buttons.Add(new MenuButton { Text = "What is IDMA", Click = ShowAbout });
            buttons.Add(new MenuButton { Text = "Exit", Click = ExitProgram });

            Paint += Form1_Paint;
            MouseMove += Form1_MouseMove;
            MouseLeave += Form1_MouseLeave;
            MouseClick += Form1_MouseClick;
            KeyDown += Form1_KeyDown;
            SizeChanged += Form1_SizeChanged;

            SetPositions();
        }

        void OpenMap()
        {
            using (Form2 frm2 = new Form2())
            {
                frm2.ShowDialog(this);
            }
        }

        void ShowAbout()
        {
            MessageBox.Show(
                "IDMA - Interesting Data Monitoring and Analysis\r\n\r\n" +
                "The program shows how surface air temperature deviates from the norm " +
                "in different regions of the planet: the Arctic, the Amazon river basin, " +
                "Antarctica and the Earth as a whole.\r\n\r\n" +
                "Click a region on the world map to open its chart. " +
                "To zoom a chart, select an area with the mouse.\r\n\r\n" +
                "Data source: NASA Giovanni (FLDAS Noah).",
                "What is IDMA", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void ExitProgram()
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        void SetPositions()
        {
            int w = ClientSize.Width;
            int h = ClientSize.Height;
            if (w == 0 || h == 0) return;

            int titleY = (int)(h * 0.17);
            int subtitleY = titleY + 62;

            titleRect = new Rectangle(0, titleY - 40, w, 80);
            subtitleRect = new Rectangle(0, subtitleY - 18, w, 36);
            lineY = subtitleY + 28;
            footerRect = new Rectangle(0, h - 28, w, 20);

            int btnWidth = 260;
            int btnHeight = 50;
            int gap = 14;
            int top = (int)(h * 0.46);

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Rect = new Rectangle(w / 2 - btnWidth / 2, top + i * (btnHeight + gap), btnWidth, btnHeight);
            }

            Invalidate();
        }

        int FindButton(Point p)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].Rect.Contains(p)) return i;
            }
            return -1;
        }

        void DrawText(Graphics g, string text, Font font, Rectangle rect, Color color, float outline)
        {
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            GraphicsPath path = new GraphicsPath();
            float size = g.DpiY * font.SizeInPoints / 72f;
            path.AddString(text, font.FontFamily, (int)font.Style, size, rect, format);

            Pen pen = new Pen(Color.FromArgb(220, 0, 0, 0), outline);
            pen.LineJoin = LineJoin.Round;
            g.DrawPath(pen, path);

            SolidBrush brush = new SolidBrush(color);
            g.FillPath(brush, path);

            pen.Dispose();
            brush.Dispose();
            path.Dispose();
            format.Dispose();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                LinearGradientBrush back = new LinearGradientBrush(ClientRectangle,
                    Color.FromArgb(46, 18, 84), Color.FromArgb(14, 5, 26), LinearGradientMode.Vertical);
                g.FillRectangle(back, ClientRectangle);
                back.Dispose();
            }

            DrawText(g, "IDMA", titleFont, titleRect, Color.White, 5f);
            DrawText(g, "Interesting Data Monitoring and Analysis", subtitleFont, subtitleRect,
                Color.FromArgb(205, 195, 235), 2f);

            int center = ClientSize.Width / 2;
            Pen linePen = new Pen(Color.FromArgb(90, 255, 255, 255), 1f);
            g.DrawLine(linePen, center - 130, lineY, center + 130, lineY);
            linePen.Dispose();

            for (int i = 0; i < buttons.Count; i++)
            {
                Color color = Color.White;
                if (i == selected) color = Color.Gold;
                DrawText(g, buttons[i].Text, buttonFont, buttons[i].Rect, color, 4f);
            }

            TextRenderer.DrawText(g, "Data: NASA Giovanni (FLDAS Noah)", footerFont, footerRect,
                Color.FromArgb(140, 130, 175),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            SetPositions();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int index = FindButton(e.Location);
            if (index == selected) return;

            selected = index;
            if (index >= 0) Cursor = Cursors.Hand;
            else Cursor = Cursors.Default;
            Invalidate();
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            selected = -1;
            Cursor = Cursors.Default;
            Invalidate();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            int index = FindButton(e.Location);
            if (index >= 0) buttons[index].Click();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                selected = (selected + 1) % buttons.Count;
                Invalidate();
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (selected <= 0) selected = buttons.Count - 1;
                else selected--;
                Invalidate();
            }
            else if (e.KeyCode == Keys.Enter && selected >= 0)
            {
                buttons[selected].Click();
            }
        }
    }
}