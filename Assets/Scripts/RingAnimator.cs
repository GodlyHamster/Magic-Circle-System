using Unity.Mathematics;
using UnityEngine;

public class RingAnimator : MonoBehaviour
{
    private Transform _origin;
    
    public AnimationCurve xPos;
    public AnimationCurve yPos;
    public AnimationCurve zPos;
    [Space]
    public AnimationCurve xRot;
    public AnimationCurve yRot;
    public AnimationCurve zRot;

    private void Start()
    {
        _origin = gameObject.transform;
        
        if (xPos.length == 0){ xPos.AddKey(new Keyframe(0, _origin.position.x)); xPos.AddKey(new Keyframe(1, _origin.position.x));}
        if (yPos.length == 0){ yPos.AddKey(new Keyframe(0, _origin.position.y)); yPos.AddKey(new Keyframe(1, _origin.position.y));}
        if (zPos.length == 0){ zPos.AddKey(new Keyframe(0, _origin.position.z)); zPos.AddKey(new Keyframe(1, _origin.position.z));}

        if (xRot.length == 0){ xRot.AddKey(new Keyframe(0, _origin.rotation.x/2)); xRot.AddKey(new Keyframe(1, _origin.rotation.x/2));}
        if (yRot.length == 0){ yRot.AddKey(new Keyframe(0, _origin.rotation.y/2)); yRot.AddKey(new Keyframe(1, _origin.rotation.y/2));}
        if (zRot.length == 0){ zRot.AddKey(new Keyframe(0, _origin.rotation.z/2)); zRot.AddKey(new Keyframe(1, _origin.rotation.z/2));}
    }

    void Update()
    {
        float t = Time.time;
        transform.position = new Vector3(xPos.Evaluate(t), yPos.Evaluate(t), zPos.Evaluate(t));
        
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation = new Vector3(xRot.Evaluate(t), yRot.Evaluate(t), zRot.Evaluate(t)) * (Mathf.PI * 2);
        transform.rotation = quaternion.Euler(rotation);
    }
}
