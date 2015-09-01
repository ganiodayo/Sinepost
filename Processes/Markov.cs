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

using UnityEngine;
using System.Collections.Generic;

namespace Sinepost.Processes
{

	public class Markov : Process {
		
		//comparable list search with lambdas
		
		class State {
			
			public float[] past;
			public List<float> future = new List<float>();
			
			public State(float[] past){
				
				this.past = past;
				
			}
			
			public float Select(){
				
				return future[UnityEngine.Random.Range(0, future.Count)];
				
			}
			
		}
		
		public bool init;
		
		private int order;
		private float[] status;
		private List<State> states = new List<State>();
		
		public Markov(int order){
			
			this.order = order < 1 ? 1 : order;
			
		}
		
		public void Stream(List<float> data){
			
			float[] temp = new float[order];
			
			if(data.Count > order){
				
				init = true;
				
				for(int i = 0; i < data.Count; i++){
					
					for(int j = 0; j < this.order; j++)
						temp[j] = data[(i + j) % data.Count];
					
					if(states.Count > 0){
						
						bool test = false;
						
						for(int k = 0; k < states.Count; k++){
							
							int match = 0;
							
							for(int l = 0; l < order; l++){
								
								if(Mathf.Approximately(temp[l], states[k].past[l]))
									match++;
								else
									break;
								
							}
							
							if(match == order){
								
								states[k].future.Add(data[(i + this.order) % data.Count]);
								break;
								
							}
							
						}
						
						if(!test)
							states.Add(new State(temp));
						
					} else
						states.Add(new State(temp));
					
				}
				
			}
			
		}
		
		public void Stream(params float[] data){
			
			float[] temp = new float[order];
			
			if(data.Length > order){
				
				init = true;
				
				for(int i = 0; i < data.Length; i++){
					
					for(int j = 0; j < this.order; j++)
						temp[j] = data[(i + j) % data.Length];
					
					if(states.Count > 0){
						
						bool test = false;
						
						for(int k = 0; k < states.Count; k++){
							
							int match = 0;
							
							for(int l = 0; l < order; l++){
								
								if(Mathf.Approximately(temp[l], states[k].past[l]))
									match++;
								else
									break;
								
							}
							
							if(match == order){
								
								states[k].future.Add(data[(i + this.order) % data.Length]);
								break;
								
							}
							
						}
						
						if(!test)
							states.Add(new State(temp));
						
					} else
						states.Add(new State(temp));
					
				}
				
			}
			
		}
		
		public float Next(){
			
			if(init){
				
				if(states.Count > 0 && status == null)
					status = states[UnityEngine.Random.Range(0, states.Count)].past;
				
				float current = status[status.Length - 1];
				State state;
				
				for(int i = 0; i < states.Count; i++){
					
					int match = 0;
					
					for(int j = 0; j < order; j++){
						
						if(Mathf.Approximately(status[j], states[i].past[j]))
							match++;
						else
							break;
						
					}
					
					if(match == order){
						
						state = states[i];
						current = state.Select();
						
						for(i = 0; i < status.Length; i++)
							status[i] = i == status.Length - 1 ? current : status[i + 1];
						
						break;
						
					}
					
				}
				
				return current;
				
			} else
				return 0f;
			
		}
		
	}

}
