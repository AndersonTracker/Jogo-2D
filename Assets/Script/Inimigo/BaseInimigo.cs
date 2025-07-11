using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class BaseInimigo : MonoBehaviour
{

    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private string groundTag = "Ground";

    private Rigidbody2D _rb;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = gravityScale;
        _rb.freezeRotation = true;  

        
        var col = GetComponent<BoxCollider2D>();
        col.isTrigger = false;
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        
        if (_isGrounded)
            Debug.Log($"{name} está no chão");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(groundTag))
        {
            _isGrounded = true;
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(groundTag))
        {
            _isGrounded = false;
        }
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }
}