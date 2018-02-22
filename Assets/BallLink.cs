using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLink : MonoBehaviour {

    private LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        line.SetPositions(new[]{transform.parent.position, transform.position});
    }
}
