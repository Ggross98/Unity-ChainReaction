using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutronObject : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    // private NeutronData data;

    private float speed = 6f;
    private SpriteTrace trace;
    public Vector3 dir;
    // public int layer = -1;

    public void Init(NeutronData d){
        rigidbody = GetComponent<Rigidbody2D>();
        dir = d.dir;
    }

    void Start()
    {
        trace = GetComponentInChildren<SpriteTrace>();
    }

    public void SetPosition(Vector2 pos){
        transform.localPosition = pos;
    }

    public void Release(){
        
        Move();
        // Debug.Log(data.dir + ", " + rigidbody.velocity);

        trace = GetComponentInChildren<SpriteTrace>();
        if(trace != null) trace.ShowTrace();
    }

    public void Move(bool m = true){
        if(m) rigidbody.velocity = dir.normalized * speed;
        else rigidbody.velocity = Vector2.zero;
    }

    public void Bounce(){
        dir = -dir;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log(other.gameObject.name);
        if(MapController.Instance.status == 4) return;
        if(other.CompareTag("Wall")){
            Destroy(gameObject);
        }else if(other.CompareTag("Nuclei")){
            // Debug.Log("Enter nuclei");
            var atom = other.GetComponentInParent<AtomObject>();
            if(atom != null){
                // atom.ReleaseOuterOrbit();
                MapController.Instance.ReleaseAtom(atom);
                Destroy(gameObject);
            }
        }else if(other.CompareTag("Orbit")){
            var atom = other.GetComponentInParent<AtomObject>();
            if(atom != null && atom.IsCatchable()){
                trace.HideTrace();
                MapController.Instance.DoCatch(atom, this);
                atom.CatchForeighNeutron(this, 1);
            }
        }
    }
    
}
