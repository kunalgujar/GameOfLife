using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.Drawing;

namespace GameOfLife.RuleSets
{
    public class Rule1358d357 : RuleSet
    {
        public Rule1358d357(int width, int height) : base(width, height)
        {
        }

         protected override Bitmap TickAlgorithm(Color?[,] current, Color?[,] next, ObjectPool<Bitmap> _pool, bool runParallel)
        {
            var bmp = _pool.GetObject();
            using (FastBitmap fastBmp = new FastBitmap(bmp))
            {
                // For every row
                Action<int> body = i =>
                {
                    // For every column
                    for (int j = 0; j < _height; j++)
                    {
                        int count = 0;
                        int r = 0, g = 0, b = 0;

                        // Count neighbors
                        for (int x = i - 1; x <= i + 1; x++)
                        {
                            for (int y = j - 1; y <= j + 1; y++)
                            {
                                if ((x == i && j == y) || x < 0 || x >= _width || y < 0 || y >= _height) continue;
                                Color? c = current[x, y];
                                if (c.HasValue)
                                {
                                    count++;
                                    r += c.Value.R;
                                    g += c.Value.G;
                                    b += c.Value.B;
                                }
                            }
                        }

                        if (count < 1 || count >= 4) next[i, j] = null;
                        else if (current[i, j].HasValue && (count == 1 || count == 3|| count == 5|| count == 8)) next[i, j] = current[i, j];
                        else if (!current[i, j].HasValue && (count == 3|| count == 5|| count == 7)) next[i, j] = Color.FromArgb(r / count, g / count, b / count);
                        else next[i, j] = null;

                        // Render the cell
                        fastBmp.SetColor(i, j, current[i, j] ?? Color.White);
                    }
                };

                 //Process the rows serially or in parallel based on the RunParallel property setting
                if (runParallel)
                {
                    Parallel.For(0, _width,
                        body);
                }
                else
                {
                    for (int i = 0; i < _width; i++)
                        body(i);
                }
            }
            return bmp;
        }


    }
}
