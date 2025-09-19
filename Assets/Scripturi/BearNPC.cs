using UnityEngine;

public class BearNPC : MonoBehaviour
{
    [Header("References")]
    public Animator animator; // Drag your Animator here

    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;
    public float walkTime = 3f;
    public float idleTime = 2f;

    private bool isWalking = false;
    private float timer;

    private void Start()
    {
        timer = idleTime; // Start by idling
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
                transform.Rotate(0f, Random.Range(-45f, 45f), 0f); // Small random rotation
            }
            else
            {
                timer = idleTime;
            }
        }
    }
}
