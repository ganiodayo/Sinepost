namespace Otherwise {

	public class Score : Notated {
		
		private readonly Section[] sections;
		
		public Score(params Section[] sections){
			
			this.sections = sections;
			
		}
		
		public Section this[int index]{
			
			get{
				
				return sections[index];
				
			}
			
		}
		
		public override void Read(){}
		
	}

}
