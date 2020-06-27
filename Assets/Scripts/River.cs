using Assets.Scripts.Exceptions;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class River : MonoBehaviour
{
    [SerializeField] private GameObject[] riverSegments;
    [SerializeField] private GameObject startSegment;
    private Transform segmentParent;
    
    private Vector3 spawnPosition;
    private float leftBorderX;
    private float rightBorderX;
    private float endPositionZ;
    private bool canGenerate => spawnPosition.z < endPositionZ;

    void Awake()
    {
        //validate river segments
        foreach (var segment in riverSegments)
        {
            if (segment.GetComponent<RiverSegment>().shift.z < 0)
                throw new BadRiverSegmentException(segment.name);
        }

        //get references to children
        segmentParent = transform.Find("segments");
        var config = transform.Find("config points").transform;

        //find objects indicating positional parameters
        var start = config.Find("start"); //indicates position of first segment
        var end = config.Find("end"); //indicates Z coordinate where generation should stop
        var leftBorder = config.Find("left screen border"); //indicates X coordinate of left screen border
        var rightBorder = config.Find("right screen border"); //indicates X coordinate of right screen border

        endPositionZ = end.position.z;
        leftBorderX = leftBorder.position.x;
        rightBorderX = rightBorder.position.x;
        spawnPosition = start.position;
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
    private void PlaceSegment(GameObject segmentPrefab)
    {
        var segmentInstance = Instantiate(segmentPrefab, segmentParent);
        var segmentComponent = segmentInstance.GetComponent<RiverSegment>();

        segmentComponent.PlaceStartOfSegmentAt(spawnPosition);
        spawnPosition += segmentComponent.shift; //set spawnPosition at the end of last instantiated segment
    }
}
