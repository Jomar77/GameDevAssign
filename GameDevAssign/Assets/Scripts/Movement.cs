using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movement :
MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpHeight = 10f;
    public bool grounded;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource jumpSound;

    // Start is called before the

    void Start()
    {
        anim =
        GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        jumpSound = GetComponent<AudioSource>();
    }
    // Update is called once per

    void Update()
    {
        anim.SetFloat("Speed",rb.velocity.y);
        if
        (Input.GetKey(KeyCode.D))
        {
            sr.flipX = false;
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            anim.SetBool("Running", true);
        }
        if
        (Input.GetKey(KeyCode.A))
        {
            sr.flipX = true;
            rb.velocity = new Vector2(-moveSpeed,rb.velocity.y);
            anim.SetBool("Running", true);
        }
         if(Input.GetKey(KeyCode.W))
        {
            sr.flipY = true;
            rb.velocity = new Vector2(moveSpeed, rb.velocity.x);
            anim.SetBool("Running", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            sr.flipY = false;
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.x);
            anim.SetBool("Running", true);
        }

    }
}