﻿using System.Globalization;
void PrintMatrix(List<List<double>> matrix)
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
void PrintList(List<double> list)
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
List<List<double>> GenerateIdentityMatrix(int n)//отримати одиничну матрицю
{
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
    return I;
}
List<List<double>> GenerateWeightsMatrix(List<List<double>> training_lists)
{
    double GetSumOfInputs(int i, int j)
    {
        double output = 0;
        foreach (var list in training_lists)
        {
            output += list[i] * list[j];
        }
        return output;
    }
    int number_of_samples = training_lists.Count;     //кількість образів
    int number_of_dimensions_in_sample = training_lists[0].Count;//розмірність образу
    double N_double = number_of_samples;
    double n_double = number_of_dimensions_in_sample;
    List<List<double>> identity_matrix = GenerateIdentityMatrix(number_of_dimensions_in_sample); //одинична матриця
    List<List<double>> weights_matrix = new List<List<double>>();//вагова матриця
    for (int i = 0; i < number_of_dimensions_in_sample; i++)//заповнення вагової матриці
    {
        List<double> temp = new List<double>();
        for (int j = 0; j < number_of_dimensions_in_sample; j++)
        {
            double W_value = (1.0 / n_double) * (GetSumOfInputs(i, j) - N_double * identity_matrix[i][j]);
            temp.Add(W_value);
        }
        weights_matrix.Add(temp);
    }
    return weights_matrix;
}
List<double> GetNewState(List<double> current_state, List<List<double>> W)
{
    double GetS(List<double> state, List<double> line_of_W)
    {
        double output = 0;
        for (int i = 0; i < state.Count; i++)
        {
            output += state[i] * line_of_W[i];
        }
        return output;
    }
    List<double> new_state = new List<double>();
    Console.WriteLine("i\tf\ts[i]");
    for (int i = 0; i < current_state.Count; i++)
    {
        double value = GetS(current_state, W[i]);
        if (value < 0)
            new_state.Add(-1);
        else
            new_state.Add(1);
        Console.WriteLine(i + "\t" + Math.Round(value, 2) + "\t" + new_state[i]);
    }
    return new_state;
}
List<double> Hopfield(List<List<double>> training_lists, List<double> corrupted_sample, int max_iterations=100)
{
    List<List<double>> weights_matrix = GenerateWeightsMatrix(training_lists);
    Console.WriteLine("Вагова матриця:");
    PrintMatrix(weights_matrix);
    List<double> current_state = new List<double>(corrupted_sample);
    bool IsStatesSame = false;//чи збігається отриманий стан з попереднім станом
    int iterations = 0;
    while (!IsStatesSame && iterations < max_iterations)
    {
        Console.WriteLine("\n\nІтерація: " + iterations);
        List<double> new_state = new List<double>(GetNewState(corrupted_sample, weights_matrix));
        if (CheckSame(new_state, current_state))
            IsStatesSame = true;
        current_state = new List<double>(new_state);
        iterations++;
    }
    return current_state;
}

List<double> sample_for_training_1 = new List<double>() { 1, 1, 1, -1, 1 };
List<double> sample_for_training_2 = new List<double>() { 1, 1, -1, -1, -1 };
List<double> sample_for_training_3 = new List<double>() { 1, 1, -1, -1, 1 };
List<double> corrupted_sample_for_restoring = new List<double>() { 1, 1, 1, 1, 1 };
Console.WriteLine("Еталонні образи:");
PrintList(sample_for_training_1);
PrintList(sample_for_training_2);
PrintList(sample_for_training_3);
Console.WriteLine("Спотворений вектор:");
PrintList(corrupted_sample_for_restoring);
List<double> final = Hopfield(new List<List<double>>() { sample_for_training_1, sample_for_training_2, sample_for_training_3 }, corrupted_sample_for_restoring);
Console.WriteLine("Відновлений образ:");
PrintList(final);