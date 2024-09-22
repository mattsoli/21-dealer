using UnityEngine;

 public enum Suit 
    { 
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum Value 
    { 
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 0)]
public class Card : ScriptableObject {
    public Suit suit;
    public Value value;
    public GameObject model;

    public override string ToString()
    {
        return suit + ":" + value;
    }
}

