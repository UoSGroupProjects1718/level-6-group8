﻿using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class  Utility
{
    public static string NumberToCommaSeparatedString(int val)
    {
        StringBuilder sb = new StringBuilder(val.ToString());

        for (int i = sb.Length -3; i >= 3; i-=3)
        {
            sb.Insert(i, ',');
        }

        return sb.ToString();
    }

    public static string NumberToCommaSeparatedString(uint val)
    {
        return NumberToCommaSeparatedString((int)val);
    }

    /// <summary>
    /// Checks to see if the provided (x, y) coordinate is over a UI object
    /// </summary>
    /// <param name="x">Mouse/Touch x pos</param>
    /// <param name="y">Mouse/Touch y pos</param>
    /// <returns>True if over a UI object, otherwise false</returns>
    public static bool IsOverUIObject(float x, float y)
    {
        // https://answers.unity.com/questions/1073979/android-touches-pass-through-ui-elements.html

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(x, y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return (results.Count > 0);
    }
}