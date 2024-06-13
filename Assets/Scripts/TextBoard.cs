using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextBoard : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private WordGuess wordGuessPrefab;
    //[SerializeField] private Color[] colorList;
    [SerializeField] private float xSpacing;
    [SerializeField] private Animator anim;
    [SerializeField] private Image im;

    private WordGuess guess;
    
    private List<WordGuess> previousGuesses;
    private GameMaster gm;
    private Vector2 guessPos;

    // Start is called before the first frame update
    void Start()
    {
        float yPos = 0f;
        guessPos = new Vector2(0f, yPos);
        previousGuesses = new List<WordGuess>();
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        im = GetComponent<Image>();

        guess = Instantiate(wordGuessPrefab, rectTransform);
        //guess.GetComponent<RectTransform>().position = rectTransform.position + guessPos;
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

        bool win = CheckIfWin(result);
        if(!win){
            if(gm.GetNumGuesses() == 6){
                gm.GameLose();
            } else {
                previousGuesses.Add(guess);
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
        return gm.GetNumGuesses();
    }

    public void SetGuessHistory(char[][] letters, int[][] colors){
        for(int i = 0; i < letters.Length; i++){
            WordGuess tempGuess = Instantiate(wordGuessPrefab);
            for(int j = 0; j < 5; j++){
                tempGuess.AddToWord(letters[i][j]);
            }
            tempGuess.SetColors(colors[i]);
            guess = tempGuess;
            previousGuesses.Add(tempGuess);
            if(i < 5){
                MoveAllUp();
            }
        }
    }

    public GuessHistoryStruct GetGuessHistory(){
        GuessHistoryStruct output = new GuessHistoryStruct();
        int[][] colorHistory = new int[previousGuesses.Count][];
        char[][] guessHistory = new char[previousGuesses.Count][];
        for(int i = 0; i < colorHistory.Length; i++){
            colorHistory[i] = previousGuesses[i].GetColors();
            guessHistory[i] = previousGuesses[i].GetWord().ToCharArray();
        }
        return output;
    }

    public void MoveAllUp(){
        for(int i = 0; i < gm.GetNumGuesses(); i++){
            previousGuesses[i].MoveUp();
        }
    }

    public void Reset(){
        SceneManager.LoadScene("game");
    }
}

public struct GuessHistoryStruct{
    public char[][] guesses;
    public int[][] colors;
}
