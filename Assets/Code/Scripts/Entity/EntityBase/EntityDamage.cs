using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityDamage : MonoBehaviour
{
    [SerializeField] protected string targetTag;
    [SerializeField] protected FormSkill formSkill;
    [SerializeField] protected GameObject source;
    [SerializeField] protected bool isProjectile;
    
    protected float damage;
    
    private List<GameObject> alreadyHitTargets = new List<GameObject>();

    public void Initialize(GameObject attacker, FormSkill skill)
    {
        source = attacker;
        formSkill = skill;
        
        if (source != null && source.TryGetComponent(out Stats stats))
        {
            damage = stats.GetDamage();
        }
    }
    
    private void OnEnable()
    {
        alreadyHitTargets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (formSkill != null && formSkill.canDestroyProjectile && target.CompareTag(targetTag + "Projectiles"))
        {
            Destroy(target.gameObject);
            if(isProjectile)  Destroy(gameObject);
            return;
        }
        
        if (target.CompareTag(targetTag))
        {
            if (target.TryGetComponent(out Stats targetStats))
            {
                targetStats.TakeDamage(damage);
            }
            
            if (target.TryGetComponent(out EffectHandler targetHandler))
            {
                foreach (Effect effect in formSkill.effectsToApply)
                {
                    targetHandler.AddEffect(effect, source);
                }
            }

            if (isProjectile) Destroy(gameObject);
        }
    }
}
