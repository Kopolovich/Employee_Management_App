using BlApi;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlTest;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    

    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans =  Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
        {
            s_bl.Reset();
            DalTest.Initialization.Do();
        }
            

        mainMenu();
    }

    static void mainMenu()
    {
        Console.WriteLine("""
                        Choose entity:
                        Enter 0 to exit
                        Enter 1 for Task 
                        Enter 2 for Engineer
                        Enter 3 to plan project schedule (after all wanted tasks were already entered)
                        Enter 4 to see project status
                        """);
        
        int choice = int.Parse(Console.ReadLine());
        while (choice != 0)
        {
            switch (choice)
            {
                case 0:
                    break;
                case 1:
                    { subMenuTask(); break; }
                case 2:
                    { subMenuEngineer(); break; }
                case 3: 
                    {
                        try
                        {
                            Console.WriteLine("Please enter project scheduled start date");
                            DateTime date;
                            if (!DateTime.TryParse(Console.ReadLine(), out date))
                                throw new BO.BlInvalidValueException("Scheduled project start date has to be in date time format and can not be null");
                            s_bl.CreateProjectSchedule(date);
                            Console.WriteLine("dates have been assigned to all tasks!");
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(
                            $"""
                            Name of exception: {ex.GetType().Name}
                            {ex.Message}
                            """);
                            if (ex.InnerException != null)
                            {
                                Console.WriteLine($"Inner exception: {ex.InnerException.GetType().Name}, {ex.InnerException.Message}");
                            }
                        }
                        break;
                    }
                    case 4: Console.WriteLine(s_bl.GetProjectStatus()); break;
                default:
                    Console.WriteLine("please enter number between 0 and 2");
                    break;
            }
            Console.WriteLine("""
                        Choose entity:
                        Enter 0 to exit
                        Enter 1 for Task 
                        Enter 2 for Engineer
                        Enter 3 to plan project schedule (after all wanted tasks were already entered)
                        Enter 4 to see project status
                        """);
            choice = int.Parse(Console.ReadLine());

        }

    }

    static void subMenuTask()
    {
        Console.WriteLine("""   
                        Choose method:
                        Enter 0 to exit
                        Enter 1 for create 
                        Enter 2 for read
                        Enter 3 for read all
                        Enter 4 for update
                        Enter 5 for delete
                        Enter 6 to assign scheduled start date to task
                        """);
        int choice = int.Parse(Console.ReadLine());
        while (choice != 0)
        {
            try
            {
                switch (choice)
                {
                    case 0:
                        break;
                    case 1:
                        {

                            BO.Task newTask = createNewTask(); //creating new task with info from user
                            s_bl.Task.Create(newTask); //adding task to list
                            break;

                        }
                    case 2:
                        {
                            Console.WriteLine("enter id of task:");
                            int id;
                            if (!int.TryParse(Console.ReadLine(), out id))
                                throw new BO.BlInvalidValueException("Id has to contain numbers only");
                            Console.WriteLine(s_bl.Task.Read(id)); //Reading and printing
                            break;
                        }
                    case 3:
                        {
                            List<BO.TaskInList> tasks = s_bl.Task.ReadAll().ToList(); //reading list
                            foreach (BO.TaskInList task in tasks)
                                Console.WriteLine(task); //printing each task                            
                            break;
                        }

                    case 4:
                        {
                            Console.WriteLine("enter id of task to update:");
                            int id;
                            if (!int.TryParse(Console.ReadLine(), out id))
                                throw new BO.BlInvalidValueException("Id has to contain numbers only");
                            BO.Task currentTask = s_bl.Task.Read(id);

                            Console.WriteLine(currentTask); //printing current task
                            BO.Task updatedTask = createUpdatedTask(currentTask);//creating new task with updated info
                            s_bl.Task.Update(updatedTask);//updating
                            break;
                        }

                    case 5:
                        {
                            Console.WriteLine("enter id of task to delete:");
                            int id;
                            if (!int.TryParse(Console.ReadLine(), out id))
                                throw new BO.BlInvalidValueException("Id has to contain numbers only");
                            s_bl.Task.Delete(id); //delete
                            break;
                        }
                    case 6:
                        {
                            Console.WriteLine("Enter id of task to assign date to");
                            int id;
                            if (!int.TryParse(Console.ReadLine(), out id))
                                throw new BO.BlInvalidValueException("Id has to contain numbers only");

                            Console.WriteLine("Enter scheduled start date");
                            DateTime date;
                            if (!DateTime.TryParse(Console.ReadLine(), out date))
                                throw new BO.BlInvalidValueException("The date you entered is not in DateTime format");

                            s_bl.Task.AssignScheduledDateToTask(id, date);

                            break;
                        }
                    default:
                        Console.WriteLine("please enter number between 0 and 6");
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(
                    $"""
                    Name of exception: {ex.GetType().Name}
                    {ex.Message}
                    """);
                if(ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.GetType().Name}, {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("""   
                        Choose method:
                        Enter 0 to exit
                        Enter 1 for create 
                        Enter 2 for read
                        Enter 3 for read all
                        Enter 4 for update
                        Enter 5 for delete
                        Enter 6 to assign scheduled start date to task
                        """);
            choice = int.Parse(Console.ReadLine());

        }
    }

    static void subMenuEngineer()
    {
        Console.WriteLine("""   
                        Choose method:
                        Enter 0 to exit
                        Enter 1 for create 
                        Enter 2 for read
                        Enter 3 for read all
                        Enter 4 for update
                        Enter 5 for delete
                        """);
        int choice = int.Parse(Console.ReadLine());
        while (choice != 0)
        {
            try
            {
                switch (choice)
                {
                    case 0:
                        break;
                    case 1:
                        {
                            BO.Engineer newEngineer = createNewEngineer(); //creating new engineer with info from user
                            s_bl.Engineer.Create(newEngineer); //adding to list
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("enter id of Engineer:");
                            int id;
                            if (!int.TryParse(Console.ReadLine(), out id))
                                throw new BO.BlInvalidValueException("Id has to contain numbers only");
                            Console.WriteLine($"{s_bl.Engineer.Read(id)}"); //reading and printing
                            break;
                        }
                    case 3:
                        {
                            List<BO.Engineer> engineers = s_bl.Engineer.ReadAll().ToList(); //reading list
                            foreach (BO.Engineer engineer in engineers)
                                Console.WriteLine(engineer); //print each engineer
                            break;
                        }

                    case 4:
                        {
                            Console.WriteLine("enter id of engineer to update:");
                            int id;
                            if (!int.TryParse(Console.ReadLine(), out id))
                                throw new BO.BlInvalidValueException("Id has to contain numbers only");
                            BO.Engineer currentEngineer = s_bl.Engineer.Read(id);
                            Console.WriteLine(currentEngineer); //printing current engineer                                
                            BO.Engineer updatedEngineer = createUpdatedEngineer(currentEngineer);
                            s_bl.Engineer.Update(updatedEngineer); //updating
                            break;
                        }

                    case 5:
                        {
                            Console.WriteLine("enter id of engineer to delete:");
                            int id;
                            if (!int.TryParse(Console.ReadLine(), out id))
                                throw new BO.BlInvalidValueException("Id has to contain numbers only");
                            s_bl.Engineer.Delete(id); //delete
                            break;
                        }
                    default:
                        Console.WriteLine("please enter number between 0 and 5");
                        break;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(
                    $"""
                    Name of exception: {ex.GetType().Name}
                    {ex.Message}
                    """);
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.GetType().Name}, {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("""   
                        Choose method:
                        Enter 0 to exit
                        Enter 1 for create 
                        Enter 2 for read
                        Enter 3 for read all
                        Enter 4 for update
                        Enter 5 for delete
                        """);
            choice = int.Parse(Console.ReadLine());

        }
    }

    /// <summary>
    /// Help method for creating new task
    /// </summary>
    /// <returns> new logic task entity </returns>
    /// <exception cref="BO.BlInvalidValueException"> for invalid inputs </exception>
    /// <exception cref="FormatException"> for wrong inputs </exception>
    static BO.Task createNewTask()
    {
        //assigning new level and check it with try parse method
        Console.WriteLine("enter number between 0 to 4 for level of complexity (Beginner, AdvancedBeginner, Intermediate, Advanced, Expert)");
        int levelInt;
        int.TryParse(Console.ReadLine(), out levelInt);
        BO.EngineerExperience level = (BO.EngineerExperience)levelInt;

        //assigning new alias
        Console.WriteLine("enter alias:");
        string? alias = Console.ReadLine();

        //assigning new description
        Console.WriteLine("enter description:");
        string? description = Console.ReadLine();

        //assigning new Required Effort Time and check it with try parse method
        Console.WriteLine("Enter Required Effort Time:");
        TimeSpan requiredEffortTime;
        if (!TimeSpan.TryParse(Console.ReadLine(), out requiredEffortTime))
            throw new BO.BlInvalidValueException("Required effort time has to be in time span format");

        //assigning new deliverables
        Console.WriteLine("enter deliverables:");
        string? deliverables = Console.ReadLine();

        //assigning new remarks
        Console.WriteLine("enter remarks: (optional)");
        string? remarks = Console.ReadLine();

        //assigning new task dependencies
        Console.WriteLine("Does this task depend on previos tasks?");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        List<BO.TaskInList>? dependencies = null;
        while (ans == "Y")
        {
            Console.WriteLine("Enter id of previous task:");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
                throw new BO.BlInvalidValueException("Id has to contain numbers only");

            //reads requested task props
            BO.Task depTask = s_bl.Task.Read(id);
            //creating new list of dependencies
            if(dependencies == null)
                dependencies = new List<BO.TaskInList>();
            dependencies.Add(new BO.TaskInList()
            {
                Id = id,
                Description = depTask.Description,
                Alias = depTask.Alias,
                Status = depTask.Status
            });
            Console.WriteLine("Does this task depend on more previos tasks?");
            ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        }
            
           return new BO.Task()
        {
            Id = 0,
            Description = description,
            Alias = alias,
            CreatedAtDate = DateTime.Now,
            Status = BO.Status.Unscheduled,
            Dependencies = dependencies,
            RequiredEffortTime = requiredEffortTime,
            StartDate = null,
            ScheduledDate = null,
            ForecastDate = null,
            CompleteDate = null,
            Deliverables = deliverables,
            Remarks = remarks,
            Engineer = null,
            Complexity = level
        };
    }

    /// <summary>
    /// Help method for updating requested task
    /// </summary>
    /// <param name="oldTask"> un-updated task </param>
    /// <returns> task with updates </returns>
    /// <exception cref="FormatException"> for wrong input </exception>
    /// <exception cref="BO.BlInvalidValueException"> for invalid values </exception>
    static BO.Task createUpdatedTask(BO.Task oldTask)
    {

        Console.WriteLine("enter new values only for the fields you want to update, the rest will stay without change, \n if you enter info in worng format those fields will not be updated (those fields will stay the same as before)");
        //assigning level and check it with try parse method
        Console.WriteLine("enter number between 0 to 4 for level of complexity (Beginner, AdvancedBeginner, Intermediate, Advanced, Expert)");
        int levelInt;
        bool success = int.TryParse(Console.ReadLine(), out levelInt);
        BO.EngineerExperience level = success ? (BO.EngineerExperience)levelInt : oldTask.Complexity;

        //assigning alias
        Console.WriteLine("enter alias:");
        string? alias = Console.ReadLine();
        if (alias == "") alias = oldTask.Alias;

        //assigning description
        Console.WriteLine("enter description:");
        string? description = Console.ReadLine();
        if (description == "") description = oldTask.Description;

        //assigning Required Effort Time and check it with try parse method
        Console.WriteLine("Enter Required Effort Time:");
        TimeSpan? requiredEffortTime;
        TimeSpan time;
        requiredEffortTime = TimeSpan.TryParse(Console.ReadLine(), out time) ? time : oldTask.RequiredEffortTime;

        //assigning start date and check it with try parse method
        Console.WriteLine("enter start date:");
        DateTime? startDate;
        DateTime date;
        startDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : oldTask.StartDate;

        //assigning scheduled date and check it with try parse method
        Console.WriteLine("enter scheduled date:");
        DateTime? scheduledDate;
        scheduledDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : oldTask.ScheduledDate;

        //assigning complete date and check it with try parse method
        Console.WriteLine("enter complete date");
        DateTime? completeDate;
        completeDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : oldTask.CompleteDate;

        //assigning deliverables
        Console.WriteLine("enter deliverables: (optional)");
        string? deliverables = Console.ReadLine();
        if (deliverables == "") deliverables = oldTask.Deliverables;

        //assigning remarks
        Console.WriteLine("enter remarks: (optional)");
        string? remarks = Console.ReadLine();
        if (remarks == "") remarks = oldTask.Remarks;

        //assigning engineer id that working on task and check it with try parse method
        Console.WriteLine("enter id of engineer working on task:");
        int? engineerId;
        int engId;
        engineerId = int.TryParse(Console.ReadLine(), out engId) ? engId : (oldTask.Engineer != null ? oldTask.Engineer.Id : null);

        //assigning dependencies
        Console.WriteLine("Does this task depend on previos tasks?");
        string? ans = Console.ReadLine(); //?? throw new FormatException("Wrong input");
        List<BO.TaskInList>? dependencies = null;
        while (ans == "Y")
        {
            Console.WriteLine("Enter id of previous task:");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
                throw new BO.BlInvalidValueException("Id has to contain numbers only");
            BO.Task depTask = s_bl.Task.Read(id);
            if (dependencies == null)
                dependencies = new List<BO.TaskInList>();
            dependencies.Add(new BO.TaskInList()
            {
                Id = id,
                Description = depTask.Description,
                Alias = depTask.Alias,
                Status = depTask.Status
            });
            Console.WriteLine("Does this task depend on more previos tasks?");
            ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        }


        return new BO.Task() 
        {
            Id = oldTask.Id,
            Description = description,
            Alias = alias,
            CreatedAtDate = DateTime.Now,
            Status = oldTask.Status,
            Dependencies = dependencies,
            RequiredEffortTime = requiredEffortTime,
            StartDate = startDate,
            ScheduledDate = scheduledDate,
            ForecastDate = oldTask.ForecastDate,
            CompleteDate = completeDate,
            Deliverables = deliverables,
            Remarks = remarks,
            Engineer = engineerId == null ? null : new BO.EngineerInTask() { Id = (int)engineerId , Name = s_bl.Engineer.Read((int)engineerId).Name},
            Complexity = level
        };
       
    }

    /// <summary>
    /// Help methos for creating new engineer
    /// </summary>
    /// <returns> new engineer logic entity </returns>
    /// <exception cref="BO.BlInvalidValueException"> for invalid inputs </exception>
    static BO.Engineer createNewEngineer()
    {
        //assigning new id and check it with try parse method
        Console.WriteLine("enter id of engineer:");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
            throw new BO.BlInvalidValueException("Id has to contain numbers only");

        //assigning new leval and check it with try parse method
        Console.WriteLine("enter level of engineer:");
        int levelInt;
        int.TryParse(Console.ReadLine(), out levelInt);
        BO.EngineerExperience level = (BO.EngineerExperience)levelInt;

        //assigning new email
        Console.WriteLine("enter an email:");
        string? email = Console.ReadLine();

        //assigning new cost and check it with try parse method
        Console.WriteLine("enter hourly salary:");
        double cost;
        if (!double.TryParse(Console.ReadLine(), out cost))
            throw new BO.BlInvalidValueException("cost has to contain numbers only");

        //assigning new name
        Console.WriteLine("enter a name:");
        string? name = Console.ReadLine();

        //assigning new current tasks in engineer
        BO.TaskInEnginner? currentTask = null;
        Console.WriteLine("Do you want to assign task to engineer?");
        string? ans = Console.ReadLine();
        if (ans != null && ans == "Y")
        {
            Console.WriteLine("enter task id:");
            int taskId;
            if (!int.TryParse(Console.ReadLine(), out taskId))
                throw new BO.BlInvalidValueException("Id has to contain numbers only");
            currentTask = new BO.TaskInEnginner()
            {
                Id = taskId,
                Alias = s_bl.Task.Read(taskId).Alias
            };
        }

        return new BO.Engineer()
        {
            Id = id,
            Name = name,
            Email = email,
            Level = level,
            Cost = cost,
            Task = currentTask
        };    
    }

    /// <summary>
    /// Help method for updating engineer
    /// </summary>
    /// <param name="currentEngineer"> unupdated engineer </param>
    /// <returns> engineer with updates </returns>
    /// <exception cref="BO.BlInvalidValueException"> for invalid inputs </exception>
    static BO.Engineer createUpdatedEngineer(BO.Engineer currentEngineer)
    {
        Console.WriteLine("enter new values only for the fields you want to update, the rest will stay without change");
        //assigning level and check it with try parse method
        Console.WriteLine("enter number between 0 to 4 for level of complexity (Beginner, AdvancedBeginner, Intermediate, Advanced, Expert)");
        int levelInt;
        bool success = int.TryParse(Console.ReadLine(), out levelInt);
        BO.EngineerExperience level = success ? (BO.EngineerExperience)levelInt : currentEngineer.Level;

        //assigning email
        Console.WriteLine("enter an email:");
        string? email = Console.ReadLine();
        if (email == "") email = currentEngineer.Email;

        //assigning cost and check it with try parse method
        Console.WriteLine("enter hourly salary:");
        double? cost;
        double _cost;
        cost = double.TryParse(Console.ReadLine(), out _cost) ? _cost : currentEngineer.Cost;

        //assigning name
        Console.WriteLine("enter a name:");
        string? name = Console.ReadLine();
        if (name == "") name = currentEngineer.Name;

        //assigning current tasks in engineer
        Console.WriteLine("Do you want to update current task?");
        string? ans = Console.ReadLine();
        BO.TaskInEnginner? currentTask = null;
        if (ans != null && ans == "Y")
        {
            Console.WriteLine("enter task id:");
            int taskId;
            if (!int.TryParse(Console.ReadLine(), out taskId))
                throw new BO.BlInvalidValueException("Id has to contain numbers only");
            currentTask = new BO.TaskInEnginner()
            {
                Id = taskId,
                Alias = s_bl.Task.Read(taskId).Alias
            };
        }


        return new BO.Engineer()
        {
            Id = currentEngineer.Id,
            Name = name,
            Email = email,
            Level = level,
            Cost = cost,
            Task = currentTask
        }; //creating new engineer with updated details
    }
}

 




