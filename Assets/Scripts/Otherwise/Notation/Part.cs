namespace Otherwise {

	public class Part : Notated {
		
		rhythmic beat;
		cyclic note;
		public int index;
		
		public Part(rhythmic beat, cyclic note){
			
			this.temperament = EqualTemperament(12u, 440f);
			
		}
		
		public Part(rhythmic beat, cyclic note, Tuning temperament){
			
			this.temperament = temperament;
			
		}
		
		public static Part operator + (Part input1, Part input2){
			
			Part output = input1;
			
			return output;
			
		}
		
		public override void Read(){
			
			
			
		}
		
	}

}
