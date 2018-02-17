using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashResetter : MonoBehaviour {

    public bool used = false;
    public Color notUsedColor;
    public Color usedColor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TriggerDashResetter(this);
        }
    }

    public bool IsUsed() { return used; }
    public void Use()
    {
        used = true;
        GetComponent<SpriteRenderer>().color = usedColor;
    }
    public void Unuse()
    {
        used = false;
        GetComponent<SpriteRenderer>().color = notUsedColor;
    }




}
