using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{

    private Vector3 newPos;
    protected override void Start()
    {
        base.Start();
        
        var pos = Screen.height / 4f;
        var posX = 0;

        var nePos = new Vector3(posX, pos, 0f);
        background.transform.position = newPos;
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        fakeImage.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        // background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        // base.OnPointerUp(eventData);
        background.transform.position = newPos;
    }
}