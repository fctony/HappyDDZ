using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAudio : AudioBase
{
    private void Awake()
    {
        Bind(AudioEvent.PLAY_EFFECT_AUDIO);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.PLAY_EFFECT_AUDIO:
                {
                    playeEffectAudio(message.ToString());
                    break;
                }
            default:
                break;
        }
    }

    /// <summary>
    /// 播放音乐的组件
    /// </summary>
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    private void playeEffectAudio(string assetName)
    {
        AudioClip ac = Resources.Load<AudioClip>("Sound/" + assetName);
        audioSource.clip = ac;
        audioSource.Play();
    }


}
