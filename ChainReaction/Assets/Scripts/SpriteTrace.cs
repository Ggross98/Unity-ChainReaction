using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrace : MonoBehaviour 
{
    [SerializeField] private GameObject target, traceObjectPrefab;
    [SerializeField] private Transform traceParent;

    private List<GameObject> traceList;

    [SerializeField] private int num = 5;
    [SerializeField] private float interval = 0.03f, alpha0 = 1, alpha1 = 0.1f;
    [SerializeField] private Vector3 scale0 = new Vector3(0.5f, 0.5f), scale1 = new Vector3(0.1f, 0.1f);


    void Start()
    {
        CreateTraceObjects(num);
    }

    public void ShowTrace(){
        if(target == null || traceObjectPrefab == null || traceParent == null) return;

        
        var color = target.GetComponent<SpriteRenderer>().color;
        var color0 = Utils.GetColor(color, alpha0);
        var color1 = Utils.GetColor(color, alpha1);

        StartCoroutine(ActivateTraceObjects(num, interval, color0, color1, scale0, scale1));

    }

    public void HideTrace(){
        StopAllCoroutines();
        foreach(var obj in traceList) obj.SetActive(false);
    }

    private void CreateTraceObjects(int num){
        traceList = new List<GameObject>();

        for(int i = 0; i<num; i++){
            var obj = Instantiate(traceObjectPrefab, traceParent);
            traceList.Add(obj);
            obj.SetActive(false);
        }
    }

    private IEnumerator ActivateTraceObjects(int num, float interval, Color color0, Color color1, Vector3 scale0, Vector3 scale1){

        var deltaColor = new Color();
        var deltaScale = new Vector3();
        if(num > 1){
            deltaColor = (color1 - color0) / num;
            deltaScale = (scale1 - scale0) / num;
        }

        var pos0 = target.transform.position;
        for(int i = 0; i<num; i++){
            var obj = traceList[i];

            obj.GetComponent<SpriteRenderer>().color = color0 + deltaColor * i;
            obj.transform.localScale = scale0 + deltaScale * i;
            obj.transform.position = pos0;
            obj.SetActive(true);
            
            float timer = 0;
            while(timer < interval){
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            
        }    
        yield return null;
    }
}
