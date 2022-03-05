using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    //ссылка на наш кастомный NetworkManager
    [SerializeField] private NetworkLobbyManager networkManager = null;

    //ссылки на UI обьекты, ничего интерессного
    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        NetworkLobbyManager.OnClientConnected += HandleClientConnected;
        NetworkLobbyManager.OnClientDisconected += HandleClientDisconnected;
    }
    private void OnDisable()
    {
        NetworkLobbyManager.OnClientConnected -= HandleClientConnected;
        NetworkLobbyManager.OnClientDisconected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAdress = ipAddressInputField.text;
        networkManager.networkAddress = ipAdress;
        networkManager.StartClient();

        joinButton.interactable = false;

    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;
        this.gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }
    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;

    }

}
