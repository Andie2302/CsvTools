using CsvTools.Values;

namespace CsvTools.Rest;

public interface ICsvValueVisitor
{
    void Visit<T>(CsvValue<T> csvValue);
}

public interface ICsvValueVisitor < out TResult >
{
    TResult Visit < T > ( CsvValue< T > csvValue );
}
