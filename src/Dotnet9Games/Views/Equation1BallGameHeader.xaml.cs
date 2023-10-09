using System.Windows;
using System.Windows.Controls;
using Dotnet9Games.Models;

namespace Dotnet9Games.Views;

/// <summary>
///     小学二年级算术游戏，100以内2位数加减
/// </summary>
public partial class Equation1BallGameHeader : UserControl, IBallGame
{
    private QuestionKind _questionKind;
    private int customQuestionCount;
    private int questionCount;

    public Equation1BallGameHeader(Canvas canvas)
    {
        InitializeComponent();
        _canvas = canvas;
    }

    /// <summary>
    ///     修改算术题个数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeQuestionKind_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.Tag is QuestionKind kind &&
            radioButton.IsChecked == true)
            _questionKind = kind;
    }

    /// <summary>
    ///     修改算术题个数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeQuestionCount_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            if (int.TryParse(textBox.Text, out var count) && count is > 0 and < 10)
            {
                customQuestionCount = count;
            }
            else
            {
                customQuestionCount = _level;
                textBox.Text = _level.ToString();
            }
        }
    }
}