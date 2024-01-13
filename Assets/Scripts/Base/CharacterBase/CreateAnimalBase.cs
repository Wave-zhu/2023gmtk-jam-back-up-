using Game.Move;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnimalBase : MonoBehaviour
{
    // Start is called before the first frame update
    bool isInsideTrigger;
    public GameObject animal;
    private Collider2D _col;
    private void OnTriggerEnter2D(Collider2D other)
    {
        isInsideTrigger = true;
        _col = other;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInsideTrigger = false;
        _col = null;
    }
    private void Update()
    {
        if (isInsideTrigger)
        {
            if(GameInputManager.MainInstance.SwitchPlayer)
            {
                Destroy(_col.gameObject);
                string iconName = animal.GetComponent<AnimalBase>().GetType().Name;
                print(iconName);
                animalIcon.MainInstance.changeIcon(iconName);
                var ani=Instantiate(animal, transform.position, Quaternion.identity);
                var con = ani.GetComponent<PlayerController>();
                CameraManager.MainInstance.ResetFollowPoint(con._followPointHorizontal, con._followPointVertical);
            }
        }
    }

}
