using System;
using Dotnet9Games.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Dotnet9Games.Views
{
    internal interface IBallGame
    {
        /// <summary>
        /// 游戏类型
        /// </summary>
        /// <returns></returns>
        GameKind GameKind();

        /// <summary>
        /// 游戏状态
        /// </summary>
        /// <returns></returns>
        GameStatus GameStatus();

        /// <summary>
        /// 重新初始游戏等级
        /// </summary>
        void InitGameLevel();

        /// <summary>
        /// 升级游戏
        /// </summary>
        void UpgradeGameLevel();

        /// <summary>
        /// 游戏级别
        /// </summary>
        /// <returns></returns>
        int Level();

        /// <summary>
        /// 生成气球
        /// </summary>
        /// <returns></returns>
        void CreateBalls();

        /// <summary>
        /// 获取所有气球
        /// </summary>
        /// <returns></returns>
        List<IBall> GetBalls();

        /// <summary>
        /// 获取当前得分
        /// </summary>
        /// <returns></returns>
        int CountScores();

        /// <summary>
        /// 统计气球情况
        /// </summary>
        /// <returns></returns>
        string CountBallCount();

        /// <summary>
        /// 清空
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// 当前游戏状态
    /// </summary>
    public enum GameStatus
    {
        [Description("失败")] Fail,
        [Description("成功")] Success,
        [Description("继续")] Continue
    }
}