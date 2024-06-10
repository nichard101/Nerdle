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

    public void SetKey(char key){
        this.key = key;
        keyText.text = key.ToString();
    }

    public Button GetButton(){
        return GetComponent<Button>();
    }

    public bool IsBackspace(){
        return isBackspace;
    }
}
