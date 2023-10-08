using System.Windows.Controls;

namespace Dotnet9Games.Views
{
    /// <summary>
    /// 经典游戏
    /// </summary>
    public partial class ClassicsBallGameHeader : UserControl, IBallGame
    {

        public ClassicsBallGameHeader(Canvas canvas)
        {
            InitializeComponent();
            _canvas = canvas;
        }

    }
}