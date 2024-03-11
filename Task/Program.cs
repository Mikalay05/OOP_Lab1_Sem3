using Newtonsoft.Json;
using System.Text;
//Cортировка
//поиск
//топ
namespace Task
{
    struct Coffee
    {
        public string Name;//название сорта кофе
        public decimal Price;//цена
        public int Rating;//рейтинг качества (от 1 до 10)
        public string RoastLevel;//степень обжарки (светлая, средняя, темная)
        public bool IsGround;//является ли кофе молотым
    }
    internal class Program
    {
        const string filePath = "coffees.json";
        /// <summary>
        /// Выводит список доступных действий
        /// </summary>
        static void OutputOfFeatures()
        {
            Console.WriteLine($"Доступные действия:\n"
                + $"1. Новый кофе\n"
                + $"2. Изменения данных о коффе\n"
                + $"3. Удалить кофе\n"
                + $"4. Посмотреть сущщесвующие кофе\n"
                + $"5. Поиск кофе\n"
                + $"6. Посмотреть рейтинг по разным параметрам\n"
                + $"7. Сохранить изменения и выйти");
        }
        /// <summary>
        /// Вывод цветного сообщения в консоль
        /// </summary>
        /// <param name="textMessange">текст сообщения</param>
        /// <param name="colorMessange">цвет сообщения</param>
        /// <param name="defaultColor">цвет после вывода сообщения</param>
        static void OutputColorMessange(string textMessange, ConsoleColor colorMessange, ConsoleColor defaultColor = ConsoleColor.White)
        {
            Console.ForegroundColor = colorMessange;
            Console.WriteLine(textMessange);
            Console.ForegroundColor = defaultColor;
        }
        static void CreateNewCoffe(ref Coffee[] coffees)
        {
            ChangeInfoCoffee(ref coffees);
        }
        static void ChangeInfoCoffee(ref Coffee[] coffees, int idOfCoffee = -1)
        {
            Coffee thisCoffee;
            if (idOfCoffee != -1)
            {
                thisCoffee = new Coffee
                {
                    Name = coffees[idOfCoffee].Name,
                    Price = coffees[idOfCoffee].Price,
                    Rating = coffees[idOfCoffee].Rating,
                    RoastLevel = coffees[idOfCoffee].RoastLevel,
                    IsGround = coffees[idOfCoffee].IsGround,
                };
            }
            else
            {
                thisCoffee = new Coffee
                {
                    Name = "",
                    Price = 0,
                    Rating = -1,
                    RoastLevel = "",
                    IsGround = false
                };
            }
            bool createNewCoffe = true;

            while (createNewCoffe)
            {
                Console.WriteLine("Выберите параметр, который хотите изменить");
                OutputDataOfCoffee(thisCoffee);
                Console.WriteLine("Если хотите вернуться в меню (без сохранения), введите '-1'");
                Console.WriteLine("Если хотите сохранить кофе, введите '0'");
                switch (Console.ReadLine() ?? "")
                {
                    case "-1":
                        Console.Clear();//Выход без сохранения
                        createNewCoffe = false;
                        break;
                    case "0":
                        Console.Clear();//Сохранить кофе
                        SafeChangeOfCoffee(ref coffees, thisCoffee, ref createNewCoffe, idOfCoffee);
                        break;
                    case "1":
                        Console.Clear();//Название
                        NewNameOfCoffee(coffees, ref thisCoffee);
                        break;
                    case "2":
                        Console.Clear();//Цена
                        NewPriceOfCoffee(ref thisCoffee);
                        break;
                    case "3":
                        Console.Clear();//Рейтинг
                        NewRatingOfCoffee(ref thisCoffee);
                        break;
                    case "4":
                        Console.Clear();//Степень прожарки
                        NewRoastLevelOfCoffee(ref thisCoffee);
                        break;
                    case "5":
                        Console.Clear();//Молотый   
                        ChangeGroundType(ref thisCoffee);
                        break;
                    default: Console.Clear(); OutputColorMessange("Ошибка ввода!", ConsoleColor.Red); break;
                }
            }
        }
        static void SafeChangeOfCoffee(ref Coffee[] coffees, Coffee thisCoffee, ref bool createNewCoffe, int idOfCoffee)
        {
            if (!String.IsNullOrEmpty(thisCoffee.Name))
            {
                if (thisCoffee.Price != 0)
                {
                    if (!String.IsNullOrEmpty(thisCoffee.RoastLevel))
                    {
                        if (idOfCoffee != -1)
                        {
                            coffees[idOfCoffee] = thisCoffee;
                            createNewCoffe = false;
                        }
                        else
                        {
                            Array.Resize(ref coffees, coffees.Length + 1);
                            coffees[coffees.Length - 1] = thisCoffee;
                            createNewCoffe = false;
                        }
                        OutputColorMessange("Кофе успешно сохранено!", ConsoleColor.Green);

                    }
                    else
                    {
                        OutputColorMessange("Измените степень прожарки!", ConsoleColor.Red);
                    }
                }
                else
                {
                    OutputColorMessange("Измените цену на кофе!", ConsoleColor.Red);

                }
            }
            else
            {
                OutputColorMessange("Измените имя кофе!", ConsoleColor.Red);
            }
        }
        /// <summary>
        /// Изменения типа кофе (молотый / не молотый)
        /// </summary>
        /// <param name="thisCoffee"></param>
        static void ChangeGroundType(ref Coffee thisCoffee)
        {
            if (AgreeToChangeGroundType())
            {
                thisCoffee.IsGround = !thisCoffee.IsGround;
                Console.Clear();
                OutputColorMessange($"Тип кофе был изменен на {(thisCoffee.IsGround ? "молотый" : "не молотый")}!", ConsoleColor.Green);
            }
        }
        /// <summary>
        /// Получает согласие пользователя на изменения типа
        /// </summary>
        /// <returns></returns>
        static bool AgreeToChangeGroundType()
        {
            while (true)
            {
                Console.WriteLine("Вы согласны на изменение вида молотого кофе? (Введите 'да' или для возврата ничего не вводите)");
                string input = Console.ReadLine()?.ToLower();
                if (String.IsNullOrEmpty(input))
                {
                    Console.Clear();
                    return false;
                }
                if (input == "да")
                {
                    return true;
                }
                else
                {
                    Console.Clear();
                    OutputColorMessange("Ошибка ввода!", ConsoleColor.Red);
                }
            }
        }
        /// <summary>
        ///Изменение степени прожарки 
        /// </summary>
        /// <param name="thisCoffee">кофе, которому меняем рейтинг</param>
        static void NewRoastLevelOfCoffee(ref Coffee thisCoffee)
        {
            bool workWithNewRoastLevel = true;
            string newRoastLevel;

            while (workWithNewRoastLevel)
            {
                Console.Write("Введите новую степень обжарки кофе (светлая, средняя, темная, для возврата ничего не вводите): ");
                newRoastLevel = Console.ReadLine()?.ToLower();

                if (string.IsNullOrEmpty(newRoastLevel))
                {
                    Console.Clear();
                    return;
                }

                if (newRoastLevel == "светлая" || newRoastLevel == "средняя" || newRoastLevel == "темная")
                {
                    workWithNewRoastLevel = false;
                    thisCoffee.RoastLevel = newRoastLevel;
                    Console.Clear();
                    OutputColorMessange("Степень обжарки изменена!", ConsoleColor.Green);
                }
                else
                {
                    Console.Clear();
                    OutputColorMessange("Недопустимое значение степени обжарки. Пожалуйста, выберите из предложенных вариантов: светлая, средняя, темная.", ConsoleColor.Red);
                }
            }
        }

