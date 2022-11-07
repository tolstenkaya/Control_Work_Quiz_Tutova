List<User> players = new List<User>();
User P = new User();
P.createDir();
ConsoleKeyInfo key;
int n = 0;
bool b = false;
do
{
    Console.Clear();
    Console.WriteLine("Welcome to the Quiz.");
    Console.WriteLine("1 - Log in.");
    Console.WriteLine("2 - Log up.");
    Console.Write("Your choice: ");
    key = Console.ReadKey();
    try
    {
        if (key.Key == ConsoleKey.NumPad1 || key.Key == ConsoleKey.D1)
        {
            n = 1;
        }
        if (key.Key == ConsoleKey.NumPad2 || key.Key == ConsoleKey.D2)
        {
            n = 2;
        }
        if (key.Key == ConsoleKey.Escape)
        {
            n = 0;
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    switch (n)
    {
        case 1:
            {
                Console.Clear();
                P.signIn(players, ref b);
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                break;
            }
        case 2:
            {
                Console.Clear();
                P = new User();
                P.signUp(players, ref b);
                players.Add(P);
                P.saveUser(P);
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                break;
            }
    }

} while (b != true);
Console.Clear();
List<Play> quiz = new List<Play>();
Play Pl = new Play();
Pl.createDir();
if (b == true)
{
    do
    {
        Console.Clear();
        Console.WriteLine("1 - Start a new quiz.");
        Console.WriteLine("2 - View past quiz results.");
        Console.WriteLine("3 - View Top-5 for a specific quiz.");
        Console.WriteLine("4 - Change settings.");
        Console.WriteLine("Esc. - Exit.");
        Console.Write("Your choice: ");
        key = Console.ReadKey();
        try
        {
            if (key.Key == ConsoleKey.NumPad1 || key.Key == ConsoleKey.D1)
            {
                n = 1;
            }
            if (key.Key == ConsoleKey.NumPad2 || key.Key == ConsoleKey.D2)
            {
                n = 2;
            }
            if (key.Key == ConsoleKey.NumPad3 || key.Key == ConsoleKey.D3)
            {
                n = 3;
            }
            if (key.Key == ConsoleKey.NumPad4 || key.Key == ConsoleKey.D4)
            {
                n = 4;
            }
            if (key.Key == ConsoleKey.Escape)
            {
                n = 0;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        switch (n)
        {
            case 1:
                {
                    Console.Clear();
                    Pl.startNewQuiz(P);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                }
            case 2:
                {
                    Console.Clear();
                    Pl.seeLastQuiz(P);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                }
            case 3:
                {
                    Console.Clear();
                    Pl.top5InQuiz();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                }
            case 4:
                {
                    Console.Clear();
                    Pl.changeSetting(P);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                }
        }
    } while (key.Key != ConsoleKey.Escape);
}
class User
{
    public string[] read_usersLogin = new string[0];
    public string[] read_usersAllInformation = new string[0];
    static int count_user = 0;
    string login;
    string password;
    DateTime date_birthday;
    int number_correct_answeers = 0;
    public int NumberCorrectAnswers
    {
        get { return number_correct_answeers; }
        set { number_correct_answeers = value; }
    }
    public string Login
    {
        get { return login; }
        set { login = value; }
    }
    public string Password
    {
        get { return password; }
        set { password = value; }
    }
    public DateTime DateBirthday
    {
        get { return date_birthday; }
        set { date_birthday = value; }
    }
    public User()
    {
        Login = "";
        Password = "";
        date_birthday = new DateTime();
    }
    public User(string login, string password)
    {
        Login = login;
        Password = password;
    }
    public User(string login, string password, DateTime date_birthday) : this(login, password)
    {
        this.date_birthday = date_birthday;
    }
    public void signIn(List<User> users, ref bool b)
    {
        int f = 0;
        do
        {
            Console.Write("Enter login: ");
            Login = Console.ReadLine();
            if (checkExistLogin(Login))
            {
                Console.Write("Enter password: ");
                Password = Console.ReadLine();
                f = 1;
                if (!checkCorrectEnteredData(Login, Password))
                {
                    Console.Clear();
                    Console.WriteLine("Wrong entered login or password. Try again: ");
                    f = 0;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You are not registered in the system!\nGo to \"Register\".");
                break;
            }
        } while (!checkCorrectEnteredData(Login, Password));
        if (f == 1)
        {
            Console.Clear();
            User item = new User(Login, Password);
            users.Add(item);
            Console.WriteLine("You are welcome to the quiz!");
            b = true;
        }
    }
    public void signUp(List<User> users, ref bool b)
    {
        do
        {
            Console.Write("Create login: ");
            login = Console.ReadLine();
            if (checkExistLogin(login)) { Console.WriteLine("The entered login already exist! Create a new one."); }
        } while (checkExistLogin(login));
        Console.Write("Create password: ");
        password = Console.ReadLine();
        int t = 0;
        do
        {
            Console.Write("Enter your date of birth (in format d.m.y): ");
            string dateStr = Console.ReadLine();
            if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null,
                                       System.Globalization.DateTimeStyles.AllowWhiteSpaces |
                                       System.Globalization.DateTimeStyles.AdjustToUniversal,
                                       out date_birthday))
            {
                t = 1;
            }
            else
            {
                Console.WriteLine($"Incorrect date entered: {dateStr}");
                t = 0;
            }
            if (t == 1 && date_birthday > DateTime.Now) { Console.WriteLine($"You cannot be born in the future: {dateStr}. Please, change entered date!"); t = 0; }
        } while (t != 1);
        b = true;
        count_user++;
        User item = new User(login, password, date_birthday);
        users.Add(item);
        Console.Clear();
        Console.WriteLine("New user was added!");
    }
    public bool checkExistLogin(string log)
    {
        readUserLogin();
        foreach (var key in read_usersLogin)
        {
            if (key == log)
            {
                return true;
            }
        }
        return false;
    }
    public bool checkCorrectEnteredData(string log, string pas)
    {
        readUserLogin();
        for (int i = 0; i < read_usersAllInformation.Length; i++)
        {
            if (read_usersAllInformation[i] == log && read_usersAllInformation[i + 1] == pas)
            {
                return true;
            }
        }
        return false;
    }
    public void createDir()
    {
        string path = @"C:\Quiz";
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            dir.Create();
        }
    }
    public async void saveUser(User users)
    {
        string path = @"C:\Quiz\Registered users.txt";
        using (StreamWriter writer1 = new StreamWriter(path, true))
        {
            await writer1.WriteLineAsync(users.Login);
            await writer1.WriteLineAsync(users.Password);
            await writer1.WriteLineAsync(users.DateBirthday.ToShortDateString());
        }
    }
    public async void readUserLogin()
    {
        string path = @"C:\Quiz\Registered users.txt";
        int size = 0;
        string line = "";
        int count_line = 0;
        using (StreamReader sr = new StreamReader(path))
        {
            while ((line = sr.ReadLine()) != null)
            {
                count_line++;
                if (count_line % 3 == 1)
                {
                    size++;
                    Array.Resize(ref read_usersLogin, size);
                    read_usersLogin[size - 1] = line;
                }
                Array.Resize(ref read_usersAllInformation, count_line);
                read_usersAllInformation[count_line - 1] = line;
            }
        }
    }
    public async void readUsers(List<User> users)
    {
        string path = @"C:\Quiz\Registered users.txt";
        int size = 0;
        string line = "";
        int count_line = 0;
        using (StreamReader sr = new StreamReader(path))
        {
            while ((line = sr.ReadLine()) != null)
            {
                count_line++;
                if (count_line % 3 == 1)
                {
                    size++;
                    Array.Resize(ref read_usersLogin, size);
                    read_usersLogin[size - 1] = line;
                }
                Array.Resize(ref read_usersAllInformation, count_line);
                read_usersAllInformation[count_line - 1] = line;
            }
        }
    }
}
class Play
{
    string[] category;
    string[] questions;
    string[] right_answers;
    public void getAllFiles()
    {
        string path = @"C:\Quiz\Category";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] all_quiz = dir.GetFiles();
        category = new string[all_quiz.Length];
        for (int i = 0; i < category.Length; i++)
        {
            category[i] = all_quiz[i].Name.ToString();
            for (int j = 0; j < category[i].Length; j++)
            {
                if (category[i][j] == '.')
                {
                    category[i] = category[i].Remove(j);
                }
            }
        }
        int count = 0;
        foreach (var key in category)
        {
            count++;
            Console.WriteLine(count + ". " + key);
        }
    }
    public void readFile(string ct)
    {
        string path = @"C:\Quiz\Category\" + ct + ".txt";
        string line = "";
        int count_line = 0;
        int quest = 0;
        using (StreamReader sr = new StreamReader(path))
        {
            while ((line = sr.ReadLine()) != null)
            {
                count_line++;
                if (count_line % 2 != 0)
                {
                    quest++;
                    Array.Resize(ref questions, quest);
                    questions[quest - 1] = line;
                }
                else
                {
                    Array.Resize(ref right_answers, quest);
                    right_answers[quest - 1] = line;
                }
            }
        }
    }
    public void createDir()
    {
        string path = @"C:\Quiz\Statistics";
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            dir.Create();
        }
    }
    public async void writeFile(User user, string category)
    {
        string path = @"C:\Quiz\Statistics\" + user.Login + ".txt";

        using (StreamWriter writer1 = new StreamWriter(path, true))
        {
            await writer1.WriteLineAsync(category);
            await writer1.WriteLineAsync(user.NumberCorrectAnswers.ToString());
            writer1.Close();
        }

    }
    public async void seeLastQuiz(User user)
    {
        string path = @"C:\Quiz\Statistics\" + user.Login + ".txt";
        string line = "";
        int count_line = 0;
        using (StreamReader sr = new StreamReader(path))
        {
            while ((line = sr.ReadLine()) != null)
            {
                count_line++;
                if (count_line % 2 != 0)
                {
                    Console.Write($"\n{line} -- ");
                }
                else
                {
                    Console.Write($"{line}");
                }
            }
        }
    }
    public void startNewQuiz(User user)
    {
        Console.WriteLine("All quiz categories.\n");
        getAllFiles();
        Console.Write("\nChoose the category: ");
        string str = Console.ReadLine();
        int num_category = 0;
        string play_category = "";
        try
        {
            num_category = Convert.ToInt32(str);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        for (int i = 0; i < category.Length; i++)
        {
            if (i == num_category - 1)
            {
                play_category = category[i];
            }
        }
        readFile(play_category);
        string user_answer = "";
        for (int i = 0; i < questions.Length; i++)
        {
            Console.WriteLine(i + 1 + ". " + questions[i]);
            Console.Write("Your answer: ");
            user_answer = Console.ReadLine();
            if (user_answer == right_answers[i])
            {
                user.NumberCorrectAnswers++;
            }
        }
        writeFile(user, play_category);
    }
    public void top5InQuiz()
    {
        getAllFiles();
        Console.Write("\nChoose the category: ");
        string str = Console.ReadLine();
        int num_category = 0;
        string play_category = "";
        try
        {
            num_category = Convert.ToInt32(str);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        for (int i = 0; i < category.Length; i++)
        {
            if (i == num_category - 1)
            {
                play_category = category[i];
            }
        }
        string path = @"C:\Quiz\Category\" + play_category + ".txt";
        string line = "";
        int count_line = 0;
        int count_q = 0;
        using (StreamReader sr = new StreamReader(path))
        {
            while ((line = sr.ReadLine()) != null)
            {
                count_line++;
                if (count_line % 2 == 1 && count_q < 6)
                {
                    count_q++;
                    Console.WriteLine(line);
                }

            }
        }
    }
    public async void changeSetting(User user)
    {
        string path = @"C:\Quiz\Registered users.txt";
        int count_line = 0;
        string line = "";
        int count = 0;
        string[] login = new string[count];
        string[] password = new string[count];
        string[] dataBirthday = new string[count];
        using (StreamReader sr = new StreamReader(path))
        {
            while ((line = sr.ReadLine()) != null)
            {
                count_line++;
                if (count_line % 3 == 1)
                {
                    count++;
                    Array.Resize(ref login, count);
                    login[count - 1] = line;
                }
                if (count_line % 3 == 2)
                {
                    Array.Resize(ref password, count);
                    password[count - 1] = line;
                }
                if (count_line % 3 == 0)
                {
                    Array.Resize(ref dataBirthday, count);
                    dataBirthday[count - 1] = line;
                }
            }
        }
        Console.Write("Enter new password: ");
        string change_password = Console.ReadLine();
        DateTime change_dateBirthday;
        string dateStr = "";
        int t = 0;
        do
        {
            Console.Write("Enter new date of birth (in format d.m.y): ");
            dateStr = Console.ReadLine();
            if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null,
                                       System.Globalization.DateTimeStyles.AllowWhiteSpaces |
                                       System.Globalization.DateTimeStyles.AdjustToUniversal,
                                       out change_dateBirthday))
            {
                t = 1;
            }
            else
            {
                Console.WriteLine($"Incorrect date entered: {dateStr}");
                t = 0;
            }
            if (t == 1 && change_dateBirthday > DateTime.Now) { Console.WriteLine($"You cannot be born in the future: {dateStr}. Please, change entered date!"); t = 0; }
        } while (t != 1);
        Console.WriteLine(count);
        for (int i = 0; i < count; i++)
        {
            if (login[i] == user.Login)
            {
                password[i] = change_password;
                dataBirthday[i] = dateStr;
            }
        }
        using (StreamWriter writer1 = new StreamWriter(path, false))
        {
            for (int i = 0; i < count; i++)
            {
                await writer1.WriteLineAsync(login[i]);
                await writer1.WriteLineAsync(password[i]);
                await writer1.WriteLineAsync(dataBirthday[i]);
            }

        }
    }
}