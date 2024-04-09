using System.Windows.Input;

namespace LearningWPF.Infrastructure
{
    public class KeyEventArgsInfo
    {
        public Key PressedKey { get; }
        public string Source { get; }

        public KeyEventArgsInfo(Key pressedKey, string source)
        {
            PressedKey = pressedKey;
            Source = source;
        }
    }
}
