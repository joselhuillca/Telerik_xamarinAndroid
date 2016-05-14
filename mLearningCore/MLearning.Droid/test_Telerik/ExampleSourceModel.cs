using System;
using System.Collections.Generic;

namespace MLearning.Droid
{
	public class ExampleSourceModel {
		private List<String> dependencies;
		private int currentIndex;

		public ExampleSourceModel(List<String> dependencyFileNames) {
			this.dependencies = dependencyFileNames;

			if (this.dependencies == null) {
				this.dependencies = new List<String>();
			}
		}

		public List<String> getDependencies() {
			return this.dependencies;
		}

		public int getCurrentIndex() {
			return this.currentIndex;
		}

		public void setCurrentIndex(int value) {
			this.currentIndex = value;
		}
	}


}

