using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{   
    [Header(" Elements ")]
    [SerializeField] private Text keyText;
    private char key;

    [Header(" Settings ")]
    [SerializeField] private bool isBackspace;
    [SerializeField] private bool isEnter;

    public void SetKey(char key){
        this.key = key;
        keyText.text = key.ToString();
    }

    public void SetColour(Color colour){
        GetComponent<Image>().color = colour;
    }

    public Button GetButton(){
        return GetComponent<Button>();
    }

    public bool IsBackspace(){
        return isBackspace;
    }

    public bool IsEnter(){
        return isEnter;
    }
}
