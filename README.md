Last minute project to pass a class.

Basic ASP.NET MVC app. App requires the user to either register or login. Once logged in you can add a habit and mark it as done for the day. Once day rolls over to the next day it'll automatically reset all of the intended habits. If you add a specific time for the habit it'll order itself on the top of the habit list according to its time.

Documentation:

# HabitsTracker

## Application to track your daily habits

### Content:
- [Intro](#intro)
- [Application setup](#application-setup)
- [Used technologies](#used-technologies)
- [Usage instructions](#usage-instructions)
- [Challenges during development](#challenges-during-development)
- [Final thoughts](#final-thoughts)

---

## Intro

**Habits Tracker** is an application made using **ASP.NET Core MVC** architecture. It is designed with simplicity in mind for everyday habit tracking, focusing on ease of use with the mindset of *"less is more"*.

---

## Application setup

To set up the application, follow these steps:

1. **Download the source code** from the repository.
2. Ensure you have the following installed on your system:
   - **Visual Studio 2022 Community Edition**
   - **.NET 8.0 SDK**
   - **Microsoft SQLExpress server (LocalDB)**
3. Click the green `Code` button on GitHub, select `Download .zip`, extract the files, and open `HabitsTracker.sln` in Visual Studio 2022.
4. Run the following commands:
   ```sh
   Add-Migration FirstMigration
   Update-Database
   ```
5. The application is now ready to run.

Alternatively, you can simply download and run the `.exe` version for an easier setup.

---

## Used technologies

### Backend:
- **ASP.NET Core 8.0**
- **EntityFrameworkCore**
- **SQL Server**

### Frontend:
- **HTML/CSS**
- **Razor Views**

---

## Usage instructions

1. Run the application.
2. You will be greeted with a welcome screen. Click `Register`.
3. After registering, log in using your credentials.
4. Once logged in, enter the name of the habit you want to track daily.
5. Optionally, set a time for the habit. Habits with a time assigned will appear at the top in descending order.
6. Click the radial button next to a habit to mark it as **"done"** for the day.
7. At midnight, all habits reset. 
8. Click a habit name to view its history on the calendar:
   - **Green:** Completed that day.
   - **Red:** Not completed that day.
   - If the habit did not exist on a specific date, the calendar will remain blank.

---

## Challenges during development

- **Modifying pages** was straightforward, making it easy to build a visually distinctive project.
- **Controllers** streamlined development and troubleshooting.
- **Strikethrough function** was difficult to implement using JavaScript. A traditional HTML form submission and page refresh approach was used instead.
- **Calendar function** initially worked with JavaScript but was later reworked to be more backend-focused.
- **Displaying month/year on the calendar** took significant debugging efforts.

---

## Final thoughts

Working in **.NET** requires following strict development rules, making it more complex than other tech stacks with more streamlined tutorials. However, its power is evident, and learning it has been rewarding.

I had fun developing this project, even under time constraints, and I look forward to expanding it further. Future improvements may include:
- A **statistics page**.
- **Habit categories**.
- **User comments on habits**.
- A **mobile-friendly version**.
