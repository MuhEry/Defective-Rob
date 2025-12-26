using UnityEngine;
using UnityEngine.SceneManagement; // Sahne geçişleri için kütüphane
using UnityEngine.UI; // Eğer UI elementin Text değil Image ise bunu kullanabilirsin

public class Door : MonoBehaviour
{
    [Header("Ayarlar")]
    public string sonrakiSahneAdi; // Geçilecek sahnenin tam adı
    public GameObject uyariYazisiUI; // "F'ye Bas" yazısı (Game Object olarak)

    private bool kapiyaYakinMi = false; // Oyuncu kapıda mı?

    void Start()
    {
        // Oyun başladığında uyarı yazısı kapalı olsun
        if (uyariYazisiUI != null)
            uyariYazisiUI.SetActive(false);
    }

    void Update()
    {
        // Eğer oyuncu kapıya yakınsa VE 'F' tuşuna basarsa
        if (kapiyaYakinMi && Input.GetKeyDown(KeyCode.F))
        {
            SahneDegistir();
        }
    }

    void SahneDegistir()
    {
        // Sahneyi ismine göre yükle
        SceneManager.LoadScene(sonrakiSahneAdi);
    }

    // Oyuncu Trigger alanına girdiğinde
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Giren objenin etiketi "Player" mı?
        {
            kapiyaYakinMi = true;
            if (uyariYazisiUI != null)
                uyariYazisiUI.SetActive(true); // Yazıyı göster
        }
    }

    // Oyuncu Trigger alanından çıktığında
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            kapiyaYakinMi = false;
            if (uyariYazisiUI != null)
                uyariYazisiUI.SetActive(false); // Yazıyı gizle
        }
    }
}