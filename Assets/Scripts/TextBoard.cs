using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoard : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private WordGuess wordGuessPrefab;
    [SerializeField] private Color[] colorList;
    [SerializeField] private float xSpacing;
    [SerializeField] private Animator anim;

    private WordGuess guess;
    private int numGuesses;
    
    private WordGuess[] previousGuesses;
    private GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        numGuesses = 0;
        previousGuesses = new WordGuess[5];
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        guess = Instantiate(wordGuessPrefab, rectTransform);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void AddToWord(char letter){
        guess.AddToWord(letter);
    }

    public void Backspace(){
        guess.Backspace();
    }

    public void EnterGuess(){
        string word = guess.GetWord();
        if(word.Length == 5){
            gm.EnterGuess(word);
        }
    }

    public void GuessChecked(int[] result){
        guess.SetColors(result);
        guess.SetInactive();
        numGuesses++;

        bool win = CheckIfWin(result);
        if(!win){
            if(numGuesses == 6){
                gm.GameLose();
            } else {
                previousGuesses[numGuesses-1] = guess;
                MoveAllUp();
                guess = Instantiate(wordGuessPrefab, rectTransform);
            }
        } else {
            gm.GameWin();
        }
    }

    public bool CheckIfWin(int[] result){
        foreach(int i in result){
            if(i != 3){
                return false;
            }
        }
        return true;
    }

    public int GetNumGuesses(){
        return numGuesses;
    }

    public void MoveAllUp(){
        for(int i = 0; i < numGuesses; i++){
            previousGuesses[i].MoveUp();
        }
    }
}
