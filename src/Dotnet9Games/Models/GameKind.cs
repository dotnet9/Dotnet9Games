using System.ComponentModel;

namespace Dotnet9Games.Models;

/// <summary>
///     游戏种类
/// </summary>
public enum GameKind
{
    [Description("经典")] Classics,
    [Description("小学二年级")] Equation1
}