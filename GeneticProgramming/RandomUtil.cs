using System;

namespace GeneticProgramming
{
    public static class RandomUtil
    {
        public static Random Random = new Random(DateTime.Now.Millisecond);
    }
}