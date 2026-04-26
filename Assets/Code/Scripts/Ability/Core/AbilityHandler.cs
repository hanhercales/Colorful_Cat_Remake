using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EntityDamage hitbox;

    public FormSkill rawBasicAttack;
    public FormSkill infusedBasicAttack;
    public FormSkill specialAttack;
    public ActiveAbility buffSkill;

    public float infusionDuration = 5;
    public bool isInfused { get; private set; }
    private float  infusionTimer = 0f;
    
    public List<PassiveAbility> equippedAbilities = new List<PassiveAbility>();
    
    private Dictionary<ActiveAbility, float> cooldownTimers = new Dictionary<ActiveAbility, float>();
    
    private FormSkill currentBasicAttack => isInfused ? infusedBasicAttack : rawBasicAttack;

    private void Start()
    {
        foreach (var passive in equippedAbilities)
        {
            if(passive != null) passive.OnEquip(this.gameObject);
        }
    }

    private void Update()
    {
        if (isInfused)
        {
            infusionTimer -= Time.deltaTime;
            if (infusionTimer <= 0f)
                isInfused = false;
        }
    }

    public bool CanUseBasicAttack() => currentBasicAttack != null && IsCooldownFinished(currentBasicAttack);
    public bool CanUseSpecial() => specialAttack != null && IsCooldownFinished(specialAttack);
    public bool CanUseBuff() => buffSkill != null && IsCooldownFinished(buffSkill);
    
    public void ExecuteBasicAttack()
    {
        if (hitbox != null)
        {
            hitbox.Initialize(this.gameObject, currentBasicAttack);
        }

        SetCooldown(currentBasicAttack);
    }

    public void ExecuteSpecialAttack()
    {
        if (hitbox != null)
        {
            hitbox.Initialize(this.gameObject, specialAttack);
        }

        specialAttack.Activate(this.gameObject, null);
        
        SetCooldown(specialAttack);
    }

    public void ExecuteBuffSkill()
    {
        isInfused = true;
        infusionTimer = infusionDuration;

        buffSkill.Activate(this.gameObject, null);

        SetCooldown(buffSkill);
    }
    
    private bool IsCooldownFinished(ActiveAbility ability)
    {
        if (ability == null) return false;
        if (!cooldownTimers.ContainsKey(ability)) return true;
        return Time.time >= cooldownTimers[ability];
    }

    private void SetCooldown(ActiveAbility ability)
    {
        cooldownTimers[ability] = Time.time + ability.cooldownTime;
    }

    private void TriggerAnimation(AnimationClip clip, string fallback)
    {
        if (animator == null) return;
        if (clip != null) animator.SetTrigger(clip.name);
        else animator.SetTrigger(fallback);
    }
}
