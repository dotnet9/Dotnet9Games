using System;
using System.Collections.Concurrent;
using System.IO;
using System.Media;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dotnet9Games.Helpers;

internal class BallHelper
{
    private static SpeechSynthesizer _speechSynthesizer;
    private static SoundPlayer _soundPlayer;
    private static ConcurrentStack<string> _needPlayWords;


    private static SpeechSynthesizer SpeechSynthesizer
    {
        get
        {
            if (_speechSynthesizer == null)
            {
                //初始化文字语音播放对象
                _speechSynthesizer = new SpeechSynthesizer();
                _speechSynthesizer.SetOutputToDefaultAudioDevice();
            }

            return _speechSynthesizer;
        }
    }

    /// <summary>
    ///     播放气球爆破的声音
    /// </summary>
    internal static void PlayBallSound()
    {
        Task.Run(async () =>
        {
            const string file = "气球爆裂声.mp3";
            if (!File.Exists(file))
            {
                using FileStream fs = new(file, FileMode.Create);
                await fs.WriteAsync(Resource.气球爆裂声, 0, Resource.气球爆裂声.Length);
            }

            MediaPlayer player = new();
            player.Open(new Uri(file, UriKind.Relative));
            player.Play();
        });
    }

    /// <summary>
    ///     播放背景音乐
    /// </summary>
    internal static void PlayBackgroundMusic()
    {
        if (_soundPlayer == null) _soundPlayer = new SoundPlayer(Resource.浪漫惬意游戏背景配乐);

        _soundPlayer.PlayLooping();
    }

    /// <summary>
    ///     释放背景音乐播放
    /// </summary>
    internal static void CloseBackgroundMusic()
    {
        _soundPlayer?.Dispose();
    }

    /// <summary>
    ///     播放文字语音
    /// </summary>
    /// <param name="word"></param>
    internal static void PlayWordSound(string word)
    {
        if (_needPlayWords == null)
        {
            _needPlayWords = new ConcurrentStack<string>();
            Task.Run(async () =>
            {
                while (true)
                {
                    while (_needPlayWords.TryPop(out var currentWord)) SpeechSynthesizer.Speak(currentWord);

                    await Task.Delay(TimeSpan.FromMilliseconds(30));
                }
            });
        }

        // 如果有等等播放的语音先清空，不然会出现语音播放滞后的情况
        while (!_needPlayWords.IsEmpty) _needPlayWords.TryPop(out _);

        _needPlayWords.Push(word);
    }
}