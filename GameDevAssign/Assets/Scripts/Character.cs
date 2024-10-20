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

    // Timer fields
    public float timeRemaining = 60f; // 1 minute timer
    private bool timerIsRunning = false;

    Vector2 targetVelocity;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {



        handleInput(playerNumber);

        UpdateState();


        if (timerIsRunning)
        {
            UpdateTimer();
        }
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
            case 3:
                targetVelocity = Movement(KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L);
                break;
            case 4:
                targetVelocity = Movement(KeyCode.Keypad8, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6);
                break;
        }

        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);


        ClampToCameraBounds();

    }

    public Vector2 Movement(KeyCode upKey, KeyCode leftKey, KeyCode downKey, KeyCode rightKey)
    {
        Vector2 targetVelocity = Vector2.zero;


        if (Input.GetKey(leftKey))
        {
            transform.Rotate(0, 0, moveSpeed * 50 * Time.deltaTime);
        }
        else if (Input.GetKey(rightKey))
        {
            transform.Rotate(0, 0, -moveSpeed * 50 * Time.deltaTime);
        }
        if (Input.GetKey(downKey))
        {
            targetVelocity = -transform.up * moveSpeed;
            GetAnimator.SetBool("IsRunning", true);
        }
        else if (Input.GetKey(upKey))
        {
            GetAnimator.SetBool("IsRunning", true);

            targetVelocity = transform.up * moveSpeed;
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
        GetCC2d.enabled = true;

        Debug.Log("State updated: " + (state == PlayerState.isZombie ? "Zombie" : "Civilian"));
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


    public CapsuleCollider2D GetCC2d
    {
        get
        {
            if (state == PlayerState.isCiv)
            {
                return civ.GetComponent<CapsuleCollider2D>();
            }
            else
            {
                return zombie.GetComponent<CapsuleCollider2D>();
            }
        }
    }
    public float stateSwitchCooldown = 1.0f;
    private float lastStateSwitchTime;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (Time.time - lastStateSwitchTime < stateSwitchCooldown) return; // Prevent switching too fast

        Character otherCharacter = other.gameObject.GetComponent<Character>();

        if (otherCharacter != null)
        {
            if (this.state == PlayerState.isCiv && otherCharacter.state == PlayerState.isZombie)
            {
                this.state = PlayerState.isZombie;
                otherCharacter.state = PlayerState.isCiv;

                Debug.Log("Civilian turned into Zombie!");
            }
            else if (this.state == PlayerState.isZombie && otherCharacter.state == PlayerState.isCiv)
            {
                otherCharacter.state = PlayerState.isZombie;
                this.state = PlayerState.isCiv;

                Debug.Log("Zombie infected a civilian!");


            }



            this.UpdateState();
            otherCharacter.UpdateState();

            lastStateSwitchTime = Time.time; // Record the time of the last state switch
        }
    }




    void ClampToCameraBounds()
    {
        // Get the main camera's boundaries
        Camera cam = Camera.main;
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        // Calculate the boundaries
        float minX = cam.transform.position.x - cameraWidth / 2f;
        float maxX = cam.transform.position.x + cameraWidth / 2f;
        float minY = cam.transform.position.y - cameraHeight / 2f;
        float maxY = cam.transform.position.y + cameraHeight / 2f;

        // Clamp the player's position
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        // Apply the clamped position back to the player
        transform.position = clampedPosition;
    }

    void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
            // Handle timer end logic (e.g., reset or change state)
            Debug.Log($"Player {playerNumber} timer finished!");
            // You can change the player state here if needed
            // Example: state = PlayerState.isCiv; UpdateState(); 
        }
    }


}
