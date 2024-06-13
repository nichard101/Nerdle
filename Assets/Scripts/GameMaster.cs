using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private TextBoard boardPrefab;
    [SerializeField] private Keyboard keyboardPrefab;
    [SerializeField] private HUDManager hud;
    [SerializeField] private Color[] colorList;
    private int[] shuffled;
    private string currentWord;
    private int currentIndex;
    private int numGuesses;
    private bool isGameOver;
    private GuessHistoryStruct guessHistory;
    private Dictionary<int, int> scoreList;
    private Dictionary<int, string> wordList;

    void Start(){
        if(!LoadDictionary() || !LoadShuffle()){
            scoreList = new Dictionary<int, int>();
            wordList = GameData.ConvertDictionary();
            SaveDictionary();
            shuffled = ShuffleList(wordList.Count);
            SaveShuffle();
        }   
        if(!Load()){    // if the load fails for whatever reason, generate a blank slate
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
        if(currentIndex == wordList.Count){            // if we reach the end of the random list, reroll it all and start again
            shuffled = ShuffleList(wordList.Count);
            SaveShuffle();
            currentIndex = 0;
            Save();
        }
        currentWord = wordList[shuffled[currentIndex]];
    }

    private void Save(){        
        string[] saveContents = new string[]{
            ""+currentIndex,
            ""+(0 + " " + scoreList[0] + " " + 1 + " " + scoreList[1] + " " + 2 + " " + scoreList[2] + " " + 3 + " " + scoreList[3] + " " + 4 + " " + scoreList[4] + " " + 5 + " " + scoreList[5] + " " + 6 + " " + scoreList[6])
        };
        string saveString = string.Join("|", saveContents);
        
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream fs = new FileStream(GetPath(), FileMode.Create);
        formatter.Serialize(fs, saveString);
        fs.Close();
        
        Debug.Log("Saved!");
    }

    private bool Load(){
        try{
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream fs = new FileStream(GetPath(), FileMode.Open);
            string saveString = formatter.Deserialize(fs) as string;
            fs.Close();
            
            string[] saveArray = saveString.Split("|");
            currentIndex = int.Parse(saveArray[0]);  // current word index
            FillDictionary(saveArray[1]);            // score history
            Debug.Log("Loaded!");
            return true;
        } catch(Exception e){
            Debug.Log("Load failed!");
            return false;
        }
    }

    private bool LoadDictionary(){
        try{
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(GetPathDict(), FileMode.Open);
            wordList = formatter.Deserialize(fs) as Dictionary<int, string>;
            fs.Close();
            return true;
        } catch(Exception e){
            Debug.Log("Dictionary load failed!");
            return false;
        }
    }

    private void SaveDictionary(){
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(GetPathDict(), FileMode.Create);
        formatter.Serialize(fs, wordList);
        fs.Close();
    }

    private bool LoadShuffle(){
        try{
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(GetPathShuffle(), FileMode.Open);
            shuffled = formatter.Deserialize(fs) as int[];
            fs.Close();
            return true;
        } catch(Exception e){
            Debug.Log("Dictionary load failed!");
            return false;
        }
    }

    private void SaveShuffle(){
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(GetPathShuffle(), FileMode.Create);
        formatter.Serialize(fs, shuffled);
        fs.Close();
    }

    private string GetPath(){
        return Application.persistentDataPath + "/data.qnd";
    }

    private string GetPathDict(){
        return Application.persistentDataPath + "/dict.qnd";
    }

    private string GetPathShuffle(){
        return Application.persistentDataPath + "/shuffle.qnd";
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
        return wordList.ContainsValue(word.ToLower());
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
            //Save();
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

}
