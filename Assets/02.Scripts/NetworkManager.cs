using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    #region Private Variable

    #endregion

    #region Photon Callback Function

    // AnyTime
    public override void OnDisconnected(DisconnectCause cause)
    {
        LogMessage.Log("DisConnected");
    }

    // InGame

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        LogMessage.Log("new Player Entered");

    }

    public override void OnLeftRoom()
    {
        LogMessage.Log("OnLeftRoom()");

        PhotonNetwork.LoadLevel("MainScene");
    }
    #endregion

    #region Monobehaviour Function

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.Disconnect();


        }

    }

    private void OnApplicationQuit()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    #endregion

}
