using System.Collections;
using System.Collections.Generic;
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
        Ace,
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

[CreateAssetMenu(fileName = "Card", menuName = "21-dealer/Card", order = 0)]
public class Card : ScriptableObject {
    public Suit suit;
    public Value value;
    public GameObject cardModel;
}

