using System;
using System.Collections.Generic;
using System.Text;

public class DailyAssignment
{
    public DateTime timestamp = DateTime.Today.AddDays(-1);
    public List<AssignmentData2> assignments = new List<AssignmentData2>();
}

public class AssignmentData2
{

}

public class AssigmentData : IGameModuleData
{
    public string Key => "Assigments";
}


public class AssignmentProgress
{
    public DateTime timestamp = DateTime.Today.AddDays(-1);
    public List<AssignmentResult> Results = new List<AssignmentResult>();
}

public class AssignmentResult
{
    public string? Id;
    public bool Evaluation;
}
