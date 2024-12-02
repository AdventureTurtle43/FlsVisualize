using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform flsTransform;
    [SerializeField] private MeshRenderer flsRenderer;
    private Vector3 startPosition;
    private Vector3 target;

    private Color32 flsColor;

    private int fps = 24;

    public Queue<Vector3> flightPath;
    public Queue<Color32> colorSequence;
    public Queue<int> durations;

    private float curDuration;
    
    private float currentTime = 0;

    private bool mover = true;
    void Start(){
        startPosition = flightPath.Dequeue();
        flsTransform.position = startPosition;

        flsColor = colorSequence.Dequeue();
        flsRenderer.material.color = flsColor;

        curDuration = durations.Dequeue();
        curDuration = curDuration/fps;

        if(flightPath.Count != 0){
            target = flightPath.Peek();
        }
        else {
            mover = false;
        }
    }

    // Update is called once per frame
    void Update(){
        if(mover){
            
            currentTime += Time.deltaTime/curDuration;
            if(flsTransform.position != target){
                flsTransform.position = Vector3.Lerp(startPosition, target, currentTime);
            } else if(flightPath.Count != 0){
                currentTime = 0;
                startPosition = target;
                target = flightPath.Dequeue();
                curDuration = durations.Dequeue();
                curDuration = curDuration/fps;
                flsColor = colorSequence.Dequeue();
                flsRenderer.material.color = flsColor;
            }
            
            
        }
    }

    void Move(){

    }
}
