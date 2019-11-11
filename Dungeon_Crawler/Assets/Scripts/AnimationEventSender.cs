using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventSender : MonoBehaviour
{
    public UnityEvent onEventFire;

    public void Send()
    {
        onEventFire.Invoke();
    }
}
