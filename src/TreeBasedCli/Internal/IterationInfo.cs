namespace TreeBasedCli.Internal
{
    internal class IterationInfo<T>
    {
        public IterationInfo(T element, int index, bool isFirst, bool isLast)
        {
            this.CurrentElement = element;
            this.Index = index;
            this.IsFirstIteration = isFirst;
            this.IsLastIteration = isLast;
        }

        public T CurrentElement { get; }
        public int Index { get; }
        public bool IsFirstIteration { get; }
        public bool IsLastIteration { get; }
    }
}