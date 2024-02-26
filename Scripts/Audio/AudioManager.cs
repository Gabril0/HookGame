using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip stageIntro;
    [SerializeField] AudioClip stageSongDrum;
    [SerializeField] AudioClip stageSong;
    [SerializeField] AudioSource drumSource;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.loop = false;
        source.clip = stageIntro;
        source.Play();
    }
    void Update()
    {
        if (!source.isPlaying) {
            source.loop = true;
            drumSource.loop = true;
            drumSource.clip = stageSongDrum;
            drumSource.Play();
            source.clip = stageSong;
            source.Play();
        }
        if (GameObject.Find("GameManager").GetComponent<GameManager>().displayResults) {
            drumSource.Stop();
        }
        if (!GameObject.Find("Player").GetComponent<PlayerController>().isAlive) {
            drumSource.Stop();
            source.Stop();
        }
    }
}
