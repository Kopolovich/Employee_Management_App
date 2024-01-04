using Dal;
using DalApi;
using DO;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
namespace DalTest
{
   
    public class Program
    {
        private static ITask? s_dalTask = new TaskImplementation(); 
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); 
        private static IDependency? s_dalDependency = new DependencyImplementation(); 

        
        private static void Main(string[] args)
        {
            try
            {                          
                Initialization.Do(s_dalDependency,s_dalTask, s_dalEngineer);
                void mainMenu()
                {
                    Console.WriteLine("""
                        Choose entity:
                        Enter 0 to exit
                        Enter 1 for Task 
                        Enter 2 for Engineer
                        Enter 3 for Dependency
                        """);
                    int choice = int.Parse(Console.ReadLine());
                    while(choice != 0)
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
                                { subMenuDependency(); break; }
                            default:
                                Console.WriteLine("please enter number between 0 and 3");
                                break;
                        }
                        Console.WriteLine("""
                        Choose entity:
                        Enter 0 to exit
                        Enter 1 for Task 
                        Enter 2 for Engineer
                        Enter 3 for Dependency
                        """);
                        choice = int.Parse(Console.ReadLine());

                    }

                }

                mainMenu();

                void subMenuTask()
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
                        switch (choice)
                        {
                            case 0:
                                break;
                            case 1:
                                {
                                    DO.Task newTask = createNewTask(); //creating new task with info from user
                                    s_dalTask.Create(newTask); //adding task to list
                                    break;
                                }
                            case 2:
                                {
                                    Console.WriteLine("enter id of task:");
                                    int id = int.Parse(Console.ReadLine());
                                    Console.WriteLine($"{s_dalTask.Read(id)}"); //Reading and printing
                                    break;
                                } 
                            case 3:
                                {
                                    List<DO.Task> tasks = s_dalTask.ReadAll(); //reading list
                                    foreach (DO.Task task in tasks) 
                                        Console.WriteLine(task); //printing each task                            
                                    break;
                                }
                            
                            case 4:
                                {
                                    Console.WriteLine("enter id of task to update:");
                                    int id = int.Parse(Console.ReadLine());
                                    Console.WriteLine($"{s_dalTask.Read(id)}"); //printing current task
                                    DO.Task updatedTask = createNewTask(id);//creating new task with updated info
                                    s_dalTask.Update(updatedTask);//updating
                                    break;
                                }

                            case 5:
                                {
                                    Console.WriteLine("enter id of task to delete:");
                                    int id = int.Parse(Console.ReadLine());
                                    s_dalTask.Delete(id); //delete
                                    break;
                                }
                            default:
                                Console.WriteLine("please enter number between 0 and 5");
                                break;
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
                void subMenuEngineer()
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
                        switch (choice)
                        {
                            case 0:
                                break;
                            case 1:
                                {
                                    Engineer newEngineer = createNewEngineer(); //creating new engineer with info from user
                                    s_dalEngineer.Create(newEngineer); //adding to list
                                    break;
                                }
                            case 2:
                                {
                                    Console.WriteLine("enter id of Engineer:");
                                    int id = int.Parse(Console.ReadLine());
                                    Console.WriteLine($"{s_dalEngineer.Read(id)}"); //reading and printing
                                    break;
                                }
                            case 3:
                                {
                                    List<Engineer> engineers = s_dalEngineer.ReadAll(); //reading list
                                    foreach (Engineer engineer in engineers)
                                        Console.WriteLine(engineer); //print each engineer
                                    break;
                                }

                            case 4:
                                {
                                    Console.WriteLine("enter id of engineer to update:");
                                    int id = int.Parse(Console.ReadLine());
                                    Console.WriteLine($"{s_dalEngineer.Read(id)}"); //printing current engineer

                                    //receving updated input from user
                                    Console.WriteLine("enter number between 0 to 4 for level of engineer (Beginner, AdvancedBeginner, Intermediate, Advanced, Expert)");
                                    int levelInt = int.Parse(Console.ReadLine());
                                    EngineerExperience level = (EngineerExperience)levelInt;
                                    Console.WriteLine("enter an email:");
                                    string? email = Console.ReadLine();
                                    Console.WriteLine("enter hourly salary:");
                                    double cost = double.Parse(Console.ReadLine());
                                    Console.WriteLine("enter a name:");
                                    string? name = Console.ReadLine();

                                    Engineer engineer = new(id, level, email, cost, name); //creating new engineer with updated details
                                    s_dalEngineer.Update(engineer); //updating
                                    break;
                                }

                            case 5:
                                {
                                    Console.WriteLine("enter id of engineer to delete:");
                                    int id = int.Parse(Console.ReadLine());
                                    s_dalEngineer.Delete(id); //delete
                                    break;
                                }
                            default:
                                Console.WriteLine("please enter number between 0 and 5");
                                break;
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
                void subMenuDependency()
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
                        switch (choice)
                        {
                            case 0:
                                break;
                            case 1:
                                {
                                    Dependency newDependency = createNewDependency(); //creating new dependency
                                    s_dalDependency.Create(newDependency); //adding to list
                                    break;
                                }
                            case 2:
                                {
                                    Console.WriteLine("enter id of Dependency:");
                                    int id = int.Parse(Console.ReadLine());
                                    Console.WriteLine($"{s_dalDependency.Read(id)}"); //reading and printing
                                    break;
                                }
                            case 3:
                                {
                                    List<Dependency> dependencies = s_dalDependency.ReadAll(); //reading list
                                    foreach (Dependency dependency in dependencies)
                                        Console.WriteLine(dependency); //print each dependency
                                    break;
                                }

                            case 4:
                                {
                                    Console.WriteLine("enter id of Dependency to update:");
                                    int id = int.Parse(Console.ReadLine());
                                    Console.WriteLine($"{s_dalDependency.Read(id)}"); //printing current dependency
                                    Dependency updatedDependency = createNewDependency(id); 
                                    s_dalDependency.Update(updatedDependency); //update
                                    break;
                                }

                            case 5:
                                {
                                    Console.WriteLine("enter id of Dependency to delete:");
                                    int id = int.Parse(Console.ReadLine());
                                    s_dalDependency.Delete(id); //delete
                                    break;
                                }
                            default:
                                Console.WriteLine("please enter number between 0 and 5");
                                break;
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

                //methods to create new items with details recieves from user
                //also used to create updated items
                DO.Task createNewTask(int id = 0)
                {
                    Console.WriteLine("enter number between 0 to 4 for level of complexity (Beginner, AdvancedBeginner, Intermediate, Advanced, Expert)");
                    int levelInt = int.Parse(Console.ReadLine());
                    EngineerExperience level = (EngineerExperience)levelInt;
                    Console.WriteLine("enter alias:");
                    string? alias = Console.ReadLine();
                    Console.WriteLine("enter description:");
                    string? description = Console.ReadLine();
                    Console.WriteLine("Enter Required Effort Time:");
                    string? time = Console.ReadLine();
                    TimeSpan? requiredEffortTime = TimeSpan.Parse(time);
                    Console.WriteLine("enter 1 if task is milestone, otherwise 0");
                    int isMile = int.Parse(Console.ReadLine());
                    bool isMilestone;
                    if (isMile == 1) { isMilestone = true; }
                    else { isMilestone = false; }
                    Console.WriteLine("enter start date:");
                    string? date = Console.ReadLine();
                    DateTime? startDate = DateTime.Parse(date);
                    Console.WriteLine("enter scheduled date:");
                    date = Console.ReadLine();
                    DateTime? scheduledDate = DateTime.Parse(date);
                    Console.WriteLine("enter complete date");
                    date = Console.ReadLine();
                    DateTime? completeDate = DateTime.Parse(date);
                    Console.WriteLine("enter deliverables:");
                    string? deliverables = Console.ReadLine();
                    Console.WriteLine("enter remarks: (optional)");
                    string? remarks = Console.ReadLine();
                    Console.WriteLine("enter id of engineer working on task:");
                    int? engineerId = int.Parse(Console.ReadLine());
             
                    DO.Task? task = new(id, level, alias, description, DateTime.Now, requiredEffortTime,
                        isMilestone, startDate, scheduledDate, null, completeDate, deliverables, remarks, engineerId);
                    return task;
                }
                Engineer createNewEngineer() 
                {
                    Console.WriteLine("enter id of engineer:");
                    int id = int.Parse(Console.ReadLine());
                    Console.WriteLine("enter number between 0 to 4 for level of engineer (Beginner, AdvancedBeginner, Intermediate, Advanced, Expert)");
                    int levelInt = int.Parse(Console.ReadLine());
                    EngineerExperience level = (EngineerExperience)levelInt;
                    Console.WriteLine("enter an email:");
                    string? email = Console.ReadLine();
                    Console.WriteLine("enter hourly salary:");
                    double? cost = int.Parse(Console.ReadLine());
                    Console.WriteLine("enter a name:");
                    string? name = Console.ReadLine();

                    Engineer engineer = new(id, level, email, cost, name);
                    return engineer; 
                }
                Dependency createNewDependency(int id = 0)
                {
                    Console.WriteLine("enter id of dependent task:");
                    int DependentTaskId = int.Parse(Console.ReadLine());
                    Console.WriteLine("enter id of task that is dependent on:");
                    int dependsOnTaskId = int.Parse(Console.ReadLine());
                    Dependency dependency = new(id, DependentTaskId,dependsOnTaskId);
                    return dependency;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }


    }
}