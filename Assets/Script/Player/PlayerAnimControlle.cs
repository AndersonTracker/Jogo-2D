using UnityEngine;
using System.Collections;

public class PlayerAnimControlle : MonoBehaviour
{

    [Header("Propriedades de ataque")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask attackLayer;

    private Animator animator;
    private bool isJumping = false;
    private bool isAttacking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.InputManager.OnJump += OnJump;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance?.InputManager != null)
            GameManager.Instance.InputManager.OnJump -= OnJump;
    }

    private void Update()
    {
        bool isMoving = GameManager.Instance.InputManager.Movement != 0;
        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.A) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void OnJump()
    {
        if (!isJumping)
            StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        isJumping = true;
        animator.SetBool("Pulo", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Pulo", false);
        isJumping = false;
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        animator.SetBool("Ataque", true);
        Attack();
        yield return new WaitForSeconds(1f); // dura 1 segundo
        animator.SetBool("Ataque", false); 
        isAttacking = false;
    }

    private void PlayDeadAnim()
    {
        animator.SetTrigger("dead");
    }

    private void Attack()
    {
        Collider2D[] hittedEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, attackLayer);
        print("Making enemy take damage");
        print(hittedEnemies.Length);

        foreach (Collider2D hittedEnemy in hittedEnemies)
        {
            print("Checking enemy");
            if (hittedEnemy.TryGetComponent(out Health enemyHealth))
            {
                print("Getting damage");
                enemyHealth.TakeDamage();
            }
        }
    }
}