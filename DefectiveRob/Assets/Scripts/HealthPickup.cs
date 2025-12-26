using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float iyilesmeMiktari = 20f; // Ne kadar can verecek?
    public Renderer gorsel1;
    public Renderer gorsel2;
    [Header("Ses Ayarları")]
    public AudioClip deathSound; // Editörden sürükleyeceğin patlama sesi
    private AudioSource audioSource;
    private bool canhakki;
    void Start()
    {
        canhakki = true;
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& canhakki)
        {
            // Oyuncunun scriptine ulaş
            RobotController player = other.GetComponent<RobotController>();

            if (player != null)
            {
               if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
                // 1. Oyuncunun canını artır
                player.Heal(iyilesmeMiktari);

                gorsel1.enabled = false;
                gorsel2.enabled = false;
                Destroy(gameObject, 2f); 
            }
        }
    }
}
