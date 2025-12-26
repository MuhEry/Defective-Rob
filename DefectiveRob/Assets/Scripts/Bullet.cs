using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20f;
    public string shooterTag = ""; // "Player" veya "Enemy" - Kim ateş etti?

    void OnCollisionEnter(Collision collision)
    {
        // Kendini atan kişiye zarar verme
        if (collision.gameObject.CompareTag(shooterTag))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            return;
        }

        // DÜŞMANA ÇARPTI (Player mermisi ise)
        if (shooterTag == "Player" && collision.gameObject.CompareTag("Enemy"))
        {
            // DroneAI var mı kontrol et
            DroneAI drone = collision.gameObject.GetComponent<DroneAI>();
            if (drone != null)
            {
                drone.TakeDamage(damage);
            }
            
            // Başka enemy tipleri varsa onları da ekle
            // EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            // if (enemy != null) enemy.TakeDamage(damage);
        }

        // PLAYER'A ÇARPTI (Enemy mermisi ise)
        else if (shooterTag == "Enemy" && collision.gameObject.CompareTag("Player"))
        {
            RobotController player = collision.gameObject.GetComponent<RobotController>();
            if (player != null)
            {
                player.TakeDamage(damage); // Player'a TakeDamage fonksiyonu eklemen gerek
            }
        }

        // Mermiyi yok et
        Destroy(gameObject);
    }
}