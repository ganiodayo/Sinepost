using System;
using System.Collections.Generic;

namespace Sinepost {

    public abstract partial class Modulatable {

        protected abstract class Parameter : Modulatable {

            public string name;
            protected float actual;
            protected Instrument context;

            public static implicit operator float(Parameter value){

                return value.Value;

            }

            public abstract float Value{ get; set; }

        }

        protected class Parameter<T> : Parameter where T : Modulatable {

            protected readonly float modifier;

            public Parameter(string name, float f = 1f){

                this.name = name;
                actual = f;
                modifier = f;

            }

            public Parameter(Instrument context, string name, float f = 1f){

                this.context = context;
                this.name = name;
                actual = f;
                modifier = f;

            }

            public override float Value{

                get{

                    return actual * this.Modulation;

                } set{

                    actual = value * modifier;

                    if(context != null){

                        foreach(T signal in context)
                            signal[name] = actual;
                            

                    }

                }

            }

        }

    }

}
