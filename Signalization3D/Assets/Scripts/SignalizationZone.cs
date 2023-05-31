using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SignalizationZone : MonoBehaviour
{
    private bool _isIntruderInHouse;

    public event UnityAction<bool> Alarmed;
  
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _isIntruderInHouse = true;
            Alarmed?.Invoke(_isIntruderInHouse);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _isIntruderInHouse = false;
            Alarmed?.Invoke(_isIntruderInHouse);
        }
    }

}
