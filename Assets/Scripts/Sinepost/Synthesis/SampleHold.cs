﻿using UnityEngine;
using System.Collections;

namespace Sinepost {

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
