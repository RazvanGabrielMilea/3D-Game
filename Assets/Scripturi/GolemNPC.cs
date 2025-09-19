using UnityEngine;

public class GolemNPC : MonoBehaviour
{
    public Animator animator;
    public float walkSpeed = 1f;
    public float movementRadius = 5f;

    private float timer = 0f;
    private int state = 0; // 0: idle, 1: walk, 2: punch

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        SetState(0);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (state == 1)
        {
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);

            // If we exceed the movement radius, go idle and rotate
            if (Vector3.Distance(startPosition, transform.position) > movementRadius)
            {
                SetState(0); // Idle
                transform.Rotate(0f, Random.Range(120f, 180f), 0f); // Turn around
                return;
            }
        }

        if (timer <= 0f)
        {
            state = (state + 1) % 3;
            SetState(state);
        }
    }

    void SetState(int newState)
    {
        state = newState;
        switch (state)
        {
            case 0: // Idle
                animator.SetBool("isWalking", false);
                timer = 2f;
                break;
            case 1: // Walk
                animator.SetBool("isWalking", true);
                timer = 2f;
                break;
            case 2: // Punch
                animator.SetBool("isWalking", false);
                animator.SetTrigger("Punch");
                timer = 2f;
                break;
        }
    }
}
