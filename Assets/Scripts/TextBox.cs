using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    
    [Header(" Text ")]
    [SerializeField] private Text textBox;
    
    private Color color;
    private char letter;
    
    
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLetter(char letter){
        this.letter = letter;
        textBox.text = letter.ToString();
    }

    public void SetColor(Color color){
        this.color = color;
        GetComponent<Image>().color = color;
    }
}