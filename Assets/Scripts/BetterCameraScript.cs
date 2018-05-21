using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BetterCameraScript : NetworkBehaviour {

    private Transform myTransform;
    private Transform target;
    public Vector3 offset = new Vector3 (0f, 0.6f, -0.5f);
    private bool justStarted;
    public float panSpeed = 4f;

    void Awake () {
        //target = GameObject.FindGameObjectWithTag ("Player").transform;
        justStarted = true;
        LockMouse ();
    }

    void Start () {
        myTransform = this.transform;
    }

    void OnConnectedToServer () {
        if (target == null) {
            target = GameObject.FindGameObjectWithTag ("Player").transform;
        }
    }

    void Update () {
        // Follow character when spacebar is held
        OnConnectedToServer ();
        FollowPlayer ();
        PanCamera ();
    }

    void FollowPlayer () {
        if (Input.GetKey (KeyCode.Space)) {
            if (target) {
                myTransform.position = target.position + offset;
                myTransform.LookAt (target.position, Vector3.up);

            }
        }

        if (justStarted) {
            justStarted = false;
            if (target) {
                myTransform.position = target.position + offset;
                myTransform.LookAt (target.position, Vector3.up);

            }
        }
    }

    void PanCamera () {
        // Pan with mouse
        if (Input.mousePosition.x > Screen.width - 2f) {
            Vector3 temp = transform.position;
            temp.x += panSpeed * Time.deltaTime;

            transform.position = temp;
        }

        if (Input.mousePosition.x < 1f) {
            Vector3 temp = transform.position;
            temp.x -= panSpeed * Time.deltaTime;

            transform.position = temp;
        }

        if (Input.mousePosition.y > Screen.height - 2f) {
            Vector3 temp = transform.position;
            temp.z += panSpeed * Time.deltaTime;

            transform.position = temp;
        }

        if (Input.mousePosition.y < 1f) {
            Vector3 temp = transform.position;
            temp.z -= panSpeed * Time.deltaTime;

            transform.position = temp;
        }
    }

    void LockMouse () {
        // Confines mouse to game (Only in .exe)
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

} // BetterCameraScript