using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class TurnIndicatingSlider : MonoBehaviour
{
    private Slider slider;

    [SerializeField]private Color backgroundColor = Color.white;
    [SerializeField]private Color playerTurnColor = Color.green;
    [SerializeField]private Color nonPlayerTurnColor = Color.red;
    public bool isPlayerTurn = true;

    public Color BackgroundColor
    {
        get => backgroundColor;
        set
        {
            backgroundColor = value;
            UpdateColor();
        }
    }
    
    public Color PlayerTurnColor
    {
        get => playerTurnColor;
        set
        {
            playerTurnColor = value;
            UpdateColor();
        }
    }
    
    public Color NonPlayerTurnColor
    {
        get => nonPlayerTurnColor;
        set
        {
            nonPlayerTurnColor = value;
            UpdateColor();
        }
    }
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 1;
    }
    
    
    public void SetValue(float value)
    {
        slider.value = value;
    }

    public void UpdateColor()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
        if(isPlayerTurn)
            slider.fillRect.GetComponent<Image>().color = playerTurnColor;
        else
            slider.fillRect.GetComponent<Image>().color = nonPlayerTurnColor;
        slider.colors = new ColorBlock()
        {
            normalColor = backgroundColor,
            highlightedColor = backgroundColor,
            pressedColor = backgroundColor,
            selectedColor = backgroundColor,
            disabledColor = backgroundColor,
        };
    }
    
    private void OnValidate()
    {
        UpdateColor();
    }
}