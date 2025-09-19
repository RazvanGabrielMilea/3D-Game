using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyBlackboard))]
[RequireComponent(typeof(Animator))]
public class EnemyAIController : MonoBehaviour
{
    private EnemyBlackboard bb;
    private NavMeshAgent agent;
    private Animator anim;
    private BTNode root;

    [Header("Ranges")]
    public float seeRange = 8f;
    public float punchRange = 2f;
    public float lowHealthThreshold = 0.5f; // 1 HP of 2 => 0.5

    [Header("Wander Settings")]
    public float wanderRadius = 5f;
    public float wanderMinIdle = 2f;
    public float wanderMaxIdle = 5f;

    [Header("Flee Settings")]
    public float fleeMinTime = 5f;
    public float fleeMaxTime = 10f;

    void Start()
    {
        // Cache components
        bb = GetComponent<EnemyBlackboard>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // --- Build each subtree ---

        // 1) Flee: only when health is low AND not yet fled; runs 5–10s then sets bb.hasFled
        var fleeSeq = new SequenceNode();
        fleeSeq.AddChild(new HealthBelowNode(bb, lowHealthThreshold));
        fleeSeq.AddChild(new FleeNode(bb, agent, anim, fleeMinTime, fleeMaxTime));

        // 2) Punch: only when healthy AND within punchRange, one-shot per entry
        var punchSeq = new SequenceNode();
        punchSeq.AddChild(new HealthAboveNode(bb, lowHealthThreshold));
        punchSeq.AddChild(new CheckPlayerInRangeNode(bb, punchRange));
        punchSeq.AddChild(new PunchNode(bb, anim, punchRange));

        // 3) Chase: when within seeRange (distance‐only), continuous Running
        var chaseSeq = new SequenceNode();
        chaseSeq.AddChild(new CheckPlayerDistanceNode(bb, seeRange));
        chaseSeq.AddChild(new MoveToPlayerNode(bb, agent, anim, stopDistance: 1.5f));

        // 4) Wander: default roam around spawn point with idle pauses
        var wander = new WanderNode(agent, anim, wanderRadius, wanderMinIdle, wanderMaxIdle);

        // --- Assemble Dynamic Selector ---

        var dyn = new DynamicSelectorNode();
        dyn.AddChild(fleeSeq);
        dyn.AddChild(punchSeq);
        dyn.AddChild(chaseSeq);
        dyn.AddChild(wander);

        dyn.weightFunctions = new List<Func<float>>()
        {
            // Flee if low health AND not yet fled
            () => (bb.healthPercent <= lowHealthThreshold && !bb.hasFled) ? 1f : 0f,

            // Punch if in punch range
            () => bb.playerDistance <= punchRange ? 0.9f : 0f,

            // Chase if in see range
            () => bb.playerDistance <= seeRange ? 0.5f : 0f,

             // Otherwise wander
            () => 0.1f
        };

        root = dyn;
    }

    //void Update()
    //{
        // Simply tick the tree; each node handles its own animations
    //    root.Tick();
    //}
    
    void LateUpdate()
{
    root.Tick();
}
}
