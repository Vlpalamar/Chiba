using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class FireStun : Spell
{
    public SpellType SpellType = SpellType.HeroTarget;

    [Header("Status")]
    public  bool Status =false;
    
    [Header("Stats")]
    [SerializeField] private float StanDuration=0f;
    [SerializeField] private float damage = 0f;
    [SerializeField] private float speed=0f;

    [SerializeField] private AttributeSpell attribute;
    [SerializeField] private Hero user = null;

    [SerializeField] private GameObject boltPrefab = null;
    private Hero Target;
    public override AttributeSpell Attribute
    {
        get { return attribute; }
        set { attribute = value; }
    }


    public override void UseSpell(Hero enemyTarget) //в поинт как правило приходит расположение курсора 
    {
        if (isLocalPlayer)
        {
            CmdUseSpell(enemyTarget);
        }
        else
        {
            Server_UseSpell(enemyTarget);
        }


    }

    [Server]
    private void Server_UseSpell(Hero enemyTarget)
    {
        RpcUseSpell(enemyTarget);
    }

    [Command]
    private void CmdUseSpell(Hero enemyTarget)
    {
        Server_UseSpell(enemyTarget);
    }

    [ClientRpc]
    private void RpcUseSpell(Hero enemyTarget)
    {
        user = this.gameObject.GetComponent<Hero>();
        Target = enemyTarget;
        var tmp = Instantiate(boltPrefab,user.SpellPossition.position,Quaternion.identity).GetComponent<Bolt>();
        tmp.Init(StanDuration,damage,speed,enemyTarget,user);
        
        
    }   



}
