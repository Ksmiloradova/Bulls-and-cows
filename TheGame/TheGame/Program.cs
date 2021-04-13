using System;

namespace TheGame
{
    class Program
    {
        static void Main(string[] args)
        {
            GameRules();
            do
            {
                int volume = UserSurveyAboutLength();
                string generated = RandomGeneration(volume);
                GameProcess(generated);
                Console.WriteLine($"{Environment.NewLine}Нажмите пробел, если хотите сыграть ещё раз или другую клавишу — для завершения.{Environment.NewLine}");
            } while (Console.ReadKey().Key == ConsoleKey.Spacebar);
        }

        // Метод выводит на экран правила игры.
        public static void GameRules()
        {
            Console.Write($"Добро пожаловать в \"Быки и коровы\"!{Environment.NewLine}{Environment.NewLine}Напомню правила игры:{Environment.NewLine}{Environment.NewLine}1. Вы вводите количество цифр в загадываемом числе (от 2 до 10, в классическом случае - 4).{Environment.NewLine}2. Я генерирую число из неповторяющихся цифр. Число не может начинаться с нуля{Environment.NewLine}(при отгадывании ведущие нули не будут учитываться).{Environment.NewLine}3. Вы пробуете угадать число, а я говорю, сколько отгаданных цифр в Вашем предположении на нужных местах (быки) {Environment.NewLine}и сколько — на других позициях (коровы).{Environment.NewLine}4. Шаг 3 повторяется, пока Ваше число не совпадёт с загаданным.{Environment.NewLine}{Environment.NewLine}");
        }

        // Метод определяет, сколько цифр будет в загадваемом числе.
        public static int UserSurveyAboutLength()
        {
            int value;
            Console.Write($"Если Вы хотите играть в классическую версию игры — введите '4',{Environment.NewLine}иначе — желаемое количесво цифр в загадываемом числе: ");
            // Проверка корректности ввода количества цифр.
            while (!(int.TryParse(Console.ReadLine(), out value) && (value > 1) && (value < 11)))
            {
                Console.Write($"{Environment.NewLine}Нужно ввести число от 2 до 10.{Environment.NewLine}Попробуйте, пожалуйста, ещё раз: ");
            }
            return value;
        }

        // Метод "загадывает" число.
        public static string RandomGeneration(int volume)
        {
            var rnd = new Random();
            // Переменная под число.
            var generated = "";
            // Возможная цифра.
            int digit;
            for (var i = 0; i < volume;)
            {
                digit = rnd.Next(10);
                // Если в числе уже есть такая цифра или в качестве первой цифры сгенерировался ноль,
                // то меняю цифру.
                if ((generated.IndexOf(digit.ToString()) > -1) || ((generated.Length == 0) && (digit == 0)))
                {
                    //digit = rnd.Next(10);
                }
                // Иначе присоединяю цифру к концу.
                else
                {
                    generated += digit;
                    i++;
                }
            }
            return generated;
        }

        // Метод, организующий процесс отгадывания.
        public static void GameProcess(string generated)
        {
            int repeat, step = 2;
            Console.Write($"{Environment.NewLine}{Environment.NewLine}Число загадано.{Environment.NewLine}{Environment.NewLine}1) Ваше предположение: ");
            var attempt = Console.ReadLine();
            // Пока игрок не угадал число.
            while (attempt != generated)
            {
                repeat = IsNotRepeatability(attempt);
                // Проверка ввода (конвертируемость, количество символов, ненулевая первая цифра.
                if (long.TryParse(attempt, out long check) && (check >= Math.Pow(10, generated.Length - 1)) && (check < Math.Pow(10, generated.Length) - 1) && (repeat == 0))
                {
                    // Подсчёт и вывод количества животных.
                    BullsAndCowsCounting(generated, step, attempt);
                    // Увеличение счётчика попыток.
                    step++;
                }
                else
                {
                    Console.Write($"{Environment.NewLine}Ожидается {generated.Length}-значное число из неповторяющихся цифр.{Environment.NewLine}Число не может начинаться с нуля.{Environment.NewLine}{Environment.NewLine}Пожалуйста, повторите ввод: ");
                }
                // Считывание попытки.
                attempt = Console.ReadLine();
            }
            Console.WriteLine($"{Environment.NewLine}Поздравляю с победой!");
        }

        // Подсчёт и вывод на экран количества животных.
        public static void BullsAndCowsCounting(string generated, int step, string attempt)
        {
            var bulls = 0;
            var cows = 0;
            for (var i = 0; i < generated.Length; i++)
            {
                // Проверка, совпадает ли i-ая цифра в посылке с i-ой цифрой в загаданном числе.
                if (generated[i] == attempt[i])
                {
                    bulls++;
                }
                // Если цифра не бык, проверяю, не корова ли он (встречается ли цифра в загаданном числе).
                else if (generated.IndexOf(attempt[i]) + 1 > 0)
                {
                    cows++;
                }
            }
            Console.Write($"Быков: {bulls}  Коров: {cows}{Environment.NewLine}{Environment.NewLine}{step}) Ваше следующее предположение: ");
        }

        // Подсчёт повторяющихся цифр.
        public static int IsNotRepeatability(string attempt)
        {
            int repeat = 0;
            // Беру цифру.
            for (var i = 0; i < attempt.Length - 1; i++)
            {
                // Проверяю последующие цифры.
                for (var j = i + 1; j < attempt.Length; j++)
                {
                    // Если есть совпадение, увеличиваю счётчик.
                    if (attempt[i] == attempt[j])
                    {
                        repeat++;
                    }
                }
            }
            return repeat;
        }
    }
}
