using System;
using System.Collections.Generic;

namespace Sinepost {

    public abstract partial class Modulatable { //partial? ParameterList class

        protected class ParameterList : Dictionary<string, Parameter> { //get instrument?

            public void Add(params Parameter[] parameters) {

                for(int i = 0; i < parameters.Length; i++)
                    this.Add(parameters[i].name, parameters[i]);

            }

            public static ParameterList operator + (ParameterList list, Parameter parameter){

                list.Add(parameter.name, parameter);

                return list;

            }

        }

    }

}
