namespace Otherwise {
	
	public class Section : Notated {
		
		private readonly Part[] parts;
		
		public Section(params Part[] parts){
			
			this.parts = parts;
			
		}
		
		public override void Read(){
			
		}
		
		public Part this[int index]{
			
			get{
				
				return parts[index];
				
			}
			
		}
		
	}
	
}