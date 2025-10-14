using UnityEngine;

[CreateAssetMenu(fileName = "InstantHealEffect", menuName = "Effect/Instant Heal")]
public class InstantHealEffect : Effect
{
    public float healAmount;

    public override void OnApply(EffectManager manager)
    {
        var health = manager.gameObject.GetComponent<Stats>();
        if(health != null)
            health.HpChange(healAmount);
    }
}
