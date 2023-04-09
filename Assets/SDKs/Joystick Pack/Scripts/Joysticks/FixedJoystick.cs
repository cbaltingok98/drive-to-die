using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    public void Reset()
    {
        base.OnPointerUp(null);
    }
}