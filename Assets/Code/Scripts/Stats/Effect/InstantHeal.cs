using System;
using UnityEngine;

public class InstantHeal : Effect
{
    EffectType effectType => EffectType.InstantHeal;

    private void OnEnable()
    {
        StartEffect();
    }

    protected override void StartEffect()
    {
        stats = GetComponent<Stats>();
        stats.currentHealth += effectValue;
        
        stats.CheckHealth();
    }
}
