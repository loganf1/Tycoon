using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIManager 
{
    //Stores the informartion for the AIs
    //private static Dictionary<int, ComputerPlayer> AIs = new Dictionary<int, ComputerPlayer>();
    private static List<ComputerPlayer> AIPlayers = new List<ComputerPlayer>();
    private static bool Started = false;
    /*
    public AIManager (int AINum)
    {
        AIs = new Dictionary<int, ComputerPlayer>();
    }
    */

    public static void Init()
    {
        if (Started) return;

        AIPlayers.Add(new ComputerPlayer("AI_1", ComputerPlayer.Difficulty.Easy));
        AIPlayers.Add(new ComputerPlayer("AI_2", ComputerPlayer.Difficulty.Easy));
        AIPlayers.Add(new ComputerPlayer("AI_3", ComputerPlayer.Difficulty.Easy));
        Started = true;
    }

    public static void AddAI(int num, ComputerPlayer p)
    {
        AIPlayers[num] = p;
    }

    public static void UpdateAI(int num, ComputerPlayer.Difficulty newDiff)
    {
        AIPlayers[num].SetDiff(newDiff);
    }

    public static ComputerPlayer Get(int num)
    {
        return AIPlayers[num];
    }
}
