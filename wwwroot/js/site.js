document.addEventListener('DOMContentLoaded', function () {
    // Your code here

    console.log('DOM fully loaded and parsed');

});

document.querySelectorAll('[data-filter]').forEach(item => {
    item.addEventListener('click', e => {
        e.preventDefault();
        const filter = item.dataset.filter;
        console.log(`Filter selected: ${filter}`);
        // Add logic to show/hide events based on filter
    });
});
