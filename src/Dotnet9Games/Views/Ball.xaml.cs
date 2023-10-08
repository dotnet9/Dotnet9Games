using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dotnet9Games.Helpers;

namespace Dotnet9Games.Views
{
    /// <summary>
    /// 气球
    /// </summary>
    public partial class Ball : UserControl, IBall
    {
        private readonly Canvas _parent;
        private readonly Random _random;
        public UserControl Owner { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }

        /// <summary>
        /// 当前气球表示的分数
        /// </summary>
        public int Score { get; set; }

        public Ball(Canvas parent, Random random)
        {
            _parent = parent;
            _random = random;
            InitializeComponent();
            Owner = this;
            CreateBall();
        }

        /// <summary>
        /// 创建气球
        /// </summary>
        private void CreateBall()
        {
            X = _random.Next((int)_parent.ActualWidth - 50);
            Y = _random.Next((int)_parent.ActualHeight - 50);
            SpeedX = _random.NextDouble() * 2 - 1;
            SpeedY = _random.NextDouble() * 2 - 1;


            var random = new Random(DateTime.Now.Millisecond);

            var randomSize = random.Next(BallConst.MinNumber, BallConst.MaxNumber);
            this.Width = this.Height = randomSize;
            Score = BallConst.MaxScore - (randomSize / 10);
            TextBlockBallNumber.Text = $"{Score}";
            EllipseBall.ToolTip = $"{Score}号球";
            GradientStopDiff.Color =
                Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
        }
    }
}