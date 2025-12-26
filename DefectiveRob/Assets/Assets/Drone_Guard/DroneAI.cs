using UnityEngine;

public class DroneAI : MonoBehaviour
{
    [Header("Ses Ayarları")]
    public AudioClip deathSound; // Editörden sürükleyeceğin patlama sesi
    private AudioSource audioSource;
    [Header("Zamanlama")]
    public float wakeUpDelay = 4f;
    private float wakeUpTimer;
    private bool isWokenUp = false;

    [Header("Devriye")]
    public float moveSpeed = 3f;
    public float patrolDistance = 5f;
    private float startX;
    private int direction = 1;

    [Header("Saldırı")]
    public float fireRate = 1f;
    private float nextFireTime;
    public GameObject bulletPrefab;
    public Transform muzzle;
    public float bulletSpeed = 15f;

    [Header("Sağlık")]
    public float maxHealth = 100f;
    private float currentHealth;

    private Rigidbody rb;
    private Collider col;
    private Animator anim;

    private bool playerInSightLine = false;
    private bool isDead = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        anim = GetComponentInChildren<Animator>();

        startX = transform.position.x;
        wakeUpTimer = wakeUpDelay;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        if (!isWokenUp)
        {
            wakeUpTimer -= Time.deltaTime;
            if (wakeUpTimer <= 0)
                isWokenUp = true;
            return;
        }

        if (playerInSightLine)
            Attack();
        else
            Patrol();
    }
    void FixedUpdate()
    {
        if (isDead)
        {
            Vector3 gravity = -9f * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }
    public void SetPlayerInSight(bool value)
    {
        playerInSightLine = value;
    }

    void Patrol()
    {
        if (transform.position.x >= startX + patrolDistance) direction = -1;
        else if (transform.position.x <= startX - patrolDistance) direction = 1;

        rb.linearVelocity = new Vector3(direction * moveSpeed, 0f, 0f);

        transform.rotation = direction > 0
            ? Quaternion.Euler(0, 90, 0)
            : Quaternion.Euler(0, -90, 0);
    }

    void Attack()
    {
        rb.linearVelocity = Vector3.zero;

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (!bulletPrefab || !muzzle) return;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        var rbBullet = bullet.GetComponent<Rigidbody>();
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.shooterTag = "Enemy";
        if (rbBullet != null)
            rbBullet.linearVelocity = transform.forward * bulletSpeed;

        Destroy(bullet, 3f);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        if (anim) anim.SetBool("isDead", true);
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        if (col) col.enabled = false;
        enabled = false;
    }
}
