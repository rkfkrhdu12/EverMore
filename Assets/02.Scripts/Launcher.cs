using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public void Connect()
    {
        if (PhotonNetwork.IsConnected) { LogMessage.Log("Is Connected True"); return; }

        if (!string.IsNullOrWhiteSpace(PlayerPrefs.GetString(_playerNamePrefKey)))
        {
            if (PhotonNetwork.ConnectUsingSettings()) { LogMessage.Log("Connected Error"); }
        }
        else
        {
            SetPlayerName("Player");

            if (!string.IsNullOrWhiteSpace(PlayerPrefs.GetString(_playerNamePrefKey)))
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
    }

    public string GetPlayerName()
    {
        return PlayerPrefs.GetString(_playerNamePrefKey);
    }

    public void SetPlayerName(string playerName) 
    {
        if (string.IsNullOrWhiteSpace(playerName)) { return; }

        PlayerPrefs.SetString(_playerNamePrefKey, playerName);
    }

    public void Disconnect() => PhotonNetwork.Disconnect();
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    #region Private Variable

    private string _gameVersion = "0.1";

    private const string _playerNamePrefKey = "PlayerNickName";

    #endregion

    #region Monobehaviour Function
    private void Awake()
    {
        if (PhotonNetwork.GameVersion != _gameVersion)
            PhotonNetwork.GameVersion = _gameVersion;

        PhotonNetwork.AutomaticallySyncScene = true;

        // Connect();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.DeleteKey(_playerNamePrefKey);
        }
    }

    #endregion

    #region Photon Callback Function

    public override void OnConnectedToMaster()
    {
        LogMessage.Log("Server Connected");

        PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString(_playerNamePrefKey);

        PhotonNetwork.LoadLevel("MainScene");
    }
    
    public override void OnJoinedRoom()
    {
        LogMessage.Log("OnJoinedRoom()");

        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    #endregion

    #region Private Function

    private void CreateRoom() => PhotonNetwork.CreateRoom(PhotonNetwork.CountOfRooms.ToString(), new RoomOptions { MaxPlayers = 2 });

    #endregion

}
