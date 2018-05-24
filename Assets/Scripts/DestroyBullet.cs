using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour {

    public float timer = 3f;

    void Start () {
        Destroy (gameObject, timer);
    }

    void OnCollisionEnter (Collision target) {
        Destroy (gameObject);
    }

} // DestroyBullet
