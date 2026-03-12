using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _clipList = new List<AudioClip>();

    public void PlayOneShotSound()
    {
        int randomIndex = Random.Range(0, _clipList.Count - 1);

        SoundsHelper.RandomPitch(_audioSource);
        _audioSource.PlayOneShot(_clipList[randomIndex]);
    }

    public void StopSound()
    {
        _audioSource.Stop();
    }

    public void PlaySoundLoop()
    {
        _audioSource.loop = true;
        _audioSource.clip = _clipList[0];
        _audioSource.Play();
    }
}
