using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_2425.Helpers
{
    public class AccessabilityHelper
    {
        public async void TextToSpeach(string text, bool isEnabled)
        {
            if (isEnabled)
            {
                await TextToSpeech.Default.SpeakAsync(text);
            }
            else
            {
                SemanticScreenReader.Default.Announce(text);
            }
        }

        public void VibrationHelper(int time, bool isEnabled)
        {
            if (isEnabled)
            {
                Vibration.Default.Vibrate(time);
            }
        }

        public void HapticHelper(bool isEnabled)
        {
            if (isEnabled)
            {
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            }
        }

    }
}
