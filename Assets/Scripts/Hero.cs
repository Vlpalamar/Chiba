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


   [SerializeField] private Vector3 startPosition;
    private float damage=5f;

    [SyncVar]
    [SerializeField] private Hero enemyTarget=null;//сериализацию можно будет убрать, сейчас нужна что бы смотреть все ли правильно работает 

    private bool hasTarget= false;
    private bool isWalking= false;
    private Animator anim;
    private NavMeshAgent navAgent;

    // private  NetworkIdentity assignAuthorityObj; 

    public bool IsWalking
    {
        get { return isWalking; }
        set { isWalking = value; }
    }
     
[SyncVar]
[SerializeField] private float speed;

[SyncVar(hook = nameof(HandleCurrentHealthChanged))]
[SerializeField] private float curentHealth=30;


[SyncVar]
[SerializeField] private float maxHealth=50;

[SyncVar]
[SerializeField] private float stun = 0;

    [SerializeField] private Transform spellPossition= null;

[SerializeField] private string heroName = "";

[SerializeField] private string playerName = "";


public float Stun
{
    get { return Stun; }
    set { GetStun(value); }


}

public Hero EnemyTarget
{
    get { return enemyTarget; }
    set { enemyTarget = value; }
}

    public string PlayerName
{
    get { return playerName; }
    set { playerName = value; }
}

public string HeroName
{
    get { return heroName; }
    
}
public float Speed
{
    get{ return speed;}
    set{speed=value;}
}

