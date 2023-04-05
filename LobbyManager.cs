using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    public Text connectionInfoText;
    public Button joinButton;
    
    private void Start()
    {
        // 게임 버전이 다를 경우 매칭 x
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text ="Connecting To Master Server...";
    }
    
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to Master Server";
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : Connection Disabled {cause.ToString()} - Try reconnecting...";

        // 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public void Connect()
    {
        joinButton.interactable = false;
        if(PhotonNetwork.IsConnected){
            connectionInfoText.text = "Connecting to Random Room..";
            PhotonNetwork.JoinRandomRoom();
        } else {
            connectionInfoText.text = "Offline : Connection Disabled - Try reconnecting...";

            // 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "There is no empty room, Creating new Room";
        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = 2});
    }
    
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room.";
        
        // 씬 이동의 경우 아래 함수를 적용해야 한다! (참가자들도 자동으로 이동)
        PhotonNetwork.LoadLevel("Main");
    }
}