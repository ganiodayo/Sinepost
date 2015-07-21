using UnityEngine;
using System.Collections;

namespace Otherwise {

	public sealed class Orchestra : MonoBehaviour, IAudible {
		
		private readonly Instrumentalist[] members;
		
		public Orchestra(params Instrumentalist[] members){
			
			this.members = members;
			
		}
		
		void Awake(){
			
			
			
		}
		
		public void OnEnable(){}
		
		public void OnTransformChildrenChanged(){}
		
		public Instrumentalist this[int index]{
			
			get{
				
				return members[index];
				
			}
			
		}
		
	}

}
