using System;
using System.Collections.Generic;

// Інтерфейс виразу
public interface IExpression
{
    double Interpret();
}

// Клас для чисел
public class NumberExpression : IExpression
{
    private double _number;

    public NumberExpression(double number)
    {
        _number = number;
    }

    public double Interpret()
    {
        return _number;
    }
}

// Абстрактний клас для операторів
public abstract class OperatorExpression : IExpression
{
    protected IExpression _left;
    protected IExpression _right;

    public OperatorExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public abstract double Interpret();
}

// Клас для додавання
public class AdditionExpression : OperatorExpression
{
    public AdditionExpression(IExpression left, IExpression right) : base(left, right) { }

    public override double Interpret()
    {
        return _left.Interpret() + _right.Interpret();
    }
}

// Клас для множення
public class MultiplicationExpression : OperatorExpression
{
    public MultiplicationExpression(IExpression left, IExpression right) : base(left, right) { }

    public override double Interpret()
    {
        return _left.Interpret() * _right.Interpret();
    }
}

// Клас для ділення
public class DivisionExpression : OperatorExpression
{
    public DivisionExpression(IExpression left, IExpression right) : base(left, right) { }

    public override double Interpret()
    {
        return _left.Interpret() / _right.Interpret();
    }
}

public class ExpressionParser
{
    public static IExpression Parse(string expression)
    {
        string rpn = ConvertToRPN(expression);
        return ParseRPN(rpn);
    }

    private static string ConvertToRPN(string infix)
    {
        Stack<char> operators = new Stack<char>();
        Queue<string> output = new Queue<string>();

        int i = 0;
        while (i < infix.Length)
        {
            if (char.IsWhiteSpace(infix[i]))
            {
                i++;
                continue;
            }

            if (char.IsDigit(infix[i]))
            {
                string number = string.Empty;
                while (i < infix.Length && (char.IsDigit(infix[i]) || infix[i] == '.'))
                {
                    number += infix[i];
                    i++;
                }
                output.Enqueue(number);
            }
            else if (infix[i] == '(')
            {
                operators.Push(infix[i]);
                i++;
            }
            else if (infix[i] == ')')
            {
                while (operators.Peek() != '(')
                {
                    output.Enqueue(operators.Pop().ToString());
                }
                operators.Pop();
                i++;
            }
            else if (IsOperator(infix[i]))
            {
                while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(infix[i]))
                {
                    output.Enqueue(operators.Pop().ToString());
                }
                operators.Push(infix[i]);
                i++;
            }
        }

        while (operators.Count > 0)
        {
            output.Enqueue(operators.Pop().ToString());
        }

        return string.Join(" ", output);
    }

    private static bool IsOperator(char c)
    {
        return c == '+' || c == '*' || c == '/';
    }

    private static int Precedence(char c)
    {
        switch (c)
        {
            case '+':
                return 1;
            case '*':
            case '/':
                return 2;
            default:
                return 0;
        }
    }

    private static IExpression ParseRPN(string rpn)
    {
        Stack<IExpression> stack = new Stack<IExpression>();
        string[] tokens = rpn.Split(' ');

        foreach (string token in tokens)
        {
            switch (token)
            {
                case "+":
                    {
                        IExpression right = stack.Pop();
                        IExpression left = stack.Pop();
                        stack.Push(new AdditionExpression(left, right));
                        break;
                    }
                case "*":
                    {
                        IExpression right = stack.Pop();
                        IExpression left = stack.Pop();
                        stack.Push(new MultiplicationExpression(left, right));
                        break;
                    }
                case "/":
                    {
                        IExpression right = stack.Pop();
                        IExpression left = stack.Pop();
                        stack.Push(new DivisionExpression(left, right));
                        break;
                    }
                default:
                    stack.Push(new NumberExpression(double.Parse(token)));
                    break;
            }
        }

        return stack.Pop();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введіть вираз (наприклад, '(3+4)*2'):");
        string input = Console.ReadLine();
        IExpression expression = ExpressionParser.Parse(input);
        double result = expression.Interpret();
        Console.WriteLine("Результат: " + result);
    }
}