using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] music;
    public AudioSource[] sfx;

    public int musicNumber;

    Dictionary<string, int> dicSfxNumber = new Dictionary<string, int>()
    {
        {"Explosion", 0},
        {"FadeOut", 1},
        {"PlayerHurt", 2},
        {"PlayerJump", 3},
    };
    Dictionary<string, int> dicBGMNumber = new Dictionary<string, int>()
    {
        {"MainBGM", 0},
        {"GoalBGM", 1},
    };


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayMusic("MainBGM");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayMusic("GoalBGM");
        }
    }

    public void PlayMusic(string stringToNumber)
    {
        int a = dicBGMNumber[stringToNumber];
        for (int i = 0; i < music.Length; i++)
        {
            music[i].Stop();
        }
        music[a].Play();
    }

    public void PlaySfx(string stringTonumber)
    {
        int a = dicSfxNumber[stringTonumber];
        sfx[a].Play();
    }

}
