using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class favori_obje : MonoBehaviour {

    public string urun_url;
    public string resim_url;

    public void sepete_ekle()
    {
        GameObject.Find("firebase-message").GetComponent<book_view>().sepete_ekle_genel(
            gameObject.transform.Find("urun_title").GetComponentInChildren<Text>().text, 
            gameObject.transform.Find("ürün_fiyatı").GetComponentInChildren<Text>().text,
            gameObject.transform.Find("site_ismi").GetComponentInChildren<Text>().text, 
            urun_url,
            (Texture2D)gameObject.transform.Find("Image").GetComponentInChildren<RawImage>().texture);
    }
    public void sepetten_cikar()
    {
        GameObject.Find("firebase-message").GetComponent<book_view>().sepetten_cikar_genel(gameObject.transform.Find("urun_title").GetComponentInChildren<Text>().text);

        Destroy(gameObject);
    }
}
