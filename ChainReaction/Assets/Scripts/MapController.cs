using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController: SingletonMonoBehaviour<MapController>
{
    [SerializeField] private GameObject atomPrefab, positionMarkPrefab;
    [SerializeField] private Transform atomParent, positionMarkParent;
    // [SerializeField] private Vector2 distance = new Vector2(1,1);

    // ************************************** 游戏状态控制 **************************************
    private AtomObject[,] atomMap;
    private GameObject[,] positionMarkMap;
    private MapData mapData;
    // private MapData mapData = MapData.MAP_TEST;
    private int leftSteps = 1;
    public const int WAITING = 0, REACTING = 1, WIN = 2, LOSE = 3, NOTSTARTED = -1, DRAGGING = 4;
    public int status = NOTSTARTED;
    private List<NeutronObject> neutronChecklist;
    private AtomObject draggingAtom = null;
    private Vector2Int lastMapPos;
    private List<AtomObject> entangledAtomList;

    // ************************************** 地图显示大小 **************************************
    private Vector3 globalSize = Vector3.one;
    private float distance = 3;

    // **************************************   UI控制    **************************************
    private GameUIPresenter ui;

    void Awake(){

    }

    void Start()
    {
        ui = GameUIPresenter.Instance;
        StartGame();
    }

    void Update()
    {

        switch(status){
            case WAITING:
                break;
            case REACTING:
                // 将运行完毕的中子对象从检查列表删除
                for(int i = neutronChecklist.Count -1; i>=0; i--){
                    var n = neutronChecklist[i];
                    if(n == null || !n.isActiveAndEnabled) neutronChecklist.Remove(n);
                }
                // 所有中子运行完毕后进行胜负检查
                if(neutronChecklist.Count == 0){
                    if(CheckWin()){
                        Win();
                    }else if(leftSteps > 0){
                        status = WAITING;
                    }else{
                        Lose();
                    }
                }
                break;
            case DRAGGING:
                if(draggingAtom == null) break;
                // 转换鼠标位置
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                draggingAtom.SetWorldPosition(mousePos);
                break;
            case WIN:
                break;
            case LOSE:
                break;
        }

        ui.ShowTitle(mapData.name);
        ui.ShowLeftSteps(leftSteps);
        ui.ShowStateInfo(status);

        // if(Input.GetKeyDown(KeyCode.R)){
        //     StartGame();
        // }
    }

    public bool IsInMapRange(Vector2Int pos){
        if(pos.x < 0 || pos.y < 0) return false;
        if(pos.y >= mapData.height) return false;
        if((pos.y % 2 == 0 && pos.x >= mapData.width) || (pos.y % 2 == 1 && pos.x >= mapData.width - 1)) return false;

        return true;
    }

    private void Win(){
        status = WIN;
        NextLevel();
    }

    private void Lose(){
        status = LOSE;
        Restart();
    }

    public void Restart(){
        StartCoroutine(DelayStart(2f));
    }

    public void RestartImmediate(){
        StopAllCoroutines();
        StartGame();
    }

    private IEnumerator DelayStart(float time){
        yield return new WaitForSeconds(time);
        StartGame();
        yield return null;
    }

    public void NextLevel(){
        AccountController.Instance.NextLevel();
        StartCoroutine(DelayStart(2f));
    }   

    /// <summary>
    /// 将原子对象从地图原位置上移除，但不删除其对象。
    /// 用于移动原子对象。
    /// </summary>
    /// <param name="atom"></param>
    public void RemoveAtomObject(AtomObject atom){
        for(int i = 0; i<mapData.width; i++){
            for(int j = 0; j<mapData.height;j++){
                if(atomMap[i,j] == atom){
                    atomMap[i,j] = null;
                    return;
                }
            }
        }    
    }

    public void Exit(){
        StopAllCoroutines();
        SceneManager.LoadScene("Menu");
    }

    public bool CheckWin(){
        for(int i = 0; i < atomMap.GetLength(0); i++){
            for(int j = 0; j < atomMap.GetLength(1); j++){
                var atom = atomMap[i,j];
                if(atom!=null){
                    if(!atom.IsCollapsed()) return false;
                }
            }
        }
        return true;
    }

    public bool CanOperateAtoms(){
        return status == WAITING;
    }

    public void DoRelease(AtomObject atom){
        ReleaseAtom(atom);

        leftSteps --;
    }

    public void ReleaseAtom(AtomObject atom, List<AtomObject> releasedEntangledAtomList = null){
        atom.ReleaseOuterOrbit();
        status = REACTING;
        // 量子纠缠
        if(atom.IsEntangled()){
            // Debug.Log("Test entangled");
            var group = atom.Group();
            
            var toRelease = new List<AtomObject>();

            foreach( var eAtom in entangledAtomList){
                if( eAtom == atom || toRelease.Contains(eAtom) || eAtom.IsCollapsed() || eAtom.Group() != group) continue;
                if( releasedEntangledAtomList!= null && releasedEntangledAtomList.Contains(eAtom)) continue;
                toRelease.Add(eAtom);
            }

            if( releasedEntangledAtomList == null) releasedEntangledAtomList = new List<AtomObject>();
            releasedEntangledAtomList.AddRange(toRelease);
            releasedEntangledAtomList.Add(atom);

            foreach( var eAtom in toRelease){
                ReleaseAtom(eAtom, releasedEntangledAtomList);
            }
        }
    }

    public void DoCatch(AtomObject atom, NeutronObject neutron){
        neutron.transform.SetParent(atom.transform.Find("NeutronParent"));
        if(neutronChecklist.Contains(neutron)) neutronChecklist.Remove(neutron);
    }

    public void StartDrag(AtomObject obj){
        if(status == DRAGGING) return;

        status = DRAGGING;
        draggingAtom = obj;
        lastMapPos = WorldToMapPosition(obj.transform.position);
    }

    public void EndDrag(){
        // draggingAtom.SetPosition(draggingAtomLastPosition);
        if(status == DRAGGING){
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mapPos = WorldToMapPosition(mousePos);

            Vector2Int newMapPos = lastMapPos;
            if(IsInMapRange(mapPos) && mapPos != lastMapPos){
                var x = mapPos.x;
                var y = mapPos.y;
                
                if(atomMap[x,y] == null){
                    newMapPos = mapPos;
                    atomMap[lastMapPos.x, lastMapPos.y] = null;
                    atomMap[x,y] = draggingAtom;
                }   
            }
            draggingAtom.SetLocalPosition(MapToLocalPosition(newMapPos));
            
            var lPos = WorldToLocalPosition(mousePos);
            // Debug.Log("Stop dragging. The final position is "+ lPos + ", " + newMapPos);
            
            draggingAtom = null;
            status = WAITING;
        }
        
    }

    public void StartGame(){
        LoadMap(AccountController.Instance.GetCurrentLevelMapData());

        neutronChecklist = new List<NeutronObject>();
        status = WAITING;

    }

    public void AddNeutronToChecklist(NeutronObject neutron){
        neutronChecklist.Add(neutron);
    }

    public void LoadMap(MapData d){
        mapData = d;
        // Debug.Log(mapData);
        Clear();

        // 刷新地图
        
        atomMap = new AtomObject[mapData.width, mapData.height];
        
        // 根据地图元素数量决定尺寸
        if(mapData.width >= 6 || mapData.height >= 5) distance = 1.8f;
        else distance = 3;
        var lPos = LocalToWorldPosition(Vector2.zero);
        atomParent.localPosition = lPos;

        // 显示位置标记
        positionMarkMap = new GameObject[mapData.width, mapData.height];
        positionMarkParent.localPosition = lPos;
        for(int i = 0; i<mapData.width; i++){
            for(int j = 0; j<mapData.height; j++){
                if( j % 2 == 1 && i == mapData.width - 1) continue;    
                var obj = Instantiate(positionMarkPrefab, positionMarkParent);
                obj.name = "Position "+ i + ", "+ j;
                obj.transform.localPosition = MapToLocalPosition(new Vector2Int(i,j));
                positionMarkMap[i,j] = obj;
            }
        }

        // 创建原子

        entangledAtomList = new List<AtomObject>();

        foreach(var ad in mapData.atomList){

            var atom = CreateAtom(ad);

            var mPos = ad.pos;
            atomMap[mPos.x, mPos.y] = atom;
            atom.name = "Atom "+mPos.x + ", "+mPos.y;
            atom.SetLocalPosition(MapToLocalPosition(mPos));

            if(atom.IsEntangled()){
                entangledAtomList.Add(atom);
            }
            
        }

        leftSteps = mapData.steps;
        // Debug.Log("Map Loaded");
    }



    public Vector2 MapToLocalPosition(Vector2Int mPos){
        var mX = mPos.x;
        var mY = mPos.y;

        var x = mY %2 == 0 ? mX * distance : mX * distance + distance / 2f;
        var y = mY / 2f * Mathf.Sqrt(3f) * distance;
    
        return new Vector2( x, y );
    }

    public Vector2 MapToWorldPosition(Vector2Int mPos){
        var mX = mPos.x;
        var mY = mPos.y;

        var x = mY %2 == 0 ? mX * distance : mX * distance + distance / 2f;
        var y = mY / 2f * Mathf.Sqrt(3f) * distance;

        var deltaX = (mapData.width - 1) / 2f * distance;
        var deltaY = (mapData.height - 1) / 2f / 2f * Mathf.Sqrt(3f) * distance;
    
        return new Vector2( x -deltaX, y -deltaY );
    }



    public Vector2 LocalToWorldPosition(Vector2 wPos){
        var deltaX = (mapData.width - 1) / 2f * distance;
        var deltaY = (mapData.height - 1) / 2f / 2f * Mathf.Sqrt(3f) * distance;
        return new Vector2( wPos.x - deltaX, wPos.y - deltaY);
    }

    public Vector2 WorldToLocalPosition(Vector2 lPos){
        var deltaX = (mapData.width - 1) / 2f * distance;
        var deltaY = (mapData.height - 1) / 2f / 2f * Mathf.Sqrt(3f) * distance;
        return new Vector2( lPos.x + deltaX, lPos.y + deltaY);
    }

    public Vector2Int LocalToMapPosition(Vector2 lPos){

        var width = distance;
        var height = distance / 2f * Mathf.Sqrt(3f);
        var dx = width / 2f;
        var dy = height / 2f;

        var y = Mathf.FloorToInt((lPos.y + dy )/ height);
        var x = y % 2 == 0 ? Mathf.FloorToInt((lPos.x + dx) / distance) : Mathf.FloorToInt(lPos.x / width );
        return new Vector2Int(x, y);
    }

    public Vector2Int WorldToMapPosition(Vector2 wPos){
        return LocalToMapPosition(WorldToLocalPosition(wPos));
    }

    public void Clear(){

        atomMap = null;
        positionMarkMap = null;

        for (int i = 0; i < atomParent.childCount; i++) {
			Destroy (atomParent.GetChild (i).gameObject);
		}
        for (int i = 0; i < positionMarkParent.childCount; i++) {
			Destroy (positionMarkParent.GetChild (i).gameObject);
		}

    }

    private AtomObject CreateAtom(AtomData ad){

        var obj = Instantiate(atomPrefab, atomParent);
        var atom = obj.GetComponent<AtomObject>();
        atom.Init(ad);

        return atom;
    }


}