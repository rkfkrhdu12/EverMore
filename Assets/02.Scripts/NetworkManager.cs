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
    [SerializeField]
    private GameObject _respawnButton;

    #endregion

    #region Photon Callback Function

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("new Player Entered");

        _respawnButton.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom()");

        SceneManager.LoadScene(0);
    }
    #endregion

    #region Monobehaviour Function

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveRoom();
        }

        if (_respawnButton.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            Spawn();
        }
    }

    #endregion

    #region Private Function

    public void Spawn()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

        _respawnButton.SetActive(false);
    }

    #endregion
}
