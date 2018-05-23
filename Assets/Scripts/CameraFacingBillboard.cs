using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour {
    public Camera m_Camera;

    void Update () {
        transform.rotation = Camera.main.transform.rotation;
    }
} // CameraFacingBillboard