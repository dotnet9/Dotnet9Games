using System;
using System.IO;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dotnet9Games.Helpers
{
    internal class BallHelper
    {
        private static SpeechSynthesizer _speechSynthesizer;

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
        /// 播放气球爆破的声音
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
        /// 播放文字语音
        /// </summary>
        /// <param name="word"></param>
        internal static void PlayWordSound(string word)
        {
            Task.Run(() => { SpeechSynthesizer.Speak(word); });
        }
    }
}