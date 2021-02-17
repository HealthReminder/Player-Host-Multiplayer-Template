using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using System;
[System.Serializable]   public class PlayerManager : MonoBehaviourPun,IPunObservable
{
    //Before anything I reccomend that the player scripts be separate as the following
    //PlayerManager -> Only calls other scripts and has player RPCs
    //PlayerController -> Handles the gameplayer and physics functions like moving
    //PlayerView -> Handles GUI
    //PlayerInput -> Handles Input


    //This class will control one instance of a player each
    //It handles camera control
    public int playerID;
    public int photonViewID;
    public new PhotonView photonView;

    //Local controlled variables
    public Camera player_camera;
    public AudioListener player_listener;

    //Synchronized variables
    public Vector2 moving_direction;
    bool can_move = false;
    bool can_look = false;


    private void Start() {
        OnJoinedRoom();
    }

    private void OnJoinedRoom()
    {
        //Makes sure only the player camera the player control is on
        if (!photonView.IsMine)
        {
            player_camera.enabled = false;
            player_listener.enabled = false;
            can_move = false;
        } else
        {
            player_camera.enabled = true;
            player_listener.enabled = true;
            can_move = true;
        }
    }

    private void Update() {
        
        if(!photonView.IsMine)
            return;

        if (can_move)
        {
            //Debug.Log(Input.GetAxis("Horizontal") + " "+ Input.GetAxis("Vertical") +" "+(playerData.speed/100) );
            moving_direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Move(moving_direction.x, moving_direction.y, 3.0f/10.0f);
        }

    }
    public void Move(float xInput,float yInput,float speed) {
		// Use input up and down for direction, multiplied by speed
		moving_direction = new Vector2(xInput, yInput);
        moving_direction = transform.TransformDirection(moving_direction);
        moving_direction *= speed*Time.deltaTime*100;
		
		transform.Translate(moving_direction);
	}
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!photonView.IsMine)
            return;
        //Player could take damage here
        //Sending RPC take damage
    }
    private void OnCollisionStay2D(Collision2D other) {
        if(!photonView.IsMine)
            return;

        //Player could be teleported
        //Sending RPC collided
        //photonView.RPC("RPC_OnCollisionObject", RpcTarget.AllBuffered, BitConverter.GetBytes(obj.objID));
        //photonView.RPC("RPC_Teleport", RpcTarget.AllBuffered, (byte)1,(byte)0,(byte)1);

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}

    
}
