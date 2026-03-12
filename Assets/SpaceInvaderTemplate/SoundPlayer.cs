using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _clipList = new List<AudioClip>();

    public void PlaySound()
    {
        int randomIndex = Random.Range(0, _clipList.Count - 1);

        SoundsHelper.RandomPitch(_audioSource);
        _audioSource.PlayOneShot(_clipList[randomIndex]);
    }
}
