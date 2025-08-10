using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))] // تضمین وجود کامپوننت‌ها
public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 10f;
    public float rotationSpeed = 15f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip jumpSound; // فیلد سریالایز شده
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Camera mainCam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        mainCam = Camera.main;

        // اعتبارسنجی اولیه
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("AudioSource was added automatically");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector2 touchPosition = mainCam.ScreenToWorldPoint(
                Input.mousePosition
            );
            ApplyForceFromTouch(touchPosition);
        }
    }

    void ApplyForceFromTouch(Vector2 touchPos)
    {
        // محاسبه جهت
        Vector2 direction = ((Vector2)transform.position - touchPos).normalized;

        // اعمال نیرو
        rb.velocity = direction * jumpForce;

        // چرخش
        float torque = (touchPos.x > transform.position.x) ? -rotationSpeed : rotationSpeed;
        rb.AddTorque(torque);

        // پخش صدا با بررسی null
        PlayJumpSound();
    }

    void PlayJumpSound()
    {
        if (jumpSound == null)
        {
            Debug.LogError("Jump sound clip is not assigned!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing!");
            return;
        }

        audioSource.PlayOneShot(jumpSound);
    }
}