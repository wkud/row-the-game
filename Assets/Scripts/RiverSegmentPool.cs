using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class RiverSegmentPool
{
	private List<RiverSegment> pool = new List<RiverSegment>();

	public void Push(RiverSegment segment) //is called when segment exits screen
	{
		segment.gameObject.SetActive(false);
		pool.Add(segment);
	}
	public RiverSegment TryPop(RiverSegmentType segmentType) //is called when segment needs to be reused
	{
		var pooledSegment = pool.FirstOrDefault(s => s.type == segmentType);
		if (pooledSegment != null) //if found
		{
			pooledSegment.gameObject.SetActive(true);
			pool.Remove(pooledSegment);
		}
		return pooledSegment;
	}
}
