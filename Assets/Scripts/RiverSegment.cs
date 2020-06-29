using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public enum RiverSegmentType
{
    Unassigned,
    StraightRiver,
    LargeCurve,
    Meander,
    SlightCurve,
    SlightUnevenCurve
}

public class RiverSegment : MonoBehaviour
{
    [SerializeField] public RiverSegmentType type;
    public UnityAction onExitScreen;

    public Vector3 shift => end.position - start.position; //difference between start and end of segment
    public Vector3 endPosition => new Vector3(end.position.x, 0, end.position.z); //ignore Y coordinate
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    private River river;
    void Awake() => river = FindObjectOfType<River>();
    void FixedUpdate()
    {
        transform.position += Vector3.back * river.FlowSpeed; 

        if (end.position.z < river.bottomScreenBorderZ)
            onExitScreen();
    }

    public bool WillFitWithinScreenBorders(Vector3 spawnPosition, float leftBorderX, float rightBorderX)
    {
        var endPosition = spawnPosition + shift; 
        return endPosition.x > leftBorderX && endPosition.x < rightBorderX;
    }
    public void PlaceStartOfSegmentAt(Vector3 spawnPosition)
    {
        var prefabVerticalOffset = transform.position.y; //save y offset

        var offsetFromPivotToStartOfSegment = start.position - transform.position; //difference between current position and (desired) start position
        transform.position = spawnPosition - offsetFromPivotToStartOfSegment;

        transform.position += Vector3.up * prefabVerticalOffset; //restore y offset
    }
}