        /// <summary>
        /// Изменение рейтинга кофе
        /// </summary>
        /// <param name="thisCoffee">кофе, которому меняем рейтинг</param>
        static void NewRatingOfCoffee(ref Coffee thisCoffee)
        {
            bool workWithNewRating = true;
            int newRating;

            while (workWithNewRating)
            {
                Console.Write("Введите новый рейтинг кофе (от 1 до 10, '-1' для обозначения 'Без рейтинга', для возврата ничего не вводите): ");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.Clear();
                    return;
                }

                if (int.TryParse(input, out newRating))
                {
                    if (newRating == -1 || (newRating >= 1 && newRating <= 10))
                    {
                        workWithNewRating = false;
                        thisCoffee.Rating = newRating;
                        Console.Clear();
                        OutputColorMessange("Рейтинг изменен!", ConsoleColor.Green);
                    }
                    else
                    {
                        Console.Clear();
                        OutputColorMessange("Рейтинг должен быть числом от 1 до 10 или равняться '-1'.", ConsoleColor.Red);
                    }
                }
                else
                {
                    Console.Clear();
                    OutputColorMessange("Ошибка ввода рейтинга!", ConsoleColor.Red);
                }
            }
        }

        /// <summary>
        /// Изменения цены кофе
        /// </summary>
        /// <param name="thisCoffee">кофе, которому меняем цену</param>
        static void NewPriceOfCoffee(ref Coffee thisCoffee)
        {
            bool workWithNewPrice = true;
            decimal newPrice;
            while (workWithNewPrice)
            {
                Console.Write("Введите новую цену кофе (для возврата ничего не вводите): ");
                string input = Console.ReadLine() ?? "";
                if (String.IsNullOrEmpty(input))
                {
                    Console.Clear();
                    return;
                }
                if (decimal.TryParse(input, out newPrice))
                {
                    if (newPrice > 0)
                    {
                        workWithNewPrice = false;
                        thisCoffee.Price = newPrice;
                        Console.Clear();
                        OutputColorMessange("Цена изменена!", ConsoleColor.Green);
                    }
                    else
                    {
                        OutputColorMessange("Цена должна быть положительным числом.", ConsoleColor.Red);
                    }
                }
                else
                {
                    Console.Clear();
                    OutputColorMessange("Ошибка ввода цены!", ConsoleColor.Red);
                }
            }

        }
        /// <summary>
        /// Изменение имя кофе
        /// </summary>
        /// <param name="coffees">смотрим, чтобы имя было универсальное</param>
        /// <param name="thisCoffee">меняет название этого объекта кофе</param>
        static void NewNameOfCoffee(Coffee[] coffees, ref Coffee thisCoffee)
        {
            bool workWithName = true;
            string nameOfCoffee;
            while (workWithName)
            {
                Console.Write("Введите название кофе (для возврата ничего не вводите): ");
                nameOfCoffee = Console.ReadLine() ?? "";
                if (String.IsNullOrEmpty(nameOfCoffee))
                {
                    Console.Clear();
                    return;
                }
                if (ItNewNameOfCoffe(nameOfCoffee, coffees))
                {
                    thisCoffee.Name = nameOfCoffee;
                    Console.Clear();
                    OutputColorMessange("Название изменено!", ConsoleColor.Green);
                    workWithName = false;
                }
                else
                {
                    Console.Clear();
                    OutputColorMessange("Такое название кофе уже существует!", ConsoleColor.Red);
                }
            }
        }
        /// <summary>
        /// Вывод данные о кофе с пронумерованными параметрами
        /// </summary>
        /// <param name="coffee">кофе, о котором выдаются параметры</param>
        static void OutputDataOfCoffee(Coffee coffee)
        {
            Console.WriteLine("Данные о кофе:\n" +
                $"1. Название кофе: {coffee.Name}\n" +
                $"2. Цена кофе: {coffee.Price}\n" +
                $"3. Рейтинг качества: {(coffee.Rating == -1 ? "Без рейтинга" : coffee.Rating + "/10")}\n" +
                $"4. Степень обжарки: {coffee.RoastLevel}\n" +
                $"5. Кофе: {(coffee.IsGround ? "молотый" : "не молотый")}\n");
        }
        /// <summary>
        /// Проверяет, существует ли такое название кофе
        /// </summary>
        /// <param name="newNameCoffe">Новое имя кофе</param>
        /// <param name="coffees">Массив существующих кофе</param>
        /// <returns></returns>
        static bool ItNewNameOfCoffe(string newNameCoffe, Coffee[] coffees)
        {
            foreach (Coffee elementOfCoffee in coffees)
            {
                if (elementOfCoffee.Name == newNameCoffe)
                {
                    return false;
                }
            }
            return true;
        }
        static void OutputCoffee(Coffee[] coffees)
        {
            int columnWidthNumber = 7;
            int columnWidthName = 24;
            int columnWidthPrice = 10;
            int columnWidthRating = 15;
            int columnWidthRoastLevel = 20;
            int columnWidthIsGround = 15;
            string leveling = "-";

            string formattedString = String.Format("{0," + leveling + columnWidthNumber + "}{1," + leveling + columnWidthName + "}{2," + leveling + columnWidthPrice + "}{3," + leveling + columnWidthRating + "}{4," + leveling + columnWidthRoastLevel + "}{5," + leveling + columnWidthIsGround + "}", "№ п/п", "Название", "Цена", "Рейтинг", "Степень прожарки", "Тип кофе");

            Console.WriteLine($"{formattedString}");

            int number = 1;
            foreach (Coffee cof in coffees)
            {
                string outputCof = String.Format("{0," + leveling + columnWidthNumber + "}{1," + leveling + columnWidthName + "}{2," + leveling + columnWidthPrice + "}{3," + leveling + columnWidthRating + "}{4," + leveling + columnWidthRoastLevel + "}{5," + leveling + columnWidthIsGround + "}", number, cof.Name, cof.Price, (cof.Rating == -1 ? "Без рейтинга" : cof.Rating + "/10"), cof.RoastLevel, (cof.IsGround ? "молотый" : "не молотый"));
                Console.WriteLine(outputCof);
                number++;
            }

        }
        static void Main(string[] args)
        {
            string jsonText = File.ReadAllText(filePath, Encoding.UTF8);
            // Десериализация JSON в массив структур Coffee
            Coffee[] coffees = JsonConvert.DeserializeObject<Coffee[]>(jsonText);
            bool exit = false;
            while (!exit)
            {
                OutputOfFeatures();
                string goTo = Console.ReadLine() ?? "";
                switch (goTo)
                {
                    case "1":
                        Console.Clear(); // Новый кофе
                        CreateNewCoffe(ref coffees);
                        break;
                    case "2":
                        Console.Clear(); //Изменения данных о коффе
                        ChangeCoffee(ref coffees);
                        break;
                    case "3":
                        Console.Clear();//Удалить кофе
                        DeleteCoffe(ref coffees);
                        break;
                    case "4":
                        Console.Clear();//Посмотреть сущесвующие кофе
                        OutputCoffee(coffees);
                        Console.WriteLine("<нажмите любую кнопку, чтобы продолжить>");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case "5":
                        Console.Clear();//Поиск кофе
                        break;
                    case "6":
                        Console.Clear();//Посмотреть рейтинг по разным параметрам
                        break;
                    case "7":
                        Console.Clear();//Сохранить изменения и выйти
                        exit = TrySaveDateInFile(coffees);
                        break;
                    default: Console.Clear(); OutputColorMessange("Ошибка ввода!", ConsoleColor.Red); break;
                }
                static void ChangeCoffee(ref Coffee[] coffees)
                {
                    int index = InputIDOfCoffee(coffees);
                    if (index == -1)
                    {
                        return;
                    }
                    ChangeInfoCoffee(ref coffees, index);
                }
                static void SaveToJsonFile(Coffee[] data)
                {
                    // Сериализация массива структур Coffee в JSON
                    string jsonText = JsonConvert.SerializeObject(data, Formatting.Indented);

                    // Запись JSON в файл
                    File.WriteAllText(filePath, jsonText);
                }
                static bool TrySaveDateInFile(Coffee[] coffees)
                {
                    try
                    {
                        SaveToJsonFile(coffees);

                        OutputColorMessange("Данные сохранены в файл", ConsoleColor.Green);
                        Console.WriteLine("Для заверщения программы, нажмите любую кнопку");
                        Console.ReadLine();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        OutputColorMessange($"An error occurred: {ex.Message}", ConsoleColor.Red);
                        return false;
                    }
                }
                static int InputIDOfCoffee(Coffee[] coffees)
                {
                    while (true)
                    {
                        OutputCoffee(coffees);
                        Console.Write("Введите номер коффе, который вы хотите изменить (для возврата ничего не пишите): ");
                        string input = Console.ReadLine();
                        if (String.IsNullOrEmpty(input))
                        {
                            Console.Clear();
                            return -1;
                        }
                        if (int.TryParse(input, out int index))
                        {
                            index--;
                            if (index < 0 || index >= coffees.Length)
                            {
                                Console.Clear();
                                OutputColorMessange("Invalid index.", ConsoleColor.Red);

                            }
                            else
                            {
                                Console.Clear();
                                return index;
                            }

                        }
                        else
                        {
                            Console.Clear();
                            OutputColorMessange("Ошибка ввода.", ConsoleColor.Red);
                        }
                    }
                }
                static void DeleteCoffe(ref Coffee[] coffees)
                {
                    while (true)
                    {
                        OutputCoffee(coffees);
                        Console.Write("Введите номер коффе, который вы хотите удалить (для возврата ничего не пишите): ");
                        string input = Console.ReadLine();
                        if (String.IsNullOrEmpty(input))
                        {
                            Console.Clear();
                            return;
                        }
                        if (int.TryParse(input, out int indexFroDelete))
                        {
                            if (TryRemoveCoffeeAtIndex(ref coffees, indexFroDelete - 1))
                            {
                                Console.Clear();
                                OutputColorMessange("Элмент был удален!", ConsoleColor.Green);
                                return;
                            }
                        }
                        else
                        {
                            Console.Clear();
                            OutputColorMessange("Ошибка ввода.", ConsoleColor.Red);
                        }
                    }
                }
                static bool TryRemoveCoffeeAtIndex(ref Coffee[] coffees, int index)
                {
                    if (index < 0 || index >= coffees.Length)
                    {
                        Console.Clear();
                        OutputColorMessange("Invalid index.", ConsoleColor.Red);
                        return false;
                    }

                    for (int i = index; i < coffees.Length - 1; i++)
                    {
                        coffees[i] = coffees[i + 1];
                    }

                    Array.Resize(ref coffees, coffees.Length - 1);
                    return true;
                }
            }
        }
    }
}