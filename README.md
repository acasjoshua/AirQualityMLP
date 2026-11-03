# PM2.5 estimation with a hand-written back-propagation MLP (C#)

**Track:** Regression. The network (input / hidden / output classes + `NeuralNet` in `AirQualityMLP/Backprop/`) is the only
learning code used. Estimates PM2.5 (ug/m3) from the 11 other pollutants + hour of day.
A short data description is in [`DATA_DESCRIPTION.md`](DATA_DESCRIPTION.md).

## Run it
1. Open `AirQualityMLP.sln` (or `.slnx`) in Visual Studio. Target: .NET Framework 3.5, Windows Forms app, no NuGet packages.
2. Press F5. The window (`Form1`) opens and finds `data/air-quality-data.csv` by itself (or use **Browse...**).
3. Pick a mode and press **Run experiment**:
   * **Quick test** (default) - tiny grid, finishes in seconds; good for a first look.
   * **Full run** - the complete 70-configuration grid search and 5-seed final training, about 7 minutes.
   Results are written to `results/` (`results_quick/` for the quick test).

The same experiment can be run from a command prompt (no window), which is how the files in `results/` were produced:

| Command line | Effect |
|---|---|
| `AirQualityMLP.exe` | opens the window |
| `AirQualityMLP.exe --run` | full experiment, console output + result files |
| `AirQualityMLP.exe --quick` | tiny grid / few epochs, writes `results_quick/` |
| `AirQualityMLP.exe --gradcheck` | only verifies the back-propagation maths |
| `--data f` / `--out dir` | custom input file / output folder |

## The window
| Tab | What it shows |
|---|---|
| **Data** | cards with row count, period, train/validation/test split and dropped rows; table of min / mean / median / max and correlation with PM2.5 for every column |
| **Tuning** | the grid search filling in live; rows shaded green by validation RMSE, the winner in bold |
| **Training** | live loss curve (training vs validation) and an epoch table for the primary network, plus the result of each extra seed |
| **Results** | test RMSE / MAE / R² cards, actual-vs-predicted scatter, comparison with the two baselines, confusion matrix of air-quality bands, error by band |
| **Try it** | edit the 11 pollutant readings and the hour, or load a random unseen test hour, and see the estimated PM2.5, its band on the colour scale and the error against the real measurement |
| **Log** | the full console log; the **Gradient check** button prints the back-propagation check here |

The experiment runs on a background thread, so the window stays responsive, and **Stop** cancels it cleanly.

## Project layout
```
data/air-quality-data.csv        the dataset
AirQualityMLP/Backprop/          NeuralNet, INeuron, HNeuron, ONeuron  (the given library)
AirQualityMLP/Data/              CsvLoader, DataSplit, Preprocessor (scaling + features)
AirQualityMLP/Training/          MlpRegressor (epoch loop), HyperparameterTuner, GradientCheck, TrainingConfig
AirQualityMLP/Evaluation/        RegressionMetrics, AqiCategory, LinearBaseline (reference only), PlotWriter
AirQualityMLP/Experiment/         ExperimentRunner (the whole pipeline, used by the window and the command line)
AirQualityMLP/Form1.cs            the window (tabs, live tables and charts, Try it predictor)
AirQualityMLP/UI/                ChartPanel, BandScale (small custom controls)
AirQualityMLP/Program.cs         entry point: window or command line
results/                         output of the full run (committed so it can be inspected)
```
Changes made to the given library (the maths of the forward pass and weight updates is unchanged): learning rates are
settable instead of constants, `SetSeed` makes initial weights repeatable, one shared random generator replaces one per
class (separate time-seeded generators produced identical sequences), `learn()` returns the error, and accessors
for individual weights were added for the gradient check.

## 1. Back-propagation correctness
`--gradcheck` (also runs at the start of every run) performs one `learn()` step on a 5-4-2 network with learning rate 1
and compares each weight change with a central-difference derivative of E = 1/2 sum (d - o)^2:
**34 of 34 parameters agree, max relative error 3.6e-9 (PASS).** Sigmoid inputs are clamped to +/-40 to avoid overflow, and
training aborts if the loss becomes NaN (reported as "DIVERGED"; none of the 70 grid runs diverged).

## 2. Data pipeline
* Parsed from CSV by column name, invariant culture, bad rows dropped and counted.
* Inputs (13): 11 pollutants + hour as sin/cos (23:00 and 00:00 are neighbours).
* Min-max scaling to [0,1] with **min/max taken from the training rows only** (no leakage); target scaled to [0.1, 0.9]
  because the output is a sigmoid. Optional log1p transform of inputs and target was tested as a hyperparameter.
* Random split, seed 42: **train 6,669 / validation 1,429 / test 1,430** (70/15/15). Validation is used for tuning and
  early stopping; the test set is used once, at the end.

## 3. Hyperparameter tuning (validation RMSE in ug/m3, `results/tuning_results.csv`)
Grid: learning rate {0.05, 0.1, 0.2, 0.5, 1, 2, 4} x hidden nodes {4, 8, 16, 32, 64} x {raw, log1p} = 70 runs, 400 epochs each,
no early stopping, same seed. Best validation RMSE per cell, raw inputs/target:

