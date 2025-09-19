using UnityEngine;

public class HedgehogNPC : MonoBehaviour
{
    [Header("References")]
    public Animator animator; // Drag the Animator with your controller

    [Header("Movement Speeds")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;

    [Header("Timing")]
    public float idleTime = 2f;
    public float walkTime = 2f;
    public float runTime = 2f;

    private enum HedgehogState { Idle, Walk, Run }
    private HedgehogState currentState = HedgehogState.Idle;

    private float timer;

    private void Start()
    {
        SetState(HedgehogState.Idle);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (currentState == HedgehogState.Walk)
        {
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        }
        else if (currentState == HedgehogState.Run)
        {
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
        }

        if (timer <= 0f)
        {
            AdvanceState();
        }
    }

    private void SetState(HedgehogState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case HedgehogState.Idle:
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                timer = idleTime;
                break;

            case HedgehogState.Walk:
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                timer = walkTime;
                transform.Rotate(0f, Random.Range(-30f, 30f), 0f); // Optional: turn
                break;

            case HedgehogState.Run:
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
                timer = runTime;
                transform.Rotate(0f, Random.Range(-15f, 15f), 0f); // Optional: smaller turn
                break;
        }
    }

    private void AdvanceState()
    {
        switch (currentState)
        {
            case HedgehogState.Idle:
                SetState(HedgehogState.Walk);
                break;

            case HedgehogState.Walk:
                SetState(HedgehogState.Run);
                break;

            case HedgehogState.Run:
                SetState(HedgehogState.Walk);
                break;
        }
    }
}
