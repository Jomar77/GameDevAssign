using UnityEngine;
using System.Collections;

public enum PlayerState
{
    isZombie,
    isCiv
}

public class Character : MonoBehaviour
{
    public bool isClampingEnabled = true;

    public float smoothTime = 0.1f;
    private Vector2 currentVelocity = Vector2.zero;
    private Rigidbody2D rb;

    public GameObject zombie;
    public GameObject civ;
    public PlayerState state = PlayerState.isCiv;

    // Timer fields
    public float timeRemaining = 60f; // 1 minute timer
    private bool timerIsRunning = false;

    public float stateSwitchCooldown = 5.0f;
    private float lastStateSwitchTime;

    Vector2 targetVelocity;

    public int playerNumber { get; private set; }  // Player number
    public float remainingTime { get; private set; }

    // Initialization method to mimic constructor
    public void InitializeCharacter(int playerNum, float initialTime)
    {
        playerNumber = playerNum;
        remainingTime = initialTime;
        timerIsRunning = state == PlayerState.isZombie;
    }

    public float moveSpeed;
    private float turnSpeed;

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

        GameUIManager.Instance.UpdatePlayerInfo(playerNumber, remainingTime);

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
        //CODE FOR ROTATION AND SPEED
        if (this.state == PlayerState.isCiv)
        {
            moveSpeed = 1.6f;
            turnSpeed = 240f;
        }
        else
        {
            moveSpeed = 2.3f;
            turnSpeed = 75f;
        }

        if (Input.GetKey(leftKey))
        {
            transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(rightKey))
        {
            transform.Rotate(0, 0, -turnSpeed * Time.deltaTime);
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

        if (state == PlayerState.isZombie && !timerIsRunning)
        {
            timerIsRunning = true;
        }
        else if (state == PlayerState.isCiv && timerIsRunning)
        {
            timerIsRunning = false;
            Debug.Log($"Player {playerNumber} is a civilian. Timer paused at {Mathf.CeilToInt(timeRemaining)} seconds.");
        }
    }

    // GET SET GO
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

    //COLLISION THING
    void OnCollisionEnter2D(Collision2D other)
    {
        if (Time.time - lastStateSwitchTime < stateSwitchCooldown) return;
        Character otherCharacter;
        if (other.gameObject.GetComponent<Character>())
        {

            otherCharacter = other.gameObject.GetComponent<Character>();
        }
        else{
            return;
        }

        if (otherCharacter.state != this.state)
        {
            if (this.playerNumber > otherCharacter.playerNumber)
            {
                PlayerState tempState = this.state;
                this.state = otherCharacter.state;
                otherCharacter.state = tempState;

                this.UpdateState();
                otherCharacter.UpdateState();
                Debug.Log($"Character {this.playerNumber} switched to {this.state}, Character {otherCharacter.playerNumber} switched to {otherCharacter.state}");

                lastStateSwitchTime = Time.time;

                GameManager.Instance.StartZoom(3f, 0.1f, () =>
                {
                    // After zoom-in is complete, wait 3 seconds and zoom back out
                    GameManager.Instance.ZoomOutAfterDelay(5f, 2f, 0.1f);  // Adjust zoom-out size and duration
                });
            }

            Vector3 collisionPoint = (this.transform.position + otherCharacter.transform.position) / 2;
            GameManager.Instance.StartCollisionZoom(collisionPoint, 3f, 0.1f, 2f, () =>
            {
                // After zoom-in completes, wait 2 seconds, then zoom out
                GameManager.Instance.ZoomOutAfterDelay(5f, 2f, 0.1f);
            });

            GameManager.Instance.EnableVignette(.6f);

            // Activate slow-motion
            Time.timeScale = 0.5f;  // Slow down

            // Return to normal speed, disable vignette, and reset zoom
            StartCoroutine(ResetEffectsAfterDelay(2f));
        }
    }


    private IEnumerator ResetEffectsAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;  // Normal speed
        GameManager.Instance.DisableVignette();
    }



    //CAMERA STUFF
    void ClampToCameraBounds()
    {
        if (!isClampingEnabled) return;
        Camera cam = Camera.main;
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        CapsuleCollider2D collider = GetCC2d;

        float colliderHalfWidth = collider.bounds.extents.x;
        float colliderHalfHeight = collider.bounds.extents.y;

        float minX = cam.transform.position.x - cameraWidth / 2f + colliderHalfWidth;
        float maxX = cam.transform.position.x + cameraWidth / 2f - colliderHalfWidth;
        float minY = cam.transform.position.y - cameraHeight / 2f + colliderHalfHeight;
        float maxY = cam.transform.position.y + cameraHeight / 2f - colliderHalfHeight;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        transform.position = clampedPosition;
    }


    //TIMER STUFF

    void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            Debug.Log($"Player {playerNumber}: {timeRemaining}");

        }
        else
        {
            timeRemaining = 0;
            timerIsRunning = false;  // Timer stops when time reaches 0
            Debug.Log($"Player {playerNumber} has been zombified!");
            // Additional logic can be added here, such as changing state added something 
        }
    }

}