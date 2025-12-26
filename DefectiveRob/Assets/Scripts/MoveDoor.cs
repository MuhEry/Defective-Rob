using UnityEngine;
using UnityEngine.UI; // Eğer UI elementin Text değil Image ise bunu kullanabilirsin

public class MoveDoor : MonoBehaviour
{
    [Header("Ayarlar")]
    public float bekle = 0.5f; // Kapının açılmadan önce bekleme süresi
    public Transform kapi;
    public Transform hedefNokta;
    public float hareketHizi = 3.0f;
    public GameObject yazi;
    private bool aciliyor = false;
    private bool kapiyaYakinMi = false; // Oyuncu kapıda mı?

    void Start()
    {
        if (yazi != null)
            yazi.SetActive(false);
    }

    void Update()
    {
        // Eğer oyuncu kapıya yakınsa VE 'F' tuşuna basarsa
        if (kapiyaYakinMi && Input.GetKeyDown(KeyCode.F))
        {
            Invoke("ac", bekle); // Kapıyı açma fonksiyonunu çağır
        }
        if (aciliyor)
        {
            // Mevcut konumdan, hedef konuma, belirlenen hızda git
            kapi.position = Vector3.MoveTowards(kapi.position, hedefNokta.position, hareketHizi * Time.deltaTime);
        }
        if(kapi.position == hedefNokta.position)
        {
            aciliyor = false;
        }
    }
    void ac()
    {
        aciliyor = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            kapiyaYakinMi = true;
            if (yazi != null)
                yazi.SetActive(true);
        }
    }

    // Oyuncu Trigger alanından çıktığında
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            kapiyaYakinMi = false;
            if (yazi != null)
                yazi.SetActive(false); // Yazıyı gizle
        }
    }
}