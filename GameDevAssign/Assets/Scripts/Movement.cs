using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1f;  // Maximum movement speed
    public float smoothTime = 0.1f; // Time for smooth damp
    private Vector2 currentVelocity = Vector2.zero; // Current velocity for SmoothDamp

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool wasVertical;

    public void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        // Target velocity starts at zero (stopped)
        Vector2 targetVelocity = Vector2.zero;

        // Detect movement input and set target velocity
        if (Input.GetKey(KeyCode.D)) // Move right
        {
            sr.flipX = false;
            targetVelocity = new Vector2(moveSpeed, rb.velocity.y);
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsVertical", false);
            wasVertical = false;
        }
        else if (Input.GetKey(KeyCode.A)) // Move left
        {
            sr.flipX = true;
            targetVelocity = new Vector2(-moveSpeed, rb.velocity.y);
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsVertical", false);
            wasVertical = false;
        }
        else if (Input.GetKey(KeyCode.S)) // Move down
        {
            targetVelocity = new Vector2(rb.velocity.x, -moveSpeed);
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsVertical", true);
            wasVertical = true;
        }
        else if (Input.GetKey(KeyCode.W)) // Move up
        {
            targetVelocity = new Vector2(rb.velocity.x, moveSpeed);
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsVertical", true);
            wasVertical = true;
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }

        // Smoothly damp towards the target velocity
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
    }
}
