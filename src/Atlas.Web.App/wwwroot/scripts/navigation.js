window.addEventListener('resize', () => {
    const elements = document.querySelectorAll('.expanded');

    if (elements.length === 0)
        return;

    elements.forEach((element) => {
        element.classList.remove('expanded');
    });
}, true);

function toggleNavigation() {
    const header = document.querySelector('header');
    const hamburger = document.querySelector('.hamburger');

    toggleExpand(header);
    toggleExpand(hamburger);

    function toggleExpand(element) {
        const expanded = element.getAttribute('aria-expanded') === 'true';

        element.classList.toggle('expanded');
        element.setAttribute('aria-expanded', !expanded);
    }
}
