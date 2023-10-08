using Dotnet9Games.Helpers;
using Dotnet9Games.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

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