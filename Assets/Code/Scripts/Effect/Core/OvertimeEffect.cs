using UnityEngine;

public abstract class OvertimeEffect : Effect
{
    public float duration;
    public float tickInterval = 1f;
    
    public abstract void OnTick(GameObject target);
}
