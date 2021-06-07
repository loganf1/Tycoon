using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameIO : ScriptableObject
{
    Text handDisp;
    InputField input;
    private string lastInput;
    public GameIO()
    {
        //handDisp = GameObject.Find("HandText").GetComponent<Text>();
    }

    public void Awake()
    {
        handDisp = GameObject.Find("HandText").GetComponent<Text>();
        input = GameObject.Find("InputField").GetComponent<InputField>();
        input.onEndEdit.AddListener(InputMethod);
    }

    public void DisplayHand(List<Card> hand)
    {
        TycoonUtil.SortHand(hand);
        int i = 1;
        string s = "";
        foreach (Card c in hand)
        {
            s += i++ + ": ";
            s += c.ToString();
            s += "\n";

        }
        handDisp.text = s;
        // handDisp.
        Debug.Log(s);
    }

    public string GetInput()
    {
        //need to wait 
       // while (lastInput == null)
       // {
            
        //}
        return lastInput;
    }

    private void InputMethod(string s)
    {
        lastInput = s;
    }
 
}
