using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Text textPrefab;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Animator anim;
    private GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gm.GetGameOver()){
            
        }
    }

    public void UpdateText(){
        int guesses = gm.GetNumGuesses();
        string message = " Guesses Left";
        if(guesses == 5){
            message = " Guess Left";
        }
        textPrefab.text = (6-guesses) + message;
    }

    public void Reset(){
        buttonPrefab.gameObject.SetActive(false);
    }

    public void GameWin(){
        textPrefab.text = "You Win!";
        buttonPrefab.gameObject.SetActive(true);
    }

    public void InvalidWord(){
        textPrefab.text = "Not a valid word";
        anim.Play("TextFlash", -1, 0f);
    }

    public void GameLose(string answer){
        textPrefab.text = "Answer: " + answer.Substring(0,1).ToUpper() + answer.Substring(1);
        buttonPrefab.gameObject.SetActive(true);
    }
}
