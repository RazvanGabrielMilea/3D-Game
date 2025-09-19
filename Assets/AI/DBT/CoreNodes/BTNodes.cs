using UnityEngine;

// Every node returns one of these three states.
public enum NodeState
{
    Success,
    Failure,
    Running
}

// Abstract base class for all Behavior-Tree nodes.
public abstract class BTNode
{
    // Called each “tick” (e.g. once per frame or once per fixed update).
    public abstract NodeState Tick();

    // Optional cleanup hook if a running child is interrupted.
    public virtual void Abort() 
    { 
        // Default does nothing. Override in children if needed.
    }
}
