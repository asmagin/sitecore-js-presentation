namespace Sparrow.Sample.Console
{
    using System;

    using Sitecore.Js.Presentation.Configuration;
    using Sitecore.Js.Presentation.Managers;

    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // System.Console.WriteLine("Test #\t\tInit time, s\t\tExec time, s");

                // for (var j = 0; j < 1; j++)
                // {

                // var start = DateTime.Now;
                // using (var engine = new Engine())
                // {
                // var inittime = (DateTime.Now - start).TotalSeconds;

                // var command = @"common__ext.header.renderToString({})";
                // start = DateTime.Now;

                // for (var i = 0; i < 1; i++)
                // {
                // var result = engine.Execute(command);
                // // System.Console.WriteLine(i);
                // }

                // var time = (DateTime.Now - start).TotalSeconds;
                // System.Console.WriteLine($"{j}\t\t{inittime}\t\t{time}");
                // }
                // }
                using (
                    var manager =
                        new JsEngineManager(
                            new JsEngineManagerConfiguration
                                {
                                    Modules =
                                        {
                                            @"d:\.projects\Asmagin\src\client\dist\assets\shared.js",
                                            @"d:\.projects\Asmagin\src\client\dist\assets\vendor.js",
                                            @"d:\.projects\Asmagin\src\client\dist\assets\common.js"
                                        }
                                }))
                {
                    var engine = manager.GetEngine();
                    var command = @"common__ext.header.renderToString({ })";
                    try
                    {
                        var result = engine.Execute<string>(command);
                        System.Console.WriteLine(result);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }

                    System.Console.WriteLine(engine.Execute<string>("console.getCalls()"));

                    while (true)
                    {
                        System.Console.WriteLine("> ");
                        command = System.Console.ReadLine();
                        try
                        {
                            var result = engine.Execute<string>(command);
                            System.Console.WriteLine(result);
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine(ex.Message);
                        }

                        System.Console.WriteLine(engine.Execute<string>("console.getCalls()"));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
        }
    }
}