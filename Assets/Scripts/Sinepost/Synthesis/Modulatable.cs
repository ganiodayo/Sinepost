using System;
using System.Collections.Generic;

namespace Sinepost {

    public abstract partial class Modulatable {

        protected float offset = 1f;
        private List<Signal> modulators = new List<Signal>();
        protected ParameterList parameters = new ParameterList();
        //abstract enum parameters?

        public static Modulatable operator + (Modulatable modulatable, float offset){

            modulatable.offset = offset; //?

            return modulatable;

        }

        public float this[string name]{

            get{

                return parameters[name]; //try catch exposed parameter throw error

            } set{

                parameters[name].Value = value;

            }

        }

        protected float Modulation{

            get{

                float output = offset;

                foreach(Signal signal in modulators)
                    output += signal.Datum;

                return output;

            }

        }

        public Signal Modulator{

            set{

                modulators.Add(value);

            }

        }

        public void Modulate(string parameter = "Ring", params Signal[] modulators){

            if(parameter != "Ring"){

                for(int i = 0; i < modulators.Length; i++)
                    parameters[parameter].Modulator = modulators[i];

            } else{

                for(int i = 0; i < modulators.Length; i++)
                    Modulator = modulators[i];

            }

        }


    }

}
