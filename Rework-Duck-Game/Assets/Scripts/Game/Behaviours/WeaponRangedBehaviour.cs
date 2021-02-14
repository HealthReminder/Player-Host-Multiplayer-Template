using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRangedBehaviour : MonoBehaviour
{
    public float fireRate = 0.01f;
    public GameObject bulletPrefab;
    public Transform bulletPoint;
    
    float fireRateCooldown= 0;
    private void Update() {
        if(fireRateCooldown > 0)
            fireRateCooldown -= Time.deltaTime;
    }
    public void OnShot(float playerID) {
        if(fireRateCooldown <= 0){
            fireRateCooldown = fireRate;
            GameHost.instance.players_array[(int)playerID].GetComponent<PlayerManager>().OnShotWeapon(GetComponent<SmartObject>().objID);
        }
    }
    public void Shoot(Vector2 spawnPos, Vector3 spawnRot, int ignoringPlayerID) {
        GameObject newBullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        //newBullet.transform.rotation = Quaternion.LookRotation( newBullet.transform.forward, spawnRot);
        //newBullet.transform.forward = spawnRot;
        newBullet.transform.rotation = Quaternion.Euler(0, 0,spawnRot.z);
        //newBullet.transform.rotation = Quaternion.LookRotation(spawnRot,newBullet.transform.forward);
        Debug.Log("Spawned with rotation of: "+newBullet.transform.rotation.eulerAngles + " for received vector of "+spawnRot);
        newBullet.GetComponent<ColliderTriggerBehaviour>().ignoringPlayerID = ignoringPlayerID;
    }

    /*public void Shoot(Vector2 spawnPos, Quaternion spawnRot, int ignoringPlayerID) {
        Instantiate(bulletPrefab, spawnPos, spawnRot).GetComponent<ColliderTriggerBehaviour>().ignoringPlayerID = ignoringPlayerID;
    } */

}
