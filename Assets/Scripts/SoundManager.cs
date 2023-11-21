using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicsource;
    public void SetMusicVolume(float volume)
    {
        musicsource.volume = volume;
    }
}
