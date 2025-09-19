using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Tooltip("How many hits this enemy can take before dying")]
    public int maxHealth = 2;

    // We make this private but expose it via a property
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Call this to damage the enemy by 1 “hit.”
    /// </summary>
    public void TakeDamage(int amount = 1)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        // TODO: play a death effect, sound, animation…
        Destroy(gameObject);
    }

    // ← Add this property so other scripts can read the current health
    public int CurrentHealth
    {
        get { return currentHealth; }
    }
}
