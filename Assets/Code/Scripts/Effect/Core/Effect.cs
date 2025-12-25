using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public string effectName;
    public Sprite effectSprite;
    public string description;
    
    public abstract bool OnApply(GameObject target, GameObject source);
    public abstract void OnRemove(GameObject target,  GameObject source);
}
