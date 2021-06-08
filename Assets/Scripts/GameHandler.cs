using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    private GameController gc = new GameController(4);

    Player lastPlayer = null;
    bool rev = false, SkipAllowed = false;
    List<Card> lastPlayed = new List<Card>();

    Text handDisp, AI1_Text, AI2_Text, AI3_Text, Prev, Errors;
    Button skipButton, nextRoundButton, playButton;
    
    List<Toggle> toggleList;
    Dictionary<int, string> AILastPlayed;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    void Start()
    {
        Debug.Log("GameHandler.start");

        handDisp = GameObject.Find("HandText").GetComponent<Text>();
        
        AI1_Text = GameObject.Find("AI1").GetComponent<Text>();
        AI2_Text = GameObject.Find("AI2").GetComponent<Text>();
        AI3_Text = GameObject.Find("AI3").GetComponent<Text>();
        Prev = GameObject.Find("Prev").GetComponent<Text>();
        Errors = GameObject.Find("Errors").GetComponent<Text>();
        skipButton = GameObject.Find("SkipButton").GetComponent<Button>();
        nextRoundButton = GameObject.Find("NextRoundButton").GetComponent<Button>();

        playButton = GameObject.Find("PlayButton").GetComponent<Button>();

        toggleList = new List<Toggle>() {
            GameObject.Find("Toggle1").GetComponent<Toggle>(),
            GameObject.Find("Toggle2").GetComponent<Toggle>(),
            GameObject.Find("Toggle3").GetComponent<Toggle>(),
            GameObject.Find("Toggle4").GetComponent<Toggle>(),
            GameObject.Find("Toggle5").GetComponent<Toggle>(),
            GameObject.Find("Toggle6").GetComponent<Toggle>(),
            GameObject.Find("Toggle7").GetComponent<Toggle>(),
            GameObject.Find("Toggle8").GetComponent<Toggle>(),
            GameObject.Find("Toggle9").GetComponent<Toggle>(),
            GameObject.Find("Toggle10").GetComponent<Toggle>(),
            GameObject.Find("Toggle11").GetComponent<Toggle>(),
            GameObject.Find("Toggle12").GetComponent<Toggle>(),
            GameObject.Find("Toggle13").GetComponent<Toggle>(),
            GameObject.Find("Toggle14").GetComponent<Toggle>()
        };
  
        skipButton.onClick.AddListener(SkipMethod);
        nextRoundButton.onClick.AddListener(NextRoundMethod);
        playButton.onClick.AddListener(InputMethod);

        playButton.interactable = false;

        AILastPlayed = new Dictionary<int, string>();
        AILastPlayed[0] = "";
        AILastPlayed[1] = "";
        AILastPlayed[2] = "";

        AddPlayers();
        StartGame();

    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void AddPlayers()
    {
        if (gc == null)
        {
            Debug.Log("Null 1 ");
        }
        if (AIManager.Get(0) == null)
        { 
            Debug.Log("Null 2 ");
        }
        gc.AddPlayer(AIManager.Get(0));
        gc.AddPlayer(AIManager.Get(1));
        gc.AddPlayer(AIManager.Get(2));
        gc.AddPlayer("me", false);
    }

    private void DisplayHand()
    {
        List<Card> hand = gc.GetHumanPlayer().GetHand();
        
        TycoonUtil.SortHand(hand);
        int i = 1;
        string s = "Hand: \n";
        foreach (Card c in hand)
        {
            s += i++ + ": ";
            s += c.ToString();
            s += "\n";
            
        }
        handDisp.text = s;
        
    }

    private void StartGame()
    {
        Debug.Log("Started Game");
        gc.StartRound();
        DisplayHand();

        PlayTurn();
       
    }

    private void NextRoundMethod()
    {
        if (gc.IsHandOver())
        {
            gc.StartNextRound();
            lastPlayer = null;
            lastPlayed = new List<Card>();
            rev = false;

            AILastPlayed[0] = "";
            AILastPlayed[1] = "";
            AILastPlayed[2] = "";

            DisplayHand();
            UpdateDisplays();

            PlayTurn();
        }
    }

    private void SkipMethod()
    {
        if (SkipAllowed)
        {
            PlayMyTurn("skip");

            UpdateDisplays();
            DisplayHand();

            //input.DeactivateInputField();
            playButton.interactable = false;
            SkipAllowed = false;

            PlayTurn();

        }
    }

    private void PlayTurn()
    {
        UpdateDisplays();
        if (gc.IsHandOver())
        {
            DisplayRoundResults();
            return;
        }

        Player p;
        do
        {
            p = gc.GetNextPlayer();
            if (p.GetStatus())
            {
                if (lastPlayer.Equals(p))
                {
                    //clear pile
                    lastPlayed = new List<Card>();
                }
                gc.IncNextPlayer();
            }
        } while (p.GetStatus());

        if (p is ComputerPlayer)
        {
            PlayAITurn();
        }
        else
        {
            SkipAllowed = true;
            //input.ActivateInputField();
            playButton.interactable = true;
        }
    }


    private void InputMethod()
    {
       // Debug.Log(s);

        if (gc.IsHandOver())
        {
            DisplayRoundResults();
            return;
        }
        
        Player p = gc.GetNextPlayer();
        // if out do something
        string s = DeterminePlayFromToggles();

        Debug.Log("PLAY FROM TOGGLES" + s);

        string[] inputs;
        int[] intInputs = new int[4];
        List<Card> turnHand;
        bool valid2 = false;
        
        
        bool valid = TycoonUtil.ValidateInputs(s, p.GetHandSize());
        if (valid)
        {
          
            inputs = p.GetInputs(s);
            if (inputs[0].Equals("skip"))
            {
                valid2 = true;
            }
            else
            {
                intInputs = TycoonUtil.SortInputs(inputs);

                turnHand = p.GetPlay(intInputs);
                if (lastPlayer != null && lastPlayer.Equals(gc.GetHumanPlayer())) {
                    valid2 = TycoonUtil.ValidatePlay(new List<Card>(), turnHand, rev);
                } else {
                    valid2 = TycoonUtil.ValidatePlay(lastPlayed, turnHand, rev);
                }
                
            }
        } else {
            Debug.Log("Invalid play bc syntax");
            Errors.text = "Invalid play because syntax";
        }

        if (valid2)
        {
            
            PlayMyTurn(s);

            UpdateDisplays();
            DisplayHand();
            //input.DeactivateInputField();
            playButton.interactable = false;
            SkipAllowed = false;
            
            //PlayOtherTurns();
           
            PlayTurn();
        }
        else {
            Debug.Log("Invalid play bc invalid play");
            Errors.text = "Invalid play";
        }

        //update

    }

    private void PlayMyTurn(string s)
    {
        System.Object[] arr = new System.Object[3];
       
        arr = gc.PlayATurn(lastPlayer, lastPlayed, rev, s );

        if (((List<Card>)arr[1]).Count != 0)
        {
            lastPlayer = (Player)arr[0];
            lastPlayed = (List<Card>)arr[1];
            rev = (bool)arr[2];

            if (lastPlayed[0].GetValue().Equals(Value.EIGHT))
            {

            } else
            {
                gc.IncNextPlayer();
            }
        } else
        {
            gc.IncNextPlayer();
        }
        ClearAllToggles();
        Errors.text = "";
        
        ////
        //////
        /////
        ///not always inc I e played 8
        ///or maybe make an 9 unbeatable 
        
    }

    private void PlayAITurn()
    {
        System.Object[] arr = new System.Object[3];

        Player p = gc.GetNextPlayer();
        // if out do something

        arr = gc.PlayATurn(lastPlayer, lastPlayed, rev, "");

        UpdateLastPlayed(p, (List<Card>)arr[1]);

        if (((List<Card>)arr[1]).Count != 0)
        {
            lastPlayer = (Player)arr[0];
            lastPlayed = (List<Card>)arr[1];
            rev = (bool)arr[2];
            
        }

        UpdateDisplays();
        //clear pile essentially
        if (lastPlayed.Count > 0 && lastPlayed[0].GetValue().Equals(Value.EIGHT))
        {
            lastPlayed = new List<Card>();
        } else
        {
            gc.IncNextPlayer();
        }
        
        PlayTurn();
    }

    private void PlayOtherTurns()
    {
        System.Object[] arr = new System.Object[3];

        if (gc.IsHandOver())
        {
            DisplayRoundResults();
            return;
        }

        Player p = gc.GetNextPlayer();
        // if out do something

        while (p is ComputerPlayer && !gc.IsHandOver())
        {
            
            arr = gc.PlayATurn(lastPlayer, lastPlayed, rev, "");
            if (((List<Card>)arr[1]).Count != 0) { 
                lastPlayer = (Player)arr[0];
                lastPlayed = (List<Card>)arr[1];
                rev = (bool)arr[2];
            }

            p = gc.GetNextPlayer();
            //if out do something
            UpdateDisplays();
            //clear pile essentially
            if (lastPlayed.Count > 0 && lastPlayed[0].GetValue().Equals(Value.EIGHT))
            {
                lastPlayed = new List<Card>();
            }
        }

        if (gc.IsHandOver())
        {
            DisplayRoundResults();
            return;
        }

        if (p is Player)
        {
            //input.ActivateInputField();
            playButton.interactable = true;
            SkipAllowed = true;
        }
        
    }

    private void UpdateDisplays()
    {
        List<Player> playerList = gc.GetPlayerList();
        //Debug.Log(playerList.Count);
        string s = "";
        
        s = playerList[0].GetName() + "\n" + "Hand Size: " + playerList[0].GetHandSize() + "\nLast Played: " + AILastPlayed[0];
        AI1_Text.text = s;
        s = playerList[1].GetName() + "\n" + "Hand Size: " + playerList[1].GetHandSize() + "\nLast Played: " + AILastPlayed[1];
        AI2_Text.text = s;
        s = playerList[2].GetName() + "\n" + "Hand Size: " + playerList[2].GetHandSize() + "\nLast Played: " + AILastPlayed[2];
        AI3_Text.text = s;

        if (lastPlayed.Count != 0)
        {
            if (rev) {
                s = "Revolution Active \n";
            }
            else
            {
                s = "";
            }

            s += "Last Played: " + lastPlayer.GetName() + "\n" + "Played: ";
            int i = 1;
            foreach (Card c in lastPlayed)
            {
                s += i++ + ": ";
                s += c.ToString();
                s += "\n";
            }
            Prev.text = s;
        }
    }

    private void DisplayRoundResults()
    {
        Debug.Log("dis round res");
       

        Player p1 = gc.GetPlayerWithRank(Rank.TYCOON);
        Player p2 = gc.GetPlayerWithRank(Rank.RICH);
        Player p3 = gc.GetPlayerWithRank(Rank.POOR);
        Player p4 = gc.GetPlayerWithRank(Rank.BEGGAR);

        string s = "Results: \nPlayer: " + p1.GetName() + " is Tycoon \n";
        s += "Player: " + p2.GetName() + " is Rich \n";
        s += "Player: " + p3.GetName() + " is Poor \n";
        s += "Player: " + p4.GetName() + " is Beggar \n\n";

        s += gc.GetScoreReport();

        if (gc.GetRoundCounter() >= 3)
        {
            s += "\n The Winner is: " + gc.GetWinner() + "!!!!"; 
        } else
        {
            s += "\nClick 'Next R' to begin next round";
        }

        Prev.text = s;
    }

    private string DeterminePlayFromToggles()
    {
        int cnt = 1;
        string s = "(";
        foreach (Toggle t in toggleList)
        {
            if (t.isOn)
            {
                s += cnt + ",";
            }
            cnt++;
        }

        if (s.Length > 1)
        {
            s = s.Substring(0,s.Length - 1);
            s += ")";
            return s;
        } else
        {
            return "";
        }
    }

    private void ClearAllToggles()
    {
        foreach (Toggle t in toggleList)
        {
            t.isOn = false;
        }
    }

    private void UpdateLastPlayed(Player p, List<Card> play)
    {
        string s = "";
        if (play.Count == 0)
        {
            s = "skip";
        }
        else
        {
            foreach (Card c in play)
            {
                s += c.GetValue().ToString() + ",";
            }
            s = s.Substring(0, s.Length - 1);
        }

        if (p.GetName().Equals("AI_1"))
        {
            AILastPlayed[0] = s;
        } else if (p.GetName().Equals("AI_2"))
        {
            AILastPlayed[1] = s;
        } else if (p.GetName().Equals("AI_3"))
        {
            AILastPlayed[2] = s;
        }
    }
}
