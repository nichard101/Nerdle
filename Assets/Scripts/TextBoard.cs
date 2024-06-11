using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoard : MonoBehaviour
{
    [SerializeField] private TextBox textBoxPrefab;
    [SerializeField] private Color[] colorList;
    // Start is called before the first frame update
    void Start()
    {
        this.colorList = new Color[4];
        this.colorList[0] = new Color(1f, 1f, 1f);              // white
        this.colorList[1] = new Color(0.471f, 0.486f, 0.494f);  // grey
        this.colorList[2] = new Color(0.788f, 0.706f, 0.345f);  // yellow
        this.colorList[3] = new Color(0.416f, 0.667f, 0.392f);  // green
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
