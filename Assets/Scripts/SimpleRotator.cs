using UnityEngine;
using System.Collections;

public class SimpleRotator : MonoBehaviour {

    public Vector3 speed;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        transform.rotation = transform.rotation * Quaternion.Euler(speed);
    }
}
