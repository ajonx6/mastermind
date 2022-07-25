using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickColor : MonoBehaviour {
    // private int[] colors = new int[] {0xff0000, 0x00ff00, 0x0000ff, 0xff00ff, 0x5f1fff, 0xff5F1f};
    
    [HideInInspector] public bool active = false;
    [HideInInspector] public int currentColorIndex = -1;
    [HideInInspector] public Color currentColor;

    private Color uncoveredColor = new Color(0.5943396f, 0.5943396f, 0.5943396f);
    private Color activeColor = new Color(1f, 1f, 1f);
    private Color[] colors = new Color[] {new Color(1f, 0f, 0f), new Color(0f, 1f, 0f), new Color(0f, 0f, 1f), new Color(1f, 0f, 1f), new Color(0.373f, 0.122f, 1f), new Color(1f, 0.373f, 0.122f)};
    private bool changed = false;

    public void EnableCell() {
        active = true;
        gameObject.GetComponent<SpriteRenderer>().color = activeColor;
    }

    public void OnMouseDown() {
        if (!active || changed) return;
        changed =  true;
        currentColorIndex++;
        if (currentColorIndex >= colors.Length) currentColorIndex = 0;
        currentColor = colors[currentColorIndex];
        gameObject.GetComponent<SpriteRenderer>().color = currentColor;
    }

    public void OnMouseUp() {
        changed = false;
    }
}