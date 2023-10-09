using System.Windows.Controls;
using System.Windows.Media;
using Dotnet9Games.Helpers;

namespace Dotnet9Games.Views;

/// <summary>
///     气球
/// </summary>
public partial class Ball : UserControl, IBall
{
    private readonly Canvas _parent;

    public Ball(Canvas parent, int score = -1)
    {
        _parent = parent;
        InitializeComponent();
        Owner = this;
        CreateBall(score);
    }

    public UserControl Owner { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double SpeedX { get; set; }
    public double SpeedY { get; set; }

    /// <summary>
    ///     当前气球表示的分数
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    ///     创建气球
    /// </summary>
    private void CreateBall(int score = -1)
    {
        X = RandomHelper.ShareRandom().Next((int)_parent.ActualWidth - 50);
        Y = RandomHelper.ShareRandom().Next((int)_parent.ActualHeight - 50);
        SpeedX = RandomHelper.ShareRandom().NextDouble() * 2 - 1;
        SpeedY = RandomHelper.ShareRandom().NextDouble() * 2 - 1;

        var randomSize = RandomHelper.ShareRandom().Next(BallConst.MinNumber, BallConst.MaxNumber);
        Width = Height = randomSize;
        if (score <= 0)
            Score = BallConst.MaxScore - randomSize / 10;
        else
            Score = score;

        TextBlockBallNumber.Text = $"{Score}";
        EllipseBall.ToolTip = $"{Score}号球";
        GradientStopDiff.Color =
            Color.FromArgb(255, (byte)RandomHelper.ShareRandom().Next(256),
                (byte)RandomHelper.ShareRandom().Next(256), (byte)RandomHelper.ShareRandom().Next(256));
    }
}