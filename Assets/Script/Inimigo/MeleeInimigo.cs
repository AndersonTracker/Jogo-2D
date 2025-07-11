using UnityEngine;
using System.Collections;

public class MeleeInimigo : BaseInimigo
{
    [SerializeField] private Transform detectPosition;
    [SerializeField] private Vector2 detectBoxSize;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Animator animator;
    [SerializeField] private float attackCooldown;

    [SerializeField] private AudioClip[] audioClips;

    private bool isDead = false;
    private float cooldownTimer;
    private Health InimigoHealth;


    protected void Update()
    {
        if (isDead)
            return;
        print("is in sight? " + PlayerInSight());

        cooldownTimer += Time.deltaTime;

        InimigoHealth = GetComponent<Health>();
        InimigoHealth.OnHurt += InimigoHurtAnim;
        InimigoHealth.OnDead += InimigoDeadAnim;

        VerifyCanAttack();

    }

    private void InimigoHurtAnim()
    {
        animator.SetTrigger("HurtInimigo");
    }

    private void InimigoDeadAnim()
    {
        isDead = true;
        animator.SetTrigger("DeadInimigo");
        StartCoroutine(DestroyEnemy(1));
    }

    private void VerifyCanAttack()
    {
        if (cooldownTimer < attackCooldown) return;
        if (PlayerInSight())
        {
            animator.SetTrigger("attack");
            AttackPlayer();
        }
    }

    private bool PlayerInSight()
    {
        Collider2D playerCollider = CheckPlayerInDetectArea();
        return playerCollider != null;
    }

    private void OnDrawGizmos()
    {
        if (detectPosition == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(detectPosition.position, detectBoxSize);
    }

    private void AttackPlayer()
    {
        cooldownTimer = 0;
        if (CheckPlayerInDetectArea().TryGetComponent(out Health playerHealth))
        {
            print("Making player take damage");
            playerHealth.TakeDamage();
        }
    }

    private IEnumerator DestroyEnemy(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);

    }

    private Collider2D CheckPlayerInDetectArea()
    {
        return Physics2D.OverlapBox(detectPosition.position, detectBoxSize, 0f, playerLayer);
    }
}