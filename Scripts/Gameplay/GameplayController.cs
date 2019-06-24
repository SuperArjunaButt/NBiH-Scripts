using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{

    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip deathMusic;

    private AudioSource musicSource;

    //// Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        StartBackgroundMusic();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void StopBackgroundMusic()
    {
        musicSource.Stop();
        musicSource.clip = deathMusic;
        musicSource.loop = false;
        musicSource.Play();
    }

    public void StartBackgroundMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
}
