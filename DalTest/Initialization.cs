namespace DalTest;
using Dal;
using DalApi;
using DO;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

/// <summary>
/// class to initialize lists
/// </summary>
public static class Initialization
{
    private static IDal? s_dal;
    private static readonly Random s_rand = new();
    
    /// <summary>
    /// method to create items and add to list of Engineers
    /// </summary>
   private static void createEngineers()
    {
        //array of engineer names
        string[] engineerNames = 
            {"Dani Levi", "Eli Amar", "Yair Cohen",
            "Ariela Levin", "Dina Klein"};

        foreach (var _name in engineerNames)
        {
            //creating new unique Id
            int _id;
            do
                _id = s_rand.Next(200000000, 400000000);
            while (s_dal!.Engineer.Read(_id) != null);

            //creating email address for every engineer using his name
            int findSpace = _name.IndexOf(' ');
            string firstName = _name.Substring(0, findSpace);
            string lastName = _name.Substring(findSpace + 1);
            string? _email = $"{firstName}{lastName}@gmail.com";

            //randomly assigning level of experience for each engineer
            int length = Enum.GetNames(typeof(EngineerExperience)).Length;
            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, length);

            //assigning engineer hourly salary according to level
            double? _cost = 120 + (int)_level * 5;                        

            Engineer newEngineer = new(_id, _level, _email , _cost, _name);

            s_dal!.Engineer.Create(newEngineer);
        }
    }

    /// <summary>
    /// method to create items and add to list of Tasks
    /// </summary>
    private static void createTasks()
    {
        //array of task descriptions
        string[] taskDescriptions =
        {
            "Create a project timeline with milestones",
            "Manage and mitigate project risks",
            "Coordinate teams for effective communication",
            "Allocate resources based on project requirements",
            "Optimize budget with cost-benefit analysis",
            "Maintain project documentation and progress reports",
            "Use project management tools for task tracking",
            "Conduct regular team meetings for updates",
            "Choose appropriate technologies and methodologies",
            "Control project scope to prevent scope creep",
            "Implement quality control measures",
            "Build and maintain stakeholder relationships",
            "Manage changes in project scope or requirements",
            "Provide feedback and conduct performance reviews",
            "Ensure compliance with industry standards",
            "Develop contingency plans for challenges",
            "Foster innovation and continuous improvement",
            "Coordinate user acceptance testing and feedback",
            "Manage vendor relationships and contracts",
            "Conduct post-project evaluations for improvement"
        };

        //iterating through descriptions and adding new task with current description
        foreach (var _description in taskDescriptions)
        {
            int _id = 0; //will be overriden by Create method (with automatic random id)

            //randomly assigning complexity for each task
            int length = Enum.GetNames(typeof(EngineerExperience)).Length;
            EngineerExperience _complexity = (EngineerExperience)s_rand.Next(0, length);

            //randomly assigning alias for each task, containing capital letter and 2 digit number
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int index = s_rand.Next(0, letters.Length);
            char ch = letters[index];
            string? _alias = $"{ch}{s_rand.Next(10, 99)}";

            //setting created at date to random date in the past year
            DateTime _createdAtDate = DateTime.Today.AddDays(-s_rand.Next(1, 365));


            TimeSpan? _requiredEffortTime = TimeSpan.FromDays(s_rand.Next(1, 10));
            bool? _isMilestone = false;

            Task newTask = new(_id, _complexity, _createdAtDate, _description, _alias,
                _requiredEffortTime, _isMilestone);

            s_dal!.Task.Create(newTask);
        }       

    }

    /// <summary>
    /// method to create items and add to list of Dependencies
    /// </summary>
    private static void createDependencies()
    {
        //array of task descriptions
        string[] taskDescriptions =
        {
            "Create a project timeline with milestones",
            "Manage and mitigate project risks",
            "Coordinate teams for effective communication",
            "Allocate resources based on project requirements",
            "Optimize budget with cost-benefit analysis",
            "Maintain project documentation and progress reports",
            "Use project management tools for task tracking",
            "Conduct regular team meetings for updates", 
            "Choose appropriate technologies and methodologies",
            "Control project scope to prevent scope creep",
            "Implement quality control measures", 
            "Build and maintain stakeholder relationships",
            "Manage changes in project scope or requirements",
            "Provide feedback and conduct performance reviews",
            "Ensure compliance with industry standards",
            "Develop contingency plans for challenges",
            "Foster innovation and continuous improvement",
            "Coordinate user acceptance testing and feedback",
            "Manage vendor relationships and contracts",
            "Conduct post-project evaluations for improvement"
        };

        //Creating array of task id's using task descriptions
        int[] tasksId = new int[taskDescriptions.Length];
        for (int i = 0; i < taskDescriptions.Length; i++) 
        {
            int? temp = s_dal!.Task.FindId(taskDescriptions[i]);
            if(temp!=null)
                tasksId[i] = temp.Value;
        }

        //Set the last task to be dependant on all other tasks
        for (int i = 0; i < tasksId.Length - 1; i++)
        {
            if (tasksId[i] != 0)
            {
                int _id = 0; //will be overriden by Create method (with automatic random id)
                int _dependentTask = tasksId[tasksId.Length - 1];
                int _dependsOnTask = tasksId[i];
                Dependency newDependency = new(_id, _dependentTask, _dependsOnTask);
                s_dal!.Dependency.Create(newDependency);
            }
        }

        //Setting tasks 7 and 10 to be dependent on the first 5 tasks
        for (int i = 0; i < 5; i++)
        {
            if (tasksId[i] != 0)
            {
                {

                    int _id = 0; //will be overriden by Create method (with automatic random id)
                    int _dependentTask = tasksId[7];
                    int _dependsOnTask = tasksId[i];
                    Dependency newDependency = new(_id, _dependentTask, _dependsOnTask);
                    s_dal!.Dependency.Create(newDependency);

                }

                {
                    int _id = 0; //will be overriden by Create method (with automatic random id)
                    int _dependentTask = tasksId[10];
                    int _dependsOnTask = tasksId[i];
                    Dependency newDependency = new(_id, _dependentTask, _dependsOnTask);
                    s_dal!.Dependency.Create(newDependency);

                }
            }

        }

        //Set task 17 to be dependent on tasks 7 and 10
        {
            int _id = 0; //will be overriden by Create method (with automatic random id)
            int _dependentTask = tasksId[17];
            int _dependsOnTask = tasksId[10];
            Dependency newDependency = new(_id, _dependentTask, _dependsOnTask);
            s_dal!.Dependency.Create(newDependency);
        }
        {
            int _id = 0; //will be overriden by Create method (with automatic random id)
            int _dependentTask = tasksId[17];
            int _dependsOnTask = tasksId[7];
            Dependency newDependency = new(_id, _dependentTask, _dependsOnTask);
            s_dal!.Dependency.Create(newDependency);
        }

        //Set task 16 to be dependent on the first 12 tasks exept 7,10
        for (int i = 0;i<12;i++)
        {
            if(i !=7 && i!=10)
            {
                int _id = 0; //will be overriden by Create method (with automatic random id)
                int _dependentTask = tasksId[16];
                int _dependsOnTask = tasksId[i];
                Dependency newDependency = new(_id, _dependentTask, _dependsOnTask);
                s_dal!.Dependency.Create(newDependency);
            }
        }
        //Total of 41 dependencies
    }

    /// <summary>
    /// publich method to call private methods to initialize lists
    /// </summary>
    public static void Do () 
    {
        s_dal = Factory.Get;
        //s_dal.Reset();
        createTasks();
        createEngineers();  
        createDependencies();
    }
}
