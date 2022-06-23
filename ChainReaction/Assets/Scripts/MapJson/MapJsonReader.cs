using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapJsonReader : SingletonMonoBehaviour<MapJsonReader>
{
    Dictionary<string, List<MapData>> mapListDict;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        mapListDict = new Dictionary<string, List<MapData>>();
        LoadMapFromJsonFile("CustomMaps/CustomMaps.json", false, "Custom");
        LoadMapFromJsonFile("Maps.json", true, "Campaign");
    }

    void Start()
    {
        
    }

    private AtomData ReadAtomData(AtomJsonObject ajo)
    {
        //Create(int x, int y, int t, string[] simple = null)

        var x = ajo.x;
        var y = ajo.y;
        var t = ajo.type;
        var g = ajo.group;
        var simple = ajo.neutrons;

        return AtomData.Create(x, y, t, g, simple);
    }

    private MapData ReadMapData(MapJsonObject mjo)
    {
        var w = mjo.width;
        var h = mjo.height;
        var _steps = mjo.steps;
        var _name = mjo.name;

        var ad = new List<AtomData>();
        foreach (var ajo in mjo.atoms)
        {
            ad.Add(ReadAtomData(ajo));
        }

        return new MapData(w, h, ad, _steps, _name);
    }

    private void LoadMapFromJsonFile(string path, bool asset, string listName)
    {
        var mapList = new List<MapData>();

        var mljo = JsonReader.GetDataFromJson<MapListJsonObject>(path, asset);
        var mjoList = mljo.maps;

        foreach (var mjo in mjoList)
        {
            mapList.Add(ReadMapData(mjo));
        }

        mapListDict.Add(listName, mapList);
    }

    public MapData GetMapAtIndex(string listName, int index)
    {
        var mapList = mapListDict[listName];
        if (index < 0 || index >= mapList.Count) return null;
        return mapList[index];
    }

    public int MapCount(string listName)
    {
        var mapList = mapListDict[listName];
        return mapList.Count;
    }
}

[System.Serializable]
public class MapListJsonObject
{
    public List<MapJsonObject> maps = new List<MapJsonObject>();
}

[System.Serializable]
public class MapJsonObject
{
    public string name = "Default Name";
    public int width = 1, height = 1;
    public int steps = 1;
    public List<AtomJsonObject> atoms = null;

}

[System.Serializable]
public class AtomJsonObject
{
    public int x = 0, y = 0;
    public int type = 0;
    public int group = 0;
    public string[] neutrons = null;
}