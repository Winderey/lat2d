using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static GameManager Instance => _instance;


    // Start is called before the first frame update
    public Transform respawnPoint;
    public int coinScore = 0;
    // Start is called before the first frame update


    public void GetCoin()
    {
        coinScore++;
    }

    public void MyMethod()
    {
        Debug.Log("Do smth");
    }
    
    public void Respawn(Transform playerPos)
    {
        playerPos.position = respawnPoint.position;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

    else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
