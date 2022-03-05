using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attribute", menuName = "Spell/Attribute")]
public class AttributeSpell : ScriptableObject
{
    [Header("Description")]
    public string Name;
    public Sprite Icone;

    [Header("Attribute")]
    public  float CoolDown;
    public  float ManaCoast;
}
