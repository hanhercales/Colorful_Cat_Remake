using System;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float pullForce = 15f;
    public float pullRadius = 5f;
    public string targetTag = "Enemy";

    private GameObject source;
    private List<Effect> effectsToApply;

    public void Initialize(GameObject source, List<Effect> effectsToApply, float lifetime)
    {
        this.source = source;
        this.effectsToApply = effectsToApply;
        
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pullRadius);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag(targetTag) && col.TryGetComponent(out Rigidbody2D rb))
            {
                float distance = Vector2.Distance(transform.position, col.transform.position);
                
                Vector2 direction = (transform.position - col.transform.position).normalized;
                
                float forceMultiplier = 1f + (1f - (distance / pullRadius));
                
                rb.AddForce(direction * pullForce * forceMultiplier, ForceMode2D.Force);
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag)) return;
        
        if (other.TryGetComponent(out EffectHandler effectHandler))
        {
            if (effectsToApply != null)
            {
                foreach (Effect effect in effectsToApply)
                {
                    effectHandler.AddEffect(effect, source);
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
