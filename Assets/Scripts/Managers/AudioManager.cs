using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum audioTypes
{
    walking,
    openDoor,
    winTime,
    lostTime,
    Startjumping,
    Finishjumping,
    idle,
    closeDoor,
    none,
}

[System.Serializable]
public class AudioParameters
{
    public audioTypes type;
    public AudioClip clip;
    public bool loop;

    [Range(0, 1)]
    public float space;

    [Range(0, 1)]
    public float volume;
}
public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioParameters> audios;
    static List<AudioParameters> audios_;
    AudioSource source;
    private void Awake()
    {
        audios_ = audios;
        source = GetComponent<AudioSource>();

        Puzzle.winPuzzle += WinPuzzleSound;
        Game.winGame += WinPuzzleSound;
        RockPaperGame.winGame += WinPuzzleSound;

        Puzzle.lostPuzzle += LostPuzzleSound;
        Game.lostGame += LostPuzzleSound;
        RockPaperGame.lostGame += LostPuzzleSound;
    }

    public static void InitializeAudio(AudioSource source,audioTypes type)
    {
        var parameters = audios_.Find(x => x.type == type);
        if(parameters!=null && source)
        {
            source.clip = parameters.clip;
            source.loop = parameters.loop;
            source.spatialBlend = parameters.space;
            source.volume = parameters.volume;
        }

    }
    void LostPuzzleSound(Player player, in IPlayable puzzle)
    {
        InitializeAudio(source, audioTypes.lostTime);
        source.Play();
    }
    void WinPuzzleSound(Player player, in IPlayable puzzle)
    {
        InitializeAudio(source, audioTypes.winTime);
        source.Play();
    }
}
