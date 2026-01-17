# ShareXe - Carpooling Backend API 🚗

![Platform](https://img.shields.io/badge/Platform-.NET%209.0-purple.svg)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue.svg)

**ShareXe** is a hub-to-hub carpooling service that allows users to book specific seats on a car ride. Unlike traditional ride-hailing apps, ShareXe focuses on route optimization between major hubs (Hospitals, Parks, Universities) and provides a unique "Seat Selection" feature.

This repository contains the **RESTful API** built with **ASP.NET Core**, following **Clean Architecture** principles.

## 🚀 Key Features

- **Authentication & Identity**:
  - Multi-provider login (Google, Facebook, Phone) via **Firebase Auth**.
  - Strict separation between User Identity and User Profile.
  - JWT-based authorization for internal API protection.
- **Booking Engine**:
  - **Seat Selection Logic**: Handle concurrency to prevent double-booking on specific seats (e.g., Seat 1A).
  - **Hub-to-Hub Routing**: Optimize rides between predefined public locations.
- **Real-time Communication (SignalR)**:
  - Live driver location tracking.
  - In-app chat between driver and passenger.
  - Real-time trip status updates.
  <!-- - **Background Jobs (Hangfire)**:
  - Auto-cancel booking requests if no driver accepts within 5 minutes.
  - Scheduled rides processing.
  - Fire-and-forget notification dispatching. -->
- **Storage & Caching**:
  - **MinIO**: High-performance object storage for vehicle images and avatars.
  - **Redis**: Distributed caching and SignalR backplane.

## 🛠 Tech Stack

| Component            | Technology              | Description                              |
| :------------------- | :---------------------- | :--------------------------------------- |
| **Framework**        | .NET 8 Web API          | Core backend framework.                  |
| **Database**         | SQL Server 2022         | Relational DB with Spatial Data support. |
| **Caching**          | Redis                   | Caching & Pub/Sub for SignalR.           |
| **Real-time**        | SignalR Core            | WebSocket management.                    |
| **Storage**          | MinIO (S3 Compatible)   | Blob storage.                            |
| **Auth**             | Firebase Admin SDK      | Token verification.                      |
| **Containerization** | Docker & Docker Compose | Orchestration for Dev/Prod.              |
| **CI/CD**            | GitHub Actions          | Automated deployment.                    |

## 🔧 Prerequisites

- [.NET SDK 9.0.309](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) (not required for [Full Docker mode](#-option-1-for-frontendmobile-developers-full-docker-mode))
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (Optional)

## 📦 Getting Started

Choose the setup method that matches your role.

### 🎯 Option 1: For Frontend/Mobile Developers (Full Docker Mode)

_Use this if you just need the API up and running to develop the Flutter App. No .NET SDK required._

This repo uses GitHub Actions to automatically build and push the latest backend image to Docker Hub whenever changes are merged.

1. **Download Docker Compose File**:

```bash
curl -o docker-compose.yml https://raw.githubusercontent.com/WhiteAngel2406/ShareXe/main/docker-compose.yml
```

2. **Pull the Latest Image**:

```bash
docker-compose pull
```

This command is used whenever there is a new update to the backend image.

3. **Start the Services**:

```bash
docker-compose up -d
```

The API will be accessible at `http://localhost:5012`.

Head over to the `http://localhost:5012/swagger` to explore the endpoints.

### 💻 Option 2: For Backend Developers (Development Mode)

_Use this if you need to write code, debug, or run migrations._

1. Pull the Repository:

```bash
git clone https://github.com/WhiteAngel2406/ShareXe.git
cd ShareXe
```

1. Prepare the Environment Variables: Create a `.env` file at the root of the project, then refer to `.env.example` for the required variables. For the Firebase-related variables (the variables prefixed with `FIREBASE_`), head over to [this section](#-firebase-setup) for guidance.

2. Start the Infrastructure Services:

```bash
docker-compose up -d
```

3. Restore .NET Dependencies & Tools:

```bash
dotnet restore
dotnet tool restore
```

4. Apply Database Migrations:

```bash
dotnet ef database update
```

5. Run the Backend:

```bash
dotnet run
```

or using the watch command for hot-reloading:

```bash
dotnet watch run
```

The API will be accessible at `http://localhost:5012`.

Head over to the `http://localhost:5012/swagger` to explore the endpoints.

## 🔐 Firebase Setup

This repo uses **Firebase Authentication** to handle user identities (Google, Facebook, Email/Password). You need to configure a Firebase project and provide credentials.

**1. Create a Firebase Project & Enable Auth**:

1. Go to the [Firebase Console](https://console.firebase.google.com/).
2. Create a new project (e.g., "sharexe-dev").
3. Navigate to **Build > Authentication**.
4. Click on **Get Started**.
5. In the **Sign-in** method tab, enable the following providers:

- **Email/Password**
- **Google** (You don't need to configure Client ID/Secret for backend testing, just enable it)

**2. Generate Service Account Key**:

1. Go to **Project Settings** (Gear icon ⚙️).
2. In the **General** tab, scroll down to **Your apps**.
3. Click the **Web (</>)** icon to register a dummy web app (name it "ShareXe Backend" or whatever).
4. Copy the `firebaseConfig` object values. You will need:
   - `apiKey`
   - `projectId`

**3. Configure Environment Variables**:

Update your `.env` file with the credentials obtained above:

```ini
# .env file

FIREBASE_PROJECT_ID=your-project-id
FIREBASE_API_KEY=your-web-api-key
```
