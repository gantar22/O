public class Pair<T, U> {
    public Pair() {
    }

    public Pair(T first, U second) {
        this.fst = first;
        this.snd = second;
    }

    public T fst { get; set; }
    public U snd { get; set; }
}