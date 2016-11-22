using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(ColorView), typeof(Selectable))]
public class RemoveColorButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        CommandStream.Instance.Push(new RemoveColorCommand(gameObject));
    }
}
