document.addEventListener('DOMContentLoaded', function () {
    const habitsList = document.getElementById('habitsList');

    // Add click handlers to habit titles
    habitsList.addEventListener('click', async function (e) {
        const titleElement = e.target.closest('.habit-title');
        if (!titleElement) return;

        const habitItem = titleElement.closest('.habit-item');
        const habitId = habitItem.dataset.habitId;

        // Toggle existing calendar if present
        let calendar = habitItem.querySelector('.habit-calendar');
        if (calendar) {
            calendar.style.display = calendar.style.display === 'none' ? 'block' : 'none';
            return;
        }

        // Fetch habit completions
        try {
            const response = await fetch(`/HabitTracker/GetHabitCompletions/${habitId}`);
            if (!response.ok) throw new Error('Failed to fetch completions');
            const data = await response.json();

            // Create and render calendar
            calendar = createCalendar(data.completions, habitItem.dataset.createdDate);
            habitItem.appendChild(calendar);
        } catch (error) {
            console.error('Error:', error);
        }
    });
});

function createCalendar(completions, createdDate) {
    const calendar = document.createElement('div');
    calendar.className = 'habit-calendar';

    const date = new Date();
    const month = date.getMonth();
    const year = date.getFullYear();

    // Create calendar header
    const header = document.createElement('div');
    header.className = 'calendar-header';
    ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'].forEach(day => {
        const dayElement = document.createElement('div');
        dayElement.textContent = day;
        header.appendChild(dayElement);
    });
    calendar.appendChild(header);

    // Create calendar grid
    const grid = document.createElement('div');
    grid.className = 'calendar-grid';

    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);

    // Add empty cells for days before the first of the month
    for (let i = 0; i < firstDay.getDay(); i++) {
        grid.appendChild(document.createElement('div'));
    }

    // Add days of the month
    for (let day = 1; day <= lastDay.getDate(); day++) {
        const currentDate = new Date(year, month, day);
        const dayElement = document.createElement('div');
        dayElement.className = 'calendar-day';
        dayElement.textContent = day;

        // Check if habit was completed on this date
        const dateString = currentDate.toISOString().split('T')[0];
        if (completions.some(c => c.split('T')[0] === dateString)) {
            dayElement.classList.add('completed-day');
        } else if (currentDate <= new Date()) {
            dayElement.classList.add('missed-day');
        }

        grid.appendChild(dayElement);
    }

    calendar.appendChild(grid);
    return calendar;
}