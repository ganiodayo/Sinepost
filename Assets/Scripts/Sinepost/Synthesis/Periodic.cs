using UnityEngine;

namespace Sinepost {

	public abstract class Periodic : Signal {

        protected Parameter<Periodic> frequency;

        protected virtual void Setup(float amplitude, float frequency){

            this.amplitude = new Parameter<Signal>("Amplitude", amplitude);
            this.frequency = new Parameter<Periodic>("Frequency", frequency);
            parameters.Add(this.amplitude, this.frequency);

        }

        protected virtual void Setup(Instrument context, float amplitude, float frequency){

            this.amplitude = new Parameter<Signal>(context, "Amplitude", amplitude);
            this.frequency = new Parameter<Periodic>(context, "Frequency", frequency);
            parameters.Add(this.amplitude, this.frequency);

        }
		
	}

}