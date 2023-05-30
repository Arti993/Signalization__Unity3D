using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SignalizationZone : MonoBehaviour
{
    public event UnityAction Alarmed;
    public event UnityAction Disalarmed;
  
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            Alarmed?.Invoke();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            Disalarmed?.Invoke();
        }
    }

}
