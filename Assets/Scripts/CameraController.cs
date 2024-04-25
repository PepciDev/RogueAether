using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 camOGPos;

    float playerDistance;

    Vector3 betweenPlayerPos;

    public GameObject playerOne;
    public GameObject playerTwo;

    float maxYPos = 4;
    float maxXPos = 3;

    float maxDistance = -14;
    float minDistance = -8;

    void Start()
    {
        camOGPos = transform.position;
    }

    void Update()
    {
        betweenPlayerPos = Vector3.Lerp(playerOne.transform.position, playerTwo.transform.position, 0.5f);

        playerDistance = Vector3.Distance(playerOne.transform.position, playerTwo.transform.position);


        transform.position = new Vector3(Mathf.Clamp(betweenPlayerPos.x, camOGPos.x - maxXPos, camOGPos.x + maxXPos)
            , Mathf.Clamp(betweenPlayerPos.y, camOGPos.y - maxYPos/2.6f, camOGPos.y + maxYPos)
            , Mathf.Clamp(camOGPos.z * playerDistance/12, maxDistance, minDistance));
    }
}
