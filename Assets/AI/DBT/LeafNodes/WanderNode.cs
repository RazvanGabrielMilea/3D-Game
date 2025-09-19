using UnityEngine;
using UnityEngine.AI;

public class WanderNode : BTNode
{
    private NavMeshAgent agent;
    private Animator    anim;
    private Vector3     origin;
    private float       radius;

    private Vector3     target;
    private bool        hasTarget    = false;
    private bool        isWalking    = false;
    private float       pauseTimer   = 0f;
    private float       minPause, maxPause;

    private float       rotateSpeed = 120f;

    /// <summary>
    /// wanderRadius: how far from origin to roam
    /// minIdle, maxIdle: how long to stay in idle between legs
    /// </summary>
    public WanderNode(NavMeshAgent agent, Animator anim, float wanderRadius, float minIdle, float maxIdle)
    {
        this.agent    = agent;
        this.anim     = anim;
        this.origin   = agent.transform.position;
        this.radius   = wanderRadius;
        this.minPause = minIdle;
        this.maxPause = maxIdle;
    }

    public override NodeState Tick()
    {
        // If still idling, count down
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return NodeState.Running;
        }

        // Pick a new target if needed
        if (!hasTarget)
        {
            // Randomize speed
            agent.speed = agent.speed * Random.Range(0.8f, 1.2f);

            Vector2 rnd = Random.insideUnitCircle * radius;
            target = origin + new Vector3(rnd.x, 0, rnd.y);
            agent.SetDestination(target);
            hasTarget = true;

            // Start walking
            anim.SetBool("Walk", true);
            isWalking = true;
        }

        // Smooth rotation
        Vector3 dir = (target - agent.transform.position).normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion desired = Quaternion.LookRotation(dir);
            agent.transform.rotation = Quaternion.RotateTowards(
                agent.transform.rotation,
                desired,
                rotateSpeed * Time.deltaTime
            );
        }

        // Arrival?
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Stop and set a new longer idle
            if (isWalking)
            {
                anim.SetBool("Walk", false);
                isWalking = false;
                pauseTimer = Random.Range(minPause, maxPause);
            }
            hasTarget = false;
        }

        return NodeState.Running;
    }

    public override void Abort()
    {
        if (isWalking)
        {
            anim.SetBool("Walk", false);
            isWalking = false;
        }
        if (agent.hasPath)
            agent.ResetPath();
        hasTarget  = false;
        pauseTimer = 0f;
    }
}
