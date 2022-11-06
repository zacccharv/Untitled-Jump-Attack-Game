using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBoxPosition : MonoBehaviour
{
    private GameObject character;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = character.transform.position;
    }
}
