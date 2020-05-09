﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodScript : MonoBehaviour
{
    public TextMeshProUGUI[] charText;
    public GameObject charParent;
    public GameObject answerParent;
    public GameObject answerPlaceHolder;
    private List<string> foods;
    private string onScreenChar;
    private bool clearButton;
    const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";

    private List<TextMeshProUGUI> activeAnswers;
    public MiniGameController miniGameControllerInstance;
    public GameObject BackspaceButton;

    private Stack<GameObject> pressedChar;
    public List<Sprite> CurrentFoodSprite;
    private string activeFood;
    public Image CurrentFood;

    public GameObject Tutorial;

    // Start is called before the first frame update
    void Start()
    {
        activeAnswers = new List<TextMeshProUGUI>();
        pressedChar = new Stack<GameObject>();
        clearButton = true;
    }

    void OnEnable(){
        foods = new List<string> (new string[] {"sate", "bakso", "ayam goreng", "burger", "pizza"}); 
        onScreenChar = "";
        Tutorial.SetActive(true);
    }

    void OnDisable(){
        charParent.SetActive(false);
        answerParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {   
        if(foods.Count <= 0){
            miniGameControllerInstance.CloseMiniGame(this.gameObject, "Makan");
            return;
        }

        if(clearButton){
            foreach(TextMeshProUGUI c in charText){
                c.transform.parent.gameObject.GetComponent<Button>().interactable = true;
            }
            clearButton = false;
        }

        if(onScreenChar.Length > 1){
            return;
        }

        activeFood = foods[Random.Range(0, foods.Count - 1)];
        onScreenChar = shuffleChar();
        for(int i = 0; i < onScreenChar.Length; i++){
            charText[i].text = "" + onScreenChar[i];
        }

        generateAnswerSpace();         
        MapFoodSprite(activeFood);
    }

    string shuffleChar(){
        string shuffledChar = activeFood.Replace(" ", "");

        while (shuffledChar.Length < 15){
            shuffledChar = shuffledChar + ALPHABET[Random.Range(0, ALPHABET.Length-1)];
        }


        string newString = "";
        while (newString.Length < 15){
            int randomIndex = Random.Range(0, shuffledChar.Length-1);
            newString = newString + shuffledChar[randomIndex];
            shuffledChar = shuffledChar.Remove(randomIndex, 1);
        }

        return newString;
    }

    void generateAnswerSpace(){
        int len = activeFood.Length;
        int i = 0;
        for(; i < len; i++){
            if(activeFood[i].ToString() == " "){
                continue;
            }

            GameObject answerPh = Instantiate(answerPlaceHolder, answerParent.transform.position, transform.rotation);
            answerPh.transform.Translate(new Vector3((i * 56), 0f, 0f)); 
            answerPh.transform.SetParent(answerParent.transform, false);
            activeAnswers.Add(answerPh.GetComponentInChildren<TextMeshProUGUI>());
        }
        
        float backSpaceX = answerParent.transform.position.x + (i * 56) + (BackspaceButton.GetComponent<RectTransform>().rect.width / 2);
        float backSpaceY = answerParent.transform.position.y + (BackspaceButton.GetComponent<RectTransform>().rect.height / 2);
        BackspaceButton.GetComponent<RectTransform>().localPosition = new Vector3(backSpaceX, backSpaceY, answerParent.transform.position.z);
    }

    public void FillAnswer(GameObject charGo){
        pressedChar.Push(charGo.transform.parent.gameObject);
        string answer = "";
        string realAnswer = activeFood.Replace(" ", "");

        foreach(TextMeshProUGUI a in activeAnswers){
            answer += a.text;

            if(a.text == ""){
                a.text = charGo.GetComponent<TextMeshProUGUI>().text;
                answer += a.text;
                break;
            }
        }

        if (answer.Length == realAnswer.Length){
            if(answer == realAnswer){
                foods.Remove(activeFood);
                onScreenChar = "";

                foreach(TextMeshProUGUI a in activeAnswers){
                    Destroy(a.gameObject.transform.parent.gameObject);
                }

                activeAnswers.RemoveRange(0, activeAnswers.Count);
                miniGameControllerInstance.AddProgressTrack(5 - foods.Count, 5);
            } else {
                foreach(TextMeshProUGUI a in activeAnswers){
                    a.text = "";
                }
            }

            clearButton = true;
        }
    }

    public void RestartFood(){
        foods = new List<string> (new string[] {"sate", "bakso", "ayam goreng", "burger", "pizza"}); 
        onScreenChar = "";
        clearButton = true;

        foreach(TextMeshProUGUI a in activeAnswers){
            Destroy(a.gameObject.transform.parent.gameObject);
        }

        activeAnswers.RemoveRange(0, activeAnswers.Count);
    }

    public void Backspace(){
        if (pressedChar.Count > 0){
            pressedChar.Pop().GetComponent<Button>().interactable = true;
        }

        foreach(TextMeshProUGUI a in activeAnswers){
            if(a.text == ""){
                int index = activeAnswers.IndexOf(a);
                if(index != 0){
                    activeAnswers[index-1].text = "";   
                }
                break;
            }
        }
    }

    public void MapFoodSprite(string name){
        switch(name){
			case "sate":
				CurrentFood.sprite = CurrentFoodSprite[0];
				break;
			case "bakso":
				CurrentFood.sprite = CurrentFoodSprite[1];
				break;
			case "ayam goreng":
				CurrentFood.sprite = CurrentFoodSprite[2];
				break;
			case "burger":
				CurrentFood.sprite = CurrentFoodSprite[3];
				break;
			case "pizza":
				CurrentFood.sprite = CurrentFoodSprite[4];
				break;
			default:
				CurrentFood.sprite = CurrentFoodSprite[5];
				break;
		}
    }
}
