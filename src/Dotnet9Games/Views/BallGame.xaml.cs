using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dotnet9Games.Helpers;
using Dotnet9Games.Models;

namespace Dotnet9Games.Views;

/// <summary>
///     气球小游戏，只用于测试Lib.Harmony拦截
/// </summary>
public partial class BallGame : UserControl
{
    private const double MaxSeconds = 60;
    private readonly Dictionary<GameKind, UserControl> _ballKind;
    private readonly Stopwatch _stopwatch = new();
    private IBallGame _currentBallGame;

    public BallGame()
    {
        InitializeComponent();

        CompositionTarget.Rendering += CompositionTargetBalloon_Rendering;

        // 游戏类型管理
        _ballKind = new Dictionary<GameKind, UserControl>
        {
            { GameKind.Classics, new ClassicsBallGameHeader(CanvasPlayground) },
            { GameKind.Equation1, new Equation1BallGameHeader(CanvasPlayground) }
        };
        _currentBallGame = (IBallGame)_ballKind[GameKind.Classics];
        ShowBallGameInfo(GameKind.Classics);
    }


    /// <summary>
    ///     开始游戏，比如生成飘散的彩色气球或播放爆炸动画
    /// </summary>
    public void StartGame()
    {
        _currentBallGame.Clear();
        _currentBallGame.InitGameLevel();
        ShowGameOver(false);
        GenerateBalloons();
    }

    /// <summary>
    ///     升级游戏
    /// </summary>
    private void UpgradeGame()
    {
        Dispatcher.Invoke(() =>
        {
            _currentBallGame.UpgradeGameLevel();
            BallHelper.PlayWordSound($"恭喜进入第{_currentBallGame.Level()}关");
            GenerateBalloons();
        });
    }

    /// <summary>
    ///     修改游戏类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ChangeGameKind(object sender, RoutedEventArgs args)
    {
        var radioButton = (RadioButton)sender;
        var gameKind = (GameKind)radioButton.Tag;
        if (radioButton.IsChecked == true)
        {
            _currentBallGame = (IBallGame)_ballKind[gameKind];
            ShowBallGameInfo(gameKind);
            ShowGameOver();
        }
    }

    /// <summary>
    ///     切换游戏类型提示信息
    /// </summary>
    /// <param name="gameKind"></param>
    private void ShowBallGameInfo(GameKind gameKind)
    {
        GridBallGameHeader.Children.Clear();
        GridBallGameHeader.Children.Add(_ballKind[gameKind]);
    }


    /// <summary>
    ///     生成彩色气球
    /// </summary>
    private void GenerateBalloons()
    {
        _currentBallGame.CreateBalls();
        _stopwatch.Restart();
    }


    private void CompositionTargetBalloon_Rendering(object sender, EventArgs e)
    {
        // 更新气球位置
        foreach (var balloon in _currentBallGame.GetBalls())
        {
            // 更新气球的位置
            balloon.X += balloon.SpeedX;
            balloon.Y += balloon.SpeedY;

            // 边界检测
            if (balloon.X < 0 || balloon.X > CanvasPlayground.ActualWidth - balloon.Owner.Width) balloon.SpeedX *= -1;

            if (balloon.Y < 0 || balloon.Y > CanvasPlayground.ActualHeight - balloon.Owner.Height) balloon.SpeedY *= -1;

            // 更新气球的位置
            Canvas.SetLeft(balloon.Owner, balloon.X);
            Canvas.SetTop(balloon.Owner, balloon.Y);
        }

        // 更新倒计时
        UpdateCountTime();
    }

    /// <summary>
    ///     开启计时
    /// </summary>
    private void UpdateCountTime()
    {
        if (!_stopwatch.IsRunning) return;

        var seconds = MaxSeconds - _stopwatch.Elapsed.TotalSeconds;
        if (seconds >= 0)
        {
            CountGame(seconds);
            var gameStatus = _currentBallGame.GameStatus();
            if (gameStatus != GameStatus.Success) return;

            UpgradeGame();
            return;
        }

        BallHelper.PlayWordSound("游戏结束");
        ShowGameOver();
    }

    /// <summary>
    ///     统计游戏信息
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private void CountGame(double seconds)
    {
        Dispatcher.Invoke(() =>
        {
            RunTimeCount.Text = $"{seconds:f2}秒";
            RunScores.Text = _currentBallGame.CountScores().ToString();
            RunBallCount.Text = _currentBallGame.CountBallCount();
        });
    }

    /// <summary>
    ///     显示游戏结束标识
    /// </summary>
    /// <param name="show"></param>
    private void ShowGameOver(bool show = true)
    {
        _stopwatch.Stop();
        Dispatcher.Invoke(() => { GridGameOver.Visibility = show ? Visibility.Visible : Visibility.Collapsed; });
    }

    /// <summary>
    ///     重写MeasureOverride方法，引出Size参数为负数异常
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

    /// <summary>
    ///     游戏加载时播放背景音乐
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BallGame_OnLoaded(object sender, RoutedEventArgs e)
    {
        BallHelper.PlayBackgroundMusic();
    }

    /// <summary>
    ///     游戏未加载时停止播放音乐
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BallGame_OnUnloaded(object sender, RoutedEventArgs e)
    {
        BallHelper.CloseBackgroundMusic();
    }
}