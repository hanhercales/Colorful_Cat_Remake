using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    
    private GameObject source;
    private Vector2 direction;
    private List<Effect> effectsToApply;
    [SerializeField] private bool destroyOnHit;
     float timer;
    [SerializeField] private float damage = 1f;

    public string targetTag = "Enemy";

    public void Initialize(GameObject source, Vector2 direction, List<Effect> effects)
    {
        this.source = source;
        this.direction = direction.normalized;
        this.effectsToApply = effects;
        
        timer = lifetime;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        
        timer -= Time.deltaTime;
        if(timer <= 0f)
            SimpleObjectPool.Instance.ReturnToPool(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            if (other.TryGetComponent(out EffectHandler handler))
            {
                if (effectsToApply != null)
                {
                    foreach (Effect effect in effectsToApply)
                        handler.AddEffect(effect, source);
                }
            }

            if (other.TryGetComponent(out Stats stats))
            {
                stats.TakeDamage(damage);
            }
            
            if(destroyOnHit) SimpleObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
}
