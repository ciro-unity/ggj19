using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnManager : Singleton<SpawnManager>
{
	public GameObject[] prefabs;

    private Transform[] spawnPoints;
	private bool[] busyFlags;
	private PickupObject[] objectRefs;

	private float objectSpawnRate = 3f;
	private float lastObjectSpawnedTime = 0f;


	private void Awake()
	{
		int count = transform.childCount;
		
		//Create all the arrays
		spawnPoints = new Transform[count];
		busyFlags = new bool[count];
		objectRefs = new PickupObject[count];

		//Initialisation
		for(int i=0; i<count; i++)
		{
			spawnPoints[i] = transform.GetChild(i);
			busyFlags[i] = false;
			objectRefs[i] = null;
		}
	}


	private void Update()
	{
		if(Time.time >= lastObjectSpawnedTime + objectSpawnRate)
		{
			bool objectSpawned = false;
			int tries = 0;

			do
			{
				//safety mechanism
				tries++;
				if(tries > 1000)
					break;

				//Spawn object
				int objectSlot = Random.Range(0, spawnPoints.Length);
				if(!busyFlags[objectSlot])
				{
					//Free slot, spawn
					int whichPrefab = Random.Range(0, prefabs.Length);
					//Instantiate and move
					GameObject newPrefab = Instantiate<GameObject>(prefabs[whichPrefab]);
					newPrefab.transform.position = spawnPoints[objectSlot].position;
					float initialScale = newPrefab.transform.localScale.x;
					newPrefab.transform.localScale = Vector3.one * 0.01f;
					newPrefab.transform.DOScale(Vector3.one * initialScale, .5f).SetEase(Ease.OutElastic);
					
					PickupObject newPickup = newPrefab.GetComponent<PickupObject>();
					objectRefs[objectSlot] = newPickup;
					newPickup.OnPickedUp += FreeSpot; //hook up listener
					
					busyFlags[objectSlot] = true;

					objectSpawned = true; //exits while
				}
			}
			while(!objectSpawned);
			
			lastObjectSpawnedTime = Time.time;
		}
	}

	private void FreeSpot(PickupObject pickupGotten)
	{
		for(int i=0; i<objectRefs.Length; i++)
		{
			if(objectRefs[i] == pickupGotten)
			{
				objectRefs[i] = null;
				busyFlags[i] = false;
			}
		}
	}
}
