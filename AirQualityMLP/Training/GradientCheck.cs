using System;
using System.IO;
using Backprop;

namespace AirQualityMLP.Training
{
    /// <summary>
    /// Proves the back-propagation maths: for a small random network the weight change made by learn()
    /// (with learning rate 1, so delta-w = -dE/dw) is compared with a numerical derivative of the loss
    /// E = 1/2 * sum (desired - actual)^2, obtained by nudging every single weight and bias by +/- epsilon.
    /// </summary>
    public static class GradientCheck
    {
        public static bool Run(TextWriter log)
        {
            const int inputs = 5, hidden = 4, outputs = 2;
            const double eps = 1e-6;
            Random rnd = new Random(123);

            NeuralNet.SetSeed(7);
            NeuralNet net = new NeuralNet(inputs, hidden, outputs);
            net.LearningRateIn = 1.0;
            net.LearningRateOut = 1.0;

            // an identical copy that will actually perform the learn() step
            StringWriter snapshot = new StringWriter();
            net.saveWeights(snapshot);
            NeuralNet trained = new NeuralNet(inputs, hidden, outputs);
            trained.LearningRateIn = 1.0;
            trained.LearningRateOut = 1.0;
            trained.loadWeights(new StringReader(snapshot.ToString()));

            double[] x = new double[inputs];
            double[] d = new double[outputs];
            for (int i = 0; i < inputs; i++) { x[i] = rnd.NextDouble(); net.setInputs(i, x[i]); trained.setInputs(i, x[i]); }
            for (int o = 0; o < outputs; o++) { d[o] = 0.1 + 0.8 * rnd.NextDouble(); net.setDesiredOutput(o, d[o]); trained.setDesiredOutput(o, d[o]); }

            trained.learn();    // one back-propagation step on the copy

            double worst = 0, sum = 0; int count = 0;
            log.WriteLine("Gradient check ({0}-{1}-{2} network, central differences, eps={3})", inputs, hidden, outputs, eps);
            log.WriteLine("{0,-14}{1,16}{2,16}{3,12}", "parameter", "backprop", "numerical", "rel. error");

            for (int i = 0; i < inputs; i++)
                for (int h = 0; h < hidden; h++)
                {
                    int ii = i, hh = h;
                    Compare(log, "w_in[" + i + "," + h + "]", net, trained,
                        delegate(NeuralNet n) { return n.GetInputWeight(ii, hh); },
                        delegate(NeuralNet n, double v) { n.SetInputWeight(ii, hh, v); },
                        eps, ref worst, ref sum, ref count, i == 0 && h < 2);
                }
            for (int h = 0; h < hidden; h++)
                for (int o = 0; o < outputs; o++)
                {
                    int hh = h, oo = o;
                    Compare(log, "w_hid[" + h + "," + o + "]", net, trained,
                        delegate(NeuralNet n) { return n.GetHiddenWeight(hh, oo); },
                        delegate(NeuralNet n, double v) { n.SetHiddenWeight(hh, oo, v); },
                        eps, ref worst, ref sum, ref count, h == 0);
                }
            for (int h = 0; h < hidden; h++)
            {
                int hh = h;
                Compare(log, "bias_hid[" + h + "]", net, trained,
                    delegate(NeuralNet n) { return n.GetHiddenBias(hh); },
                    delegate(NeuralNet n, double v) { n.SetHiddenBias(hh, v); },
                    eps, ref worst, ref sum, ref count, h == 0);
            }
            for (int o = 0; o < outputs; o++)
            {
                int oo = o;
                Compare(log, "bias_out[" + o + "]", net, trained,
                    delegate(NeuralNet n) { return n.GetOutputBias(oo); },
                    delegate(NeuralNet n, double v) { n.SetOutputBias(oo, v); },
                    eps, ref worst, ref sum, ref count, true);
            }

            bool pass = worst < 1e-5;
            log.WriteLine("... ({0} parameters checked; only a few shown)", count);
            log.WriteLine("max relative error = {0:E2}, mean = {1:E2}  ->  {2}", worst, sum / count, pass ? "PASS" : "FAIL");
            return pass;
        }

        private delegate double Getter(NeuralNet n);
        private delegate void Setter(NeuralNet n, double v);

        private static void Compare(TextWriter log, string name, NeuralNet original, NeuralNet afterStep,
                                    Getter get, Setter set, double eps, ref double worst, ref double sum, ref int count, bool show)
        {
            double before = get(original);
            double analytic = -(get(afterStep) - before);     // delta-w = -lr * dE/dw  with lr = 1

            set(original, before + eps); double up = original.Loss();
            set(original, before - eps); double down = original.Loss();
            set(original, before);
            double numeric = (up - down) / (2 * eps);

            double rel = Math.Abs(analytic - numeric) / Math.Max(Math.Abs(analytic) + Math.Abs(numeric), 1e-12);
            if (rel > worst) worst = rel;
            sum += rel; count++;
            if (show) log.WriteLine("{0,-14}{1,16:E6}{2,16:E6}{3,12:E1}", name, analytic, numeric, rel);
        }
    }
}
