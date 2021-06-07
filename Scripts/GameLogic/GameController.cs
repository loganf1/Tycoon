using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameController
{
    private int numPlayers, dealer, nextPlayer, roundCounter;
    private List<Player> playerList;

    private Dictionary<Player, int> scores;
    private Player lastWinner;
    private Queue<Rank> ranks;

   // private GameHandler gh;

    public GameController(int playerNum)//, GameHandler gamehandler )
    {
        numPlayers = playerNum;
        playerList = new List<Player>();
       
        dealer = new System.Random().Next(numPlayers);
        
        scores = new Dictionary<Player, int>();
        lastWinner = null;

        roundCounter = 0;

    }

    public bool AddPlayer(Player p)
    {
        if (false)
        {
            //check for dup names
        }
        playerList.Add(p);
        scores[p] = 0;
        return true;
    }

    public bool AddPlayer(String name, bool isAI)
    {
        Player p;
        if (false)
        {

        }
        if (isAI)
        {
            p = new ComputerPlayer(name, ComputerPlayer.Difficulty.Easy);
        } else
        {
            p = new Player(name);
        }
        return AddPlayer(p);
    }

    
    public void Deal()
    {
        //choose dealer
        // if first round then random
        //otherwise deal in order of rank
        if (lastWinner == null)
        {
            //TycoonUtil.Shuffle(playerList);
            //Collections.shuffle(playerList);
        }
        else
        {/*
            List<Player> gamers = new List<Player>
            {
                GetPlayerWithRank(Rank.TYCOON),
                GetPlayerWithRank(Rank.RICH),
                GetPlayerWithRank(Rank.POOR),
                GetPlayerWithRank(Rank.BEGGAR)
            };
            playerList = gamers; */
        }
        Deck deck = new Deck();

        for (int i = 0; i < 13; i++)
        {
            foreach (Player p in playerList)
            {
                p.AddCardToHand(deck.Draw());
            }
        }

        playerList[(dealer + 1 )% 4].AddCardToHand(deck.Draw());
        playerList[(dealer + 2) % 4].AddCardToHand(deck.Draw());

        //dealer = 0;
        dealer = (dealer + 1) % numPlayers;
        nextPlayer = dealer;
    }

    public void Deal2()
    {
        //choose dealer
        /*
        Deck deck = new Deck();
        for (int i = 0; i < 1; i++)
        {
            foreach (Player p in playerList)
            {
                p.AddCardToHand(deck.Draw());
            }
        }

        playerList[0].AddCardToHand(deck.Draw());
        playerList[1].AddCardToHand(deck.Draw());

        dealer = (dealer + 1) % numPlayers;
        nextPlayer = dealer;
        */

        playerList[0].AddCardToHand(new Card(Value.FOUR, Suit.DIAMOND));
        playerList[1].AddCardToHand(new Card(Value.FOUR, Suit.HEART));
        playerList[2].AddCardToHand(new Card(Value.FOUR, Suit.SPADE));

        playerList[3].AddCardToHand(new Card(Value.EIGHT, Suit.DIAMOND));
        playerList[3].AddCardToHand(new Card(Value.EIGHT, Suit.SPADE));
        playerList[3].AddCardToHand(new Card(Value.FIVE, Suit.DIAMOND));

        nextPlayer = 3;

    }

    public List<Player> GetPlayerList()
    {
        return playerList;
    }
    
    public bool IsHandOver()
    {
        int count = 0;
        foreach (Player p in playerList)
        {
            if (p.GetStatus())
            {
                count++;
            }
        }
        return numPlayers - count <= 1;
    }

    private void UpdateScores()
    {
        //scores.ReplaceAll((key, oldValue) => oldValue + key.getRank().getNum());

        List<Player> keys = new List<Player>(scores.Keys);
        foreach ( Player p in keys)
        {
            scores[p] = scores[p] + (int)p.GetRank();
        } 
    }

    public string GetScoreReport()
    {
        UpdateScores();

        string s = "Scores: \n";

        foreach (KeyValuePair<Player, int> p in scores)
        {
            s += p.Key.GetName() + " : " + p.Value + "\n";
        }

        return s;
        
    }

    private void ResetScores()
    {
        //scores.replaceAll((key, oldValue)-> 0);

        foreach (Player p in new List <Player>(scores.Keys))
        {
            scores[p] = 0;
        }
    }

    private void HandReset()
    {
        foreach (Player p in playerList)
        {
            p.SetOut(false);
            p.ClearHand();
        }
    }

    private void CardAdjust()
    {
        Player pT = null, pR = null, pP = null, pB = null;
        foreach (Player p in playerList)
        {
            if (p.GetRank() == Rank.TYCOON)
            {
                pT = p;
            }
            else if (p.GetRank() == Rank.RICH)
            {
                pR = p;
            }
            else if (p.GetRank() == Rank.POOR)
            {
                pP = p;
            }
            else
            {
                pB = p;
            }
        }

        CardSwap(pT, pB, 2);
        CardSwap(pR, pP, 1);
    }

    private void CardSwap(Player richer, Player poorer, int numCards)
    {
        List<Card> cards = poorer.GetBest(2);
        List<Card> cards2 = richer.GetWorst(2);

        foreach (Card c in cards)
        {
            richer.AddCardToHand(c);
        }

        foreach (Card c in cards2)
        {
            poorer.AddCardToHand(c);
        }
    }

    private void DisplayPlayerStatuses(Player curr)
    {
        foreach (Player p in playerList)
        {
            if (!p.Equals(curr))
            {
                if (p.GetStatus())
                {
                    //System.out.println(p.getName() + " is out Rank: " + p.getRank());
                    Debug.Log(p.GetName() + " is out Rank: " + p.GetRank());
                }
                else
                {
                    //System.out.println(p.getName() + " has " + p.getHandSize() + " cards left!");
                    Debug.Log(p.GetName() + " has " + p.GetHandSize() + " cards left!");
                }
            }
        }
        //System.out.println("");\
        //Debug.Log("");
    }
    
    public Player GetPlayerWithRank(Rank r)
    {
        foreach (Player p in playerList)
        {
            if (p.GetRank() == r)
            {
                return p;
            }
        }
        return null;
    }


    //****************************************//


    public void StartRound()
    {
        ranks = TycoonUtil.GenerateRankQueue();
        ResetScores();
        HandReset();
        roundCounter++;
        Deal();
       // Deal2();
    }

    public void StartNextRound()
    {
        if (roundCounter >= 3)
        {
            Application.Quit();
        }
        else
        {
            ranks = TycoonUtil.GenerateRankQueue();
            // UpdateScores();
            HandReset();
            Deal();
            CardAdjust();
            roundCounter++;
        }
    }

    
    public Player GetNextPlayer()
    {
        return playerList[nextPlayer];
    }

    public void IncNextPlayer()
    {
        nextPlayer = (nextPlayer + 1) % 4;
    }

    public Player GetHumanPlayer()
    {
        Player ret = null;
        foreach (Player p in playerList) {
            if( p.GetType() != typeof(ComputerPlayer))
            {
                ret = p;
            } 
        }
        return ret;
    }

    public System.Object[] PlayATurn(Player lastPlayer, List<Card> lastPlayed, bool rev, string userplay)
    {
        int playerTurn = nextPlayer;
        Player p = playerList[playerTurn];
        List<Card> play = new List<Card>();

        //Clear pile if needed
        if (p.Equals(lastPlayer))
        {
            //System.out.println("Clear Pile");
            Debug.Log("Clear Pile");
            lastPlayed.Clear();
        }
        //return null;
       
        //check if out
        if (!p.GetStatus())
        {
            //display card status
            if (p.GetType() == typeof(Player)) 
            {
                DisplayPlayerStatuses(p);
            }
            //Debug.Log("in gc p is " + p.GetName());
            if (p is ComputerPlayer)
            {
                play = ((ComputerPlayer) p).PlayTurn(lastPlayed, rev);
            } else
            {
                play = p.PlayTurn(lastPlayed, rev, userplay);
            }
            //play = p.PlayTurn(lastPlayed, rev, gameIO);
            string s = "";
            
            foreach (Card c in play)
            {
                s += c.GetValue() + " suit: " + c.GetSuit() + "\n";
            }
            Debug.Log(p.GetName() + " played: \n" + s);

            //update last and handle 8 and 3-spade
            if (play.Count != 0)
            {
                //handle 8
                if (play[0].GetValue() == Value.EIGHT)
                {
                    playerTurn -= 1; //lets player go again
                }
                else if (play.Count == 1 && play[0].Equals(new Card(Value.THREE, Suit.SPADE)) &&
                        lastPlayed.Count == 1 && lastPlayed[0].Equals(new Card(Value.JOKER)))
                {
                    playerTurn -= 1; //lets player go again
                }
                else
                {
                    lastPlayed = play;
                }
                if (play.Count == 4)
                {
                    //revolution
                    rev = !rev;
                }
               
                lastPlayer = p;
            }
            //get players out
            if (p.IsHandEmpty())
            {
                p.SetOut(true);

                p.SetRank(ranks.Dequeue());
                Debug.Log(p.GetName() + " is out with rank: " + p.GetRank().ToString());
                //System.out.println(p.GetName() + " is out! Rank: " + p.GetRank());
                if (p.GetRank() == Rank.TYCOON)
                {
                    if (lastWinner != null && lastWinner != p)
                    {
                        lastWinner.SetOut(true);
                        lastWinner.SetRank(Rank.BEGGAR);
                        //System.out.println(lastWinner.GetName() + " is out! Rank: " + lastWinner.GetRank());
                    }
                    lastWinner = p;
                }
                ///if num out = 3
                if (NumOut() == 3)
                {
                    Player p4 = NotOutPlayer();
                    p4.SetOut(true);
                    p4.SetRank(ranks.Dequeue());
                    Debug.Log(p4.GetName() + " is out with rank: " + p4.GetRank().ToString());
                }
                if (p.GetRank() == Rank.POOR)
                {

                }
            }
        }
        // change this so outed players turns are considered in order to clear pile
        /*
        Player p2;
        do {
            Debug.Log("loop check");
            playerTurn = (playerTurn + 1) % numPlayers; //update player
            p2 = playerList[playerTurn];
            //CHECKKKKKK
        } while (!(p2 is ComputerPlayer) && p2.GetStatus());

        nextPlayer = playerTurn;    
        */
        System.Object [] ret =  new System.Object[3];
        ret[0] = p;
        ret[1] = play;
        ret[2] = rev;
        return ret;
    }

    private int NumOut ()
    {
        int cnt = 0;
        foreach (Player p in playerList) {
            if (p.GetStatus())
            {
                cnt++;
            }
        }
        return cnt;
    }

    private Player NotOutPlayer()
    {
        foreach (Player p in playerList)
        {
            if (!p.GetStatus())
            {
                return p;
            }
        }
        return null;
    }
}