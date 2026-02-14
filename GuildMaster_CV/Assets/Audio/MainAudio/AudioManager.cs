using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<AudioClip> _backgroundTracks; // Список треков
    [SerializeField] private AudioSource _audioSource;
    [SerializeField, Range(0, 1)] private float _targetVolume = 0.5f;
    [SerializeField] private float _fadeDuration = 1.5f; // Плавный переход

    private int _currentTrackIndex = 0;
    private Coroutine _activeFadeCoroutine;

    public AudioSource AudioSource => _audioSource;
    public float TargetVolume => _targetVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _audioSource.volume = 0f; // Начинаем с нулевой громкости
    }

    private void Start()
    {
        if (_backgroundTracks.Count > 0)
        {
            StartCoroutine(StartPlaybackWithFade());
        }
    }

    private IEnumerator StartPlaybackWithFade()
    {
        // Первый запуск с плавным увеличением громкости
        _audioSource.clip = _backgroundTracks[0];
        _audioSource.Play();
        yield return FadeVolume(0f, _targetVolume, _fadeDuration);

        // Запускаем цикл воспроизведения
        StartCoroutine(TrackPlaybackLoop());
    }

    private IEnumerator TrackPlaybackLoop()
    {
        while (true)
        {
            // Ждём окончания трека (минус время фейда)
            float waitTime = Mathf.Max(0, _audioSource.clip.length - _fadeDuration * 2);
            yield return new WaitForSeconds(waitTime);

            // Переход к следующему треку
            _currentTrackIndex = (_currentTrackIndex + 1) % _backgroundTracks.Count;
            yield return SwitchTrack(_currentTrackIndex);
        }
    }

    private IEnumerator SwitchTrack(int trackIndex)
    {
        // Плавное затухание текущего трека
        yield return FadeVolume(_audioSource.volume, 0f, _fadeDuration / 2);

        // Смена трека
        _audioSource.Stop();
        _audioSource.clip = _backgroundTracks[trackIndex];
        _audioSource.Play();

        // Плавное увеличение громкости
        yield return FadeVolume(0f, _targetVolume, _fadeDuration / 2);
    }

    private IEnumerator FadeVolume(float startVolume, float endVolume, float duration)
    {
        if (_activeFadeCoroutine != null)
        {
            StopCoroutine(_activeFadeCoroutine);
        }

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, endVolume, timer / duration);
            yield return null;
        }

        _audioSource.volume = endVolume;
        _activeFadeCoroutine = null;
    }

    public void ChangeTrack(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < _backgroundTracks.Count)
        {
            _currentTrackIndex = trackIndex;
            if (_activeFadeCoroutine != null)
            {
                StopCoroutine(_activeFadeCoroutine);
            }
            _activeFadeCoroutine = StartCoroutine(SwitchTrack(trackIndex));
        }
    }

    public void SetVolume(float volume)
    {
        _targetVolume = Mathf.Clamp01(volume); // Ограничиваем значение от 0 до 1
        _audioSource.volume = _targetVolume;   // Меняем громкость сразу


        //_targetVolume = Mathf.Clamp01(volume);
        //if (_activeFadeCoroutine != null)
        //{
        //    StopCoroutine(_activeFadeCoroutine);
        //}
        //_activeFadeCoroutine = StartCoroutine(FadeVolume(_audioSource.volume, _targetVolume, _fadeDuration / 2));
    }
}
