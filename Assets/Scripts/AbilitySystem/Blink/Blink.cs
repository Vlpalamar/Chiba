using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Blink : Spell
{
    public SpellType SpellType = SpellType.Target;

    [SerializeField]
    private  AttributeSpell attribute;
    [SerializeField]
    private GameObject user = null;

    public override GameObject User
    {
        get { return user;}
        set { user= value ; }
    }

    // эта хуйня передается в мэнэджэр  и от нее из атрибута будет братся иконка  имя скила, мана кост, кулдаун 
    public override AttributeSpell Attribute    {
        get { return attribute; }
        set { attribute = value; }

    }

    public override void UseSpell(Vector3 _point)
    {
        user.transform.position = _point;
       user.GetComponent<NavMeshAgent>().isStopped=true;
       user.GetComponent<Hero>().IsWalking = false;
    }
}
