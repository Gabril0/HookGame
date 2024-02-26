using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip rollClip;
    [SerializeField] private AudioClip hookClip;
    [SerializeField] private AudioClip deathClip;
    private PlayerController controller;
    private AudioSource rollSource;
    private AudioSource hookSource;
    private bool hookLock = false;
    private bool deathPlayed = false;

    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
        rollSource = transform.Find("RollSound").GetComponent<AudioSource>();
        hookSource = transform.Find("HookSound").GetComponent<AudioSource>();
        rollSource.clip = rollClip;
        hookSource.clip = hookClip;
    }

    void Update()
    {
        if (controller.isRolling && controller.isOnGround && !rollSource.isPlaying)
        {
            rollSource.Play();
        }
        else if (!controller.isRolling || !controller.isOnGround || !controller.isAlive
            || GameObject.Find("GameManager").GetComponent<GameManager>().isPaused || GameObject.Find("GameManager").GetComponent<GameManager>().displayResults)
        {
            rollSource.Stop();
        }

        if (controller.hittedHook && !hookSource.isPlaying && !hookLock)
        {
            hookSource.Play();
            hookLock = true;
        }

        if (!controller.hittedHook)
        {
            hookLock = false;
        }

        if (!deathPlayed && !controller.isAlive)
        {
            PlayDeathClip();
        }
    }


    void PlayDeathClip()
    {
        hookSource.clip = deathClip;
        hookSource.Play();
        deathPlayed = true;
    }
}
