using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;


//наш кастомный NetworkManager
public class NetworkLobbyManager : NetworkManager
{
    //ссылка на нужную ссцену
    [Scene]
    [SerializeField] private  string menuScene= string.Empty;

    //сылка на коммнату 
    [Header("Room")]
    [SerializeField] private  NetworkLobbyRoom roomPlayerPrefab= null;

    public static event Action OnClientConnected;// ивент, что должно сработать когда кто то подключаеться 
    public static event Action OnClientDisconected;//ивент, что должно сработать когда кто то отключаеться  

    private const string HeroPregabsFolderName = "HeroPrefabs"; // название папки с префабами героев
    
    //срабатывает когда загружается хост
    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>(HeroPregabsFolderName).ToList();

    //срабатывает когда загружается клиент
    public override void OnStartClient()
    {

        var spawnablePrefabs = Resources.LoadAll<GameObject>(HeroPregabsFolderName);
        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    //вызывает
    public override void OnClientConnect(NetworkConnection conn)
    {
        
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers>=maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().name!=menuScene)
        {
            conn.Disconnect();
            return;
        }

    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().name==menuScene)
        {
            NetworkLobbyRoom lobbyRoomInstance = Instantiate(roomPlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, lobbyRoomInstance.gameObject);
        }
    }


}
