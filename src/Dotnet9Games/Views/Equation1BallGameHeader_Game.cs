using Dotnet9Games.Helpers;
using Dotnet9Games.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dotnet9Games.Views
{
    public partial class Equation1BallGameHeader
    {
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        private readonly Canvas _canvas;
        private int _level;
        private int _currentLevelBallCount;
        private int _ballScores;
        private List<IBall> _balls = new List<IBall>();


        public GameKind GameKind()
        {
            return Models.GameKind.Classics;
        }

        public GameStatus GameStatus()
        {
            if (_balls.Count <= 0)
            {
                return Views.GameStatus.Success;
            }

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

        private void SetGameLevel(int level)
        {
            _level = level;
            _currentLevelBallCount = _level * 2;
            _ballScores = 0;
        }


        public void CreateBalls()
        {
            for (var i = 0; i < _currentLevelBallCount; i++)
            {
                var ball = new Ball(_canvas, _random);
                ball.MouseLeftButtonDown += RemoveBall_OnMouseLeftButtonDown;
                ;
                _canvas.Children.Add(ball.Owner);
                _balls.Add(ball);
            }
        }

        private void RemoveBall_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is IBall ball))
            {
                return;
            }

            BallHelper.PlayBallSound();
            BallHelper.PlayWordSound($"{ball.Score}分");

            _ballScores += ball.Score;
            _balls.Remove(ball);
            _canvas.Children.Remove(ball.Owner);
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
        /// 获取当前得分
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
    }
}
