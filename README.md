## System Architecture Diagram - Patient Management Application

**System Diagram:**

Following Diagram represent:

![Patient Management System Architecture][(https://github.com/vilesh-m/patient-management-system/blob/task/reactapp/Logical%20Diagram.png)]
---

**Components:**

*   **User Controller:** Manages user authentication and authorization logic.
*   **Web API:** Provides the backend services (API endpoints).
*   **JWT Service:**  Generates and verifies JSON Web Tokens. (Mocks token)
*   **LiteDB:** In-memory NoSQL database for storing patient data.
*   **DB Service:** Interacts with LiteDB to perform CRUD operations. Its named as PatientRepository in code.

*   **React App:** The user interface built with React.js.
    -  **Authentication Card:** Handles login and logout functionality, retrieving tokens.
    - **Patient Management Card:**  The UI element for managing patient records.

---

**Flow & Interactions:**

1.  **Token Acquisition:** The React App receives a token from the **Authentication Card** (via login), then this token is passed in every subsequent call.

2.  **Authentication Request:**
    *   The React App sends an authentication request to the **User Controller**Login Controller in the **Web API**.

3.  **Authorization:** 
    * The **Patient Controller** has the Authorize action filters on each operation, and uses the configured validator to verify the token and performs Authorization acording to the roles in the token.

4.  **API Calls & Data Interaction:**
    *   The React App makes API calls to the **Web API** for various operations:
        *   `Get Patients`: Retrieves all patients 
        *    `Post Patient`: Creates a new patient record in LiteDB.
        *   `Delete Patient`: Deletes a patient record in LiteDB.
        *   `Upload Attachment`:  Handles file uploads
        *   `Download Attachment`: Handles file downloads 
        *   `Search`: Searches for patients within the database. Its a one text input search supporting multiple fields like name, medical history and attachment types.

5 **Data Storage:** The **DB Service** interacts with the **LiteDB** NoSQL store, managing data based on user roles.

---

**Video Demonstration:**

To better understand the system's flow and interactions, please watch the following video:

[Embedded Video - Patient Management System Demo](https://github.com/vilesh-m/patient-management-system/blob/task/reactapp/react-app-demo.mp4) 


**Key Features/Technologies:**

*   **JWT Authentication:**  Uses JSON Web Tokens for secure authentication and authorization. I have used a mocked token to showcase how I would secure a WebApi. Tokens can be configured to be issued by Azure B2C/AD or OKTA.
*   **LiteDB:** In-memory or local file based NoSQL database for efficient patient record storage. I had used this for the first time. Was fun!
*   **ASP.NET Core** - WebApi are based on AspNetCore MVC. 
*   **Swagger/OpenAPI Specification** 
*   **React App:** Modern JavaScript/Typescript framework I have use to showcase backend API.


