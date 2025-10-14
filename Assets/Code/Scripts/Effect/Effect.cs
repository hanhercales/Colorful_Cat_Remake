using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public enum EffectType
    {
        Heal,
        Drain,
        Burn,
        Stun,
        Poison,
        Slow,
        OverTimeHeal,
        ShadowSummon
    }
    
    public string effectName;
    public EffectType effectType;
    public float duration;
    public string effectDescription;
    
    public virtual void OnApply(EffectManager manager){}
    public virtual void OnTick(EffectManager manager){}
    public virtual void OnExpire(EffectManager manager){}
}
