using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Transform tetrisBlock;
    // Start is called before the first frame update
    void Start()
    {
        SpawnBlock(tetrisBlock);
    }

    public void SpawnBlock(Transform tetrisBlock)
    {
        Instantiate(tetrisBlock, transform.position, Quaternion.identity);
    }
}
