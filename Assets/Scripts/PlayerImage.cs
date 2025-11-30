using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class PlayerImage : MonoBehaviourPunCallbacks
{
    public TMP_Text playerName;
    public bool playerTeam;
    [SerializeField] public Image teamButton;
    public int roomSize = 0;
    public Player player;





    public void SetPlayerInfo(Player _player, bool team = false)
    {
        player = _player;
        playerName.text = _player.NickName;
        playerTeam = team;

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable() { { "team", "red" } };

        if (team)
        {
            playerProperties["team"] = "red";

        }
        else
        {
            playerProperties["team"] = "blue";

        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        if (player != PhotonNetwork.LocalPlayer)
        {
            teamButton.GetComponent<Button>().interactable = false;
        }

    }

    public void OnClickTeamChange()
    {
        
        playerTeam = !playerTeam;
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        if (playerTeam)
        {
            playerProperties["team"] = "red";
        }
        else
        {
            playerProperties["team"] = "blue";
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        

    }

    // public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    // {
    //     if (changedProps.ContainsKey("team"))
    //     {
    //         bool team = (bool)changedProps["team"];
    //         // Burada targetPlayer'ın UI'sini güncelle (ör: kırmızı/mavi buton)
    //         UpdatePlayerUI(team);
    //     }
    // }

    public void UpdatePlayerUI(bool t)
    {
        if (!t)
        {
            teamButton.color = Color.red;
        }
        else
        {
            teamButton.color = Color.blue;
        }
    }

    public Player GetPlayer()
    {
        return player;
    }

}
