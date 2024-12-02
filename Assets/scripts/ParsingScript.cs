using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public struct FlsInstance {
    Vector3 pos;
    Color32 col;
    int duration;

}
public class ParsingScript : MonoBehaviour
{
    [SerializeField] private GameObject FlsPrefab;

    // Start is called before the first frame update
    void Start()
    {
        readTextFile("Assets/FLS.txt");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void readTextFile(string file_path){
        StreamReader inp_stm = new StreamReader(file_path);

        int flsCount = 0;


        while(!inp_stm.EndOfStream){
            string line = inp_stm.ReadLine();
            Queue<Vector3> flightPath = new Queue<Vector3>();
            Queue<Color32> colorSequence = new Queue<Color32>();
            Queue<int> durations = new Queue<int>();
            // Do Something with the input. 
            while(line != "coordinate: " && !inp_stm.EndOfStream) {
                line = inp_stm.ReadLine();
            }
            line = inp_stm.ReadLine();
            //handle the x y z

            while(line == "  - " && !inp_stm.EndOfStream){
                line = inp_stm.ReadLine();
                string[] fields = line.Split(' ');
                float x = float.Parse(fields[5]);

                line = inp_stm.ReadLine();
                fields = line.Split(' ');
                float z = float.Parse(fields[5]);

                line = inp_stm.ReadLine();
                fields = line.Split(' ');
                float y = float.Parse(fields[5]);

                line = inp_stm.ReadLine();

                Vector3 step = new Vector3(x, y, z);
                flightPath.Enqueue(step);
            }


            
            line = inp_stm.ReadLine();
            //handle color
            while(line == "  - " && !inp_stm.EndOfStream) {
                line = inp_stm.ReadLine();
                string[] fields = line.Split(' ');
                byte r = byte.Parse(fields[5]);

                line = inp_stm.ReadLine();
                fields = line.Split(' ');
                byte g = byte.Parse(fields[5]);

                line = inp_stm.ReadLine();
                fields = line.Split(' ');
                byte b = byte.Parse(fields[5]);

                line = inp_stm.ReadLine();
                fields = line.Split(' ');
                byte a = byte.Parse(fields[5]);

                line = inp_stm.ReadLine();

                Color32 colInstance = new Color32(r, g, b, a);
                colorSequence.Enqueue(colInstance);
            }

            line = inp_stm.ReadLine();
            //handle duration
            while(line == "  - " && !inp_stm.EndOfStream) {
                line = inp_stm.ReadLine();
                string[] fields = line.Split(' ');
                int start = int.Parse(fields[5]);

                line = inp_stm.ReadLine();
                fields = line.Split(' ');
                int end = int.Parse(fields[5]);

                line = inp_stm.ReadLine();

                durations.Enqueue(end-start);
            }

            flsCount++;

            GameObject FlsInstance = Instantiate(FlsPrefab);

            MovementScript move = FlsInstance.GetComponent<MovementScript>();

            
            move.flightPath = new Queue<Vector3>();
            move.colorSequence = new Queue<Color32>();
            move.durations = new Queue<int>(durations);

            foreach ( Vector3 obj in flightPath ){
                move.flightPath.Enqueue(obj);
            }

            foreach ( Color32 obj in colorSequence ){
                Color32 temp = new Color32(obj.r, obj.g, obj.b, obj.a);
                move.colorSequence.Enqueue(obj);
            }

            foreach ( int obj in durations ){
                move.durations.Enqueue(obj);
            }



        }


        inp_stm.Close( );  
    }
}
