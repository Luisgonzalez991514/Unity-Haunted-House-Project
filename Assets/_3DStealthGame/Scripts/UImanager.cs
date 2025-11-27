using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
  
public class UImanager : MonoBehaviour
{
    public Image sprintUI;
    public PlayerMovement player;
   
    void Start()
    {
        Color c = sprintUI.color;
        c.a = 0f;
        sprintUI.color = c;
    }

    
    void Update()
    {
         if(player.cooldownTimer >= player.cooldownTime){
            Color c = sprintUI.color;
            c.a = 1f;
            sprintUI.color = c;
        }
        else{
            Color c = sprintUI.color;
            c.a = 0f;
            sprintUI.color = c;
        }
    }
}
