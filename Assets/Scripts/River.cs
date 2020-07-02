using Assets.Scripts.Exceptions;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class River : MonoBehaviour
{
    [SerializeField] private RiverSegment[] riverSegments;
    [SerializeField] private RiverSegment startSegment;
    private RiverSegmentPool pool = new RiverSegmentPool();
    private Transform segmentParent;
    private RiverSegment lastSegment;

    public float despawnPositionZ => despawnPoint.position.z;
    private float spawnEndPositionZ => spawnEndPoint.position.z;
    private Transform spawnEndPoint;
    private Transform despawnPoint;
    
    private Vector3 spawnPosition => lastSegment != null ? lastSegment.endPosition : firstSpawnPosition;
    private Vector3 firstSpawnPosition;
    private float leftBorderX;
    private float rightBorderX;
    private bool canGenerate => spawnPosition.z < spawnEndPositionZ;

    void Awake()
    {
        //validate river segments
        foreach (var segment in riverSegments)
        {
            if (segment.shift.z < 0)
                throw new BadRiverSegmentException(segment.name);
        }

        segmentParent = transform.Find("segments");
        
        //find objects indicating positional parameters
        GetParameters();
    }
    private void GetParameters()
    {
        //static config points
        var config = transform.Find("static config points").transform;

        var start = config.Find("first spawn position"); //indicates position of first segment
        var leftBorder = config.Find("left screen border"); //indicates X coordinate of left screen border
        var rightBorder = config.Find("right screen border"); //indicates X coordinate of right screen border

        leftBorderX = leftBorder.position.x;
        rightBorderX = rightBorder.position.x;
        firstSpawnPosition = start.position;

        //pool related config points (they move with camera speed)
        var pool = transform.Find("camera dependent config points");

        spawnEndPoint = pool.Find("spawn end"); //indicates Z coordinate where generation should stop
        despawnPoint = pool.Find("despawn"); //indicates Z coordinate where segments should despawn
    }

    void Start()
    {
        PlaceSegment(startSegment);
        GenerateRiver();
    }

    private void GenerateRiver()
    {
        while (canGenerate)
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
            segment.onDespawn += delegate
            {
                pool.Push(segment);
                GenerateRiver();
            };
        }

        //set segment up
        segment.PlaceStartOfSegmentAt(spawnPosition);

        lastSegment = segment;
    }
}
