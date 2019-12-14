using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionAction : MonoBehaviour
{
    [Serializable]
    public class OnEvent : UnityEvent { }

    public OnEvent OnEnterTrigger;
    private void OnTriggerEnter(Collider other) => OnEnterTrigger?.Invoke();

    public OnEvent OnStayTrigger;
    private void OnTriggerStay(Collider other) => OnStayTrigger?.Invoke();

    public OnEvent OnExitTrigger;
    private void OnTriggerExit(Collider other) => OnExitTrigger?.Invoke();
}
