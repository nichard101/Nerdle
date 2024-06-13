using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private TextBoard boardPrefab;
    [SerializeField] private Keyboard keyboardPrefab;
    [SerializeField] private HUDManager hud;
    [SerializeField] private Color[] colorList;
    private string[] wordList;
    private int[] shuffled;
    private string currentWord;
    private int currentIndex;
    private int numGuesses;
    private bool isGameOver;
    private GuessHistoryStruct guessHistory;
    private Dictionary<int, int> scoreList;

    void Start(){
        wordList = File.ReadAllLines("Assets/sgb-words.txt");
        //Save();
        //currentWord = "sofia";
            
        if(!Load()){    // if the load fails for whatever reason, generate a blank slate
            scoreList = new Dictionary<int, int>();
            shuffled = ShuffleList(wordList.Length);
            scoreList.Add(0,0);
            scoreList.Add(1,0);
            scoreList.Add(2,0);
            scoreList.Add(3,0);
            scoreList.Add(4,0);
            scoreList.Add(5,0);
            scoreList.Add(6,0);
            currentIndex = 0;
            numGuesses = 0;
        }
        if(currentIndex == wordList.Length){            // if we reach the end of the random list, reroll it all and start again
            shuffled = ShuffleList(wordList.Length);
            currentIndex = 0;
            Save();
        }
        currentWord = wordList[shuffled[currentIndex]];
    }

    private void Save(){
        string test = "hello";
        string shuffleOrder = IntArrToString(shuffled);
        string[] saveContents = new string[]{
            ""+IntArrToString(shuffled),
            ""+currentIndex,
            ""+(0 + " " + scoreList[0] + " " + 1 + " " + scoreList[1] + " " + 2 + " " + scoreList[2] + " " + 3 + " " + scoreList[3] + " " + 4 + " " + scoreList[4] + " " + 5 + " " + scoreList[5] + " " + 6 + " " + scoreList[6])
        };
        string saveString = string.Join("|", saveContents);
        File.WriteAllText(Application.dataPath + "/save.txt", saveString);
        Debug.Log("Saved!");
    }

    private bool Load(){
        try{
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            string[] saveArray = saveString.Split("|");
            shuffled = StringToIntArr(saveArray[0]); // shuffled word order
            currentIndex = int.Parse(saveArray[1]);  // current word index
            FillDictionary(saveArray[2]);            // score history
            Debug.Log("Loaded!");
            return true;
        } catch(Exception e){
            Debug.Log("Load failed!");
            return false;
        }
        
    }

    private void FillDictionary(string input){
        string[] array = input.Split(" ");
        scoreList = new Dictionary<int, int>();
        for(int i = 1; i < array.Length; i+=2){
            scoreList.Add(int.Parse(array[i-1]), int.Parse(array[i]));
        }
    }

    private int[] ShuffleList(int length){
        int[] intArray = new int[length];
        for(int i = 0; i < length; i++){
            intArray[i] = i;
        }

        for (int t = 0; t < length; t++ )
        {
            int tmp = intArray[t];
            int r = UnityEngine.Random.Range(t, length);
            intArray[t] = intArray[r];
            intArray[r] = tmp;
        }
        return intArray;
    }

    private bool IsValidWord(string word){
        foreach(string w in wordList){
            if(w.ToUpper().Equals(word.ToUpper())){
                return true;
            }
        }
        return false;
    }

    public void EnterGuess(string g){
        if(!IsValidWord(g)){
            hud.InvalidWord();
        } else {
            numGuesses++;
            StringBuilder answer = new StringBuilder(currentWord.ToUpper());
            StringBuilder guess = new StringBuilder(g.ToUpper());
            
            int[] result = {1,1,1,1,1}; // all positions start as grey
            Debug.Log(answer + " " + guess);

            // green checks
            int i = 0;
            int counter = 0;

            while(i < 5){
                Debug.Log(answer + " " + answer.Length);
                char a = answer[i];
                char b = guess[i];
                if(a == b && a != ' '){
                    answer[i] = ' ';
                    guess[i] = ' ';
                    result[counter] = 3; // sets this position to green
                }
                i++;
                
                counter++;
            }

            // yellow checks
            i = 0;
            counter = 0;
            while(i < 5){
                char a = guess[i];
                if(a != ' '){
                    int index = FindIndexOfLetter(answer.ToString(), a);
                    if(index != -1){
                        answer[index] = ' ';
                        guess[i] = ' ';
                        if(result[counter] == 1){
                            result[counter] = 2; // sets this position to yellow if not already green
                        }
                    } else {
                    
                    }
                }
                i++;
                counter++;
            }
            Save();
            hud.UpdateText();
            boardPrefab.GuessChecked(result);
            keyboardPrefab.UpdateColors(g, result);
        }
    }

    private int FindIndexOfLetter(string word, char letter){
        for(int i = 0; i < word.Length; i++){
            if(word[i] == letter){
                return i;
            }
        }
        return -1;
    }

    public void ResetGame(){
        isGameOver = false;
        keyboardPrefab.ResetColors();
        boardPrefab.Reset();
    }

    public void GameLose(){
        isGameOver = true;
        hud.GameLose(currentWord);
        scoreList[0] += 1;
        Debug.Log(scoreList[0]);
        currentIndex++;
        Save();
    }

    public void GameWin(){
        isGameOver = true;
        hud.GameWin();
        scoreList[numGuesses] += 1;
        currentIndex++;
        Save();
    }

    public bool GetGameOver(){
        return isGameOver;
    }

    public int GetNumGuesses(){
        return numGuesses;
    }

    public Color GetColor(int index){
        return colorList[index];
    }

    public Dictionary<int, int> GetScores(){
        return scoreList;
    }

    public string IntArrToString(int[] array){
        string output = "";
        for(int i = 0; i < array.Length; i++){
            output += array[i] + " ";
        }
        return output.Trim();
    }

    public int[] StringToIntArr(string input){
        string[] split = input.Split(" ");
        int[] output = new int[split.Length];
        for(int i = 0; i < split.Length; i++){
            output[i] = int.Parse(split[i]);
        }
        return output;
    }
}
