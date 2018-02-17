using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public GameObject target;
    public Animator anim;

    private Vector3 offsetTarget;

    void Start()
    {
        offsetTarget = transform.position - target.transform.position;
    }

    void FixedUpdate () {
        float fireInput = Input.GetAxis("Fire1");
        anim.SetBool("Shooting", fireInput > 0);
	}

    void LateUpdate()
    {
        transform.RotateAround(target.transform.position, Vector3.forward, 300 * Time.deltaTime);
        //transform.position = target.transform.position + offsetTarget;
        //transform.RotateAround(transform.position, Vector3.forward, 360 * Time.deltaTime);
    }
}
