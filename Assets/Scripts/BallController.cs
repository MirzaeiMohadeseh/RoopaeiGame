using UnityEngine;
using UnityEngine.EventSystems;

public class BallController : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForce = 10f;
    public AudioClip kickSound;
    public AudioClip groundHitSound; 

    private Rigidbody2D _rb;
    private AudioSource _audioSource;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!ScoreCounter.Instance._isGameActive &&
           (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            if (!IsPointerOverUIElement())
            {
                ScoreCounter.Instance.StartGame();
                _rb.gravityScale = 1f;
                PlaySound(kickSound);
            }
        }
    }
    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.currentSelectedGameObject != null;
    }

    public void ApplyForce(Vector2 touchPosition)
    {
        Vector2 forceDirection = ((Vector2)transform.position - touchPosition).normalized;

        _rb.AddForce(forceDirection * jumpForce, ForceMode2D.Impulse);
        PlaySound(kickSound);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ScoreCounter.Instance.HandleGroundHit();
            PlaySound(groundHitSound); 
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    public void ResetBall()
    {
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        transform.position = ScoreCounter.Instance.startPosition.position;
        _rb.gravityScale = 0f;
    }
    public void ApplyPurchasedItems()
    {
        if (PlayerPrefs.GetInt("GoldenBall_Unlocked", 0) == 1)
        {
            // اعمال اثر توپ طلایی
        }

        if (PlayerPrefs.GetInt("ZeroGravity_Unlocked", 0) == 1)
        {
            Physics2D.gravity = Vector2.zero;
        }
    }
}