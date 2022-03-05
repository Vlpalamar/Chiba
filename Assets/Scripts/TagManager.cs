using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TagManager : NetworkBehaviour
{

    public const string TagEnemy="Enemy";
    public const string TagPlayer="Player";
    public const string TagProjectile="Projectile";
    // Start is called before the first frame update

#region  singelton
    public static TagManager instance;
    void Start()
    {
        if (instance == null)
            instance = this; 
        else if(instance == this)
            Destroy(gameObject); 
    }
#endregion

}
