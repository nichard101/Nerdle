using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatScreen : MonoBehaviour
{
    [SerializeField] private Text win;
    [SerializeField] private Text loss;
    [SerializeField] private Text[] nums;
    private GameMaster gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        Dictionary<int, int> scores = gm.GetScores();
        int one = scores[1];
        int two = scores[2];
        int three = scores[3];
        int four = scores[4];
        int five = scores[5];
        int six = scores[6];
        int wins = one + two + three + four + five + six;
        int[] arr = new int[]{one, two, three, four, five, six};

        win.text = "" + wins;
        loss.text = "" + scores[0];
        for(int i = 0; i < arr.Length; i++){
            nums[i].text = "" + arr[i];
        }

    }
}
