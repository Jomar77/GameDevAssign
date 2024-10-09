using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1f; 
    public float smoothTime = 0.1f;
    private Vector2 currentVelocity = Vector2.zero; 

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
        Vector2 targetVelocity = Vector2.zero;

        if (Input.GetKey(KeyCode.D)) 
        {
            sr.flipX = false;
            targetVelocity = new Vector2(moveSpeed, rb.velocity.y);
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsVertical", false);
            wasVertical = false;
        }
        else if (Input.GetKey(KeyCode.A)) 
        {
            sr.flipX = true;
            targetVelocity = new Vector2(-moveSpeed, rb.velocity.y);
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsVertical", false);
            wasVertical = false;
        }
        else if (Input.GetKey(KeyCode.S)) 
        {
            targetVelocity = new Vector2(rb.velocity.x, -moveSpeed);
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsVertical", true);
            wasVertical = true;
        }
        else if (Input.GetKey(KeyCode.W)) 
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

        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
    }
}
