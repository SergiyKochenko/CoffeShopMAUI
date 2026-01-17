# CoffeShopMAUI

## Overview
CoffeShopMAUI is a .NET 8 MAUI mobile ordering app for a small coffee shop. Customers browse categorized menus, adjust quantities, review a cart, and complete checkout that generates a unique order number, captures contact details, and stores the order for later retrieval.

## Features
- Menu pages for hot drinks, cold drinks, and food
- Quantity adjustments, cart review, and item removal
- Checkout form with customer name/phone plus receipt screen
- Order history listing all orders placed today
- Local persistence using file I/O or SQLite
- MVVM pages targeting Android, iOS, Windows, and macOS

## Architecture
- Multi-target .NET 8 MAUI solution with `AppShell` navigation
- `App.xaml`/`App.xaml.cs` bootstraps resources and main page
- ViewModels expose commands and observable state for each page
- Persistence handled through a repository abstraction for testability

## Prerequisites
- .NET 8 SDK
- Visual Studio 2022 17.8+ with the .NET MAUI workload installed
- Emulator or device for Android/iOS, or Windows/Mac Catalyst runtime

## Getting Started
1. Clone the repo: `git clone https://github.com/<your-account>/CoffeShopMAUI.git`
2. Open `CoffeShopMAUI.sln` in Visual Studio.
3. Restore NuGet packages and build.
4. Select a target (Android emulator, Windows, iOS, Mac Catalyst) and press **F5**.

## Usage
1. Pass the splash/login screen to reach the home menu.
2. Pick a category and add items with the required quantities.
3. Open the cart to adjust or remove items.
4. Continue to checkout, enter customer details, and confirm to view the receipt.
5. Visit the order history page to review the day’s orders.

## Project Structure
```
CoffeShopMAUI/
 ?? App.xaml / App.xaml.cs
 ?? AppShell.xaml / AppShell.xaml.cs
 ?? MauiProgram.cs
 ?? Pages/
 ?? ViewModels/
 ?? Resources/
```

## Testing & Validation
- Create multiple orders and verify they appear in the order history for the current date.
- Ensure checkout buttons remain disabled when the cart is empty or input is invalid.
- Deploy to each platform target to confirm consistent UI and functionality.