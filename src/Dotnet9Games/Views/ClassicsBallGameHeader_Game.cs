using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Dotnet9Games.Helpers;
using Dotnet9Games.Models;

namespace Dotnet9Games.Views;

/// <summary>
///     经典游戏处理逻辑
/// </summary>
public partial class ClassicsBallGameHeader
{
    private readonly Canvas _canvas;
    private readonly List<IBall> _balls = new();
    private int _ballScores;
    private int _currentLevelBallCount;
    private int _level;

    public GameKind GameKind()
    {
        return Models.GameKind.Classics;
    }

    public GameStatus GameStatus()
    {
        if (_balls.Count <= 0) return Views.GameStatus.Success;

        return Views.GameStatus.Continue;
    }


    public void InitGameLevel()
    {
        SetGameLevel(1);
    }

    public void UpgradeGameLevel()
    {
        _level++;
        SetGameLevel(_level);
    }

    public int Level()
    {
        return _level;
    }


    public void CreateBalls()
    {
        for (var i = 0; i < _currentLevelBallCount; i++)
        {
            var ball = new Ball(_canvas);
            ball.MouseLeftButtonDown += RemoveBall_OnMouseLeftButtonDown;
            ;
            _canvas.Children.Add(ball.Owner);
            _balls.Add(ball);
        }

        TextBlockTips.Text = $"第{_level}关：点破{_currentLevelBallCount}个气球进入第{_level + 1}关";
    }

    public List<IBall> GetBalls()
    {
        return _balls;
    }

    public string CountBallCount()
    {
        return $"{_balls?.Count}/{_currentLevelBallCount}";
    }

    /// <summary>
    ///     获取当前得分
    /// </summary>
    /// <returns></returns>
    public int CountScores()
    {
        return _ballScores;
    }

    public void Clear()
    {
        _canvas.Children.Clear();
        _balls.Clear();
    }

    private void SetGameLevel(int level)
    {
        _level = level;

        var time = level;
        _currentLevelBallCount = 1;
        while (time > 0)
        {
            _currentLevelBallCount *= 2;
            time -= 1;
        }

        _ballScores = 0;
    }

    private void RemoveBall_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!(sender is IBall ball)) return;

        BallHelper.PlayBallSound();
        BallHelper.PlayWordSound($"{ball.Score}分");

        _ballScores += ball.Score;
        _balls.Remove(ball);
        _canvas.Children.Remove(ball.Owner);
    }
}