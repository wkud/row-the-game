using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class RiverSegment : MonoBehaviour
{
    public Vector3 shift => end.position - start.position; //difference between start and end of segment
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    public bool WillFitWithinScreenBorders(Vector3 spawnPosition, float leftBorderX, float rightBorderX)
    {
        var endPosition = spawnPosition + shift; 
        return endPosition.x > leftBorderX && endPosition.x < rightBorderX;
    }
    public void PlaceStartOfSegmentAt(Vector3 spawnPosition)
    {
        var offsetFromPivotToStartOfSegment = start.position - transform.position; //difference between current position and (desired) start position
        transform.position = spawnPosition - offsetFromPivotToStartOfSegment;
    }
}
