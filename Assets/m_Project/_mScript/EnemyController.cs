using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform ennemy;
    [SerializeField] Transform position1;
    [SerializeField] Transform position2;

    [SerializeField] Animator animator;

    [SerializeField] float enemySpeed = 10f;

    private bool leftPosition = false;
    private bool rightPosition = true;
    private bool pause = false;
    public int ennemyHP = 1;
    public bool isDead = false;

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Pause", pause);

        Calm();


    }

    public void Calm()
    {
        if (pause == false)
        {

            if (Vector3.Distance(ennemy.transform.position, position1.transform.position) > 0.001f && leftPosition == false)
            {
                ennemy.localScale = new Vector3(-2f, ennemy.localScale.y, ennemy.localScale.z);
                ennemy.transform.position = Vector3.MoveTowards(ennemy.transform.position, position1.transform.position, enemySpeed);
                if (Vector3.Distance(ennemy.transform.position, position1.transform.position) < 0.001f)
                {
                    StartCoroutine(Pause());
                }
            }


            //================


            if (Vector3.Distance(ennemy.transform.position, position2.transform.position) > 0.001f && leftPosition == true)
            {
                ennemy.localScale = new Vector3(2f, ennemy.localScale.y, ennemy.localScale.z);
                ennemy.transform.position = Vector3.MoveTowards(ennemy.transform.position, position2.transform.position, enemySpeed);
                if (Vector3.Distance(ennemy.transform.position, position2.transform.position) < 0.001f)
                {
                    StartCoroutine(Pause());
                }
            }

        }
    }

    public void LoseHP()
    {
        ennemyHP = ennemyHP - 1;
        if (ennemyHP <= 0)
        {
            isDead = true;
        }
        else
        {
            Debug.Log("a");
        }
    }



    IEnumerator Pause()
    {
        pause = true;
        yield return new WaitForSeconds(1f);
        leftPosition = !leftPosition;
        rightPosition = !rightPosition;
        pause = false;
    }

    /*IEnumerator CheckDistance()
    {
        
    }*/
}
