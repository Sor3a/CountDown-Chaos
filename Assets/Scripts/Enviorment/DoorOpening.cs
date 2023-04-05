using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour, Door
{
    Animator animator;
    AudioSource source;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }
    IEnumerator closeDoorSound()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.InitializeAudio(source, audioTypes.closeDoor);
        source.Play();
    }
    IEnumerator closingDoor()
    {
        yield return new WaitForSeconds(4f);
        animator.SetBool("character_nearby", false);
        StartCoroutine(closeDoorSound());
    }
    public bool OpenDoor()
    {
        if (animator.GetBool("character_nearby") == true) return false;
        StopAllCoroutines();
        animator.SetBool("character_nearby", true);
        AudioManager.InitializeAudio(source, audioTypes.openDoor);
        source.Play();
        StartCoroutine(closingDoor());
        return true;
    }
}
public interface Door
{
    public bool OpenDoor();
}
