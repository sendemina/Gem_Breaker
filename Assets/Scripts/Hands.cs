using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] float screenWidthInUnits = 16f;
    [SerializeField] float minX = 2f;
    [SerializeField] float maxX = 14f;

    // Cached references
    GameStatus gameStatus;
    Sphere sphere;
    
    // Start is called before the first frame update
    void Start()
    {
        gameStatus = FindObjectOfType<GameStatus>();
        sphere = FindObjectOfType<Sphere>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 handsPos = new Vector2(transform.position.x, transform.position.y);
        handsPos.x = Mathf.Clamp(GetXPos(), minX, maxX);
        transform.position = handsPos;
    }

    private float GetXPos()
    {
        if (gameStatus.IsAutoPlayEnabled())
        {
            return sphere.transform.position.x;
        }
        else
        {
            return Input.mousePosition.x / Screen.width * screenWidthInUnits;
        }
    }
}
