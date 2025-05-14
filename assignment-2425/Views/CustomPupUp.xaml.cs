using CommunityToolkit.Maui.Views;
using System.Threading.Tasks;

namespace assignment_2425.Views;

public partial class CustomPopUp : Popup
{
	public CustomPopUp(string text)
	{
		InitializeComponent();
        DisplayText.Text = text;
    }

    //When background pressed close the popup
    private void OnBackgroundTapped(object sender, TappedEventArgs e)
    {
        ClosePopup();
    }

    // Close the popup after animation completes
    private void ClosePopup()
    {

        Close();
    }
    //When the x pressed close the popup
    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        ClosePopup();
    }
}