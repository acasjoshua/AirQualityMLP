using System;

namespace Backprop
{
	public class HNeuron
	{
		private double[] weights;   // one weight per output neuron
		private int idno;
		private double hactivation;
		private double bias;
		private double error;

		public HNeuron(int idnodata, int size)
		{
			idno = idnodata;
			hactivation = 0.0;
			bias = 0.01;
			error = 0.0;
			weights = new double[size];
			for (int x = 0; x < size; x++)
				weights[x] = NeuralNet.RandomWeight();
		}

		/// <summary>Back propagate the output derivatives to this neuron.</summary>
		public void calculateErr(double[] der)
		{
			double result = 0.0;
			for (int x = 0; x < der.Length; x++)
				result += der[x] * weights[x];
			error = result * hactivation * (1 - hactivation);
		}

		/// <summary>Adjust the hidden -> output weights.</summary>
		public void setWeight(double lrpout, double[] der)
		{
			double temp = hactivation * lrpout;
			for (int x = 0; x < der.Length; x++)
				weights[x] += temp * der[x];
		}

		public void setWeight(int pos, double dat) { weights[pos] = dat; }
		public double getWeight(int outid) { return weights[outid]; }
		public void setErr(double x) { error = x; }
		public double getErr() { return error; }
		public void setHactivation(double data) { hactivation = data; }
		public double getHactivation() { return hactivation; }
		public void setIdno(int x) { idno = x; }
		public int getIdno() { return idno; }
		public void setBias(double b) { bias = b; }
		public double getBias() { return bias; }
		public void changeBias(double lrpin) { bias += error * lrpin; }
	}
}
