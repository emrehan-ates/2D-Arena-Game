using UnityEngine;
using TMPro;

public class RoomItem : MonoBehaviour
{

    public TMP_Text roomName;
    public LobbyManager manager;

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void Start()
    {
        manager = FindFirstObjectByType<LobbyManager>();

    }

    public void OnClickJoin()
    {
        manager.JoinRoom(roomName.text);
    }
}