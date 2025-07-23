# Network Manager

A .NET console application for managing network settings on Windows, built with the Spectre.Console library for a visually appealing and interactive user interface. This app allows users to view and modify DNS settings, IP configurations, and network adapter states, with features like loading animations and tabulated output for clarity.

## Features

- **DNS Management**:

  - Display current DNS settings for all active network adapters.
  - Set new primary and secondary DNS addresses for a selected adapter.

- **IP Configuration**:

  - View current IP addresses, subnet masks, and gateways for active network adapters.
  - Configure a static IP address, subnet mask, and optional gateway, or switch to DHCP.

- **Network Adapter Control**:

  - List available network adapters with their status (Enabled/Disabled).
  - Enable or disable a selected network adapter.

- **User Interface**:

  - Colorful and structured console output using Spectre.Console:
    - **Cyan** for headers and adapter names.
    - **Yellow** for titles and key information.
    - **Green** for prompts and success messages.
    - **Red** for error messages.
    - **Magenta** for loading animation messages.
    - **Grey** for secondary information and prompts.
  - Loading animations (dot-based) during operations like fetching settings or applying changes.
  - Tabulated output for network adapter details, DNS, and IP configurations.
  - Input validation for user prompts to ensure correct inputs.
  - Welcome and exit animations for a polished experience.

- **Platform**:

  - Designed for Windows, using `netsh` commands for network configuration.
  - Compatible with terminals supporting ANSI escape sequences for Spectre.Console's formatting.

## Prerequisites

- **.NET SDK**: Version 8.0 or later (compatible with Spectre.Console).
- **Operating System**: Windows (required for `netsh` commands used in DNS, IP, and adapter management).
- **Administrative Privileges**: Some operations (e.g., setting DNS or IP, enabling/disabling adapters) require running the application as an administrator.

## Installation

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/your-username/network-manager.git
   cd network-manager
   ```

2. **Restore Dependencies**: Ensure the Spectre.Console library is included by adding it to your project file (`Network.Manager.Console.App.csproj`):

   ```xml
   <ItemGroup>
       <PackageReference Include="Spectre.Console" Version="0.49.1" />
   </ItemGroup>
   ```

   Then, restore the packages:

   ```bash
   dotnet restore
   ```

3. **Build the Project**:

   ```bash
   dotnet build
   ```

## Usage

1. **Run the Application**: Run the application with administrative privileges to allow network configuration changes:

   ```bash
   dotnet run
   ```

   On Windows, you may need to run the terminal as an administrator (e.g., right-click Command Prompt or PowerShell and select "Run as administrator").

2. **Menu Options**:

   - The app displays a menu with the following options, styled with Spectre.Console:
     - **1. Show Current DNS**: Displays DNS settings for all active network adapters in a table.
     - **2. Set New DNS**: Prompts for an adapter name, primary DNS, and optional secondary DNS, then applies the settings.
     - **3. Show Current IP**: Shows IP addresses, subnet masks, and gateways for active adapters in a table.
     - **4. Set New IP (Static/DHCP)**: Allows setting a static IP address, subnet mask, and optional gateway, or switching to DHCP.
     - **5. Enable/Disable Network Adapter**: Lists adapters and allows enabling or disabling a selected adapter.
     - **6. Exit**: Exits the application with a farewell animation.

3. **Example Workflow**:

   - Launch the app to see a welcome animation with a yellow "Network Manager" title and dots.
   - Select option `1` to view current DNS settings in a table, with adapter names in cyan.
   - Choose option `2`, enter an adapter name (e.g., "Ethernet"), and set a primary DNS (e.g., `8.8.8.8`) and optional secondary DNS (e.g., `8.8.4.4`). A loading animation shows while settings are applied.
   - Select option `4` to set a static IP (e.g., `192.168.1.100`, subnet `255.255.255.0`) or switch to DHCP for an adapter.
   - Use option `5` to disable a network adapter, then re-enable it, with a loading animation for each action.
   - Exit with option `6` to see a farewell animation.

## Configuration

- **Network Commands**: The app uses `netsh` commands to manage DNS, IP, and adapter settings, executed via `ProcessStartInfo`.
- **Output Encoding**: Configured to use UTF-8 for correct display of Spectre.Console's formatted output.
- **Error Handling**: Displays error messages in red for issues like invalid inputs, failed `netsh` commands, or network errors.

## Notes

- **Spectre.Console**: Provides a modern, colorful console experience with tables, prompts, and animations. Colors may not display correctly in terminals without ANSI support or in CI/CD environments.
- **Administrative Privileges**: Operations like setting DNS, IP, or toggling adapters require elevated permissions. Run the app as an administrator to avoid errors.
- **Loading Animation**: A dot-based animation (`...`) appears during operations like fetching settings or applying changes, styled in magenta for visibility.
- **Dependencies**: The app relies on `System.Net.NetworkInformation` for network interface details and `Spectre.Console` for UI rendering.

## Contributing

Contributions are welcome! Please submit a pull request or open an issue on the repository for bug reports, feature requests, or improvements.

## License

This project is licensed under the MIT License. See the LICENSE file for details.

## Acknowledgments

- Spectre.Console for providing a rich console UI with tables, prompts, and animations.
- Windows `netsh` utility for network configuration capabilities.
