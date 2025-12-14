using UnityEngine;

public class PlayerController_L5 : MonoBehaviour
{
    // -----------------------------
    //  ENUM FOR GRAVITY DIRECTIONS
    // -----------------------------
    public enum GravityDirection { Down, Up, Left, Right }
    public GravityDirection currentGravity = GravityDirection.Down;

    // -----------------------------
    //  PUBLIC SETTINGS
    // -----------------------------
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpForce = 3f;
    public float gravityStrength = 30f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Death Settings")]
    public float deathY = -10f;
    public float deathCheckDelay = 1f;

    [Header("Gravity Switch Settings")]
    public KeyCode switchGravityKey = KeyCode.E; // Press E to switch!
    public bool canSwitchGravity = false; // Can player switch right now?

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spriteRenderer;
    float inputX;
    bool isGrounded;
    bool facingRight = true;
    float timeSinceGravityChange = 0f;

    // Current available gravity direction (set by GravityZone)
    GravityDirection availableGravityDirection = GravityDirection.Down;
    
    // Reference to DashMechanic to read dash key and dash speed
    DashMechanic_L5 dashMechanic;
    float baseMoveSpeed;

    // -----------------------------
    //  INITIALIZATION
    // -----------------------------
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        SetGravity(GravityDirection.Down);

