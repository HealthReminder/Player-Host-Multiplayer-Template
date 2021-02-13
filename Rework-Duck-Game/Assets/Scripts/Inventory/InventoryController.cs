using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public Transform inventorySlot;
    public SmartObject handSlot;
    public SpriteRenderer itemSelector;

    public void PickUpObject(SmartObject objPickedUp, float playerID) {
        Debug.Log("Picked up "+objPickedUp.name + " to "+playerID);
        
        if(handSlot != null)
            DropObject(handSlot.transform.position);

        handSlot = objPickedUp;
        if(handSlot){
            handSlot.hasOwner = true;
            objPickedUp.GetComponent<SpriteOrder>().followingTransform = transform;
            if(handSlot.hasOnPickedUpEvent){
                handSlot.onPickedUp.Invoke(playerID);
                Debug.Log("Called pickedup event on "+ objPickedUp);
            }
            handSlot.gameObject.layer = 2;
        }
    }

    public void DropObject(Vector3 droppedPos) {
        if(handSlot != null){
            handSlot.hasOwner = false;
            handSlot.GetComponent<SpriteOrder>().followingTransform = null;
            handSlot.transform.position = droppedPos;
            if(handSlot.hasOnDroppedEvent){
                handSlot.onDropped.Invoke(1);
            }
            handSlot.gameObject.layer = 0;
        }
        handSlot = null;
    }

    public void EnableSelector (Transform selectorTransform){
        if(!itemSelector.enabled)
            itemSelector.enabled = true;
        itemSelector.transform.position = selectorTransform.position;
        itemSelector.transform.localScale = selectorTransform.lossyScale;

        //itemSelector.color+= new Color(0,0,0,1);
        //itemSelector.color+= new Color(0,0,0,Mathf.InverseLerp(10,25));
        
    }   
    public void DisableSelector (){
        if(itemSelector.enabled)
            itemSelector.enabled = false;
    } 
}
