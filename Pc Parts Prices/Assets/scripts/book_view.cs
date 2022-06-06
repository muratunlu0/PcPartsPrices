using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine;

public class book_view : MonoBehaviour
{
    private int urun_gosterim_sayisi = 12;
    private int return_sayac;
    private int return_sayac_count = 14;

    WWW www_resim;
    WWW www_request;

    public string[] site_name_index;

    ArrayList sepet_title_list = new ArrayList();
    ArrayList sepet_fiyat_list = new ArrayList();
    ArrayList sepet_site_name_list = new ArrayList();
    ArrayList sepet_site_url_list = new ArrayList();

    public InputField arama_yazi;
    public GameObject content_fiyat_sonuclari;
    public GameObject fiyat_sonuclari_panel;
    public Scrollbar scrollbar_fiyat_sonuclari;

    public GameObject content_sepet;
    public GameObject sepet_panel;
    public Scrollbar scrollbar_sepet;

    public GameObject genel_obje;
    public GameObject site_title_obje;

    public Text kategori_title;
    int arama_sonuc_Sayisi = 1;

    public GameObject profilim_panel;

    public Text islem_durumu;
    private int sonuclar_site_sayisi = 0;

    private void Start()
    {
        StartCoroutine(check());
    }
    IEnumerator check()
    {
        yukleniyor_paneli.SetActive(true);
        www_request = new WWW("https://musterilimit.firebaseio.com/eri%C5%9Fim/everypc/.json");

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }

