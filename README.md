# Unity_Talk  
**Online Collaboration Platform**

Unity_Talk is a web-based collaboration platform built using the ASP.NET MVC 4.7 framework and C#. It provides a comprehensive environment for real-time communication, file sharing, task management, and collaboration. Users can interact through group-based chats, virtual meetings, and more.

## Features  

### 1. **User Registration & Profile Management**
- Sign up, log in, and manage user profiles.
- Set active status (active/away).
  
### 2. **Group Management**
- Join public/private groups or create new ones.
- Assign admin roles to manage membership and set group visibility.
- Share group codes for others to join.
- Group admins can delete groups or remove themselves.

### 3. **To-Do List**
- Create, edit, and manage tasks.
- Track task completion status.

### 4. **File Sharing**
- Upload and share files securely within groups.
- Admins and uploaders can manage file deletions.

### 5. **Whiteboard**
- Collaborative space for drawing diagrams or taking notes.

### 6. **Default eBooks**
- Administrators provide a collection of eBooks for reading and downloading.

### 7. **Feedback**
- Users can submit feedback to platform administrators for improvements.

### 8. **Virtual Meetings**
- Each group has a meeting link for video/audio meetings.
- Screen sharing, recording, and file sharing available during meetings.

### 9. **Real-time Chat**
- Integrated chat in each group.
- Built using **SignalR** and **WebSocket** for real-time communication.

## Technology Stack  

- **ASP.NET MVC 4.7 Framework**: Core structure of the platform.
- **C# Language**: Backend logic and functionality.
- **SQL Server Management Studio 2019**: For data persistence and management.
- **SignalR & WebSocket**: Enabling real-time chat functionality.
- **HTML, CSS, JavaScript**: Front-end development for a responsive user interface.
- **CSHTML**: Dynamic front-end elements.
- **API Integration**: For virtual meeting capabilities (video, audio, screen sharing, etc.).

## How to Run the Project  

1. Clone this repository:
    ```bash
    git clone https://github.com/Anirban-Manna-10/UnityTalk.git
    ```
2. Open the project in **Visual Studio**.
3. Restore the required NuGet packages:
    ```bash
    Update-Package -reinstall
    ```
4. Set up your **SQL Server** database (use SQL Server Management Studio 2019).
5. Run the application through Visual Studio.
