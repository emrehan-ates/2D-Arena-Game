using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviourPunCallbacks
{
    public Transform redSpawn;
    public Transform blueSpawn;
    //public SpriteRenderer teamIdentifier;
    public GameObject playerPrefab;
    public GameObject bluePrefab;
    GameObject pla;
    TeamIdentifier ti;
    public Canvas gameOverCanvas;
    public List<PlayerController> players = new List<PlayerController>();
    TMP_Text username;
    
    void Awake()
    {

        string team = "";
        //team = false;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("team"))
        {
            team = (string)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        }
        
        if (team == "red")
        {
            SpawnPlayer(false);
        }
        else if (team == "blue")
        {
            SpawnPlayer(true);
        }
        else
        {
            UnityEngine.Debug.Log("Yarrayedik");
        }
        
        gameOverCanvas.gameObject.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        foreach (PlayerController player in players)
        {
            if (player.isGameOver)
            {
                photonView.RPC("ShowGameOver", RpcTarget.All, player.team == "red" ? "blue": "red");
            }
        }
    }
    
    [PunRPC]
    public void ShowGameOver(string winner)
    {
        TMP_Text[] winText = gameOverCanvas.GetComponentsInChildren<TMP_Text>();
        Image[] winPanel = gameOverCanvas.GetComponentsInChildren<Image>();

        foreach (TMP_Text text in winText)
        {
            if (text.CompareTag("WinText"))
                text.text = winner + " Wins";
        }

        foreach (Image i in winPanel)
        {
            if (i.CompareTag("WinText"))
                i.color = (winner == "red") ? Color.red : Color.blue;
        }

        gameOverCanvas.gameObject.SetActive(true);
    }

    public void SpawnPlayer(bool team)
    {

        GameObject p;

        PlayerController player;
        if (team)
        {
            p = PhotonNetwork.Instantiate(bluePrefab.name, blueSpawn.position, Quaternion.identity);
            player = p.GetComponent<PlayerController>();
            player.team = "blue";
        }
        else
        {
            p = PhotonNetwork.Instantiate(playerPrefab.name, redSpawn.position, Quaternion.identity);
            player = p.GetComponent<PlayerController>();
            player.team = "red";
        }
        
        username = p.GetComponentInChildren<TMP_Text>();
        
        players.Add(player);
        photonView.RPC("SetName", RpcTarget.AllBuffered, PhotonNetwork.NickName, player.photonView.ViewID);
        //user.text = PhotonNetwork.NickName;
    }
    
    [PunRPC]
    public void SetName(string nick, int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null)
        {
            TMP_Text user = pv.GetComponentInChildren<TMP_Text>();
            if (user != null)
                user.text = nick;
        }
    }
    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
}
