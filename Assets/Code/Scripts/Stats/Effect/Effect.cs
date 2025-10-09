using UnityEngine;

public class Effect : MonoBehaviour
{
    public enum EffectType
    {
        InstantHeal,
        OverTimeHeal,
        Burn,
        Poison,
        Stun,
        Slow,
        Drain,
        Shadow
    }
    
    public EffectType effectType;
    
    protected Stats stats;
    
    [SerializeField] protected float duration;
    [SerializeField] protected int effectValue;

    protected virtual void StartEffect(){}
    
    protected virtual void EndEffect(){}

    public void SetDuration(float newDuration)
    {
        duration = newDuration;
    }
    
    public void SetEffectValue(int newValue)
    {
        effectValue = newValue;
    }
}
