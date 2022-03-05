using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Bolt : NetworkBehaviour
{
    [SerializeField] private float stanDuration=0;
    [SerializeField] private float damage =0;
    [SerializeField] private float speed=0;

    [SerializeField] private Hero user = null;
    [SerializeField] private Hero enemyTarget = null;

     public float StanDuration
     {
         get {return stanDuration ; }
         set { stanDuration = value; }
     }

     public float Damage
     {
         get { return damage; }
         set { damage = value; }
     }

     public float Speed
     {
        get { return speed; }
         set { speed = value; }
     }

     public Hero User
    {
         get { return user; }
         set { user = value; }
     }

     public Hero EnemyTarget
    {
         get { return enemyTarget; }
         set { enemyTarget = value; }
     }



    public void Init(float StunDuration, float damage, float speed, Hero enemyTarget, Hero user)
    {
        this.user = user;
        this.damage = damage;
        this.enemyTarget = enemyTarget;
        this.stanDuration = StunDuration;
        this.speed = speed;

    }

    // Update is called once per frame
    void Update()
    {
       
            
        
        this.transform.position = Vector3.MoveTowards(this.transform.position, enemyTarget.SpellPossition.position, speed);
        if (this.transform.position == enemyTarget.SpellPossition.position)
        {
            //
           
           
        }
    }


   
   
}
