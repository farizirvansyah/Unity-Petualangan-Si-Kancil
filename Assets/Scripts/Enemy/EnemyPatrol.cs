using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 1f;

    private Transform target;
    private Animator animator;

    void Start()
    {
        target = pointA;
    }

    void Update()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("isMoving", true);
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            target = target == pointA ? pointB : pointA;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
