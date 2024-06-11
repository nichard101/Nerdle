using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Key keyPrefab;
    [SerializeField] private Key backspaceKeyPrefab;
    [SerializeField] private Key enterKeyPrefab;
    [SerializeField] private TextBoard TextBoardObject;

    [Header(" Settings ")]
    [Range(0f, 1f)]
    [SerializeField] private float widthPercent;
    [Range(0f, 1f)]
    [SerializeField] private float heightPercent;
    [Range(0f, 0.5f)]
    [SerializeField] private float bottomOffset;

    [Header(" Keyboard Lines ")]
    [SerializeField] private KeyboardLine[] lines;

    [Header(" Key Settings ")]
    [Range(0f, 1f)]
    [SerializeField] private float scaleValue;
    [Range(0f, 1f)]
    [SerializeField] private float keyXSpacing;

    IEnumerator Start()
    {
        CreateKeys();
        
        yield return null;

        UpdateRectTransform();
        PlaceKeys();
    }

    void Update()
    {
        UpdateRectTransform();
        PlaceKeys();
    }

    private void UpdateRectTransform(){
        float width = widthPercent * Screen.width;
        float height = heightPercent * Screen.height;

        rectTransform.sizeDelta = new Vector2(width,height);

        Vector2 position;

        position.x = Screen.width / 2;
        position.y = bottomOffset * Screen.height + height / 2;

        rectTransform.position = position;
    }

    private void CreateKeys(){
        for(int i = 0; i < lines.Length; i++){
            for(int j = 0; j < lines[i].keys.Length; j++){
                char key = lines[i].keys[j];
                if(key == ','){
                    Key keyInstance = Instantiate(enterKeyPrefab, rectTransform);

                    keyInstance.GetButton().onClick.AddListener(() => EnterPressedCallback());
                } else if(key == '.'){
                    Key keyInstance = Instantiate(backspaceKeyPrefab, rectTransform);

                    keyInstance.GetButton().onClick.AddListener(() => BackspacePressedCallback());
                } else {
                    Key keyInstance = Instantiate(keyPrefab, rectTransform);
                    keyInstance.SetKey(key);
                    
                    keyInstance.GetButton().onClick.AddListener(() => KeyPressedCallback(key));
                }
            }
        }
    }

    private void PlaceKeys(){
        int lineCount = lines.Length;

        float lineHeight = (rectTransform.rect.height / lineCount);

        float keyWidth = lineHeight * scaleValue;
        float xSpacing = keyXSpacing * lineHeight;

        int currentIndex = 0;

        for(int i = 0; i < lineCount; i++){
            float halfKeyCount = lines[i].keys.Length / 2f;

            bool containsBackspace = lines[i].keys.Contains(".");

            if(containsBackspace){
                //halfKeyCount += .5f;
            }

            bool containsEnter = lines[i].keys.Contains(",");

            if(containsEnter){
                //halfKeyCount += .5f;
            }

            float startX = rectTransform.position.x - (keyWidth + xSpacing) * halfKeyCount + (keyWidth+xSpacing)/2;
            float lineY = rectTransform.position.y + rectTransform.rect.height / 2 - lineHeight / 2 - i * lineHeight;
            
            for(int j = 0; j < lines[i].keys.Length; j++){
                bool isBackspaceKey = lines[i].keys[j] == '.';
                bool isEnterKey = lines[i].keys[j] == ',';
                float keyX = startX + j * (keyWidth+xSpacing);


                if(isBackspaceKey){
                    keyX += keyWidth - xSpacing;
                }

                if(isEnterKey){
                    keyX -= (keyWidth - xSpacing) ;
                }

                Vector2 keyPosition = new Vector2(keyX, lineY);

                RectTransform keyRectTransform = rectTransform.GetChild(currentIndex).GetComponent<RectTransform>();
                keyRectTransform.position = keyPosition;

                float thisKeyWidth = keyWidth;
                if(isBackspaceKey || isEnterKey){
                    thisKeyWidth *= 2;
                }

                keyRectTransform.sizeDelta = new Vector2(thisKeyWidth, keyWidth);
                currentIndex++;
            }
        }
    }

    private void BackspacePressedCallback(){
        Debug.Log("Backspace");
        TextBoardObject.Backspace();
    }
    
    private void EnterPressedCallback(){
        Debug.Log("Enter");
    }

    private void KeyPressedCallback(char key){
        Debug.Log("Key " + key);
        TextBoardObject.AddToWord(key);
    }
}

[System.Serializable]
public struct KeyboardLine{
    public string keys;

}