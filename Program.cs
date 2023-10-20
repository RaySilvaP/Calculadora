namespace Calculadora
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string lastResult = "";

            PrintMenu();

            while (true)
            {
                string expression = "";
                var firstKey = Console.ReadKey();

                //Key equal to backspace
                if (firstKey.KeyChar == (char)8)
                {
                    lastResult = "";
                    Console.Clear();
                    PrintMenu();
                }
                //Key equal to enter
                else if (firstKey.KeyChar == (char)13)
                {
                    Console.WriteLine();
                    return;
                }
                else
                {
                    expression += lastResult + firstKey.KeyChar;
                }

                expression += Console.ReadLine();

                if (expression == "") return;

                try
                {
                    expression = expression.Replace(" ", "");

                    List<string> postfixExpression = ConvertInfixToPostfix(expression);
                    lastResult = PostfixEval(postfixExpression).ToString();
                    Console.Write(lastResult);
                }
                catch
                {
                    Console.WriteLine("Mathematical expression invalid!");
                    return;
                }
            }
        }
        private static List<string> ConvertInfixToPostfix(string expression)
        {
            Stack<string> stack = new();
            List<string> postfix = new();
            string temp = "";

            foreach (var ch in expression)
            {
                if (ch == '(' || ch == '{' || ch == '[')
                {
                    if (temp != "") postfix.Add(temp);
                    temp = "";

                    stack.Push(ch.ToString());
                }
                else if (ch == ')' || ch == '}' || ch == '}')
                {
                    if (temp != "") postfix.Add(temp);
                    temp = "";

                    if (ch == ')')
                    {
                        while (stack.Peek() != "(")
                        {
                            postfix.Add(stack.Pop());
                        }
                    }
                    else if (ch == '}')
                    {
                        while (stack.Peek() != "{")
                        {
                            postfix.Add(stack.Pop());
                        }
                    }
                    else if (ch == ']')
                    {
                        while (stack.Peek() != "[")
                        {
                            postfix.Add(stack.Pop());
                        }
                    }

                    stack.Pop();
                }
                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '^')
                {
                    if (temp == "" && ch == '-') temp += ch;
                    else
                    {
                        if (temp != "") postfix.Add(temp);
                        temp = "";

                        if (stack.Count == 0)
                        {
                            stack.Push(ch.ToString());
                        }
                        else if (GetPrecedenceOfOperators(ch.ToString()) > GetPrecedenceOfOperators(stack.Peek()))
                        {
                            stack.Push(ch.ToString());
                        }
                        else
                        {
                            postfix.Add(stack.Pop());
                            stack.Push(ch.ToString());
                        }
                    }

                }
                else
                {
                    temp += ch;
                }
            }

            if (temp != "") postfix.Add(temp);

            while (stack.Count > 0)
            {
                postfix.Add(stack.Pop());
            }

            return postfix;
        }
        private static int GetPrecedenceOfOperators(string ch)
        {
            if (ch == "+" || ch == "-")
                return 1;
            else if (ch == "*" || ch == "/")
                return 2;
            else if (ch == "^")
                return 3;
            else
                return 0;
        }
        private static double PostfixEval(List<string> expression)
        {
            Stack<double> stack = new();

            foreach (var ch in expression)
            {
                if (double.TryParse(ch, out double n))
                {
                    stack.Push(n);
                }
                else
                {
                    double op1 = stack.Pop();
                    double op2 = stack.Pop();

                    switch (ch)
                    {
                        case "+": stack.Push(op2 + op1); break;
                        case "-": stack.Push(op2 - op1); break;
                        case "*": stack.Push(op2 * op1); break;
                        case "/": stack.Push(op2 / op1); break;
                        case "^": stack.Push(Math.Pow(op2, op1)); break;
                    }
                }
            }

            return stack.Pop();
        }
        private static void PrintMenu()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("-----Calculadora 2000-----\n");
            Console.WriteLine("+ - Adição\n- - Subtração\n* - Multiplicação");
            Console.WriteLine("/ - Divisão\n^ - Potenciação");
            Console.WriteLine("Exemplo: 2 + 10 * 5 / (10 - 8).\n");
        }
    }
}
