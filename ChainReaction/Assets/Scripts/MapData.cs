using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public int width, height;
    public List<AtomData> atomList;
    
    public int steps;
    public string name;

    public MapData(int w, int h, List<AtomData> ad = null, int _steps = 1, string _name = "Map"){
        width = w;
        height = h;

        if(ad == null)
            atomList = new List<AtomData>();
        else
            atomList = ad;
        
        steps = _steps;
        name = _name;
    }

    // ********************************* Static Content ********************************** //
    // *********************************    Obsolete!   ********************************** //

    // public static MapData MAP_TEST = new MapData(
    //     5, 3,
    //     new List<AtomData>(){
    //         // 普通单层
    //         AtomData.Create(0,1,0,0,new string[]{"3"}),
    //         // 普通单层多个
    //         AtomData.Create(2,1,0,0,new string[]{"012345"}),
    //         // 普通双层
    //         AtomData.Create(2,0,0,0,new string[]{"024", "135"}),
    //         // 可移动
    //         AtomData.Create(3,0,1,0,new string[]{"135"}),
    //         // 可捕获
    //         AtomData.Create(1,1,2),
    //         // 可捕获且可移动
    //         AtomData.Create(2,2,3)

    //     },
    //     3, "Test Map"
    // );

    // public static MapData MAP_TEST_LARGE = new MapData(
    //     8, 5,
    //     new List<AtomData>(){
    //         // 普通单层
    //         AtomData.Create(0,1,0,0,new string[]{"3"}),
    //         // 普通单层多个
    //         AtomData.Create(5,4,0,0,new string[]{"012345"}),
    //         // 普通双层
    //         AtomData.Create(5,0,0,0,new string[]{"024", "135"}),
    //         // 可移动
    //         AtomData.Create(3,0,1,0,new string[]{"135"}),
    //         // 可捕获
    //         AtomData.Create(1,4,2),
    //         // 可捕获且可移动
    //         AtomData.Create(2,2,3)

    //     },
    //     3, "Test Map"
    // );

    // public static MapData MAP_1_1 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         new AtomData(1,1,0,0,AtomData.FULL),

    //         new AtomData(1,0,0,0,AtomData.BOTTOMLEFT),
    //         new AtomData(0,1,0,0,AtomData.LEFT),
    //         new AtomData(1,2,0,0,AtomData.UPPERLEFT),
    //         new AtomData(2,0,0,0,AtomData.BOTTOMRIGHT),
    //         new AtomData(2,1,0,0,AtomData.RIGHT),
    //         new AtomData(2,2,0,0,AtomData.UPPERRIGHT),
    //     },
    //     1, "1-1 Click to release"
    // );

    // public static MapData MAP_1_2 = new MapData(
    //     4, 1,
    //     new List<AtomData>(){
    //         new AtomData(0,0,0,0,AtomData.BOTTOMLEFT),
    //         new AtomData(1,0,0,0,AtomData.RIGHT),
    //         new AtomData(2,0,0,0,AtomData.LEFT),
    //         new AtomData(3,0,0,0,AtomData.LEFT)
    //     },
    //     1, "1-2"
    // );

    

    // public static MapData MAP_1_3 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         AtomData.Create(0,0,0,0,new string[]{"0"}),
    //         AtomData.Create(0,1,0,0,new string[]{"5"}),
    //         AtomData.Create(1,2,0,0,new string[]{"3"}),
    //         AtomData.Create(1,0,0,0,new string[]{"1"}),
    //         AtomData.Create(2,0,0,0,new string[]{"1"}),
    //         AtomData.Create(3,0,0,0,new string[]{"1"}),
    //         AtomData.Create(2,1,0,0,new string[]{"5"}),
    //         AtomData.Create(2,2,0,0,new string[]{"5"}),
    //     },
    //     1, "1-3"
    // );

    // public static MapData MAP_1_4 = new MapData(
    //     5, 1,
    //     new List<AtomData>(){
    //         new AtomData(0,0,0,0,AtomData.LEFT),
    //         new AtomData(1,0,0,0,new List<NeutronData>[]{
    //             new List<NeutronData>{NeutronData.LEFT},
    //             new List<NeutronData>{NeutronData.RIGHT}
    //         }),
    //         new AtomData(2,0,0,0,AtomData.RIGHT),
    //         new AtomData(3,0,0,0,new List<NeutronData>[]{
    //             new List<NeutronData>{NeutronData.LEFT},
    //             new List<NeutronData>{NeutronData.LEFT}
    //         }),
    //         new AtomData(4,0,0,0,AtomData.LEFT)
    //     },
    //     1, "1-4"
    // );

    // public static MapData MAP_1_5 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         new AtomData(0,1,0,0,AtomData.UPPERRIGHT),
    //         new AtomData(0,2,0,0,AtomData.BOTTOMRIGHT),
    //         new AtomData(1,1,0,0,new List<NeutronData>[]{
    //             new List<NeutronData>{NeutronData.BOTTOMRIGHT},
    //             new List<NeutronData>{NeutronData.RIGHT}
    //         }),
    //         new AtomData(1,2,0,0,new List<NeutronData>[]{
    //             new List<NeutronData>{NeutronData.BOTTOMRIGHT},
    //             new List<NeutronData>{NeutronData.RIGHT}
    //         }),
    //         new AtomData(2,0,0,0,new List<NeutronData>[]{
    //             new List<NeutronData>{NeutronData.UPPERRIGHT},
    //             new List<NeutronData>{NeutronData.UPPERLEFT}
    //         }),
    //         new AtomData(2,1,0,0,AtomData.BOTTOMRIGHT),
    //         new AtomData(3,0,0,0,AtomData.LEFT),
    //         new AtomData(3,2,0,0,AtomData.LEFT)
    //     },
    //     1, "1-5"
    // );
    // public static MapData MAP_1_6 = new MapData(
    //     3, 3,
    //     new List<AtomData>(){
    //         new AtomData(0,0,0,0,AtomData.UPPERRIGHT),
    //         new AtomData(0,1,0,0,AtomData.UPPERLEFT),
    //         new AtomData(0,2,0,0,AtomData.UPPERLEFT),
    //         new AtomData(1,1,0,0,AtomData.BOTTOMRIGHT),
    //         new AtomData(2,0,0,0,AtomData.BOTTOMRIGHT),
    //         new AtomData(2,2,0,0,AtomData.BOTTOMLEFT)
    //     },
    //     2, "1-6"
    // );

    // public static MapData MAP_1_7 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         AtomData.Create(0,1,0,0,new string[]{"3"}),
    //         AtomData.Create(0,2,0,0,new string[]{"3"}),
    //         AtomData.Create(1,0,0,0,new string[]{"3"}),
    //         AtomData.Create(1,2,0,0,new string[]{"5"}),
    //         AtomData.Create(2,0,0,0,new string[]{"1"}),
    //         AtomData.Create(2,1,0,0,new string[]{"5"}),
    //         AtomData.Create(2,2,0,0,new string[]{"5"}),
    //         AtomData.Create(3,0,0,0,new string[]{"3"}),
    //     },
    //     2, "1-7"
    // );
    // public static MapData MAP_1_8 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         new AtomData(0,0,0,0,AtomData.UPPERRIGHT),
    //         new AtomData(0,1,0,0,new List<NeutronData>[]{
    //             new List<NeutronData>{NeutronData.UPPERLEFT},
    //             new List<NeutronData>{NeutronData.RIGHT}
    //         }),
    //         new AtomData(0,2,0,0,AtomData.BOTTOMRIGHT),
    //         new AtomData(1,1,0,0,AtomData.UPPERRIGHT),
    //         new AtomData(2,0,0,0,AtomData.LEFT),
    //         new AtomData(2,1,0,0,AtomData.BOTTOMRIGHT),
    //         new AtomData(2,2,0,0,new List<NeutronData>[]{
    //             new List<NeutronData>{NeutronData.BOTTOMRIGHT},
    //             new List<NeutronData>{NeutronData.BOTTOMRIGHT}
    //         }),
    //         new AtomData(3,0,0,0,new List<NeutronData>[]{
    //             new List<NeutronData>{NeutronData.UPPERLEFT},
    //             new List<NeutronData>{NeutronData.UPPERLEFT}
    //         }),
    //         new AtomData(3,2,0,0,AtomData.BOTTOMLEFT)
    //     },
    //     2, "1-8"
    // );

    // public static MapData MAP_1_9 = new MapData(
    //     5, 3,
    //     new List<AtomData>(){
    //         AtomData.Create(0,0,0,0,new string[]{"2"}),
    //         AtomData.Create(0,1,0,0,new string[]{"5"}),
    //         AtomData.Create(0,2,0,0,new string[]{"4"}),
    //         AtomData.Create(1,1,0,0,new string[]{"4"}),
    //         AtomData.Create(1,2,0,0,new string[]{"4"}),
    //         AtomData.Create(2,0,0,0,new string[]{"4"}),
    //         AtomData.Create(2,1,0,0,new string[]{"4"}),
    //         AtomData.Create(2,2,0,0,new string[]{"4"}),
    //         AtomData.Create(3,0,0,0,new string[]{"1"}),
    //         AtomData.Create(3,2,0,0,new string[]{"5"}),
    //         AtomData.Create(4,0,0,0,new string[]{"0"}),
    //         AtomData.Create(4,2,0,0,new string[]{"0"}),
    //     },
    //     3, "1-9"
    // );

    // public static MapData MAP_1_10 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         AtomData.Create(0,0,0,0,new string[]{"5"}),
    //         AtomData.Create(0,1,0,0,new string[]{"5", "3"}),
    //         AtomData.Create(1,1,0,0,new string[]{"4", "2"}),
    //         AtomData.Create(1,2,0,0,new string[]{"4"}),
    //         AtomData.Create(2,0,0,0,new string[]{"4"}),
    //         AtomData.Create(2,1,0,0,new string[]{"0", "0"}),
    //         AtomData.Create(2,2,0,0,new string[]{"0"}),
    //         AtomData.Create(3,0,0,0,new string[]{"1"}),
    //     },
    //     2, "1-10"
    // );

    // public static MapData MAP_2_1 = new MapData(
    //     3, 3,
    //     new List<AtomData>(){
    //         AtomData.Create(0,0,0,0,new string[]{"3"}),
    //         AtomData.Create(2,0,1,0,new string[]{"2"}),
    //         AtomData.Create(2,2,0,0,new string[]{"2"})
    //     },
    //     1, "2-1 Drag to move"
    // );

    // public static MapData MAP_2_2 = new MapData(
    //     3, 3,
    //     new List<AtomData>(){
    //         AtomData.Create(0,0,0,0,new string[]{"5"}),
    //         AtomData.Create(0,2,0,0,new string[]{"3", "4"}),
    //         AtomData.Create(1,1,1,0,new string[]{"5", "2"}),
    //         AtomData.Create(2,2,0,0,new string[]{"2"}),
    //     },
    //     2, "2-2"
    // );

    // public static MapData MAP_2_3 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         // fixed
    //         AtomData.Create(1,0,0,0,new string[]{"5"}),
    //         AtomData.Create(2,0,0,0,new string[]{"4"}),
    //         AtomData.Create(1,2,0,0,new string[]{"345"}),
    //         // movable
    //         AtomData.Create(0,1,1,0,new string[]{"3"}),
    //         AtomData.Create(2,2,1,0,new string[]{"5"}),
    //         AtomData.Create(1,1,1,0,new string[]{"1"}),
    //     },
    //     1, "2-3"
    // );

    // public static MapData MAP_2_4 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         // fixed
    //         AtomData.Create(0,1,0,0,new string[]{"0", "2"}),
    //         AtomData.Create(2,1,0,0,new string[]{"2", "2"}),
    //         // movable
    //         AtomData.Create(0,2,1,0,new string[]{"0", "4"}),
    //         AtomData.Create(1,0,1,0,new string[]{"0", "2"}),
    //         AtomData.Create(3,2,1,0,new string[]{"4", "4"}),
    //     },
    //     2, "2-4"
    // );

    // public static MapData MAP_2_5 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         // fixed
    //         AtomData.Create(0,0,0,0,new string[]{"2"}),
    //         AtomData.Create(0,2,0,0,new string[]{"0"}),
    //         AtomData.Create(3,0,0,0,new string[]{"1"}),
    //         AtomData.Create(3,2,0,0,new string[]{"0"}),
    //         // movable
    //         AtomData.Create(0,1,1,0,new string[]{"4"}),
    //         AtomData.Create(2,1,1,0,new string[]{"0"}),
    //         AtomData.Create(1,2,1,0,new string[]{"5","2"}),
    //         AtomData.Create(2,2,1,0,new string[]{"0","0"}),
    //         AtomData.Create(2,0,1,0,new string[]{"0","3"}),
    //     },
    //     1, "2-5"
    // );

    // public static MapData MAP_3_1 = new MapData(
    //     3, 1,
    //     new List<AtomData>(){
    //         // fixed
    //         AtomData.Create(0,0,0,0,new string[]{"0"}),
    //         AtomData.Create(1,0,0,0,new string[]{"3"}),

    //         // catchable
    //         AtomData.Create(2,0,2)
    //     },
    //     2, "3-1 Catch"
    // );

    // public static MapData MAP_3_2 = new MapData(
    //     3, 3,
    //     new List<AtomData>(){
    //         // fixed
    //         AtomData.Create(1,2,0,0,new string[]{"2","5"}),
    //         AtomData.Create(1,0,0,0,new string[]{"4","1"}),
    //         AtomData.Create(1,1,0,0,new string[]{"15"}),
    //         // catchable
    //         AtomData.Create(0,1,2)
    //     },
    //     2, "3-2"
    // );

    // public static MapData MAP_3_3 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         // fixed
    //         AtomData.Create(1,1,0,0,new string[]{"12"}),
    //         // movable
    //         AtomData.Create(0,0,1,0,new string[]{"0"}),
    //         AtomData.Create(0,2,1,0,new string[]{"1"}),
    //         AtomData.Create(3,0,1,0,new string[]{"2"}),
    //         AtomData.Create(3,2,1,0,new string[]{"3"}),
    //         // catchable
    //         AtomData.Create(1,2,2),
    //         AtomData.Create(2,2,2),
    //     },
    //     2, "3-3"
    // );

    // public static MapData MAP_3_4 = new MapData(
    //     3, 3,
    //     new List<AtomData>(){
    //         // fixed
    //         AtomData.Create(0,0,0,0,new string[]{"0"}),
    //         AtomData.Create(1,0,0,0,new string[]{"0"}),
    //         AtomData.Create(2,0,0,0,new string[]{"1"}),
    //         AtomData.Create(0,1,0,0,new string[]{"2"}),
    //         // movable and catchable
    //         AtomData.Create(0,2,3)
    //     },
    //     2, "3-4 Move and catch"
    // );

    // public static MapData MAP_3_5 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         // fixed
    //         AtomData.Create(0,0,0,0,new string[]{"3"}),
    //         AtomData.Create(0,1,0,0,new string[]{"4"}),
    //         AtomData.Create(1,2,0,0,new string[]{"5", "4"}),
    //         AtomData.Create(2,0,0,0,new string[]{"0"}),
    //         AtomData.Create(2,1,0,0,new string[]{"0"}),
    //         AtomData.Create(3,0,0,0,new string[]{"3"}),
    //         AtomData.Create(3,2,0,0,new string[]{"05"}),
    //         // movable and catchable
    //         AtomData.Create(0,2,3)
    //     },
    //     2, "3-5"
    // );

    // public static MapData MAP_4_1 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         // fixed
    //         AtomData.Create(0,0,0,0,new string[]{"2"}),
    //         AtomData.Create(0,1,0,0,new string[]{"15"}),
    //         AtomData.Create(2,0,0,0,new string[]{"1"}),
    //         AtomData.Create(1,1,0,0,new string[]{"24"}),
    //         // movable
    //         AtomData.Create(1,2,1,0,new string[]{"24", "135"}),
    //         // movable and catchable
    //         AtomData.Create(0,2,3),
    //         AtomData.Create(3,2,3)
    //     },
    //     2, "4-1 All-round"
    // );

    // public static MapData MAP_4_2 = new MapData(
    //     4, 3,
    //     new List<AtomData>(){
    //         // fixed
    //         AtomData.Create(0,1,0,0,new string[]{"3"}),
    //         AtomData.Create(1,1,0,0,new string[]{"5"}),
    //         AtomData.Create(3,2,0,0,new string[]{"0"}),
    //         // movable
    //         AtomData.Create(1,0,1,0,new string[]{"24", "03"}),
    //         AtomData.Create(2,1,1,0,new string[]{"23"}),
            
    //         // movable and catchable
    //         AtomData.Create(0,2,3),
    //         AtomData.Create(1,2,3),
    //         AtomData.Create(2,2,3),
    //     },
    //     2, "4-2"
    // );

    // private static List<MapData> dataList = new List<MapData>{
    //     MAP_TEST_LARGE, 
    //     // 普通原子、步数
    //     MAP_1_1, MAP_1_2, MAP_1_3, MAP_1_4, MAP_1_5,
    //     MAP_1_6, MAP_1_7, MAP_1_8, MAP_1_9, MAP_1_10,
    //     // // 可移动
    //     MAP_2_1, MAP_2_2, MAP_2_3, MAP_2_4, MAP_2_5,
    //     // // 可捕获
    //     MAP_3_1, MAP_3_2, MAP_3_3, MAP_3_4, MAP_3_5,
    //     // // 综合
    //     MAP_4_1, MAP_4_2,
    // };
    
    // [Obsolete("Please use MapJsonReader.MapCount instead!")]
    // public static int MapCount(){
    //     return dataList.Count;
    // }

    // [Obsolete("Please use MapJsonReader.GetMapAtIndex instead!")]
    // public static MapData GetAtIndex(int index){
    //     if(index < 0 || index > dataList.Count-1) return MAP_TEST;
    //     return dataList[index];
    // }
    
}
