# Medical Delivery Management System

A desktop medical delivery-management application developed independently using **C#**, **.NET 8**, **WPF**, and **XAML**.

The system manages the complete delivery lifecycle — from order creation and courier assignment to delivery tracking and completion.

It follows a layered architecture with clear separation between the **Presentation Layer**, **Business Logic Layer**, and **Data Access Layer**.

> **Solo Project** — Designed and developed independently as part of a .NET course.
---

## Submitted By

**Gilat Malka**  
**ID:** 213196363

---

## Overview

This project simulates a medical delivery company management system.

The application supports two main user roles:

- **Admin** — manages couriers, orders, deliveries, system configuration, system clock, database initialization/reset, and simulator control.
- **Courier** — views personal delivery details, chooses available orders, tracks active deliveries, completes deliveries, and reviews delivery history.

The system stores its data in XML files and also includes an in-memory DAL implementation that was used during earlier development stages.

---

## Main Features

- Role-based login for Admin and Courier users.
- Courier management:
  - add couriers;
  - update courier details;
  - delete couriers;
  - view couriers;
  - filter couriers.
- Order management:
  - create orders;
  - update orders;
  - cancel orders;
  - view orders;
  - filter orders.
- Delivery lifecycle management:
  - assign an order to a courier;
  - start a delivery;
  - complete a delivery;
  - track delivery status and schedule status.
- Admin dashboard:
  - system time controls;
  - order summary by status;
  - delivery timing summary;
  - editable system configuration values;
  - database initialization;
  - database reset;
  - simulator start/stop.
- Courier dashboard:
  - active delivery details;
  - available orders list;
  - completed delivery history.
- XML-based persistence for orders, couriers, deliveries, and system configuration.
- Observer pattern for refreshing UI data when entities, clock, or configuration values change.
- Layered architecture with DTO/BO separation and custom exception handling.

---

## Technologies Used

- **C#**
- **.NET 8**
- **WPF**
- **XAML**
- **XML Serialization**
- **LINQ**
- **Object-Oriented Programming**
- **Layered Architecture**

### Design Patterns

- **Factory**
- **Singleton**
- **Observer**

---

## Project Structure

```text
.
├── PL/                       # Presentation Layer - WPF screens and UI logic
├── BL/                       # Business Logic Layer - business rules and services
├── DalFacade/                # DAL interfaces, data objects, factory, and configuration
├── DalXml/                   # XML-based data access implementation
├── DalList/                  # In-memory data access implementation
├── DalTest/                  # Console application for testing the DAL
├── BITest/                   # Console application for testing the BL
├── xml/                      # XML data files and configuration files
├── Stage0/                   # Initial development stage project
└── dotNet5786_6363_9172.sln  # Solution file
```

---

## Architecture

The project is divided into three main layers:

1. **Presentation Layer**
2. **Business Logic Layer**
3. **Data Access Layer**

This structure improves maintainability, separates responsibilities, and allows each layer to be tested and updated independently.

---

## Presentation Layer — `PL`

The Presentation Layer contains the WPF user interface.

This layer includes:

- login screen;
- admin windows;
- courier windows;
- order windows;
- delivery windows;
- converters;
- UI helpers;
- XAML views;
- code-behind files.

The UI communicates with the Business Logic Layer and does not access the data directly.

---

## Business Logic Layer — `BL`

The Business Logic Layer contains the main application logic and validation rules.

The BL exposes services through interfaces such as:

- `IAdmin`
- `ICourier`
- `IOrder`
- `IDelivery`

This layer is responsible for:

- validating user actions;
- converting DAL entities into business objects;
- calculating delivery status;
- managing order assignment;
- completing deliveries;
- handling login validation;
- notifying the UI about data changes.

---

## Data Access Layer — `DalFacade`, `DalXml`, `DalList`

The Data Access Layer is accessed through interfaces and a factory.

### `DalFacade`

Defines the DAL contracts, data objects, factory, and configuration.

### `DalXml`

Stores and loads data from XML files.

### `DalList`

Provides an in-memory data access implementation.

The active DAL implementation is selected through:

```text
xml/dal-config.xml
```

---

## Main Entities

### Courier

Represents a delivery courier.

Main details include:

- courier ID;
- full name;
- phone number;
- email;
- password;
- vehicle type;
- maximum delivery distance;
- activity status;
- work start date.

---

### Order

