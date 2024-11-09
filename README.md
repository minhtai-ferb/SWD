
# SWD392 - Fall 2024 Project: Japanese Learning App with AI Integration

## Project Overview
This project is a part of the SWD392 course in Fall 2024. It focuses on creating a **Japanese Learning Application** that applies **AI technology** to provide a personalized vocabulary learning experience for users.

## Innovation Aspect
We aim to innovate in the area of language learning by using AI to personalize the process of acquiring Japanese vocabulary. The application adapts to each user's learning habits and pace, making it more effective and tailored to their individual needs.

## IT Related Aspect
The mobile application is built using the following technologies:
- **Back End:** C# with ASP.NET Core framework, utilizing **SQL ServerL** as the database. 
- **Mobile Development:** Java and the **Flutter** framework for cross-platform development.
- **Firebase Integration:**
  - Google login for user authentication.
  - FCM (Firebase Cloud Messaging) for push notifications.
  - Firebase Storage for media and content storage.
- **Front End:** **React** for web-based UI.
- **Server-Side Technologies:**
  - **Cloudflare** for content delivery and protection.
  - **Nginx** as a reverse proxy server.
  - **Docker** for containerized services.
  - **Spring Security + JWT** for authentication.
  - **Swagger** for API documentation.
  - **Redis** for caching.

## Problem This Application Solves
- **Motivation & Inefficiency:** Tackles the common issue of lack of motivation and inefficient learning methods in language learning, especially for vocabulary.
- **One-Size-Fits-All:** Addresses the issue of generic, non-adaptive learning experiences in current apps by creating a personalized system tailored to each learner's needs.
- **Convenience & Flexibility:** Offers a solution that is available anytime, anywhere, adapting to the user’s learning pace and style.
- **Real-Life Application:** Provides tools that help learners apply Japanese vocabulary effectively in real-world contexts.
- **Cost-Effective Learning:** Offers a free platform with basic features and affordable premium options.

## Key Features
- **AI-Powered Vocabulary Learning:** The app uses AI to analyze user data and provide tailored content recommendations.
- **Synonym-Antonym Practice:** Focused exercises to strengthen vocabulary learning.
- **Google Login:** Seamless login through Google accounts.
- **Push Notifications:** Stay updated with notifications through FCM.
- **Cross-Platform Availability:** Accessible on both iOS and Android using Flutter.

## Repository Structure


- Backend        # Contains all backend services (ASP.NET Core, SQL Server, Redis, etc.)
- Documents      # Contains project documentation, reports, diagrams, etc.
- Frontend       # Web app (React) for Admin interface
- MobileApp      # Mobile app for learners (Flutter)


### Explanation of the Repository Structure:

- **Backend:** This directory contains all the backend logic and services needed for the application. This includes the ASP.NET Core application that connects to the SQL Server database, handles user authentication, processes API requests, and manages caching through Redis.
  
- **Documents:** All project-related documentation like system architecture, design diagrams, and reports are stored in this folder. It serves as the central repository for any technical and functional documents.

- **Frontend:** This folder contains the source code for the web-based admin interface built using React. The admin panel is used to manage the content, monitor user progress, and perform other administrative tasks.

- **MobileApp:** The mobile application for learners is developed using Flutter. This folder contains the mobile app's code, which is cross-platform (iOS and Android). It provides the main interface for learners to interact with the AI-powered vocabulary learning features.
