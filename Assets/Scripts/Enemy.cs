using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public float attackRadius = 1.0f;

    bool booler;

    Quaternion rot;
    RaycastHit2D rchit;

    public Animator animator;

    private void Start(){
        rot = new Quaternion();

    }

    private void Update()
    {
        rchit = Physics2D.Raycast(gameObject.transform.position - new Vector3(0,0.50f,0), Vector2.left, attackRadius);
        if(rchit.collider != null){
            Debug.DrawRay(gameObject.transform.position - new Vector3(0,0.50f,0), Vector2.left * attackRadius, Color.green);
            booler = true;
        }

        if(rchit.collider == null){
            Debug.DrawRay(gameObject.transform.position - new Vector3(0,0.50f,0), Vector2.left * attackRadius, Color.red);
            booler = false;
        }

    }
}
