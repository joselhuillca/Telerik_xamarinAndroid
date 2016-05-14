using System;

namespace MLearning.Droid
{
	public class ObjetoTelerik:Java.Lang.Object
	{
		private String title;
		private String Suubtitle;

		public ObjetoTelerik(String name, String country) {
			this.title = name;
			this.Suubtitle = country;
		}

		public String getName() {
			return title;
		}

		public void setName(String name) {
			this.title = name;
		}

		public String getCountry() {
			return Suubtitle;
		}

		public void setCountry(String country) {
			this.Suubtitle = country;
		}


		public override String toString() {
			return String.Format("%s (%s)", title, Suubtitle);
		}
	}
}

