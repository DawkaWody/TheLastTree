using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip _rainSfx;
    [SerializeField] private AudioClip _waterCollectSfx;
    [SerializeField] private AudioClip _hitSfx;
    [SerializeField] private AudioClip _leafPadCollectSfx;
    [SerializeField] private AudioClip _waterUseSfx;
    [SerializeField] private AudioSource _secondAudioSource;

    private AudioSource _audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayRainSfx()
    {
        _audioSource.clip = _rainSfx;
        _audioSource.loop = true;
        _audioSource.volume = 0.2f;
        _audioSource.Play();
    }

    public void StopRainSfx()
    {
        _audioSource.Stop();
    }

    public void PlayWaterCollectSfx()
    {
        AudioSource.PlayClipAtPoint(_waterCollectSfx, transform.position);
    }

    public void PlayHitSfx()
    {
        AudioSource.PlayClipAtPoint(_hitSfx, transform.position);
    }

    public void PlayLeafPadCollectSfx()
    {
        AudioSource.PlayClipAtPoint(_leafPadCollectSfx, transform.position);
    }

    public void PlayWaterUseSfx(float duration)
    {
        _secondAudioSource.clip = _waterUseSfx;
        _secondAudioSource.Play();
        StartCoroutine(StopWaterUseSfx(duration));
    }

    private IEnumerator StopWaterUseSfx(float duration)
    {
        yield return new WaitForSeconds(duration);
        _secondAudioSource.Stop();
    }
}
