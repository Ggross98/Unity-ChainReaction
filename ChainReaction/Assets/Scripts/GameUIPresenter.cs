using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIPresenter : SingletonMonoBehaviour<GameUIPresenter>
{
    [SerializeField] private Text title, stateInfo, leftStepsInfo;

    public void ShowStateInfo(int state){
        if(state == MapController.WAITING || state == MapController.REACTING || state == MapController.NOTSTARTED){
            stateInfo.text = "";
        }else if(state == MapController.WIN){
            stateInfo.text = "Completed!";
        }else if(state == MapController.LOSE){
            stateInfo.text = "Failed!";
        }
    }

    public void ShowLeftSteps(int s){
        leftStepsInfo.text = "Left Steps : "+s;
    }

    public void ShowTitle(string t){
        title.text = t;
    }
    
}
