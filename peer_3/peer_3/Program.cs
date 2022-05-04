using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace peer_3
{
    /// <summary>
    ///     Основной класс программы.
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     Метод приветствия пользователя.
        /// </summary>
        public static void Greetings()
        {
            Console.Title = "Фаловый менеджер";
            Console.SetWindowSize(160, 40);
            Console.WriteLine("Приветствую!");
            Console.WriteLine("Это программа файловый менеджер.");
            Console.WriteLine("Изначально ты находишься в папке: {0}", Directory.GetCurrentDirectory());
            Console.WriteLine("В функциях, требующих путь, ты можешь указывать или полный путь до " +
                              "файла или относительный от папки в которой ты находишься.");
            Console.WriteLine("");
            Console.WriteLine("Для начала работы нажми любую клавишу.");
            Console.ReadKey();
        }

        /// <summary>
        ///     Метод вывода сообщений об ошибках.
        /// </summary>
        /// <param name="message">Сообщений, выводимое пользователю.</param>
        private static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        ///     Метод получения шаблона для поиска файла.
        /// </summary>
        /// <returns>Строка - шаблон пользователя для поиска.</returns>
        public static string GetUserRegex()
        {
            Console.WriteLine("Введи шаблон для поиска:");
            var userRegex = Console.ReadLine();
            return userRegex ?? "";
        }
        
        /// <summary>
        ///     Метод вывода меню.
        /// </summary>
        public static void Menu()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" 0. Просмотр текущего месторасположение");
            Console.WriteLine(" 1. Просмотр списка дисков компьютера и выбор диска;");
            Console.WriteLine(" 2. Переход в другую директорию (выбор папки);");
            Console.WriteLine(" 3. Просмотр списка файлов в директории;");
            Console.WriteLine(" 4. Вывод содержимого текстового файла в консоль в кодировке UTF-8;");
            Console.WriteLine(" 5. Вывод содержимого текстового файла в консоль в выбранной пользователем кодировке;");
            Console.WriteLine(" 6. Копирование файла;");
            Console.WriteLine(" 7. Перемещение файла в выбранную директорию;");
            Console.WriteLine(" 8. Удаление файла;");
            Console.WriteLine(" 9. Создание текстового файла в кодировке UTF-8;");
            Console.WriteLine("10. Создание текстового файла в кодировке выбранной пользователем кодировке;");
            Console.WriteLine("11. Конкатенация содержимого нескольких текстовых файлов;");
            Console.WriteLine("12. Просмотр списка файлов в директории по маске;");
            Console.WriteLine("13. Просмотр списка файлов в директории и всех ее поддиреториях по маске;");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        ///     Метод получения пути до директории от пользователя.
        /// </summary>
        /// <param name="message">Сообщение, дополняющее запрос пути.</param>
        /// <returns>Стррока - путь до директории.</returns>
        public static string GetUserDirectoryPath(string message)
        {
            Console.WriteLine("Введи название папки {0}:", message);
            string path;
            while (!Directory.Exists(path = Console.ReadLine()))
                ErrorMessage("Этой папки не существует, давай по новой");

            return path;
        }

        /// <summary>
        ///     Метод получения пользовательсткой кодировки.
        /// </summary>
        /// <returns>Кодировка, требуемая пользователем.</returns>
        public static Encoding GetUserEncoding()
        {
            Encoding[] encodings = { Encoding.UTF8, Encoding.Unicode, Encoding.UTF32, Encoding.ASCII };
            Console.WriteLine("1. UTF8");
            Console.WriteLine("2. UTF16");
            Console.WriteLine("3. UTF32");
            Console.WriteLine("4. ASCII (без русской части)");

            var choice = GetUserInput("Введи номер кодировки: ", 1, 4);
            return encodings[choice - 1];
        }

        /// <summary>
        ///     Метод считывания строк до окончания ввода.
        /// </summary>
        /// <returns>Список строк, введенных пользователем.</returns>
        private static List<string> GetInputLines()
        {
            var fileLines = new List<string>();
            Console.WriteLine("Введи текст, который хочешь записать в файл: (ctrl+z для окончания ввода)");
            string inputLine;
            while ((inputLine = Console.ReadLine()) != null)
                fileLines.Add(inputLine);
            return fileLines;
        }

        /// <summary>
        ///     Метод проверки пользовательского ввода.
        /// </summary>
        /// <param name="message">Сообщение, выводимое пользователю, о том, что ему необходимо ввести.</param>
        /// <param name="bottomBorder">Нижняя граница возможных значений.</param>
        /// <param name="upperBorder">Верхняя граница возможных значений.</param>
        /// <returns>Число, введенное пользователем.</returns>
        public static int GetUserInput(string message, int bottomBorder, int upperBorder)
        {
            int choice;
            Console.WriteLine(message);
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < bottomBorder || choice > upperBorder)
                ErrorMessage("Неккоректный ввод, попробуй еще раз");

            return choice;
        }

        /// <summary>
        ///     Метод считывания пользовательского пути до файла.
        /// </summary>
        /// <param name="message">Сообщение, дополняющее запрос пути.</param>
        /// <returns>Строка - путь до файла.</returns>
        private static string GetFilePath(string message)
        {
            Console.WriteLine(message);
            string path;
            while (!File.Exists(path = Console.ReadLine()))
                ErrorMessage("Данного файла не существует, давай по новой");

            return path;
        }

        /// <summary>
        ///     Метод получение требуемой пользователем операции.
        /// </summary>
        public static void GetUsersChoice()
        {
            var choice = GetUserInput("Введи номер операции от 0 до 13:", 0, 13);
            Console.Clear();
            switch (choice)
            {
                case 1:
                    ChangeDirectory(GetDrives());
                    break;
                case 2:
                    foreach (var directory in GetAllDirectories(Directory.GetCurrentDirectory()))
                        Console.WriteLine(directory);
                    ChangeDirectory(GetUserDirectoryPath("в которую хочешь перейти"));
                    break;
                case 3:
                    var directoryChoice =
                        GetUserInput(
                            "Вы хотите ввести путь до директории или использовать текущую (0 - ввести, 1 - текущая)",
                            0, 1);
                    var path = directoryChoice == 0
                        ? GetUserDirectoryPath("список файлов в которой хочешь получить")
                        : Directory.GetCurrentDirectory();
                    GetListOfFiles(path);
                    break;
                case 4:
                    foreach (var line in GetFileLines(Encoding.UTF8))
                        Console.WriteLine(line);
                    break;
                case 5:
                    foreach (var line in GetFileLines(GetUserEncoding()))
                        Console.WriteLine(line);
                    break;
                case 6:
                    CopyFile();
                    break;
                case 7:
                    MoveFile();
                    break;
                case 8:
                    DeleteFile();
                    break;
                case 9:
                    CreateFile(Encoding.UTF8, GetInputLines());
                    break;
                case 10:
                    CreateFile(GetUserEncoding(), GetInputLines());
                    break;
                case 11:
                    ConcatenateFiles();
                    break;
                case 12:
                    var userRegex = GetUserRegex();
                    directoryChoice =
                        GetUserInput(
                            "Вы хотите ввести путь до директории или использовать текущую (0 - ввести, 1 - текущая)",
                            0, 1);
                    path = directoryChoice == 0
                        ? GetUserDirectoryPath("список файлов в которой хочешь получить")
                        : Directory.GetCurrentDirectory();
                    GetListOfFiles(path, userRegex);
                    break;
                case 13:
                    userRegex = GetUserRegex();
                    path = GetUserDirectoryPath("Введи начальную директорию");
                    GetListOfFileFromSubDirs(path, userRegex);
                    break;
                case 0:
                    Console.WriteLine("Ваша текущая директория: ");
                    Console.WriteLine(Directory.GetCurrentDirectory());
                    break;
                default:
                    ErrorMessage("Ты как вообще сюда попал?");
                    break;
            }
        }

        /// <summary>
        ///     Метод получение файлов из всех поддиректорий.
        /// </summary>
        /// <param name="path">Путь до исходной директории.</param>
        /// <param name="pattern">Шаблон поиска.</param>
        public static void GetListOfFileFromSubDirs(string path, string pattern = "")
        {
            var subDirectories = GetAllDirectories(path);
            GetListOfFiles(path, pattern);
            foreach (var directory in subDirectories) GetListOfFiles(directory, pattern);
        }

        /// <summary>
        ///     Метод конкатенации нескольких файлов.
        /// </summary>
        private static void ConcatenateFiles()
        {
            var allFileLines = new List<string>();
            var filesAmount = GetUserInput("Введи количество файлов: (1 - 100)", 1, 100);
            for (var i = 0; i < filesAmount; i++)
            {
                Console.WriteLine("Введи {0} файл:", i + 1);
                var fileLines = GetFileLines(Encoding.UTF8);
                
                allFileLines.AddRange(fileLines);
            }
            foreach (var line in allFileLines)
                Console.WriteLine(line);
            if (Convert.ToBoolean(GetUserInput("Хочешь сохранить в файл? (0 - нет, 1 - да)", 0, 1)))
                CreateFile(Encoding.UTF8, allFileLines);
        }

        /// <summary>
        ///     Метод удаления файла.
        /// </summary>
        public static void DeleteFile()
        {
            var filePath = GetFilePath("Введи путь до удаляемого файла:");
            try
            {
                File.Delete(filePath);
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage("Доступ к данному файлу запрещен.");
            }
            catch (Exception exception)
            {
                ErrorMessage("Непредвиденная ошибка: " + exception.Message);
            }
        }

        /// <summary>
        ///     Метод перемещения файла.
        /// </summary>
        public static void MoveFile()
        {
            var sourceFile = GetFilePath("Введи путь до исходного файла:");
            Console.WriteLine("Введи путь до получаемого файла: ");
            var destinationFile = Console.ReadLine();
            
            try
            {
                var overwrite = 0;
                if (string.IsNullOrEmpty(destinationFile)) 
                    destinationFile = Path.GetFileName(sourceFile);
                if (string.IsNullOrEmpty(Path.GetFileName(destinationFile)))
                    destinationFile += Path.GetFileName(sourceFile);
                if (File.Exists(destinationFile))
                {
                    ErrorMessage("Файл будет перезаписан!");
                    overwrite = 1;
                }

                File.Move(sourceFile, destinationFile, Convert.ToBoolean(overwrite));
                Console.WriteLine("Файл успешно перемещен!");
            }
            catch (ArgumentNullException)
            {
                ErrorMessage("Название файла не может быть null.");
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage("Доступ к данному файлу запрещен.");
            }
            catch (DirectoryNotFoundException)
            {
                ErrorMessage("Отсутствует часть пути до файла, пробуй по новой.");
            }
            catch (NotSupportedException)
            {
                ErrorMessage("Путь до файла имеет недопустимый формат.");
            }
            catch (IOException)
            {
                ErrorMessage("Перезапись файла невозможна.");
            }
            catch (ArgumentException)
            {
                ErrorMessage("Пустое имя файла недопустимо.");
            }
            catch (Exception exception)
            {
                ErrorMessage("Непредвиденная ошибка: " + exception.Message);
            }
        }

        /// <summary>
        ///     Метод копирования файла.
        /// </summary>
        public static void CopyFile()
        {
            var sourceFile = GetFilePath("Введи путь до исходного файла:");
            Console.WriteLine("Введи путь до получаемого файла: ");
            var destinationFile = Console.ReadLine();
            try
            {
                var overwrite = 0;
                if (string.IsNullOrEmpty(destinationFile))
                    destinationFile = Path.GetFileName(sourceFile);
                if (string.IsNullOrEmpty(Path.GetFileName(destinationFile)))
                    destinationFile += Path.GetFileName(sourceFile);
                if (File.Exists(destinationFile))
                {
                    ErrorMessage("Файл будет перезаписан!");
                    overwrite = 1;
                }

                File.Copy(sourceFile, destinationFile, Convert.ToBoolean(overwrite));
                Console.WriteLine("Файл успешно скопирован!");
            }
            catch (ArgumentNullException)
            {
                ErrorMessage("Название файла не может быть null.");
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage("Доступ к данному файлу запрещен.");
            }
            catch (DirectoryNotFoundException)
            {
                ErrorMessage("Отсутствует часть пути до получаемого файла, пробуй по новой.");
            }
            catch (NotSupportedException)
            {
                ErrorMessage("Путь до получаемого файла имеет недопустимый формат.");
            }
            catch (IOException)
            {
                ErrorMessage("Перезапись файла невозможна.");
            }
            catch (ArgumentException)
            {
                ErrorMessage("Пустое имя файла недопустимо.");
            }
            catch (Exception exception)
            {
                ErrorMessage("Непредвиденная ошибка: " + exception.Message);
            }
        }

        /// <summary>
        ///     Метод создания файла.
        /// </summary>
        /// <param name="encoding">Кодировка, требуемая пользователем.</param>
        /// <param name="fileLines">Строки создаваемого файла.</param>
        public static void CreateFile(Encoding encoding, List<string> fileLines)
        {
            Console.WriteLine("Введи путь до создаваемого файла:");
            var filePath = Console.ReadLine();
            while (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Название файла не может быть пустым, давай еще раз");
                filePath = Console.ReadLine();
            }

            if (string.IsNullOrEmpty(Path.GetFileName(filePath)))
                filePath += "a.txt";

            if (File.Exists(filePath))
                ErrorMessage("Указанный файл будет перезаписан!");
            try
            {
                using var streamWriter = new StreamWriter(filePath, false, encoding);
                foreach (var line in fileLines)
                    streamWriter.WriteLine(line);
            }
            catch (DirectoryNotFoundException)
            {
                ErrorMessage("Отсутствует часть пути до получаемого файла, пробуй по новой.");
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage("Доступ к данной папке запрещен.");
            }
            catch (IOException)
            {
                ErrorMessage("Перезапись файла невозможна.");
            }
            catch (Exception exception)
            {
                ErrorMessage("Непредвиденная ошибка: " + exception.Message);
            }
        }

        /// <summary>
        ///     Метод получения строк файла.
        /// </summary>
        /// <param name="encoding">Кодировка файла.</param>
        /// <returns>Считанные из предложенного пользователем файла строки.</returns>
        public static List<string> GetFileLines(Encoding encoding)
        {
            var path = GetFilePath("Введи путь до файла, строки которого хочешь прочитать:");


            string[] fileLines = null;
            try
            {
                fileLines = File.ReadAllLines(path, encoding);
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage("Доступ к данному файлу запрещен.");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Непредвиденная ошибка: " + exception.Message);
            }

            return fileLines?.ToList();
        }

        /// <summary>
        ///     Метод получения списка файлов, соответсвующих заданному шаблону, по умолчанию - шаблон пуст.
        /// </summary>
        /// <param name="path">Путь до директории.</param>
        /// <param name="pattern">Шаблон, которому должны соответствовать файлы.</param>
        public static void GetListOfFiles(string path, string pattern = "")
        {
            string[] fileList;
            try
            {
                fileList = Directory.GetFiles(path, pattern);
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage("Доступ к данной папке запрещен. " + path);
                return;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Непредвиденная ошибка: " + exception.Message);
                return;
            }

            Console.WriteLine("Список файлов в директории {0}:", path);
            foreach (var file in fileList)
                Console.WriteLine(file);
        }

        /// <summary>
        ///     Метод получения списка  дисков компьютера.
        /// </summary>
        /// <returns>Выбранным пользователем для перехода диск.</returns>
        public static string GetDrives()
        {
            var allDrives = DriveInfo.GetDrives();

            for (var i = 0; i < allDrives.Length; i++)
            {
                Console.WriteLine("{0}. Диск {1}", i + 1, allDrives[i].Name);
                if (allDrives[i].IsReady)
                    Console.WriteLine("Название диска: {0}", allDrives[i].VolumeLabel);
            }

            var selectedDrive = GetUserInput("Введите номер диска, в который хотите перейти", 1, allDrives.Length);
            return allDrives[selectedDrive - 1].Name;
        }

        /// <summary>
        ///     Метод получения списка директорий.
        /// </summary>
        public static string[] GetAllDirectories(string path)
        {
            string[] allDirectories = null;
            try
            {
                allDirectories = Directory.GetDirectories(path);
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage("Доступ к данной папке запрещен. " + path);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Непредвиденная ошибка: " + exception.Message);
            }

            return allDirectories;
        }

        /// <summary>
        ///     Метод изменения текущей директории.
        /// </summary>
        /// <param name="path">Путь до получаемой директории.</param>
        public static void ChangeDirectory(string path)
        {
            try
            {
                Directory.SetCurrentDirectory(path);
            }
            catch (DirectoryNotFoundException)
            {
                ErrorMessage("Данная папка не существует.");
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage("Доступ к данной папке запрещен.");
            }
            catch (Exception exception)
            {
                ErrorMessage("Непредвиденная ошибка: " + exception);
            }
        }

        /// <summary>
        ///     Основной метод программы.
        /// </summary>
        private static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Greetings();
            do
            {
                Console.Clear();
                Menu();
                GetUsersChoice();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Нажми esc чтобы закончить или любую другую клавишу чтобы продолжить:");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}