public float CurentHealth
{
    get{ return curentHealth;}
    set{ Main_SetCurrentHealthValue(value); }
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
        Main_Spawn(startPosition);
        //GameManager.Inst.heroes.Add(this.gameObject);
        CmdAddToServer(this.transform);
        



    }



    // Start is called before the first frame update
    void Start()
    {
        
      
        Assert.IsNotNull(autoAtack);
        Assert.IsNotNull(spellPossition);
        navAgent=GetComponent<NavMeshAgent>();
        anim=GetComponent<Animator>();
        if (isServer)
        {
              navAgent.speed= speed;
        }
      
        startPosition = this.transform.position;

    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;
        if (stun > 0) // если есть стан
        {
            stun -= Time.fixedDeltaTime;
            if (stun <= 0) stopStunAnim();  //если стан закончился 
            return;
        }
    }

    private void stopStunAnim()
    {
        anim.SetBool("isStun", false);
    }
    // Update is called once per frame
    void Update()
    {
        
        if(!isLocalPlayer)
         return;

       
        if (stun>0)
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


    [Command(requiresAuthority = false)]
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
             isWalking = false;
             anim.SetBool("isWalking", isWalking);
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
        Debug.Log("RpcAttack");
        Projectile proj = Instantiate(autoAtack, spellPossition.position, spellPossition.rotation) as Projectile;
        proj.EnemyTarget = enemyTarget;
        proj.Damage = damage;
    }



    public void GetDamage(float damage)
    {
        //other logic
        print($"dali pizdu na {damage} ediniz");
        if (curentHealth - damage <= 0)
        {
              Main_Die();  
        }
        else
        {
            Server_SetCurrentHealthValue(curentHealth - damage);
        }



    }


    #region SetCurrentHealthValue
    private void Main_SetCurrentHealthValue(float newValue)
    {
        if (isServer)
        {
            Server_SetCurrentHealthValue(newValue);
        }
        else
        {
            CmdSetCurrentHealthValue(newValue);
        }
    }


    [Command(requiresAuthority = false)]
    private void CmdSetCurrentHealthValue(float newValue)
    {
        Server_SetCurrentHealthValue(newValue);
    }

    [Server]
    private void Server_SetCurrentHealthValue(float newValue)
    {
        RpcSetCurrentHealth(newValue);
    }


    #endregion

    //SetCurrentHealthValue Logic
    [ClientRpc]
    private void RpcSetCurrentHealth(float newValue)
    {
        curentHealth = newValue;
    }

    //if(!isServer) return;

    ////other logic
    //print($"dali pizdu na {damage} ediniz");
    //if(curentHealth-damage<=0)
    //{
    //  //  Main_Die();  later


    //}
    //else
    //{
    //    curentHealth -= damage;
    //}

    //}


    //--------------вызывать через Main_Die()

    //блок методов целькоторых правильно вызвать RpcDie
    #region Die_semiMethods
    void Main_Die()
    {
        if (isServer)
        {
            Server_Die();

        }
        else
        {
            CmdDie();
        }

    }
    [Command]
    void CmdDie()
    {
        Server_Die();

    }
    [Server]
    void Server_Die()
    {
        RpcDie();
    }


    #endregion

   //main Die Logic
    [ClientRpc]
    void RpcDie()
    {
        //other logic
        //anim die

        Main_Spawn(startPosition);
    }


    //--------------вызывать через Main_Spawn()
    //блок методов целькоторых правильно вызвать RpcSpawn
    #region Spawn_seniMethods

    public void Main_Spawn(Vector3 newPos)
    {
       
        if (isServer)
        {
            Server_Spawn(newPos);
        }
        else
        {
            CmdSpawn(newPos);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSpawn(Vector3 newPos)
    {
        Server_Spawn(newPos);
    }

    [Server]
    private void Server_Spawn(Vector3 newPos)
    {
        RpcSpawn(newPos);
    }


    #endregion
    [ClientRpc]
    void RpcSpawn(Vector3 newPos)
    {
        enemyTarget = null;
        CurentHealth = maxHealth;
        navAgent.isStopped = true;
       
       isWalking = false;
        stun = 0.1f;
        //otherLogic
        this.transform.position = newPos;
        navAgent.destination = newPos;
       anim.SetTrigger("Respawn");
        //respawn anim
       
    }



    [Command]
    private void CmdAddToServer(Transform transform)
    {
        GameManager.Inst.ServerAddToServer(transform);
    }

    void OnTriggerEnter(Collider other) 
    {
        print("hit0");
        if (other.CompareTag(TagManager.TagProjectile)) // проверка что герой столкнулся с каким то скиллом/автоаттакой
        {

            if (other.GetComponent<Projectile>())
            {
                Projectile pj = other.GetComponent<Projectile>(); // получение ссылки на то с чем столкнулся герой 
                if (pj.EnemyTarget == this) // проверка что хуйня летит именно в нашего персонажа что бы не ловить лишнее 
                {
                    print("hit"); //проверка что блок отрабатывает 
                    GetDamage(pj.Damage); // получение урона 
                    Destroy(other.gameObject); // уничтожить этот скилл так как он уже долетел 
                }
            }

            if (other.GetComponent<Bolt>())
            {
                print("hit1");
                Bolt bolt = other.GetComponent<Bolt>();
                if (bolt.EnemyTarget==this)
                {
                    print("hit2");
                    GetStun(bolt.StanDuration);
                    GetDamage(bolt.Damage);
                    Destroy(other.gameObject);

                }
            }
           
           //TODO: принимать урон от войд зонны

        }
    }

    public void GetStun(float dur)
    {
        if (isServer)
        {
            Server_SetStun(dur);
        }
        else
        {
            CmdSetStun(dur);
        }

    }

    #region SetStun
    [Server]
    public void Server_SetStun(float dur)
    {
        RpcSetStun(dur);
    }

    [Command]
    private void CmdSetStun(float dur)
    {
        Server_SetStun(dur);
    }

    [ClientRpc]
    private void RpcSetStun(float dur)
    {
        if (stun>dur)
        return;
        
        
        anim.SetBool("isStun", true);
        stun = dur;
    }

    #endregion







    public void HandleCurrentHealthChanged(float oldHealth, float newValue)
    {
       
    }
}
