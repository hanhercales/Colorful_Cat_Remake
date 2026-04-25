using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [System.Serializable]
    public class ActiveEffect
    {
        public OvertimeEffect overtimeEffect;
        public float timer;
        public float tickTimer;
        public GameObject source;
        
        public void Reset()
        {
            overtimeEffect = null;
            timer = 0;
            tickTimer = 0;
            source = null;
        }
    }
    
    [SerializeField]
    private List<ActiveEffect> activeEffects =  new List<ActiveEffect>();
    
    private Stack<ActiveEffect> effectPool = new Stack<ActiveEffect>();

    public void AddEffect(Effect effect, GameObject source)
    {
        if (effect == null) return;

        if (effect is InstantEffect instantEffect)
        {
            instantEffect.OnApply(this.gameObject, source);
            return;
        }

        if (effect is OvertimeEffect overtimeEffect)
        {
            ActiveEffect existing =  activeEffects.Find(x => x.overtimeEffect == overtimeEffect);

            if (existing != null)
            {
                existing.timer = overtimeEffect.duration;
                existing.source = source;
                return;
            }
            
            ActiveEffect newEffect = GetFromPool();
            newEffect.source = source;
            newEffect.timer = overtimeEffect.duration;
            newEffect.tickTimer = overtimeEffect.tickInterval;
            newEffect.overtimeEffect = overtimeEffect;
            
            overtimeEffect.OnApply(this.gameObject, source);
            overtimeEffect.OnTick(this.gameObject);
            
            activeEffects.Add(newEffect);
        }
    }

    private void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var activeEffect = activeEffects[i];
            
            activeEffect.tickTimer -= Time.deltaTime;
            
            if(activeEffect.tickTimer <= 0)
            {
                activeEffect.overtimeEffect.OnTick(this.gameObject);
                activeEffect.tickTimer = activeEffect.overtimeEffect.tickInterval;
            }
            
            activeEffect.timer -= Time.deltaTime;

            if (activeEffect.timer <= 0)
            {
                activeEffect.overtimeEffect.OnRemove(this.gameObject,  activeEffect.source);
                activeEffects.RemoveAt(i);
                ReturnToPool(activeEffect);
            }
        }
    }

    private ActiveEffect GetFromPool()
    {
        if(effectPool.Count > 0) return effectPool.Pop();
        return new ActiveEffect();
    }

    private void ReturnToPool(ActiveEffect activeEffect)
    {
        activeEffect.Reset();
        effectPool.Push(activeEffect);
    }
}
