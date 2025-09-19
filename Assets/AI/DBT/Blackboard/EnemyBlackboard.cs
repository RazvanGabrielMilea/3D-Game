using UnityEngine;

public class EnemyBlackboard : MonoBehaviour
{
    [HideInInspector] public Vector3 playerPosition;
    [HideInInspector] public bool seesPlayer;
    [HideInInspector] public float playerDistance;
    [HideInInspector] public bool isHitAndRun;
    [HideInInspector] public bool hasFled = false;
    [HideInInspector] public bool isAttackingGroup;
    [HideInInspector] public float healthPercent= 1f;

    // Example timers/flags (you’ll hook these to your PlayerController events).
    private float hitAndRunTimer = 0f;
    private float groupAttackTimer = 0f;

    void Update()
    {
        // 1) Find the player (assumes Player has tag “Player”)
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerPosition = playerObj.transform.position;
            playerDistance = Vector3.Distance(transform.position, playerPosition);

            // 2) Line-of-sight raycast:
            Vector3 origin = transform.position + Vector3.up * 1.2f;
            Vector3 dir = (playerPosition - origin).normalized;
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, 100f))
            {
                seesPlayer = hit.collider.CompareTag("Player");
            }
            else
            {
                seesPlayer = false;
            }
        }

        // 3) Health percentage (using your EnemyHealth component)
        var healthComp = GetComponent<EnemyHealth>();
        if (healthComp != null && healthComp.maxHealth > 0)
        {
            healthPercent = (float)healthComp.CurrentHealth / healthComp.maxHealth;
        }
        else
        {
            healthPercent = 1f; // fallback if no health component found
        }

        // 4) Timed flags (example: isHitAndRun is true for 2 seconds after set)
        if (hitAndRunTimer > 0f)
        {
            hitAndRunTimer -= Time.deltaTime;
            if (hitAndRunTimer <= 0f)
                isHitAndRun = false;
        }

        if (groupAttackTimer > 0f)
        {
            groupAttackTimer -= Time.deltaTime;
            if (groupAttackTimer <= 0f)
                isAttackingGroup = false;
        }

    }

    // Public methods to set flags (called externally when player does certain actions)
    public void TriggerHitAndRun(float duration = 2f)
    {
        isHitAndRun = true;
        hitAndRunTimer = duration;
    }

    public void TriggerGroupAttack(float duration = 2f)
    {
        isAttackingGroup = true;
        groupAttackTimer = duration;
    }
}
