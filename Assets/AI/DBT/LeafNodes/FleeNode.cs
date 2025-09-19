using UnityEngine;
using UnityEngine.AI;

public class FleeNode : BTNode
{
    private EnemyBlackboard bb;
    private NavMeshAgent agent;
    private Animator anim;
    private float minTime, maxTime;

    private bool  fleeingStarted = false;
    private float timer          = 0f;
    private float fleeDuration   = 0f;

    /// <param name="minFleeSeconds">Minimum seconds to flee</param>
    /// <param name="maxFleeSeconds">Maximum seconds to flee</param>
    public FleeNode(
        EnemyBlackboard blackboard,
        NavMeshAgent agent,
        Animator animator,
        float minFleeSeconds = 5f,
        float maxFleeSeconds = 10f
    )
    {
        bb       = blackboard;
        this.agent = agent;
        anim     = animator;
        minTime  = minFleeSeconds;
        maxTime  = maxFleeSeconds;
    }

    public override NodeState Tick()
    {
        // 1) Start the flee on first Tick
        if (!fleeingStarted)
        {
            fleeingStarted = true;
            timer = 0f;
            // pick a random flee duration
            fleeDuration = Random.Range(minTime, maxTime);
            Debug.Log($"[FleeNode] START: setting Run=true (health={bb.healthPercent:F2})");
            // turn on run animation
            anim.SetBool("Run", true);
        }

        // 2) Move away from the player each frame
        Vector3 away = (agent.transform.position - bb.playerPosition).normalized;
        agent.SetDestination(agent.transform.position + away * 10f);

        // 3) Advance time & check for completion
        timer += Time.deltaTime;
        if (timer >= fleeDuration)
        {   
            Debug.Log("[FleeNode] END: setting Run=false");
            // Stop running animation
            anim.SetBool("Run", false);
            // Mark as done so we don't flee again
            bb.hasFled = true;
            return NodeState.Success;
        }

        // Still fleeing
        return NodeState.Running;
    }

    public override void Abort()
    {
        // If somehow interrupted, stop run
        anim.SetBool("Run", false);
        if (agent.hasPath)
            agent.ResetPath();
        // We *do not* reset bb.hasFled here — once you flee, you stay “hasFled.”
    }
}
