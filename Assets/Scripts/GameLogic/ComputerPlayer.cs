using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : Player
{
    public enum Difficulty { Easy, Medium, Hard };
    private Difficulty diff;

    public ComputerPlayer(string n, Difficulty d) : base(n)
    {
        diff = d;
    }

    public void SetDiff(Difficulty d)
    {
        diff = d;
    }

    public Difficulty GetDiff()
    {
        return diff;
    }

    public List<Card> PlayTurn(List<Card> lastPlayed, bool revolution)
    {
       // int i = 1;
        List<List<Card>> plays = GeneratePossiblePlays(lastPlayed, revolution);
        /*
        for (ArrayList<Card> play : plays){
            System.out.println("Possible play : " + i++ + "\n");
            for (Card c : play) {
                System.out.println(c);
            }
        }  */
        if (plays.Count == 0)
        {
            //System.out.println("Skip \n");
            return new List<Card>();
        }
        Dictionary<List<Card>, int> map = RankPlays(plays, lastPlayed, revolution);
        /*
        for (Map.Entry<ArrayList<Card>, Integer> e : map.entrySet()){
            System.out.println("Possible play : " + i++);
            for (Card c : e.getKey()) {
                System.out.println(c);
            }
            System.out.println("Score: : " + e.getValue() + "\n");
        }
        */
        return PlaySelect(map);
        //return getPlay(map);
    }

    private List<List<Card>> GeneratePossiblePlays(List<Card> lastPlayed, bool revolution)
    {
        List<List<Card>> plays = new List<List<Card>>();
        TycoonUtil.SortHand(hand);

        if (lastPlayed.Count == 4)
        {
            plays = Generate4CardPlays(lastPlayed);
        }

        if (lastPlayed.Count == 3)
        {
            plays = Generate3CardPlays(lastPlayed);
        }

        if (lastPlayed.Count == 2)
        {
            plays = Generate2CardPlays(lastPlayed);
        }

        if (lastPlayed.Count == 1)
        {
            plays = Generate1CardPlays(lastPlayed);
        }

        if (lastPlayed.Count == 0)
        {
            plays = Generate4CardPlays(lastPlayed);
            plays.AddRange(Generate3CardPlays(lastPlayed));
            plays.AddRange(Generate2CardPlays(lastPlayed));
            plays.AddRange(Generate1CardPlays(lastPlayed));
        }

        List<List<Card>> plays2 = new List<List<Card>>();
        foreach (List<Card> play in plays)
        {
            if (TycoonUtil.ValidatePlay(lastPlayed, play, revolution))
            {
                plays2.Add(play);
            }
        }
        return plays2;
    }

    private List<List<Card>> Generate4CardPlays(List<Card> lastPlayed)
    {
        List<List<Card>> plays = new List<List<Card>>();

        if (JokerCount(hand) == 1)
        {
            List<List<Card>> ps = Generate3CardPlaysJokerless(lastPlayed);
            foreach (List<Card> play in ps)
            {
                play.Add(hand[hand.Count - 1]);
            }
            plays.AddRange(ps);

        }
        else if (JokerCount(hand) == 2)
        {
            List<List<Card>> ps = Generate2CardPlaysJokerless(lastPlayed);
            foreach (List<Card> play in ps)
            {
                play.Add(hand[hand.Count - 1]);
                play.Add(hand[hand.Count - 2]);
            }
            plays.AddRange(ps);
        }

        foreach (Value v in Value.GetValues(typeof(Value)))
        {
            if (v != Value.JOKER)
            {
                List<Card> play = GetAll(v, 4);
                if (play.Count == 4)
                {
                    plays.Add(play);
                }
            }
        }

        return plays;
    }

    private List<List<Card>> Generate3CardPlays(List<Card> lastPlayed)
    {
        List<List<Card>> plays = new List<List<Card>>();

        if (JokerCount(hand) == 1)
        {
            List<List<Card>> ps = Generate2CardPlaysJokerless(lastPlayed);
            foreach (List<Card> play in ps)
            {
                play.Add(hand[hand.Count - 1]);
            }
            plays.AddRange(ps);

        }
        else if (JokerCount(hand) == 2)
        {
            List<List<Card>> ps = Generate1CardPlaysJokerless(lastPlayed);
            foreach (List<Card> play in ps)
            {
                play.Add(hand[hand.Count - 1]);
                play.Add(hand[hand.Count - 2]);
            }
            plays.AddRange(ps);
        }

        plays.AddRange(Generate3CardPlaysJokerless(lastPlayed));

        return plays;
    }

    private List<List<Card>> Generate3CardPlaysJokerless(List<Card> lastPlayed)
    {
        List<List<Card>> plays = new List<List<Card>>();

        foreach (Value v in Value.GetValues(typeof(Value)))
        {
            if (v != Value.JOKER)
            {
                List<Card> play = GetAll(v, 3);
                if (play.Count == 3)
                {
                    plays.Add(play);
                }
            }
        }

        return plays;
    }

    private List<List<Card>> Generate2CardPlays(List<Card> lastPlayed)
    {
        List<List<Card>> plays = new List<List<Card>>();

        if (JokerCount(hand) == 1)
        {

            List<List<Card>> ps = Generate1CardPlaysJokerless(lastPlayed);

            foreach (List<Card> play in ps)
            {
                play.Add(hand[hand.Count - 1]);
            }
            plays.AddRange(ps);

        }
        else if (JokerCount(lastPlayed) == 2)
        {
            List<Card> play = new List<Card>();
            play.Add(hand[hand.Count - 1]);
            play.Add(hand[hand.Count - 2]);
            plays.Add(play);
        }

        plays.AddRange(Generate2CardPlaysJokerless(lastPlayed));
        return plays;
    }

    private List<List<Card>> Generate2CardPlaysJokerless(List<Card> lastPlayed)
    {
        List<List<Card>> plays = new List<List<Card>>();

        foreach (Value v in Value.GetValues(typeof(Value)))
        {
            if (v != Value.JOKER)
            {
                List<Card> play = GetAll(v, 2);
                if (play.Count == 2)
                {
                    plays.Add(play);
                }
            }
        }

        return plays;
    }

    private List<List<Card>> Generate1CardPlays(List<Card> lastPlayed)
    {
        List<List<Card>> plays = new List<List<Card>>();

        if (JokerCount(hand) >= 1)
        {
            List<Card> play = new List<Card>();
            play.Add(hand[hand.Count - 1]);
            plays.Add(play);
        }

        plays.AddRange(Generate1CardPlaysJokerless(lastPlayed));

        return plays;
    }

    private List<List<Card>> Generate1CardPlaysJokerless(List<Card> lastPlayed)
    {
        List<List<Card>> plays = new List<List<Card>>();
        foreach (Value v in Value.GetValues(typeof(Value)))
        {
            if (v != Value.JOKER)
            {
                List<Card> play = GetAll(v, 1);

                if (play.Count == 1)
                {
                    plays.Add(play);
                }
            }
        }

        return plays;
    }

    private int JokerCount(List<Card> h)
    {
        int count = 0;
        foreach (Card c in h)
        {
            if (c.GetValue() == Value.JOKER)
            {
                count++;
            }
        }
        return count;
    }

    private List<Card> GetAll(Value v, int n)
    {
        List<Card> play = new List<Card>();
        int cnt = 0;
        foreach (Card c in hand)
        {
            if (c.GetValue() == v && cnt < n)
            {
                play.Add(c);

                cnt++;
            }
        }
        return play;
    }

    private Dictionary<List<Card>, int> RankPlays(List<List<Card>> plays, List<Card> lastPlayed, bool revolution)
    {
        Dictionary<List<Card>, int> map = new Dictionary<List<Card>, int>();

        foreach (List<Card> play in plays)
        {
            map[play] = RatePlay(play, lastPlayed, revolution);
        }
        return map;
    }

    private int RatePlay(List<Card> play, List<Card> lastPlayed, bool revolution)
    {
        int score = 20;
        TycoonUtil.SortHand(play);
        //closer to lastPlayed the better
        //using jokers bad (waste)
        int size = lastPlayed.Count;
        if (size > 0)
        {

            if (revolution)
            {
                //I droped the .ordinal here 
                score = 15 - (lastPlayed[0].GetValue() - play[0].GetValue());
            }
            else
            {
                score = 15 - (play[0].GetValue() - lastPlayed[0].GetValue());
            }

            if (size == 1 && play[0].GetValue() == Value.EIGHT)
            {
                score += 10;
            }

            score += 10 * (4 - JokerCount(play));
        }

        if (lastPlayed.Count == 0)
        {
            score += play.Count * 100;
            if (revolution)
            {
                score += 15 + (int) play[0].GetValue();
            }
            else
            {
                score += 15 - (int) play[0].GetValue();
            }

            score += 10 * (4 - JokerCount(play));
        }
        return score;
    }

    private List<Card> PlaySelect(Dictionary<List<Card> , int>  map)
    {
        System.Random r = new System.Random();
        int select = r.Next(10);
        if (map.Count < 3)
        {
            return RemovePlay(GetPlay(map));
        }
        List<Card> play1 = GetPlay(map);
        map.Remove(play1);
        List<Card> play2 = GetPlay(map);
        map.Remove(play2);
        List<Card> play3 = GetPlay(map);
        map.Remove(play3);

        if (diff == Difficulty.Easy)
        {
            if (select <= 6)
            {
                return RemovePlay(play3);
            }
            else if (select <= 8)
            {
                return RemovePlay(play2);
            }
            else
            {
                return RemovePlay(play1);
            }
        }
        else if (diff == Difficulty.Medium)
        {
            if (select <= 2)
            {
                return RemovePlay(play3);
            }
            else if (select <= 6)
            {
                return RemovePlay(play2);
            }
            else
            {
                return RemovePlay(play1);
            }
        }
        else
        {
            if (select <= 2)
            {
                return RemovePlay(play2);
            }
            else
            {
                return RemovePlay(play1);
            }
        }
    }


    private List<Card> GetPlay(Dictionary<List<Card>, int> map)
    {
        int highestScore = 0;
        List<Card> bestPlay = new List<Card>();

        foreach (KeyValuePair<List<Card>, int> e in map)
        {

            if (e.Value > highestScore)
            {
                highestScore = e.Value;
                bestPlay = e.Key;
            }
        }

        return bestPlay;
    }

    private List<Card> RemovePlay(List<Card> play)
    {
        foreach (Card c in play)
        {
            hand.Remove(c);
        }
        return play;
    }


}
