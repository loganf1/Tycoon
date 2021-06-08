using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;

public class TycoonUtil
{
    public static void SortHand(List<Card> hand)
    {
        // System.Collections.sort(hand);
        hand.Sort();
    }

    public static void Shuffle<T>(List<T> list)
    {
        System.Random ran = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ran.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    
    public static int[] SortInputs(string[] inputs)
    {
        int[] ins = new int[inputs.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            //Debug.Log(inputs[i]);
            ins[i] = Int32.Parse(inputs[i]);
        }
        Array.Sort(ins);
        return ins;
    }
    
    //no skips here
    public static bool ValidatePlay(List<Card> lastPlayed, List<Card> play, bool revolution)
    {
        
        //check play is Valid within itself
        if (play.Count > 1)
        {
            Card c1 = play[0];
            for (int i = 1; i < play.Count; i++)
            {
                Card c2 = play[i];

                if (c1.GetValue() != c2.GetValue() && c2.GetValue() != Value.JOKER)
                {
                    
                    return false;
                }
            }
        }

        //null check
        if (lastPlayed == null || lastPlayed.Count == 0)
        {
            return true;
        }

        //8-check
        if (lastPlayed[0].GetValue().Equals(Value.EIGHT))
        {
            
            return true;
        }

        //size check
        if (lastPlayed.Count != play.Count)
        {
            // System.out.print("Invalid because wrong size");
            
            return false;
        }

        //3 spade counter check
        if (!revolution && play.Count == 1 && play[0].Equals(new Card(Value.THREE, Suit.SPADE)) &&
            lastPlayed.Count == 1 && lastPlayed[0].Equals(new Card(Value.JOKER)))
        {
            //System.out.println("3-spade trump");
            return true;
        }

        //stronger check
        if (revolution)
        {
            Debug.Log("rev is true");
            if (lastPlayed[0].CompareTo(play[0]) <= 0)
            {
                // System.out.print("Invalid because wrong strength ");
                
                return false;
            }
        }
        else
        {
            if (lastPlayed[0].CompareTo(play[0]) >= 0)
            {
                // System.out.print("Invalid because wrong strength ");
                
                return false;
            }
        }

        return true;
    }

    public static bool ValidateInputs(string play, int handSize)
    {
        //manual regex
        Regex rx = new Regex("\\(((\\d+,?){1,4})\\)");
        string[] inputs = new string[4];

        bool valid = false;
        
        if (play.Equals("skip", StringComparison.InvariantCultureIgnoreCase))
        {
            inputs[0] = "skip";
            return true;//return inputs;
        }
        MatchCollection matches = rx.Matches(play);
        if (matches.Count > 0)
        {
            Debug.Log("play: " + play);
            
            string s1 = matches[0].Value;
            s1 = s1.Substring(1, s1.Length - 2);

            Debug.Log("s1: " + s1);
            inputs = s1.Split(',');
           

            valid = true;

            //within range check
            foreach (string s in inputs)
            {
                if (Int32.Parse(s) > handSize)
                {
                    valid = false;
                }
                Debug.Log("hand size issue");
            }

            //uniqueness test
            if (inputs.Length >= 2)
            {
                for (int i = 0; i < inputs.Length - 1; i++)
                {
                    for (int j = i + 1; j < inputs.Length; j++)
                    {
                        if (Int32.Parse(inputs[i]) == Int32.Parse(inputs[j]))
                        {
                            //System.out.println("Invalid Play -- Try Again:");
                            valid = false;
                        }
                    }
                }
            }
        }
       
        if (valid)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public static Queue<Rank> GenerateRankQueue()
    {
        Queue<Rank> ranks = new Queue<Rank>();
        ranks.Enqueue(Rank.TYCOON);
        ranks.Enqueue(Rank.RICH);
        ranks.Enqueue(Rank.POOR);
        ranks.Enqueue(Rank.BEGGAR);

        return ranks;
    }
    
}
