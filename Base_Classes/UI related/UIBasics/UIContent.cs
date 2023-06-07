using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIContent : MonoBehaviour
{
    public abstract void Initialize();
    public virtual Vector2 GetSize()
    {
        Vector2 size = new Vector2();
        size.x = this.GetComponent<RectTransform>().rect.width;
        size.y = this.GetComponent<RectTransform>().rect.height;
        return size;
    }
}
