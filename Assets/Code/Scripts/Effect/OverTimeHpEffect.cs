using UnityEngine;

[CreateAssetMenu(fileName = "newOverTimeEffect", menuName = "Effect/Over Time Effect")]
public class OverTimeHpEffect : Effect
{
    public float hpPerTick;

    public override void OnTick(EffectManager manager)
    {
        var health = manager.gameObject.GetComponent<Stats>();
        if (health != null)
        {
            if(effectType == EffectType.OverTimeHeal)
                health.HpChange(hpPerTick);
            else if(effectType == EffectType.Burn || effectType == EffectType.Poison)
                health.TakeDamage(hpPerTick);
        }
    }
}
