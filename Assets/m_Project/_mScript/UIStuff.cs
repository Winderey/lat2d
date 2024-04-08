using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIStuff : MonoBehaviour
{

    public GameManager gameManger;
    public TMP_Text scoreTxt;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreTxt.text = gameManger.coinScore.ToString();
    }
}
