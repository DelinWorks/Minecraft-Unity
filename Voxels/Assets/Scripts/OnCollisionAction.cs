using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionAction : MonoBehaviour
{
    [Serializable]
    public class Event : UnityEvent { }

    public Event OnEnterTrigger;
    private void OnTriggerEnter(Collider other) => OnEnterTrigger?.Invoke();

    public Event OnStayTrigger;
    private void OnTriggerStay(Collider other) => OnStayTrigger?.Invoke();

    public Event OnExitTrigger;
    private void OnTriggerExit(Collider other) => OnExitTrigger?.Invoke();
}
