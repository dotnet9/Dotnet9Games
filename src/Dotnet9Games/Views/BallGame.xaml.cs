using Dotnet9Games.Helpers;
using Dotnet9Games.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dotnet9Games.Views
{
    /// <summary>
    ///     气球小游戏，只用于测试Lib.Harmony拦截
    /// </summary>
    public partial class BallGame : UserControl
    {
        private const int MaxSeconds = 60;
        private readonly Dictionary<GameKind, IBallGame> _ballKind;
        private IBallGame _currentBallGame;

        public BallGame()
        {
            InitializeComponent();

            CompositionTarget.Rendering += CompositionTargetBalloon_Rendering;

            // 游戏类型管理
            _ballKind = new Dictionary<GameKind, IBallGame>()
            {
                { GameKind.Classics, new ClassicsBallGame(CanvasPlayground) },
                { GameKind.Equation1, new Equation1BallGame(CanvasPlayground) }
            };
            _currentBallGame = _ballKind[GameKind.Classics];

            // 游戏类型单选框，可进行游戏切换
            var classicsRadioButton = new RadioButton()
                { Content = GameKind.Classics.Description(), Tag = GameKind.Classics, IsChecked = true };
            classicsRadioButton.Click += ChangeGameKind;
            var equation1RadioButton = new RadioButton()
                { Content = GameKind.Equation1.Description(), Tag = GameKind.Equation1 };
            equation1RadioButton.Click += ChangeGameKind;
            StackPanelGameKind.Children.Add(classicsRadioButton);
            StackPanelGameKind.Children.Add(equation1RadioButton);
        }


        /// <summary>
        ///     开始游戏，比如生成飘散的彩色气球或播放爆炸动画
        /// </summary>
        public void StartGame()
        {
            _currentBallGame.Clear();
            _currentBallGame.InitGameLevel();
            ShowGameOver(show: false);
            GenerateBalloons();
        }

        /// <summary>
        /// 升级游戏
        /// </summary>
        private void UpgradeGame()
        {
            this.Dispatcher.Invoke(() =>
            {
                _currentBallGame.UpgradeGameLevel();
                BallHelper.PlayWordSound($"恭喜进入第{_currentBallGame.Level()}关");
                GenerateBalloons();
            });
        }

        /// <summary>
        /// 修改游戏类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ChangeGameKind(object sender, RoutedEventArgs args)
        {
            var radioButton = (RadioButton)sender;
            var gameKind = (GameKind)radioButton.Tag;
            if (radioButton.IsChecked == true)
            {
                _currentBallGame = _ballKind[gameKind];
            }
        }


        /// <summary>
        ///     生成彩色气球
        /// </summary>
        private void GenerateBalloons()
        {
            _currentBallGame.CreateBalls();
            StartCountTime();
        }


        private void CompositionTargetBalloon_Rendering(object sender, EventArgs e)
        {
            foreach (var balloon in _currentBallGame.GetBalls())
            {
                // 更新气球的位置
                balloon.X += balloon.SpeedX;
                balloon.Y += balloon.SpeedY;

                // 边界检测
                if (balloon.X < 0 || balloon.X > ActualWidth - balloon.Owner.Width)
                {
                    balloon.SpeedX *= -1;
                }

                if (balloon.Y < 0 || balloon.Y > ActualHeight - balloon.Owner.Height)
                {
                    balloon.SpeedY *= -1;
                }

                // 更新气球的位置
                Canvas.SetLeft(balloon.Owner, balloon.X);
                Canvas.SetTop(balloon.Owner, balloon.Y);
            }
        }

        /// <summary>
        /// 开启计时
        /// </summary>
        private void StartCountTime()
        {
            Task.Run(async () =>
            {
                var seconds = MaxSeconds;
                while (seconds >= 0)
                {
                    CountGame(seconds);
                    var gameStatus = _currentBallGame.GameStatus();
                    if (gameStatus == GameStatus.Success)
                    {
                        UpgradeGame();
                        return;
                    }

                    seconds--;
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                BallHelper.PlayWordSound($"游戏结束");
                ShowGameOver();
            });
        }

        /// <summary>
        /// 统计游戏信息
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private void CountGame(int seconds)
        {
            this.Dispatcher.Invoke(() =>
            {
                RunTimeCount.Text = TimeHelper.FormatSeconds(seconds);
                RunScores.Text = _currentBallGame.CountScores().ToString();
                RunBallCount.Text = _currentBallGame.CountBallCount();
                TextBlockGameTitle.Text =
                    $"第{_currentBallGame.Level()}关-点球大战-{_currentBallGame.GameKind().Description()}";
            });
        }

        private void ShowGameOver(bool show = true)
        {
             this.Dispatcher.Invoke(() =>
            {
                TextBlockGameOver.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            });
        }

        /// <summary>
        /// 重写MeasureOverride方法，引出Size参数为负数异常
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            // 计算最后一个元素宽度，不需要关注为什么这样写，只是为了引出Size异常使得

            //var lastChild = _balloons.LastOrDefault();
            //if (lastChild != null)
            //{
            //    var remainWidth = ActualWidth;
            //    foreach (var balloon in _balloons)
            //    {
            //        remainWidth -= balloon.Ball.Width;
            //    }

            //    lastChild.Ball.Measure(new Size(remainWidth, lastChild.Ball.Height));
            //}

            return base.MeasureOverride(constraint);
        }
    }
}