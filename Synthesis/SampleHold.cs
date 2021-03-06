﻿#region License
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

using System.Collections;

namespace Sinepost.Synthesis {

	public class SampleHold : Periodic, IEnumerator {

		float jitter, slew; //parameters?

		public SampleHold(){}
		
		public override float Render{
			
			get{
				
				return 0f;
				
			}
			
		}
		
		public override float Datum{
			
			get{
				
				return 0f;
				
			}
			
		}

		public object Current{

			get{

				return 0f;

			}

		}

		public bool MoveNext(){

			return true;

		}

		public void Reset(){}

	}

}
