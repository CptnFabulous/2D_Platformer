using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceMeter : MonoBehaviour
{
    public Color filledColour;
    public Color criticalColour;
    public Color emptyColour;

    [HideInInspector] public Resource previousValues;

    //RectTransform rt;


    public abstract void Generate(Resource values);
    public abstract void Refresh(Resource resource);
}
