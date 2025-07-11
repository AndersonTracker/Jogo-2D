using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float speedMoved = 4;
    [SerializeField] private float JumpForce  = 4;

    private Rigidbody2D _rb;
    private IsGroundedChecker isGroundedChecker;
    private Health     playerHealth;
    private Animator   animator;

    private bool isDead = false;

    private void Awake()
    {
        _rb              = GetComponent<Rigidbody2D>();
        isGroundedChecker= GetComponent<IsGroundedChecker>();
        animator         = GetComponent<Animator>();

        playerHealth = GetComponent<Health>();
        playerHealth.OnHurt += PlayHurtAnim;
        playerHealth.OnDead += PlayDeadAnim;
    }

    private void PlayHurtAnim()
    {
        animator.SetTrigger("hurt");
    }

    private void PlayDeadAnim()
    {
        isDead = true;
        animator.SetTrigger("dead");
    }

    private void Start()
    {
        GameManager.Instance.InputManager.OnJump += HandleJump;
    }

    private void Update()
    {
        if (isDead) 
            return;

        var input = GameManager.Instance?.InputManager;
        if (input == null) 
            return;

        float moveDirection = input.Movement;
        transform.Translate(moveDirection * Time.deltaTime * speedMoved, 0, 0);

        if (moveDirection < 0)      transform.localScale = new Vector3(-1, 1, 1);
        else if (moveDirection > 0) transform.localScale = Vector3.one;
    }

    private void HandleJump()
    {
        if (isDead) 
            return; 

        if (isGroundedChecker.isGrounded())
        {
            _rb.velocity += Vector2.up * JumpForce;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHurt -= PlayHurtAnim;
            playerHealth.OnDead -= PlayDeadAnim;
        }
        if (GameManager.Instance?.InputManager != null)
            GameManager.Instance.InputManager.OnJump -= HandleJump;
    }
}