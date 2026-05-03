using System.Collections.Generic;
using Code.Scripts.Enum;
using UnityEngine;

public class FormSO : ScriptableObject
{
    public string formName;
    public string description;
    public Sprite icon;
    public FormType formType;
    
    public FormSkill basicAttack;
    public FormSkillType skillType;
    public FormSkill specialAttack;
    public ActiveAbility buffSkill;
    public FormSkill infusedBasicAttack;
}
