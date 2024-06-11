using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoard : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextBox textBoxPrefab;
    [SerializeField] private WordGuess wordGuessPrefab;
    [SerializeField] private Color[] colorList;
    [SerializeField] private float xSpacing;
    private string word;
    private TextBox[] textBoxArray;
    private TextBox[][] previousGuesses;

    // Start is called before the first frame update
    void Start()
    {
        word = "";
        textBoxArray = new TextBox[5];
        previousGuesses = new TextBox[6][];

        float startX = rectTransform.position.x-2f*xSpacing;
        float startY = rectTransform.position.y;
        for(int i = 0; i < 5; i++){
            float boxX = startX + xSpacing*i;
            TextBox newTextBox = Instantiate(textBoxPrefab, rectTransform);
            newTextBox.SetLetter(' ');
            textBoxArray[i] = newTextBox;
            Vector2 boxPosition = new Vector2(boxX, startY);

            RectTransform boxRectTransform = rectTransform.GetChild(i).GetComponent<RectTransform>();
            boxRectTransform.position = boxPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < word.Length; i++){
            textBoxArray[i].SetLetter(word[i]);
        }
    }

    public void AddToWord(char letter){
        if(word.Length < 5){
            word += letter;
        }
    }

    public void Backspace(){
        if(word.Length > 0){
            word.Remove(word.Length-1);
        }
    }
}
