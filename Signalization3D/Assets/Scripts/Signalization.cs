using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SignalizationZone))]
[RequireComponent(typeof(AudioSource))]
public class Signalization : MonoBehaviour
{
    [SerializeField] private float _volumeChangeDuration;

    private SignalizationZone _signalizationZone;
    private AudioSource _audioSource;
    private Coroutine _currentCoroutine;

    private float _maxVolume = 1;
    private float _minVolume = 0;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _signalizationZone = GetComponent<SignalizationZone>();

        _audioSource.volume = _minVolume;
        _currentCoroutine = null;

        _signalizationZone.Alarmed += OnAlarmed;
    }

    private void OnDisable()
    {
        _signalizationZone.Alarmed -= OnAlarmed;
    }

    private void OnAlarmed(bool isIntruderInHouse)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = StartCoroutine(ControlSignalization(_volumeChangeDuration, isIntruderInHouse));
    }

    private IEnumerator ControlSignalization(float duration, bool isIntruderInHouse)
    {
        float stepsCount = 10f;
        float proportion;
        var waitForSeconds = new WaitForSeconds(duration / stepsCount);

        if (isIntruderInHouse)
        {
            _maxVolume = 1;
            _minVolume = _audioSource.volume;
            _audioSource.Play();
            stepsCount -= _audioSource.volume * stepsCount;
        }
        else
        {
            _minVolume = 0;
            _maxVolume = _audioSource.volume;
            stepsCount -= (stepsCount - _audioSource.volume * stepsCount);
        }

        for (int i = 1; i <= stepsCount; i++)
        {
            proportion = i / stepsCount;

            if (isIntruderInHouse)
                _audioSource.volume = Mathf.Lerp(_minVolume, _maxVolume, proportion);
            else
                _audioSource.volume = Mathf.Lerp(_maxVolume, _minVolume, proportion);

            yield return waitForSeconds;
        }

        if (isIntruderInHouse == false)
            _audioSource.Stop();

        _minVolume = 0;
        _maxVolume = 1;
        _currentCoroutine = null;
    }
}
