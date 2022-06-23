using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCopy : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parent;

    [SerializeField] private int num;
    [SerializeField] private Vector3 angle0, deltaAngle;

    void Start()
    {
        Copy(num, angle0, deltaAngle);
    }

    public void Copy(int count, Vector3 angel0, Vector3 dAngel){
        for(int i = 0; i<count; i++){
            var angel = angel0 + dAngel * i;

            var obj = Instantiate(prefab, parent);
            obj.transform.rotation = Quaternion.Euler(angel);

        }
    }
}