Represents a customer delivery order.

Main details include:

- order ID;
- order type;
- description;
- address;
- coordinates;
- customer details;
- weight;
- opening date;
- order status.

---

### Delivery

Represents the connection between an order and a courier.

Main details include:

- delivery ID;
- order ID;
- courier ID;
- delivery start time;
- actual distance;
- expected distance;
- completion status;
- delivery end time.

---

### Config

Represents system configuration values.

Configuration includes:

- delivery range;
- courier speed values;
- base delivery price;
- price per kilometer;
- maximum delivery duration;
- system clock;
- admin credentials.

---

## User Roles

### Admin

The admin can:

- manage couriers;
- manage orders;
- view delivery information;
- update system configuration;
- advance the system clock;
- initialize the database;
- reset the database;
- start and stop the simulator.

---

### Courier

A courier can:

- log in using courier credentials;
- view personal courier information;
- choose an available order;
- view active order details;
- complete a delivery;
- view delivery history.

---

## Getting Started

### Prerequisites

Before running the project, make sure you have:

- Windows OS;
- Visual Studio 2022 or later;
- .NET 8 SDK;
- WPF workload installed in Visual Studio.

---

## Running the Project

1. Clone the repository:

```bash
git clone <repository-url>
```

2. Open the solution file in Visual Studio:

```text
dotNet5786_6363_9172.sln
```

3. Set `PL` as the startup project.

4. Make sure the `xml` folder exists at the solution level and contains the required XML files:

```text
couriers.xml
deliveries.xml
orders.xml
data-config.xml
dal-config.xml
```

5. Run the project.

---

## Demo Login Details

### Admin

```text
ID: 123456789
Password: admin123
```

---

### Example Courier

```text
ID: 1
Password: 1234
```

Additional courier users can be found in:

```text
xml/couriers.xml
```

---

## Data Storage

The project uses XML files for persistent data storage.

```text
orders.xml        # Stores delivery orders
couriers.xml      # Stores courier records
deliveries.xml    # Stores delivery records
data-config.xml   # Stores system configuration and admin credentials
dal-config.xml    # Selects the active DAL implementation
```

---

## Development Highlights

This project demonstrates:

- multi-project .NET solution structure;
- clean separation between UI, business logic, and data access;
- XML persistence and XML serialization;
- use of interfaces and factories for flexible DAL selection;
- observer-based UI refresh mechanism;
- custom exception classes for DAL and BL error handling;
- WPF data binding;
- dependency properties;
- system simulation with configurable time advancement;
- object-oriented design principles.

---

## Screenshots

Add screenshots here after uploading them to the repository:

### Login
<img width="1215" height="709" alt="image" src="https://github.com/user-attachments/assets/5d2f91a6-6932-4ea4-ab50-84c006a4e6fc" />

### Admin Dashboard
The admin dashboard provides system controls, delivery statistics, order summaries, system time management and simulator controls.
<img width="1327" height="916" alt="image" src="https://github.com/user-attachments/assets/0986f67e-1098-46b5-8bfe-5f34f1943951" />

### Couriers Management
Admins can view, filter, add, update and remove couriers while monitoring delivery performance.
<img width="1342" height="955" alt="image" src="https://github.com/user-attachments/assets/7560f082-0d4a-4959-93ce-f68bdbe2ad57" />

### Orders Management
The order management screen displays order types, delivery status, schedule status and timing information.
<img width="1327" height="939" alt="image" src="https://github.com/user-attachments/assets/c7cdd94e-ad88-46ae-af59-18febff897fe" />

### Courier Workspace
Couriers can view their personal information, delivery statistics, active orders, delivery history and available orders.
<img width="1321" height="963" alt="image" src="https://github.com/user-attachments/assets/7719d7fa-3908-439c-8534-c6c531debc42" />

---

## Future Improvements

- Add a database-based DAL implementation using SQL Server or PostgreSQL.
- Add unit tests for the BL and DAL layers.
- Improve authentication and password handling.
- Add more detailed reports and analytics for deliveries.
- Add maps/geolocation integration for route estimation.
- Improve UI styling and responsiveness.

---

## Notes

This project was developed as part of a .NET course assignment.

The system follows a staged development structure, starting from basic DAL functionality and progressing to a full WPF application with business logic, XML persistence, UI synchronization, and simulation features.

---

## Author

**Gilat Kedem**  
**ID:** 213196363
