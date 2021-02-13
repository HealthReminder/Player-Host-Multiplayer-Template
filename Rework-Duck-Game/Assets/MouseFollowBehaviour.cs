using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollowBehaviour : MonoBehaviour
{
    bool isFollowing = false;
    Transform playerMouse;
    public void EnableMouseFollow(float playerID) {
        playerMouse = GameHost.instance.players_array[(int)playerID].GetComponent<PlayerManager>().mousePositionTransform;
        isFollowing = true;
    }

    public void DisableMouseFollow() {
        isFollowing = false;
        playerMouse = null;
    }

    private void Update() {
        if(!isFollowing)
            return;

        if(playerMouse){
            Vector3 vectorToTarget = playerMouse.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10);
        }
    }
}
