using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBoom : MonoBehaviour {

    AudioSource dokomademoBoom;
    GameObject boomObj;
    private void Awake()
    {
        boomObj = transform.Find("Boom").gameObject;
         boomObj.transform.localScale = new Vector3(0,0);
         dokomademoBoom = transform.GetComponent<AudioSource>();
        boomObj.SetActive(false);
    }

    public void SetBoomAudio(AudioClip boom,Vector3 whereToBoom)
    {
        boomObj.transform.position = whereToBoom;
        dokomademoBoom.clip = boom;
        gameObject.SetActive(true);
        boomObj.SetActive(true);
        Boom();
    }

    private void Boom()
    {
            dokomademoBoom.Play();
            Invoke("HideAudioObj", dokomademoBoom.clip.length);
    }

    void HideAudioObj()
    {
        gameObject.SetActive(false);
        boomObj.transform.localScale = new Vector3(0, 0);
        boomObj.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        Resize();
    }

    void Resize()
    {
        if (boomObj.transform.localScale.x > 1.5f)
            boomObj.SetActive(false);
        else
            boomObj.transform.localScale = new Vector3(boomObj.transform.localScale.x + 0.3f, boomObj.transform.localScale.y + 0.3f);
    }

}
