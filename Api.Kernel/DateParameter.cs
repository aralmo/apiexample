public readonly struct DateParameter
{
    //just a url date parameter mapping to format as ddMMyyyy
    public readonly DateOnly Date { get; init; }

    //duck typing converter for minimal api.
    public static bool TryParse(string input, out DateParameter date)
    {
        if (DateOnly.TryParseExact(input, "ddMMyyyy", out DateOnly parsed))
        {
            date = new DateParameter() { Date = parsed };
            return true;
        }

        date = default;
        return false;
    }

    public static implicit operator DateOnly(DateParameter p)
        => p.Date;
}
