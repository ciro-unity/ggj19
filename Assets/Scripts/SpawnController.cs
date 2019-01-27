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
        Transform newPiece = Instantiate<Transform>(tetrisBlock, transform.position, Quaternion.identity);
        newPiece.SetParent(this.transform);
    }
}
