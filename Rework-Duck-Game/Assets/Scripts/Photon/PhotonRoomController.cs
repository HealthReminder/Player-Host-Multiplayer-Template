using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRoomController : MonoBehaviourPunCallbacks, IInRoomCallbacks {
    //Room info
    public static PhotonRoomController instance;
    private PhotonView photon_view;

    public bool is_multiplayer_room_loaded;
    public int scene_current;

    //Player info
    Player[] photonPlayers;
    public int player_count;
    public int id_in_room;

    //Singleton pattern
    private void Awake() {
        if(PhotonRoomController.instance == null) 
            PhotonRoomController.instance = this;
        else
            if(PhotonRoomController.instance != this){
                //Destroy(PhotonRoomController.instance.gameObject);
                PhotonRoomController.instance = this;
            }
        DontDestroyOnLoad(this.gameObject);
        photon_view = GetComponent<PhotonView>();
    }


    public override void OnJoinedRoom(){
        base.OnJoinedRoom();
        photonPlayers = PhotonNetwork.PlayerList;
        player_count = photonPlayers.Length;
        id_in_room = player_count-1;
        PhotonNetwork.NickName = id_in_room.ToString();
        Debug.Log("Entered room with id of " +id_in_room);
        StartCoroutine(LoadRoom());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        player_count++;

    }
    IEnumerator LoadRoom()
    {
        Debug.Log("Started game");
        is_multiplayer_room_loaded = true;
        //Only the master must load the level. Otherwise the scene is loaded multiple times
        if (!PhotonNetwork.IsMasterClient)
           yield break;

        Debug.Log("Finished setting up everythin. Loading level for all.");
        PhotonNetwork.LoadLevel(MultiplayerSettings.instance.multiplayerScene);

        yield break;
    }


    public override void OnLeftRoom()
    {
        player_count--;
    }
    //When the player enters room the scene is loaded and the player view created
    //This is necessary so we can communicate with the player during the game
   
    public int lastView;
    public override void OnRoomListUpdate (List< RoomInfo > roomList) {
        Debug.Log("Room list updated");
    }
}
