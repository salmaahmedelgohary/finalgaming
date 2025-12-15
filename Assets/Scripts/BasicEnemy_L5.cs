using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple enemy that patrols and chases player
public class BasicEnemy_5 : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 3f;
    public float chaseSpeed = 5f;
    public float detectionRange = 8f;
    public float attackRange = 1.5f;
    public int damage = 1;
    
    [Header("Patrol Points")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 2f;
    
    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;
    
    private Transform player;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private bool isChasing = false;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    
    enum EnemyState { Patrol, Chase, Attack }
    private EnemyState currentState = EnemyState.Patrol;
    
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("Zero").transform;
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.gravityScale = 0; // Floating enemy
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // State machine
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                // Check if player is nearby
                if (distanceToPlayer < detectionRange)
                {
                    currentState = EnemyState.Chase;
                }
                break;
                
            case EnemyState.Chase:
                ChasePlayer();
                // Check if player is in attack range
                if (distanceToPlayer < attackRange)
                {
                    currentState = EnemyState.Attack;
                }
                // Check if player escaped
                else if (distanceToPlayer > detectionRange * 1.5f)
                {
                    currentState = EnemyState.Patrol;
                }
                break;
                
            case EnemyState.Attack:
                AttackPlayer();
                // Go back to chase if player moves away
                if (distanceToPlayer > attackRange)
                {
                    currentState = EnemyState.Chase;
                }
                break;
        }
        
        // Flip sprite based on movement direction
        if (rb != null && rb.velocity.x != 0)
        {
            sprite.flipX = rb.velocity.x < 0;
        }
    }
    
    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        
        // Move towards patrol point
        if (Vector2.Distance(transform.position, targetPoint.position) > 0.1f)
        {
            rb.velocity = direction * moveSpeed;
            waitTimer = 0f;
        }
        else
        {
            // Wait at point
            rb.velocity = Vector2.zero;
            waitTimer += Time.deltaTime;
            
            if (waitTimer >= waitTimeAtPoint)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                waitTimer = 0f;
            }
        }
    }
    
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;
    }
    
    void AttackPlayer()
    {
        rb.velocity = Vector2.zero;
        // Add attack logic here (damage player, animation, etc.)
        Debug.Log("🔥 Enemy attacking!");
    }
    
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        
        // Flash effect
        StartCoroutine(FlashRed());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    System.Collections.IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
    
    void Die()
    {
        Debug.Log("💀 Enemy defeated!");
        // Add death animation, effects, etc.
        Destroy(gameObject);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If player touches enemy, damage player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Call player damage method here
            Debug.Log("💥 Player hit by enemy!");
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualize detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Visualize attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}