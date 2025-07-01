using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckPoint_HandleTrigger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public CheckpointDirector director;

    public void OnPointerDown(PointerEventData eventData)
    {
        director.clickflag = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        director.clickflag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        director.clickflag = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        director.clickflag = false;
    }
}
