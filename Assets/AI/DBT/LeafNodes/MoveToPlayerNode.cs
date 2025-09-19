using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayerNode : BTNode
{
    private EnemyBlackboard bb;
    private NavMeshAgent agent;
    private Animator anim;
    private float stoppingDistance;
    //private bool wasWalking = false;

    public MoveToPlayerNode(EnemyBlackboard blackboard, NavMeshAgent navAgent, Animator animator, float stopDistance = 1.5f)
    {
        bb = blackboard;
        agent = navAgent;
        anim = animator;
        stoppingDistance = stopDistance;
        agent.stoppingDistance = stoppingDistance;
    }

   public override NodeState Tick()
{
    // Always walk when this node is active
    if (!anim.GetBool("Walk"))
        anim.SetBool("Walk", true);

    // Keep moving toward the last known player position
    agent.SetDestination(bb.playerPosition);

    // Always return Running so we don’t complete/abort
    return NodeState.Running;
}



   public override void Abort()
{
    anim.SetBool("Walk", false);
    if (agent.hasPath)
        agent.ResetPath();
}

}
