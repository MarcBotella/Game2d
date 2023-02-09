using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int attackDamage = 5;
    public float attackRadius = 1f;
    public float attackDelay = 1f;
    public LayerMask playerLayer;
    private float nextAttackTime;
    public Animator m_animator;
    private Collider m_collider;
    public AudioSource AudioSource;
    

    private void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if(GameManager.life!=0 && GameManager.enemyLife!=0){
                Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRadius, playerLayer);
                foreach (Collider2D player in hitPlayers)
                {
                    m_animator.SetTrigger("Hit");
                    player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
                    nextAttackTime = Time.time + attackDelay;
                    break;
                }
            }else{
                SceneManager.LoadScene("FirstLevel");
                GameManager.life = 100;
            }
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void TakeDamage(int damage){
        GameManager.enemyLife = GameManager.enemyLife - damage;
        if(GameManager.enemyLife == 0){
            m_animator.SetTrigger("Death"); 
            Destroy(gameObject);
            GameManager.enemyLife = 20;       
        }else{
            m_animator.SetTrigger("Hurt");
        }
    }
}
