using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomData
{
    public Vector2Int pos;

    public int layer;

    public const int NORMAL = 0, MOVABLE = 1, CATCHING = 2, CATCHING_MOVABLE = 3, ENTANGLED = 4, ENTANGLED_MOVABLE = 5;
    public int type;
    public int group;

    public List<NeutronData>[] neutronLists;

    public AtomData()
    {
        pos = Vector2Int.zero;
        type = 0;
        group = 0;

        layer = 1;
        neutronLists = new List<NeutronData>[] { new List<NeutronData>() };
    }

    public AtomData(int x, int y, int t = NORMAL, int g = 0, List<NeutronData>[] data = null)
    {
        pos = new Vector2Int(x, y);
        type = t;
        group = g;

        if (data != null)
        {
            layer = data.Length;
            neutronLists = data;
        }
        else
        {
            layer = 1;
            neutronLists = new List<NeutronData>[] { new List<NeutronData>() };
        }
    }

    [Obsolete("Use AtomData.Create instead.")]
    public AtomData(int x, int y, int t, int g, AtomData template)
    {
        pos = new Vector2Int(x, y);
        type = t;
        group = g;

        layer = template.layer;
        neutronLists = template.neutronLists;
    }

    public static AtomData Create(int x, int y, int t, int g = 0, string[] simple = null)
    {
        List<NeutronData>[] ndList;

        if (simple == null || simple.Length == 0) ndList = new List<NeutronData>[] { new List<NeutronData>() };
        else
        {
            ndList = new List<NeutronData>[simple.Length];
            for (int i = 0; i < simple.Length; i++)
            {
                var nd = new List<NeutronData>();
                var str = simple[i];
                foreach (char ch in str)
                {
                    int d = ch - '0';
                    nd.Add(NeutronData.Create(d));
                }

                ndList[i] = nd;
            }
        }
        return new AtomData(x, y, t, g, ndList);
    }



    // public void SetPos(int x, int y){
    //     pos.x = x;
    //     pos.y = y;
    // }


    // ********************************* Static Content ********************************** //
    public static AtomData FULL = new AtomData(
        0, 0, 0, 0,
        new List<NeutronData>[]{new List<NeutronData>(){
            NeutronData.BOTTOMLEFT,
            NeutronData.BOTTOMRIGHT,
            NeutronData.LEFT,
            NeutronData.RIGHT,
            NeutronData.UPPERLEFT,
            NeutronData.UPPERRIGHT
        }}
    );

    public static AtomData LEFT = new AtomData(
        0, 0, 0, 0,
        new List<NeutronData>[]{new List<NeutronData>(){
            NeutronData.LEFT
        }}
    );

    public static AtomData RIGHT = new AtomData(
        0, 0, 0, 0,
        new List<NeutronData>[]{new List<NeutronData>(){
            NeutronData.RIGHT
        }}
    );

    public static AtomData UPPERLEFT = new AtomData(
        0, 0, 0, 0,
        new List<NeutronData>[]{new List<NeutronData>(){
            NeutronData.UPPERLEFT
        }}
    );

    public static AtomData UPPERRIGHT = new AtomData(
        0, 0, 0, 0,
        new List<NeutronData>[]{new List<NeutronData>(){
            NeutronData.UPPERRIGHT
        }}
    );

    public static AtomData BOTTOMLEFT = new AtomData(
        0, 0, 0, 0,
        new List<NeutronData>[]{new List<NeutronData>(){
            NeutronData.BOTTOMLEFT
        }}
    );
    public static AtomData BOTTOMRIGHT = new AtomData(
        0, 0, 0, 0,
        new List<NeutronData>[]{new List<NeutronData>(){
            NeutronData.BOTTOMRIGHT
        }}
    );

    public static AtomData CATHING_EMPTY = new AtomData(
        0, 0, CATCHING
    );

    public static AtomData CATHIGN_MOVABLE_EMPTY = new AtomData(
        0, 0, CATCHING_MOVABLE
    );


}


