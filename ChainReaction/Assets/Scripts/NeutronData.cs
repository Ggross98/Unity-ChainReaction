using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutronData
{
    public Vector2 dir;

    public NeutronData(float x, float y){
        dir = new Vector2(x, y);
    }

    // public static NeutronData UPPER = new NeutronData(0,1);
    // public static NeutronData BOTTOM = new NeutronData(0,-1);

    // ********************************************** static content **********************************************

    public static NeutronData Create(int d){
        NeutronData nd = LEFT;
        switch(d){
            case 0: nd = LEFT; break;
            case 1: nd = UPPERLEFT; break;
            case 2: nd = UPPERRIGHT; break;
            case 3: nd = RIGHT; break;
            case 4: nd = BOTTOMRIGHT; break;
            case 5: nd = BOTTOMLEFT; break;
            
        }
        return nd;
    }
    public static NeutronData UPPERLEFT = new NeutronData(-1, Mathf.Sqrt(3f));
    public static NeutronData UPPERRIGHT = new NeutronData(1, Mathf.Sqrt(3f));
    public static NeutronData LEFT = new NeutronData(-1,0);
    public static NeutronData RIGHT = new NeutronData(1,0);
    public static NeutronData BOTTOMLEFT = new NeutronData(-1,-Mathf.Sqrt(3f));
    public static NeutronData BOTTOMRIGHT = new NeutronData(1,-Mathf.Sqrt(3f));
}
