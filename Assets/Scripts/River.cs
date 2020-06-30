﻿using Assets.Scripts.Exceptions;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class River : MonoBehaviour
{
    public float bottomScreenBorderZ { get; private set; }
    [SerializeField] public float FlowSpeed;

    [SerializeField] private RiverSegment[] riverSegments;
    [SerializeField] private RiverSegment startSegment;
    private RiverSegmentPool pool = new RiverSegmentPool();
    private Transform segmentParent;
    private RiverSegment lastSegment;

    private Vector3 spawnPosition => lastSegment != null ? lastSegment.endPosition : firstSpawnPosition;
    private Vector3 firstSpawnPosition;
    private float leftBorderX;
    private float rightBorderX;
    private float endPositionZ;
    private bool canGenerate => spawnPosition.z < endPositionZ;

    void Awake()
    {
        //validate river segments
        foreach (var segment in riverSegments)
        {
            if (segment.shift.z < 0)
                throw new BadRiverSegmentException(segment.name);
        }

        //get references to children
        segmentParent = transform.Find("segments");
        var config = transform.Find("config points").transform;
        
        //find objects indicating positional parameters
        GetParameters(config);
    }
    private void GetParameters(Transform config)
    {
        var start = config.Find("start"); //indicates position of first segment and Z coordinate of bottom screen border
        var end = config.Find("end"); //indicates Z coordinate where generation should stop
        var leftBorder = config.Find("left screen border"); //indicates X coordinate of left screen border
        var rightBorder = config.Find("right screen border"); //indicates X coordinate of right screen border

        endPositionZ = end.position.z;
        leftBorderX = leftBorder.position.x;
        rightBorderX = rightBorder.position.x;
        bottomScreenBorderZ = start.position.z;
        firstSpawnPosition = start.position;
    }

    void Start()
    {
        PlaceSegment(startSegment);
        GenerateRiver();
    }

    private void GenerateRiver()
    {
        while(canGenerate)
        {
            var availableSegments = riverSegments.Where(s => s
                .GetComponent<RiverSegment>()
                .WillFitWithinScreenBorders(spawnPosition, leftBorderX, rightBorderX));
            
            var randomSegment = RandomUtility.RandomizeFrom(availableSegments);
            PlaceSegment(randomSegment);
        }
    }
    private void PlaceSegment(RiverSegment segmentPrefab)
    {
        //get segment from pool or instatiate it
        var segment = pool.TryPop(segmentPrefab.type); //try get already instantiated segment from pool
        if (segment == null) 
        {
            var segmentGameObject = Instantiate(segmentPrefab.gameObject, segmentParent);
            segment = segmentGameObject.GetComponent<RiverSegment>();
            segment.onExitScreen += delegate 
            {
                pool.Push(segment);
                GenerateRiver();
            };
        }

        //set segment up
        segment.PlaceStartOfSegmentAt(spawnPosition + Vector3.back * FlowSpeed); //predict position in next frame (to fix gaps between segments)

        lastSegment = segment;
    }
}
