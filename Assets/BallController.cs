using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 0.5f; // Topun normal hareket hızı
    public float sprintSpeedBonus = 0.3f; // Shift ile hız artışı
    public float jumpForce = 15f;
    public float normalMaxVelocity = 3f; // Normal maksimum hız sınırı
    public float sprintMaxVelocity = 5f; // Koşarkenki maksimum hız sınırı
    public float rotationSpeed = 2f; // Topun dönme hızı
    private bool isGrounded;
    private Camera mainCamera;

    // Hızlanma efekti için
    public ParticleSystem speedEffect;
    private bool wasSprintingLastFrame = false;

    // Ses efektleri için
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip coinSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        Debug.Log("Top başlatıldı");
        
        // Fizik ayarları
        rb.linearDamping = 1f; // Sürtünme ekle

        // Audio Source yoksa ekle
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }

        // Particle System yoksa oluştur
        if (speedEffect == null)
        {
            CreateSpeedEffect();
        }
    }

    void CreateSpeedEffect()
    {
        // Yeni bir GameObject oluştur
        GameObject effectObj = new GameObject("SpeedEffect");
        effectObj.transform.parent = this.transform;
        effectObj.transform.localPosition = Vector3.zero;

        // Particle System ekle
        speedEffect = effectObj.AddComponent<ParticleSystem>();
        var main = speedEffect.main;
        main.loop = true;
        main.startLifetime = 0.5f;
        main.startSpeed = 2f;
        main.startSize = 0.2f;
        main.maxParticles = 100;

        // Particle System şeklini ayarla
        var shape = speedEffect.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;

        // Renk ve opaklık ayarları
        var colorOverLifetime = speedEffect.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        colorOverLifetime.color = gradient;

        // Başlangıçta durdur
        speedEffect.Stop();
    }

    void Update()
    {
        // Hareket kontrolü
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Sprint kontrolü
        float currentSpeed = moveSpeed;
        float currentMaxVelocity = normalMaxVelocity;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        
        // Hızlanma efektini kontrol et
        if (isSprinting && !wasSprintingLastFrame && speedEffect != null)
        {
            speedEffect.Play();
        }
        else if (!isSprinting && wasSprintingLastFrame && speedEffect != null)
        {
            speedEffect.Stop();
        }
        wasSprintingLastFrame = isSprinting;

        if (isSprinting)
        {
            currentSpeed += sprintSpeedBonus;
            currentMaxVelocity = sprintMaxVelocity;
        }

        // Kamera yönüne göre hareket vektörünü hesapla
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        
        // Y eksenini sıfırla (yerden yükselmemesi için)
        cameraForward.y = 0;
        cameraRight.y = 0;
        
        // Vektörleri normalize et
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Hareket vektörünü oluştur
        Vector3 movement = (cameraForward * verticalInput + cameraRight * horizontalInput);
        
        // Hareketi uygula
        if (movement.magnitude > 0.1f)
        {
            // Debug mesajları
            Debug.Log("Mevcut hız: " + rb.linearVelocity.magnitude);
            Debug.Log("Maksimum hız: " + currentMaxVelocity);

            // Mevcut hızı kontrol et
            if (rb.linearVelocity.magnitude < currentMaxVelocity)
            {
                // Sabit hızda hareket
                rb.linearVelocity = movement.normalized * currentSpeed;

                // Hareket yönünde dönme ekle
                Vector3 moveDirection = rb.linearVelocity.normalized;
                Vector3 rotationAxis = Vector3.Cross(Vector3.up, moveDirection);
                rb.angularVelocity = rotationAxis * (currentSpeed * rotationSpeed);
            }
        }
        else
        {
            // Hareket tuşlarına basılmıyorsa yavaşla
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.linearVelocity = new Vector3(horizontalVelocity.x * 0.9f, rb.linearVelocity.y, horizontalVelocity.z * 0.9f);
        }

        // Zıplama kontrolü
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                // Zıplama sesi çal
                if (jumpSound != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                }

                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    // Altın toplama sesi çal
    public void PlayCoinSound()
    {
        if (coinSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}