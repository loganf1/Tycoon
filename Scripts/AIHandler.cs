using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AIHandler : MonoBehaviour
{
    private Dropdown AI1, AI2, AI3;
    private Button returnBut;
    private GameObject AIMan;
    int AI1_diff, AI2_diff, AI3_diff;

    // Start is called before the first frame update
    //probably not running this script when switching to this scene
    void Start()
    {
        Debug.Log("Started AI Menu");
        AI1 = GameObject.Find("Dropdown1").GetComponent<Dropdown>();
        AI2 = GameObject.Find("Dropdown2").GetComponent<Dropdown>();
        AI3 = GameObject.Find("Dropdown3").GetComponent<Dropdown>();

        SetButtonValues();

        returnBut = GameObject.Find("ReturnButton").GetComponent<Button>();
        AIMan = GameObject.Find("AIManager");
        //AIMan

        returnBut.onClick.AddListener(ReturnButtonEvent);
        AI1.onValueChanged.AddListener(delegate { AIButton1(); });
        AI2.onValueChanged.AddListener(delegate { AIButton2(); });
        AI3.onValueChanged.AddListener(delegate { AIButton3(); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ReturnButtonEvent()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void AIButton1()
    {
        Debug.Log("AI1 changed" + AI1.value);
        switch (AI1.value)
        {
            case 0:  AIManager.UpdateAI(0, ComputerPlayer.Difficulty.Easy); break;
            case 1: AIManager.UpdateAI(0, ComputerPlayer.Difficulty.Medium); break;
            case 2: AIManager.UpdateAI(0, ComputerPlayer.Difficulty.Hard); break;
        }
        AI1.RefreshShownValue();
    }

    private void AIButton2()
    {
        Debug.Log("AI2 changed" + AI2.value);
        switch (AI2.value)
        {
            case 0: AIManager.UpdateAI(1, ComputerPlayer.Difficulty.Easy); break;
            case 1: AIManager.UpdateAI(1, ComputerPlayer.Difficulty.Medium); break;
            case 2: AIManager.UpdateAI(1, ComputerPlayer.Difficulty.Hard); break;
        }
        AI2.RefreshShownValue();
    }

    private void AIButton3()
    {
        Debug.Log("AI3 changed: " + AI3.value);
        switch (AI3.value)
        {
            case 0: AIManager.UpdateAI(2, ComputerPlayer.Difficulty.Easy); break;
            case 1: AIManager.UpdateAI(2, ComputerPlayer.Difficulty.Medium); break;
            case 2: AIManager.UpdateAI(2, ComputerPlayer.Difficulty.Hard); break;
        }
        AI3.RefreshShownValue();
    }

    private void SetButtonValues()
    {
        AI1.value = (int) AIManager.Get(0).GetDiff();
        AI2.value = (int)AIManager.Get(1).GetDiff();
        AI3.value = (int)AIManager.Get(2).GetDiff();
    }
}
