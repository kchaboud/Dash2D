using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{

    public bool activated = true;
    private Vector3 startPos;
    private Vector3 sleepPos;
    private Vector3 warningPos;

    void Start()
    {
        startPos = transform.position;
        sleepPos = startPos - Vector3.up;
        warningPos = startPos - Vector3.up * 0.56f;
        StartCoroutine(SpikeCycle());
    }

    private IEnumerator SpikeCycle()
    {
        float gap;
        while (activated)
        {
            yield return new WaitForSeconds(1f);
            gap = 0.0f;
            while (transform.position.y > sleepPos.y)
            {
                gap += 0.2f;
                transform.position = startPos - Vector3.up * gap;
                yield return new WaitForSeconds(0.05f);
            }
            transform.position = sleepPos;
            yield return new WaitForSeconds(0.5f);
            gap = 0.15f;
            while (transform.position.y < warningPos.y)
            {
                transform.position = transform.position + Vector3.up * gap;
                yield return new WaitForSeconds(0.05f);
            }
            transform.position = warningPos;
            yield return new WaitForSeconds(0.5f);
            gap = 0.0f;
            while (transform.position.y < startPos.y - 0.2f)
            {
                gap += 0.2f;
                transform.position = transform.position + Vector3.up * gap;
                yield return new WaitForSeconds(0.05f);
            }
            transform.position = startPos;
        }
    }
}
