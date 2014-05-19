using System;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.Drawing;

namespace GameOfLife
{
    public abstract class RuleSet
    {
        protected int _width = 0;
        protected int _height = 0;

        /// <summary>
        /// Instantiates the RuleSet with a copy of the game field, and the maximum X,Y boundaries.
        /// </summary>
        /// <param name="field">int[][] game field</param>
        /// <param name="width">Maximum X boundary</param>
        /// <param name="height">Maximum Y boundary</param>
        public RuleSet(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public RuleSet()
        {
            _width = 500;
            _height = 500;
        }

        public Bitmap Tick(Color?[,] current, Color?[,] next, ObjectPool<Bitmap> pool, bool runParallel)
        {
            return TickAlgorithm(current, next, pool, runParallel);
        }

        protected abstract Bitmap TickAlgorithm(Color?[,] current, Color?[,] next, ObjectPool<Bitmap> pool, bool runParallel);

    }
}
