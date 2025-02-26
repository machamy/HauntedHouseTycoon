using UnityEngine;
using UnityEngine.UI;

[System.Flags]
public enum CardDirection
{
    None = 0,
    Up=1,
    Down=2,
    Left=4,
    Right=8,
}

public class CardUI : MonoBehaviour
{
    public CardDirection cardDirection;
    public bool start;
    public bool exit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created\
    public bool CanMove(CardDirection direction)
    {
        return (direction & cardDirection) != 0;
    
    }
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
