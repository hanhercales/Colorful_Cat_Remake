using System;
using UnityEngine;

public class FormManager : MonoBehaviour
{
    public static FormManager Instance { get; private set; }
    
    [SerializeField] private FormItem currentForm;
    public FormItem CurrentForm => currentForm;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void ChangeForm(FormItem newForm)
    {
        currentForm = newForm;
        
        //UI event
    }
    
    public ActiveAbility GetCurrentActiveAbility()
    {
        if (currentForm != null)
        {
            return currentForm.ability;
        }
        return null;
    }
}