| lr \ hidden | 4 | 8 | 16 | 32 | 64 |
|---|---|---|---|---|---|
| 0.05 | 24.80 | 24.46 | 24.51 | 24.51 | 24.60 |
| 0.1 | 23.96 | 23.35 | 23.57 | 23.71 | 24.13 |
| 0.2 | 22.17 | 20.63 | 20.83 | 20.25 | 21.61 |
| 0.5 | 18.85 | 18.27 | 17.80 | 18.03 | 18.40 |
| 1 | 18.46 | 17.40 | 16.68 | 16.41 | 17.21 |
| 2 | 17.79 | 17.08 | 15.76 | 15.49 | 15.65 |
| 4 | 16.56 | 15.85 | 15.56 | 15.40 | **15.01** |

With log1p inputs/target the best cell was 16.41 (lr 4, 64 hidden), always worse than raw at equal settings (full table in the CSV, plots
`tuning_raw.png`, `tuning_log_transform.png`).

What the experiments show
* **Learning rate matters most.** Online back-propagation with sigmoid units is slow: at lr 0.05 the error is still about 24.5 after 400 epochs,
  and every doubling of the rate helps until about 2 to 4. Nothing diverged up to lr 4, so the grid was widened until the gains flattened
  (2 -> 4 changes the best value only from 15.49 to 15.01).
* **Hidden size:** 4 nodes under-fits (16.6 at best); from 16 to 64 the results are within about 0.5 of each other. That difference is
  smaller than the seed-to-seed spread of the final models (sd 0.6), so 32 or 64 nodes are equally defensible. The winner, **lr 4, 64 hidden nodes**, was chosen
  because it has the lowest validation error.
* **Epochs:** validation RMSE after 50 / 100 / 200 / 400 epochs for the winner is 21.7 / 17.6 / 17.5 / 15.7, so the curve is still
  improving at 400. The final training therefore allows up to 1,000 epochs and stops after 50 epochs without improvement,
  restoring the best epoch (best epochs were 288 to 748 across seeds).
* **Log transform** lowers the error on small values but optimises the log error, which does not minimise RMSE in real units, so raw scaling won.

## 4. Training log
`results/training_log.csv` (every epoch) and the console print the loss; excerpt for the primary model (seed 1):

| epoch | train MSE (scaled) | validation MSE (scaled) | validation RMSE (ug/m3) |
|---|---|---|---|
| 1 | 0.013232 | 0.008442 | 84.0 |
| 10 | 0.001412 | 0.000585 | 25.4 |
| 50 | 0.000716 | 0.000426 | 21.7 |
| 100 | 0.000593 | 0.000282 | 17.6 |
| 200 | 0.000494 | 0.000278 | 17.5 |
| 400 | 0.000435 | 0.000224 | 15.7 |

Training MSE is the average error measured *while* the weights are changing during that epoch, which is why it is higher than the validation
error (measured after the epoch). See `results/loss_curve.png`. The spikes in the validation curve come from the large learning rate; early stopping keeps the best epoch.

## 5. Evaluation on the untouched test set (ug/m3)
| Model | RMSE | MAE | R2 |
|---|---|---|---|
| Predict the training mean | 92.04 | 63.07 | -0.001 |
| Linear regression (same inputs, reference) | 31.79 | 20.09 | 0.881 |
| **MLP, primary model (seed 1)** | **15.19** | **10.39** | **0.973** |
| MLP, mean +/- sd over 5 seeds | 15.75 +/- 0.59 | 10.63 +/- 0.29 | 0.971 +/- 0.002 |

Analysis
* The network explains about 97% of the variance and halves the error of linear regression, so the non-linear hidden layer is doing real work
  (PM2.5 depends non-linearly on PM10, NH3 and the time of day).
* Error grows with pollution level (`error by band` in `results/run_log.txt`): MAE 5.0 in the Good band, 19.7 in the Severe band, and the model
  **under-predicts extreme episodes** (bias -9.6 in Severe, -4.9 in Very Poor) because very few such hours exist in training.
* Turning the predictions into the six Indian CPCB PM2.5 bands (an approximation, as those bands are defined for 24-hour averages) gives 73.4% exact accuracy and
  **98.8% within one band**. Mistakes are almost always to a neighbouring band; precision / recall per band are in the confusion matrix in `results/run_log.txt`
  (Severe: 92% precision, 88% recall).
* Test RMSE (15.2) is close to the validation RMSE (14.5), so there is no sign of over-fitting to the validation set.

Limitations: random split of an hourly series (neighbouring hours are correlated, so a chronological split would be harder); one station only; seeds
vary the result by about +/-0.6 RMSE; the primary seed was fixed in advance (seed 1), not chosen on test results.

## Files in `results/`
`run_log.txt` (full console output), `training_log.csv` (+ `_seed2..5`), `tuning_results.csv`, `metrics_summary.txt`,
`test_predictions.csv`, `loss_curve.png`, `actual_vs_predicted.png`, `tuning_raw.png`, `tuning_log_transform.png`.

## Publishing to GitHub
```
git init && git add . && git commit -m "PM2.5 MLP regression"
git remote add origin <your repo url> && git push -u origin main
```
