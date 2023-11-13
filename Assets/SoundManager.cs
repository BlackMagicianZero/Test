using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private PlayOneShotBehaviour playOneShotBehaviour;
    public AudioSource musicsource;

    void Awake()
    {
        playOneShotBehaviour = GetComponent<PlayOneShotBehaviour>();
    }
    public void SetOneShotVolume(float value)
    {
        playOneShotBehaviour.SetVolume(value);
    }
    public void SetMusicVolume(float volume)
    {
        musicsource.volume = volume;
    }
}
