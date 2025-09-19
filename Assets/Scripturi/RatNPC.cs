using UnityEngine;

public class RatNPC : MonoBehaviour
{
    public Animator animator;

    private float timer;
    private bool isTalking = false;

    void Start()
    {
        SetState(false); // Start with idle
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isTalking = !isTalking;
            SetState(isTalking);
        }
    }

    void SetState(bool talking)
    {
        isTalking = talking;
        animator.SetBool("isTalking", isTalking);

        if (isTalking)
            timer = 10f;
        else
            timer = 3f;
    }
}
