using UnityEngine;
public class ayarlar : MonoBehaviour
{
    protected virtual void Start()
    {
        Screen.fullScreen = false;
    }
    public void puanla()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.muratunlu0.kitsort");
    }
    public void premium()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.app.kitsortprime");
    }
    public void app_insta()
    {
        Application.OpenURL("https://www.instagram.com/kitsortapp/");
    }
    public void developer_insta()
    {
        Application.OpenURL("https://www.instagram.com/muratunlu0/");
    }
    public void booksite_add()
    {
        string email = "muratunlu.official@gmail.com";

        string subject = MyEscapeURL("KitSort: Site Ekleme");

        string body = MyEscapeURL("Hangi kitap satış sitelerini ekleyeyim ?: ");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}