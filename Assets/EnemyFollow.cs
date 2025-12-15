using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Ceiling Delay")]
    [SerializeField] private float flipDelay = 0.5f;   // seconds after Zero flips
    private float flipTimer = 0f;

    private Transform target;              // Zero
    private PlayerController playerCtrl;   // to read IsOnCeiling
    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool onCeiling = false;        // enemy’s own state

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
            playerCtrl = playerObj.GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        if (target == null || playerCtrl == null) return;

        // 1) Handle delayed ceiling/floor flip
        if (playerCtrl.IsOnCeiling != onCeiling)
        {
            // Zero changed side: start or continue counting
            flipTimer += Time.deltaTime;
            if (flipTimer >= flipDelay)
            {
                // After the delay, actually flip enemy
                if (playerCtrl.IsOnCeiling)
                    FlipToCeiling();
                else
                    FlipToFloor();

                flipTimer = 0f;
            }
        }
        else
        {
            // Same side as Zero, no need to count
            flipTimer = 0f;
        }

        // 2) Ground check
        bool isGroundAhead = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        float distance = Vector2.Distance(transform.position, target.position);

        if (isGroundAhead && distance > stopDistance)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

            if (direction.x > 0 && !facingRight)
                FlipHorizontal();
            else if (direction.x < 0 && facingRight)
                FlipHorizontal();
        }
        else if (!isGroundAhead)
        {
            FlipHorizontal();
        }
    }

    private void FlipToCeiling()
    {
        onCeiling = true;
        rb.gravityScale = -Mathf.Abs(rb.gravityScale);

        Vector3 scale = transform.localScale;
        scale.y = -Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    private void FlipToFloor()
    {
        onCeiling = false;
        rb.gravityScale = Mathf.Abs(rb.gravityScale);

        Vector3 scale = transform.localScale;
        scale.y = Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    private void FlipHorizontal()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        if (groundCheck != null)
        {
            Vector3 localPos = groundCheck.localPosition;
            localPos.x *= -1f;
            groundCheck.localPosition = localPos;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
