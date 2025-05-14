using SQLite;

namespace assignment_2425.Models
{
    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public bool Vibration { get; set; } = false;
        public bool TextToSpeech { get; set; } = false;
        public bool DarkMode { get; set; } = false;
        public bool HapticFeedback { get; set; } = false;
    }
}
