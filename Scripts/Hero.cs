using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;
using UnityEngine.Networking;
using Mirror;




public class Hero : NetworkBehaviour 
{
   


[SerializeField] private Projectile autoAtack;

[SerializeField]  private float  attackRange=10f;

[SerializeField]  private float  attackSpeed=2f;

[SerializeField] private float attackDeley=2f;



    private Transform startPosition;
    private float damage=5f;

    [SyncVar]
    [SerializeField]
    private Hero enemyTarget=null;

    private bool hasTarget= false;
    private bool isWalking= false;
    private Animator anim;
    private NavMeshAgent navAgent;

    // private  NetworkIdentity assignAuthorityObj; 
     
[SyncVar]
[SerializeField] private float speed;

[SyncVar]
[SerializeField] private float curentHealth=30;

[SyncVar]
[SerializeField] private float maxHealth=50;

[SerializeField] private Transform spellPossition= null;

public float Speed
{
    get{ return speed;}
    set{speed=value;}
}

public float CurentHealth
{
    get{ return curentHealth;}
    set{curentHealth=value;}
}

public float MaxHealth
{
    get{ return maxHealth;}
    set{maxHealth=value;}
}


public Transform SpellPossition
{
    get {return spellPossition;}
    
}




    public override void OnStartLocalPlayer()
    {
        this.tag=TagManager.TagPlayer;
       

    }
    // Start is called before the first frame update
    void Start()
    {
        
      
        Assert.IsNotNull(autoAtack);
        Assert.IsNotNull(spellPossition);
        navAgent=GetComponent<NavMeshAgent>();
        anim=GetComponent<Animator>();
        if(isServer)
        navAgent.speed= speed; 
        startPosition=this.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!isLocalPlayer)
         return;
        
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Input.GetButtonDown("Fire2"))
        {
            if(Physics.Raycast(ray, out hit, 100))
            {
                if(hit.collider.CompareTag(TagManager.TagEnemy))
                {
                    Hero enemy= hit.collider.GetComponent<Hero>();
                   CmdGetTarget(enemy);
                }
                else
                {
                    isWalking=true;
                    hasTarget=false;
                    navAgent.destination= hit.point;
                    navAgent.isStopped=false;
                }

            }
        }
        if(hasTarget)
        {
            MoveAndShoot();
        }
        if(navAgent.remainingDistance<=navAgent.stoppingDistance|| navAgent.remainingDistance<=attackRange && hasTarget)
        {
            isWalking=false;
        }
        else
        {
            isWalking=true;
        }
        

        anim.SetBool("isWalking", isWalking);
        
    }


[Command]
void CmdGetTarget(Hero enemy)
{
RpcGetTarget(enemy);
}

[ClientRpc]
void RpcGetTarget(Hero enemy)
{
 enemyTarget=enemy;
 hasTarget= true;
}
  void  MoveAndShoot()
    {
        
        if(!hasTarget || enemyTarget==null) return;
        navAgent.destination=enemyTarget.transform.position;
        if( navAgent.remainingDistance>=attackRange)
        {
            navAgent.isStopped=false;
            isWalking=true;
        }
        if(navAgent.remainingDistance<=attackRange)
        {
            transform.LookAt(enemyTarget.transform);
            if(Time.time>attackDeley)
            {
             
                attackDeley= Time.time+attackSpeed;
                
                CmdAttack();
                
            }
             navAgent.isStopped=true;
             isWalking=false;
        }

    }
    [Command]
    void CmdAttack()
    {
        RpcAttack();
        
    }

    [ClientRpc]
    void RpcAttack()
    {
    
        anim.SetTrigger("Attack");
        RpcFire();
    }


    [ClientRpc]
    void RpcFire()
    {
        
        Debug.Log("RpcAttack");
        Projectile proj = Instantiate(autoAtack,spellPossition.position, spellPossition.rotation)  as Projectile ;
        proj.EnemyTarget= enemyTarget;
        proj.Damage=damage;
    }


//через команду передает на сервер 

     void GetDamage(float damage)
    {
       if(!isServer) return;
        
        //other logic
        print($"dali pizdu na {damage} ediniz");
        if(curentHealth-damage<=0)
        {
           RpcDie();
          
        }
        else
        {
            CurentHealth-=damage;
        }
        RpcRecountHP( curentHealth);
    }


    [ClientRpc]
    void RpcRecountHP(float health)
    {
        curentHealth= health;
    }


    [ClientRpc]
    void RpcDie()
    {
       CurentHealth=MaxHealth;
        RpcSpawn();
    }
    [ClientRpc]
    void RpcSpawn()
    {if(isLocalPlayer)
        this.transform.position= startPosition.position;
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag(TagManager.TagProjectile)) // проверка что герой столкнулся с каким то скиллом/автоаттакой
        {
            
            
            Projectile pj = other.GetComponent<Projectile>(); // получение ссылки на то с чем столкнулся герой 

            //TODO: здесь будет свитч который будет мэнэджить какой тип у той хуйни которая прилетает в игрока 
            //ДЛЯ НАПРАВЛЕННЫХ СКИЛОВ В ОДНОГО ИГРОКА/АВТРОАТАКИ ПОДХОДИТ ЭТОТ БЛОК
            if(pj.EnemyTarget==this) // проверка что хуйня летит именно в нашего персонажа что бы не ловить лишнее 
            {
            print("hit"); //проверка что блок отрабатывает 
            GetDamage(pj.Damage); // получение урона 
            Destroy(other.gameObject); // уничтожить этот скилл так как он уже долетел 
            }
           //TODO: принимать урон от войд зонны

        }
    }
}
