using System.Collections.Generic;

namespace Otherwise {

    public abstract class Modulatable {

        private List<Signal> modulators = new List<Signal>();

        protected float Modulate{

            get{

                float output = modulators.Count == 0 ? 1f : 0f;

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

    }

}
