using System;
using System.Globalization;
using System.IO;

namespace Backprop
{
	/// <summary>
	/// A fully connected 3 layer (input - hidden - output) sigmoid network trained by
	/// on-line back propagation.
	/// </summary>
	public class NeuralNet
	{
		private INeuron[] ineuron;
		private HNeuron[] hneuron;
		private ONeuron[] oneuron;

		private double lrpout = 0.2;   // learning rate for the hidden -> output weights and output bias
		private double lrpin = 0.15;   // learning rate for the input -> hidden weights and hidden bias

		private double[] errorComponent;
		private double[] errorDerivative;
		private double[] desiredout;

		// One shared generator for every neuron. Separate time-seeded Random objects created
		// within the same millisecond produce identical sequences.
		private static Random rand = new Random();
		private static readonly object randLock = new object();

		/// <summary>Re-seeds the generator used for the initial weights, so a run can be repeated exactly.</summary>
		public static void SetSeed(int seed)
		{
			lock (randLock)
			{
				rand = new Random(seed);
			}
		}

		internal static double RandomWeight()
		{
			lock (randLock)
			{
				return rand.Next(-1000, 1000) / 2000.0;   // -0.5 .. +0.5
			}
		}

		public NeuralNet() : this(3072, 64, 10)
		{
		}

		public NeuralNet(int input, int hidden, int output)
		{
			if (input < 1 || hidden < 1 || output < 1)
				throw new ArgumentException("Every layer needs at least one neuron.");

			ineuron = new INeuron[input];
			hneuron = new HNeuron[hidden];
			oneuron = new ONeuron[output];
			errorComponent = new double[output];
			errorDerivative = new double[output];
			desiredout = new double[output];
			createNeurons(input, hidden, output);

			// With many inputs the initial weighted sum is huge and the sigmoids saturate,
			// so use smaller steps for wide input layers.
			if (input > 16)
				lrpin = 4.0 / input;
		}

		public int InputCount { get { return ineuron.Length; } }
		public int HiddenCount { get { return hneuron.Length; } }
		public int OutputCount { get { return oneuron.Length; } }

		public double LearningRateOut { get { return lrpout; } set { lrpout = value; } }
		public double LearningRateIn { get { return lrpin; } set { lrpin = value; } }

		public void createNeurons(int i, int h, int o)
		{
			for (int x = 0; x < i; x++)
				ineuron[x] = new INeuron(x, h);
			for (int x = 0; x < h; x++)
				hneuron[x] = new HNeuron(x, o);
			for (int x = 0; x < o; x++)
				oneuron[x] = new ONeuron(x);

			// keep the starting weighted sum of wide input layers in a sensible range
			if (i > 16)
			{
				double scale = Math.Sqrt(16.0 / i);
				for (int x = 0; x < i; x++)
					for (int y = 0; y < h; y++)
						ineuron[x].setWeight(y, ineuron[x].getWeight(y) * scale);
			}
		}

		// ---- parameter access (used by GradientCheck and for inspecting a trained network) ----
		public double GetInputWeight(int input, int hidden) { return ineuron[input].getWeight(hidden); }
		public void SetInputWeight(int input, int hidden, double v) { ineuron[input].setWeight(hidden, v); }
		public double GetHiddenWeight(int hidden, int output) { return hneuron[hidden].getWeight(output); }
		public void SetHiddenWeight(int hidden, int output, double v) { hneuron[hidden].setWeight(output, v); }
		public double GetHiddenBias(int hidden) { return hneuron[hidden].getBias(); }
		public void SetHiddenBias(int hidden, double v) { hneuron[hidden].setBias(v); }
		public double GetOutputBias(int output) { return oneuron[output].getBias(); }
		public void SetOutputBias(int output, double v) { oneuron[output].setBias(v); }

		/// <summary>E = 1/2 * sum (desired - actual)^2 for the current inputs / desired outputs.</summary>
		public double Loss()
		{
			this.run();
			double e = 0.0;
			for (int x = 0; x < oneuron.Length; x++)
			{
				double d = desiredout[x] - oneuron[x].getOActivation();
				e += d * d;
			}
			return 0.5 * e;
		}

		public double getOuputData(int pos)
		{
			return oneuron[pos].getOActivation();
		}

		public void setInputs(int pos, double data)
		{
			ineuron[pos].setInput(data);
		}

		public void setDesiredOutput(int pos, double data)
		{
			desiredout[pos] = data;
		}

		public double sigmoid(double dat)
		{
			if (dat > 40.0) dat = 40.0;
			if (dat < -40.0) dat = -40.0;
			return 1.0 / (1.0 + Math.Exp(-dat));
		}

		public void calculateHA()
		{
			for (int x = 0; x < hneuron.Length; x++)
			{
				double summation = 0.0;
				for (int y = 0; y < ineuron.Length; y++)
					summation += ineuron[y].getInput() * ineuron[y].getWeight(x);
				hneuron[x].setHactivation(sigmoid(summation + hneuron[x].getBias()));
			}
		}

