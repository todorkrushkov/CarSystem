# Vehicle Violation Management System

A desktop application for managing vehicle violations, registrations, and fines built with WPF and .NET Framework.

## Overview

The system allows traffic officers or administrative staff to manage people, vehicles, violations, and associated fines through a clean Windows desktop interface.

## Features

- **People management** — register and manage vehicle owners with gender classification
- **Vehicle registrations** — track cars by fuel type and emission standard
- **Violations** — create and manage traffic violations linked to people and vehicles
- **Fines** — assign and track fines per person
- **References** — manage lookup data (fuel types, emission standards, genders, fine types)
- **Export** — export data to external formats via `ExportService`
- **Soft delete** — records are marked as deleted rather than permanently removed

## Project Structure

```
CarSystem/
├── CarSystem.App/          # WPF UI layer (windows, forms, view models)
├── CarSystem.Services/     # Business logic layer
├── CarSystem.Data/         # Entity Framework DbContext and migrations
├── CarSystem.Data.Models/  # Domain models
└── CarSystem.sln
```

## Technology Stack

- **UI** — WPF (.NET Framework)
- **ORM** — Entity Framework 6 (Code First, SQL Server)
- **DI** — Autofac
- **Mapping** — AutoMapper
- **Database** — SQL Server (`CarSystemDb`)

## Getting Started

### Prerequisites

- Visual Studio 2019 or later
- .NET Framework 4.x
- SQL Server (local or remote)

### Setup

1. Open `CarSystem.sln` in Visual Studio.
2. Update the connection string in `App.config` if your SQL Server instance differs from the default.
3. Build the solution — NuGet packages will restore automatically.
4. Run the application. Entity Framework will automatically apply migrations and create the database on first launch.
