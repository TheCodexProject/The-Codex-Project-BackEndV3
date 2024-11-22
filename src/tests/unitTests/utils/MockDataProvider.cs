using domain.models.organization;
using domain.models.project;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.user;
using domain.models.workItem;
using domain.models.workspace;

namespace unitTests.utils;

/// <summary>
/// This class is response for providing mock data for unit tests.
/// </summary>
public static class MockDataProvider
{
    private static readonly User User = User.Create("John", "Doe", "johndoe@mail.com");
    private static readonly Organization Organization = Organization.Create("Test Organization", User);
    private static readonly Workspace Workspace = Workspace.Create(Organization, "Test Workspace");
    private static readonly Project Project = Project.Create(Workspace, "Test Project");
    private static readonly WorkItem Task = WorkItem.Create(Project, "Test Task");


    public static User GetUser() => User.Create("John", "Doe", "johndoe@mail.com");
    public static Organization GetOrganization() => Organization.Create("Test Organization", User);
    public static Workspace GetWorkspace() => Workspace.Create(Organization, "Test Workspace");
    public static Project GetProject() => Project.Create(Workspace, "Test Project");
    public static WorkItem GetTask() => WorkItem.Create(Project, "Test Task");

    public static List<User> GetUsers(int count)
    {
        var users = new List<User>();
        for (var i = 0; i < count; i++)
        {
            users.Add(User.Create($"User", $"Doe", $"userdoe@mail.com").Value);
        }

        return users;
    }

    public static List<Organization> GetOrganizations(int count, bool useDifferent = false)
    {
        var toUse = useDifferent ? User.Create("Jane", "Doe", "janedoe@mail.com").Value : User;

        var organizations = new List<Organization>();
        for (var i = 0; i < count; i++)
        {
            organizations.Add(Organization.Create($"Organization {i}", toUse));
        }

        return organizations;
    }

    public static List<Workspace> GetWorkspaces(int count, bool useDifferent = false)
    {
        var toUse = useDifferent ? Organization.Create("Different Organization", User).Value : Organization;

        var workspaces = new List<Workspace>();
        for (var i = 0; i < count; i++)
        {
            workspaces.Add(Workspace.Create(toUse, $"Workspace {i}").Value);
        }

        return workspaces;
    }

    public static List<Project> GetProjects(int count, bool useDifferent = false)
    {
        var toUse = useDifferent ? Workspace.Create(Organization, "Different").Value : Workspace;

        var projects = new List<Project>();
        for (var i = 0; i < count; i++)
        {
            projects.Add(Project.Create(toUse, $"Project {i}"));
        }

        return projects;
    }

    public static List<WorkItem> GetTasks(int count)
    {
        var tasks = new List<WorkItem>();
        for (var i = 0; i < count; i++)
        {
            tasks.Add(WorkItem.Create(Project, $"Task {i}").Value);
        }

        return tasks;
    }

    public static List<Resource> GetResources(int count, ResourceLevel level)
    {
        var resources = new List<Resource>();

        var id = level switch
        {
            ResourceLevel.Project => Project.Id,
            ResourceLevel.Workspace => Workspace.Id,
            ResourceLevel.Organization => Organization.Id,
            _ => Guid.Empty
        };

        for (var i = 0; i < count; i++)
        {
            resources.Add(Resource.Create($"New Resource {i}", $"Resource {i}", id, level).Value);
        }

        return resources;
    }

    public static List<ProjectActivity> GetProjectActivities(int count, bool useDifferent = false)
    {
        var toUse = useDifferent ? Project.Create(Workspace, "Different").Value : Project;

        var projectActivities = new List<ProjectActivity>();
        for (var i = 0; i < count; i++)
        {
            projectActivities.Add(ProjectActivity.Create(toUse, $"Activity {i}",ProjectActivityType.Milestone).Value);
        }

        return projectActivities;
    }
}