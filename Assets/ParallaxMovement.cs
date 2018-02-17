using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMovement : MonoBehaviour {

    public GameObject cam;
    private Vector3 originalPos;
    private float offset;

	void Start() {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        originalPos = cam.transform.position;
        offset = transform.position.x - cam.transform.position.x;
	}
	
	void LateUpdate() {
        transform.position = new Vector3(originalPos.x + offset + (cam.transform.position.x - originalPos.x)/2, transform.position.y, transform.position.z);
	}
}
