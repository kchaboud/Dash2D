using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public AudioSource music;
    public AudioSource dashSound;
    public AudioSource swordSound;

    public float slowdownFactor = 0.01f;
    public float slowdownLength = 0.5f;
    private Coroutine ResetTimeCo;

    private void Update()
    {
        //if (Time.timeScale < 1f)
        //{
        //    Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        //    Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        //    Time.fixedDeltaTime = Time.timeScale * 0.02f;
        //}
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        music.pitch = Time.timeScale;
        swordSound.pitch = Time.timeScale;
    }

    public void ResetTimeScale()
    {        
        ResetTimeCo = StartCoroutine(ResetTimeScaleCoroutine());
    }

    public void StopResetTimeScale()
    {
        if (ResetTimeCo == null) return;
        StopCoroutine(ResetTimeCo);
    }

    private IEnumerator ResetTimeScaleCoroutine()
    {
        float timeScaleIncrease = (1 - slowdownFactor) / 2;
        while (Time.timeScale < 1f)
        {
            Time.timeScale += timeScaleIncrease;
            if (Time.timeScale > 1f) Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            music.pitch = Time.timeScale;
            swordSound.pitch = Time.timeScale;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
