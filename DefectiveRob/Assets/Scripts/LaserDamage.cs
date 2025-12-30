using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    [Header("Lazer Ayarları")]
    public float saniyeBasinaHasar = 30f; // Her saniye kaç can gidecek?
    private AudioSource sesKaynagi;

    void Start()
    {
        sesKaynagi = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sesKaynagi.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sesKaynagi.Stop();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // Çarpan şey Player mı?
        if (other.CompareTag("Player"))
        {
            RobotController player = other.GetComponent<RobotController>();

            if (player != null)
            {
                player.TakeDamage(saniyeBasinaHasar * Time.deltaTime);
            }
        }
    }
}