using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance; // Agregamos la variable estática

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text ErrorMessage;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomItemPrefab;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerItemPrefab;
    [SerializeField] GameObject BotonStart;

    private void Awake()
    {
        // Establecemos la instancia Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Ya existe una instancia de Launcher. Se destruirá esta instancia.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("Conectando");

        PhotonNetwork.ConnectUsingSettings();

        MenuManager.Instance.OpenMenuName("Loading");
    }

    public void StartGame()
    {
        // el 1 es porque es el numero de build de nuestra escena de juego 
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Conectado al lobby");

        MenuManager.Instance.OpenMenuName("Home");

        PhotonNetwork.NickName = "player" + Random.Range

        (0, 1000).ToString("0000");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);

        MenuManager.Instance.OpenMenuName("Loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenuName("Room");
        roomName.text = PhotonNetwork.CurrentRoom.Name;

        foreach (Transform playerT in PlayerListContent)
        {
            Destroy(playerT.gameObject);
        }

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++) 
        {
            Instantiate(PlayerItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        BotonStart.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ErrorMessage.text = "Error al crear Sala" + message;

        MenuManager.Instance.OpenMenuName("Error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

        MenuManager.Instance.OpenMenuName("Loading");
    }

    public void JoinRoom(RoomInfo _info)
    {
        PhotonNetwork.JoinRoom(_info.Name);

        MenuManager.Instance.OpenMenuName("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenuName("Home");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transfo in roomListContent)
        {
            Destroy(transfo.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            { continue; }
            Instantiate(roomItemPrefab,
            roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerItemPrefab,
        PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        BotonStart.SetActive(PhotonNetwork.IsMasterClient);
    }

}
