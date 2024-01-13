using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Tool.Singleton;

public class animalIcon : Singleton<animalIcon>
{
    public Sprite monkeyIcon;
    public Sprite catIcon;
    public Sprite batIcon;
    public Sprite dogIcon;
    public Sprite fishIcon;
    public Sprite geckoIcon;
    public Sprite giraffeIcon;
    public Sprite ratIcon;
    public Sprite scorpionIcon;
    // public GameObject activeAnimal;

    void Start()
    {
        // changeIcon(activeAnimal.GetComponent<AnimalBase>().GetType().Name);
    }

    public void changeIcon(string animalType)
    {

        if (animalType == "Monkey")
        {
            GetComponent<Image>().sprite = monkeyIcon;
        }
        if (animalType == "Cat")
        {
            GetComponent<Image>().sprite = catIcon;
        }
        if (animalType == "Bat")
        {
            GetComponent<Image>().sprite = batIcon;
        }
        if (animalType == "Dog")
        {
            GetComponent<Image>().sprite = dogIcon;
        }
        if (animalType == "Fish")
        {
            GetComponent<Image>().sprite = fishIcon;
        }
        if (animalType == "Gecko")
        {
            GetComponent<Image>().sprite = geckoIcon;
        }
        if (animalType == "Giraffe")
        {
            GetComponent<Image>().sprite = giraffeIcon;
        }
        if (animalType == "Rat")
        {
            GetComponent<Image>().sprite = ratIcon;
        }
        if (animalType == "Scorpion")
        {
            GetComponent<Image>().sprite = scorpionIcon;
        }
    }
}
