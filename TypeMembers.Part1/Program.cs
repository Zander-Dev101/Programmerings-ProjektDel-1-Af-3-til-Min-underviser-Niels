using System.Globalization;

Console.WriteLine("Type Members - Del 1");
Console.WriteLine("Indtast din foedselsdato (fx 31-12-1990):");

var birthDate = ReadBirthDate();
var calculator = new PensionCalculator();
var result = calculator.Calculate(birthDate);

Console.WriteLine();
Console.WriteLine($"Brugerens alder: {result.Age}");
Console.WriteLine($"Antal aar til pension: {result.YearsUntilPension}");

if (!string.IsNullOrWhiteSpace(result.Reminder))
{
    Console.WriteLine(result.Reminder);
}

return;

static DateOnly ReadBirthDate()
{
    string[] formats = ["dd-MM-yyyy", "d-M-yyyy", "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd"];

    while (true)
    {
        var input = Console.ReadLine();

        if (DateOnly.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var birthDate))
        {
            return birthDate;
        }

        Console.WriteLine("Ugyldigt format. Proev igen med fx 31-12-1990:");
    }
}

internal sealed class PensionCalculator
{
    public const int PensionAge = 67;

    public PensionResult Calculate(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = CalculateAge(birthDate, today);
        var yearsUntilPension = Math.Max(PensionAge - age, 0);
        var reminder = yearsUntilPension < 5
            ? "Du skal tjekke din pensionsopsparing."
            : null;

        return new PensionResult(age, yearsUntilPension, reminder);
    }

    private static int CalculateAge(DateOnly birthDate, DateOnly today)
    {
        var age = today.Year - birthDate.Year;

        if (today < birthDate.AddYears(age))
        {
            age--;
        }

        return age;
    }
}

internal sealed record PensionResult(int Age, int YearsUntilPension, string? Reminder);
