using System.Windows.Controls;

namespace Dotnet9Games.Views
{
    /// <summary>
    /// 小学二年级算术游戏，100以内2位数加减
    /// </summary>
    public partial class Equation1BallGameHeader : UserControl, IBallGame
    {
        public Equation1BallGameHeader(Canvas canvas)
        {
            InitializeComponent();
            _canvas = canvas;
        }
    }
}