
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using GameOfLife.RuleSets;
using Microsoft.Drawing;

namespace GameOfLife
{
    /// <summary>Represents the game of life board.</summary>
    internal class GameBoard
    {
        /// <summary>Arrays used to store the current and next state of the game.</summary>
        private Color?[][,] _scratch;
        /// <summary>Index into the scratch arrays that represents the current stage of the game.</summary>
        private int _currentIndex;
        /// <summary>A pool of Bitmaps used for rendering.</summary>
        private ObjectPool<Bitmap> _pool;
        /// <summary>string for ruleset</summary>
        private string _ruleSet;

        /// <summary>
        /// Initializes the game board.
        /// </summary>
        /// <param name="width">The width of the board.</param>
        /// <param name="height">The height of the board.</param>
        /// <param name="initialDensity">The initial population density to use to populate the board.</param>
        /// <param name="pool">The pool of Bitmaps to use.</param>
        /// <param name="ruleSet">The rule set.</param>
        public GameBoard(int width, int height, double initialDensity, ObjectPool<Bitmap> pool, string ruleSet)
        {
            // Validate parameters
            if (width < 1) throw new ArgumentOutOfRangeException("width");
            if (height < 1) throw new ArgumentOutOfRangeException("height");
            if (pool == null) throw new ArgumentNullException("pool");
            if (ruleSet == null) throw new ArgumentNullException("pool");
            if (initialDensity < 0 || initialDensity > 1) throw new ArgumentOutOfRangeException("initialDensity");

            // Store parameters
            _pool = pool;
            Width = width;
            Height = height;
            _ruleSet = ruleSet;

            // Create the storage arrays
            _scratch = new Color?[2][,] { new Color?[width, height], new Color?[width, height] };

            // Populate the board randomly based on the provided initial density
            Random rand = new Random();
            for (int i = 0; i < width; i ++)
            {
                for (int j = 0; j < height; j++)
                {
                    _scratch[_currentIndex][i, j] = (rand.NextDouble() < initialDensity) ? Color.FromArgb(rand.Next()) : (Color?)null;
                }
            }
        }

        /// <summary>Moves to the next stage of the game, returning a Bitmap that represents the state of the board.</summary>
        /// <returns>A bitmap that represents the state of the board.</returns>
        /// <remarks>The returned Bitmap should be added back to the pool supplied to the constructor when usage of it is complete.</remarks>
        public Bitmap MoveNext()
        {
            // Get the current and next stage board arrays
            int nextIndex = (_currentIndex + 1) % 2;
            Color?[,] current = _scratch[_currentIndex];
            Color?[,] next = _scratch[nextIndex];

            RuleSet ruleSet;
            switch (_ruleSet)
            {
                case "ConwaysGameOfLife":
                    ruleSet = new ConwaysGameOfLife(Width, Height);
                    break;
                case "Rule1358d357":
                    ruleSet = new Rule1358d357(Width, Height);
                    break;
                case "Rule27d257":
                    ruleSet = new Rule27d257(Width, Height);
                    break;
                default:
                    ruleSet = new ConwaysGameOfLife(Width, Height);
                    break;
            }

            //RuleSet ruleSet = new ConwaysGameOfLife(Width, Height);
            // Get a Bitmap from the pool to use
            var bmp = ruleSet.Tick(current, next, _pool, RunParallel);

            // Update and return
            _currentIndex = nextIndex;
            return bmp;
        }

        /// <summary>Gets the width of the board.</summary>
        public int Width { get; private set; }
        /// <summary>Gets the height of the board.</summary>
        public int Height { get; private set; }

        /// <summary>Gets or sets whether to run in parallel.</summary>
        public bool RunParallel { get; set; }
    }
}