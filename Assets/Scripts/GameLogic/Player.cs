using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Player 
{
    private string name;
    protected List<Card> hand;
    private bool isOut;
    private Rank rank;


    public Player(string n)
    {
        name = n;
        hand = new List<Card>();
        isOut = false;
    }

    
    public bool GetStatus()
    {
        return isOut;
    }

    public int GetHandSize()
    {
        return hand.Count;
    }

    public List<Card> GetHand()
    {
        return hand;
    }

    public void SetOut(bool s)
    {
        isOut = s;
    }
    
    public void SetRank(Rank r)
    {
        rank = r;
    }
    
    public Rank GetRank()
    {
        return rank;
    }

    public string GetName()
    {
        return name;
    }

    public override bool Equals(System.Object o)
    {
        if (o != null && GetType() == o.GetType()){
            Player p = ((Player)o);
            return (name.Equals(p.GetName()));
        }
        return false;
    }

    public void AddCardToHand(Card c)
    {
        hand.Add(c);
    }

    public void ClearHand()
    {
        hand.Clear();
    }

    public void DisplayHand()
    {
        TycoonUtil.SortHand(hand);
        int i = 1;
        string s = "Hand: \n";
        foreach (Card c in hand)
        {
            s += i++ + ": ";
            s += c.ToString();
            s += "\n";
            //Debug.Log(i++ + ": ");
            //Debug.Log(c);
            //System.out.print(i++ + ": ");
            //System.out.println(c);
        }
        Debug.Log(s);
    }
    
    public bool IsHandEmpty()
    {
        return hand.Count == 0;
    }

    
    public List<Card> PlayTurn(List<Card> lastPlayed, bool revolution, string play)
    {
        List<Card> turnHand = new List<Card>();
        string[] inputs = new string[4];
        int[] intInputs = new int[4];
        //this.DisplayHand();

        if (revolution)
        {
            //System.out.println("Revolution is Active!");
        }
          
        inputs = GetInputs(play);
        if (inputs[0].Equals("skip", StringComparison.InvariantCultureIgnoreCase))
        {
            return new List<Card>();
        }
        intInputs = TycoonUtil.SortInputs(inputs);
           
        turnHand = GetPlay(intInputs);
      
        for (int i = intInputs.Length - 1; i >= 0; i--)
        {
            hand.RemoveAt(intInputs[i] - 1);
        }

        return turnHand;
    }
    
    public string[] GetInputs(string play)
    {
       
        //manual regex
        Regex rx = new Regex("\\(((\\d+,?){1,4})\\)");
        //Pattern p = Pattern.compile("\\(((\\d+,?){1,4})\\)");
        string[] inputs = new string[4];
       
        if (play.Equals("skip", StringComparison.InvariantCultureIgnoreCase))
        {
            inputs[0] = "skip";
            return inputs;
        }
        MatchCollection matches = rx.Matches(play);
        //m = p.matcher(play);
        if (matches.Count > 0)
        {
            //GroupCollection groups = matches[0].Groups;

            string s1 = matches[0].Value;
            s1 = s1.Substring(1, s1.Length - 2);
            //System.out.println("your play: " + s1);

            inputs = s1.Split(',');
        }  

        return inputs;
    }

    public List<Card> GetPlay(int[] inputs)
    {
        List<Card> turnHand = new List<Card>();

        for (int i = 0; i < inputs.Length; i++)
        {
            turnHand.Add(hand[inputs[i] - 1]);
        }
        foreach (Card c in turnHand)
        {
            //System.out.println(c.getValue());
        }
        return turnHand;
    }
    
    public List<Card> GetBest(int numCards)
    {
        TycoonUtil.SortHand(hand);
        List<Card> cards = new List<Card>();

        for (int i = 0; i < numCards; i++)
        {
            Card c = hand[hand.Count - 1];
            hand.RemoveAt(hand.Count - 1);
            cards.Add(c);
            //cards.Add(hand.remove(hand.size() - 1));
        }

        return cards;
    }

    public List<Card> GetWorst(int numCards)
    {
        TycoonUtil.SortHand(hand);
        List<Card> cards = new List<Card>();

        for (int i = 0; i < numCards; i++)
        {
            Card c = hand[0];
            hand.RemoveAt(0);
            cards.Add(c);
            //cards.Add(hand.remove(0));
        }

        return cards;
    }
    
}
