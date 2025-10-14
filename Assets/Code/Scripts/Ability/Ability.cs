using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Normal Ability")]
public class Ability : ScriptableObject
{
    public AbilityID abilityID;
    public string abilityName;
    public string abilityDescription;
}
