using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class Spell : NetworkBehaviour
{
    public  enum SpellType
    {
        HeroTarget, //направленный на игрока 
        Target, //направленный в точку 
        Area, // направленный на область 
    
    }

    public virtual SpellType type { get; set; }
    public virtual AttributeSpell Attribute { get; set; }
    public virtual GameObject User { get; set; }

    public virtual void UseSpell(Vector3 _point)
    {
        Debug.Log("Activate ");

    }
    public virtual void UseSpell(Hero enemyTarget)
    {
        Debug.Log("Activate on Target ");

    }

}
