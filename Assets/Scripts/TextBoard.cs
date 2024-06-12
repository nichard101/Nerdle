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

    private WordGuess guess;
    
    private WordGuess[] previousGuesses;
    private GameMaster gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        previousGuesses = new WordGuess[5];
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();

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
            int[] result = gameMaster.EnterGuess(word);
            string output = "";
            foreach(int i in result){
                output += i + " ";
            }
            Debug.Log(output);
            
            guess.SetColors(result);
        }
    }
}
