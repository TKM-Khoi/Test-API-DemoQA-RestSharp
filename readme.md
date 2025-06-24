# 📘 Restsharp Automation Project (C#)

This project demonstrates a **Restsharp** test automation framework for the demoqc apis using **NUnit**, **Restsharp** and **ExtentReports**. It’s structured cleanly with separation of concerns, applying layered architecture and concentrated data.

---

## 📁 Folder & File Structure Explanation

```
.

TranKhaiMinhKhoiFinalRestSharp
├── Core                                            # Core logic and base utilities
│   ├── Client
│   │   └── ApiClient.cs                            # HTTP client setup for sending REST API requests
│   ├── Reports
│   │   └── ExtentReportHelper.cs                   # Helper class to generate ExtentReports
│   ├── ShareData
│   │   └── DataStorage.cs                          # Shared static data for use across tests
│   ├── Utils                                       # Common utility classes
│   │   ├── ConfigurationUtils.cs                   # Read config values from appsettings
│   │   ├── FilePathExtension.cs                    # Extensions to work with file paths
│   │   ├── JsonFileUtils.cs                        # Helpers for reading/writing JSON files
│   │   └── RestExtension.cs                        # Extension methods for REST operations
│   └── Core.csproj                                 # Project file for Core project
|
├── ProjectTest                                     # Main test project
│   ├── Configurations
│   │   └── appsettings.json                        # Configuration settings for tests
│   ├── DataProvider                                # Provides test data to the tests
│   │   ├── ObjectData
│   │   │   ├── AccountDataProvider.cs              # Supplies account-related test data
│   │   │   └── BookDataProvider.cs                 # Supplies book-related test data
│   │   └── TestCaseData
│   │       ├── AddBookDataProvider.cs              # Data for adding a book
│   │       ├── DeleteBookDataProvider.cs           # Data for deleting a book
│   │       ├── GetAccountDetailDataProvider.cs     # Data for retrieving account info
│   │       └── ReplaceBookDataProvider.cs          # Data for replacing book info
│   ├── TestData                                    # Raw JSON files and schemas for tests
│   │   ├── ObjectData
│   │   │   ├── account_data.json                   # Account test data
│   │   │   └── book_data.json                      # Book test data
│   │   ├── Schema
│   │   │   └── replace_book_success_schema.json    # Schema for validating replace response
│   │   └── TestCaseData
│   │       ├── add_book_data.json                  # Add book test cases
│   │       ├── delete_book_data.json               # Delete book test cases
│   │       ├── get_account_detail_data.json        # Get account details test cases
│   │       └── replace_book_data.json              # Replace book test cases
│   ├── Tests                                       # C# test files
│   │   ├── AddBookTest.cs                          # Tests for adding a book
│   │   ├── DeleteBookTest.cs                       # Tests for deleting a book
│   │   ├── GetUserDetailTest.cs                    # Tests for getting user details
│   │   ├── ReplaceBookTest.cs                      # Tests for replacing a book
│   │   └── Hooks.cs                                # Hooks for test lifecycle (before/after)
│   └── ProjectTest.csproj                          # Project file for the test project
|
└── Service                                         # Backend service logic and API models
    ├── Const
    │   ├── EndpointConst.cs                        # Constants for endpoint apis
    │   ├── ErrorConst.cs                           # Constants for error codes/messages
    │   ├── FilePathConst.cs                        # Constants for file paths
    │   └── TestDataKeyConsts.cs                    # Constants for test data keys
    ├── Extensions
    │   └── ApiResponseExtension.cs                 # Helpers to validate/parse responses
    ├── Models
    │   ├── Data
    │   │   ├── ObjectData
    │   │   │   ├── AccountData.cs                  # Json data model for account entity data
    │   │   │   └── BookData.cs                     # Json data model for book entity data
    │   │   └── TestCaseData
    │   │       ├── AddBookData.cs                  # Json data model for add book case
    │   │       ├── DeleteBookData.cs               # Json data model for delete book case
    │   │       ├── GetAccountDetailData.cs         # Json data model for get account case
    │   │       └── ReplaceBookData.cs              # Json data model for replace book case
    │   ├── DTOs
    │   │   └── IsbnDto.cs                          # DTO for ISBN structure
    │   ├── Response
    │   │   ├── AddBookResponseDto.cs               # Response model for add book api
    │   │   ├── ReplaceBookResponseDto.cs           # Response model for replace book api
    │   │   ├── TokenResponseDto.cs                 # Response model for token request api
    │   │   └── UserDetailResponseDto.cs            # Response model for user details api
    │   └── Requests
    │       ├── AddBookRequest.cs                   # Request model for add book
    │       └── TokenRequest.cs                     # Request model for token
    ├── Services        
    │   ├── AccountService.cs                       # API methods for account services
    │   └── BookService.cs                          # API methods for book services
    └── Service.csproj                              # Project file for the service logic
```

---

## ✅ Work Completed

### 🔧 Implemented Features:
- Automated tests for Get User Detail api, Add Book To User Collection api, Delete Book From User Collection api and Replace Book From User Collection Api
- Centralized data keys, api message and code
- Parallel Execution on Feature level

---

## ⚠️ Known Issues & Workarounds

### 1.Error: Difficulty in managing UserId and BookIsbn
**Cause**: Long and complicated UserId and BookIsbn

**Resolution**:  
Seperate books and users from the testcases' data into their own file, connecting via keys.

---

### 2.Issue:  Complicated data setup cleanup for repeated usage and different tests cases may affect each other' user book collection

**Cause**: running tests Paralel on Feature layer


**Resolution**: 
Using Add Book To User Collection api and Delete All of user's book collection api, with the precondition that those apis work fine in the happy case

Using different accounts for each test cases.

Saving each features' accounts' info into different sets to avoid 1 case might remove the set's entiy while another is looping through that set

Remove all books from account's book collection incase the apis fail


**Status**:  
Still unresolved. Possibly caused by report rendering logic or test runner quirks.

---

## 🚀 How to Run

### 1. Clone the repository (if you are authorized)
```bash
git clone https://gitlab.com/anhtien1306/rookies-batch8.git
git checkout TranKhaiMinhKhoi-final-api
cd <your-folder>
```

### 2. Install dependencies & build
```bash
dotnet restore
dotnet build
```

### 3. Execute Tests
```bash
dotnet test
```

---

## 🔖 Tag Filtering

Run only tests with specific tag:
**By feature**:  
```bash
dotnet test --filter "Category=add"
dotnet test --filter "Category=delete"
dotnet test --filter "Category=replace"
dotnet test --filter "Category=detail"
```

**By api outcome**:  
```bash
dotnet test --filter "Category=success"
dotnet test --filter "Category=fail"
```

---

## 🧵 Parallel Execution

Enable **feature-level parallelization** in `Hooks.cs`:
```csharp
[Parallelizable(ParallelScope.Fixtures)]
```

---

## 🔗 Useful References

- [RestSharp](https://restsharp.dev/) - HTTP client for .NET
- [NUnit](https://nunit.org/) - Unit testing framework
- [ExtentReports](https://extentreports.com/) - Test reporting

---

## 🙋 Author

**Tran Khai Minh Khoi**  

---

Thank you for your effort, dedication and feedback!
