namespace Otherwise {

	public abstract class Periodic : Signal {
		
		protected float frequency, frequencyModifier = 1f;
		
		public abstract float Frequency{ get; set; }
		
	}

}