using UnityEngine;
using System.Collections;
using Sinepost;

public class Sine : MonoBehaviour {

    Oscillator x, y;

    void Awake(){
        
        x = new Oscillator(Wavetable.Sawtooth + Wavetable.Sine, 0.4f, 0.045f);

    }

    void Update(){

        transform.position = new Vector3(x.Render, 0f, 0f);

    }

}
