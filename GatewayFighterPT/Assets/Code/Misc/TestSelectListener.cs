using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestSelectListener : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public ScrollRect scrollRect;
    public TestScroll testScroll;
    public int index;

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("OnDeselect called");
    }

    public void OnSelect(BaseEventData eventData)
    {
        testScroll.SetVerticalPosition(index);
    }
}
