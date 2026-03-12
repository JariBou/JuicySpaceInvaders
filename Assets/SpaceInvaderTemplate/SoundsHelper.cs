using UnityEngine;

public static class SoundsHelper
{
    static public void RandomPitch(AudioSource source, float minPitch = 0.5f, float maxPitch = 1.5f)
    {
        source.pitch = Random.Range(minPitch, maxPitch);
    }
}