		public void calculateOA()
		{
			for (int x = 0; x < oneuron.Length; x++)
			{
				double summation = 0.0;
				for (int y = 0; y < hneuron.Length; y++)
					summation += hneuron[y].getHactivation() * hneuron[y].getWeight(x);
				oneuron[x].setOActivation(sigmoid(summation + oneuron[x].getBias()));
			}
		}

		public void calculateEC()
		{
			for (int x = 0; x < oneuron.Length; x++)
				errorComponent[x] = desiredout[x] - oneuron[x].getOActivation();
		}

		public void calculateDER()
		{
			for (int x = 0; x < oneuron.Length; x++)
			{
				double o = oneuron[x].getOActivation();
				errorDerivative[x] = o * (1 - o) * errorComponent[x];
			}
		}

		/// <summary>One training step on the current inputs / desired outputs.
		/// Returns the sum of squared output errors before the weights were changed.</summary>
		public double learn()
		{
			this.run();
			this.calculateEC();
			this.calculateDER();

			// hidden errors must be computed BEFORE the hidden->output weights change
			for (int x = 0; x < hneuron.Length; x++)
				hneuron[x].calculateErr(errorDerivative);

			for (int x = 0; x < hneuron.Length; x++)          // hidden -> output weights
				hneuron[x].setWeight(lrpout, errorDerivative);

			for (int x = 0; x < ineuron.Length; x++)          // input -> hidden weights
				for (int y = 0; y < hneuron.Length; y++)
					ineuron[x].setWeight(y, hneuron[y].getErr(), lrpin);

			for (int x = 0; x < oneuron.Length; x++)          // output biases
				oneuron[x].changeBias(lrpout, errorDerivative);
			for (int x = 0; x < hneuron.Length; x++)          // hidden biases
				hneuron[x].changeBias(lrpin);

			double sse = 0.0;
			for (int x = 0; x < errorComponent.Length; x++)
				sse += errorComponent[x] * errorComponent[x];
			return sse;
		}

		public void run()
		{
			this.calculateHA();
			this.calculateOA();
		}

		// ---------------------------------------------------------------- persistence
		// File layout: "inputs hidden outputs" header, then one value per line:
		// input->hidden weights, hidden->output weights, hidden biases, output biases.

		public static void PeekSize(string path, out int input, out int hidden, out int output)
		{
			using (StreamReader sr = new StreamReader(path))
				ReadHeader(sr, out input, out hidden, out output);
		}

		public static void ReadHeader(TextReader sr, out int input, out int hidden, out int output)
		{
			string[] p = (sr.ReadLine() ?? "").Split(' ');
			if (p.Length != 3
				|| !int.TryParse(p[0], out input)
				|| !int.TryParse(p[1], out hidden)
				|| !int.TryParse(p[2], out output))
				throw new InvalidDataException("Not a valid weights file.");
		}

		public void saveWeights(String path)
		{
			using (StreamWriter sw = new StreamWriter(path))
				saveWeights(sw);
		}

		public void saveWeights(TextWriter sw)
		{
			{
				sw.WriteLine("{0} {1} {2}", ineuron.Length, hneuron.Length, oneuron.Length);
				for (int x = 0; x < ineuron.Length; x++)
					for (int y = 0; y < hneuron.Length; y++)
						sw.WriteLine(ineuron[x].getWeight(y).ToString("R", CultureInfo.InvariantCulture));
				for (int x = 0; x < hneuron.Length; x++)
					for (int y = 0; y < oneuron.Length; y++)
						sw.WriteLine(hneuron[x].getWeight(y).ToString("R", CultureInfo.InvariantCulture));
				for (int x = 0; x < hneuron.Length; x++)
					sw.WriteLine(hneuron[x].getBias().ToString("R", CultureInfo.InvariantCulture));
				for (int x = 0; x < oneuron.Length; x++)
					sw.WriteLine(oneuron[x].getBias().ToString("R", CultureInfo.InvariantCulture));
			}
		}

		public void loadWeights(String path)
		{
			using (StreamReader sr = new StreamReader(path))
				loadWeights(sr);
		}

		public void loadWeights(TextReader sr)
		{
			int i, h, o;
			ReadHeader(sr, out i, out h, out o);
			if (i != ineuron.Length || h != hneuron.Length || o != oneuron.Length)
				throw new InvalidDataException(string.Format(
					"The file holds a {0}-{1}-{2} network but the current network is {3}-{4}-{5}.",
					i, h, o, ineuron.Length, hneuron.Length, oneuron.Length));

			{
				for (int x = 0; x < ineuron.Length; x++)
					for (int y = 0; y < hneuron.Length; y++)
						ineuron[x].setWeight(y, ReadValue(sr));
				for (int x = 0; x < hneuron.Length; x++)
					for (int y = 0; y < oneuron.Length; y++)
						hneuron[x].setWeight(y, ReadValue(sr));
				for (int x = 0; x < hneuron.Length; x++)
					hneuron[x].setBias(ReadValue(sr));
				for (int x = 0; x < oneuron.Length; x++)
					oneuron[x].setBias(ReadValue(sr));
			}
		}

		private static double ReadValue(TextReader sr)
		{
			string line = sr.ReadLine();
			if (line == null)
				throw new InvalidDataException("The weights file is incomplete.");
			return double.Parse(line, CultureInfo.InvariantCulture);
		}
	}
}
