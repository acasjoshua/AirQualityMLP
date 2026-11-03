using System;

namespace Backprop
{
	public class INeuron
	{
		private int idno;
		private double input;
		private double[] weights;   // one weight per hidden neuron

		public INeuron(int idnodata, int size)
		{
			idno = idnodata;
			input = 0;
			weights = new double[size];
			for (int x = 0; x < size; x++)
				weights[x] = NeuralNet.RandomWeight();
		}

		public void setWeight(int hidno, double err, double lrpin)
		{
			weights[hidno] += err * lrpin * input;
		}

		public void setWeight(int pos, double dat) { weights[pos] = dat; }
		public double getWeight(int hidno) { return weights[hidno]; }
		public void setInput(double data) { input = data; }
		public double getInput() { return input; }
		public void setID(int no) { idno = no; }
		public int getID() { return idno; }
	}
}
