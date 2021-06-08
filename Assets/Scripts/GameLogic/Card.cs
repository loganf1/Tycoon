using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Card : IComparable<Card>
{
    private Value value;
    private Suit suit;

    public Card(Value val, Suit s)
    {
        value = val;
        suit = s;
    }

    public Card(Value val)
    {
        value = val;
    }

    public override string ToString()
    {
        if (value == Value.JOKER)
        {
            return "Value: " + value.ToString();
        }
        return "Value: " + value.ToString() + " Suit: " + suit.ToString();
    }

    public Value GetValue()
    {
        return value;
    }

    public Suit GetSuit()
    {
        return suit;
    }
    
    public override bool Equals(System.Object o)
    {
        if (o != null && GetType() == o.GetType()){
            Card c = ((Card) o);
            return (value == c.GetValue() && suit == c.GetSuit());
        }
        return false;
    }
    
    
    public int CompareTo(Card c)
    {
        return this.value.CompareTo(c.value);
    }
    
}
