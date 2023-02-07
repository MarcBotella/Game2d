using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public float attackRadius = 1.0f;
    public float attackRate = 1.0f;
    private float nextAttackTime = 0.0f;

    public Animator animator;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, attackRadius);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;
                hit.collider.GetComponent<PlayerHealth>().TakeDamage(damage);
                animator.SetTrigger("atack1");
            }
        }
    }
}
