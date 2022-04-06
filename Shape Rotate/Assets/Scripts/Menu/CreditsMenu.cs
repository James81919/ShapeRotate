using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : Popup
{
    public void Button_Facebook()
    {
        Application.OpenURL("https://www.facebook.com/OFFICIALVentureGames/");
    }

    public void Button_Instagram()
    {
        Application.OpenURL("https://www.instagram.com/venture_games/");
    }

    public void Button_Twitter()
    {
        Application.OpenURL("https://twitter.com/games_venture?s=20");
    }

    public void Button_VentureGamesWebsite()
    {
        Application.OpenURL("http://venturegames.net/");
    }

    public void Button_SentientSoundsWebsite()
    {
        Application.OpenURL("https://www.sentientsounds.co.nz/");
    }
}
