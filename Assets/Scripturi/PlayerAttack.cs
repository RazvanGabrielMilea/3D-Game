using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("How far away you can hit an enemy")]
    public float attackRadius = 2f;

    [Tooltip("Layers you consider “enemies”")]
    public LayerMask enemyLayer;

    [Tooltip("Amount of damage per click")]
    public int damagePerHit = 1;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // find all colliders in range on the enemyLayer
            Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
            foreach (var col in hits)
            {
                EnemyHealth eh = col.GetComponent<EnemyHealth>();
                if (eh != null)
                {
                    eh.TakeDamage(damagePerHit);
                }
            }
        }
    }

    // (Optional) draw the radius in the Scene view for tweaking:
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
