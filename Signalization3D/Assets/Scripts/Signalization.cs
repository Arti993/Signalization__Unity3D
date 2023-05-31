using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SignalizationZone))]
[RequireComponent(typeof(AudioSource))]
public class Signalization : MonoBehaviour
{
    [SerializeField] private float _volumeChangeSpeed;

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

    private void OnDestroy()
    {
        _signalizationZone.Alarmed -= OnAlarmed;
    }

    private void OnAlarmed(bool isIntruderInHouse)
    {
        float desiredVolume;

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        else
        {
            if (isIntruderInHouse)
                _audioSource.Play();
        }

        if (isIntruderInHouse)
            desiredVolume = _maxVolume;
        else
            desiredVolume = _minVolume;

        _currentCoroutine = StartCoroutine(ControlSignalization(desiredVolume));
    }

    private IEnumerator ControlSignalization(float desiredVolume)
    {
        while (_audioSource.volume != desiredVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, desiredVolume, _volumeChangeSpeed * Time.deltaTime);
            yield return null;
        }

        if(_audioSource.volume == _minVolume)
            _audioSource.Stop();

        _currentCoroutine = null;
    }
}
