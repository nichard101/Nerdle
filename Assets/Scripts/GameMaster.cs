using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private TextBoard boardPrefab;
    private string[] wordList;
    private string[] shuffledWords;
    private string currentWord;
    private int currentIndex = 0;

    void Start(){
        wordList = File.ReadAllLines("Assets/sgb-words.txt");
        shuffledWords = ShuffleList(wordList);
        //currentWord = shuffledWords[currentIndex];
        currentWord = "sofia";
    }

    private string[] ShuffleList(string[] wordList){
        for (int t = 0; t < wordList.Length; t++ )
        {
            string tmp = wordList[t];
            int r = UnityEngine.Random.Range(t, wordList.Length);
            wordList[t] = wordList[r];
            wordList[r] = tmp;
        }
        return wordList;
    }

    public int[] EnterGuess(string g){
        StringBuilder answer = new StringBuilder(currentWord.ToUpper());
        StringBuilder guess = new StringBuilder(g.ToUpper());
        //string answer = DeepCopyString(currentWord).ToUpper();
        //guess = guess.ToUpper();
        int[] result = {0,0,0,0,0}; // all positions start as grey
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
                    if(result[counter] == 0){
                        result[counter] = 2; // sets this position to yellow if not already green
                    }
                }
            }
            i++;
            counter++;
        }
        return result;
    }

    private int FindIndexOfLetter(string word, char letter){
        for(int i = 0; i < word.Length; i++){
            if(word[i] == letter){
                return i;
            }
        }
        return -1;
    }

    private string RemoveLetterAtIndex(string input, int index){
        if(input.Length==1){
            return "";
        }
        if(index==0){
            return input.Substring(1);
        }
        if(index==4){
            return input.Substring(0,4);
        }
        return input.Substring(0,index) + input.Substring(index+1);
    }

    private string DeepCopyString(string input){
        string output = "";
        foreach(char c in input){
            output += c.ToString();
        }
        return output;
    }
}
