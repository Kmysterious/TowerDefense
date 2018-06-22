using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour {

    public AudioClip titleMusic;
    public AudioClip gameOverMusic;
    public AudioClip[] backgroundMusic;
    GameObject volumeSlider;
    AudioSource music;
    public static MusicManager instance;
    // Use this for initialization

    private void Awake()
    {
      if(instance == null)
        {//避免換場景就被Destroy
            instance = this;
            DontDestroyOnLoad(this);
        }
      else if(this != instance)
        {
            Destroy(gameObject);
        }
    }
    void Start () {
        AudioListener.volume = Global.volume;
        music = transform.GetComponent<AudioSource>();
        music.clip = titleMusic;
        music.Play();
    }

    public void PlayTitleMusic()
    {
        music.clip = titleMusic;
        music.Play();
    }

    public void StartGameMusic()
    {
        int a = Random.Range(0, backgroundMusic.Length);
        music.clip = backgroundMusic[a];
        music.Play();
    }

    public void GameOverMusic()
    {
        music.clip = gameOverMusic;
        music.Play();
    }

    public void SetVolume()
    {
        volumeSlider = GameObject.FindGameObjectWithTag("VolumeController");
        Global.volume =  volumeSlider.transform.Find("Slider").GetComponent<Slider>().value;
        AudioListener.volume = Global.volume;
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
