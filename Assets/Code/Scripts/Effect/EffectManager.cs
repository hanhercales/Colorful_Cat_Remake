using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private Dictionary<Effect, Coroutine> runningEffects = new Dictionary<Effect, Coroutine>();

    public void ApplyEffect(Effect effect)
    {
        if (runningEffects.ContainsKey(effect))
        {
            StopCoroutine(runningEffects[effect]);
            runningEffects.Remove(effect);
        }
        
        Coroutine effectCoroutine = StartCoroutine(EffectCoroutine(effect));
        runningEffects.Add(effect, effectCoroutine);
    }

    private IEnumerator EffectCoroutine(Effect effect)
    {
        effect.OnApply(this);
        
        float timer = effect.duration;
        float tickTimer = 1f;

        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;

            if (tickTimer <= 0)
            {
                effect.OnTick(this);
                tickTimer = 1f;
            }
        }
        
        effect.OnExpire(this);
        runningEffects.Remove(effect);
    }
}
