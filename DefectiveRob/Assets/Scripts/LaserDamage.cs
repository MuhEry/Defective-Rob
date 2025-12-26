using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    [Header("Lazer Ayarları")]
    public float saniyeBasinaHasar = 30f; // Her saniye kaç can gidecek?

    // OnTriggerStay: Nesne lazerin içinde kaldığı SÜRECE çalışır (her karede)
    private void OnTriggerStay(Collider other)
    {
        // Çarpan şey Player mı?
        if (other.CompareTag("Player"))
        {
            RobotController player = other.GetComponent<RobotController>();

            if (player != null)
            {
                // Time.deltaTime ile çarpmak çok önemlidir.
                // Bu sayede bilgisayarın hızı ne olursa olsun saniyede tam 30 can gider.
                // Eğer çarpmazsak, saniyede 60-100 kere vurur ve karakter anında ölür.
                player.TakeDamage(saniyeBasinaHasar * Time.deltaTime);
            }
        }
    }
}