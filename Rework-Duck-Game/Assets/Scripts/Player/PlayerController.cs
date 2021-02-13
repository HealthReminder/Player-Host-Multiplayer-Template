using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 movingDirection = Vector2.zero;
    public Transform appearance;
    public Animator animator;
    /* public void Move(float xInput,float yInput,float speed)
    {
        movingDirection = new Vector2(xInput, yInput);
        movingDirection = transform.TransformDirection(movingDirection);
        movingDirection *= speed;        
    }*/

    //public void UpdateRotation (Vector3 rotationVector) {
    //    if(rotationVector.magnitude > 0.25f){
    //            animator.SetBool("isWalking", true);
    //            appearance.rotation = Quaternion.Slerp(appearance.rotation, Quaternion.LookRotation(moveDirection), 0.15F);
    //        } else 
    //            animator.SetBool("isWalking", false);
    // }


	public void Move(float xInput,float yInput,float speed) {
		// Use input up and down for direction, multiplied by speed
		movingDirection = new Vector2(xInput, yInput);
		movingDirection = transform.TransformDirection(movingDirection);
		movingDirection *= speed*Time.deltaTime*100;
		
		transform.Translate(movingDirection);
	}

    public SmartObject GetInteractingObjectMouse(Camera cam, Vector3 playerPosition, float maxDistance) {

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        SmartObject obj = null;
        if (hit.collider != null) {
            if(hit.collider.GetComponent<SmartObject>())
                obj = hit.collider.GetComponent<SmartObject>();
        }

        if(obj)
            if(Vector2.Distance(obj.transform.position, playerPosition) > maxDistance)
                obj = null;
            
        return(obj);  
    }

    public SmartObject GetInteractingObject() {  
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2);
        //Debug.Log("Found "+hitColliders.Length+" interactable objects");
        SmartObject closestObject = null;
        float smallerDistance = 99;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            float currentDistance = Vector3.Distance(transform.position,hitColliders[i].transform.position);
            if(currentDistance <= smallerDistance){
                if(hitColliders[i].GetComponent<SmartObject>()){
                    closestObject = hitColliders[i].GetComponent<SmartObject>();
                    smallerDistance = currentDistance;
                }
            }
        }
        return(closestObject);

    }
}