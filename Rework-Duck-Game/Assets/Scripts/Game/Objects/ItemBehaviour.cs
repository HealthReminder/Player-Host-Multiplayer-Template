using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemBehaviour : SerializableBehaviour
{
    
    Transform ownerTransform;

    [Header("Synchronized variables")]
    public int ownerID = -1;

    private void Update() {
        if(ownerTransform)
            transform.position = ownerTransform.position;
    }
    public void GetPickedUp(float playerID) {
        Debug.Log("Picked up "+gameObject.name);
        ownerID = (int)playerID;
        if(GameHost.instance.players_array[ownerID])
            ownerTransform = GameHost.instance.players_array[ownerID].GetComponent<PlayerManager>().inventoryController.inventorySlot;

        //gameObject.SetActive(false);
    }

    public void GetDropped() {
        Debug.Log("Dropped "+gameObject.name);
        ownerTransform = null;
        ownerID = -1;
        //gameObject.SetActive(true);
    }

    public override byte[] SerializeData() {
        if(ownerID == -1)
            return(new byte[0]);
        byte[] data;
        data = BitConverter.GetBytes(ownerID);
        
        return(data);
    }
    public override void DeserializeData(byte[] data) {
        Debug.Log("Started deserialization");
        if(data.Length == 0)
            ownerID = -1;
        else
            ownerID = BitConverter.ToInt32(data,0);
        
        if(ownerID >=0)
            if(GameHost.instance.players_array[ownerID])
                ownerTransform = GameHost.instance.players_array[ownerID].GetComponent<PlayerManager>().inventoryController.inventorySlot;
            else
                Debug.LogError("Player not found.");
        Debug.Log("Deseralized item ownership by owner "+ownerID);
    }

}
