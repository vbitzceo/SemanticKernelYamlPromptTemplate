using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

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
            
            // Create kernel function from YAML file
            var chatFunction = kernel.CreateFunctionFromPromptYaml(
                File.ReadAllText(yamlPromptPath));

            // Demo different scenarios
            await RunDemoScenarios(kernel, chatFunction);

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

    static async Task RunDemoScenarios(Kernel kernel, KernelFunction chatFunction)
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

        // Scenario 4: Interactive mode
        Console.WriteLine("📝 Scenario 4: Interactive Mode");
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
