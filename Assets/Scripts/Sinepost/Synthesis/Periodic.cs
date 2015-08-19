using UnityEngine;

namespace Sinepost {

	public abstract class Periodic : Signal {

        protected Parameter<Periodic> frequency;

        protected virtual void Setup(float amplitude, float frequency){

            base.Setup(amplitude);
            this.frequency = new Parameter<Periodic>("Frequency", frequency);
            parameters += this.frequency;

        }

        protected virtual void Setup(Instrument context, float amplitude, float frequency){

            base.Setup(context, amplitude);
            this.frequency = new Parameter<Periodic>(context, "Frequency", frequency);
            parameters += this.frequency;

        }
		
	}

}