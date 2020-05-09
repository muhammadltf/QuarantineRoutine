﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroomScript : MonoBehaviour
{
    private int broomCounter;
    public MiniGameController miniGameControllerInstance;
    public Image BackgroundMessy;
    bool isRight;

    private float targetFill;
    private bool coolingDown;
    public GameObject Tutorial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        isRight = true;
        broomCounter = 0;
        targetFill = 0f;
        coolingDown = false;
        Tutorial.SetActive(true);
    }

    void OnDisable(){
    }

    // Update is called once per frame
    void Update()
    {
        if(broomCounter >= 50){
            miniGameControllerInstance.CloseMiniGame(this.gameObject, "Sapu");
        }

        if(coolingDown){
            // BackgroundMessy.fillAmount -= 0.25f * Time.deltaTime;
            // if(BackgroundMessy.fillAmount <= targetFill){
            //     coolingDown = false;
            // }

            float newA = BackgroundMessy.color.a - 0.25f * Time.deltaTime;
            BackgroundMessy.color = new Color(BackgroundMessy.color.r, BackgroundMessy.color.g, BackgroundMessy.color.b, newA);

            if(BackgroundMessy.color.a <= targetFill){
                coolingDown = false;
            }
        }
    }

    public void MoveButton(GameObject btn){
        if(isRight){
            btn.gameObject.transform.Translate(-5f, 0f, 0f);
            isRight = false;
        } else {
            btn.gameObject.transform.Translate(5f, 0f, 0f);
            isRight = true;
        }

        broomCounter++;
        miniGameControllerInstance.AddProgressTrack(broomCounter, 50);
        
        // targetFill = BackgroundMessy.fillAmount - (1f/ 20f);
        targetFill = BackgroundMessy.color.a - (1f / 50f);
        coolingDown = true;
    }

    public void RestartBroom(){
        broomCounter = 0;
    }
}
