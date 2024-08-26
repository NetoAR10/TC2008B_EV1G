using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This follow controller class will update the events from the player cam.
/// Standar coding documentarion can be found in 
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    // offset para mover la camara a un lugar deseado
    private Vector3 offset = new Vector3(0,200,0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // codigo para mover la camara a primera persona
        transform.position = player.transform.position + offset;
    }
}

