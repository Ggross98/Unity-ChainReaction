using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountController : SingletonMonoBehaviour<AccountController>
{

    // 读取地图的索引
    public int currentLevel = 0;
    // 游玩模式，0：固定关卡；1：自定义关卡
    public int playMode = 0;


    // 存档管理
    public string accountName = "Player";

    public Account account;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadAccount(accountName);
    }

    private void LoadAccount(string name){
        account = GetAccount(name);
    }

    public Account GetAccount(string name){
        Account a;
        bool hasAccount = PlayerPrefs.GetInt(name, -1) == 1;
        if(hasAccount){
            var n = name+"_ClearLevelsCount";
            var count = PlayerPrefs.GetInt(n);
            // Debug.Log("Try get account: "+n);
            var clearLevels = new List<int>();
            if(count > 0){
                for(int i = 0; i<count; i++){
                    var level = PlayerPrefs.GetInt(name+"_" + i, -1);
                    // Debug.Log("Level: "+level);
                    if(level != -1 && !clearLevels.Contains(level)) clearLevels.Add(level);
                }
            }
            a = new Account(name, clearLevels);
        }else{
            a = new Account(name, new List<int>());
        }
        return a;
    }

    public List<int> GetClearLevelList(){
        return account.clearLevels;
    }

    public void NextLevel(){
        // Debug.Log(currentLevel + " clear!");
        // Debug.Log(account.clearLevels);

        switch(playMode){
            case 0:
                if(!account.clearLevels.Contains(currentLevel)){
                    account.clearLevels.Add(currentLevel);
                    SaveAccount(account);
                }
                break;
            case 1:
            break;
        }
        
        currentLevel ++;
    }


    public void SaveAccount(Account a){
        PlayerPrefs.SetInt(a.name, 1);
        var cl = a.clearLevels;
        var n = a.name+"_ClearLevelsCount";
        PlayerPrefs.SetInt(n, cl.Count);
        // Debug.Log(cl.Count);
        for(int i = 0; i<cl.Count; i++){
            PlayerPrefs.SetInt(a.name+"_"+i, cl[i]);
        }

        // Debug.Log("Save account!");
        // Debug.Log(a.clearLevels.Count);
        // Debug.Log(GetAccount(a.name).clearLevels.Count);
    }

    public MapData GetCurrentLevelMapData(){

        if(playMode == 0) {
            // if(currentLevel >= MapData.MapCount()) currentLevel = 0; 
            if(currentLevel >= MapJsonReader.Instance.MapCount("Campaign")) currentLevel = 0; 
            // return MapData.GetAtIndex(currentLevel);
            return MapJsonReader.Instance.GetMapAtIndex("Campaign", currentLevel);
        }
        else {
            if(currentLevel >= MapJsonReader.Instance.MapCount("Custom")) currentLevel = 0; 
            return MapJsonReader.Instance.GetMapAtIndex("Custom", currentLevel);
        }
    }
}

public class Account
{
    public string name;
    public List<int> clearLevels;
    // public int clearLevelsCount;

    public Account(string n, List<int> cl){
        name = n;
        clearLevels = cl;
    }
}
