using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using System.IO;
public class LocalPlayerController : MonoBehaviour
{
    //This script creates the network object for the player to control
    int controlled_view_id = -1;
    private void Start()
    {
        if(controlled_view_id == -1)
            CreatePlayer();
    }
    private void CreatePlayer()
    {
        Debug.Log("Trying to create player");
        GameObject newPlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"),
        transform.position, Quaternion.identity, 0);
        controlled_view_id = newPlayer.GetComponent<PhotonView>().ViewID;
        byte[] viewByte = BitConverter.GetBytes(controlled_view_id);
        Debug.Log("Sending player array of " + viewByte.Length + " bytes");
        GameHost.instance.photonView.RPC("RPC_NewPlayer", RpcTarget.AllBuffered,viewByte);

    }
}
