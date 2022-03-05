using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        GameSpawnSystem.AddSpawnPoint(this.transform);
    }

    private void OnDestroy()
    {
        GameSpawnSystem.RemoveSpawnPoint(this.transform);
    }
}
