using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentedMeter : ResourceMeter
{
    
    public Image prefab;
    public Vector3[] offsets;

    Image[] segments;


    public override void Generate(Resource values)
    {
        //rt = GetComponent<RectTransform>();

        prefab.gameObject.SetActive(true);

        Vector3 anchoredPosition = prefab.rectTransform.anchoredPosition;
        int offsetIndex = 0;

        segments = new Image[values.max];
        for (int i = 0; i < segments.Length; i++)
        {
            segments[i] = Instantiate(prefab, transform);
            segments[i].rectTransform.anchoredPosition = anchoredPosition;

            anchoredPosition += offsets[offsetIndex];
            offsetIndex++;
            if (offsetIndex >= offsets.Length)
            {
                offsetIndex = 0;
            }
        }
        prefab.gameObject.SetActive(false);
        Refresh(values);
    }

    public override void Refresh(Resource resource)
    {
        Color fillColour = resource.critical ? criticalColour : filledColour;
        for (int i = 0; i < segments.Length; i++)
        {
            
            segments[i].color = (i + 1 <= resource.current) ? fillColour : emptyColour;
        }
        previousValues = resource;
        /*
        // Counts up only through the values making up the difference between the old and new value. The other icons don't need to change.
        int min = Mathf.Max(1, previousValues.current);
        if (resource.critical == previousValues.critical)
        {
            min = Mathf.Min(min, resource.current);
        }
        int max = Mathf.Max(previousValues.current, resource.current);
        Color fillColour = resource.critical ? criticalColour : filledColour;
        for (int i = min; i < max; i++)
        {
            Debug.Log(i - 1);
            segments[i - 1].color = (i <= resource.current) ? fillColour : emptyColour;
        }
        previousValues = resource;
        */
    }

    
    
}
