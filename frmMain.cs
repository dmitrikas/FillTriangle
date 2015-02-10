using System;
using System.Drawing;
using System.Windows.Forms;

namespace Fill_Triangle
{
    public partial class frmMain : Form
    {
        private Point[] triangle = new Point[3];
        private int mouseClickCount = 0;

        public frmMain()
        {
            InitializeComponent();
        }        

        private void frmMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FillTriangle();
        }

        private void FillTriangle()
        {
            Graphics gr = this.CreateGraphics();

            gr.Clear(Color.Black);

            Point a, b, c;

            a = triangle[0];
            b = triangle[1];
            c = triangle[2];

            //gr.DrawPolygon(new Pen(Color.White), new Point[] { a, b, c});

            if (a.Y == b.Y && a.Y == c.Y) 
                return; // i dont care about degenerate triangles

            //Sort y coordinate
            if (a.Y > b.Y) swap(ref a, ref b);
            if (a.Y > c.Y) swap(ref a, ref c);
            if (b.Y > c.Y) swap(ref b, ref c);
    
            int total_height = c.Y - a.Y;
            for (int i = 0; i < total_height; i++) {
                bool second_half = (i > b.Y - a.Y) || (b.Y == a.Y);
                int segment_height = second_half ? c.Y - b.Y : b.Y - a.Y;

                float alpha =(float)i/total_height;
                float beta = (float)(i - (second_half ? b.Y - a.Y : 0)) / segment_height; // be careful: with above conditions no division by zero here
                
                Point D = sum(a, multiply(negation(c, a), alpha));
                Point M = second_half ? sum(b, multiply(negation(c, b), beta)) : sum(a, multiply(negation(b, a), beta));

                if (D.X > M.X) swap(ref D, ref M);

                for (int j = D.X; j <= M.X; j++)
                {
                    gr.FillRectangle(new SolidBrush(Color.Red), j, D.Y, 1, 1);
                }

                //gr.DrawLine(new Pen(Color.YellowGreen), new Point(D.X, i+ a.Y) , new Point(M.X, i+a.Y));

                //DEBUG
                //gr.FillRectangle(new SolidBrush(second_half ? Color.Yellow : Color.Red), D.X, D.Y, 1, 1);
                //gr.FillRectangle(new SolidBrush(second_half ? Color.Blue : Color.White), M.X, M.Y, 1, 1);

                //gr.DrawLine(new Pen(Color.AliceBlue), 70, D.Y, 80, D.Y);
            }
            
            
            //gr.FillPolygon(new SolidBrush(Color.Blue), new Point[] { a, b, c });
            //gr.DrawPolygon(new Pen(Color.White), new Point[] { a, b, c });
        }

        private Point sum(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        private Point negation(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        private Point multiply(Point a, float m)
        {
            return new Point((int)Math.Round(a.X * m, 0), (int) Math.Round(a.Y * m, 0));
        }

        private void swap(ref Point a, ref Point b)
        {
            Point buffer = a;
            a = b;
            b = buffer;
        }

        private void frmMain_MouseClick(object sender, MouseEventArgs e)
        {
            triangle[mouseClickCount].X = e.X;
            triangle[mouseClickCount].Y = e.Y;

            mouseClickCount += 1;

            switch (mouseClickCount)
            {

                case 1:
                    this.Text = string.Format("Click on form for put second point of Triangle X1:{0}, Y1:{1}", e.X, e.Y);
                    break;
                case 2:
                    this.Text = string.Format("Click on form for put third point of Triangle X1:{0}, Y1:{1}, X2:{2}, Y2:{3}", triangle[0].X, triangle[0].Y, e.X, e.Y);
                    break;
                case 3:
                    this.Text = string.Format("Click on form for put fist point of Triangle X1:{0}, Y1:{1}; X2:{2}, Y2:{3}; X3:{4}, Y3:{5}", triangle[0].X, triangle[0].Y, triangle[1].X, triangle[1].Y, e.X, e.Y);
                    break;

            }

            if (mouseClickCount == 3)
            {
                mouseClickCount = 0;

                FillTriangle();
            }
        }

        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
            int i = this.Text.IndexOf("Click");
            this.Text = string.Format("{0}, {1} ", e.X, e.Y);
        }
    }
}
