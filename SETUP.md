# Setup Instructions

## Prerequisites Installation

### 1. Install .NET 8.0 SDK

Download and install the .NET 8.0 SDK from:
https://dotnet.microsoft.com/download/dotnet/8.0

### 2. Verify Installation

Open a new PowerShell window and run:
```powershell
dotnet --version
```

You should see version 8.0.x or higher.

### 3. Build and Run the Project

```powershell
# Navigate to the project directory
cd "c:\develeopment\SemanticKernelYamlPromptTemplate"

# Restore NuGet packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

## Azure OpenAI Setup

1. **Create an Azure OpenAI Resource**:
   - Go to Azure Portal
   - Create a new Azure OpenAI resource
   - Deploy a GPT-4o model

2. **Get Required Information**:
   - Endpoint URL (e.g., `https://your-resource-name.openai.azure.com/`)
   - API Key
   - Deployment Name (the name you gave to your GPT-4o deployment)

3. **Configure the Application**:
   
   **Option A: Update appsettings.json**
   ```json
   {
     "AzureOpenAI": {
       "Endpoint": "https://your-resource-name.openai.azure.com/",
       "DeploymentName": "gpt-4o",
       "ApiKey": "your-api-key-here"
     }
   }
   ```
   
   **Option B: Use User Secrets (Recommended)**
   ```powershell
   dotnet user-secrets init
   dotnet user-secrets set "AzureOpenAI:Endpoint" "https://your-resource-name.openai.azure.com/"
   dotnet user-secrets set "AzureOpenAI:DeploymentName" "gpt-4o"
   dotnet user-secrets set "AzureOpenAI:ApiKey" "your-api-key-here"
   ```

## Project Structure

```
SemanticKernelYamlPromptTemplate/
├── Templates/
│   ├── ChatPrompt.yaml          # Basic chat assistant template
│   └── CodeReviewPrompt.yaml    # Code review assistant template
├── Program.cs                   # Main application
├── appsettings.json            # Configuration file
├── SemanticKernelYamlPromptTemplate.csproj
├── README.md
└── SETUP.md                    # This file
```

## Troubleshooting

### Common Issues:

1. **"dotnet command not found"**
   - Install .NET 8.0 SDK and restart your terminal

2. **Authentication errors**
   - Verify your Azure OpenAI endpoint, API key, and deployment name
   - Check that your Azure OpenAI resource is properly configured

3. **Template not found**
   - Ensure YAML files are in the Templates/ folder
   - Check that file paths are correct

4. **Package restore issues**
   - Clear NuGet cache: `dotnet nuget locals all --clear`
   - Try restore again: `dotnet restore`

### Getting Help:

- Check the Azure OpenAI documentation
- Verify Semantic Kernel documentation for latest API changes
- Ensure your Azure subscription has proper permissions
