# Benchmark Analysis Write-Up - Short Write-Up (Reality vs Theory)

## Comparison of $O(N)$ vs. $O(1)$ Speeds:

The measured times strongly match my Big-O predictions, especially as the number of items ($N$) increases.

List (My Prediction: $O(N)$): The time to find an item in the list grows bigger as $N$ grows. Going from 10,000 to 100,000 items makes the search about 10 times slower (from 0.002 ms to 0.022 ms). This confirms the $O(N)$ prediction.

HashSet/Dictionary (My Prediction: $O(1)$): The time remains zero across all sizes, from 1,000 to 250,000 items. Time doesn't change regardless of how large the collection is.

| Items ($N$) | List Search Time | HashSet/Dictionary Time |
| :---: | :---: | :---: |
| 1,000 | 0.000 ms | 0.000 ms |
| 10,000 | 0.002 ms | 0.000 ms |
| 100,000 | 0.022 ms | 0.000 ms |
| 250,000 | 0.027 ms | 0.000 ms |

There wasn't any surprise, my rig is pretty beefy so I expected computation time to be quite fast (i5-13600K - totally not flexing btw)

Given larger dataset, I'd definitely go with Hashset or Dict, since both provide instant return $O(1)$ scalling