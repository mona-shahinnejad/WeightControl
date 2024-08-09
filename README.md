# WeightControl

Welcome to the **WeightControl** repository. This project is a .NET-based application designed to manage and monitor product weight ranges, ensuring that products fall within acceptable tolerance limits. The application includes a series of handlers, services, and validation mechanisms to manage this functionality.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Usage](#usage)
- [Tests](#tests)
- [Contributing](#contributing)
- [License](#license)

## Features

- **Weight Tolerance Check:** Verify if a product's weight is within specified minimum and maximum limits.
- **Product Management:** Services and handlers to create, update, and manage product details.
- **Exception Handling:** Comprehensive error handling for invalid inputs and edge cases.
- **Result Management:** Uses `Ardalis.Result` for handling results in a clean and consistent way.

## Getting Started

### Prerequisites

Ensure you have the following installed on your development machine:

- .NET 6.0 SDK or later
- A code editor like Visual Studio, Visual Studio Code, or Rider
- Git for version control

### Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/mona-shahinnejad/WeightControl.git
    cd WeightControl
    ```

2. **Restore NuGet packages:**

    In the terminal, navigate to the root directory of the project and run:

    ```bash
    dotnet restore
    ```

3. **Build the project:**

    ```bash
    dotnet build
    ```

### Running the Application

To run the application, use the following command:

```bash
dotnet run
