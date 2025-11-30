using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using ExitGames.Client.Photon;
public class LobbyManager : MonoBehaviourPunCallbacks
{

    public TMP_InputField createRoomIF;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemList = new List<RoomItem>();
    public Transform contentObj;
    public float timeBetweenUps = 1.8f;
    float nextUpTime = 0;
    List<PlayerImage> playerList = new List<PlayerImage>();
    public PlayerImage playerImagePrefab;
    public Transform playerImageParent;

    public GameObject playButton;

    public void Awake()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        if (createRoomIF.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(createRoomIF.text, new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true });

        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
        playerImagePrefab.roomSize = playerList.Count;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpTime)
        {
            UpdateRoomList(roomList);
            nextUpTime = Time.time + timeBetweenUps;
        }

    }

    public void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemList)
        {
            Destroy(item.gameObject);
        }
        roomItemList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObj);
            newRoom.SetRoomName(room.Name);
            roomItemList.Add(newRoom);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void UpdatePlayerList()
    {
        foreach (PlayerImage player in playerList)
        {
            Destroy(player.gameObject);
        }
        playerList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        bool last = false;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerImage newPlayer = Instantiate(playerImagePrefab, playerImageParent);
            newPlayer.SetPlayerInfo(player.Value, last);
            playerList.Add(newPlayer);
            last = !last;
        }
    }



    public void JoinRoom(string name)
    {
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            Debug.Log("joined room");
            PhotonNetwork.JoinRoom(name);
        }
        else
        {
            Debug.Log("Not ready to join room. Current state: " + PhotonNetwork.NetworkClientState);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        //roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
    }

    public void OnClickStartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("team"))
        {
            string team = (string)changedProps["team"];

            // O oyuncunun UI prefabını bul
            foreach (PlayerImage item in playerList)
            {
                if (Equals(item.GetPlayer(), targetPlayer))
                {
                    if (team == "red")
                    {
                        item.UpdatePlayerUI(false);
                    }
                    else
                    {
                        item.UpdatePlayerUI(true);
                    }

                }
            }
            //UpdatePlayerList();
        }
    }
    
    
}
