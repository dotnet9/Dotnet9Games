using Dotnet9Games.Views;
using Harmony;
using System.Reflection;
using System.Runtime.CompilerServices;
using Dotnet9Playground.Helpers;

namespace Dotnet9Playground.Hooks;

internal class HookBallGameStartGame
{
    /// <summary>
    /// 拦截游戏的升级方法UpgradeGame
    /// </summary>
    public static void StartHook()
    {
        var harmony = HarmonyInstance.Create("https://dotnet9.com/HookBallGameUpgradeGame");
        var hookClassType = typeof(BallGame);
        var hookMethod =
            hookClassType!.GetMethod("UpgradeGame", BindingFlags.NonPublic | BindingFlags.Instance);
        var replaceMethod = typeof(HookBallGameStartGame).GetMethod(nameof(HookUpgradeGame));
        var replaceHarmonyMethod = new HarmonyMethod(replaceMethod);
        harmony.Patch(hookMethod, replaceHarmonyMethod);
    }

    /// <summary>
    /// UpgradeGame替换方法
    /// </summary>
    /// <param name="__instance">BallGame实例</param>
    /// <returns></returns>
    public static bool HookUpgradeGame(ref object __instance)
    {
        #region 原方法原代码

        //Invoke(() =>
        //{
        //    var level = _currentBallGame.UpgradeGameLevel();
        //    if (level >= 3)
        //    {
        //        ShowGameOver(true, "游戏体验结束，后面关卡需要【充值】");
        //        return;
        //    }

        //    BallHelper.PlayWordSound($"恭喜进入第{_currentBallGame.Level()}关");
        //    GenerateBalloons();
        //});

        #endregion

        #region 拦截替换方法逻辑

        // ref修饰符：不允许做为参数传递给其他方法，所以用临时变量保存
        var instance = __instance;
        var instanceType = __instance.GetType();

        // 升级游戏的核心代码
        var action = () =>
        {
            // 1、进行游戏关卡升级，并获取返回的关卡级别
            var currentBallGame = instance.Field("_currentBallGame", BindingFlags.Instance | BindingFlags.NonPublic);
            var level = currentBallGame.ExecuteWithReturn<int>("UpgradeGameLevel",
                BindingFlags.Instance | BindingFlags.Public);

            // 2、关键代码：注释的代码就是关卡限制代码，不执行这段代码即达到取消限制作用
            //    if (level >= 3)
            //    {
            //        ShowGameOver(true, "游戏体验结束，后面关卡需要【充值】");
            //        return;
            //    }

            // 3、播放进阶语音
            ObjectHelper.Execute(instanceType.Assembly.GetType("Dotnet9Games.Helpers.BallHelper"), "PlayWordSound",
                BindingFlags.Static | BindingFlags.NonPublic, $"恭喜进入第{level}关");
            // 4、生成气球
            instance.ExecuteVoid("GenerateBalloons", BindingFlags.Instance | BindingFlags.NonPublic);
        };

        // 同步UI线程执行升级游戏逻辑
        __instance.ExecuteVoid("Invoke", BindingFlags.Instance | BindingFlags.NonPublic, action);

        #endregion

        return false;
    }
}