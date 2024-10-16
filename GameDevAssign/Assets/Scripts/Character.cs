using UnityEngine;

public enum PlayerState
{
    isZombie,
    isCiv
}

public class Character : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float smoothTime = 0.1f;
    private Vector2 currentVelocity = Vector2.zero;
    private Rigidbody2D rb;
    public int playerNumber;
    public GameObject zombie;
    public GameObject civ;
    public PlayerState state = PlayerState.isCiv;

    Vector2 targetVelocity;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {

        handleInput(playerNumber);

        UpdateState();
    }

    void handleInput(int playerNum)
    {

        switch (playerNum)
        {

            case 1:
                targetVelocity = Movement(KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D);
                break;
            case 2:
                targetVelocity = Movement(KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow);
                break;
            case 3: // Player 3 (IJKL)
                targetVelocity = Movement(KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L);
                break;
            case 4: // Player 4 (Numpad 8456)
                targetVelocity = Movement(KeyCode.Keypad8, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6);
                break;
        }

        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);

    }

    public Vector2 Movement(KeyCode upKey, KeyCode leftKey, KeyCode downKey, KeyCode rightKey)
    {
        Vector2 targetVelocity = Vector2.zero;


        if (Input.GetKey(rightKey))
        {
            targetVelocity = new Vector2(moveSpeed, rb.velocity.y);
            GetAnimator.SetBool("IsRunning", true);
            GetAnimator.SetBool("IsVertical", false);
            Debug.Log("hi");
        }
        else if (Input.GetKey(leftKey))
        {
            targetVelocity = new Vector2(-moveSpeed, rb.velocity.y);
            GetAnimator.SetBool("IsRunning", true);
            GetAnimator.SetBool("IsVertical", false);
        }
        else if (Input.GetKey(downKey))
        {
            targetVelocity = new Vector2(rb.velocity.x, -moveSpeed);
            GetAnimator.SetBool("IsRunning", true);
            GetAnimator.SetBool("IsVertical", true);
        }
        else if (Input.GetKey(upKey))
        {
            targetVelocity = new Vector2(rb.velocity.x, moveSpeed);
            GetAnimator.SetBool("IsRunning", true);
            GetAnimator.SetBool("IsVertical", true);
        }
        else
        {
            GetAnimator.SetBool("IsRunning", false);
        }

        return targetVelocity;
    }

    public void UpdateState()
    {
        zombie.SetActive(state == PlayerState.isZombie);
        civ.SetActive(state == PlayerState.isCiv);
    }

    public Animator GetAnimator
    {
        get
        {
            if (state == PlayerState.isCiv)
            {
                return civ.GetComponent<Animator>();
            }
            else
            {
                return zombie.GetComponent<Animator>();
            }
        }
    }



    public SpriteRenderer GetSpriteRenderer
    {
        get
        {
            if (state == PlayerState.isCiv)
            {
                return civ.GetComponent<SpriteRenderer>();
            }
            else
            {
                return zombie.GetComponent<SpriteRenderer>();
            }
        }
    }



    public void OnTriggerEnter(Collider other)
    {
        //Character thisCharacter = GetComponent<Character>();
        Character otherCharacter = other.GetComponent<Character>();

        if (otherCharacter != null)
        {
            if (otherCharacter.state == PlayerState.isZombie)
            {

                otherCharacter.state = PlayerState.isCiv;
                this.state = PlayerState.isZombie;
            }
        }
    }
}
