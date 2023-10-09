using System.ComponentModel;

namespace Dotnet9Games.Models;

/// <summary>
///     题目个数类型
/// </summary>
internal enum QuestionKind
{
    [Description("同关卡级别")] SameLevel,
    [Description("自定义")] Custom
}