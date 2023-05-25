using System.Globalization;
void PrintList(List<List<double>> matrix)
{
    Console.WriteLine();
    for (int i = 0; i < matrix.Count; i++)
    {
        for (int j = 0; j < matrix[i].Count; j++)
        {
            Console.Write(Math.Round(matrix[i][j], 2) + "\t");
        }
        Console.WriteLine();
    }
}
void PrintListInLine(List<double> list)
{
    Console.WriteLine();
    foreach (double value in list) { Console.Write(value + "\t"); }
    Console.WriteLine();
}
bool CheckSame(List<double> a, List<double> b)
{
    for (int i = 0; i < a.Count; i++)
    {
        if (a[i] != b[i])
            return false;
    }
    return true;
}
List<double> Hopfield(List<List<double>> training_lists, List<double> corrupted_sample)
{
    int N = training_lists.Count;     //кількість образів
    int n = training_lists[0].Count;//розмірність образу
    double N_double = N;
    double n_double = n;
    List<List<double>> I = new List<List<double>>();
    for (int i = 0; i < n; i++)//заповнення одиничної матриці
    {
        List<double> temp = new List<double>();
        for (int j = 0; j < n; j++)
        {
            if (j == i)
                temp.Add(1);
            else
                temp.Add(0);
        }
        I.Add(temp);
    }
    List<List<double>> W = new List<List<double>>();//вагова матриця
    for (int i = 0; i < n; i++)//заповнення вагової матриці
    {
        List<double> temp = new List<double>();
        for (int j = 0; j < n; j++)
        {
            double temp_sum = 0;
            foreach (var list in training_lists)
            {
                temp_sum += list[i] * list[j];
            }
            double W_value = (1.0 / n_double) * (temp_sum - N_double * I[i][j]);
            temp.Add(W_value);
        }
        W.Add(temp);
    }
    Console.WriteLine("Вагова матриця:");
    PrintList(W);
    List<double> current_state = new List<double>(corrupted_sample);
    bool IsRepeating = false;
    int max_iterations = 100;
    int iterations = 0;
    while (!IsRepeating && iterations < max_iterations)
    {
        Console.WriteLine("\n\nІтерація: " + iterations);
        double GetS(List<double> state, List<double> w)
        {
            double value = 0;
            for (int i = 0; i < state.Count; i++)
            {
                value += state[i] * w[i];
            }
            return value;
        }
        List<double> new_state = new List<double>();
        Console.WriteLine("i\tf\ts[i]");
        for (int i = 0; i < corrupted_sample.Count; i++)
        {
            double value = 0;
            for (int k = 0; k < corrupted_sample.Count; k++)
            {
                value += corrupted_sample[k] * W[i][k];
            }
            if (value < 0)
                new_state.Add(-1);
            else
                new_state.Add(1);
            Console.WriteLine(i + "\t" + Math.Round(value, 2) + "\t" + new_state[i]);
        }


        if (CheckSame(new_state, current_state))
            IsRepeating = true;
        current_state = new List<double>(new_state);
        //DrawList(current_state);
        //PrintListInLine(current_state);
        iterations++;

    }
    return current_state;
}

List<double> sample_for_training_1 = new List<double>() { 1, 1, 1, -1, 1 };
List<double> sample_for_training_2 = new List<double>() { 1, 1, -1, -1, -1 };
List<double> sample_for_training_3 = new List<double>() { 1, 1, -1, -1, 1 };
List<double> corrupted_sample_for_restoring = new List<double>() { 1, 1, 1, 1, 1 };
Console.WriteLine("Еталонні образи:");
PrintListInLine(sample_for_training_1);
PrintListInLine(sample_for_training_2);
PrintListInLine(sample_for_training_3);
Console.WriteLine("Спотворений вектор:");
PrintListInLine(corrupted_sample_for_restoring);
List<double> final = Hopfield(new List<List<double>>() { sample_for_training_1, sample_for_training_2, sample_for_training_3 }, corrupted_sample_for_restoring);
Console.WriteLine("Відновлений образ:");
PrintListInLine(final);