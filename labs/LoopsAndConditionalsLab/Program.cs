//
// ======================= Task 1: Sum of Even Numbers =======================
//
// Result variables
int sumFor = SumWithFor();
int sumWhile = SumWithWhile();
int sumForEach = SumWithForEach();

// Print stuff out
System.Console.WriteLine("====== Task 1: Sum of Even Numbers ======");

System.Console.WriteLine("Using for loop         -> " + sumFor);

System.Console.WriteLine("Using for while loop   -> " + sumWhile);

System.Console.WriteLine("Using for forEach loop -> " + sumForEach);


// Task 3: Mini Challange
System.Console.WriteLine("====== Task 3: Mini Challenge ======");
if (sumFor > 2000)
{
    System.Console.WriteLine("That’s a big number! <- made with if/else structure");
}
else
{
    System.Console.WriteLine("That’s a small number! <- made with if/else structure");
}

Console.WriteLine(sumFor > 2000
    ? "That’s a big number! <- made with ternary operator"
    : "That’s a small number! <- made with ternary operator");


// Calculation functions for task 1
static int SumWithFor()
{
    int sum = 0;
    // Start at 2 (first even number) and increment by 2
    for (int i = 2; i <= 100; i += 2)
    {
        sum += i;
    }
    return sum;
}

static int SumWithWhile()
{
    int sum = 0;
    int i = 2; // Start at the first even number - (outside the loop)
    while (i <= 100)
    {
        sum += i;
        i += 2; // Increment by 2 to get the next even number
    }
    return sum;
}

static int SumWithForEach()
{
    int sum = 0;
    // make array to hold the 50 even numbers from 2 to 100
    int[] evenNumbers = new int[50];    

    // Populate the array with even numbers
    for (int i = 0; i < 50; i++)
    {
        evenNumbers[i] = (i + 1) * 2;
    }

    foreach (int number in evenNumbers)
    {
        sum += number;
    }
    return sum;
}

// --------------------------------------------------------------------------------------------------------
// ======================= Task 2: Grading with Conditionals =======================



// Print stuff out for task 2
System.Console.WriteLine("====== Task 2: Grading with Conditionals ======");
System.Console.WriteLine("Score 95 gets grade -> " + ifElse_GetLetterGrade(95) + " with if-else");
System.Console.WriteLine("Score 95 gets grade -> " + switch_GetLetterGrade(95) + " with switch case");

// ---------------------------------------------------------------
// <<<   Reminder note: in C#, '' is char and "" is string   >>>
// ---------------------------------------------------------------

// Calculate function for task 2
static char ifElse_GetLetterGrade(int score)
{
    if (score >= 90)
    {
        return 'A';
    }
    else if (score >= 80)
    {
        return 'B';
    }
    else if (score >= 70)
    {
        return 'C';
    }
    else if (score >= 60)
    {
        return 'D';
    }
    else
    {
        return 'F';
    }
}

static char switch_GetLetterGrade(int score)
{
    return score switch
    {
        >= 90 => 'A',
        >= 80 => 'B',
        >= 70 => 'C',
        >= 60 => 'D',
        _ => 'F',
    };
}