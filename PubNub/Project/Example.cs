using PubNub;
using System;
using Unity.Services.CloudCode.Core;

namespace HelloWorld;

public class MyModule
{
    [CloudCodeFunction("SayHello")]
    public SampleObject Hello()
    {
        return new SampleObject(new Random().Next(0, 10));
    }
}


