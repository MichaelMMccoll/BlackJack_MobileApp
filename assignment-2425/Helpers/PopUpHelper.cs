using assignment_2425.Views;
using CommunityToolkit.Maui.Views;

namespace assignment_2425.Helpers
{
    public class PopUpHelper
    {
        public async void ShowPopUp(string text, bool isEnabled)
        {
            var popUp = new CustomPopUp(text);
            Shell.Current.ShowPopup(popUp);

            if (isEnabled)
            {
                await TextToSpeech.Default.SpeakAsync(text);
            }
        }
    }
}
