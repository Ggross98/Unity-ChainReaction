using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : SingletonMonoBehaviour<LevelController>
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform[] cellParent;
    [SerializeField] private AudioClip[] bgm;
    [SerializeField] private Color clearColor, unlockedColor;
    [SerializeField] private Text modeText;
    private List<LevelCell>[] levelCells;

    private AccountController accountController;

    // 默认解锁的关卡数量
    public int unlockLevelCount = 3;

    void Start()
    {

        accountController = AccountController.Instance;
        if(SoundController.Instance.GetBGMCount() <= 0){
            SoundController.Instance.SetBGM(bgm);
        }
        

        // 测试读取json格式地图文件
        // Debug.Log("Custom maps: " + MapJsonReader.Instance.DIYMapCount());
        // var customMapData = MapJsonReader.Instance.GetDIYMapAtIndex(0);
        // Debug.Log(customMapData.atomList.Count);

        CreateLevelCells();
        ShowMode();
    }

    private void CreateLevelCells()
    {
        levelCells = new List<LevelCell>[2];

        // 固定关卡
        levelCells[0] = new List<LevelCell>();
        // var count = MapData.MapCount();
        // Debug.Log(count);
        var count = MapJsonReader.Instance.MapCount("Campaign");
        var unlock = unlockLevelCount;
        var clearList = accountController.GetClearLevelList();

        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(cellPrefab, cellParent[0]);
            var cell = obj.GetComponent<LevelCell>();
            cell.SetText((i + 1) + "");
            cell.SetController(this);

            if (clearList.Contains(i)) cell.SetNormalColor(clearColor);
            else if (unlock > 0)
            {
                cell.SetNormalColor(unlockedColor);
                unlock--;
            }
            else
            {
                cell.SetInteractable(false);
            }

        }

        // diy关卡
        levelCells[1] = new List<LevelCell>();
        var customCount = MapJsonReader.Instance.MapCount("Custom");
        for(int i = 0; i< customCount; i++){
            var obj = Instantiate(cellPrefab, cellParent[1]);
            var cell = obj.GetComponent<LevelCell>();
            cell.SetText((i + 1) + "");
            cell.SetController(this);

            cell.SetNormalColor(clearColor);
        }

    }

    public void ShowMode(){
        var mode = accountController.playMode;
        if (mode == 1)
        {
            modeText.text = "Custom";
            cellParent[0].gameObject.SetActive(false);
            cellParent[1].gameObject.SetActive(true);
        }
        else
        {
            modeText.text = "Campaign";
            cellParent[0].gameObject.SetActive(true);
            cellParent[1].gameObject.SetActive(false);
        }
    }

    public void ChangeMode()
    {
        // var mode = accountController.playMode;

        // if (mode == 0)
        // {
        //     modeText.text = "Custom";
        //     cellParent[0].gameObject.SetActive(false);
        //     cellParent[1].gameObject.SetActive(true);
        // }
        // else
        // {
        //     modeText.text = "Campaign";
        //     cellParent[0].gameObject.SetActive(true);
        //     cellParent[1].gameObject.SetActive(false);
        // }

        accountController.playMode = 1 - accountController.playMode;
        ShowMode();
    }

    public void ClickCell(LevelCell cell)
    {
        StartGame(cell.Index() - 1);
    }

    public void StartGame(int index)
    {
        accountController.currentLevel = index;
        // Debug.Log("Map "+index);
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
