using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExceptionHandler.API.Utilities
{
    public class HashingOptions
    {
        private int _iterations;

        public int Iterations
        {
            get
            {
                var random = new Random();
                return random.Next(100000, 999999);
            }
            set => _iterations = value;
        }
    }
}
