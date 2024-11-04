using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Sound[] _soundButton;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float stepCooldown = 0.5f; // Время между шагами
    private float lastStepTime;

    public void PlaySound(Sound[] clip, float volume = 1, float pitch1 = 1, float pitch2 = 1)
    {
        if (clip.Length == 0)
            return;

        _audioSource.pitch = UnityEngine.Random.Range(pitch1, pitch2);
        Sound sound = clip[UnityEngine.Random.Range(0, clip.Length)];
        AudioClip randomClip = sound.audioClip;
        _audioSource.PlayOneShot(randomClip, sound.volume);
    }

    public void PlayStepSound(Sound[] clip, float volume = 1, float pitch1 = 1, float pitch2 = 1)
    {
        if (clip.Length == 0)
            return;

        if (Time.time >= lastStepTime + stepCooldown)
        {

            _audioSource.pitch = UnityEngine.Random.Range(pitch1, pitch2);
            Sound sound = clip[UnityEngine.Random.Range(0, clip.Length)];
            AudioClip randomClip = sound.audioClip;
            _audioSource.PlayOneShot(randomClip, sound.volume);
            _audioSource.Play();
            lastStepTime = Time.time; // Обновляем время последнего шага
        }
    }

}

[Serializable]
public struct Sound
{
    public AudioClip audioClip;
    [Range(0f, 1f)] public float volume;
}
