using UnityEngine;

[CreateAssetMenu(fileName = "NewSummonSkill", menuName = "Ability/SummonSkill")]
public class SummonSkill : FormSkill
{
    public GameObject summonPrefab;
    public float lifetime = 5f;

    public override void Activate(GameObject source, GameObject target)
    {
        if (summonPrefab == null) return;
        
        Vector2 spawnPos = target != null ? target.transform.position : source.transform.position;
        
        GameObject summonObj = Instantiate(summonPrefab, spawnPos, Quaternion.identity);
        
        if (summonObj.TryGetComponent(out BlackHole blackHole))
        {
            blackHole.Initialize(source, effectsToApply, lifetime);
        }
    }
}
