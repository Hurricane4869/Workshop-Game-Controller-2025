using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    FIRE,
    PLAYERHIT,
    GAMEOVER,
    OPICK
}
public enum BGMType
{
    MAINMENU,
    GAMEPLAY
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private AudioClip[] bgmList;
    private static SoundManager instance;
    private AudioSource sfxSource;
    private AudioSource bgmSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Agar tidak hancur saat ganti scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        bgmSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
    }
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.sfxSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
    public static void PlayBGM(BGMType bgm, float volume = 0.5f)
    {
        if (instance.bgmSource.clip == instance.bgmList[(int)bgm]) return; // Cegah restart jika sudah diputar

        instance.bgmSource.clip = instance.bgmList[(int)bgm];
        instance.bgmSource.volume = volume;
        instance.bgmSource.Play();
    }
    public static void StopBGM()
    {
        instance.bgmSource.Stop();
    }
}
