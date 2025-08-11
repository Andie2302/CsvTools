using CsvTools.Values;

namespace CsvTools.Rest;

public class CsvStatisticsVisitor : ICsvValueVisitor
{
    public int TotalValues { get; private set; }
    public int ModifiedValues { get; private set; }
    public int NullValues { get; private set; }

    public void Visit<T>(CsvValue<T> csvValue)
    {
        TotalValues++;

        if (csvValue.IsModified) {
            ModifiedValues++;
        }

        if (csvValue.CurrentValue == null) {
            NullValues++;
        }
    }

    public void Reset() => TotalValues = ModifiedValues = NullValues = 0;
}