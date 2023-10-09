using System.Windows.Controls;

namespace Dotnet9Games.Views;

public interface IBall
{
    /// <summary>
    ///     气球用户控件
    /// </summary>
    public UserControl Owner { get; set; }

    /// <summary>
    ///     水平位置
    /// </summary>
    public double X { get; set; }

    /// <summary>
    ///     竖直位置
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    ///     水平偏移单位大小
    /// </summary>
    public double SpeedX { get; set; }

    /// <summary>
    ///     竖直偏移单位大小
    /// </summary>
    public double SpeedY { get; set; }

    /// <summary>
    ///     当前气球分数
    /// </summary>
    public int Score { get; set; }
}