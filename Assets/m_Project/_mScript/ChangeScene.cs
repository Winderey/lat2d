using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneToLoad;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // V�rifie si l'objet entrant dans le trigger est le joueur
        {
            SceneManager.LoadScene(sceneToLoad); // Charge la sc�ne sp�cifi�e
        }
    }
}
