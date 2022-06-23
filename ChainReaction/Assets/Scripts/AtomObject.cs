using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomObject : MonoBehaviour
{
    [SerializeField] private GameObject neutronPrefab, nuclei;
    [SerializeField] private Transform neutronParent;
    [SerializeField] private Transform[] neutronPosition = new Transform[3];
    [SerializeField] private GameObject[] orbits = new GameObject[3];
    [SerializeField] private GameObject[] orbits_dotted = new GameObject[1];
    // [SerializeField] private GameObject[] arrows = new GameObject[2];
    [SerializeField] private GameObject arrow;
    
    // 音效
    [SerializeField] private AudioClip collapseSound;
    private List<NeutronData>[] neutronDatas;
    private List<NeutronObject>[] neutronObjectList;
    // private AtomData data;

    private int layer, type, group;

    public int currentLayer;
    
    private new CircleCollider2D collider;
    private Color nucleiNormalColor, nucleiHighlightColor;
    private Vector3 nucleiScale;

    private bool collapsed = false;

    private float hightLightScale = 1.1f;
    public static List<Color> normalColorList = new List<Color>{
        Utils.GetColor(33,33,33),
        Utils.GetColor(83,33,180),
        Utils.GetColor(180,83,33),
        Utils.GetColor(33,180,83),
    };

    public static List<Color> highlightColorList = new List<Color>{
        Utils.GetColor(77,77,77),
        Utils.GetColor(130,130,180),
        Utils.GetColor(180,130,130),
        Utils.GetColor(130,180,130),
    };

    void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        // neutronList = new List<NeutronObject>();
    }

    public void Init(AtomData d){

        layer = d.layer;
        group = d.group;
        type = d.type;
        neutronDatas = d.neutronLists;

        // 创造粒子
        neutronObjectList = new List<NeutronObject>[layer];
        // 单层原子
        if(layer == 1){
            neutronObjectList[0] = new List<NeutronObject>();
            var ndList = neutronDatas[0];
            foreach(var nd in ndList){

                var neutron = CreateNeutron(nd);
                CatchNeutron(neutron, 1, 1);
            }
        // 双层原子
        }else if(layer == 2){
            float[] length = new float[2]{Mathf.Abs(neutronPosition[0].position.x - nuclei.transform.position.x), Mathf.Abs(neutronPosition[2].position.x - nuclei.transform.position.x)};
            
            for(int i = 0; i < 2; i++){
                neutronObjectList[i] = new List<NeutronObject>();
                var ndList = neutronDatas[i];
                foreach(var nd in ndList){
                    var neutron = CreateNeutron(nd);
                    CatchNeutron(neutron, i == 1 ? 2 : 0, 2);
                }
            }

        }

        //
        currentLayer = layer - 1;
        
        // 显示效果
        Refresh();
        if(IsEntangled()) nucleiNormalColor = normalColorList[Group()];
        else nucleiNormalColor = normalColorList[0];
        if(IsMovable()) nucleiHighlightColor = nucleiNormalColor;
        else nucleiHighlightColor = highlightColorList[Group()];
        // nucleiNormalColor = Utils.GetColor(33,33,33);
        nucleiScale = nuclei.transform.localScale;
        nuclei.GetComponent<SpriteRenderer>().color = nucleiNormalColor;


        // Test
        // if(IsEntangled()){
        //     Debug.Log("entangled. group is " + group);
        // }
    }

    private void CatchNeutron(NeutronObject neutron, int orbitIndex, int layer){


        var length = Mathf.Abs(neutronPosition[orbitIndex].position.x - nuclei.transform.position.x) / transform.localScale.x;
        var pos = neutron.dir.normalized * length;

        neutron.SetPosition(pos);
        int i;
        if(layer == 1) i = 0;
        else {
            if(orbitIndex == 2) i = 1;
            else i = 0;
        }
        neutronObjectList[i].Add(neutron);
    }

    public int Group(){
        return group;
    }

    public int Type(){
        return type;
    }

    public bool IsMovable(){
        return type == AtomData.MOVABLE || type == AtomData.CATCHING_MOVABLE || type == AtomData.ENTANGLED_MOVABLE;
    }

    public bool IsCatchable(){
        return type == AtomData.CATCHING || type == AtomData.CATCHING_MOVABLE;
    }

    public bool IsEntangled(){
        return type == AtomData.ENTANGLED || type == AtomData.ENTANGLED_MOVABLE;
    }

    private void Refresh(){
        // 显示轨道
        if(!IsCatchable()){
            orbits[0].SetActive(layer > 1);
            orbits[1].SetActive(layer == 1);
            orbits[2].SetActive(layer > 1);
            orbits_dotted[0].SetActive(false);
        }
        else{
            orbits[0].SetActive(false);
            orbits[1].SetActive(false);
            orbits[2].SetActive(false);
            orbits_dotted[0].SetActive(true);
        }
        // 显示箭头
        if(!IsMovable()){
            arrow.SetActive(false);
        }
        else{
            arrow.SetActive(true);
        }
    }

    public void CatchForeighNeutron(NeutronObject neutron, int orbitIndex){
        
        // Debug.Log("Start catching foreign neutron!");
        neutron.Move(false);
        neutron.Bounce();
        CatchNeutron(neutron, orbitIndex, 1);
        

        StartCoroutine(DelayChangeTypeAfterCatch(0.1f));
        
    }

    // 捕获原子后，短暂维持时间后改变type，以支持同时捕获多个原子
    private IEnumerator DelayChangeTypeAfterCatch(float time){
        yield return new WaitForSeconds(time);
        if(type == AtomData.CATCHING){
            type = AtomData.NORMAL;
        }else if(type == AtomData.CATCHING_MOVABLE){
            type = AtomData.MOVABLE;
        }
        Refresh();
        yield return null;
    }

    private NeutronObject CreateNeutron(NeutronData nd){
        var obj = Instantiate(neutronPrefab, neutronParent);
        var neutron = obj.GetComponent<NeutronObject>();
        neutron.Init(nd);
        return neutron;
    }

    public void Clear(){
        if(neutronObjectList == null) return;
        else{
            for(int i = 0; i<neutronObjectList.Length; i++){
                var list = neutronObjectList[i];

                for(int j = 0; j<list.Count; j++){
                    var neutron = list[0];
                    list.RemoveAt(0);
                    if(neutron != null) Destroy(neutron.gameObject);
                }
                
                neutronObjectList[i] = null;
            }
        }
    }

    public void ReleaseOuterOrbit(){
        if(collapsed) return;
        
        var index = layer == 1 ? 0 : currentLayer;
        var list = neutronObjectList[index];
        foreach(var neutron in list){
            neutron.Release();

            // 将对象加入胜利/失败检测列表
            MapController.Instance.AddNeutronToChecklist(neutron);
        }

        var orbitIndex = layer == 1 ? 1 : (currentLayer == 0 ? 0 : 2);
        //orbits[orbitIndex].SetActive(false);
        StartCoroutine(ReleaseOrbit(orbitIndex, 5f, 0.1f));

        currentLayer --;
        if(currentLayer < 0){
            Collapse();
        }
    }

    private IEnumerator ReleaseOrbit(int index, float speed, float t, float interval = 0.02f){

        var orbit = orbits[index];
        var distance = Mathf.Abs(neutronPosition[index].transform.position.x - nuclei.transform.position.x);
        var scale0 = orbit.transform.localScale;
        var renderer = orbit.GetComponent<SpriteRenderer>();
        var color0 = renderer.color;
        var alpha0 = color0.a;
        float timer = 0;
        while(timer < t){

            var alpha = alpha0 * (t - timer) / t;
            orbit.GetComponent<SpriteRenderer>().color = Utils.GetColor(color0, alpha);

            orbit.transform.localScale = scale0 * (speed * timer + distance)/distance;    

            timer += interval;
            yield return new WaitForSeconds(interval);
        }

        orbit.SetActive(false);

        yield return null;
    }


    public void Collapse(){
        SoundController.Instance.PlaySoundEffect(collapseSound);

        arrow.SetActive(false);
        collider.enabled = false;
        collapsed = true;
        MapController.Instance.RemoveAtomObject(this);

        StartCoroutine(NucleiCollapse(0.2f));

        
    }

    public bool IsCollapsed(){
        return collapsed;
    }

    public void SetLocalPosition(Vector2 pos){
        transform.localPosition = pos;
    }

    public void SetWorldPosition(Vector2 pos){
        transform.position = pos;
    }

    private IEnumerator NucleiCollapse(float t, float interval = 0.02f){

        var scale0 = nuclei.transform.localScale;
        float timer = 0f;
        while(timer <= t){
            nuclei.transform.localScale = scale0 * (t - timer) / t;
            timer += interval;
            yield return new WaitForSeconds(interval);
        }
        nuclei.SetActive(false);
        yield return null;
    }

    private void OnMouseOver() {
        if(!MapController.Instance.CanOperateAtoms() || collapsed) return;
        nuclei.GetComponent<SpriteRenderer>().color = nucleiHighlightColor;
        nuclei.transform.localScale = nucleiScale * hightLightScale;
        // Debug.Log("Atom Object: Mouse Enter!");
        // switch(type){
            // case AtomData.NORMAL:
            //     nuclei.GetComponent<SpriteRenderer>().color = nucleiHighlightColor;
            //     nuclei.transform.localScale = nucleiScale * 1.1f;
            //     break;
            // case AtomData.MOVABLE:
            // case AtomData.CATCHING_MOVABLE:
            //     nuclei.transform.localScale = nucleiScale * 1.1f;
            //     break;

        // }
        
    }

    private void OnMouseDown() {
        if(!MapController.Instance.CanOperateAtoms() || collapsed) return;

        switch(type){
            case AtomData.NORMAL:
            case AtomData.ENTANGLED:
                MapController.Instance.DoRelease(this);
                nuclei.GetComponent<SpriteRenderer>().color = nucleiNormalColor;
                nuclei.transform.localScale = nucleiScale;
                break;
            case AtomData.MOVABLE:
            case AtomData.CATCHING_MOVABLE:
            case AtomData.ENTANGLED_MOVABLE:
                nuclei.transform.localScale = nucleiScale * hightLightScale;
                MapController.Instance.StartDrag(this);
                break;
            case AtomData.CATCHING:
                nuclei.transform.localScale = nucleiScale * hightLightScale;
                break;
        }

    }

    private void OnMouseUp()
    {
        if(collapsed) return;
        switch(type){
            case AtomData.MOVABLE:
            case AtomData.CATCHING_MOVABLE:
            case AtomData.ENTANGLED_MOVABLE:
                MapController.Instance.EndDrag();
                break;
        }
    }

    private void OnMouseExit() {
        if(!MapController.Instance.CanOperateAtoms() || collapsed) return;
        nuclei.GetComponent<SpriteRenderer>().color = nucleiNormalColor;
        nuclei.transform.localScale = nucleiScale;
        // Debug.Log("Atom Object: Mouse Enter!");
        // switch(type){
        //     case AtomData.NORMAL:
        //         nuclei.GetComponent<SpriteRenderer>().color = nucleiNormalColor;
        //         nuclei.transform.localScale = nucleiScale;
        //         break;
        //     case AtomData.MOVABLE:
        //         nuclei.transform.localScale = nucleiScale;
        //         break;
        //     case AtomData.CATCHING:
        //         nuclei.transform.localScale = nucleiScale;
        //         break;
        //     case AtomData.CATCHING_MOVABLE:
        //         nuclei.transform.localScale = nucleiScale;
        //         break;
        // }
        
    }
    
}
