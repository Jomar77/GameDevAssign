using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        anim =
        GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        anim.SetFloat("Speed", rb.velocity.y);
        anim.SetBool("IsRunningHor", false);
        anim.SetBool("IsRunningVer", false);
        if
        (Input.GetKey(KeyCode.D))
        {
            sr.flipX = false;
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            anim.SetBool("IsRunningHor", true);
            anim.SetBool("IsRunningVer", false);
        }
        else if
        (Input.GetKey(KeyCode.A))
        {
            sr.flipX = true;
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            anim.SetBool("IsRunningHor", true);
            anim.SetBool("IsRunningVer", false);
        }
        else if
        (Input.GetKey(KeyCode.S))
        {
            sr.Equals("Assets/Sprites/New Piskel (2).png");
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.x);
            anim.SetBool("IsRunningHor", false);
            anim.SetBool("IsRunningVer", true);
        }
        else if
        (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.x);
            anim.SetBool("IsRunningVer", true);
            anim.SetBool("IsRunningHor", false);

        }
    }
}