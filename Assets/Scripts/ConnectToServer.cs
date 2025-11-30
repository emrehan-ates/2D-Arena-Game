using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    [SerializeField] public TMP_InputField userNameInput;
    [SerializeField] public TMP_Text buttonText;

    public void OnClickConnect(){
        if(userNameInput.text.Length >= 1){
            PhotonNetwork.NickName = userNameInput.text;
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.SerializationRate = 20;
        Debug.Log("Connected to master" + PhotonNetwork.SendRate + " " + PhotonNetwork.SerializationRate);
        
        SceneManager.LoadScene("Lobby");
    }
}
