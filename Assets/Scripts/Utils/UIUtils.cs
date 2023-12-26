using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIUtils : MonoBehaviour
{
    public static VisualElement CreateUIElement(string className)
    {
        return CreateUIElement<VisualElement>(className);
    }

    public static T CreateUIElement<T>(params string[] className) where T : VisualElement, new()
    {
        var element = new T();
        foreach (var name in className)
        {
            element.AddToClassList(name);
        }
        return element;
    }
}
