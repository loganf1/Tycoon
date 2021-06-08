using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> deckList;

    public Deck()
    {
        deckList = new List<Card>();
        foreach (Value v in Value.GetValues(typeof(Value)))
        {
            if (v != Value.JOKER)
            {
                foreach (Suit s in Suit.GetValues(typeof(Suit)))
                {
                    deckList.Add(new Card(v, s));
                }
            }
        }
        deckList.Add(new Card(Value.JOKER));
        deckList.Add(new Card(Value.JOKER));

        TycoonUtil.Shuffle(deckList);
    }

    public Card Draw()
    {
        Card c = deckList[0];
        deckList.RemoveAt(0);
        return c;
    }
}
