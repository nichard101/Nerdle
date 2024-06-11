using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGuess : MonoBehaviour
{   
    [SerializeField] private TextBox textBoxPrefab;
    //[Range(0f, 1f)]
    [SerializeField] private float xSpacing;
    [SerializeField] private bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetActive(){
        return isActive;
    }
}
