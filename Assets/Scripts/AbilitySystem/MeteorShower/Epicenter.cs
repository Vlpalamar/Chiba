using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Epicenter : NetworkBehaviour
{
    private List<Hero> Enemies;

    public bool Status = false;
    public GameObject meteorPrefab;
    private float damage; 
    [SerializeField] private float duration;
    private float radius;
    private  float  interval ;
    [SerializeField]  private float curDuration;
    private float curInterval;
    private int countPerWave;
    private Vector3 spawnPoint;
    private Hero user;

    public void Init(float damage, float duration, float radius, float interval,int countPerWave, Vector3 spawnPoint, Hero user)
    {
        this.damage= damage;
        this.radius = radius;
        this.countPerWave = countPerWave;
        this.duration= duration;
        this.spawnPoint = spawnPoint;
        this.interval = interval;
        Status = true;
        this.user = user;
    }

    private void Start()
    {
        transform.Rotate(new Vector3(90f,0f,0f));
        transform.localScale = new Vector3(radius, radius, transform.localScale.z);
    }

    private void FixedUpdate()
    {
        if (Status)
        {
            curInterval += Time.fixedDeltaTime;
            curDuration += Time.fixedDeltaTime;
        }
    }
    private void Update()
    {
        if (Status)
        {
            if (curInterval>=interval)
            {
                for (int i = 0; i < countPerWave; i++)
                {
                    var spawnPos = new Vector3(Random.Range(spawnPoint.x - radius, spawnPoint.x + radius + radius), 5f , Random.Range(spawnPoint.z - radius, spawnPoint.z + radius + radius));
                    var temp = Instantiate(meteorPrefab, spawnPos, Quaternion.identity).GetComponent<Meteor>();
                }
                
                curInterval = 0f;
                DealDmg();
                if (curDuration>=duration)
                {
                    Destroy(this.gameObject);
                }

               
            }
        }

    }



    private void DealDmg()
    {
        Collider[] hitColliders = new Collider[100]; // тут будет GameManager.Instance.Players.count;
        int numColliders = Physics.OverlapSphereNonAlloc(spawnPoint,radius, hitColliders);
        
        foreach (Collider col in hitColliders)
        {
            if (col==null) 
                continue;
            if (col.gameObject.GetComponent<Hero>())
            {
                print(col);
                if (col.gameObject.GetComponent<Hero>() != user)
                {
                    col.gameObject.GetComponent<Hero>().GetDamage(damage);
                }

            }
           
        }

        //if (isServer)
        //{

        //}
        //else
        //{

        //}
    }

    //[Server]
    //private void Server_DealDmg()
    //{


    //}

    //[Command]
    //private void CmdDealDmg()
    //{

    //}
 



    //написать метод который проверяет стоит ли кто то в круге и наносить урон стоящему 
}
