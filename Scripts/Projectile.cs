using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Projectile : NetworkBehaviour
{

    [SyncVar]
    [SerializeField] private float damage;
    [SyncVar]
    [SerializeField] private float speed;
    // private float navigationTime=0;
    
    [SyncVar]
    [SerializeField]
    private Hero enemyTarget;
    public Hero EnemyTarget
    {
        get{return enemyTarget;}
        set{enemyTarget=value;}
    }
    public float Damage
    {
        get{return damage;}
        set {damage= value;}
    }
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
       this.transform.position= Vector3.MoveTowards(this.transform.position, enemyTarget.SpellPossition.position,speed);
        if(this.transform.position==enemyTarget.SpellPossition.position)
        { 
            
            // CmdHit();
        }
    }

    // [Command(requiresAuthority =false)]
    // void CmdHit()
    // {
    //    RpcHit();
            
    // }
    // [ClientRpc]
    // void RpcHit()
    // {
    //      this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
    //     enemyTarget.GetDamage(damage);
    //     Destroy(this.gameObject);

    // }

}
