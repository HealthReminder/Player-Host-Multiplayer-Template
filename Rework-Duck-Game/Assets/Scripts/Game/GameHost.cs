using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
[System.Serializable]  public class GameHost : MonoBehaviour
{
    //The game host is not a game or match manager
    //It must not contain direct gameplay code, only the information necessary for all the processes it calls
    //This is following the concept of centralizing information to facilitate readability
    //It makes the decision on the host side and send the decisions to the players
    //The players run the code on their local machine
    //The game host also synchronize all the players on callbacks and every x frames
    public static GameHost instance;
    public PhotonView photonView = null;

    //Following variables should be synchronized as much as possible
    public PhotonView[] players_array;
    public bool gameHasStarted = false;

    int player_count = 0;

    public bool isSynchronizingPlayers = false;
    void Awake(){
        instance = this;
        if (photonView == null)
            photonView = GetComponent<PhotonView>();
    }

    private void Update() {
        if(!PhotonNetwork.IsMasterClient)
            return;     

        if (!gameHasStarted)
        {
            Debug.Log("Initiated match loop.");
            gameHasStarted = true;
        }

        GameLoop();

        for (int i = 0; i < player_count; i++)
            if(players_array[i] == null)
                OnPlayerLeftRoom();
            
         
   }
    public void GameLoop() {
        //These functions deals with players leaving the current match
        if (CheckForPlayersLeft())
            OnPlayerLeftRoom();

        //This is how you would make all the players do something
        //foreach(PhotonView player in player_list){
        //    player.RPC("RPC_PlayerSetup",RpcTarget.All);
        //}
   }
bool CheckForPlayersLeft()
    {
        bool result = false;

        player_count = players_array.Length;
        for (int i = 0; i < player_count; i++)
            if (players_array[i] == null)
                result = true;

        return result;
    }
   public void OnPlayerLeftRoom ()  {
       Debug.Log("A player left the room.");
        if(!PhotonNetwork.IsMasterClient)
            return;
        
        List<PhotonView> p = new List<PhotonView>();

        for (int i = 0; i < player_count; i++)
            p.Add(players_array[i]);

        for (int o = p.Count-1; o >= 0; o--)
            if(p[o] == null)
                p.RemoveAt(o);
                             
        players_array = new PhotonView[p.Count];
        for (int u = 0; u < p.Count; u++)
            players_array[u] = p[u];

        player_count = players_array.Length;

        StartCoroutine(SynchronizePlayers());
    }

    [PunRPC] public void RPC_NewPlayer(byte[] viewBytes)
    {
        int new_player_id = -1;
        if (!PhotonNetwork.IsMasterClient)
            return;
        //This function is responsible for adding the player to the listOfPlayersPlaying 
        if (players_array == null || players_array.Length <= 0)
        {
            players_array = new PhotonView[1];
            new_player_id = 0;
        }
        else
        {
            PhotonView[] new_player_array = new PhotonView[players_array.Length + 1];
            new_player_id = players_array.Length;

            for (int i = 0; i < players_array.Length; i++)
            {
                new_player_array[i] = players_array[i];
            }
            players_array = new_player_array;
        }
        player_count = players_array.Length;

        //Deserialize information to get the viewID so the player PhotonView can be found in the network
        //And added to the player list and also be setup
        int received_view_id = BitConverter.ToInt32(viewBytes, 0);
        Debug.Log("Player " + received_view_id + " joined the room with ID of " + new_player_id);

        PhotonView player_view = PhotonNetwork.GetPhotonView(received_view_id);
        Debug.Log("Joined player with view id of:" + received_view_id + " and index of: " + new_player_id + ". New count of players is: " + players_array.Length);
        players_array[new_player_id] = player_view;

        StartCoroutine(SynchronizePlayers());
    }
    #region Synchronization

    IEnumerator SynchronizePlayers()
    {
        Debug.Log("Did not synchronize players.");
        yield break;
        isSynchronizingPlayers = true;
        for (int i = 0; i < players_array.Length; i++)
            photonView.RPC("RPC_SynchronizePlayer", RpcTarget.All,
            BitConverter.GetBytes(players_array[i].ViewID),
            BitConverter.GetBytes(i),
            BitConverter.GetBytes(players_array.Length)
            );


        while (isSynchronizingPlayers)
            yield return null;

    }
    [PunRPC]    public void RPC_SynchronizePlayer(byte[] viewIDBytes, byte[] indexBytes, byte[] listSizeBytes)
    {
        Debug.Log("RPC_SynchronizePlayer");

        int receivedViewId = BitConverter.ToInt32(viewIDBytes, 0);
        PhotonView receivedPlayer = PhotonNetwork.GetPhotonView(receivedViewId);

        int receivedIndex = BitConverter.ToInt32(indexBytes, 0);
        int receivedSize = BitConverter.ToInt32(listSizeBytes, 0);

        receivedPlayer.GetComponent<PlayerManager>().playerID = receivedIndex;
        receivedPlayer.GetComponent<PlayerManager>().photonViewID = receivedViewId;

        //If there is no list create it
        if (players_array == null || players_array.Length != receivedSize)
            players_array = new PhotonView[receivedSize];

        players_array[receivedIndex] = receivedPlayer;

        isSynchronizingPlayers = false;
        Debug.Log("Finished synchronizing player data");

    }
    #endregion
}
