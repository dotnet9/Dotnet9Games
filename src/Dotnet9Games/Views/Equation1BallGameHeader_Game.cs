using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Dotnet9Games.Helpers;
using Dotnet9Games.Models;

namespace Dotnet9Games.Views;

/// <summary>
///     小学二年级算式处理逻辑
/// </summary>
public partial class Equation1BallGameHeader
{
    private readonly Canvas _canvas;
    private readonly List<IBall> _balls = new();
    private int _ballScores;
    private int _currentLevelBallCount;
    private readonly Dictionary<string, ExpressionInfo> _expressionInfos = new();
    private int _level;

    public GameKind GameKind()
    {
        return Models.GameKind.Classics;
    }

    public GameStatus GameStatus()
    {
        if (_expressionInfos.Values.All(expression => expression.IsSuccess)) return Views.GameStatus.Success;

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
        void CreateBall(int score = -1)
        {
            var ball = new Ball(_canvas, score);
            ball.MouseLeftButtonDown += RemoveBall_OnMouseLeftButtonDown;
            ;
            _canvas.Children.Add(ball.Owner);
            _balls.Add(ball);
        }

        for (var i = 0; i < _currentLevelBallCount; i++) CreateBall(RandomHelper.ShareRandom().Next(5, 100));

        // 生成算式及结果气球
        var equationBallCount = _questionKind == QuestionKind.SameLevel ? _level : customQuestionCount;
        _expressionInfos.Clear();
        for (var i = 0; i < equationBallCount; i++)
        {
            var expressAndResult = EquationHelper.GetExpressAndResult();
            _expressionInfos[expressAndResult.Express] =
                new ExpressionInfo(expressAndResult.Express, expressAndResult.Result);

            CreateBall((int)expressAndResult.Result);
        }

        ChangeExpressionResult();
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
        _currentLevelBallCount = _level * 2;
        _ballScores = 0;
        RunLevel.Text = $"第{_level}关";
    }

    /// <summary>
    ///     更新算式显示
    /// </summary>
    private void ChangeExpressionResult()
    {
        var expressionCount = new StringBuilder();
        foreach (var expressionInfo in _expressionInfos)
        {
            if (expressionCount.Length > 0) expressionCount.Append(", ");

            if (expressionInfo.Value.IsSuccess)
                expressionCount.Append($"{expressionInfo.Key} = {expressionInfo.Value.Result}");
            else
                expressionCount.Append($"{expressionInfo.Key} = ?");
        }

        TextBlockQuestionInfo.Text = expressionCount.ToString();
    }

    private void RemoveBall_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!(sender is IBall ball)) return;

        // 消除点击的气球
        BallHelper.PlayBallSound();
        BallHelper.PlayWordSound($"{ball.Score}分");

        _ballScores += ball.Score;
        _balls.Remove(ball);
        _canvas.Children.Remove(ball.Owner);

        // 如果是算式结果气球被点击，则更新状态
        foreach (var expressionInfo in _expressionInfos)
            if (expressionInfo.Value.Result == ball.Score)
                expressionInfo.Value.IsSuccess = true;

        ChangeExpressionResult();
    }
}

/// <summary>
///     算式信息
/// </summary>
internal class ExpressionInfo
{
    public ExpressionInfo(string expression, uint result)
    {
        Expression = expression;
        Result = result;
        IsSuccess = false;
    }

    /// <summary>
    ///     算式
    /// </summary>
    public string Expression { get; set; }

    /// <summary>
    ///     算式结果，必须为正数
    /// </summary>
    public uint Result { get; set; }

    /// <summary>
    ///     版式气球是否被点破
    /// </summary>
    public bool IsSuccess { get; set; }
}