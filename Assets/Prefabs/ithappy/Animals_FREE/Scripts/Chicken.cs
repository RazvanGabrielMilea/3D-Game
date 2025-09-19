using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class ChickenWanderAI : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float wanderRadius = 5f;
    public float waitTime = 2f;
    public float obstacleDetectionDistance = 1.5f;
    public LayerMask obstacleLayers;

    private Vector3 origin;
    private Vector3 destination;
    private Animator animator;
    private CharacterController controller;
    private bool isWalking = false;

    void Start()
    {
        origin = transform.position;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        StartCoroutine(Wander());
    }

    void Update()
    {
        if (isWalking)
        {
            Vector3 dir = (destination - transform.position).normalized;

            // Check for obstacle in front
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, obstacleDetectionDistance, obstacleLayers))
            {
                StopAndWait();
                return;
            }

            controller.SimpleMove(dir * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);

            if (Vector3.Distance(transform.position, destination) < 0.5f)
            {
                StopAndWait();
            }
        }
    }

    IEnumerator Wander()
    {
        yield return new WaitForSeconds(waitTime);

        // Try finding a destination that doesn’t have an obstacle
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            Vector3 potentialDestination = new Vector3(origin.x + randomCircle.x, origin.y, origin.z + randomCircle.y);
            Vector3 dir = (potentialDestination - transform.position).normalized;

            if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, obstacleDetectionDistance, obstacleLayers))
            {
                destination = potentialDestination;
                destination.y = transform.position.y;
                isWalking = true;
                animator.SetFloat("Vert", 1);
                yield break;
            }
        }

        // No valid direction found, wait again
        StartCoroutine(Wander());
    }

    private void StopAndWait()
    {
        isWalking = false;
        animator.SetFloat("Vert", 0);
        StartCoroutine(Wander());
    }
}
