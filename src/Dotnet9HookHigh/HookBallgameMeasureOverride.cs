using Dotnet9Games.Views;
using HarmonyLib;
using System.Collections;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Dotnet9HookHigh.Helpers;

namespace Dotnet9HookHigh
{
    /// <summary>
    /// 拦截BallGame的MeasureOverride方法
    /// </summary>
    public class HookBallgameMeasureOverride
    {
        /// <summary>
        /// 拦截游戏的MeasureOverride方法
        /// </summary>
        public static void StartHook()
        {
            //var harmony =  HarmonyInstance.Create("https://dotnet9.com/HookBallgameMeasureOverride");
            // 上面是低版本Harmony实例获取代码，下面是高版本
            var harmony = new Harmony("https://dotnet9.com/HookBallgameMeasureOverride");
            var hookClassType = typeof(BallGame);
            var hookMethod =
                hookClassType.GetMethod("MeasureOverride", BindingFlags.NonPublic | BindingFlags.Instance);
            var replaceMethod = typeof(HookBallgameMeasureOverride).GetMethod(nameof(HookMeasureOverride));
            var replaceHarmonyMethod = new HarmonyMethod(replaceMethod);
            harmony.Patch(hookMethod, replaceHarmonyMethod);
        }

        /// <summary>
        /// MeasureOverride替换方法
        /// </summary>
        /// <param name="__instance">BallGame实例</param>
        /// <returns></returns>
        public static bool HookMeasureOverride(ref object __instance)
        {
            #region 原方法代码逻辑

            //var currentBalls = _currentBallGame.GetBalls();

            //var lastChild = currentBalls.LastOrDefault();
            //if (lastChild != null)
            //{
            //    var remainWidth = CanvasPlayground.ActualWidth;
            //    foreach (var balloon in currentBalls)
            //    {
            //        remainWidth -= balloon.Owner.Width;
            //    }

            //    lastChild.Owner.Measure(new Size(remainWidth, lastChild.Owner.Height));
            //}

            //return base.MeasureOverride(constraint);

            #endregion

            #region 拦截替换代码

            var instance = __instance;
            var currentBallGame = instance.Field("_currentBallGame", BindingFlags.Instance | BindingFlags.NonPublic);
            var currentBalls =
                currentBallGame.ExecuteWithReturn<IEnumerable>("GetBalls", BindingFlags.Instance | BindingFlags.Public);

            var lastChild = currentBalls?.Cast<object>().LastOrDefault();
            if (lastChild == null)
            {
                return false;
            }

            var canvasPlayground = instance.Field("CanvasPlayground",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var remainWidth = ((Canvas)canvasPlayground).ActualWidth;
            foreach (object balloon in currentBalls)
            {
                remainWidth -= GetBalloonSize(balloon).Width;
            }

            // 注意：关键代码在这，如果剩余宽度大于0才重新计算最后一个子项大小
            // 这段代码可能没什么意义，可按实际开发修改
            if (remainWidth > 0)
            {
                var lashShape = GetBalloonBall(lastChild);
                lashShape.Measure(new Size(remainWidth, lashShape.Height));
            }

            #endregion

            return false;
        }

        private static UserControl GetBalloonBall(object balloon)
        {
            return balloon.Property<UserControl>("Owner", BindingFlags.Instance | BindingFlags.Public);
        }

        private static Size GetBalloonSize(object balloon)
        {
            var shape = GetBalloonBall(balloon);
            return new Size(shape.Width, shape.Height);
        }
    }
}