# Semantic Kernel YAML Prompt Template Demo

This is a simple C# console application that demonstrates how to use YAML prompt templates with Microsoft Semantic Kernel and Azure OpenAI GPT-4o.

## Features

- 🎯 YAML-based prompt templates
- 🚀 Azure OpenAI GPT-4o integration
- 📝 Multiple demo scenarios
- 🔧 Configurable parameters
- 💬 Interactive chat mode

## Prerequisites

- .NET 8.0 SDK
- Azure OpenAI resource with GPT-4o deployment
- Visual Studio Code or Visual Studio

## Setup

1. **Clone or download this project**

2. **Configure Azure OpenAI settings** in `appsettings.json`:
   ```json
   {
     "AzureOpenAI": {
       "Endpoint": "https://your-resource-name.openai.azure.com/",
       "DeploymentName": "gpt-4o",
       "ApiKey": "your-api-key-here"
     }
   }
   ```

3. **Alternative: Use User Secrets** (recommended for production):
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "AzureOpenAI:Endpoint" "https://your-resource-name.openai.azure.com/"
   dotnet user-secrets set "AzureOpenAI:DeploymentName" "gpt-4o"
   dotnet user-secrets set "AzureOpenAI:ApiKey" "your-api-key-here"
   ```

## Running the Application

1. **Restore packages**:
   ```bash
   dotnet restore
   ```

2. **Build the project**:
   ```bash
   dotnet build
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

## YAML Prompt Template Structure

The application uses a YAML prompt template located in `Templates/ChatPrompt.yaml` with the following structure:

```yaml
name: ChatPrompt
description: A simple chat prompt template
template_format: semantic-kernel
template: |
  You are a helpful AI assistant named {{$assistant_name}}.
  # ... template content
input_variables:
  - name: assistant_name
    description: The name of the AI assistant
    default: "Alex"
  # ... other variables
execution_settings:
  default:
    max_tokens: 1000
    temperature: 0.7
```

## Demo Scenarios

The application runs several demo scenarios:

1. **Programming Assistant** - Specialized in software development
2. **Science Assistant** - Focused on physics and astronomy
3. **Default Values** - Shows how default parameters work
4. **Interactive Mode** - Real-time chat interaction

## Key Components

- **Program.cs** - Main application logic and demo scenarios
- **Templates/ChatPrompt.yaml** - YAML prompt template definition
- **appsettings.json** - Configuration file for Azure OpenAI settings

## Benefits of YAML Prompt Templates

- 📋 **Structured Configuration** - Clear separation of prompt logic and parameters
- 🔄 **Reusability** - Templates can be easily shared and modified
- 📖 **Readability** - Human-readable format for complex prompts
- ⚙️ **Parameter Management** - Built-in support for default values and validation
- 🎛️ **Execution Settings** - Configurable AI model parameters

## Troubleshooting

- Ensure your Azure OpenAI endpoint and API key are correct
- Verify that your GPT-4o deployment name matches the configuration
- Check that the YAML template file exists in the Templates folder
- Make sure you have the required .NET 8.0 SDK installed

## License

This project is provided as-is for educational and demonstration purposes.
