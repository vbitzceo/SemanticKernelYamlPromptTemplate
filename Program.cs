using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace SemanticKernelYamlPromptTemplate;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 Semantic Kernel YAML Prompt Template Demo");
        Console.WriteLine("============================================\n");

        try
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();

            // Get Azure OpenAI configuration
            var endpoint = configuration["AzureOpenAI:Endpoint"];
            var deploymentName = configuration["AzureOpenAI:DeploymentName"];
            var apiKey = configuration["AzureOpenAI:ApiKey"];

            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(deploymentName) || string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("❌ Please configure your Azure OpenAI settings in appsettings.json");
                Console.WriteLine("Required settings:");
                Console.WriteLine("- AzureOpenAI:Endpoint");
                Console.WriteLine("- AzureOpenAI:DeploymentName");
                Console.WriteLine("- AzureOpenAI:ApiKey");
                return;
            }

            // Create kernel with Azure OpenAI
            var kernelBuilder = Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion(
                    deploymentName: deploymentName,
                    endpoint: endpoint,
                    apiKey: apiKey);

            var kernel = kernelBuilder.Build();

            // Load YAML prompt template
            var yamlPromptPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ChatPrompt.yaml");
            
            if (!File.Exists(yamlPromptPath))
            {
                Console.WriteLine($"❌ YAML prompt template not found at: {yamlPromptPath}");
                return;
            }

            Console.WriteLine($"📄 Loading YAML prompt template from: {yamlPromptPath}");
            
            // Create chat kernel function from YAML file
            var chatFunction = kernel.CreateFunctionFromPrompt(
                File.ReadAllText(yamlPromptPath));

            // Create code review function from YAML file
            var codeReviewPromptPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "CodeReviewPrompt.yaml");
            if (!File.Exists(codeReviewPromptPath))
            {
                Console.WriteLine($"❌ Code review YAML prompt template not found at: {codeReviewPromptPath}");
                return;
            }
            Console.WriteLine($"📄 Loading code review YAML prompt template from: {codeReviewPromptPath}");
            var codeReviewFunction = kernel.CreateFunctionFromPrompt(
                File.ReadAllText(codeReviewPromptPath));

            // Demo different scenarios
            await RunDemoScenarios(kernel, chatFunction, codeReviewFunction);

            Console.WriteLine("\n✅ Demo completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static async Task RunDemoScenarios(Kernel kernel, KernelFunction chatFunction, KernelFunction codeReviewFunction)
    {
        Console.WriteLine("\n🎯 Running Demo Scenarios");
        Console.WriteLine("========================\n");

        // Scenario 1: Default assistant with programming question
        Console.WriteLine("📝 Scenario 1: Programming Assistant");
        Console.WriteLine("------------------------------------");
        
        var result1 = await kernel.InvokeAsync(chatFunction, new KernelArguments
        {
            ["assistant_name"] = "CodeMaster",
            ["topic"] = "software development and programming",
            ["user_question"] = "What are the benefits of using YAML for prompt templates in Semantic Kernel?"
        });

        Console.WriteLine($"🤖 Response: {result1}\n");

        // Scenario 2: Science assistant
        Console.WriteLine("📝 Scenario 2: Science Assistant");
        Console.WriteLine("---------------------------------");
        
        var result2 = await kernel.InvokeAsync(chatFunction, new KernelArguments
        {
            ["assistant_name"] = "Dr. Science",
            ["topic"] = "physics and astronomy",
            ["user_question"] = "How do black holes work and what happens to matter that falls into them?"
        });

        Console.WriteLine($"🤖 Response: {result2}\n");

        // Scenario 3: Using default values (only providing required parameter)
        Console.WriteLine("📝 Scenario 3: Using Default Values");
        Console.WriteLine("-----------------------------------");
        
        var result3 = await kernel.InvokeAsync(chatFunction, new KernelArguments
        {
            ["user_question"] = "What's the weather like today?"
        });

        Console.WriteLine($"🤖 Response: {result3}\n");

        // Scenario 4: Code Review with Poor Style
        Console.WriteLine("📝 Scenario 4: Code Review Assistant");
        Console.WriteLine("------------------------------------");
        
        var badCodeExample = @"
            public class calc{
            public int x,y;
            public calc(int X,int Y){x=X;y=Y;}
            public int add(){return x+y;}
            public int sub(){return x-y;}
            public void print(){Console.WriteLine(""Result: ""+add());}}";

        Console.WriteLine("🔍 Code being reviewed:");
        Console.WriteLine(badCodeExample);
        
        var codeReviewResult = await kernel.InvokeAsync(codeReviewFunction, new KernelArguments
        {
            ["code_to_review"] = badCodeExample
        });

        Console.WriteLine($"\n🤖 Code Review: {codeReviewResult}\n");

        // Scenario 5: Interactive mode
        Console.WriteLine("📝 Scenario 5: Interactive Mode");
        Console.WriteLine("-------------------------------");
        Console.WriteLine("Enter your question (or 'quit' to exit):");
        
        while (true)
        {
            Console.Write("❓ Your question: ");
            var userInput = Console.ReadLine();
            
            if (string.IsNullOrEmpty(userInput) || userInput.ToLower() == "quit")
                break;

            var interactiveResult = await kernel.InvokeAsync(chatFunction, new KernelArguments
            {
                ["assistant_name"] = "InteractiveBot",
                ["topic"] = "general knowledge and assistance",
                ["user_question"] = userInput
            });

            Console.WriteLine($"🤖 Response: {interactiveResult}\n");
        }
    }
}
