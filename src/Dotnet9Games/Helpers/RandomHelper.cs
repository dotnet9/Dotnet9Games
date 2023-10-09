using System;

namespace Dotnet9Games.Helpers;

internal class RandomHelper
{
    private static readonly Random Random = new(DateTime.Now.Millisecond);

    /// <summary>
    ///     获取共享的计时器
    /// </summary>
    /// <returns></returns>
    internal static Random ShareRandom()
    {
        return Random;
    }
}