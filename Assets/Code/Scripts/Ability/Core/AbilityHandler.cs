using System;
using System.Collections.Generic;
using Code.Scripts.Enum;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EntityDamage hitbox;

    public FormSO currentForm { get; private set; }
    public FormSkill rawBasicAttack;
    public FormSkill infusedBasicAttack;
    public FormSkillType currentSkillType;
    public FormSkill specialAttack;
    public ActiveAbility buffSkill;

    public float infusionDuration = 5f;
    public bool isInfused { get; private set; }
    private float  infusionTimer = 0f;
    
    //test
    public FormSO testForm;
    
    public List<PassiveAbility> equippedAbilities = new List<PassiveAbility>();
    
    private Dictionary<ActiveAbility, float> cooldownTimers = new Dictionary<ActiveAbility, float>();
    
    private FormSkill currentBasicAttack => isInfused ? infusedBasicAttack : rawBasicAttack;

    private void Start()
    {
        EquipForm(testForm);
        
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

    public void EquipForm(FormSO form)
    {
        if (form == null) return;

        currentForm = form;

        rawBasicAttack = form.basicAttack;
        infusedBasicAttack = form.infusedBasicAttack;
        currentSkillType = form.skillType;
        specialAttack = form.specialAttack;
        buffSkill = form.buffSkill;
        
        isInfused = false;
    }

    public bool CanUseBasicAttack() => currentBasicAttack != null && IsCooldownFinished(currentBasicAttack);

    public bool CanUseActiveSkill()
    {
        if (currentSkillType == FormSkillType.SpecialAttack)
            return specialAttack != null && IsCooldownFinished(specialAttack);
        return buffSkill != null && IsCooldownFinished(buffSkill);
    }
    
    public void ExecuteBasicAttack()
    {
        if (hitbox != null)
        {
            hitbox.Initialize(this.gameObject, currentBasicAttack);
        }

        SetCooldown(currentBasicAttack);
    }

    public void ExecuteActiveSkill()
    {
        if (currentSkillType == FormSkillType.SpecialAttack)
        {
            if (hitbox != null && !specialAttack.isRangedAttack)
            {
                hitbox.Initialize(this.gameObject, specialAttack);
            }
            specialAttack.Activate(this.gameObject, null);
            SetCooldown(specialAttack);
        }

        if (currentSkillType == FormSkillType.BuffInfusion)
        {
            isInfused = true;
            
            infusionTimer = infusionDuration;
            
            buffSkill.Activate(this.gameObject, null);
            SetCooldown(buffSkill);
        }
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
}
