using UnityEngine;

public class CrowNPC : MonoBehaviour
{
    [Header("References")]
    public Animator animator; // Drag the crow's Animator here

    [Header("Movement Settings")]
    public float moveSpeed = 1.2f;
    public float walkTime = 2.5f;
    public float idleTime = 3f;

    private bool isWalking = false;
    private float timer;

    private void Start()
    {
        timer = idleTime;
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (isWalking)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        if (timer <= 0f)
        {
            isWalking = !isWalking;

            if (animator != null)
            {
                animator.SetBool("isWalking", isWalking);
            }

            if (isWalking)
            {
                timer = walkTime;
                transform.Rotate(0f, Random.Range(-40f, 40f), 0f); // Optional: face a new direction
            }
            else
            {
                timer = idleTime;
            }
        }
    }
}