        // Cache dash mechanic and original movement speed
        dashMechanic = GetComponent<DashMechanic_L5>();
        baseMoveSpeed = moveSpeed;
    }

    // -----------------------------
    //  UPDATE
    // -----------------------------
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        timeSinceGravityChange += Time.deltaTime;
        // If DashMechanic is present, holding its dash key temporarily sets movement speed
        KeyCode dashKeyToUse = dashMechanic != null ? dashMechanic.dashKey : KeyCode.LeftShift;
        if (Input.GetKey(dashKeyToUse) && dashMechanic != null)
        {
            moveSpeed = dashMechanic.dashSpeed;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
        }

        // --------- GRAVITY SWITCH (NEW!) ---------
        if (Input.GetKeyDown(switchGravityKey) && canSwitchGravity)
        {
            SwitchToAvailableGravity();
        }

        // DEBUG: Show current gravity state
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log($"🔍 CURRENT STATE: Gravity={currentGravity}, Can Switch={canSwitchGravity}, Available={availableGravityDirection}");
        }

        FlipSprite();

        // --------- JUMP ---------
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            Jump();

        //CheckDeath();

        // --------- ANIMATIONS ---------
        anim.SetFloat("Speed", Mathf.Abs(inputX));
        anim.SetBool("IsGrounded", IsGrounded());

        float verticalVel = Vector2.Dot(rb.velocity, transform.up);
        anim.SetFloat("VerticalVelocity", verticalVel);
    }

    // -----------------------------
    //  MOVEMENT + MANUAL GRAVITY
    // -----------------------------
    void FixedUpdate()
    {
        Vector2 gravityForce = -transform.up * gravityStrength;
        float perpVelocity = Vector2.Dot(rb.velocity, transform.up);

        if (!IsGrounded() || perpVelocity > 0.1f)
        {
            rb.AddForce(gravityForce);
        }
        else
        {
            rb.AddForce(gravityForce * 0.3f);
        }

        float parallelVelocity = inputX * moveSpeed;
        rb.velocity = (transform.right * parallelVelocity) + (transform.up * perpVelocity);

        if (IsGrounded() && Mathf.Abs(perpVelocity) < 2f)
        {
            rb.velocity = transform.right * parallelVelocity + transform.up * (perpVelocity * 0.3f);
        }
    }

    // -----------------------------
    //  SPRITE FLIPPING
    // -----------------------------
    void FlipSprite()
    {
        if (inputX != 0)
        {
            switch (currentGravity)
            {
                case GravityDirection.Down:
                case GravityDirection.Up:
                    if (inputX > 0 && !facingRight)
                    {
                        facingRight = true;
                        spriteRenderer.flipX = false;
                    }
                    else if (inputX < 0 && facingRight)
                    {
                        facingRight = false;
                        spriteRenderer.flipX = true;
                    }
                    break;

                case GravityDirection.Right:
                    if (inputX > 0)
                        spriteRenderer.flipX = false;
                    else
                        spriteRenderer.flipX = true;
                    break;

                case GravityDirection.Left:
                    if (inputX > 0)
                        spriteRenderer.flipX = false;
                    else
                        spriteRenderer.flipX = true;
                    break;
            }
        }
    }

    // -----------------------------
    //  GROUND CHECK
    // -----------------------------
    public bool IsGrounded()
    {
        Vector2 checkPos = groundCheck.position;
        Collider2D hit = Physics2D.OverlapCircle(checkPos, groundRadius, groundLayer);
        isGrounded = hit != null;
        return isGrounded;
    }

    // -----------------------------
    //  JUMP
    // -----------------------------
    void Jump()
    {
        Vector2 up2D = transform.up;
        Vector2 currentVel = rb.velocity;
        float perpVel = Vector2.Dot(currentVel, up2D);

        if (perpVel < 0)
        {
            rb.velocity = currentVel - perpVel * up2D;
        }

        rb.AddForce(up2D * jumpForce, ForceMode2D.Impulse);
    }

    // -----------------------------
    //  DEATH CHECK
    // -----------------------------
    //void CheckDeath()
    //{
    //    if (timeSinceGravityChange < deathCheckDelay)
    //        return;

    //    bool fellOff = false;

    //    switch (currentGravity)
    //    {
    //        case GravityDirection.Down:
    //            fellOff = transform.position.y < deathY;
    //            break;
    //        case GravityDirection.Up:
    //            fellOff = transform.position.y > -deathY;
    //            break;
    //        case GravityDirection.Left:
    //            fellOff = transform.position.x < deathY;
    //            break;
    //        case GravityDirection.Right:
    //            fellOff = transform.position.x > -deathY;
    //            break;
    //    }

    //    if (fellOff)
    //    {
    //        Die();
    //    }
    //}

    //void Die()
    //{
    //    Debug.Log("Player Died!");
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(
    //        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
    //    );
    //}

    // -----------------------------
    //  GRAVITY SWITCHING (NEW SYSTEM!)
    // -----------------------------

    // Called by GravityZone when player enters
    public void EnableGravitySwitch(GravityDirection newDirection)
    {
        canSwitchGravity = true;
        availableGravityDirection = newDirection;
        Debug.Log($"💡 Press [{switchGravityKey}] to switch to {newDirection} gravity!");

        // Trigger Nova's message here!
        NovaGuide_L5 nova = FindObjectOfType<NovaGuide_L5>();
        if (nova != null)
        {
            nova.ShowGravitySwitchMessage(newDirection, switchGravityKey);
        }
    }

    // Called by GravityZone when player exits
    public void DisableGravitySwitch()
    {
        canSwitchGravity = false;
        Debug.Log("🚫 Left gravity zone");
    }

    // Actually switch the gravity when player presses the key
    void SwitchToAvailableGravity()
    {
        SetGravity(availableGravityDirection);
        canSwitchGravity = false; // Disable after switching

        // Hide Nova's message
        NovaGuide_L5 nova = FindObjectOfType<NovaGuide_L5>();
        if (nova != null)
        {
            nova.HideMessage();
        }
    }

    // Internal method to change gravity
    void SetGravity(GravityDirection dir)
    {
        currentGravity = dir;
        timeSinceGravityChange = 0f;
        rb.velocity = Vector2.zero;

        switch (dir)
        {
            case GravityDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                Debug.Log("⬇️ Gravity DOWN - Player upright");
                break;
            case GravityDirection.Up:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                Debug.Log("⬆️ Gravity UP - Player upside down");
                break;
            case GravityDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                Debug.Log("⬅️ Gravity LEFT - Player rotated -90°");
                break;
            case GravityDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                Debug.Log("➡️ Gravity RIGHT - Player rotated 90°");
                break;
        }
    }
}