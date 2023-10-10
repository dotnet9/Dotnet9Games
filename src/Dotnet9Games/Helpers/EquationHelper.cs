using System;

namespace Dotnet9Games.Helpers;

internal static class EquationHelper
{
    /// <summary>
    ///     获取小学二年级的随机算式及结果
    /// </summary>
    /// <returns></returns>
    public static EquationResult GetExpressAndResult()
    {
        var minNumber = 5;
        var maxNumber = 50;

        int GetRandomNumber()
        {
            return RandomHelper.ShareRandom().Next(minNumber, maxNumber);
        }

        var num1 = 0;
        var num2 = 0;
        var num3 = 0;
        var operation1 = 0;
        var operation2 = 0;
        var result = 0;

        // 检查结果是否在0到100之间，如果不是则重新生成算式
        // 默认会进入一次
        do
        {
            num1 = GetRandomNumber(); // 生成第一个数，范围为1到100
            num2 = GetRandomNumber(); // 生成第二个数，范围为1到100
            num3 = GetRandomNumber(); // 生成第三个数，范围为1到100


            // 随机选择加法或减法操作
            operation1 = RandomHelper.ShareRandom().Next(0, 2); // 0代表加法，1代表减法
            operation2 = RandomHelper.ShareRandom().Next(0, 2); // 0代表加法，1代表减法

            if (operation1 == 0)
            {
                result = num1 + num2;
            }
            else
            {
                // 确保差为正数，因为小学二年级还未学到负数
                var minValue = Math.Min(num1, num2);
                var maxValue = Math.Max(num1, num2);
                num1 = maxValue;
                num2 = minValue;
                result = num1 - num2;
            }

            if (operation2 == 0)
            {
                result += num3;
            }
            else
            {
                // 确保差为正数，因为小学二年级还未学到负数
                if (result < num3)
                {
                    operation2 = 0;
                    result += num3;
                }
                else
                {
                    result -= num3;
                }
            }
        } while (result > 100);

        // 输出算式和结果
        var expression = $"{num1}{(operation1 == 0 ? "+" : "-")}{num2}{(operation2 == 0 ? "+" : "-")}{num3}";
        return new EquationResult(expression, (uint)result);
    }
}

internal class EquationResult
{
    public string Expression { get; set; }
    public uint Result { get; set; }
    internal EquationResult(string expression, uint result)
    {
        Expression = expression;
        Result = result;
    }
}