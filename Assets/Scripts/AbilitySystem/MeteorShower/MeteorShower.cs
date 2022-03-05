using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEditor;
using UnityEngine;

public class MeteorShower : Spell
{
    public SpellType spellType = SpellType.Area;

    [Header("Status")]
    public bool Status = false;

    [Header("Stats")]
    public float damage;
    public float radius;
    public float durration;
    public float interval;
    public int countPerWave;

    [SerializeField] private AttributeSpell attribute;
    [SerializeField] private Hero user = null;

    public override SpellType type
    {
        get { return spellType; }
        set { spellType = value; }
    }

    public override AttributeSpell Attribute
    {
        get {return  attribute;}
        set { attribute= value; }
    }


    public GameObject epicentrePrefab;
    private Vector3 pointCast;


    public override void UseSpell(Vector3 _point) //в поинт как правило приходит расположение курсора 
    {
        if (isLocalPlayer)
        {
            CmdUseSpell(_point);
        }
        else
        {
            Server_UseSpell(_point);
        }
       

    }

    [Server]
    private void Server_UseSpell(Vector3 _point)
    {
        RpcUseSpell(_point);
    }

    [Command]
    private void CmdUseSpell(Vector3 _point)
    {
        Server_UseSpell(_point);
    }

    [ClientRpc] 
    private void RpcUseSpell(Vector3 _point)
    {
        pointCast = new Vector3(_point.x, 0.5f, _point.z);  //+0.5 что бы обьект был не в тексурке а чуть выше 
        Status = true;
        if (Status)
        {
            var temp = Instantiate(epicentrePrefab, pointCast, Quaternion.identity).GetComponent<Epicenter>();
            user = this.gameObject.GetComponent<Hero>();
            temp.Init(damage, durration, radius, interval, countPerWave, pointCast,user);
            

            //NetworkServer.Spawn(temp.gameObject);
            Status = false;

        }

    }


    //public void Update()
    //{
    //    if (Status)
    //    {
    //       var temp = Instantiate(epicentrePrefab, pointCast, Quaternion.identity).GetComponent<Epicenter>();
    //       temp.Init(damage, durration, radius, interval, countPerWave, pointCast, user);
           
    //       //NetworkServer.Spawn(temp.gameObject);
    //        Status = false; 

    //    }
    //}



}
