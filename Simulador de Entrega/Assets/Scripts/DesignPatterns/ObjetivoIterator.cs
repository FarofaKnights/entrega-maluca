public interface ObjetivoIterator {
    Objetivo[] Next();
    Objetivo[] GetCurrent();
    bool HasNext();
    void Reset();
}
