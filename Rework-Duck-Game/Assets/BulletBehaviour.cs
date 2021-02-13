using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float bulletSpeed;
    private void Update() {
        transform.Translate(transform.right*bulletSpeed,Space.World);
    }

}