        yield return www_request;
        if (string.IsNullOrEmpty(www_request.error) && www_request.text == "\"tamam\"")
        {
            Debug.Log("Erişim verildi");
            Debug.Log(www_request.text);
            yukleniyor_paneli.SetActive(false);
        }
        else
        {
            Debug.Log("Erişim reddedildi");
        }
    }
    public void sepete_ekle_genel(string title, string fiyat, string site_ismi, string site_url, Texture2D resim)
    {
        sepet_title_list.Add(title);
        sepet_fiyat_list.Add(fiyat);
        sepet_site_name_list.Add(site_ismi);
        sepet_site_url_list.Add(site_url);
        urun_obje_create_sepet(title, fiyat, resim, site_ismi, site_url);
        bildirim_create("Ürün sepete eklendi");
    }
    private int gecici_sepet_index = -1;
    public void sepetten_cikar_genel(string title)
    {
        gecici_sepet_index = sepet_title_list.IndexOf(title);
        if (gecici_sepet_index >= -1)
        {
            gecici_sepet_index = sepet_title_list.IndexOf(title);
            sepet_title_list.RemoveAt(gecici_sepet_index);
            sepet_fiyat_list.RemoveAt(gecici_sepet_index);
            sepet_site_name_list.RemoveAt(gecici_sepet_index);
            sepet_site_url_list.RemoveAt(gecici_sepet_index);
            bildirim_create("Ürün sepetten çıkartıldı");
        }
    }
    public void sepeti_bosalt()
    {
        panel_content_destroy(content_sepet);
        sepet_panel.SetActive(false);
        sepet_title_list.Clear();
        sepet_fiyat_list.Clear();
        sepet_site_name_list.Clear();
        sepet_site_url_list.Clear();
        bildirim_create("Sepet boşaltıldı");
    }
    public void Sipariş_ver()
    {
        if (sepet_site_url_list.Count > 0)
        {
            string email = "muratunlu.offical@gmail.com";
            string subject = MyEscapeURL("EveryPC: Sipariş Ver");
            string body = MyEscapeURL("Ad Soyad girin:\"------------\"   |\n");
            body += MyEscapeURL("Telefon numarası girin:\"------------\"   |    \nÜrünler:\n");

            for (int i = 0; i < sepet_site_url_list.Count; i++)
            {
                body += sepet_site_url_list[i] + MyEscapeURL("  |   \n");
            }
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }
        else
        {
            bildirim_create("Sepet Boş");
        }
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void arama_kategories(Text kategori)
    {
        arama_yazi.text = kategori.text;
        if (arama_yazi.text != "")
        {
            arama_sonuc_Sayisi++;
            kategori_title.text = arama_yazi.text;
            scrollbar_fiyat_sonuclari.value = 1;
            islem_durumu.text = "YÜKLENİYOR..";
            fiyat_sonuclari_panel.SetActive(true);
            sonuclar_site_sayisi = 0;

            if (content_fiyat_sonuclari.transform.childCount > 0)
            {
                panel_content_destroy(content_fiyat_sonuclari);
            }
            StartCoroutine(Starter(arama_sonuc_Sayisi));
        }
    }
    public void arama_View()
    {
        if (arama_yazi.text != "")
        {
            arama_sonuc_Sayisi++;
            kategori_title.text = arama_yazi.text;
            scrollbar_fiyat_sonuclari.value = 1;
            islem_durumu.text = "YÜKLENİYOR..";
            fiyat_sonuclari_panel.SetActive(true);
            sonuclar_site_sayisi = 0;

            if (content_fiyat_sonuclari.transform.childCount > 0)
            {
                panel_content_destroy(content_fiyat_sonuclari);
            }
            StartCoroutine(Starter(arama_sonuc_Sayisi));
        }
    }
    string urun_title = "";
    string urun_fiyat = "";
    string urun_link = "";
    string poster_url = "";
    int urun_count = 0;
    int kacinci_index = 0;

    int siteden_sonuc_Var = 0;
    IEnumerator Starter(int profil_login_count_in)
    {
        for (int i = 0; i < site_name_index.Length; i++)
        {
            if (profil_login_count_in == arama_sonuc_Sayisi && arama_yazi.text != "")
            {
                urun_count = 0;
                kacinci_index = 0;
                siteden_sonuc_Var = 0;

                yield return StartCoroutine("a" + i.ToString());
            }
        }
        if (profil_login_count_in == arama_sonuc_Sayisi)
        {
            islem_durumu.text = sonuclar_site_sayisi + " SİTEDEN SONUÇ BULUNDU";
        }
    }
    IEnumerator a0()
    {
        www_request = new WWW("https://www.kafabilgisayar.com/arama?tip=1&word=" + arama_yazi.text + "&sc=2");

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }

        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("urun_resmi\">", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("urun_resmi\">", kacinci_index) + "urun_resmi\">".Length;

                string search = "title=\"";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                        urun_title = urun_title.Replace("�", "'");
                        Debug.Log(urun_title);
                    }
                }
                search = "data-original=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        poster_url = www_request.text.Substring(start, end - start);
                        Debug.Log(poster_url);
                    }
                }
                search = "<span>KDV Dahil";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("&nbsp;TL", start);
                    if (end >= 0)
                    {
                        if (www_request.text.Substring(start, end - start) != "")
                        {
                            urun_fiyat = www_request.text.Substring(start, end - start);
                            urun_fiyat = urun_fiyat.Replace(".", string.Empty);
                            urun_fiyat = urun_fiyat.Replace(",", ".");
                            urun_fiyat = urun_fiyat.Replace(" ", string.Empty);
                        }
                        else
                        {
                            urun_fiyat = "";
                        }
                        Debug.Log(urun_fiyat);
                    }
                }
                else
                {
                    urun_fiyat = "";
                }
                search = "href=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_link = "https://www.kafabilgisayar.com" + www_request.text.Substring(start, end - start);
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[0]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat + " TL", poster_url, site_name_index[0], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    IEnumerator a1()
    {
        www_request = new WWW("https://www.kibristeknoloji.com/page.php?searchType=3&act=arama&str=" + arama_yazi.text);

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }

        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("nopad relative\">", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("nopad relative\">", kacinci_index) + "nopad relative\">".Length;

                string search = "title=\"";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                        Debug.Log(urun_title);
                    }
                }
                search = "src=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        poster_url = karakter_edited("https://www.kibristeknoloji.com" + www_request.text.Substring(start, end - start));
                        Debug.Log(poster_url);
                    }
                }
                search = "fiyat_deger\">";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("</span>", start);
                    if (end >= 0)
                    {
                        if (www_request.text.Substring(start, end - start) != "")
                        {
                            urun_fiyat = www_request.text.Substring(start, end - start);
                        }
                        else
                        {
                            urun_fiyat = "";
                        }
                        Debug.Log(urun_fiyat);
                    }
                }
                search = "name\"><a href=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_link = karakter_edited("https://www.kibristeknoloji.com/" + www_request.text.Substring(start, end - start));
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[1]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat + " USD", poster_url, site_name_index[1], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    IEnumerator a2()
    {
        www_request = new WWW("https://digikeycomputer.com/arama/?src=" + arama_yazi.text + "&stock=true");

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }
      
        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("listeGosterim col-md-4", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi && www_request.text.IndexOf("herhangi bir sonuç bulunamadı.") < 0)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("listeGosterim col-md-4", kacinci_index) + "listeGosterim col-md-4".Length;
                //////////////////////////////////////////
                string search = "data-short-limit=\"95\">";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("<", start);
                    if (end >= 0)
                    {
                        urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                        Debug.Log(urun_title);
                    }
                }
                search = "image=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        poster_url = karakter_edited("https://digikeycomputer.com" + www_request.text.Substring(start, end - start));
                        Debug.Log(poster_url);
                    }
                }
                search = "class=\"h2\">";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf(" TL", start);
                    if (end >= 0)
                    {
                        if (www_request.text.Substring(start, end - start) != "")
                        {
                            urun_fiyat = www_request.text.Substring(start, end - start);
                            urun_fiyat = urun_fiyat.Replace(".", string.Empty);
                            urun_fiyat = urun_fiyat.Replace(",", ".");
                            urun_fiyat = urun_fiyat.Replace(" ", string.Empty);
                        }
                        else
                        {
                            urun_fiyat = "";
                        }
                        Debug.Log(urun_fiyat);
                    }
                }
                search = "href=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_link = karakter_edited("https://digikeycomputer.com/" + www_request.text.Substring(start, end - start));
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[2]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat + " TL", poster_url, site_name_index[2], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    IEnumerator a3()
    {
        www_request = new WWW("https://www.boravin.com/arama?tip=1&word=" + arama_yazi.text);
        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }
      
        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("urun_resmi\">", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi && www_request.text.IndexOf("Ürün bulunamadı.") < 0)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("urun_resmi\">", kacinci_index) + "urun_resmi\">".Length;
                //////////////////////////////////////////
                string search = "title=\"";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                        urun_title = urun_title.Replace("�", "'");
                        Debug.Log(urun_title);
                    }
                }

                search = "data-original=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        poster_url = karakter_edited(www_request.text.Substring(start, end - start));
                        Debug.Log(poster_url);
                    }
                }

                search = "<span>";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("&nbsp;TL", start);
                    if (end >= 0)
                    {
                        urun_fiyat = www_request.text.Substring(start, end - start);
                        urun_fiyat = urun_fiyat.Replace(".", string.Empty);
                        urun_fiyat = urun_fiyat.Replace(",", ".");
                        urun_fiyat = urun_fiyat.Replace(" ", string.Empty);
                        Debug.Log(urun_fiyat);
                    }
                }
                else
                {
                    urun_fiyat = "";
                }

                search = "href=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_link = karakter_edited("https://digikeycomputer.com/" + www_request.text.Substring(start, end - start));
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[3]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat + " TL", poster_url, site_name_index[3], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    
    IEnumerator a4()
    {
        www_request = new WWW("https://sales.chipcomputer.org/arama/?src=" + arama_yazi.text + "&stock=true");

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }

        yield return www_request;
        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("col-md-3\"> <a", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi && www_request.text.IndexOf("herhangi bir sonuç bulunamadı") < 0)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("col-md-3\"> <a", kacinci_index) + "col-md-3\"> <a".Length;
                //////////////////////////////////////////
                string search = "itemprop=\"name\" title=\"";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                        urun_title = urun_title.Replace("�", "'");
                        Debug.Log(urun_title);
                    }
                }

                search = "image=\"/Resim/";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        poster_url = karakter_edited("https://sales.chipcomputer.org/Resim/" + www_request.text.Substring(start, end - start));
                        Debug.Log(poster_url);
                    }
                }

                search = "itemprop=\"price\">";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("</p>", start);
                    if (end >= 0)
                    {
                        urun_fiyat = www_request.text.Substring(start, end - start);
                        urun_fiyat = urun_fiyat.Replace(".", string.Empty);
                        urun_fiyat = urun_fiyat.Replace(",", ".");
                        urun_fiyat = urun_fiyat.Replace("$", "USD");
                        Debug.Log(urun_fiyat);
                    }
                }
                else
                {
                    urun_fiyat = "";
                }

                search = "href=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_link = karakter_edited("https://sales.chipcomputer.org" + www_request.text.Substring(start, end - start));
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[4]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat, poster_url, site_name_index[4], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    IEnumerator a5()
    {
        www_request = new WWW("https://sharafstore.com/?s=" + arama_yazi.text + "&post_type=product&dgwt_wcas=1");

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }

        yield return www_request;
        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("product-element-top\">", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("product-element-top\">", kacinci_index) + "product-element-top\">".Length;
                //////////////////////////////////////////
                string search = "product-title\"><a href=";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    p = p + search.Length;
                    search = "\">";
                    p = www_request.text.IndexOf(search, p);
                    if (p >= 0)
                    {
                        int start = p + search.Length;
                        int end = www_request.text.IndexOf("</a>", start);
                        if (end >= 0)
                        {
                            urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                            // urun_title = urun_title.Replace("�", "'");
                            Debug.Log(urun_title);
                        }
                    }
                }

                search = "src=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        poster_url = karakter_edited("https:" + www_request.text.Substring(start, end - start));
                        Debug.Log(poster_url);
                    }
                }

                search = "Price-amount amount\"><span";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    p = p + search.Length;
                    search = "</span>";
                    p = www_request.text.IndexOf(search, p);
                    if (p >= 0)
                    {
                        int start = p + search.Length;
                        int end = www_request.text.IndexOf("</s", start);
                        if (end >= 0)
                        {
                            urun_fiyat = www_request.text.Substring(start, end - start);
                            urun_fiyat = urun_fiyat.Replace(".", string.Empty);
                            urun_fiyat = urun_fiyat.Replace(",", ".");
                            urun_fiyat = urun_fiyat.Replace(" ", string.Empty);
                            urun_fiyat = urun_fiyat.Replace("\n", string.Empty);
                            Debug.Log("fiyat: " + www_request.text.Substring(start, end - start));
                        }
                    }
                    else
                    {
                        urun_fiyat = "";
                    }
                }
                else
                {
                    urun_fiyat = "";
                }

                search = "href=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_link = karakter_edited(www_request.text.Substring(start, end - start));
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[5]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat + " TL", poster_url, site_name_index[5], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    IEnumerator a6()
    {
        www_request = new WWW("https://fistikbilgisayar.com/index.php?route=product/search&search=" + arama_yazi.text);

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }

        yield return www_request;
        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("ttvproduct-image col-sm-12 col-md-3\"><a", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("ttvproduct-image col-sm-12 col-md-3\"><a", kacinci_index) + "ttvproduct-image col-sm-12 col-md-3\"><a".Length;
                //////////////////////////////////////////
                string search = "title=\"";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                        // urun_title = urun_title.Replace("�", "'");
                        Debug.Log(urun_title);
                    }
                }

                search = "src=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        poster_url = karakter_edited(www_request.text.Substring(start, end - start));
                        Debug.Log(poster_url);
                    }
                }

                search = "class=\"price\">";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("</span>", start);
                    if (end >= 0)
                    {
                        urun_fiyat = www_request.text.Substring(start, end - start);
                        urun_fiyat = urun_fiyat.Replace(".", string.Empty);
                        urun_fiyat = urun_fiyat.Replace(",", ".");
                        urun_fiyat = urun_fiyat.Replace(" ", string.Empty);
                        urun_fiyat = urun_fiyat.Replace("\n", string.Empty);
                        urun_fiyat = urun_fiyat.Replace("$", string.Empty);
                        Debug.Log("fiyat: " + www_request.text.Substring(start, end - start));
                    }
                }
                else
                {
                    urun_fiyat = "";
                }

                search = "href=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_link = karakter_edited(www_request.text.Substring(start, end - start));
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[6]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat + " USD", poster_url, site_name_index[6], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    IEnumerator a7()
    {
        www_request = new WWW("https://www.kibrisbilgisayarcim.com/index.php?route=product/search&search=" + arama_yazi.text + "&sub_category=true&description=true");

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }

        yield return www_request;
        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("product-thumb row\">", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("product-thumb row\">", kacinci_index) + "product-thumb row\">".Length;
                //////////////////////////////////////////
                string search = "title=\"";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                        // urun_title = urun_title.Replace("�", "'");
                        Debug.Log(urun_title);
                    }
                }

                search = "src=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        poster_url = karakter_edited(www_request.text.Substring(start, end - start));
                        Debug.Log(poster_url);
                    }
                }

                search = "class=\"price\">";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("<span", start);
                    if (end >= 0)
                    {
                        urun_fiyat = www_request.text.Substring(start, end - start);
                        urun_fiyat = urun_fiyat.Replace(".", string.Empty);
                        urun_fiyat = urun_fiyat.Replace(",", ".");
                        urun_fiyat = urun_fiyat.Replace(" ", string.Empty);
                        urun_fiyat = urun_fiyat.Replace("\n", string.Empty);
                        urun_fiyat = urun_fiyat.Replace("$", string.Empty);
                        Debug.Log("fiyat: " + www_request.text.Substring(start, end - start));
                    }
                }
                else
                {
                    urun_fiyat = "";
                }

                search = "href=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_link = karakter_edited(www_request.text.Substring(start, end - start));
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[7]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat + " USD", poster_url, site_name_index[7], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    IEnumerator a8()
    {
        www_request = new WWW("https://www.cypruscomputerglobal.com/arama/?src=" + arama_yazi.text + "&stock=true");

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }

        yield return www_request;
        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("<meta itemprop=\"url", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi && www_request.text.IndexOf("herhangi bir sonuç bulunamadı.") < 0)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("<meta itemprop=\"url", kacinci_index) + "<meta itemprop=\"url".Length;
                //////////////////////////////////////////
                string search = "title=\"";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                        // urun_title = urun_title.Replace("�", "'");
                        Debug.Log(urun_title);
                    }
                }

                search = "src=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        poster_url = karakter_edited("https://www.cypruscomputerglobal.com" + www_request.text.Substring(start, end - start));
                        Debug.Log(poster_url);
                    }
                }

                search = "price\">";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("$", start);
                    if (end >= 0)
                    {
                        urun_fiyat = www_request.text.Substring(start, end - start);
                        urun_fiyat = urun_fiyat.Replace(".", string.Empty);
                        urun_fiyat = urun_fiyat.Replace(",", ".");
                        urun_fiyat = urun_fiyat.Replace(" ", string.Empty);
                        urun_fiyat = urun_fiyat.Replace("\n", string.Empty);
                        urun_fiyat = urun_fiyat.Replace("$", string.Empty);
                        Debug.Log("fiyat: " + www_request.text.Substring(start, end - start));
                    }
                }
                else
                {
                    urun_fiyat = "";
                }

                search = "content=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_link = karakter_edited("https://www.cypruscomputerglobal.com" + www_request.text.Substring(start, end - start));
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[8]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat + " USD + KDV ", poster_url, site_name_index[8], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    IEnumerator a9()
    {
        www_request = new WWW("https://www.teknogoldonline.com/Arama?1&kelime=" + arama_yazi.text);

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }

        yield return www_request;
        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("productImage\">", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("productImage\">", kacinci_index) + "productImage\">".Length;
                //////////////////////////////////////////
                string search = "title=\"";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                        // urun_title = urun_title.Replace("�", "'");
                        Debug.Log(urun_title);
                    }
                }

                search = "src='";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("'", start);
                    if (end >= 0)
                    {
                        poster_url = karakter_edited("https://www.teknogoldonline.com" + www_request.text.Substring(start, end - start));
                        Debug.Log(poster_url);
                    }
                }

                search = "discountPrice\">";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    search = "<span>";
                    p = www_request.text.IndexOf(search, p);
                    if (p >= 0)
                    {
                        int start = p + search.Length;
                        int end = www_request.text.IndexOf("</span>", start);
                        if (end >= 0)
                        {
                            urun_fiyat = www_request.text.Substring(start, end - start);
                            urun_fiyat = urun_fiyat.Replace(".", string.Empty);
                            urun_fiyat = urun_fiyat.Replace(",", ".");
                            urun_fiyat = urun_fiyat.Replace(" ", string.Empty);
                            urun_fiyat = urun_fiyat.Replace("\n", string.Empty);
                            urun_fiyat = urun_fiyat.Replace("₺", string.Empty);
                            Debug.Log("fiyat: " + www_request.text.Substring(start, end - start));
                        }
                    }
                    else
                    {
                        urun_fiyat = "";
                    }

                }
                

                search = "href='";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("'", start);
                    if (end >= 0)
                    {
                        urun_link = karakter_edited("https://www.teknogoldonline.com" + www_request.text.Substring(start, end - start));
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[9]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat + " TL", poster_url, site_name_index[9], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    IEnumerator a10()
    {
        www_request = new WWW("https://durmazz.com/index.php?route=product/search&search=" + arama_yazi.text + "&description=true#/availability=1/sort=p.sort_order/order=ASC/limit=20");

        return_sayac = return_sayac_count; while (!www_request.isDone && return_sayac > 0) { return_sayac--; yield return new WaitForSeconds(0.5f); }

        yield return www_request;
        if (string.IsNullOrEmpty(www_request.error))
        {
            while (www_request.text.IndexOf("class=\"image \">", kacinci_index) >= 0 && urun_count < urun_gosterim_sayisi)
            {
                urun_title = "";
                urun_fiyat = "";
                urun_link = "";
                poster_url = "";

                kacinci_index = www_request.text.IndexOf("class=\"image \">", kacinci_index) + "class=\"image \">".Length;
                //////////////////////////////////////////
                string search = "title=\"";
                int p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_title = karakter_edited(www_request.text.Substring(start, end - start));
                        // urun_title = urun_title.Replace("�", "'");
                        Debug.Log(urun_title);
                    }
                }

                search = "data-src=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        poster_url = karakter_edited(www_request.text.Substring(start, end - start));
                        Debug.Log(poster_url);
                    }
                }

                search = "price\">";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("</div>", start);
                    if (end >= 0)
                    {
                        urun_fiyat = www_request.text.Substring(start, end - start);
                        urun_fiyat = urun_fiyat.Replace(",", ".");
                        urun_fiyat = urun_fiyat.Replace(" ", string.Empty);
                        urun_fiyat = urun_fiyat.Replace("\n", string.Empty);
                        urun_fiyat = urun_fiyat.Replace("$", string.Empty);
                        Debug.Log("durmazz fiyat: " + www_request.text.Substring(start, end - start));
                    }
                }
                else
                {
                    urun_fiyat = "";
                }

                search = "href=\"";
                p = www_request.text.IndexOf(search, kacinci_index);
                if (p >= 0)
                {
                    int start = p + search.Length;
                    int end = www_request.text.IndexOf("\"", start);
                    if (end >= 0)
                    {
                        urun_link = karakter_edited(www_request.text.Substring(start, end - start));
                        Debug.Log(urun_link);
                    }
                }

                urun_count++;

                if (urun_title != "" && poster_url != "" && urun_fiyat != "")
                {
                    if (siteden_sonuc_Var == 0)
                    {
                        site_title_create(site_name_index[10]);
                        sonuclar_site_sayisi++;
                        siteden_sonuc_Var = 1;
                    }
                    yield return StartCoroutine(urun_obje_create(urun_title, urun_fiyat + " USD", poster_url, site_name_index[10], urun_link));
                }
            }
            Debug.Log("bitti");
        }
    }
    
    string karakter_edited(string title)
    {
        title = title.Replace("&amp;", "&");
        title = title.Replace("&#252;", "ü");
        title = title.Replace("&#231;", "ç");
        title = title.Replace("&#39;", "'");
        title = title.Replace("&#220;", "Ü");
        title = title.Replace("&#246;", "ö");
        title = title.Replace("&#199;", "Ç");
        title = title.Replace("&#214;", "Ö");
        title = title.Replace("&#226;", "â");
        title = title.Replace("&#238;", "î");
        title = title.Replace("&quot;", "\"");

        return title;
    }
    IEnumerator urun_obje_create(string urun_title, string ürün_fiyatı, string resim_link, string site_ismi, string urun_link)
    {
        www_resim = new WWW(resim_link);
        yield return www_resim;
        if (string.IsNullOrEmpty(www_resim.error))
        {
            GameObject post = Instantiate(genel_obje);
            post.transform.SetParent(content_fiyat_sonuclari.transform);
            post.transform.GetComponent<RectTransform>().localScale = new Vector3(1.02f, 1.02f, 1.02f);

            post.transform.Find("urun_title").GetComponentInChildren<Text>().text = urun_title;
            post.transform.Find("site_ismi").GetComponentInChildren<Text>().text = site_ismi;
            post.transform.Find("ürün_fiyatı").GetComponentInChildren<Text>().text = ürün_fiyatı;

            post.GetComponent<favori_obje>().urun_url = urun_link;
            post.GetComponent<favori_obje>().resim_url = resim_link;
            post.transform.Find("Image").gameObject.transform.GetComponentInChildren<RawImage>().texture = new Texture2D(512, 512, TextureFormat.RGB24, false);
            www_resim.LoadImageIntoTexture((Texture2D)post.transform.Find("Image").gameObject.transform.GetComponentInChildren<RawImage>().texture);
            TextureScale.Bilinear((Texture2D)post.transform.Find("Image").gameObject.transform.GetComponentInChildren<RawImage>().texture, 512, 512);
        }
    }
    private void urun_obje_create_sepet(string urun_title, string ürün_fiyatı, Texture2D resim, string site_ismi, string urun_link)
    {
        GameObject post = Instantiate(genel_obje);
        post.transform.SetParent(content_sepet.transform);
        post.transform.GetComponent<RectTransform>().localScale = new Vector3(1.02f, 1.02f, 1.02f);

        post.transform.Find("urun_title").GetComponentInChildren<Text>().text = urun_title;
        post.transform.Find("site_ismi").GetComponentInChildren<Text>().text = site_ismi;
        post.transform.Find("ürün_fiyatı").GetComponentInChildren<Text>().text = ürün_fiyatı;
        post.transform.Find("sepetten çıkar").gameObject.SetActive(true);
        post.GetComponent<favori_obje>().urun_url = urun_link;
        post.transform.Find("Image").gameObject.transform.GetComponentInChildren<RawImage>().texture = resim;
    }

    private void site_title_create(string site_ismi)
    {
        GameObject post = Instantiate(site_title_obje);
        post.transform.SetParent(content_fiyat_sonuclari.transform);
        post.transform.GetComponent<RectTransform>().localScale = new Vector3(1.02f, 1.02f, 1.02f);

        post.transform.Find("site_ismi").GetComponentInChildren<Text>().text = site_ismi;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (fiyat_sonuclari_panel.active == true)
            {
                profile_view_arttir();
                fiyat_sonuclari_panel.SetActive(false);
                arama_yazi.text = "";
                yukleniyor_paneli.SetActive(false);
            }
            else if (profilim_panel.active == true)
            {
                profilim_panel.SetActive(false);
                yukleniyor_paneli.SetActive(false);
            }
            else if (sepet_panel.active == true)
            {
                sepet_panel.active = false;
                yukleniyor_paneli.SetActive(false);
            }
            else
            {
                Application.Quit();
            }
        }
    }
   
    private void panel_content_destroy(GameObject content)
    {
        if (content.transform.childCount != 0)
        {
            int i = 0;
            GameObject[] allChildren = new GameObject[content.transform.childCount];
            foreach (Transform child in content.transform)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }
            foreach (GameObject child in allChildren)
            {
                if (child.transform.name != "plaka profil ust panel (1)" && child.transform.name != "barkodpanel")
                {
                    Destroy(child.gameObject, 0);
                }
            }
        }
    }
    public void profile_view_arttir()
    {
        arama_sonuc_Sayisi++;
        StopAllCoroutines();
    }
    [Header("YUKLENİYOR PANELİ İCİN DEGİSKENLER")]
    public GameObject yukleniyor_paneli;

    [Header("BİLDİRİM ATMA İLE İLGİLİ DEGİSKENLER")]
    public Text bildirim_yazisi;
    public int bildirim_suresi = 2;
    public GameObject toast_mesaj_paneli;
    void debug_kapat()
    {
        toast_mesaj_paneli.SetActive(false);
    }
    public void bildirim_create(string mesaj)
    {
        toast_mesaj_paneli.SetActive(true);
        bildirim_yazisi.text = mesaj;
        Invoke("debug_kapat", bildirim_suresi);
    }
}

