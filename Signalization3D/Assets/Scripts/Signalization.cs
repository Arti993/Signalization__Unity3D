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

    private bool _isIntruderInZone;
    private float _maxVolume = 1;
    private float _minVolume = 0;

    private void OnDisable()
    {
        _signalizationZone.Alarmed -= OnAlarmed;
        _signalizationZone.Disalarmed -= OnDisalarmed;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _signalizationZone = GetComponent<SignalizationZone>();

        _isIntruderInZone = false;
        _audioSource.volume = _minVolume;

        _signalizationZone.Alarmed += OnAlarmed;
        _signalizationZone.Disalarmed += OnDisalarmed;
    }

    private void Update()
    {
        if (_isIntruderInZone)
        {
            if (_audioSource.volume < _maxVolume)
                _audioSource.volume += _volumeChangeSpeed * Time.deltaTime;
        }
        else
        {
            if (_audioSource.volume > _minVolume)
            {
                _audioSource.volume -= _volumeChangeSpeed * Time.deltaTime;

                if(_audioSource.volume == 0)
                    _audioSource.Stop();
            }
        }
    }

    private void OnAlarmed()
    {
        _isIntruderInZone = true;
        _audioSource.Play();
    }

    private void OnDisalarmed()
    {
        _isIntruderInZone = false;
    }
}
