using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{

    public static Color GetColor(int r, int g, int b, int a = 100){

        return new Color(r/255f, g/255f, b/255f, a/100f);
    }

    public static Color GetColor(Color color, float a){
        return new Color(color.r, color.g, color.b, a);
    }
}
