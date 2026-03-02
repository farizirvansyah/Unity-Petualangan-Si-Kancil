using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public VirtualJoystick joystick;
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    PlayerSFX playerSFX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerSFX = GetComponent<PlayerSFX>();
    }

    void Update()
    {
        // ===== KEYBOARD INPUT =====
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // ===== JOYSTICK INPUT =====
        float x = joystick.Direction.x;
        float y = joystick.Direction.y;

        // Only override if joystick is being used
        if (x != 0 || y != 0)
        {
            movement.x = x;
            movement.y = y;
        }

        bool isMoving = movement.sqrMagnitude > 0.01f;

        // ===== ANIMATOR =====
        animator.SetBool("isMoving", movement != Vector2.zero);

        // ===== SFX WALK =====
        if (isMoving)
            playerSFX.PlayWalk();
        else
            playerSFX.StopWalk();

        // ===== FLIP SPRITE =====
        if (movement.x != 0)
        {
            transform.GetChild(0).localScale = new Vector3(
                Mathf.Sign(movement.x),
                1,
                1
            );
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
