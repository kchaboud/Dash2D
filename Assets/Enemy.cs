using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Animator anim;
    public GameObject bloodParticle;

    public void TakeDamage(GameObject hitter)
    {
        PlayBloodParticle(hitter);
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        anim.SetBool("isDead", true);
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void PlayBloodParticle(GameObject hitter)
    {
        Vector3 particlePos = transform.position + new Vector3(0, 1.5f, 0);
        GameObject bloodParticleObj = Instantiate(bloodParticle, particlePos, Quaternion.identity);
        ParticleSystem bloodParticleSys = bloodParticleObj.GetComponent<ParticleSystem>();
        var vel = bloodParticleSys.velocityOverLifetime;
        if (hitter != null)
        {
            int direction = 1;
            if (hitter.transform.position.x > transform.position.x) direction = -1;
            vel.xMultiplier = direction * Random.Range(3, 8);
        }
        vel.yMultiplier = Random.Range(-1, 2);
    }
}
