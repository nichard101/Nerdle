using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGuess : MonoBehaviour
{   
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextBox textBoxPrefab;
    //[Range(0f, 1f)]
    [SerializeField] private float xSpacing;
    [SerializeField] private bool isActive;
    [SerializeField] private Color[] colorList;
    private TextBox[] textBoxArray;
    private string word;
    private Animator anim;
    private GameMaster gm;
    // Start is called before the first frame update
    void Start()
    {   
        anim = GetComponent<Animator>();
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        word = "";
        textBoxArray = new TextBox[5];

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
        if(isActive){
            ClearWord();
            UpdateWord();
        }
    }

    public void ClearWord(){
        for(int i = 0; i < 5; i++){
            textBoxArray[i].SetLetter(' ');
        }
    }

    public void SetWord(string word){
        this.word = word;
    }

    public void AddToWord(char letter){
        if(word.Length < 5){
            word += letter;
        }
    }

    public void UpdateWord(){
        for(int i = 0; i < word.Length; i++){
            textBoxArray[i].SetLetter(word[i]);
        }
    }

    public void Backspace(){
        if(this.word.Length > 0){
            this.word = this.word.Substring(0,word.Length-1);
        }
        Debug.Log(word);
    }

    public void SetColors(int[] colors){
        for(int i = 0; i < 5; i++){
            textBoxArray[i].SetColor(gm.GetColor(colors[i]));
        }
    }

    public string GetWord(){
        return word;
    }

    public bool GetActive(){
        return isActive;
    }

    public void SetInactive(){
        isActive = false;
    }

    public void MoveUp(){
        rectTransform.position += new Vector3(0f, xSpacing, 0f);
    }
}
