using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RobotController : MonoBehaviour
{
    [Header("Ses Efektleri")] // --- YENİ ---
    public AudioClip atesSesi; // --- YENİ: Editörden ses dosyasını buraya atacağız ---
    private AudioSource audioSource;
    [Header("Hareket Ayarları")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    private bool isGrounded;
    [Header("UI Ayarları")] // --- YENİ ---
    public Slider healthSlider;
    [Header("Silah Ayarları")]
    public GameObject bulletPrefab;
    public Transform muzzle;
    public float bulletSpeed = 20f;
    public float recoilForce = 5f;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    [Header("Sağlık Ayarları")]
    public float healt;
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Bileşenler")]
    private Rigidbody rb;
    private Animator anim;
    private Collider col;
    private Vector3 moveInput;

    [Header("Tema Ayarları")]
    public bool controlsInverted = false;
    public bool tuslarKarissin = false;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        currentHealth = healt;
        audioSource = GetComponent<AudioSource>();
        // Animator kontrolü
        if (anim == null)
        {
            Debug.LogWarning(gameObject.name + " üzerinde Animator bulunamadı!");
        }
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Barın maksimum değeri 100 olsun
            healthSlider.value = currentHealth;
        }
        // Rigidbody ayarları (Z ekseninde dönmemeli)
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | 
                            RigidbodyConstraints.FreezeRotationZ |
                            RigidbodyConstraints.FreezePositionZ;
        }
    }

    void Update()
    {
        if (isDead) return; // Ölüyse hiçbir şey yapma

        HandleMovementInput();
        HandleJumpInput();
        HandleShootInput();
        UpdateAnimations();
        UpdateRotation();
    }
    public void Heal(float amount)
    {
        currentHealth += amount;

        // Canımız asla maksimum canı (100) geçmemeli
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // UI Barını güncelle
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
    void HandleMovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        if (controlsInverted) moveX *= -1;
        moveInput = new Vector3(moveX, 0, 0);
    }

    void HandleJumpInput()
    {
        bool ziplamaTusuBasildiMi = false;

        if (tuslarKarissin)
        {
            // LEVEL 3 MODU: Mouse'a tıklayınca Zıpla
            ziplamaTusuBasildiMi = Input.GetMouseButtonDown(0);
        }
        else
        {
            // NORMAL MOD: Space ile Zıpla
            ziplamaTusuBasildiMi = Input.GetButtonDown("Jump");
        }

        if (ziplamaTusuBasildiMi && isGrounded)
        {
            if (anim != null) anim.SetTrigger("Jump");
        }
    }

    // --- BURAYI DA DEĞİŞTİRİYORUZ ---
    void HandleShootInput()
    {
        bool atesTusuBasildiMi = false;

        if (tuslarKarissin)
        {
            // LEVEL 3 MODU: Space tuşuna basınca Ateş Et
            // (GetButton yerine GetKey kullanıyoruz ki seri atış yapılabilsin)
            atesTusuBasildiMi = Input.GetKey(KeyCode.Space); 
        }
        else
        {
            // NORMAL MOD: Mouse ile Ateş Et
            atesTusuBasildiMi = Input.GetMouseButton(0);
        }

        if (atesTusuBasildiMi && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void UpdateAnimations()
    {
        if (anim == null) return;

        // Animator'da parametre var mı kontrol et
        if (HasParameter("Speed"))
            anim.SetFloat("Speed", Mathf.Abs(moveInput.x));

        if (HasParameter("isGrounded"))
            anim.SetBool("isGrounded", isGrounded);
    }

    void UpdateRotation()
    {
        if (moveInput.x != 0)
        {
            // 2D platformer için Euler açılarıyla dönüş
            if (moveInput.x > 0)
                transform.rotation = Quaternion.Euler(0, 90, 0);  // Sağa
            else
                transform.rotation = Quaternion.Euler(0, -90, 0); // Sola
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || muzzle == null)
        {
            return;
        }

        // Animasyon tetikle
        if (anim != null)
        {
            anim.SetTrigger("Shoot");
        }
        if (audioSource != null && atesSesi != null)
        {
            audioSource.PlayOneShot(atesSesi);
        }
        // Mermiyi oluştur
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);

        // Mermi sahibini belirt (Enemy'ye zarar versin, Player'a vermesin)
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.shooterTag = "Player";
            bulletScript.damage = 20f; // İsterseniz public değişken yapın
        }

        // Mermiyi fırlat (2D hareketi için sadece X ekseni)
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            Vector3 shootDirection = transform.forward;
            bulletRb.linearVelocity = new Vector3(shootDirection.x * bulletSpeed, 0, 0);
        }

        // Geri tepme efekti
        if (rb != null)
        {
            rb.AddForce(-transform.forward * recoilForce, ForceMode.Impulse);
        }

        // Mermiyi temizle
        Destroy(bullet, 2f);
    }

    // Animation Event'ten çağrılacak (Jump animasyonuna ekle)
    public void ApplyJumpForce()
    {
        if (rb == null) return;

        // Y eksenindeki hızı sıfırla (daha tutarlı zıplama)
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // Hareket fiziği (sadece X ekseninde)
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, 0);
        }
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        
        if (currentHealth <= 0)
        {
            Die();
            currentHealth = 0;
        }
    }

    void Die()
    {
        if (isDead) return;
        Invoke("SahneyiTekrarBaslat", 2f);
        isDead = true;
        Debug.Log("Player öldü!");

        // Ölüm animasyonu
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }
        /*
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }*/

        this.enabled = false;
    }
    void SahneyiTekrarBaslat()
    {
        // Aktif olan sahnenin ismini alıp aynısını tekrar yükler
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Yer kontrolü (Collider ile)
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Yardımcı fonksiyon: Animator'da parametre var mı kontrol et
    bool HasParameter(string paramName)
    {
        if (anim == null) return false;

        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    // Debug için görselleştirme
    void OnDrawGizmosSelected()
    {
        // Muzzle pozisyonunu göster
        if (muzzle != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(muzzle.position, 0.1f);
            Gizmos.DrawLine(muzzle.position, muzzle.position + transform.forward * 2f);
        }
    }
}