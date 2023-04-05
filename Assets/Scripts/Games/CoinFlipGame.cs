using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoinFlipGame : Game
{
    [SerializeField] Animator animator;
    bool isPlaying = false;
    private void OnEnable()
    {
        isPlaying = false;
        Intitialize();
    }
    IEnumerator resultat()
    {
        yield return new WaitForSeconds(4f);
        isPlaying = false;
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            WinGame();
            animator.SetBool("flipping", false);
            animator.SetBool("win", true);
        }
        else
        {
            animator.SetBool("flipping", false);
            animator.SetBool("lost", true);
            LostGame();
        }
            
    }
    public void PlayGame()
    {
        if(!isPlaying)
        {
            isPlaying = true;
            animator.SetBool("flipping", true);
            animator.SetBool("lost", false);
            animator.SetBool("win", false);
            StartCoroutine(resultat());
        }

    }
}
