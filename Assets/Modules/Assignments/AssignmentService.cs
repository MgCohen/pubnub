using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class AssignmentService : GameModule<AssigmentData>
{
    [Inject] private ICloudCodeService cloudCode;

    private DailyAssignment assignment;

    public override string Key => "Assignments";

    protected override Task Initialize(AssigmentData data)
    {
        return Task.CompletedTask;
    }

    public Task<DailyAssignment> GetDailyWork()
    {
        return null;
    }

    public void CompleteAssigment(AssigmentData assigment, bool evaluation)
    {

    }

    public void DeliverWork(List<AssigmentResults> results)
    {

    }

    public void Test()
    {
        cloudCode.Request(new DeliverAssignmentRequest());
    }

    public async Task Initialize()
    {
        assignment = await GetDailyWork();
    }

}

public class DeliverAssignmentRequest: PubnubRequest
{
    public override string Endpoint => "DeliverAssignment";
}

public class Assignment
{
    public AssigmentData Data { get; private set; }
    public bool? Evaluation { get; private set; }


}

public class DailyAssignment
{
    public List<AssigmentData> Assigment = new List<AssigmentData>();
}

public class CompletedAssignment
{
    public string id;
    public bool evaluation;
}

public class AssigmentData2
{
    public string id;
    public string content;
}

public class AssigmentResults
{
    public string id;
    public bool evaluation;
}
