using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Inst;

    public void Awake()
    {
        if (Inst == null)
        { // Экземпляр менеджера был найден
            Inst = this; // Задаем ссылку на экземпляр объекта
        }
        else if (Inst == this)
        { // Экземпляр объекта уже существует на сцене
            Destroy(this.gameObject); // Удаляем объект
        }
    }
    // Start is called before the first frame update
   
    [SerializeField] public SyncList<Transform> heroes = new SyncList<Transform>();

    [Server]
    public void ServerAddToServer(Transform transform)
    {
        heroes.Add(transform);
      // RpcAddToServer(transform);
    }

    //[ClientRpc]
    //private void RpcAddToServer(GameObject hero)
    //{

    //    heroes.Add(hero);

    //}




    // Update is called once per frame
    void Update()
    {
        
    }
}
