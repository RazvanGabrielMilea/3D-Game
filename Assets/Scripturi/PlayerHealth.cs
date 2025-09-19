using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Regeneration")]
    [Tooltip("Seconds after taking damage before regen starts")]
    public float regenDelay = 10f;
    [Tooltip("Seconds between each 1 HP regen tick")]
    public float regenInterval = 1f;

    [Header("Respawn Settings")]
    public Transform respawnPoint;
    public float respawnDelay = 1f;

    private CharacterController characterController;

    // ← Add this property so GameSaveManager can read health
    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    // ← Add this method so GameSaveManager can set health
    public void SetHealth(int newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        ResetHealth();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyDamage ed = collision.collider.GetComponent<EnemyDamage>();
            if (ed != null)
                TakeDamage(ed.damage);
        }
    }

    void TakeDamage(int amount)
    {
        // stop any pending or ongoing regen
        CancelInvoke(nameof(RegenTick));

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
            StartCoroutine(Respawn());
        else
            // schedule regen to start after regenDelay
            InvokeRepeating(nameof(RegenTick), regenDelay, regenInterval);
    }

    void RegenTick()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            healthSlider.value = currentHealth;
        }
        else
        {
            // reached full health: stop the regen invokes
            CancelInvoke(nameof(RegenTick));
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        // teleport logic
        characterController.enabled = false;
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        characterController.enabled = true;

        // full health on respawn (and cancel any regen invocations)
        CancelInvoke(nameof(RegenTick));
        ResetHealth();
    }

   private void ResetHealth()
{
    currentHealth = maxHealth;
    if (healthSlider != null)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value    = currentHealth;
    }
}
}
