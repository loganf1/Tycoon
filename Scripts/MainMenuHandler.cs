using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    private Button play_button, AI_properties_button;
    //private AIManager man;
    private GameObject AIMan;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Awoke Main Menu");
        AIManager.Init();
    }

    void Start()
    {
        Debug.Log("Started Main Menu");

        play_button = GameObject.Find("PlayButton").GetComponent<Button>();
        AI_properties_button = GameObject.Find("AIButton").GetComponent<Button>();

        play_button.onClick.AddListener(PlayButtonEvent);
        AI_properties_button.onClick.AddListener(AIButtonEvent);
        /*
        man = new AIManager(3);
        man.AddAI(0, new ComputerPlayer("AI_1_Jeff", ComputerPlayer.Difficulty.Easy));
        man.AddAI(1, new ComputerPlayer("AI_2_RoboCop", ComputerPlayer.Difficulty.Easy));
        man.AddAI(2, new ComputerPlayer("AI_3_Peter", ComputerPlayer.Difficulty.Easy));
        */
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AIButtonEvent()
    {
        Debug.Log("AI button clicked");
       // new AIHandler().Start();
        SceneManager.LoadScene(sceneName: "AIScene");
        /*
        Dropdown AI1, AI2, AI3;
        int AI1_diff, AI2_diff, AI3_diff;

        AI1 = GameObject.Find("Dropdown1").GetComponent<Dropdown>();
        AI2 = GameObject.Find("Dropdown2").GetComponent<Dropdown>();
        AI3 = GameObject.Find("Dropdown3").GetComponent<Dropdown>();

        AI1.onValueChanged.AddListener(delegate { Debug.Log("AI1 changed" + AI1.value); });
        AI2.onValueChanged.AddListener(delegate { Debug.Log("AI2 changed" + AI2.value); });
        AI3.onValueChanged.AddListener(delegate { Debug.Log("AI3 changed: " + AI3.value); });
        */
    }

    private void PlayButtonEvent()
    {
        Debug.Log("Play button clicked");
        SceneManager.LoadScene(sceneName: "GameScene");
        //GameController gc = new GameController(4);
        //gc.AddPlayer(new ComputerPlayer("AI_1_Jeff", ComputerPlayer.Difficulty.Easy));
        // Debug.Log(AIManager.Get(1).GetDiff().ToString());
        // gc.Test();
    }
}
