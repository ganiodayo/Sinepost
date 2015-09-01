#region License
//
// Sinepost
// Copyright (c) 2015 Jonathan A. Califa
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System.Linq;
using System.Collections.Generic;

namespace Sinepost.Notation {

    public class PitchClassSet {
            
        public readonly bool z = false;
        public readonly int[] primeForm, intervalVector;
        public readonly string code;

        public PitchClassSet(){

            intervalVector = new int[6]{0, 0, 0, 0, 0, 0};

        }
        
        public PitchClassSet(params int[] notes){

            Reduce(notes, out primeForm, out intervalVector);

        }

        public PitchClassSet(bool z, params int[] notes) {

            this.z = z;
            Reduce(notes, out primeForm, out intervalVector);

        }

        private static void Reduce(int[] notes, out int[] primeForm, out int[] intervalVector){

            for(int i = 0; i < notes.Length; i++)
                notes[i] %= 12;

            primeForm = new HashSet<int>(notes).ToArray();
            intervalVector = new int[6]{0, 0, 0, 0, 0, 0};

            int[,] matrix = new int[primeForm.Length, primeForm.Length];

            for(int i = 0; i < primeForm.Length; i++){

                for(int j = i + 1; j < primeForm.Length; j++)
                    intervalVector[(primeForm[j] - primeForm[i] - 1) % 6]++;

                for(int j = 0; j < primeForm.Length; j++)
                    matrix[i, j] = (primeForm[i + j % primeForm.Length] - primeForm[i] + 12) % 12;

            }

        }

        public override string ToString(){

            return base.ToString();

        }

    }

}
