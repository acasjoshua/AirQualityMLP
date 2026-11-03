# Data description

**File:** `data/air-quality-data.csv` (supplied with the assignment; open-data style air-quality monitoring export)

| Item | Value |
|---|---|
| Rows | 9,528 hourly records (no missing values, no duplicate timestamps, no gaps: every hour is present) |
| Period | 31 May 2019 00:00 to 30 Jun 2020 23:00 (about 13 months, so it covers every season and the spring-2020 lockdown period) |
| Station / city | Not stated in the file |
| Columns | `Datetime` plus 12 pollutant concentrations |

**Columns:** `PM2.5` (target), `PM10`, `NO2`, `NH3`, `SO2`, `CO`, `O3`, `NOx`, `NO`, `Benzene`, `Toluene`, `Xylene`.
Units are the usual monitoring units (ug/m3; CO in mg/m3). The file does not state them, so this is an assumption.

## Summary statistics (all 9,528 rows)

| Column | min | mean | median | max | skew |
|---|---|---|---|---|---|
| PM2.5 (target) | 7.43 | 92.48 | 61.39 | 847.97 | 2.56 |
| PM10 | 8.38 | 179.86 | 143.89 | 936.94 | 1.64 |
| NO2 | 10.22 | 37.59 | 33.25 | 135.04 | 1.15 |
| NH3 | 18.84 | 34.94 | 33.16 | 84.57 | 1.23 |
| SO2 | 5.23 | 12.32 | 11.23 | 45.56 | 1.98 |
| CO | 0.41 | 1.20 | 0.99 | 5.95 | 2.50 |
| O3 | 9.31 | 38.34 | 29.63 | 141.06 | 1.34 |
| NOx | 8.72 | 43.74 | 30.46 | 238.39 | 2.25 |
| NO | 2.97 | 25.79 | 12.93 | 226.06 | 2.76 |
| Benzene | 0.49 | 3.07 | 2.41 | 16.08 | 1.54 |
| Toluene | 3.87 | 23.63 | 18.82 | 122.74 | 1.79 |
| Xylene | 0.00 | 1.05 | 0.34 | 92.85 | 13.44 |

Pearson correlation with PM2.5: PM10 0.91, NH3 0.76, Benzene 0.79, CO 0.71, NO2 0.64, NOx 0.65, Toluene 0.61, NO 0.59,
SO2 0.26, Xylene 0.18, O3 -0.13.

## Why this data
* Real sensor data, hourly, clean enough to parse without guessing, but with the usual real-world features:
  strong right skew, extreme pollution episodes (PM2.5 up to 848), and 1,198 zero readings of Xylene.
* A natural regression question: *estimate PM2.5 from the other pollutants measured at the same hour*
  (a "virtual sensor" that can back up or sanity-check a PM2.5 instrument).

## Cleaning rules implemented in `CsvLoader`
Rows are dropped (and counted in the console output) if the column count is wrong, the date cannot be parsed, any value is
empty / `NA` / `NaN` / not numeric, or any concentration is negative. For this file nothing is dropped.
