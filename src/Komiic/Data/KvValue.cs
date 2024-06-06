namespace Komiic.Data;

public record KvValue<T, T1>(T Item1, T1 Item2);

public record KvValue(string Item1, string Item2) : KvValue<string, string>(Item1, Item2);