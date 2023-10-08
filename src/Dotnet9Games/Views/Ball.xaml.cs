using Dotnet9Games.Helpers;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dotnet9Games.Views
{
    /// <summary>
    /// 气球
    /// </summary>
    public partial class Ball : UserControl, IBall
    {
        private readonly Canvas _parent;
        public UserControl Owner { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }

        /// <summary>
        /// 当前气球表示的分数
        /// </summary>
        public int Score { get; set; }

        public Ball(Canvas parent)
        {
            _parent = parent;
            InitializeComponent();
            Owner = this;
            CreateBall();
        }

        /// <summary>
        /// 创建气球
        /// </summary>
        private void CreateBall()
        {
            X = BallHelper.ShareRandom().Next((int)_parent.ActualWidth - 50);
            Y = BallHelper.ShareRandom().Next((int)_parent.ActualHeight - 50);
            SpeedX = BallHelper.ShareRandom().NextDouble() * 2 - 1;
            SpeedY = BallHelper.ShareRandom().NextDouble() * 2 - 1;

            var randomSize = BallHelper.ShareRandom().Next(BallConst.MinNumber, BallConst.MaxNumber);
            this.Width = this.Height = randomSize;
            Score = BallConst.MaxScore - (randomSize / 10);
            TextBlockBallNumber.Text = $"{Score}";
            EllipseBall.ToolTip = $"{Score}号球";
            GradientStopDiff.Color =
                Color.FromArgb(255, (byte)BallHelper.ShareRandom().Next(256), (byte)BallHelper.ShareRandom().Next(256), (byte)BallHelper.ShareRandom().Next(256));
        }
    }
}