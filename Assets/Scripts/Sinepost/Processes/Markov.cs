using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sinepost {

	public class Markov {
		
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
