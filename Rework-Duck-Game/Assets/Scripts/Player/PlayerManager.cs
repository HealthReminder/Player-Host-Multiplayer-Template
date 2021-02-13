using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using System;
[System.Serializable]   public class PlayerManager : MonoBehaviourPun,IPunObservable
{
    public int playerID;
    public int photonViewID;
    public new PhotonView photonView;
    public PlayerView playerView;
    public PlayerController playerController;
    public PlayerData playerData;
    public EmoteController emoteController;
    public InventoryController inventoryController;
    public StatusView statusView;
    public Transform mousePositionTransform;

    public SmartObject interactingWith;
    public Camera playerCamera;
    public Vector2 lookingDirection;
    

    public bool isAlive = true;
    public bool canMove = true;
    public bool canInteract = true;
    public bool canFeel = true;
    public bool canTakeDamage = true;
    public bool isSimUIOn = true;
    public bool isMuted = false;
    public bool canPickUp = true;
    public bool canDrop = true;
    public bool canLook = true;


    private void Start() {
        //Get its own GUIController
        playerView = this.GetComponent<PlayerView>();
        
        if(!photonView.IsMine){
            this.enabled = false;
            return;
        }

        playerCamera.enabled = true;
        playerView.ToggleSimUI(1);
        StartCoroutine(MouseMovement());
        StartCoroutine(Movement());
        StartCoroutine(Interaction());
        StartCoroutine(GUI());
        StartCoroutine(Effects());
        StartCoroutine(EmoteChat());
    }

    private void Update() {
        
        if(!photonView.IsMine)
            return;
        
            
        if(playerData.currentHP <= 0 && isAlive){
            isAlive = false;
            photonView.RPC("RPC_Die",RpcTarget.AllBuffered);
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(!photonView.IsMine)
            return;
    
        interactingWith = playerController.GetInteractingObject();


        if(other.transform.GetComponent<SmartObject>()){
            SmartObject obj = other.transform.GetComponent<SmartObject>();
            if(obj.hasOnTriggerEnterEvent && !obj.hasOwner){
                photonView.RPC("RPC_OnTriggeredObject", RpcTarget.AllBuffered, BitConverter.GetBytes(obj.objID));
                canTakeDamage = false;
            }
         }
        
        
        
    }
    IEnumerator MouseMovement() {

        while(canLook) {
            Vector2 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePositionTransform.position = mousePos;
            yield return null;
        }

        while(!canLook)
            yield return null;
        StartCoroutine(MouseMovement());
        yield break;
    }
    IEnumerator EmoteChat() {

        while(!isMuted) {
            if (Input.GetKeyDown(KeyCode.Q)){
                emoteController.ToggleChoices(1);
            } 
            if (Input.GetKeyUp(KeyCode.Q)){
                emoteController.ToggleChoices(0);
                emoteController.OnSelectedChoice(this);
            } 
            yield return null;
        }

        while(isMuted)
            yield return null;
        StartCoroutine(EmoteChat());
        yield break;
    }
   
    IEnumerator Effects() {

        while(canFeel) {
            //Deal with the numbers of effects
            if(playerData.currentHunger > 0)
            playerData.currentHunger -=1;
        
            yield return new WaitForSeconds(1);
            //Make a consequence of these numbers
            if(playerData.currentHunger<= 0)
                photonView.RPC("RPC_ChangeHP",RpcTarget.AllBuffered,(byte)1, (byte)0,(byte)0);
            
        }

        while(!canFeel)
            yield return null;
        StartCoroutine(Effects());
        yield break;
    }
    //float EkeyDownTime = 0;
    //bool isEKeyDown = false;
    public float rightMouseDownTime;
    public float leftMouseDownTime;
    IEnumerator Interaction() {
        
            rightMouseDownTime = 0;
            leftMouseDownTime = 0;
        while(canInteract) {
            //Get the input from the mouse
            //Get the time the button has been held down
            //Do it for both mouse buttons
            if(Input.GetMouseButton(1))
                rightMouseDownTime+= Time.deltaTime;
            else if(!Input.GetMouseButton(1))
                rightMouseDownTime = 0;
            
            if(Input.GetMouseButton(0))
                leftMouseDownTime+= Time.deltaTime;
            else if(!Input.GetMouseButton(0))
                leftMouseDownTime = 0;
        
            
            interactingWith = playerController.GetInteractingObjectMouse(playerCamera, transform.position, 2f);
            if(interactingWith){
                if(interactingWith.transform.childCount>=1)
                    inventoryController.EnableSelector(interactingWith.transform.GetChild(1).transform);
            } else {
                inventoryController.DisableSelector();
            }

            //Right mouse
            if(rightMouseDownTime > 0 && rightMouseDownTime <= 0.2f){
                //Player clicked
                //interactingWith = playerController.GetInteractingObjectMouse(playerCamera, transform.position, 2f);
                if(interactingWith) {
                    //Do pickup interaction and reset click
                    if(interactingWith.hasOnPickedUpEvent && canPickUp && !interactingWith.hasOwner){
                        statusView.DisplayText("Picking up "+interactingWith.name,1);
                        canPickUp = false;  rightMouseDownTime = -0.1f;
                        photonView.RPC("RPC_OnPickedUpObject", RpcTarget.AllBuffered, BitConverter.GetBytes(interactingWith.objID));}   

                 }
            } else if(rightMouseDownTime >= 0.3f) {
                //Player held the right button down
                if(inventoryController.handSlot){
                    //Do dropping interaction
                    statusView.DisplayText("Dropping "+inventoryController.handSlot.name,1);
                    canDrop = false; rightMouseDownTime = -0.1f;

                    Vector3 pPos = transform.position;
                    photonView.RPC("RPC_OnDroppedObject", RpcTarget.AllBuffered, 
                    BitConverter.GetBytes(pPos.x),
                    BitConverter.GetBytes(pPos.y)
                    );

                }  

            }

            //Left mouse
            if(leftMouseDownTime > 0 && leftMouseDownTime <= 0.2f){
                //Player clicked
                //interactingWith = playerController.GetInteractingObjectMouse(playerCamera, transform.position, 2f);
                if(interactingWith != null) {
                    //Do activate interaction
                    if(!interactingWith.hasOwner){
                        statusView.DisplayText("Activating "+interactingWith.name,1);
                        photonView.RPC("RPC_OnActivatedObject", RpcTarget.AllBuffered, BitConverter.GetBytes(interactingWith.objID));
                        leftMouseDownTime = -1f;
                    }        
                        
                } else {
                    //Do use interaction
                    interactingWith = inventoryController.handSlot;
                    if(interactingWith){
                            statusView.DisplayText("Activating "+interactingWith.name,1);
                            photonView.RPC("RPC_OnActivatedObject", RpcTarget.AllBuffered, BitConverter.GetBytes(interactingWith.objID));
                    }   
                }
            } else if(leftMouseDownTime >= 0.3f) {
                //Player held the left button down
                //Do use interaction
                    interactingWith = inventoryController.handSlot;
                    if(interactingWith){
                            statusView.DisplayText("Activating "+interactingWith.name,1);
                            photonView.RPC("RPC_OnActivatedObject", RpcTarget.AllBuffered, BitConverter.GetBytes(interactingWith.objID));
                    }                

            }


            /*
            if(EkeyDownTime >= 0.5f && isEKeyDown){
                if(interactingWith != null){
                    if(Vector3.Distance(interactingWith.transform.position,transform.position) <= 5){
                        if(interactingWith.hasActivateEvent){
                            if(!interactingWith.hasOnPickedUpEvent && !interactingWith.hasOnDroppedEvent && !interactingWith.hasOwner){
                                statusView.DisplayText("Activating "+interactingWith.name,1);
                                photonView.RPC("RPC_OnActivatedObject", RpcTarget.AllBuffered, BitConverter.GetBytes(interactingWith.objID));
                                isEKeyDown = false;
                            }else if(Input.GetKeyUp(KeyCode.E)){
                                statusView.DisplayText("Activating "+interactingWith.name,1);
                                photonView.RPC("RPC_OnActivatedObject", RpcTarget.AllBuffered, BitConverter.GetBytes(interactingWith.objID));
                                isEKeyDown = false;
                            }
                        }
                    }  
                }
            } if(EkeyDownTime >= 1.5f&& isEKeyDown){
                if(inventoryController.handSlot){
                    statusView.DisplayText("Dropping "+inventoryController.handSlot.name,1);
                    canDrop = false;
                    photonView.RPC("RPC_OnDroppedObject", RpcTarget.AllBuffered);
                    isEKeyDown = false;
                }  
            } else {
                if(isEKeyDown)
                    if(interactingWith != null){
                        if(Vector3.Distance(interactingWith.transform.position,transform.position) <= 5){
                            if(interactingWith.hasOnPickedUpEvent && canPickUp && !interactingWith.hasOwner){
                                statusView.DisplayText("Picking up "+interactingWith.name,1);
                                canPickUp = false;
                                isEKeyDown = false;
                                photonView.RPC("RPC_OnPickedUpObject", RpcTarget.AllBuffered, BitConverter.GetBytes(interactingWith.objID));
                            }
                        }
                    }
            }

            if(Input.GetKeyDown(KeyCode.E)){
                    isEKeyDown = true;
                 }
            if(isEKeyDown)
                EkeyDownTime+= Time.deltaTime;
            else 
                EkeyDownTime = 0;
            */
            
            yield return null;
             
        }

        while(!canInteract)
            yield return null;
        StartCoroutine(Interaction());
        yield break;
    }
    IEnumerator Movement() {

        while(canMove) {
            //Debug.Log(Input.GetAxis("Horizontal") + " "+ Input.GetAxis("Vertical") +" "+(playerData.speed/100) );
            lookingDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            playerController.Move(lookingDirection.x,lookingDirection.y,playerData.speed/10);

            yield return null;
        }

        while(!canMove)
            yield return null;
        StartCoroutine(Movement());
        yield break;
    }

     IEnumerator GUI() {

        while(true) {
            if(isSimUIOn) {
                playerView.ToggleSimUI(1);
                playerView.UpdatesimUI(playerData.currentHP, playerData.currentHunger);
            } else {
                playerView.ToggleSimUI(0);
            }

            yield return null;
        }
    }

    IEnumerator DamageInvencibility(float duration){
        canTakeDamage = false;
        yield return new WaitForSeconds(duration);
        canTakeDamage = true;
    }

     private void OnCollisionStay2D(Collision2D other) {
        if(!photonView.IsMine)
            return;
        
        if(other.gameObject.GetComponent<SmartObject>() && canTakeDamage){
        SmartObject obj = other.gameObject.GetComponent<SmartObject>();
            if(obj.hasOnCollisionEnterEvent){
                photonView.RPC("RPC_OnCollisionObject", RpcTarget.AllBuffered, BitConverter.GetBytes(obj.objID));
                //photonView.RPC("RPC_ChangeHP", RpcTarget.AllBuffered, (byte)1,(byte)0,(byte)1);
                //canTakeDamage = false;
            }
         }
            
       
            
    }
    public void PostEmote(int emoteIndex){
        photonView.RPC("RPC_PostEmote",RpcTarget.AllBuffered, (byte) emoteIndex);
    }

    /*public void OnShotWeapon(int objID) {
        WeaponRangedBehaviour rP = SmartObjectManager.instance.objectsInScene[objID].GetComponent<WeaponRangedBehaviour>();
        Vector2 bulletSpawnPos = rP.bulletPoint.position;
        float bulletSpawnRot = rP.bulletPoint.rotation.eulerAngles.z;
        Debug.Log(bulletSpawnRot + " " +rP.bulletPoint.rotation.eulerAngles + " " +rP.transform.rotation.eulerAngles);
        photonView.RPC("RPC_ShootWeapon",RpcTarget.AllBuffered, 
        BitConverter.GetBytes(objID), BitConverter.GetBytes(bulletSpawnPos.x),
        BitConverter.GetBytes(bulletSpawnPos.y), BitConverter.GetBytes(bulletSpawnRot)
        );
    }

    [PunRPC] void RPC_ShootWeapon (byte[] objIDBytes,byte[] xPosByte,byte[] yPosByte,byte[] zRotByte){
        int gunID = BitConverter.ToInt32(objIDBytes,0);
        Vector2 bulletSpawnPoint = new Vector2(BitConverter.ToSingle(xPosByte,0),BitConverter.ToSingle(yPosByte,0));
        float bulletSpawnRotation = BitConverter.ToSingle(zRotByte,0);
        Debug.Log("RPC_ShootWeapon at position: "+bulletSpawnPoint+" and rotation "+bulletSpawnRotation+" from gun of ID: "+gunID);
        
        SmartObjectManager.instance.objectsInScene[gunID].GetComponent<WeaponRangedBehaviour>().Shoot(bulletSpawnPoint,bulletSpawnRotation,playerID);
        
    }*/
    
    public void OnShotWeapon(int objID) {
        WeaponRangedBehaviour rP = SmartObjectManager.instance.objectsInScene[objID].GetComponent<WeaponRangedBehaviour>();
        Vector2 bulletSpawnPos = rP.bulletPoint.position;
        Vector3 bulletSpawnRot = rP.transform.rotation.eulerAngles;
        photonView.RPC("RPC_ShootWeapon",RpcTarget.AllBuffered, BitConverter.GetBytes(objID),
        BitConverter.GetBytes(bulletSpawnPos.x),BitConverter.GetBytes(bulletSpawnPos.y),
        BitConverter.GetBytes(bulletSpawnRot.x),BitConverter.GetBytes(bulletSpawnRot.y),BitConverter.GetBytes(bulletSpawnRot.z));
        Debug.Log(bulletSpawnRot);
    }

    [PunRPC] void RPC_ShootWeapon (byte[] objIDBytes,byte[] xPosByte,byte[] yPosByte,byte[] xRotByte,byte[] yRotByte,byte[] zRotByte){
        int gunID = BitConverter.ToInt32(objIDBytes,0);
        Vector2 bulletSpawnPoint = new Vector2(BitConverter.ToSingle(xPosByte,0),BitConverter.ToSingle(yPosByte,0));
        Vector3 bulletSpawnRotation = new Vector3(BitConverter.ToSingle(xRotByte,0),BitConverter.ToSingle(yRotByte,0),BitConverter.ToSingle(zRotByte,0));
        Debug.Log("RPC_ShootWeapon at position: "+bulletSpawnPoint+" and rotation "+bulletSpawnRotation+" from gun of ID: "+gunID);
        
        SmartObjectManager.instance.objectsInScene[gunID].GetComponent<WeaponRangedBehaviour>().Shoot(bulletSpawnPoint,bulletSpawnRotation,playerID);
    }

    [PunRPC] void RPC_PostEmote (byte byteIndex){
        Debug.Log("RPC_PostEmote");
        emoteController.PostEmote((int)byteIndex);
    }
    [PunRPC] void RPC_OnTriggeredObject (byte[] idBytes){
        SmartObject obj = SmartObjectManager.instance.objectsInScene[BitConverter.ToInt32(idBytes,0)];
        Debug.Log("Triggered "+obj.name + " "+obj.objID);
        canInteract = true;
    }

    [PunRPC] void RPC_OnDroppedObject (byte[] xByte, byte[] yByte){
        Vector3 droppedPos = new Vector3(BitConverter.ToSingle(xByte,0),BitConverter.ToSingle(yByte,0));
        Debug.Log("Received dropped message on: "+droppedPos);
        if(inventoryController.handSlot != null){
            Debug.Log("Dropped "+inventoryController.handSlot.name + " "+inventoryController.handSlot.objID);
            inventoryController.DropObject(droppedPos);
        }
        canDrop = true;
    }
    [PunRPC] void RPC_OnPickedUpObject (byte[] idBytes){
        Debug.Log("Received pickedup message");
        SmartObject obj = SmartObjectManager.instance.objectsInScene[BitConverter.ToInt32(idBytes,0)];
        
        bool isCloseEnough = false;
        if(Vector3.Distance(obj.transform.position,transform.position) <= 5)
            isCloseEnough = true;

        
        if(obj.onPickedUp != null && isCloseEnough){
            inventoryController.PickUpObject(obj,playerID);
            Debug.Log("Picked up "+obj.name + " "+obj.objID);
        }
        interactingWith = null;
        canPickUp = true;
    }
     [PunRPC] void RPC_OnActivatedObject (byte[] idBytes){
        SmartObject obj = SmartObjectManager.instance.objectsInScene[BitConverter.ToInt32(idBytes,0)];

        bool isCloseEnough = false;
        if(Vector3.Distance(obj.transform.position,transform.position) <= 5)
            isCloseEnough = true;

        if(obj.onActivate!= null && isCloseEnough){
            obj.onActivate.Invoke((float)playerID);
        }
        canInteract = true;
    }

    [PunRPC] void RPC_OnCollisionObject (byte[] idBytes){
        SmartObject obj = SmartObjectManager.instance.objectsInScene[BitConverter.ToInt32(idBytes,0)];
        Debug.Log("Collided with "+obj.name + " "+obj.objID);
        //obj.isActivated = true;
        //obj.view.Activate();
        //canInteract = true;
    }

    [PunRPC] void RPC_Die (){
        StopAllCoroutines();
        canFeel = false;
        canInteract = false;
        canMove = false;
        isSimUIOn = false;
        isMuted = true;
        canTakeDamage = false;
        canPickUp = false;
        canDrop = false;
        if(!photonView.IsMine)
            return;

        playerView.ToggleSimUI(0);
        playerView.ToggleDeathUI(1);
    }

    [PunRPC] void RPC_ChangeHP(byte gain,byte signal, byte invencibilityDuration) {
        Debug.Log("RPC_ChangeHP");
        if(signal == 1)
            playerData.currentHP += (int)gain;
        else if(signal == 0)
            playerData.currentHP -= (int)gain;

        if(invencibilityDuration >0)
            StartCoroutine(DamageInvencibility(invencibilityDuration));
    }

    [PunRPC] void RPC_PlayerSetup() {
        Debug.Log("RPC_PlayerSetup");
        //Here is where you setup the player
        playerView.Setup(playerData.maxHP, playerData.maxHunger);
        if(!photonView.IsMine)
            return;

        mousePositionTransform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}

    
}
