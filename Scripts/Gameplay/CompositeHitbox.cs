using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeHitbox : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 size;
    [SerializeField] private BoxCollider2D compositeHitbox;

    public Vector2 GetOffset()
    {
        if (compositeHitbox == null) return offset;
        else return compositeHitbox.offset;
    }

    public Vector2 GetSize()
    {
        if (compositeHitbox == null) return size;
        else return compositeHitbox.size;
    }

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
